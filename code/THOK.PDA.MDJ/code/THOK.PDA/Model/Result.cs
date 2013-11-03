using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.PDA.Model
{
    public class Result
    {
        public bool IsSuccess = false;
        public string Message = string.Empty;
        public object Data { get; set; }
        public RestTask[] RestTasks = new RestTask[] { };
    }
}
