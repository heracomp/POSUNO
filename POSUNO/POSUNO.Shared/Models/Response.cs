using System;
using System.Collections.Generic;
using System.Text;

namespace POSUNO.Models
{
    public class Response
    {
        public bool IsSuccesss { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
