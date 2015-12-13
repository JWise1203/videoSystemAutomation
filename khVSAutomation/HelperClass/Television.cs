using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using SonyAPILib;
using Nito.AsyncEx;

namespace khVSAutomation
{

    class Television
    {
        enum televisionDataType
        {
            Cookie = 1,
            Command = 2
        }
        public string TelevisionName { get; set; }
        public string TelevisionIPAddress { get; set; }
        public string TelevisionMACAddress { get; set; }
        public bool TelevisionRegistered { get; set; }
        public string TelevisionGeneration { get; set; }
        public List<SonyCommands> TelevisionCommands { get; set; }


        private AutomationsEntities myDB;
        //private static tblTelevision m_objtv;
        private Logger m_objLogger;

        private string m_strHostName = "";

        private static string m_strJSONToSend;
        private string m_strCookieData = "";
        private string m_strCommandList = "";
        private List<tblTVCommandWhiteList> m_objTVCommandWhiteList = null;
        //private static CookieContainer m_objCookieContainer;
        private CookieContainer allcookies = new CookieContainer();

        
        //public Television(string p_strName, string p_strIP, string p_strMAC, string p_strCurrentSession, string p_strCookieData, string p_strCommandList, logLevel p_objLogLevel = logLevel.ErrorOnly, bool isSony = false)
        public Television(tblTelevision p_objtv, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly, bool isSony = false)
        {
            //m_objtv = p_objtv;
            TelevisionRegistered = false;
            TelevisionName = p_objtv.Name;
            TelevisionIPAddress = p_objtv.IPAddress;
            TelevisionMACAddress = p_objtv.MACAddress;
            TelevisionGeneration = "3"; //Currently operations are coded for Generation 3 TVs.
            m_strCookieData = string.IsNullOrWhiteSpace(p_objtv.CookieData) ? "" : p_objtv.CookieData;
            m_strCommandList = string.IsNullOrWhiteSpace(p_objtv.CommandList) ? "" : p_objtv.CommandList;

            m_strHostName = System.Environment.MachineName; //System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
            allcookies = new CookieContainer();
            m_strJSONToSend = "";

            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession,  p_objLogLevel, "Television()");

            //Check this televisions Registration
            m_objLogger.logToMemory(string.Format("{0}: {1}: New Television Object Created. Calling CheckReg() to Check the Registration.....", "New Television()", TelevisionName));
            var l_objStatus = checkReg();
            m_objLogger.logToMemory(string.Format("{0}: {1}: Registration Check Completed with a status of {2}", "New Television()", "New Television()", TelevisionName, l_objStatus.ToString()), l_objStatus);

            m_objTVCommandWhiteList = (from comm in myDB.tblTVCommandWhiteLists
                                           orderby comm.CommandID
                                           select comm).ToList();
            m_objLogger.logToMemory(string.Format("{0}: Television Command White List Count: {1}", "New Television()", m_objTVCommandWhiteList.Count()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvID">integer representation of the television to register (1,2,3,4,5.....)</param>
        /// <param name="p_strJSONToSend">Send Empty String, this will be assigned during this function. The Value assigned to this variable is needed for part2</param>
        /// <param name="p_objCookieContainer">Send empty object. Will be assigned during this function. The Value assigned to this variable is needed for part2</param>
        /// <param name="p_blnContinuePart2">flag to indicate whether part 2 can be started</param>
        /// <returns></returns>
        public actionStatus registerTV_Step1(ref bool p_blnPinRequired)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "registerTV_Step1()";
            p_blnPinRequired = true;
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Register the TV.", l_strFunctionName, TelevisionName));
                allcookies = new CookieContainer();

                //Removed Reference to '(Mendel's APP)'
                //m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";
                m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + "\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";

                var l_objHttpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + TelevisionIPAddress + "/sony/accessControl");
                l_objHttpWebRequest.ContentType = "application/json";
                l_objHttpWebRequest.Method = "POST";
                l_objHttpWebRequest.AllowAutoRedirect = true;
                l_objHttpWebRequest.Timeout = 500;

                using (var l_objStreamWriter = new StreamWriter(l_objHttpWebRequest.GetRequestStream()))
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Sending the following request - {2}.", l_strFunctionName, TelevisionName, m_strJSONToSend));
                    l_objStreamWriter.Write(m_strJSONToSend);
                }


