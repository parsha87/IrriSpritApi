using GISApi.Controllers;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using GISApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GISApi.Services
{

    public interface IControllerMasterService
    {
        Task<List<ControllerMaster>> GetControllerMaster();
        Task<ControllerMaster> GetControllerMasterId(int controllerId);
        Task<ControllerMaster> AddControllerMaster(ControllerMaster model);
        Task<ControllerMaster> EditControllerMaster(ControllerMaster model);
        Task<bool> DeleteControllerMaster(int controllerId);

        Task<List<UserControllerMapping>> GetControllersByUserId(string userId);
    }




    public class ControllerMasterService : IControllerMasterService
    {
        private readonly ILogger<ControllerMasterService> _logger;

        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public ControllerMasterService(GlobalDBContext globalDBContext, ILogger<ControllerMasterService> logger)
        {
            _globalDBContext = globalDBContext;
            _logger = logger;
        }

        public async Task<ControllerMaster> AddControllerMaster(ControllerMaster model)
        {
            try
            {
                var result = await _globalDBContext.ControllerMasters.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(AddControllerMaster) + "]" + ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteControllerMaster(int controllerId)
        {
            try
            {
                var result = await _GlobalDBContext.ControllerMasters.Where(x => x.Id == controllerId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(DeleteControllerMaster) + "]" + ex);

                throw ex;
            }
        }

        public async Task<ControllerMaster> EditControllerMaster(ControllerMaster model)
        {
            try
            {
                var result = _globalDBContext.ControllerMasters.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(EditControllerMaster) + "]" + ex);

                throw ex;
            }
        }

        public async Task<List<ControllerMaster>> GetControllerMaster()
        {
            try
            {
                List<ControllerMaster> controllerMasters = new List<ControllerMaster>();
                controllerMasters = await _globalDBContext.ControllerMasters.Where(x => x.IsAssigned == false).ToListAsync();
                return controllerMasters;

            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(GetControllerMaster) + "]" + ex);

                throw ex;
            }
        }

        public async Task<ControllerMaster> GetControllerMasterId(int controllerId)
        {
            try
            {
                ControllerMaster model = new ControllerMaster();
                model = await _globalDBContext.ControllerMasters.Where(x => x.Id == controllerId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(GetControllerMasterId) + "]" + ex);
                throw ex;
            }
        }


        public async Task<List<UserControllerMapping>> GetControllersByUserId(string userId)
        {
            try
            {
                List<UserControllerMapping> modelList = new List<UserControllerMapping>();
                modelList = await _globalDBContext.UserControllerMappings.Where(x => x.UserId == userId).ToListAsync();
                return modelList;
            }
            catch (Exception ex)
            {
                _logger.LogError("[" + nameof(ControllerMasterService) + "." + nameof(GetControllersByUserId) + "]" + ex);
                throw ex;
            }
        }
    }
}
