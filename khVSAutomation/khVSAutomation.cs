using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Collections;
using System.Collections.Specialized;

using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.XPath;

using rv;

namespace khVSAutomation
{

    public class Automation
    {
        //static DateTime StartTime;

        private static PJLinkConnection c = null;
        private static List<Projector> m_objProjectors = null;
        private static List<Television> m_objTelevisions = null;
        private static List<ProjectorLift> m_objProjectorLifts = null;

        //HardCoded For Now
        public static string tv1IPAddress { get { return m_objTelevisions[0].TelevisionIPAddress; } }
        public static string tv1MACAddress { get { return m_objTelevisions[0].TelevisionMACAddress; } }
        public static string tv2IPAddress { get { return m_objTelevisions[1].TelevisionIPAddress; } }
        public static string tv2MACAddress { get { return m_objTelevisions[1].TelevisionMACAddress; } }
        public static string projectorIPAddress { get { return m_objProjectors[0].projectorIP; } }
        public static string liftCOMPort { get { return m_objProjectorLifts[0].LiftCOMPort; } }
        public static int liftMovementTime { get { return m_objProjectorLifts[0].LiftMoveTime; } }

        public Automation(bool p_blnWinForm = false)
        {
            //Initialize the Automation Class
            loadConfigurations();
        }

        private static void loadConfigurations()
        {
            try
            {
                
                //load the config document
                var l_strLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                l_strLocation = l_strLocation.Substring(0, l_strLocation.LastIndexOf("\\") + 1) + khVSAutomation.Properties.Settings.Default.ConfigFileName;
                XPathDocument xpdConfig = new XPathDocument(l_strLocation);
                XPathNavigator xpnConfigNav = xpdConfig.CreateNavigator();
                XPathNodeIterator l_objNodes = xpnConfigNav.Select("//Automation/Televisions/Television");

                m_objTelevisions = new List<Television>();
                foreach (XPathNavigator l_xmlTelevision in l_objNodes)
                {
                    string l_strName = l_xmlTelevision.GetAttribute("Name", string.Empty);
                    string l_strIP = l_xmlTelevision.GetAttribute("IP", string.Empty);
                    string l_strMAC = l_xmlTelevision.GetAttribute("MAC", string.Empty);

                    m_objTelevisions.Add(new Television(l_strName, l_strIP, l_strMAC));
                }

                l_objNodes = xpnConfigNav.Select("//Automation/Projectors/Projector");
                m_objProjectors = new List<Projector>();
                foreach (XPathNavigator l_xmlProjector in l_objNodes)
                {
                    string l_strName = l_xmlProjector.GetAttribute("Name", string.Empty);
                    string l_strIP = l_xmlProjector.GetAttribute("IP", string.Empty);
                    string l_strLiftAssociation = l_xmlProjector.GetAttribute("LiftAssociation", string.Empty);

                    m_objProjectors.Add(new Projector(l_strName, l_strIP, l_strLiftAssociation));
                }

                l_objNodes = xpnConfigNav.Select("//Automation/ProjectorLifts/ProjectorLift");
                m_objProjectorLifts = new List<ProjectorLift>();
                foreach (XPathNavigator l_xmlProjectorLifts in l_objNodes)
                {
                    string l_strName = l_xmlProjectorLifts.GetAttribute("Name", string.Empty);
                    string l_strCOMPort = l_xmlProjectorLifts.GetAttribute("COMPort", string.Empty);
                    int l_intMoveTime = Int32.Parse(l_xmlProjectorLifts.GetAttribute("MoveTime", string.Empty));

                    m_objProjectorLifts.Add(new ProjectorLift(l_strName, l_strCOMPort, l_intMoveTime));
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("loadConfigurations() ERROR: " + ex.ToString());
            }
        }

        public string displaySettings()
        {
            StringBuilder myOutput = new StringBuilder();

            myOutput.AppendLine("==============CURRENT APPLICATION SETTINGS==============");
            myOutput.AppendLine("tv1IPAddress:       " + tv1IPAddress);
            myOutput.AppendLine("tv1MACAddress:      " + tv1MACAddress);
            myOutput.AppendLine("tv2IPAddress:       " + tv2IPAddress);
            myOutput.AppendLine("tv2MACAddress:      " + tv2MACAddress);
            myOutput.AppendLine("projectorIPAddress: " + projectorIPAddress);
            myOutput.AppendLine("liftCOMPort:        " + liftCOMPort);
            myOutput.AppendLine("liftMovementTime:   " + liftMovementTime);
            myOutput.AppendLine("==============END OF APPLICATION SETTINGS===============");

            return myOutput.ToString();
        }

        private void sendSerialData(string COMPort, int baudRate, System.IO.Ports.Parity paritySetting, int dataBits, System.IO.Ports.StopBits stopBits, string dataToSend)
        {
            System.IO.Ports.SerialPort liftSerialPort = new System.IO.Ports.SerialPort(COMPort, baudRate, paritySetting, dataBits, stopBits);

            try
            {
                liftSerialPort.Open();
                liftSerialPort.WriteLine(dataToSend);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                liftSerialPort.Close();
            }
        }


        /// <summary>
        /// cancels the Projector Lift Movement
        /// </summary>
        /// <returns></returns>
        public string cancelProjectorLiftMove()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Cancel the Current Life Action");
                sendSerialData(liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500037D\r");
                myprogress.AppendLine("Life Action Cancelled");
            }
            catch (Exception e)
            {
                throw new Exception("cancelProjectorLiftMove: Error Cancelling Lift Action: " + e.ToString());
            }
            return myprogress.ToString();
        }

