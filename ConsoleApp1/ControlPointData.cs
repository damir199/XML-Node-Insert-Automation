using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Settings
    {
        public List<string> software { get; set; }
        public List<object> logfile { get; set; }
    }

    public class Device
    {
        public string _id { get; set; }
        public string deviceId { get; set; }
        public string description { get; set; }
        public string deviceType { get; set; }
        public string ipAddress { get; set; }
        public string subnetMask { get; set; }
        public string defaultGateway { get; set; }
        public string primaryDns { get; set; }
    }

    public class RootObject
    {
        public Settings settings { get; set; }
        public string _id { get; set; }
        public List<Device> devices { get; set; }
        public string dataSetName { get; set; }
        public int __v { get; set; }
        public string backup { get; set; }
        public string primary { get; set; }
        public string cpPrimary { get; set; }
    }
}
