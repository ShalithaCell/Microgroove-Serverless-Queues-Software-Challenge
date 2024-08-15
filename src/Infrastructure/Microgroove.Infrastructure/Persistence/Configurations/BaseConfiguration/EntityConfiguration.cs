using Microgroove.Domain.Entities.BaseEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Infrastructure.Persistence.Configurations.BaseConfiguration
{
    /// <summary>
    /// Base Db Model Configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IAuditableEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            AppendConfig(builder);
        }

        protected abstract void AppendConfig(EntityTypeBuilder<T> builder);
    }
}
