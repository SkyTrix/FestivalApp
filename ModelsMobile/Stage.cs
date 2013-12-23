using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Stage
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
