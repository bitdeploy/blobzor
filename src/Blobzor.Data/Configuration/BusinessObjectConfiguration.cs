using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blobzor.Core.Model.Domain;

namespace Blobzor.Data.Configuration
{
    class BusinessObjectConfiguration: IEntityTypeConfiguration<BusinessObject>    
    {
        public void Configure(EntityTypeBuilder<BusinessObject> builder)
        {
            builder
                .HasKey(key => key.Id);
        }
    }
}