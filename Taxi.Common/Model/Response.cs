﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Taxi.Common.Model
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }

}
