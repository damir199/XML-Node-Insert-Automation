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

            //Using the pre-made function to fetch data and deserialize to an object?
            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());
            //USing XDocument to load my 'Devices.xml' for insertion of data.
            XDocument xmlDocument = XDocument.Load(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices.xml");
            //create a variable 'devices' which is a list of all devices in the rootobject.
            var devices = new List<Device>(account.devices);
            //create an 'elDevice' variable to hold the 'Device' Node within the XML.
            var elDevice = xmlDocument.Descendants().First(d => d.Name == "Device");
            //For Loop to iterate through the number of devices that need adding to the file.
            for (int i = 0; i < devices.Count; i++)
            {
                //Create a new XElement that is a copy of the 'elDevice' element found above. 
                //This is to allow us to insert the data for N amount of devices in 'devices.count'.
                XElement el = new XElement(elDevice);
                //Insert the data required from the devices object using 'Element.value = ""' 
                el.Element("DeviceID").Value = devices[i].deviceId;
                el.Element("Description").Value = devices[i].decription;
                el.Element("DeviceType").Value = devices[i].deviceType;

                //As Network is an inner element i use XPathSelectELement to find the required elements within
                //the Device element we copied above -- el = new XElement(elDevice) -- and insert the data required
                //from the RootObject.
                el.XPathSelectElement("//Network/IPAddress").Value = devices[i].ipAddress;
                el.XPathSelectElement("//Network/SubnetMask").Value = devices[i].subnetMask;
                el.XPathSelectElement("//Network/DefaultGateway").Value = devices[i].defaultGateway;
                el.XPathSelectElement("//Network/PrimaryDNS").Value = devices[i].primaryDns;
                elDevice.AddBeforeSelf(el);
            }

            //remove the dummy device used to insert the data.
            elDevice.Remove();

            //save document
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
           
        }
    }
}
