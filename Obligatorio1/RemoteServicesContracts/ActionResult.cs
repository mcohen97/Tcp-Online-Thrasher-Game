using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionResults
{
    public abstract class ActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
