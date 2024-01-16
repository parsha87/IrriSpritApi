using AutoMapper;
using GISApi.Data.GlobalEntities;
using GISApi.Models;
using GISApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GISApi.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValveSettingController : ControllerBase
    {

        private readonly ILogger<ValveSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly IValveSettingService _service;

        public ValveSettingController(ILogger<ValveSettingController> logger, IMapper mapper, IValveSettingService service)
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
        public async Task<ActionResult<List<ValveSetting>>> GetValveSetting()
        {
            try
            {
                List<ValveSetting> list = await _service.GetValveSetting();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ValveSettingController) + "." + nameof(GetValveSetting) + "]" + ex);
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
        public async Task<ActionResult<ValveSetting>> Get(int id)
        {
            try
            {
                ValveSetting model = await _service.GetValveSettingId(id);
              

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ValveSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("ValveSettingByControllerId/{id}")]
        public async Task<ActionResult<ValveSetting>> GetValveSettingByControllerId(int id)
        {
            try
            {
                ValveSetting model = await _service.GetDataByControllerId(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ValveSettingController) + "." + nameof(GetValveSettingByControllerId) + "]" + ex);
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
        public async Task<IActionResult> Post(ValveSetting model)
        {
            try
            {
                var result = await _service.AddValveSetting(model);
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
                var role = await _service.DeleteValveSetting(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ValveSettingController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] ValveSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditValveSetting(model);

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
