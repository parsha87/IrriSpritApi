using AutoMapper;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using GISApi.Models;
using GISApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ControllerTimeSettingController : ControllerBase
    {
        private readonly ILogger<ControllerTimeSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly IControllerTimeSettingService _service;
        public ControllerTimeSettingController(ILogger<ControllerTimeSettingController> logger, IMapper mapper, IControllerTimeSettingService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }
        /// <summary>
        /// Get User role list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ControllerTimeSetting>>> GetControllerTimeList()
        {
            try
            {
                List<ControllerTimeSetting> list = await _service.GetTimeSettingList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerTimeSettingController) + "." + nameof(GetControllerTimeList) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<ControllerTimeSetting>> Get(int id)
        {
            try
            {
                ControllerTimeSetting model = await _service.GetTimeSettingId(id);
              

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerTimeSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("ControllerTimeSettingByControllerId/{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<ControllerTimeSetting>> GetControllerTimeSettingByControllerId(int id)
        {
            try
            {
                ControllerTimeSetting model = await _service.GetDataByControllerId(id);
               

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerTimeSettingController) + "." + nameof(GetControllerTimeSettingByControllerId) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<IActionResult> Post(ControllerTimeSetting model)
        {
            try
            {
                var result = await _service.AddTimeSetting(model);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }

        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">Role Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = await _service.DeleteControllerTimeSetting(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(RoleController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] ControllerTimeSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditTimeSetting(model);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(RoleController) + "." + nameof(Delete) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //if (!IsModelValid(model))
            //{
            //    return BadRequest(new CustomBadRequest(ModelState));
            //}

        }
    }
}
