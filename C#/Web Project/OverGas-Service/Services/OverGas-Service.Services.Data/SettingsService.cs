namespace OverGas-Service.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using OverGas-Service.Data.Common.Repositories;
    using OverGas-Service.Data.Models;
    using OverGas-Service.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return this.settingsRepository.All().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.settingsRepository.All().To<T>().ToList();
        }
    }
}
