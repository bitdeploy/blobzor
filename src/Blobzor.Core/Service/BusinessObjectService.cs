using System;
using Blobzor.Core.Interface;
using Microsoft.Extensions.Logging;

namespace Blobzor.Core.Service
{
    public interface IBusinessObjectService
    {
    
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
    }
}