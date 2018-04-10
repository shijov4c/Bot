﻿using System;
using System.Collections.Generic;

namespace PictureBot.Models
{
    [Serializable]
    public class SearchHit
    {
        public SearchHit()
        {
            this.PropertyBag = new Dictionary<string, object>();
        }

        public string Key { get; set; }

        public string Title { get; set; }

        public IDictionary<string, object> PropertyBag { get; set; }
    }
}