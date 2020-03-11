using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.ServiceProcess;
using System.Diagnostics;


namespace ConsoleApp1
{
    class Program
    {


        public static void demofunc2()
        {
            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());
            XDocument xmlDocument = XDocument.Load(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices.xml");
            var devices = new List<Device>(account.devices);

            var elDevice = xmlDocument.Descendants().First(d => d.Name == "Device");

            for (int i = 0; i < devices.Count; i++)
            {
                XElement el = new XElement(elDevice);
                el.Element("DeviceID").Value = devices[i].deviceId;
                el.Element("Description").Value = devices[i].decription;
                el.Element("DeviceType").Value = devices[i].deviceType;
                /**
                el.Element("IPAddress").Value = devices[i].ipAddress;
                el.Element("SubnetMask").Value = devices[i].subnetMask;
                el.Element("DefaultGateway").Value = devices[i].defaultGateway;
                el.Element("PrimaryDNS").Value = devices[i].primaryDns;
               **/
                elDevice.AddBeforeSelf(el);

                /** xmlDocument.Element("Devices").Add(
                 new XElement("Device", new XAttribute("DeviceID", devices[i].deviceId),
                     new XElement("Description", devices[i].decription),
                     new XElement("DeviceType", devices[i].deviceType)
                 ));**/
                /**xmlDocument.Element("Network").Add(
             
                   new XElement("IPAddress", devices[i].ipAddress),
                   new XElement("SubnetMask", devices[i].subnetMask),
                   new XElement("DefaultGateway", devices[i].defaultGateway),
                   new XElement("PrimaryDNS", devices[i].primaryDns)
               );**/
            }


            xmlDocument.Save(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices100.xml");
        }
  

        public static string getData()
        {
            var client = new RestClient("http://192.168.0.26:3000/api/v1/controlpoint/5d3252aa8322aa2434b9785e");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/javascript");
            request.AddParameter("application/javascript", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            //Console.WriteLine(account._id);

            //YOU WANT TO USE THIS OBJECT AS THE DATA YOU MANIPULATE INTO THE XML FILE. 
            //THIS IS FROM THE API.
            RootObject account = JsonConvert.DeserializeObject<RootObject>(response.Content);
            //Console.WriteLine(response.Content.ToString());
            //return "";
            return response.Content;
            
            
        }



        static void Main(string[] args)
        {
            demofunc2();
 
        }
    }
}
