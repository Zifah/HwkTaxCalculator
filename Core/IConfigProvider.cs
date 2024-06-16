﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IConfigProvider
    {
        public T GetValue<T>(string key);
    }
}
