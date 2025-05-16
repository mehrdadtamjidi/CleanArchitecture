using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Common
{
    public interface IEntity
    {
    }

    public abstract class BaseDomainEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
