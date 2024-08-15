using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Domain.Entities.BaseEntity
{
    public interface IAuditableEntity
    {
        public int Id { get; set; }
    }
}
