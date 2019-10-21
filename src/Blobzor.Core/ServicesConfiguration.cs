using System;
using Blobzor.Core.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Blobzor.Core
{
    public static class ServicesConfiguration
    {
        public static void AddCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBusinessObjectService, BusinessObjectService>();
        }
    }
}
