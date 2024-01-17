using GISApi.Data.GlobalEntities;
using GISApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Services
{
    public interface ISequenceSettingService
    {
        Task<List<SequenceSetting>> GetSequenceSetting();
        Task<SequenceSetting> GetSequenceSettingId(int valveId);
        Task<SequenceSetting> AddSequenceSetting(SequenceSetting model);
        Task<SequenceSetting> EditSequenceSetting(SequenceSetting model);
        Task<bool> DeleteSequenceSetting(int valveId);
        Task<List<SequenceSetting>> GetDataByControllerId(int controllerId);
    }
    public class SequenceSettingService: ISequenceSettingService
    {
        private readonly GlobalDBContext _globalDBContext;

        public SequenceSettingService(GlobalDBContext globalDBContext)
        {
            _globalDBContext = globalDBContext;
        }

        public async Task<SequenceSetting> AddSequenceSetting(SequenceSetting model)
        {
            try
            {
                var result = await _globalDBContext.SequenceSettings.AddAsync(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteSequenceSetting(int valveId)
        {
            try
            {
                var result = await _globalDBContext.SequenceSettings.Where(x => x.Id == valveId).FirstOrDefaultAsync();
                _globalDBContext.Remove(result);
                await _globalDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SequenceSetting> EditSequenceSetting(SequenceSetting model)
        {
            try
            {
                var result = _globalDBContext.SequenceSettings.Update(model);
                await _globalDBContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SequenceSetting>> GetSequenceSetting()
        {
            try
            {
                List<SequenceSetting> SequenceSettings = new List<SequenceSetting>();
                SequenceSettings = await _globalDBContext.SequenceSettings.ToListAsync();
                return SequenceSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SequenceSetting> GetSequenceSettingId(int valveId)
        {
            try
            {
                SequenceSetting model = new SequenceSetting();
                model = await _globalDBContext.SequenceSettings.Where(x => x.Id == valveId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SequenceSetting>> GetDataByControllerId(int controllerId)
        {
            try
            {
                List<SequenceSetting> model = new List<SequenceSetting>();
                model = await _globalDBContext.SequenceSettings.Where(x => x.ControllerId == controllerId).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
