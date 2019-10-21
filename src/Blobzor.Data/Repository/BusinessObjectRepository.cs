using System;
using Blobzor.Core.Interface;
using Blobzor.Core.Model.Domain;
using Blobzor.Data.Context;

namespace Blobzor.Data.Repository
{
    public class BusinessObjectRepository : RepositoryBase<BusinessObject>, IBusinessObjectRepository
    {
        public BusinessObjectRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}