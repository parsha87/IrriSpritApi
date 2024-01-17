using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface IControllerTimeSettingService
    {
        Task<List<ControllerTimeSetting>> GetTimeSettingList();
        Task<ControllerTimeSetting> GetTimeSettingId(int timesettingId);
        Task<ControllerTimeSetting> AddTimeSetting(ControllerTimeSetting timesetting);
        Task<ControllerTimeSetting> EditTimeSetting(ControllerTimeSetting model);
        Task<bool> DeleteControllerTimeSetting(int timesettingId);
        Task<List<ControllerTimeSetting>> GetDataByControllerId(int controllerId);
    }
    public class ControllerTimeSettingService : IControllerTimeSettingService
    {
        private GlobalDBContext _globalDBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ControllerTimeSettingService> _logger;
        public ControllerTimeSettingService(GlobalDBContext globalDBContext, ILogger<ControllerTimeSettingService> logger, IMapper mapper)
        {
            _globalDBContext = globalDBContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ControllerTimeSetting> AddTimeSetting(ControllerTimeSetting timesetting)
        {
            try
            {
                var result = await _globalDBContext.ControllerTimeSettings.AddAsync(timesetting);
                await _globalDBContext.SaveChangesAsync();

                return timesetting;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteControllerTimeSetting(int timesettingId)
        {
            try
            {
                var result = await _globalDBContext.ControllerTimeSettings.Where(x => x.Id == timesettingId).FirstOrDefaultAsync();
                _globalDBContext.Remove(result);
                await _globalDBContext.SaveChangesAsync();
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
                model = await _globalDBContext.ControllerTimeSettings.Where(x => x.Id == timesettingId).FirstOrDefaultAsync();
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
        public async Task<List<ControllerTimeSetting>> GetDataByControllerId(int controllerId)
        {
            try
            {
                List<ControllerTimeSetting> model = new List<ControllerTimeSetting>();
                model = await _globalDBContext.ControllerTimeSettings.Where(x => x.ControllerId == controllerId).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
