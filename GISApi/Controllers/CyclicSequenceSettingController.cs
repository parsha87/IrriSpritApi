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
    public class CyclicSequenceSettingController : ControllerBase
    {
        private readonly ILogger<CyclicSequenceSettingController> _logger;
        private readonly IMapper _mapper;
        private readonly ICyclicSequenceSettingService _service;

        public CyclicSequenceSettingController(ILogger<CyclicSequenceSettingController> logger, IMapper mapper, ICyclicSequenceSettingService service)
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
        public async Task<ActionResult<List<CyclicSequenceSetting>>> GetCyclicSequenceSetting()
        {
            try
            {
                List<CyclicSequenceSetting> list = await _service.GetCyclicSequenceSetting();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(CyclicSequenceSettingController) + "." + nameof(GetCyclicSequenceSetting) + "]" + ex);
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
        public async Task<ActionResult<CyclicSequenceSetting>> Get(int id)
        {
            try
            {
                CyclicSequenceSetting model = await _service.GetCyclicSequenceSettingId(id);
             

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(CyclicSequenceSettingController) + "." + nameof(Get) + "]" + ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get users by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User model</returns>
        [HttpGet("CyclicSequenceByControllerId/{id}")]
        //[Authorize(Policy = "Permissions.Site Admin.User.ReadOnly,Permissions.Site Admin.User.AddUpdateDelete")]
        public async Task<ActionResult<CyclicSequenceSetting>> GetCyclicSequencegByControllerId(int id)
        {
            try
            {
                CyclicSequenceSetting model = await _service.GetDataByControllerId(id);
              

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(CyclicSequenceSettingController) + "." + nameof(GetCyclicSequencegByControllerId) + "]" + ex);
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
        public async Task<IActionResult> Post(CyclicSequenceSetting model)
        {
            try
            {
                var result = await _service.AddCyclicSequenceSetting(model);
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
                var role = await _service.DeleteCyclicSequenceService(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(CyclicSequenceSettingController) + "." + nameof(Delete) + "]" + ex);
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
        public async Task<ActionResult<OperationResult<ControllerTimeSetting>>> Put(int id, [FromBody] CyclicSequenceSetting model)
        {

            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = _service.EditCyclicSequenceSetting(model);

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
