﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Common
{
    public class SelectItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool Selected { get; set; }

        public object Select()
        {
            throw new NotImplementedException();
        }
    }
}