using AutoMapper;
using GISApi.Auth;
using GISApi.Helpers;
using GISApi.Models;
using GISApi.Services;
using GISApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using GISApi.Data.GlobalEntities;

namespace GISApi.Controllers
{

    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private IUserService _userService;
        private GlobalDBContext _globalDBContext;
        public UsersController(GlobalDBContext globalDBContext, IUserService userService, UserManager<ApplicationUser> userManager, ILogger<UsersController> logger, RoleManager<IdentityRole> roleManager,
            IConfiguration config, ApplicationDbContext appDbContext, IWebHostEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService)
        {
            _userService = userService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _webHostEnvironment = hostingEnvironment;
            _globalDBContext = globalDBContext;
            _commonService = commonService;

        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<List<AddEditUserViewModel>>> Get(bool? status)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }


                //var roles = ((ClaimsIdentity)User.Identity).Claims
                //.Where(c => c.Type == ClaimTypes.Role)
                //.Select(c => c.Value);
                string roleName = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
                var organizationId = User.Claims.Single(c => c.Type == CustomClaimTypes.OrganisationId).Value;
                int OrganisationId = Convert.ToInt32(organizationId);

                List<AddEditUserViewModel> users = await _userService.GetUsers(status, OrganisationId, roleName);
                return Ok(users.OrderBy(x => x.FirstName));
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("{userId}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<AddEditUserViewModel>> Get(string userId)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                string userIdwe = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userIdwe))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }



                AddEditUserViewModel user = await _userService.GetUserById(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="model">AddEditUserViewModel</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<OperationResult<AddEditUserViewModel>>> Put(string id, [FromBody] AddEditUserViewModel model)
        {
            var token = Request.Headers["Authorization"];
            string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
            if (!_commonService.ValidateToken(token, userId))
            {
                ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
            }


            var errors = new List<string>();
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!IsModelValid(model))
            {
                return BadRequest(new CustomBadRequest(ModelState));
            }

            if (model.RoleId == "")
            {
                ModelState.AddModelError("Error", "Assign atleast one role.");
                return BadRequest(new CustomBadRequest(ModelState));
            }
            try
            {
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _userManager.FindByIdAsync(model.Id);

                        //show proper error message to admin
                        if (user == null)
                        {
                            transaction.Rollback();
                            return NotFound();
                        }

                        var role = _roleManager.Roles.Where(x => x.Id == model.RoleId).FirstOrDefault();
                        if (role == null)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", $"Role ({model.RoleName}) not found!");
                            return BadRequest(new CustomBadRequest(ModelState));
                        }

                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.IsActive = model.IsActive;
                        user.Address = model.Address;
                        user.PhoneNumber = model.PhoneNumber;
                        user.RoleName = model.RoleName;
                        user.RoleId = model.RoleId;
                        user.Email = model.Email;
                        user.UserName = model.UserName;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", result.Errors.FirstOrDefault().Description);
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        //Send Account activation mail
                        //var userEmail = await _userManager.FindByEmailAsync(model.UserName);
                        ////Send mail if active

                        //if (model.IsActive && flagEmailChange == true)
                        //{
                        //    string code = await _userManager.GeneratePasswordResetTokenAsync(userEmail);
                        //    code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));

                        //    var callBackUrl = GenerateResetPwdCallbackUrl(code, "accountactivation");

                        //    var success = SendActivationMail(callBackUrl, user.Email, user.FirstName + " " + user.LastName);
                        //    if (!success)
                        //    {
                        //        errors.Add("Could not send account activation mail.");
                        //    }
                        //}
                        var userRoles = await _userManager.GetRolesAsync(user);
                        if (userRoles.Contains("SuperAdmin"))
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", "Cannot ");
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        ////find user's existing role
                        var userExistingRoles = await _userManager.GetRolesAsync(user);
                        //remove user from eixsting role and assign new role if newly assigned role is different.
                        //var newRoles = model.Roles.Select(x => x.Name).ToList().Except(userExistingRoles);
                        //if (newRoles.Count() > 0)
                        //{
                        //remove from existing role
                        foreach (var rolew in userExistingRoles)
                        {
                            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, rolew);
                            if (!removeRoleResult.Succeeded)
                            {
                                //rollback
                                transaction.Rollback();
                                ModelState.AddModelError("Error", removeRoleResult.Errors.FirstOrDefault().Description);
                                return BadRequest(new CustomBadRequest(ModelState));
                            }
                            await _appDbContext.SaveChangesAsync();
                        }
                        //assign role

                        var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!roleResult.Succeeded)
                        {
                            //rollback
                            transaction.Rollback();
                            ModelState.AddModelError("Error", roleResult.Errors.FirstOrDefault().Description);
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        await _appDbContext.SaveChangesAsync();


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError("[" + nameof(UsersController) + "." + nameof(Put) + "]" + ex);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = $@"[{nameof(UsersController)}.{nameof(Put)}] 
                    Exception = {ex}
                    loggedin user = {User.Identity.Name}
                    Http Request Details:
                    model = {HttpContext.Request.Form["model"]}";
                _logger.LogError(error);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new OperationResult<AddEditUserViewModel> { Succeeded = true, Data = model, Errors = errors });
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<OperationResult<AddEditUserViewModel>>> Post(AddEditUserViewModel model)
        {
            // string roleName = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            if (!IsModelValid(model))
            {
                return BadRequest(new CustomBadRequest(ModelState));
            }
            if (model.RoleId == "")
            {
                ModelState.AddModelError("Error", "Assign atleast one role.");
                return BadRequest(new CustomBadRequest(ModelState));
            }

            if (model.RoleName.ToLower() == "superadmin")
            {
                ModelState.AddModelError("Error", "Error.Dont try to hack application..:)");
                return BadRequest(new CustomBadRequest(ModelState));
            }

            var errors = new List<string>();
            try
            {
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        string RoleId = _roleManager.Roles.Where(x => x.Name == model.RoleName).Select(x => x.Id).FirstOrDefault();
                        model.RoleId = RoleId;
                        var role = _roleManager.Roles.Where(x => x.Id == model.RoleId).FirstOrDefault();
                        if (role == null)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", $"Role ({model.RoleName}) not found!");
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        List<ControllerMaster> allControllers = null;
                        if (model.ControllerIdsList.Count > 0)
                        {
                            //Get List of all controllers
                            allControllers = _globalDBContext.ControllerMasters.ToList();
                        }
                        ApplicationUser user = new ApplicationUser();
                        try
                        {
                            user = new ApplicationUser
                            {
                                UserName = model.Email,
                                Email = model.Email,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Address = model.Address,
                                PhoneNumber = model.PhoneNumber,
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                IsActive = model.IsActive,
                                RoleId = model.RoleId,
                                RoleName = model.RoleName,
                                CountryId = model.CountryId,
                                CountryName = model.CountryName,
                                IsParent = true,
                                LanguageId = model.LanguageId,
                                LanguageName = model.LanguageName,
                                ParentId = model.ParentId,
                                TimeZone = model.TimeZone,
                            };
                            var result = await _userManager.CreateAsync(user, model.Password);
                            if (!result.Succeeded)
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("ERROR", error.Description);
                                }
                                return BadRequest(new CustomBadRequest(ModelState));
                                //transaction.Rollback();
                                //ModelState.AddModelError("Error", result.Errors.FirstOrDefault().Description);
                                //return BadRequest(new CustomBadRequest(ModelState));
                            }
                            model.Id = user.Id;
                            //assign role

                            var addRoleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                            if (!addRoleResult.Succeeded)
                            {
                                //rollback
                                transaction.Rollback();
                                ModelState.AddModelError("Error", $"User not created.\nError occurred while assigning role:\n {addRoleResult.Errors.FirstOrDefault().Description}");
                                return BadRequest(new CustomBadRequest(ModelState));
                            }
                            await _appDbContext.SaveChangesAsync();

                            if (model.ControllerIdsList.Count > 0)
                            {
                                var usrMappings = _globalDBContext.UserControllerMappings.Where(x => x.UserId == user.Id).ToList();
                                _globalDBContext.UserControllerMappings.RemoveRange(usrMappings);
                                await _globalDBContext.SaveChangesAsync();


                                List<UserControllerMapping> mapplingList = new List<UserControllerMapping>();
                                foreach (var item in model.ControllerIdsList)
                                {
                                    UserControllerMapping userControllerMapping = new UserControllerMapping();
                                    userControllerMapping.ControllerId = item;
                                    userControllerMapping.IsForParent = true;
                                    userControllerMapping.UserId = user.Id;
                                    userControllerMapping.IsActive = true;
                                    userControllerMapping.ControllerNo = allControllers.Where(x => x.Id == item).Select(x => x.ControllerNo).FirstOrDefault();
                                    mapplingList.Add(userControllerMapping);
                                }

                                await _globalDBContext.UserControllerMappings.AddRangeAsync(mapplingList);
                                await _globalDBContext.SaveChangesAsync();

                                var usrMappingsFinalIds = _globalDBContext.UserControllerMappings.Where(x => x.UserId == user.Id).Select(x => x.Id).ToList();
                                var controlerMasters = _globalDBContext.ControllerMasters.Where(x => usrMappingsFinalIds.Contains(x.Id)).ToList();
                                foreach (var item in controlerMasters)
                                {
                                    item.IsAssigned = true;
                                }
                                _globalDBContext.ControllerMasters.UpdateRange(controlerMasters);
                                await _globalDBContext.SaveChangesAsync();

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Error occured in [AccountController] - RegisterUser() - CreateAsync() - User:" + user.Email + ". Exception is : " + ex);
                        }


                        //Add subuser if any
                        if (model.SubUserEmail != "" && model.SubUserPhoneNo != "")
                        {
                            model.Id = "";
                            if (!_globalDBContext.AspNetUsers.Any(x => x.UserName == model.SubUserEmail))
                            {
                                ApplicationUser subuser = new ApplicationUser();
                                try
                                {
                                    subuser = new ApplicationUser
                                    {
                                        UserName = model.SubUserEmail,
                                        Email = model.SubUserEmail,
                                        FirstName = model.SubUserFirstName,
                                        LastName = model.SubUserLastName,
                                        Address = model.Address,
                                        PhoneNumber = model.SubUserPhoneNo,
                                        EmailConfirmed = true,
                                        PhoneNumberConfirmed = true,
                                        IsActive = model.IsActive,
                                        RoleId = model.RoleId,
                                        RoleName = model.RoleName,
                                        CountryId = model.CountryId,
                                        CountryName = model.CountryName,
                                        IsParent = false,
                                        LanguageId = model.LanguageId,
                                        LanguageName = model.LanguageName,
                                        ParentId = user.Id,
                                        TimeZone = model.TimeZone,
                                    };
                                    var result = await _userManager.CreateAsync(subuser, model.Password);
                                    if (!result.Succeeded)
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            ModelState.AddModelError("ERROR", error.Description);
                                        }
                                        return BadRequest(new CustomBadRequest(ModelState));
                                    }
                                    model.Id = subuser.Id;

                                    //assign role
                                    var addRoleResultSubUser = await _userManager.AddToRoleAsync(subuser, model.RoleName);
                                    if (!addRoleResultSubUser.Succeeded)
                                    {
                                        //rollback
                                        transaction.Rollback();
                                        ModelState.AddModelError("Error", $"User not created.\nError occurred while assigning role:\n {addRoleResultSubUser.Errors.FirstOrDefault().Description}");
                                        return BadRequest(new CustomBadRequest(ModelState));
                                    }
                                    await _appDbContext.SaveChangesAsync();

                                    //Add Controllers mapping
                                    if (model.userControllerMappings.Count > 0)
                                    {
                                        var usrMappings = _globalDBContext.UserControllerMappings.Where(x => x.UserId == subuser.Id).ToList();
                                        _globalDBContext.UserControllerMappings.RemoveRange(usrMappings);
                                        await _globalDBContext.SaveChangesAsync();


                                        List<UserControllerMapping> mapplingList = new List<UserControllerMapping>();
                                        foreach (var item in model.ControllerIdsList)
                                        {
                                            UserControllerMapping userControllerMapping = new UserControllerMapping();
                                            userControllerMapping.ControllerId = item;
                                            userControllerMapping.IsForParent = false;
                                            userControllerMapping.UserId = subuser.Id;
                                            userControllerMapping.IsActive = true;
                                            userControllerMapping.ControllerNo = allControllers.Where(x => x.Id == item).Select(x => x.ControllerNo).FirstOrDefault();
                                            mapplingList.Add(userControllerMapping);
                                        }

                                        await _globalDBContext.UserControllerMappings.AddRangeAsync(mapplingList);
                                        await _globalDBContext.SaveChangesAsync();

                                        var usrMappingsFinalIds = _globalDBContext.UserControllerMappings.Where(x => x.UserId == subuser.Id).Select(x => x.Id).ToList();
                                        var controlerMasters = _globalDBContext.ControllerMasters.Where(x => usrMappingsFinalIds.Contains(x.Id)).ToList();
                                        foreach (var item in controlerMasters)
                                        {
                                            item.IsAssigned = true;
                                        }
                                        _globalDBContext.ControllerMasters.UpdateRange(controlerMasters);
                                        await _globalDBContext.SaveChangesAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("Error occured in [AccountController] - RegisterUser() - CreateAsync() - User:" + user.Email + ". Exception is : " + ex);
                                }
                            }
                            {
                                //Return error subuser already exists
                                ModelState.AddModelError("Error", $"Sub User not created. Same Email already exists\nError occurred while adding User:");
                                return BadRequest(new CustomBadRequest(ModelState));
                            }

                        }


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError("[" + nameof(UsersController) + "." + nameof(Post) + "]" + ex);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(Post) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = model.Id },
                            new OperationResult<AddEditUserViewModel> { Succeeded = true, Data = model, Errors = errors });
        }

        /// <summary>
        /// Cancel inspection request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{userId}/actions/delete")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<OperationResult>> Delete(string userId)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                // string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }

                //bool isInUse = await _userService.IsUserAlreadyInUse(userId);
                //if (isInUse)
                //{
                //    ModelState.AddModelError("ERROR", "Cannot delete this user as it is already used in estimate.");
                //    return BadRequest(new CustomBadRequest(ModelState));
                //}               
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        //show proper error message to admin
                        if (user == null)
                        {
                            transaction.Rollback();
                            return NotFound();
                        }
                        //find user's existing role
                        var userRoles = await _userManager.GetRolesAsync(user);
                        if (userRoles.Contains("BackendAdmin"))
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", "Cannot ");
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        //delete user roles
                        foreach (var role in userRoles)
                        {
                            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, role);
                            if (!removeRoleResult.Succeeded)
                            {
                                //rollback
                                transaction.Rollback();
                                ModelState.AddModelError("Error", removeRoleResult.Errors.FirstOrDefault().Description);
                                return BadRequest(new CustomBadRequest(ModelState));
                            }
                            await _appDbContext.SaveChangesAsync();
                        }
                        var removeUserResult = await _userManager.DeleteAsync(user);
                        if (!removeUserResult.Succeeded)
                        {
                            //rollback
                            transaction.Rollback();
                            ModelState.AddModelError("Error", removeUserResult.Errors.FirstOrDefault().Description);
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        string error = $@"[{nameof(UsersController)}.{nameof(Delete)}] 
                                    Exception = {ex}
                                    loggedin user = {User.Identity.Name}
                                    Http Request Details:
                                    id = {userId}";
                        _logger.LogError(error);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                return Ok(new OperationResult { Succeeded = true });
            }
            catch (Exception ex)
            {
                string error = $@"[{nameof(UsersController)}.{nameof(Delete)}] 
                    Exception = {ex}
                    loggedin user = {User.Identity.Name}
                    Http Request Details:
                    id = {userId}";
                _logger.LogError(error);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Send activation email to user
        /// </summary>
        /// <param name="model">AccountActivationModel</param>
        /// <returns></returns>
        [HttpPost("sendaccountactivationmail")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<IActionResult> SendAccountActivationMail(AccountActivationModel model)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }


                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return BadRequest();
                }
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));

                var callBackUrl = GenerateResetPwdCallbackUrl(code, "accountactivation");

                var success = SendActivationMail(callBackUrl, user.Email, user.FirstName + " " + user.LastName);
                if (success)
                    return Ok();
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(SendAccountActivationMail) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Get login user details
        /// </summary>     
        /// <returns>User model</returns>
        [HttpGet("profile")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<UserProfileViewModel>> GetProfile()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }


                // string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                UserProfileViewModel userViewModel = await _userService.GetUserProfileById(userId);
                if (userViewModel == null)
                    return NotFound();

                return Ok(userViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(GetProfile) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Update user's profile details
        /// </summary>     
        /// <returns>User model</returns>    
        [HttpPut("profile")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<IActionResult> PutProfile([FromBody] UserProfileViewModel model)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }


                ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    // user.Position = model.Position;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok(model);
                    else
                        return GetErrorResult(result);
                }
                else
                {
                    ModelState.AddModelError("Error", "User details not found.");
                    return BadRequest(new CustomBadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(PutProfile) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        //[HttpGet]
        //[Route("GetById/{userId}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetById(string userId)
        //{
        //    try
        //    {
        //        return Ok(await _userService.GetById(userId));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error occured in [UserController] - GetById Exception is : " + ex);
        //        throw ex;
        //    }
        //}
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Put(AddEditUserViewModel model)
        {
            AddEditUserViewModel addModel = null;
            try
            {
                var token = Request.Headers["Authorization"];
                string userId = User.Claims.Single(c => c.Type == ClaimTypes.PrimarySid).Value;
                if (!_commonService.ValidateToken(token, userId))
                {
                    ModelState.AddModelError("ERROR", "Someone else is logged in with this UserID.");
                    return StatusCode(StatusCodes.Status406NotAcceptable, new CustomBadRequest(ModelState));
                }


                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _userManager.FindByIdAsync(model.Id);
                        if (user == null)
                        {
                            transaction.Rollback();
                            return NotFound();
                        }

                        var role = _roleManager.Roles.Where(x => x.Id == model.RoleId).FirstOrDefault();
                        if (role == null)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", $"Role ({model.RoleName}) not found!");
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.IsActive = model.IsActive;
                        user.Address = model.Address;
                        //  user.PhoneNumber = model.MobileNo;
                        // user.Position = model.Password;
                        bool flagEmailChange = false;
                        if (user.Email != model.UserName)
                        {
                            flagEmailChange = true;
                        }
                        user.Email = model.UserName;
                        user.UserName = model.UserName;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("Error", result.Errors.FirstOrDefault().Description);
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        //Send Account activation mail
                        //var userEmail = await _userManager.FindByEmailAsync(model.UserName);
                        ////Send mail if active

                        //if (model.IsActive && flagEmailChange == true)
                        //{
                        //    string code = await _userManager.GeneratePasswordResetTokenAsync(userEmail);
                        //    code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));

                        //    var callBackUrl = GenerateResetPwdCallbackUrl(code, "accountactivation");

                        //    var success = SendActivationMail(callBackUrl, user.Email, user.FirstName + " " + user.LastName);
                        //    if (!success)
                        //    {
                        //        errors.Add("Could not send account activation mail.");
                        //    }
                        //}
                        ////find user's existing role
                        var userExistingRoles = await _userManager.GetRolesAsync(user);
                        //remove user from eixsting role and assign new role if newly assigned role is different.
                        //var newRoles = model.Roles.Select(x => x.Name).ToList().Except(userExistingRoles);
                        //if (newRoles.Count() > 0)
                        //{
                        //remove from existing role
                        foreach (var rolew in userExistingRoles)
                        {
                            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, rolew);
                            if (!removeRoleResult.Succeeded)
                            {
                                //rollback
                                transaction.Rollback();
                                ModelState.AddModelError("Error", removeRoleResult.Errors.FirstOrDefault().Description);
                                return BadRequest(new CustomBadRequest(ModelState));
                            }
                            await _appDbContext.SaveChangesAsync();
                        }
                        //assign role

                        var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!roleResult.Succeeded)
                        {
                            //rollback
                            transaction.Rollback();
                            ModelState.AddModelError("Error", roleResult.Errors.FirstOrDefault().Description);
                            return BadRequest(new CustomBadRequest(ModelState));
                        }
                        await _appDbContext.SaveChangesAsync();


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError("[" + nameof(UsersController) + "." + nameof(Put) + "]" + ex);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                //addModel = await _userService.EditUser(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(Post) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Put), new { id = 0 }, addModel);

        }




        #region private methods
        private bool SendActivationMail(string callbackUrl, string mailTo, string userName)
        {
            string bodyString = "";
            string path = $"{_webHostEnvironment.ContentRootPath}/EmailTemplate/ConfirmAccountEmail.html";

            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    bodyString = sr.ReadToEnd();
                }
                bodyString = bodyString.Replace("##hrefCode##", callbackUrl);
                bodyString = bodyString.Replace("##user_name##", userName);
                _logger.LogInformation(mailTo + " " + callbackUrl);
            }
            //var result = _emailService.SendEmail(mailTo, "JPS ERP: Account Activation", bodyString);
            return true;
        }

        private string GenerateResetPwdCallbackUrl(string code, string actionType)
        {
            string uciGuiLink = _config["ERPGUILinkReset"];

            return Microsoft.AspNetCore.Http.Extensions.UriHelper.Encode(
                new System.Uri($"{uciGuiLink}{actionType}#{HttpUtility.UrlEncode(code)}"));
        }
        private bool IsModelValid(AddEditUserViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Model cannot be null");
            }

            if (!ModelState.IsValid)
            {
                return false;
            }

            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (allErrors.ToList().Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get error from IdentityResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(new CustomBadRequest(ModelState));
                }
            }
            return BadRequest();
        }



        #endregion
    }
}