        public async void extendProjectorLift()
        {
            try
            {
                sendSerialData(liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500107B\r");
                await Task.Delay(liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void retractProjectorLift()
        {
            try
            {
                sendSerialData(liftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500127D\r");
                await Task.Delay(liftMovementTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //TODO add XML to return the TV Informtion private static void getTVInfo()
        private void getTVInfo(ref int p_intTVID, ref string p_strTVIP, ref string p_strTVMAC, ref string p_strTVName)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string RegisterTV_Part1(int tvID)
        {
            var myProgress = new StringBuilder();
            try
            {
                myProgress.AppendLine("Attempting to Register the TV");

                CookieContainer allcookies = new CookieContainer();

                string hostname = System.Environment.MachineName;
                string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + " (Mendel's APP)\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

                try
                {
                    string p_tvIP = string.Empty;
                    string p_tvMAC = string.Empty;
                    switch (tvID)
                        {

                        }
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/accessControl");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.Timeout = 500;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsontosend);
                    }

                    try
                    {
                        httpWebRequest.GetResponse();
                    }

                    catch { }
                }

                catch { Console.WriteLine("device not reachable!"); }

                Console.Write("Please enter PIN that is displayed on TV: \n");
            }
            catch (Exception e)
            {
                throw new Exception("RegisterTV: Error Registering the TV: " + e.ToString());
            }
            return myProgress.ToString();

        }
        //TODO: Figrue out how to register the TV without Extra User Interaction OOORRRR Break the function apart
        //private static string RegisterTV()
        //{
        //    StringBuilder myprogress = new StringBuilder();

        //    try
        //    {
        //        myprogress.AppendLine("Attempting to Register the TV");

        //        CookieContainer allcookies = new CookieContainer();

        //        string hostname = System.Environment.MachineName;
        //        string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + " (Mendel's APP)\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";

        //        try
        //        {
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/accessControl");
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";
        //            httpWebRequest.AllowAutoRedirect = true;
        //            httpWebRequest.Timeout = 500;

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                streamWriter.Write(jsontosend);
        //            }

        //            try
        //            {
        //                httpWebRequest.GetResponse();
        //            }

        //            catch { }
        //        }

        //        catch { Console.WriteLine("device not reachable!"); }

        //        Console.Write("Please enter PIN that is displayed on TV: \n");
        //        string pincode = Console.ReadLine();

        //        Console.WriteLine("");
        //        Console.WriteLine("");
        //        Console.WriteLine("Continuing...");
        //        Console.WriteLine("");
        //        Console.WriteLine("");

        //        try
        //        {
        //            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + tv1IPAddress + "/sony/accessControl");
        //            httpWebRequest2.ContentType = "application/json";
        //            httpWebRequest2.Method = "POST";
        //            httpWebRequest2.AllowAutoRedirect = true;
        //            httpWebRequest2.CookieContainer = allcookies;
        //            httpWebRequest2.Timeout = 500;

        //            using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
        //            {
        //                streamWriter.Write(jsontosend);
        //            }

        //            string authInfo = "" + ":" + pincode;
        //            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        //            httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;

        //            var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();


        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var responseText = streamReader.ReadToEnd();
        //                Console.WriteLine("Response Text: " + responseText);
        //            }

        //            //write register cookie to file!
        //            string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + tv1IPAddress + "/sony/appControl")));

        //            // Write the string to a file.
        //            System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
        //            file.WriteLine(answerCookie);
        //            file.Close();

        //            Console.WriteLine("Cookie: " + answerCookie);

        //        }

        //        catch { Console.WriteLine("timeout!"); }

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("RegisterTV: Error Registering the TV: " + e.ToString());
        //    }
        //    return myprogress.ToString();
        //}

        public string PowerOffTV()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Power Off the TV");

                string hostname = System.Environment.MachineName;

                myprogress.AppendLine("Configuring the HTTP Request");
                HttpWebRequest request = (HttpWebRequest)
                HttpWebRequest.Create("http://" + tv1IPAddress + "/sony/IRCC");

                string xmlString = "<?xml version=\"1.0\"?>";
                xmlString += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                xmlString += "<s:Body>";
                xmlString += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
                xmlString += "<IRCCCode>AAAAAQAAAAEAAAAvAw==</IRCCCode>";
                xmlString += "</u:X_SendIRCC>";
                xmlString += "</s:Body>";
                xmlString += "</s:Envelope>";

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] bytesToWrite = encoding.GetBytes(xmlString);

                request.Method = "POST";
                request.ContentLength = bytesToWrite.Length;
                request.Headers.Add("SOAPAction: \"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                request.ContentType = "text/xml; charset=utf-8";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                Stream newStream = request.GetRequestStream();
                newStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                newStream.Close();

                myprogress.AppendLine("Sending the Request");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                myprogress.AppendLine("Checking the Response");
                string responseFromServer = reader.ReadToEnd();
                myprogress.AppendLine("Response from Server: " + responseFromServer);
            }
            catch (Exception e)
            {
                throw new Exception("PowerOffTV: Error Powering Off the TV: " + e.ToString());
            }

            return myprogress.ToString();
        }

        public string WakeupTV()
        {
            StringBuilder myprogress = new StringBuilder();

            try
            {
                myprogress.AppendLine("Attempting to Wakeup the TV");

                Byte[] datagram = new byte[102];

                for (int i = 0; i <= 5; i++)
                {
                    datagram[i] = 0xff;
                }

                myprogress.AppendLine("Checking the MAC Address of the TV");
                string[] macDigits = null;
                if (tv1MACAddress.Contains("-"))
                {
                    macDigits = tv1MACAddress.Split('-');
                }
                else
                {
                    macDigits = tv1MACAddress.Split(':');
                }

                if (macDigits.Length != 6)
                {
                    throw new ArgumentException("Incorrect MAC address supplied!");
                }

                int start = 6;
                for (int i = 0; i < 16; i++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                    }
                }

                myprogress.AppendLine("Sending a Wakeup call to the TV");
                UdpClient client = new UdpClient();
                client.Send(datagram, datagram.Length, "255.255.255.255", 3);
                myprogress.AppendLine("TV Wakeup Sent Successfully");
            }
            catch (Exception e)
            {
                throw new Exception("WakeupTV: Error Waking the TV: " + e.ToString());
            }

            return myprogress.ToString();
        }

        private static string connectToProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            try
            {
                myProgress.Append("Is there a connection to the projector? ");
                if (c == null)
                {
                    myProgress.AppendLine("N");
                    myProgress.AppendLine("Attempting to Connect to the projector");

                    System.Net.IPAddress address = null;

                    myProgress.AppendLine("Checking the IP Address of the Projector");
                    if (System.Net.IPAddress.TryParse(projectorIPAddress, out address))
                    {
                        myProgress.AppendLine("Attempting to Connect to the projector");
                        c = new PJLinkConnection(projectorIPAddress, "JBMIAProjectorLink");
                        myProgress.AppendLine("A Connection has been established!");
                    }
                    else
                    {
                        myProgress.AppendLine("Invalid IP Address Entered");
                    }
                }
                else
                {
                    myProgress.AppendLine("Y");
                }
            }
            catch (Exception e)
            {
                throw new Exception("ConnectToProjector: Error Connecting to OR getting the status of the projector: " + e.ToString());
            }
            return myProgress.ToString(); 
        }

        public string getProjectorStatus()
        {
            if (c != null)
                return c.powerQuery().ToString();
            else
                return "No Status Available";
        }

        public string printProjectorStatus()
        {
            StringBuilder myProgress = new StringBuilder();

            try
            {
                myProgress.AppendLine("Attempting to Print the Projector Status");

                LampStatusCommand lamp = new LampStatusCommand();
                string status = lamp.getStatusOfLamp(1).ToString();

                myProgress.Append(connectToProjector());

                string power = c.powerQuery().ToString();
                ProjectorInfo pi = new ProjectorInfo();


                myProgress.AppendLine("Connection Status: Connected");
                myProgress.AppendLine("");
                myProgress.AppendLine("Power Status: " + power);
                myProgress.AppendLine("Fan Status: " + pi.FanStatus);
                myProgress.AppendLine("Lamp Status: " + pi.LampStatus);
                myProgress.AppendLine("Current Source Input: " + pi.Input);
                myProgress.AppendLine("Cover Status: " + pi.CoverStatus);
                myProgress.AppendLine("Filter Status: " + pi.FilterStatus);
                myProgress.AppendLine("Current Lamp Status: " + status);
                myProgress.AppendLine("");
            }
            catch (Exception e)
            {
                throw new Exception("printProjectorStatus: Error Printing the Projector Status: " + e.ToString()); 
            }

            return myProgress.ToString();
        }

        public string turnOnProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            myProgress.AppendLine("Attempting to turn the projector On");

            try
            {
                myProgress.Append(connectToProjector());

                //TODO:create a message bus so that these messages can be placed back in the functions
                myProgress.AppendLine("Turning Projector On");
                c.turnOn();
                myProgress.AppendLine("Projector is now:" + c.powerQuery().ToString());
            }
            catch (Exception e)
            {
                throw new Exception("turnOnProjector: Error Turning the Projector On: " + e.ToString());
            }
            
            return myProgress.ToString();
        }

        public string turnOffProjector()
        {
            StringBuilder myProgress = new StringBuilder();
            myProgress.AppendLine("Attempting to turn the Projector Off......");
            try
            {
                myProgress.Append(connectToProjector());
                
                //TODO:create a message bus so that these messages can be displayed real time
                myProgress.AppendLine("Turning Projector Off");
                c.turnOff();
                myProgress.AppendLine("Projector is now:" + c.powerQuery().ToString());
            }
            catch (Exception e)
            {
                throw new Exception("turnOffProjector: Error Turning off the Projector: " + e.ToString());
            }

            return myProgress.ToString();
        }

        public string change_Input(rv.InputCommand.InputType x, int i, string inputName)
        {
            StringBuilder myProgress = new StringBuilder();

            try
            {
                myProgress.AppendLine("Attempting to Change the Projector Input to " + inputName);

                InputCommand ic2 = new InputCommand(x, i);

                if (c.sendCommand(ic2) == Command.Response.SUCCESS)
                    myProgress.AppendLine(ic2.dumpToString());
                else
                    myProgress.AppendLine("Communication Error");
            }
            catch (Exception e)
            {
                throw new Exception("change_Input: Error Changing the Projector Input to " + inputName + ": " + e.ToString());
            }
            return myProgress.ToString();
        }

        public string changeProjectorToHDMI()
        {
            return change_Input(InputCommand.InputType.DIGITAL, 1, "HDMI");
        }

        public string changeProjectorToVGA()
        {
            return change_Input(InputCommand.InputType.RGB, 1, "VGA");
        }

        public string checkProjectorPowerStatus()
        {
            //StringBuilder myProgress = new StringBuilder();
            //try
            //{
            //    myProgress.AppendLine("Attempting to Check the Projector Power Status");
            //    myProgress.Append(connectToProjector());
            //    myProgress.AppendLine("");
            //    myProgress.AppendLine("Current Projector Power Status:");
            //    myProgress.AppendLine(c.powerQuery().ToString());
            //    myProgress.AppendLine("");
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("checkProjectorPowerStatus: Error Checking the projector power status: " + e.ToString());
            //}
            
            //return myProgress.ToString();
            connectToProjector();
            return c.powerQuery().ToString();
        }
    }
}
