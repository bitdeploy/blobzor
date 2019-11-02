using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blobzor.Core.Interface;
using Blobzor.Data.Context;
using Blobzor.Data.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blobzor.Data.Test
{
    public class BusinessObjectRepositoryTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly SqliteConnection _connection;

        public BusinessObjectRepositoryTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IBusinessObjectRepository, BusinessObjectRepository>();

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
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entities = await testRepo.GetAsync();

            // Assert
            Assert.Single(entities);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithIntParameter()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = await testRepo.GetAsync(1);

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithIntParameter2()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = await testRepo.GetAsync(99);

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public async Task GetAsync_BusinessObject_WithLambdaExpression()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = await testRepo.GetAsync(x => x.FirstName == "FirstName" && x.LastName == "LastName");

            // Assert
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Add_BusinessObject_Single()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            // Act
            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Assert
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = (await testRepo.GetAsync()).FirstOrDefault();

            Assert.Equal("FirstName", entity.FirstName);
            Assert.Equal("LastName", entity.LastName);
            Assert.Equal(new DateTime(1976, 8, 22),entity.BirthDay);
            Assert.Equal("City", entity.City);
        }

        [Fact]
        public async Task Add_BusinessObject_List()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var businessObjects = new List<Core.Model.Domain.BusinessObject>();

            // Act
            for (int i = 1; i <= 100; i++)
            {
                businessObjects.Add(new Core.Model.Domain.BusinessObject 
                {
                    FirstName = $"FirstName {i}",
                    LastName = $"LastName {i}",
                    BirthDay = new DateTime(1976, 8, 22).AddDays(i),
                    City = $"City {i}"
                });
            }
            
            addRepo.Add(businessObjects);
            await addRepo.SaveAsync();

            // Assert
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entities = await testRepo.GetAsync();

            Assert.NotEmpty(entities);
            Assert.Equal(100, entities.Count());
            
            Assert.All(entities, item => Assert.Contains("FirstName", item.FirstName));
            Assert.All(entities, item => Assert.Contains("LastName", item.LastName));
            Assert.All(entities, item => Assert.Contains("City", item.City));
        }

        [Fact]
        public async Task Update_BusinessObject()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var updateRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var updEntity = (await updateRepo.GetAsync()).FirstOrDefault();

            updEntity.FirstName = "FirstName Update";
            updEntity.LastName = "LastName Update";
            updEntity.City = "City Update";
            updEntity.BirthDay = new DateTime(2000,1,1);

            updateRepo.Update(updEntity);
            await updateRepo.SaveAsync();

            // Assert
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = (await testRepo.GetAsync()).FirstOrDefault();

            Assert.Equal("FirstName Update", entity.FirstName);
            Assert.Equal("LastName Update", entity.LastName);
            Assert.Equal(new DateTime(2000, 1, 1),entity.BirthDay);
            Assert.Equal("City Update", entity.City);
        }

        [Fact]
        public async Task Delete_BusinessObject()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            addRepo.Add(new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            });

            await addRepo.SaveAsync();

            // Act
            var deleteRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entity = (await deleteRepo.GetAsync()).FirstOrDefault();
            
            deleteRepo.Delete(entity);
            await deleteRepo.SaveAsync();

            // Assert
            var testRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var entities = await deleteRepo.GetAsync();
            Assert.Empty(entities);
        }

        [Fact]
        public async Task Refresh_BusinessObject()
        {
            // Arrange
            var addRepo = _serviceProvider.GetService<IBusinessObjectRepository>();

            var addEntity = new Core.Model.Domain.BusinessObject 
            {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDay = new DateTime(1976, 8, 22),
                City = "City"
            };

            addRepo.Add(addEntity);
            await addRepo.SaveAsync();

            var updateRepo = _serviceProvider.GetService<IBusinessObjectRepository>();
            var updateEntity = (await updateRepo.GetAsync()).FirstOrDefault();
            
            updateEntity.FirstName = "FirstName Update";
            updateEntity.LastName = "LastName Update";
            updateEntity.City = "City Update";
            updateEntity.BirthDay = new DateTime(2000,1,1);

            updateRepo.Update(updateEntity);
            await updateRepo.SaveAsync();

            Assert.Equal(updateEntity.Id, addEntity.Id);
            Assert.NotEqual(updateEntity.FirstName, addEntity.FirstName);
            Assert.NotEqual(updateEntity.LastName, addEntity.LastName);
            Assert.NotEqual(updateEntity.BirthDay, addEntity.BirthDay);

            // Act
            await addRepo.ReloadAsync();

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
