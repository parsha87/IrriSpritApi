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
    public class MaxFiltervalveSettingController : ControllerBase
    {
        private readonly ILogger<MaxFiltervalveSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly IMaxFiltervalveSettingService _service;

        public MaxFiltervalveSettingController(ILogger<MaxFiltervalveSettingController> logger, IMapper mapper, IMaxFiltervalveSettingService service)
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
        public async Task<ActionResult<List<MaxFiltervalveSetting>>> GetMaxFiltervalveSetting()
        {
            try
            {
                List<MaxFiltervalveSetting> list = await _service.GetMaxFiltervalveSetting();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(MaxFiltervalveSettingController) + "." + nameof(GetMaxFiltervalveSetting) + "]" + ex);
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
        public async Task<ActionResult<MaxFiltervalveSetting>> Get(int id)
        {
            try
            {
                MaxFiltervalveSetting model = await _service.GetMaxFiltervalveSettingId(id);
                if (model == null)
                    return NotFound();

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(MaxFiltervalveSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("MaxFiltervalveByControllerId/{id}")]
        public async Task<ActionResult<MaxFiltervalveSetting>> GetMaxFiltervalveByControllerId(int id)
        {
            try
            {
                MaxFiltervalveSetting model = await _service.GetDataByControllerId(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(MaxFiltervalveSettingController) + "." + nameof(GetMaxFiltervalveByControllerId) + "]" + ex);
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
        public async Task<IActionResult> Post(MaxFiltervalveSetting model)
        {
            try
            {
                var result = await _service.AddMaxFiltervalveSetting(model);
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
                var role = await _service.DeleteMaxFiltervalveSetting(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(MaxFiltervalveSettingController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] MaxFiltervalveSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditMaxFiltervalveSetting(model);

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
