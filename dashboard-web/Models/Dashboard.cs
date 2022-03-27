using System;
using System.Collections.Generic;

namespace dashboard_web.Models
{
    public class Dashboard
    {
        public int ID { get; set; }
        public List<Credentials> credentials { get; set; }

        public Dashboard()
        {
          
        }
    }
}
