using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface IValveSettingService
    {
        Task<List<ValveSetting>> GetValveSetting();
        Task<ValveSetting> GetValveSettingId(int valveId);
        Task<ValveSetting> AddValveSetting(ValveSetting model);
        Task<ValveSetting> EditValveSetting(ValveSetting model);
        Task<bool> DeleteValveSetting(int valveId);
        Task<ValveSetting> GetDataByControllerId(int controllerId);
    }
    public class ValveSettingService: IValveSettingService
    {
        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public ValveSettingService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<ValveSetting> AddValveSetting(ValveSetting model)
        {
            try
            {
                var result = await _globalDBContext.ValveSettings.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteValveSetting(int valveId)
        {
            try
            {
                var result = await _GlobalDBContext.ValveSettings.Where(x => x.Id == valveId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ValveSetting> EditValveSetting(ValveSetting model)
        {
            try
            {
                var result = _globalDBContext.ValveSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ValveSetting>> GetValveSetting()
        {
            try
            {
                List<ValveSetting> ValveSettings = new List<ValveSetting>();
                ValveSettings = await _globalDBContext.ValveSettings.ToListAsync();
                return ValveSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ValveSetting> GetValveSettingId(int valveId)
        {
            try
            {
                ValveSetting model = new ValveSetting();
                model = await _globalDBContext.ValveSettings.Where(x => x.Id == valveId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ValveSetting> GetDataByControllerId(int controllerId)
        {
            try
            {
                ValveSetting model = new ValveSetting();
                model = await _globalDBContext.ValveSettings.Where(x => x.ControllerId == controllerId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
