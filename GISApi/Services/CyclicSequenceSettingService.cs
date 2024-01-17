using DocumentFormat.OpenXml.Bibliography;
using GISApi.Data;
using GISApi.Data.GlobalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface ICyclicSequenceSettingService
    {
        Task<List<CyclicSequenceSetting>> GetCyclicSequenceSetting();
        Task<CyclicSequenceSetting> GetCyclicSequenceSettingId(int cyclicId);
        Task<CyclicSequenceSetting> AddCyclicSequenceSetting(CyclicSequenceSetting model);
        Task<CyclicSequenceSetting> EditCyclicSequenceSetting(CyclicSequenceSetting model);
        Task <bool>DeleteCyclicSequenceService(int cyclicId);

        Task<List<CyclicSequenceSetting>> GetDataByControllerId(int controllerId);
    }
    public class CyclicSequenceSettingService : ICyclicSequenceSettingService
    {
        private readonly GlobalDBContext _globalDBContext;
        private GlobalDBContext _GlobalDBContext;

        public CyclicSequenceSettingService(GlobalDBContext globalDBContext)
        {
          _globalDBContext = globalDBContext;   
        }

        public async Task<CyclicSequenceSetting> AddCyclicSequenceSetting(CyclicSequenceSetting model)
        {
            try
            {
                var result = await _globalDBContext.CyclicSequenceSettings.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();
                return model;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public async Task<bool> DeleteCyclicSequenceService(int cyclicId)
        {
            try
            {
                var result = await _GlobalDBContext.CyclicSequenceSettings.Where(x => x.Id == cyclicId).FirstOrDefaultAsync();
                _GlobalDBContext.Remove(result);
                await _GlobalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<CyclicSequenceSetting> EditCyclicSequenceSetting(CyclicSequenceSetting model)
        {
            try
            {
                var result = _globalDBContext.CyclicSequenceSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CyclicSequenceSetting>> GetCyclicSequenceSetting()
        {
            try
            {
                List<CyclicSequenceSetting> cyclicSequenceSettings = new List<CyclicSequenceSetting>();
                cyclicSequenceSettings = await _globalDBContext.CyclicSequenceSettings.ToListAsync();
                return cyclicSequenceSettings;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CyclicSequenceSetting> GetCyclicSequenceSettingId(int cyclicId)
        {
            try
            {
                CyclicSequenceSetting model = new CyclicSequenceSetting();
                model = await _globalDBContext.CyclicSequenceSettings.Where(x => x.Id == cyclicId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CyclicSequenceSetting>> GetDataByControllerId(int controllerId)
        {
            try
            {
               List<CyclicSequenceSetting> model = new List<CyclicSequenceSetting>();
                model = await _globalDBContext.CyclicSequenceSettings.Where(x => x.ControllerId == controllerId).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }  
  
    
}
