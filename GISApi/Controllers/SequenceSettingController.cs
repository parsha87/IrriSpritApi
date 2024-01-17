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
    
    public class SequenceSettingController : ControllerBase
    {
        private readonly ILogger<SequenceSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly ISequenceSettingService _service;

        public SequenceSettingController(ILogger<SequenceSettingController> logger, IMapper mapper, ISequenceSettingService service)
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
        public async Task<ActionResult<List<SequenceSetting>>> GetSequenceSetting()
        {
            try
            {
                List<SequenceSetting> list = await _service.GetSequenceSetting();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(SequenceSettingController) + "." + nameof(GetSequenceSetting) + "]" + ex);
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
        public async Task<ActionResult<SequenceSetting>> Get(int id)
        {
            try
            {
                SequenceSetting model = await _service.GetSequenceSettingId(id);


                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(SequenceSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("SequenceSettingByControllerId/{id}")]
        public async Task<IActionResult> GetSequenceSettingByControllerId(int id)
        {
            try
            {
                List<SequenceSetting> list = await _service.GetDataByControllerId(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(SequenceSettingController) + "." + nameof(GetSequenceSettingByControllerId) + "]" + ex);
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
        public async Task<IActionResult> Post(SequenceSetting model)
        {
            try
            {
                var result = await _service.AddSequenceSetting(model);
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
                var role = await _service.DeleteSequenceSetting(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(SequenceSettingController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] SequenceSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditSequenceSetting(model);

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
