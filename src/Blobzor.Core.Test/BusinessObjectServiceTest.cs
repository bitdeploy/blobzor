using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blobzor.Core.Interface;
using Blobzor.Core.Service;
using Blobzor.Data.Context;
using Blobzor.Data.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blobzor.Core.Test
{
    public class BusinessObjectServiceTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly SqliteConnection _connection;

        public BusinessObjectServiceTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IBusinessObjectService, BusinessObjectService>();
            serviceCollection.AddTransient<IBusinessObjectRepository, BusinessObjectRepository>();
            serviceCollection.AddLogging();

            serviceCollection.AddDbContext<AppDbContext>(options => 
            {
                options.UseSqlite(_connection);
            }, ServiceLifetime.Transient);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _serviceProvider.GetService<AppDbContext>().Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithoutParameter()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entities = await testService.GetAsync();

            // Assert
            Assert.Single(entities);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithIntParameter()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = await testService.GetAsync(1);

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithIntParameter2()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = await testService.GetAsync(99);

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithUppercaseSearch()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = await testService.GetAsync("FIRSTNAME");

            // Assert
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Add_BusinessObject_Single()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            // Act
            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Assert
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = (await testService.GetAsync()).FirstOrDefault();

            Assert.Equal("FirstName", entity.FirstName);
            Assert.Equal("LastName", entity.LastName);
            Assert.Equal(new DateTime(1976, 8, 22),entity.BirthDay);
            Assert.Equal("City", entity.City);
        }

        [Fact]
        public async Task Update_BusinessObject()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var updateService = _serviceProvider.GetService<IBusinessObjectService>();
            var updEntity = (await updateService.GetAsync()).FirstOrDefault();

            updEntity.FirstName = "FirstName Update";
            updEntity.LastName = "LastName Update";
            updEntity.City = "City Update";
            updEntity.BirthDay = new DateTime(2000,1,1);

            await updateService.UpdateAsync(updEntity);

            // Assert
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = (await testService.GetAsync()).FirstOrDefault();

            Assert.Equal("FirstName Update", entity.FirstName);
            Assert.Equal("LastName Update", entity.LastName);
            Assert.Equal(new DateTime(2000, 1, 1),entity.BirthDay);
            Assert.Equal("City Update", entity.City);
        }

        [Fact]
        public async Task Delete_BusinessObject()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            await service.AddAsync(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            // Act
            var deleteService = _serviceProvider.GetService<IBusinessObjectService>();
            var entity = (await deleteService.GetAsync()).FirstOrDefault();
            
            await deleteService.DeleteAsync(entity);

            // Assert
            var testService = _serviceProvider.GetService<IBusinessObjectService>();
            var entities = await testService.GetAsync();
            Assert.Empty(entities);
        }

        [Fact]
        public async Task Refresh_BusinessObject()
        {
            // Arrange
            var service = _serviceProvider.GetService<IBusinessObjectService>();

            var addEntity = new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            };

            await service.AddAsync(addEntity);

            var updateService = _serviceProvider.GetService<IBusinessObjectService>();
            var updateEntity = (await updateService.GetAsync()).FirstOrDefault();
            
            updateEntity.FirstName = "FirstName Update";
            updateEntity.LastName = "LastName Update";
            updateEntity.City = "City Update";
            updateEntity.BirthDay = new DateTime(2000,1,1);

            await updateService.UpdateAsync(updateEntity);

            Assert.Equal(updateEntity.Id, addEntity.Id);
            Assert.NotEqual(updateEntity.FirstName, addEntity.FirstName);
            Assert.NotEqual(updateEntity.LastName, addEntity.LastName);
            Assert.NotEqual(updateEntity.BirthDay, addEntity.BirthDay);

            // Act
            await service.ReloadAsync();

            // Assert
            Assert.Equal(updateEntity.Id, addEntity.Id);
            Assert.Equal(updateEntity.FirstName, addEntity.FirstName);
            Assert.Equal(updateEntity.LastName, addEntity.LastName);
            Assert.Equal(updateEntity.BirthDay, addEntity.BirthDay);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _serviceProvider.GetService<AppDbContext>().Database.EnsureDeleted();
                    _serviceProvider.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {            
            Dispose(true);
        }
        #endregion
    }
}
