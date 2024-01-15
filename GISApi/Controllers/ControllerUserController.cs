using AutoMapper;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using GISApi.Helpers;
using GISApi.Models;
using GISApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GISApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerUserController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private GlobalDBContext _GlobalDBContext;
        private readonly IControllerMasterService _controllerMasterService;
        public ControllerUserController(GlobalDBContext GlobalDBContext,
 ILogger<UsersController> logger, IConfiguration config, IWebHostEnvironment hostingEnvironment, IMapper mapper, IControllerMasterService controllerMasterService)
        {
            _GlobalDBContext = GlobalDBContext;
            _logger = logger;
            _mapper = mapper;
            _config = config;
            _controllerMasterService = controllerMasterService;


        }

        /// <summary>
        /// Get User role list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllControllerList()
        {
            try
            {
                List<ControllerMaster> controllers = await _controllerMasterService.GetControllerMaster();
                return Ok(controllers);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(GetAllControllerList) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetControllerListByUserId(string id)
        {
            try
            {
                List<UserControllerMapping> modeList= await _controllerMasterService.GetControllersByUserId(id);
                if (modeList == null)
                    return NotFound();

                return Ok(modeList);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(UsersController) + "." + nameof(GetControllerListByUserId) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        

        /// <summary>
        /// Edit role
        /// </summary>
        /// <param name="model">AddEditUserViewModel</param>
        /// <returns>RoleViewModel</returns>
        [HttpPut("{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<OperationResult<RoleViewModel>>> Put(string id, [FromBody] RoleViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (model.Name == "BackendAdmin" || model.Name == "SuperAdmin")
            {
                ModelState.AddModelError("Error", "Role name is already in use, please contact administration.");
                return BadRequest(new CustomBadRequest(ModelState));
            }


            var rolesInDB = await _GlobalDBContext.AspNetRoles.Where(x => x.Name.ToUpper() == model.Name.ToUpper() && x.Id != id).ToListAsync();

            if (rolesInDB != null)
            {
                if (rolesInDB.Count == 0)
                {
                    var roleInfo = await _GlobalDBContext.AspNetRoles.Where(x => x.Id == id).FirstOrDefaultAsync();
                    roleInfo.Name = model.Name;
                    // roleInfo.NormalizedName = model.Name.ToUpper();                    
                    await _GlobalDBContext.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("Error", "Role with same name exsists");
                    return BadRequest(new CustomBadRequest(ModelState));

                }
            }
            else
            {
                ModelState.AddModelError("Error", "Role with same name exsists");
                return BadRequest(new CustomBadRequest(ModelState));

            }

            //if (!IsModelValid(model))
            //{
            //    return BadRequest(new CustomBadRequest(ModelState));
            //}

        }
    }
}
