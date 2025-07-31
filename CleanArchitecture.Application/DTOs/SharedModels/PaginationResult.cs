using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.SharedModels
{
    public class PaginationResult<T>
    {
        public List<T> Result { get; set; }
        public long TotalCount { get; set; }
    }
}
