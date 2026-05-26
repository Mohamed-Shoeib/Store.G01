using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class VaildationException(IEnumerable<string> errors) : Exception("Validation Errors")
    {
        public IEnumerable<string> Errors { get; } = errors;
    }
}
