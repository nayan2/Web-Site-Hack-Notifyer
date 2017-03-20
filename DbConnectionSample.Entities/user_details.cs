using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConnectionSample.Entities
{
    public class user_details
    {
        public string username { get; set; }

        public string password { get; set; }

        public string e_from { get; set; }

        public string e_password { get; set; }

        public int port { get; set; }

        public string enablessi { get; set; }

        public string usedefaultcredentials { get; set; }

        public string notifythroughemail { get; set; } 

    }
}
