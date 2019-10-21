using System;
using Blobzor.Core.Interface;
using Blobzor.Data.Context;
using Blobzor.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Blobzor.Data
{
    public static class ServicesConfiguration
    {
        public static void AddData(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
            }, ServiceLifetime.Transient);
            
            serviceCollection.AddTransient<IBusinessObjectRepository, BusinessObjectRepository>();
        }
    }
}
