using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blobzor.Core.Interface;
using Blobzor.Core.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Blobzor.Core.Service
{
    public interface IBusinessObjectService
    {
        Task<BusinessObject> GetAsync(int Id);
        Task<IEnumerable<BusinessObject>> GetAsync();
        Task<IEnumerable<BusinessObject>> GetAsync(string searchText);

        BusinessObject New();

        Task<BusinessObject> AddAsync(BusinessObject businessObject);

        Task UpdateAsync(BusinessObject businessObject);

        Task DeleteAsync(BusinessObject businessObject);

        Task ReloadAsync();
    }

    public class BusinessObjectService : IBusinessObjectService
    {
        private readonly IBusinessObjectRepository _repository;
        private readonly ILogger<BusinessObjectService> _logger;

        public BusinessObjectService(IBusinessObjectRepository repository, ILogger<BusinessObjectService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BusinessObject>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<BusinessObject> GetAsync(int Id)
        {
            return await _repository.GetAsync(Id);
        }

        public async Task<IEnumerable<BusinessObject>> GetAsync(string searchText)
        {
            return await _repository.GetAsync(x => 
                x.FirstName.ToLower() .Contains(searchText.ToLower()) ||
                x.LastName.ToLower() .Contains(searchText.ToLower()) ||
                x.City.ToLower() .Contains(searchText.ToLower()));
        }

        public BusinessObject New()
        {
            return new BusinessObject { Id = 0 };
        }

        public async Task<BusinessObject> AddAsync(BusinessObject businessObject)
        {
            _repository.Add(businessObject);
            await _repository.SaveAsync();

            return businessObject;
        }

        public async Task UpdateAsync(BusinessObject businessObject)
        {
            _repository.Update(businessObject);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(BusinessObject businessObject)
        {
            _repository.Delete(businessObject);
            await _repository.SaveAsync();
        }

        public async Task ReloadAsync()
        {
            await _repository.ReloadAsync();
        }
    }
}