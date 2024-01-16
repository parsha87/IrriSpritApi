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
    public class FilterSequenceSettingController : ControllerBase
    {
        private readonly ILogger<FilterSequenceSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterSequenceSettingService _service;

        public FilterSequenceSettingController(ILogger<FilterSequenceSettingController> logger, IMapper mapper, IFilterSequenceSettingService service)
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
        public async Task<ActionResult<List<FilterSequenceSetting>>> GetFilterSequenceSetting()
        {
            try
            {
                List<FilterSequenceSetting> list = await _service.GetFilterSequenceSetting();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(FilterSequenceSettingController) + "." + nameof(GetFilterSequenceSetting) + "]" + ex);
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
        public async Task<ActionResult<FilterSequenceSetting>> Get(int id)
        {
            try
            {
                FilterSequenceSetting model = await _service.GetFilterSequenceSettingId(id);
             

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(FilterSequenceSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("FilterSequenceByControllerId/{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<FilterSequenceSetting>> GetFilterSequenceByControllerId(int id)
        {
            try
            {
                FilterSequenceSetting model = await _service.GetDataByControllerId(id);
               
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(FilterSequenceSettingController) + "." + nameof(GetFilterSequenceByControllerId) + "]" + ex);
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
        public async Task<IActionResult> Post(FilterSequenceSetting model)
        {
            try
            {
                var result = await _service.AddFilterSequenceSetting(model);
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
                var role = await _service.DeleteFilterSequenceSetting(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(FilterSequenceSettingController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] FilterSequenceSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditFilterSequenceSetting(model);

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