                //Attempting to see if the device is already registered, or can be registered with just this one step.
                try 
                {
                    var l_objHttpResponse = (HttpWebResponse)l_objHttpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(l_objHttpResponse.GetResponseStream()))
                    {
                        var l_strResponseText = streamReader.ReadToEnd();
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following Registration Response - {2}.", l_strFunctionName, TelevisionName, l_strResponseText));
                    }
                    updateCookieData(l_strFunctionName, l_objHttpWebRequest);
                    TelevisionRegistered = true;
                }
                catch (Exception e1)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Received the Following Error During Registration: {2}.", l_strFunctionName, TelevisionName, e1.ToString()), actionStatus.Error);
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Gen3 PIN Code Required. Must input PIN.", l_strFunctionName, TelevisionName), actionStatus.Error);
                }

                //Although we may have received an error in assuming that the TV can be registered without a PIN, 
                //we will continue and return the reference to TelevisionRegistered to indicate whether or not Step 2 must be performed.
                l_objStatus = actionStatus.Success;
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Registering - The Device is not reachable. The Error returned was: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);

            }
            finally 
            {
                p_blnPinRequired = !TelevisionRegistered;
                m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); 
            }

            if (TelevisionRegistered)
            {
                //Since we do not need step 2 let's refresh the command list now
                m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to refresh the available commands.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)"));
                var l_objRemoteCommandStatus = actionStatus.None;
                getRemoteCommandList(ref l_objRemoteCommandStatus, true);
                m_objLogger.logToMemory(string.Format("{0}: {1}: The Call to {2} returned a status of {3}.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)", l_objRemoteCommandStatus.ToString()));
            }

            return l_objStatus;
        }

        private void updateCookieData(string p_strFunctionName, HttpWebRequest p_objHttpWebRequest)
        {
            //Note: Exceptions not being caught in this function on purpose: This was intended to be a helper function.
            m_strCookieData = JsonConvert.SerializeObject(p_objHttpWebRequest.CookieContainer.GetCookies(new Uri("http://" + TelevisionIPAddress + "/sony/appControl")));

            m_objLogger.logToMemory(string.Format("{0}: {1}: Retrieved the following cookie from the registration request - {2}. Attempting to update the Database Now.", p_strFunctionName, TelevisionName, m_strCookieData));
            updateTelevisionData(televisionDataType.Cookie, p_strFunctionName, m_strCookieData);

            List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(m_strCookieData);
            allcookies.Add(new Uri(@"http://" + TelevisionIPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
        }

        public actionStatus registerTV_Step2(string p_strPIN, ref bool p_blnRegistrationSuccess)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "registerTV_Step2()";
            //p_blnRegistrationSuccess = false;
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Register the TV - Part 2 (Send PIN Code).", l_strFunctionName, TelevisionName));
                m_objLogger.logToMemory(string.Format("{0}: {1}: Pin Received: {2}.", l_strFunctionName, TelevisionName, p_strPIN));

                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(" http://" + TelevisionIPAddress + "/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 500;

                //Removed Reference to '(Mendel's APP)'
                //m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\",\"function\":\"WOL\"}]]}";
                m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + " (Mendel's APP)\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Sending the following request - {2}.", l_strFunctionName, TelevisionName, m_strJSONToSend));
                    streamWriter.Write(m_strJSONToSend);
                }

                string authInfo = "" + ":" + p_strPIN;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var l_strResponseText = streamReader.ReadToEnd();
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following Registration Response - {2}.", l_strFunctionName, TelevisionName, l_strResponseText));
                }
                updateCookieData(l_strFunctionName, httpWebRequest2);
                p_blnRegistrationSuccess = TelevisionRegistered = true;
                l_objStatus = actionStatus.Success;
            }
            catch (Exception e)
            {
                //TelevisionRegistered = false; - This could have happened in the call to refresh commands
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Registering the TV: Timeout!: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally 
            {
                p_blnRegistrationSuccess = TelevisionRegistered;
                m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); 
            }

            if (TelevisionRegistered)
            {
                //Since we do not need step 2 let's refresh the command list now
                m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to refresh the available commands.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)"));
                var l_objRemoteCommandStatus = actionStatus.None;
                getRemoteCommandList(ref l_objRemoteCommandStatus, true);
                m_objLogger.logToMemory(string.Format("{0}: {1}: The Call to {2} returned a status of {3}.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)", l_objRemoteCommandStatus.ToString()));
            }

            return l_objStatus;
        }

        /// <summary>
        /// not reliable - DO NOT USE AT THIS TIME
        /// </summary>
        /// <returns></returns>
        public actionStatus RegisterTV1Step(ref bool p_blnRegistrationSuccess)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "RegisterTV1Step()";

            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to register the TV in one step.", l_strFunctionName, TelevisionName));
            m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + "\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";
            try
            {
                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                CookieContainer allcookies = new CookieContainer();
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 10000;
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Sending the following request - {2}.", l_strFunctionName, TelevisionName, m_strJSONToSend));
                    streamWriter.Write(m_strJSONToSend);
                }
                string pincode = "0904";
                string authInfo = "" + ":" + pincode;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var l_strResponseText = streamReader.ReadToEnd();
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following Registration Response - {2}.", l_strFunctionName, TelevisionName, l_strResponseText));
                }
                updateCookieData(l_strFunctionName, httpWebRequest2);
                TelevisionRegistered = true;
                l_objStatus = actionStatus.Success;
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Received the Following Error During Registration: {2}.", l_strFunctionName, TelevisionName, e.ToString()), actionStatus.Error);
            }
            finally
            {
                p_blnRegistrationSuccess = TelevisionRegistered;
                m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName);
            }

            if (TelevisionRegistered)
            {
                //Since we do not need step 2 let's refresh the command list now
                m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to refresh the available commands.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)"));
                var l_objRemoteCommandStatus = actionStatus.None;
                getRemoteCommandList(ref l_objRemoteCommandStatus, true);
                m_objLogger.logToMemory(string.Format("{0}: {1}: The Call to {2} returned a status of {3}.", l_strFunctionName, TelevisionName, "getRemoteCommandList(true)", l_objRemoteCommandStatus.ToString()));
            }
            return l_objStatus;
        }

        public actionStatus PowerOffTV()
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "PowerOffTV()";

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Power Off the TV.", l_strFunctionName, TelevisionName));
                var l_strPowerOff = "AAAAAQAAAAEAAAAvAw==";
                l_objStatus = SendCommandByValue(l_strPowerOff);
                
                if (l_objStatus == actionStatus.Error)
                    throw new Exception("Error Returned from SendDirectCommand() - Power Off - was not successful.");
                else if (l_objStatus == actionStatus.PartialError)
                    m_objLogger.logToMemory(string.Format("{0}: {1}: A partial error was returned from the call to send the command. The Command may not have executed as expected.", l_strFunctionName, TelevisionName), l_objStatus);
                else if (l_objStatus == actionStatus.Success)
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Success.", l_strFunctionName, TelevisionName), l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Powering Off the TV: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }
        public List<string> getCommandNames()
        {
            var l_strFunctionName = "getCommandNames()";
            List<string> l_lstrCommands = new List<string>();

            //Dynamically retrieve the available commands for this TV by Name so that they can be dynamically loaded in a UI
            if (TelevisionCommands != null && TelevisionCommands.Count > 0)
            {
                //m_objLogger.logToMemory(string.Format("{0}: {1}: There were {2} commands available. Generating the list of Commands by Name now.", l_strFunctionName, TelevisionName, TelevisionCommands.Count));
                m_objLogger.logToMemory(string.Format("{0}: {1}: There were {2} commands available. Generating a list of available Commands based on the Whitelist results.", l_strFunctionName, TelevisionName, TelevisionCommands.Count));
                //foreach (var l_objCommand in TelevisionCommands) l_lstrCommands.Add(l_objCommand.name);
                foreach (var l_objCommand in TelevisionCommands)
                {
                    foreach (var commWhiteList in m_objTVCommandWhiteList)
                    {
                        if (l_objCommand.name == commWhiteList.Name)
                        {
                            switch (l_objCommand.name)
                            {
                                case "Power":
                                case "PowerOn":
                                case "PowerOff":
                                    //Do Nothing - These are special Operations that WIll be Added Separately;
                                    break;
                                case "HDMI1":
                                case "HDMI2":
                                case "HDMI3":
                                case "HDMI4":
                                    //Do Nothing - These are special Operations that WIll be Added Separately;
                                    break;
                                default:
                                    l_lstrCommands.Add(commWhiteList.DisplayValue);
                                    break;
                            }
                            break;
                        }
                    }
                }

                //Add these commands only if other items were available
                if (l_lstrCommands.Count > 0)
                {
                    l_lstrCommands.Insert(0, "HDMI 4");
                    l_lstrCommands.Insert(0, "HDMI 3");
                    l_lstrCommands.Insert(0, "HDMI 2");
                    l_lstrCommands.Insert(0, "HDMI 1");
                    l_lstrCommands.Insert(0, "Power Off");
                }

                m_objLogger.logToMemory(string.Format("{0}: {1}: List Scrubbed. Returning an edited list with {2} Commands", l_strFunctionName, TelevisionName, l_lstrCommands.Count));
            }
            else
                m_objLogger.logToMemory(string.Format("{0}: {1}: There were no commands available. Returning The Basic Commands.", l_strFunctionName, TelevisionName));

            //Power On command can always be made available since it will be a wake on land command.
            l_lstrCommands.Insert(0, "Power On");
            
            m_objLogger.writePendingToDB(p_strFunctionName: l_strFunctionName);
            return l_lstrCommands;
        }

        /// <summary>
        /// Finds the value of the Command Name passed in and then sends the command value to the TV to execute.
        /// </summary>
        /// <param name="p_strCommandName"></param>
        /// <returns></returns>
        public actionStatus SendCommandByName(string p_strCommandName)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByName()";
            var l_blnCommandLocated = false;
            try
            {
                //Determine if this is aspecial functionality command
                switch(p_strCommandName)
                {
                    case "HDMI 4":
                    case "HDMI 3":
                    case "HDMI 2":
                    case "HDMI 1":
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, TelevisionName, "ExecuteHDMICommand", p_strCommandName));
                        l_objStatus = ExecuteHDMICommand(p_strCommandName);
                        m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, TelevisionName, "ExecuteHDMICommand", l_objStatus), l_objStatus);
                        break;
                    case "Power On":
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, TelevisionName, "WakeupTV()", p_strCommandName));
                        l_objStatus = WakeupTV();
                        m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, TelevisionName, "WakeupTV()", l_objStatus), l_objStatus);
                        break;
                    case "Power Off":
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, TelevisionName, "PowerOffTV()", p_strCommandName));
                        l_objStatus = PowerOffTV();
                        m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, TelevisionName, "PowerOffTV()", l_objStatus), l_objStatus);
                        break;
                    default:
                        //Since we can have pretty-printed command names passed in, let's try locating the command in the whitelist displayname column first
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to locate {2} in the Command WhiteList.", l_strFunctionName, TelevisionName, p_strCommandName));
                        foreach (var whtListComm in m_objTVCommandWhiteList)
                        {
                            if (p_strCommandName == whtListComm.DisplayValue)
                            {
                                m_objLogger.logToMemory(string.Format("{0}: {1}: {2} was found in the whitelist. Switching to the name the TV will recognize: {3}.", l_strFunctionName, TelevisionName, p_strCommandName, whtListComm.Name));
                                p_strCommandName = whtListComm.Name;
                                break;
                            }
                        }


                        if (TelevisionCommands.Count > 0)
                        {
                            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to locate {2} in the available list of Television commands.", l_strFunctionName, TelevisionName, p_strCommandName));
                            foreach (var l_objCommand in TelevisionCommands)
                            {
                                if (l_objCommand.name == p_strCommandName)
                                {
                                    l_blnCommandLocated = true;
                                    m_objLogger.logToMemory(string.Format("{0}: {1}: {2} Located. Attempted to Send {3} to the Television.", l_strFunctionName, TelevisionName, p_strCommandName, l_objCommand.value));
                                    l_objStatus = SendCommandByValue(l_objCommand.value);

                                    if (l_objStatus == actionStatus.Error) 
                                        throw new Exception(string.Format("Attempt to send {0} to the Television Failed.", l_objCommand.value));
                                    break;
                                }
                            }
                        }
                        else
                            throw new Exception("There were no commands available in the Television Command List. Operation Cancelled.");

                        if (!l_blnCommandLocated) 
                            throw new Exception(string.Format("{0} Could not be located. Operation Cancelled.", p_strCommandName));
                        break;
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command By Name: {2}", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        private actionStatus ExecuteHDMICommand(string p_strHDMICommandName)
        {
            var l_objStatus = actionStatus.None;
            switch (p_strHDMICommandName)
            {
                case "HDMI 1":
                case "HDMI1":
                    l_objStatus = SendCommandByValue("AAAAAgAAABoAAABaAw==");
                    break;
                case "HDMI 2":
                case "HDMI2":
                    l_objStatus = SendCommandByValue("AAAAAgAAABoAAABbAw==");
                    break;
                case "HDMI 3":
                case "HDMI3":
                    l_objStatus = SendCommandByValue("AAAAAgAAABoAAABcAw==");
                    break;
                case "HDMI 4":
                case "HDMI4":
                    l_objStatus = SendCommandByValue("AAAAAgAAABoAAABdAw==");
                    break;
            }
            return l_objStatus;
        }

        public actionStatus SendCommandByValue(string p_strCommand)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "SendCommandByValue()";

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Send the following command Directly to the TV: {2}.", l_strFunctionName, TelevisionName, p_strCommand));
                //Abort this function if we are not properly registered!
                m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to check/update registration if needed.", l_strFunctionName, TelevisionName, "checkReg()"));
                var l_objRegistrationStatus = checkReg();
                if (l_objRegistrationStatus != actionStatus.Success)
                    throw new Exception("The Television Registration Check Failed. This Command cannot be executed at this time. Please re-register the TV if this continues.");

                //Check to make sure we have an auth value. If not then Abort this function!
                m_objLogger.logToMemory(string.Format("{0}: {1}: Looking for the auth Value of the registration cookie.", l_strFunctionName, TelevisionName));
                List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(m_strCookieData);
                var l_strAuthValue = bal[0].Value;

                m_objLogger.logToMemory(string.Format("{0}: {1}: Configuring the HTTP Request.", l_strFunctionName, TelevisionName), l_objStatus);
                HttpWebRequest l_objRequest = (HttpWebRequest)WebRequest.Create("http://" + TelevisionIPAddress + "/sony/IRCC");

                string xmlString = "<?xml version=\"1.0\"?>";
                xmlString += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                xmlString += "<s:Body>";
                xmlString += "<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">";
                xmlString += "<IRCCCode>" + p_strCommand + "</IRCCCode>";
                xmlString += "</u:X_SendIRCC>";
                xmlString += "</s:Body>";
                xmlString += "</s:Envelope>";

                ASCIIEncoding l_objEncoding = new ASCIIEncoding();

                byte[] l_abteBytesToWrite = l_objEncoding.GetBytes(xmlString);
                l_objRequest.KeepAlive = true;
                l_objRequest.Method = "POST";
                l_objRequest.ContentLength = l_abteBytesToWrite.Length;
                l_objRequest.ContentType = "text/xml; charset=utf-8";

                l_objRequest.Host = TelevisionIPAddress;
                l_objRequest.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";

                l_objRequest.CookieContainer = new CookieContainer();
                l_objRequest.CookieContainer.Add(new Uri(@"http://" + TelevisionIPAddress + "/sony/"), new Cookie("auth", l_strAuthValue));

                l_objRequest.Headers.Add("SOAPAction: \"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                l_objRequest.Headers.Add("Accept-Encoding", "gzip");

                m_objLogger.logToMemory(string.Format("{0}: {1}: HTTP Request configured. Attempting to Post (send) to the Television.", l_strFunctionName, TelevisionName), l_objStatus);
                System.IO.Stream os = l_objRequest.GetRequestStream();
                // Post data and close connection
                os.Write(l_abteBytesToWrite, 0, l_abteBytesToWrite.Length);
                System.Net.HttpWebResponse resp = l_objRequest.GetResponse() as HttpWebResponse;
                Stream respData = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respData);
                string response = sr.ReadToEnd();
                os.Close();
                sr.Close();
                respData.Close();
                m_objLogger.logToMemory(string.Format("{0}: {1}: HTTP Request sent, and connection closed.", l_strFunctionName, TelevisionName), l_objStatus);
                if (response != "")
                {
                    l_objStatus = actionStatus.Success;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following response from the Television. Marking this operation as a success.", l_strFunctionName, TelevisionName), l_objStatus);
                }
                else
                {
                    l_objStatus = actionStatus.PartialError;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: no response was received - Marking a parial Error. The Command may not have executed as expected.", l_strFunctionName, TelevisionName), l_objStatus);
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command to the TV: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        /// <summary>
        /// During testing it was noticed that the TVs would not power on using the "Power" or "PowerOn" Commands.
        /// Therefore instead of using the specific commands, lets use the generic command to wake on LAN.
        /// </summary>
        /// <returns></returns>
        public actionStatus WakeupTV()
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "WakeupTV()";

            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Wakeup the TV.", l_strFunctionName, TelevisionName), l_objStatus);

                Byte[] l_abteDatagram = new byte[102];

                for (int i = 0; i <= 5; i++)
                    l_abteDatagram[i] = 0xff;

                m_objLogger.logToMemory(string.Format("{0}: {1}: Checking the MAC Address of the TV.", l_strFunctionName, TelevisionName), l_objStatus);
                string[] l_astrMacDigits = null;
                if (TelevisionMACAddress.Contains("-"))
                    l_astrMacDigits = TelevisionMACAddress.Split('-');
                else
                    l_astrMacDigits = TelevisionMACAddress.Split(':');

                if (l_astrMacDigits.Length != 6)
                    throw new ArgumentException("Incorrect MAC address supplied!");

                int l_intStart = 6;
                for (int i = 0; i < 16; i++)
                    for (int x = 0; x < 6; x++)
                        l_abteDatagram[l_intStart + i * 6 + x] = (byte)Convert.ToInt32(l_astrMacDigits[x], 16);

                m_objLogger.logToMemory(string.Format("{0}: {1}: Sending a Wakeup call to the TV.", l_strFunctionName, TelevisionName), l_objStatus);
                UdpClient l_objUDPClient = new UdpClient();
                l_objUDPClient.Send(l_abteDatagram, l_abteDatagram.Length, "255.255.255.255", 3);
                m_objLogger.logToMemory(string.Format("{0}: {1}: TV Wakeup Sent Successfully.", l_strFunctionName, TelevisionName), l_objStatus);
                m_objLogger.logToMemory("TV Wakeup Sent Successfully", l_objStatus);
                l_objStatus = actionStatus.Success;

                try
                {
                    var l_objRegistrationStatus = actionStatus.None;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: TV On. Now Attempting to check/update the registration.", l_strFunctionName, TelevisionName), l_objRegistrationStatus);
                    l_objRegistrationStatus = checkReg();
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Registration Check Completed with a status of {2}.", l_strFunctionName, TelevisionName, l_objRegistrationStatus), l_objRegistrationStatus);
                }
                catch (Exception e)
                { 
                    //DOING NOTHING FOR NOW - THIS MAY BE TOO EARLY FOR THIS KIND OF OPERATION
                    m_objLogger.logToMemory(string.Format("{0}: {1}: There was an error trying to check the Registration after waking the TV {2}. Since the Wake Operation was successful we will NOT Abort operations at this time.", l_strFunctionName, TelevisionName, e.ToString()), actionStatus.Error);
                }
                
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Waking the TV: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }

        public string GetDeviceInfo(bool l_blnBasic = false)
        {
            StringBuilder l_objDeviceInfo = new StringBuilder();
            l_objDeviceInfo.AppendLine("  Name:        " + TelevisionName);

            if (!l_blnBasic)
            {
                l_objDeviceInfo.AppendLine("  IP Address:  " + TelevisionIPAddress);
                l_objDeviceInfo.AppendLine("  MAC Address: " + TelevisionMACAddress);
                l_objDeviceInfo.AppendLine("  Registered:" + TelevisionRegistered);
            }

            return l_objDeviceInfo.ToString();
        }

        public bool isDeviceConfigured()
        {
            return ((TelevisionIPAddress.Trim().Length > 0) && (TelevisionMACAddress.Trim().Length > 0));
        }

        //Logger Template
        //m_objLogger.logToMemory(string.Format("{0}: ", l_strFunctionName));

        private actionStatus checkReg()
        {
            var l_strFunctionName = "checkReg()";
            var l_objStatus = actionStatus.None;
            TelevisionRegistered = false;
            m_objLogger.logToMemory(string.Format("{0}: {1}: Checking Registration.", l_strFunctionName, TelevisionName));

            try
            {
                if (m_strCookieData == "")
                {
                    //We Need to Register the TVs the hard way
                    m_objLogger.logToMemory(string.Format("{0}: {1}: CookieData not found. The TV Must be registered before other commands can be executed. Returning a registration value of 'false'.", l_strFunctionName, TelevisionName));
                }
                else
                {
                    //Let's check the registration status
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(m_strCookieData);
                    DateTime CT = DateTime.Now;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Checking if Cookie has Expired. The Expiration Date on hand is: {2}, and the Current Date and Time is: {3}.", l_strFunctionName, TelevisionName, bal[0].Expires, CT.ToString()));

                    if (CT > Convert.ToDateTime(bal[0].Expires))
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: The Cookie has Expired! Attempting to Retrieve a NEW Cookie.", l_strFunctionName, TelevisionName));

                        m_strJSONToSend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + m_strHostName + "\"},[{\"clientid\":\"" + m_strHostName + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + m_strHostName + "\",\"function\":\"WOL\"}]]}";
                        try
                        {
                            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/accessControl");
                            httpWebRequest2.ContentType = "application/json";
                            httpWebRequest2.Method = "POST";
                            httpWebRequest2.AllowAutoRedirect = true;
                            httpWebRequest2.CookieContainer = allcookies;
                            httpWebRequest2.Timeout = 500;
                            using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                            {
                                m_objLogger.logToMemory(string.Format("{0}: {1}: Sending the following request to retrieve a new cookie: {2}.", l_strFunctionName, TelevisionName, m_strJSONToSend));
                                streamWriter.Write(m_strJSONToSend);
                            }
                            var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var l_strResponseText = streamReader.ReadToEnd();
                                m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following response: {2}.", l_strFunctionName, TelevisionName, l_strResponseText));
                            }
                            updateCookieData(l_strFunctionName, httpWebRequest2);
                            TelevisionRegistered = true;
                            l_objStatus = actionStatus.Success;
                        }
                        catch (Exception e)
                        {
                            l_objStatus = actionStatus.Error;
                            m_objLogger.logToMemory(string.Format("{0}: {1}: Failed to retrieve a new cookie. Error returned: {2}", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
                            TelevisionRegistered = false;
                        }
                    }
                    else
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: The Stored Cookie is still good.", l_strFunctionName, TelevisionName));
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Adding Cookie to the Device.", l_strFunctionName, TelevisionName));
                        //_Log.writetolog(bal[0].Name + ": Adding Cookie to Device: " + bal[0].Value, false);
                        allcookies.Add(new Uri(@"http://" + TelevisionIPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                        TelevisionRegistered = true;
                        l_objStatus = actionStatus.Success;
                    }
                }

                //Load the commands that we already have available
                if (TelevisionRegistered) loadRemoteCommandList(m_strCommandList);
            }
            catch (Exception e)
            { 
                TelevisionRegistered = false;
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: CheckReg() failed with the following error: ", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
            return l_objStatus;
        }

        private bool updateTelevisionData(televisionDataType p_objDataType, string p_strUpdatedBy, string p_strNewData = "")
        {
            var l_blnSuccess = false;
            try
            {
                tblTelevision t = myDB.tblTelevisions
                                      .First(i => i.IPAddress == TelevisionIPAddress);

                var l_objNewData = "";
                if(p_objDataType == televisionDataType.Cookie)
                {
                    l_objNewData = p_strNewData == "" ? m_strCookieData : p_strNewData;
                    t.CookieData = l_objNewData;
                }
                else if(p_objDataType == televisionDataType.Command)
                {
                    l_objNewData = p_strNewData == "" ? m_strCommandList : p_strNewData;
                    t.CommandList = l_objNewData;
                }
                else { return false; }

                if (!string.IsNullOrEmpty(l_objNewData))
                {
                    t.UpdatedBy = p_strUpdatedBy;
                    t.UpdatedDateTime = DateTime.UtcNow;
                    
                    //For Now we will run these syncronously
                    AsyncContext.Run(() => myDB.SaveChangesAsync());
                }
                l_blnSuccess = true;
            }
            catch (Exception e)
            {
                //We want to log it at this point, but let's continue since we will still have what we need in memory
                m_objLogger.logToMemory("Issue Saving  Cookie to DB: " + e.ToString(), actionStatus.Error);
                l_blnSuccess = false;
            }
            return l_blnSuccess;
        }

        #region get remote command list
        /// <summary>
        /// This method will retrieve Gen1 and Gen2 XML IRCC Command List or Gen3 JSON Command List.
        /// </summary>
        /// <returns>Returns a string containing the contents of the returned XML Command List for your Use</returns>
        /// <remarks>This method will also populate the SonyDevice.Commands object list with the retrieved command list</remarks>
        private string getRemoteCommandList(ref actionStatus p_objStatus, bool p_blnSaveCommandListToDB = false, bool p_blnLoadCommandList = true)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "getRemoteCommandList()";
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Retrieving Generation {2} Remote Command List", l_strFunctionName, TelevisionName, TelevisionGeneration));
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + TelevisionIPAddress + @"/sony/system");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                m_strJSONToSend = "{\"id\":20,\"method\":\"getRemoteControllerInfo\",\"version\":\"1.0\",\"params\":[]}";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Sending the following Request - {2}.", l_strFunctionName, TelevisionName, m_strJSONToSend));
                    streamWriter.Write(m_strJSONToSend);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var l_strResponseText = streamReader.ReadToEnd();
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Received the following response - {2}.", l_strFunctionName, TelevisionName, l_strResponseText));

                    if (l_strResponseText != "")
                    {
                        m_strCommandList = l_strResponseText;
                        l_objStatus = actionStatus.Success;
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Retrieve Command List - Successful", l_strFunctionName, TelevisionName));
                    }
                    else
                    {
                        throw new Exception(string.Format("{0}: {1}: Retrieve Command List returned no commands. Aborting Operation without Updating Commands", l_strFunctionName, TelevisionName));
                    }
                }

                //We will not get to this point if there was an issue......
                if (p_blnSaveCommandListToDB && m_strCommandList.Length > 0)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to update the DB with the latest command list - {3}.", l_strFunctionName, TelevisionName, "updateTelevisionData()", m_strCommandList));
                    updateTelevisionData(televisionDataType.Command, l_strFunctionName, m_strCommandList);
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Getting the Remote Command List: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally 
            {
                p_objStatus = l_objStatus;
            }

            //Placed here because we want to load whatever we have, even if it is old.
            //Note: Need to check the length of the commandlist to ensure that we do not accidentally place ourselves in a loop.
            if (p_blnLoadCommandList && m_strCommandList.Length > 0)
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2} to Load the current Command List - {3}.", l_strFunctionName, TelevisionName, "loadRemoteCommandList();", m_strCommandList));
                loadRemoteCommandList(m_strCommandList, false);
            }

            m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName);
            return m_strCommandList;
        }

        private actionStatus loadRemoteCommandList(string p_strCommandList, bool p_blnCanCallGetRemoteCommandList = true)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "loadRemoteCommandList";
            var l_strCommandList = "";
            try
            {
                if (p_strCommandList.Length > 0)
                {
                    l_strCommandList = p_strCommandList;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: We will be attempting to load the command list that was passed in: {2}.", l_strFunctionName, TelevisionName, l_strCommandList));
                }
                else if (p_strCommandList.Length == 0 && p_blnCanCallGetRemoteCommandList)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: The Command List Passed in was empty. Attempting to get a new one.......", l_strFunctionName, TelevisionName));
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Calling {2}.", l_strFunctionName, TelevisionName, "getRemoteCommandList()"));
                    var l_objCommandList_Get_Status = actionStatus.None;
                    l_strCommandList = getRemoteCommandList(ref l_objCommandList_Get_Status, true, false);

                    if (l_objCommandList_Get_Status == actionStatus.Error)
                        throw new Exception("call to getRemoteCommandList() Failed.");
                    else
                        m_objLogger.logToMemory(string.Format("{0}: {1}: We will be attempting to load the command list that we just retrieved: {2}.", l_strFunctionName, TelevisionName, l_strCommandList));
                }

                if (l_strCommandList.Length > 0)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Loading Commands Now.", l_strFunctionName, TelevisionName));
                    SonyCommandList dataSet = new SonyCommandList();
                    dataSet = JsonConvert.DeserializeObject<SonyCommandList>(l_strCommandList);
                    string first = dataSet.result[1].ToString();
                    List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
                    TelevisionCommands = bal;
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Completed Loading Commands. There were a total of {2} commands loaded.", l_strFunctionName, TelevisionName, TelevisionCommands.Count));
                }
                else
                {
                    throw new Exception("There was not command list found to load");
                }
            }
            catch (Exception e)
            { 
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Loading the Command List: {2}.", l_strFunctionName, TelevisionName, e.ToString()), l_objStatus);
            }
            finally
            {
                m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName);
            }

            return l_objStatus;
        }

        #endregion
    }


    #region Sony Cookie
    /// <summary>
    /// Gets or Sets the Sony Device Cookie Container Object
    /// </summary>
    [Serializable]
    class SonyCookie
    {
        /// <summary>
        /// Gets or Sets the Cookie Comment
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Comment URI
        /// </summary>
        public object CommentUri { get; set; }
        /// <summary>
        /// Gets or Sets Cookie for HTTP Only
        /// </summary>
        public bool HttpOnly { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Discard
        /// </summary>
        public bool Discard { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Domain
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Expired
        /// </summary>
        public bool Expired { get; set; }
        /// <summary>
        /// Gets or Sets the Cookies Expiration
        /// </summary>
        public string Expires { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or Sets the Is Cookie Secure
        /// </summary>
        public bool Secure { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Time Stamp
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Version
        /// </summary>
        public int Version { get; set; }
    }
    #endregion

    #region Sony Command List
    /// <summary>
    /// Gets or Sets the Sony Command List Object
    /// </summary>
    [Serializable]
    class SonyCommandList
    {
        /// <summary>
        /// Gets or Sets the Devices Command List ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Gets or Sets the Devices Command List Results
        /// </summary>
        public List<object> result { get; set; }
    }
    #endregion

    #region Sony Command
    /// <summary>
    /// Gets or Sets the Sony Device Command
    /// </summary>
    [Serializable]
    public class SonyCommands
    {
        /// <summary>
        /// Gets or Sets the Devices Command Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Gets or Sets the Devices Command Value
        /// </summary>
        public string value { get; set; }
    }
    #endregion

}
