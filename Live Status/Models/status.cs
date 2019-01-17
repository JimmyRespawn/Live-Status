using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Status.Models
{
    public class status
    {
        public string name { get; set; }
        public string description { get; set; }
        public string state { get; set; }
        public string stateIcon { get; set; }
        //appeared when limited
        public string affectedServices { get; set; }
        public string lastUpdate { get; set; }
        public List<string> affectedPlatforms { get; set; }
    }
}