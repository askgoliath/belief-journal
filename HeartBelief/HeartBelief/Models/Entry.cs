
using System;
using System.Collections.Generic;
using System.Text;

namespace HeartBelief.Models
{
    public class Entry : DbModel
    {
        public string OldBelief { get; set; }

        public string NewBelief { get; set; }
    }
}
