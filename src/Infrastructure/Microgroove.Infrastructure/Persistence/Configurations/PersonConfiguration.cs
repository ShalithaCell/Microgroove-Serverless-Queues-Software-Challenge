using Microgroove.Domain.Entities;
using Microgroove.Infrastructure.Persistence.Configurations.BaseConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microgroove.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        protected override void AppendConfig(EntityTypeBuilder<Person> builder)
        {
            builder.HasIndex(p => new { p.FirstName, p.LastName })
                .IsUnique();
        }
    }
}
