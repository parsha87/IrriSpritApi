using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface IMaxFiltervalveSettingService
    {
        Task<List<MaxFiltervalveSetting>> GetMaxFiltervalveSetting();
        Task<MaxFiltervalveSetting> GetFiltervalveSettingId(int filterValveId);
        Task<MaxFiltervalveSetting> AddMaxFiltervalveSetting(MaxFiltervalveSetting model);
        Task<MaxFiltervalveSetting> EditMaxFiltervalveSetting(MaxFiltervalveSetting model);
        Task<bool> DeleteMaxFiltervalveSetting(int filterValveId);
    }
    public class MaxFiltervalveSettingService: IMaxFiltervalveSettingService
    {
        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public MaxFiltervalveSettingService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<MaxFiltervalveSetting> AddMaxFiltervalveSetting(MaxFiltervalveSetting model)
        {
            try
            {
                var result = await _globalDBContext.MaxFiltervalveSettings.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMaxFiltervalveSetting(int filterValveId)
        {
            try
            {
                var result = await _GlobalDBContext.MaxFiltervalveSettings.Where(x => x.Id == filterValveId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MaxFiltervalveSetting> EditMaxFiltervalveSetting(MaxFiltervalveSetting model)
        {
            try
            {
                var result = _globalDBContext.MaxFiltervalveSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MaxFiltervalveSetting> GetFiltervalveSettingId(int filterValveId)
        {
            try
            {
                MaxFiltervalveSetting model = new MaxFiltervalveSetting();
                model = await _globalDBContext.MaxFiltervalveSettings.Where(x => x.Id == filterValveId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MaxFiltervalveSetting>> GetMaxFiltervalveSetting()
        {
            try
            {
                List<MaxFiltervalveSetting> MaxFiltervalveSettings = new List<MaxFiltervalveSetting>();
                MaxFiltervalveSettings = await _globalDBContext.MaxFiltervalveSettings.ToListAsync();
                return MaxFiltervalveSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
