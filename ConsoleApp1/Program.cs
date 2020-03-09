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
        
        public static string devicesXml()
        {

            //THIS IS YOUR PATH TO THE NODE IN TH XML YOU WOULD LIKE TO EDIT
            string path = "/Devices.xml/DetailData/Devices/Device";


            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());

            var devices = new List<Device>(account.devices);

            string xml = @"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices1.xml";
            
            //CREATES XML DOCUMENT USING XMLDOCUMENT, POSSIBLY TRY STREAM METHOD.
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            //SELECTIG THE NODES TO EDIT WITHIN THE XML, ACCORDING TO PATH ABOVE.
            XmlNodeList nodes = doc.SelectNodes(path);

            //Display all the book titles
            for (int i = 0; i < devices.Count; i++)
            {

                //THIS IS THE LOOP THAT MANIPULATES THE DATA IN THE XML FROM THE LIST RETRIEVED FROM API
                //Device Details
               nodes[i]["Description"].InnerText = devices[i].decription;
               nodes[i]["DeviceID"].InnerText = devices[i].deviceId;
               nodes[i]["DeviceType"].InnerText = devices[i].deviceType;
                
                //Network Details
                nodes[i]["Network"]["IPAddress"].InnerText = devices[i].ipAddress;
                nodes[i]["Network"]["SubnetMask"].InnerText = devices[i].subnetMask;
                nodes[i]["Network"]["DefaultGateway"].InnerText = devices[i].defaultGateway;
                nodes[i]["Network"]["PrimaryDNS"].InnerText = devices[i].primaryDns;

                return nodes.ToString();
                //Console.WriteLine("XML Updated");
            }


            doc.Save(@"C:\ProgramData\QSR Automation\ConnectSmart\ControlPointServer\Data\Devices.xml");
            Console.ReadLine();

            return "";
        }


        //DO NOT NEED, THIS IS A WORK IN PROGRESS BY NICK!!
        public static void csNetwork()
        {
            string path = "/ConnectSmartNetwork.xml/DetailData/NetworkConfigurations/NetworkConfiguration";

            RootObject account = JsonConvert.DeserializeObject<RootObject>(getData());

            var numbers = new List<string>();
            numbers.Add(account.primary.ToString());
            numbers.Add(account.backup.ToString());
            numbers.Add(account.cpPrimary.ToString());

            string xml = @"dataSets\ConnectSmartNetwork.xml";

            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            XmlNodeList nodes = doc.SelectNodes(path);

            //Display all the book titles
            for (int i = 0; i < numbers.Count; i++)
            {
                //Device Details
                nodes[i]["IPAddress"].InnerText = numbers[i]; 
            }

            Console.WriteLine("XML Updated");
            doc.Save(@"C:\ProgramData\QSR Automations\ConnectSmart\Common\Data\ConnectSmartNetwork.xml");
            Console.ReadLine();

            
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
            devicesXml();
            //getData();
            //RestartWindowsService();
            //devicesXml();
            //csNetwork();
        }
    }
}
