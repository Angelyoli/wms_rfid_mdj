﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Common.WebUtil
{
    public static class JsonMessageHelper
    {
        public static object getJsonMessage(bool success, string msg, object data)
        {
            return new { success = success, msg = msg, IsSuccess = success, Message = msg, data = data };
        }

        public static object getJsonMessage(bool success, string msg)
        {
            return getJsonMessage(success, msg, null);
        }
    }
}
