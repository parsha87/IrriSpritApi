using GISApi.Data;
using GISApi.Data.GlobalEntities;
using GISApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{

    public interface IControllerMasterService
    {
        Task<List<ControllerMaster>> GetControllerMaster();
        Task<ControllerMaster> GetControllerMasterId(int controllerId);
        Task<ControllerMaster> AddControllerMaster(ControllerMaster model);
        Task<ControllerMaster> EdirControllerMaster(ControllerMaster model);
        Task<bool> DeleteControllerMaster(int controllerId);
    }




    public class ControllerMasterService: IControllerMasterService
    {
        private readonly GlobalDBContext _globalDBContext;

        public ControllerMasterService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<ControllerMaster> AddControllerMaster(ControllerMaster model)
        {
            try
            {
                var result = await _globalDBContext.ControllerMasters.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> DeleteControllerMaster(int controllerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ControllerMaster> EdirControllerMaster(ControllerMaster model)
        {
            try
            {
                var result = _globalDBContext.ControllerMasters.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ControllerMaster>> GetControllerMaster()
        {
            try
            {
                List<ControllerMaster> controllerMasters = new List<ControllerMaster>();
                controllerMasters = await _globalDBContext.ControllerMasters.ToListAsync();
                return controllerMasters;

            }
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
