using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui
{
    public class response
    {
        public error error { get; set; }
        public dynamic results { get; set; }
    }

    public class error
    {
        public string errorMsg { get; set; }
        public  int errorCode { get; set; }
    }
    
}
