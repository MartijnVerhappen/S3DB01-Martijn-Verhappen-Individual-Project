using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.CustomExceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException() : base("Failed to get Entity")
        {
        }
    }
}
