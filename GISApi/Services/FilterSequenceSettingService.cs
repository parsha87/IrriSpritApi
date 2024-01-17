using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface IFilterSequenceSettingService
    {
        Task<List<FilterSequenceSetting>> GetFilterSequenceSetting();
        Task<FilterSequenceSetting> GetFilterSequenceSettingId(int filterId);
        Task<FilterSequenceSetting> AddFilterSequenceSetting(FilterSequenceSetting model);
        Task<FilterSequenceSetting> EditFilterSequenceSetting(FilterSequenceSetting model);
        Task<bool> DeleteFilterSequenceSetting(int filterId);
        Task<List<FilterSequenceSetting>> GetDataByControllerId(int controllerId);
    }
    public class FilterSequenceSettingService : IFilterSequenceSettingService
    {
        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public FilterSequenceSettingService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<FilterSequenceSetting> AddFilterSequenceSetting(FilterSequenceSetting model)
        {
            try
            {
                var result = await _globalDBContext.FilterSequenceSettings.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteFilterSequenceSetting(int filterId)
        {
            try
            {
                var result = await _GlobalDBContext.FilterSequenceSettings.Where(x => x.Id == filterId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {            
                throw ex;
            }
        }

        public async Task<FilterSequenceSetting> EditFilterSequenceSetting(FilterSequenceSetting model)
        {
            try
            {
                var result = _globalDBContext.FilterSequenceSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FilterSequenceSetting> GetFilterSequenceSettingId(int filterId)
        {
            try
            {
                FilterSequenceSetting model = new FilterSequenceSetting();
                model = await _globalDBContext.FilterSequenceSettings.Where(x => x.Id == filterId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  async Task<List<FilterSequenceSetting>> GetFilterSequenceSetting()
        {
            try
            {
                List<FilterSequenceSetting> FilterSequenceSettings = new List<FilterSequenceSetting>();
                FilterSequenceSettings = await _globalDBContext.FilterSequenceSettings.ToListAsync();
                return FilterSequenceSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FilterSequenceSetting>> GetDataByControllerId(int controllerId)
        {
            try
            {
                List<FilterSequenceSetting> model = new List< FilterSequenceSetting>();
                model = await _globalDBContext.FilterSequenceSettings.Where(x => x.ControllerId == controllerId).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
