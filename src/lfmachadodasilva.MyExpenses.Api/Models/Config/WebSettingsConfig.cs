﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lfmachadodasilva.MyExpenses.Api.Models.Config
{
    public class WebSettingsConfig
    {
        public bool ClearDatabaseAndSeedData { get; set; }

        public bool UseFakeDatabase { get; set; }
    }
}