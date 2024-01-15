using DocumentFormat.OpenXml.Wordprocessing;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services

{
    public interface IControllerTimeSettingService
    {
       Task< List<ControllerTimeSetting>> GetTimeSettingList();
        Task<ControllerTimeSetting> GetTimeSettingId(int timesettingId);
        Task<ControllerTimeSetting> AddTimeSetting(ControllerTimeSetting timesetting);
        Task<ControllerTimeSetting> EditTimeSetting(ControllerTimeSetting model);
        Task<bool>DeleteControllerTimeSetting(int timesettingId);
    }
    public class ControllerTimeSettingService : IControllerTimeSettingService
    {
        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public ControllerTimeSettingService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<ControllerTimeSetting> AddTimeSetting(ControllerTimeSetting timesetting)
        {
            try
            {
                var result = _globalDBContext.ControllerTimeSettings.AddAsync(timesetting);
                await _globalDBContext.SaveChangesAsync();

                return timesetting;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteControllerTimeSetting(int timesettingId)
        {
            try
            {
                var result = await _GlobalDBContext.ControllerTimeSettings.Where(x => x.Id == timesettingId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ControllerTimeSetting> EditTimeSetting(ControllerTimeSetting model)
        {
            try
            {
                var resukt = _globalDBContext.ControllerTimeSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ControllerTimeSetting> GetTimeSettingId(int timesettingId)
        {
            try
            {
                ControllerTimeSetting model = new ControllerTimeSetting();
                model= await _globalDBContext.ControllerTimeSettings.Where(x => x.Id == timesettingId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ControllerTimeSetting>> GetTimeSettingList()
        {
            try
            {
                List<ControllerTimeSetting> controllerTimeSettings = new List<ControllerTimeSetting>();
                controllerTimeSettings = await _globalDBContext.ControllerTimeSettings.ToListAsync();

                return controllerTimeSettings;
            }
            catch (Exception)          
            {
                throw;
            }
        }
    }


}
