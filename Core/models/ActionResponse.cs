using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.models
{
    public class ActionResponse<T>
    {
        public Boolean Success { get; set; }

        public String Message { get; set; }
        public T Data { get; set; }
    }
}
