using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.ServiceProcess;
using System.Diagnostics;


namespace ConsoleApp1
{
    class Program
    {


        public static void DeviceDetail()
        {
            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());
            XDocument xmlDocument = XDocument.Load(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices.xml");
            var devices = new List<Device>(account.devices);


            var elDevice = xmlDocument.Descendants().First(d => d.Name == "Device");
            for (int i = 0; i < devices.Count; i++)
            {
                //XElement en = new XElement(elDeviceNetwork);
                XElement el = new XElement(elDevice);
                el.Element("DeviceID").Value = devices[i].deviceId;
                el.Element("Description").Value = devices[i].decription;
                el.Element("DeviceType").Value = devices[i].deviceType;

                xmlDocument.XPathSelectElement("//Devices/Device/Network/IPAddress").Value = devices[i].ipAddress;
                xmlDocument.XPathSelectElement("//Devices/Device/Network/SubnetMask").Value = devices[i].subnetMask;
                xmlDocument.XPathSelectElement("//Devices/Device/Network/DefaultGateway").Value = devices[i].defaultGateway;
                xmlDocument.XPathSelectElement("//Devices/Device/Network/PrimaryDNS").Value = devices[i].primaryDns;
                elDevice.AddBeforeSelf(el);
            }
           
         

            xmlDocument.Save(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices100.xml");


            

          
            
            //removes my dummy device
            //elDevice.Remove();


        }

        public static void NetworkDetail()
        {
            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());
            XDocument xmlDocument = XDocument.Load(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices100.xml");
            var devices = new List<Device>(account.devices);
            var elDeviceNetwork = xmlDocument.Descendants().FirstOrDefault(n => n.Name == "Network");

            int i = 0;
            var elTest = xmlDocument.Descendants("Network").Where(x => x.Element(name: "SubnetMask").Value=="");
            
            foreach(Device device in devices)
            {
                XElement en = new XElement(elDeviceNetwork);
                en.Element("IPAddress").Value = devices[i].ipAddress;
                en.Element("SubnetMask").Value = devices[i].subnetMask;
                en.Element("DefaultGateway").Value = devices[i].defaultGateway;
                en.Element("PrimaryDNS").Value = devices[i].primaryDns;
                elDeviceNetwork.AddBeforeSelf(en);
                i++;
            }


          /** for(int i = 0; i < devices.Count; i++) {
                XElement en = new XElement(elDeviceNetwork);
                for (int j = 0; j < 1; j++)
                {
                    

                    en.Element("IPAddress").Value = devices[i].ipAddress;
                    en.Element("SubnetMask").Value = devices[i].subnetMask;
                    en.Element("DefaultGateway").Value = devices[i].defaultGateway;
                    en.Element("PrimaryDNS").Value = devices[i].primaryDns;

                    
                }
                elDeviceNetwork.AddBeforeSelf(en);

            }**/
            //elDeviceNetwork.Remove();
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
          
            return response.Content;
            
            
        }



        static void Main(string[] args)
        {
            DeviceDetail();
            //NetworkDetail();
            
 
        }
    }
}
