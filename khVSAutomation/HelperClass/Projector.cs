using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using rv;

namespace khVSAutomation
{
    public enum projectorConnectionStatus
    {
        None,
        Connected,
        Error
    }

    class Projector
    {
        public string projectorName { get; set; }
        public string projectorIP { get; set; }
        public string projectorLiftAssociation { get; set; }

        private PJLinkConnection m_objProjectorConnection;

        private AutomationsEntities myDB; 
        private Logger m_objLogger;
        public Projector(string p_strName, string p_strIP, string p_strLiftAssociation, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            projectorName = p_strName;
            projectorIP = p_strIP;
            projectorLiftAssociation = p_strLiftAssociation;
            m_objProjectorConnection = null;

            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession,  p_objLogLevel, "Projector()");
        }
        
        #region Basic Projector Functions
        
        private projectorConnectionStatus connectToProjector()
        {
            projectorConnectionStatus l_objConnectionStatus = projectorConnectionStatus.None;
            var l_strFunctionName = "connectToProjector()";
            var l_objStatus = actionStatus.None;
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: {1}: Is there already a connection to the projector? ", l_strFunctionName, projectorName), l_objStatus, false);
                if (m_objProjectorConnection == null)
                {
                    m_objLogger.logToMemory("N", l_objStatus);
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Connect to the projector", l_strFunctionName, projectorName), l_objStatus);

                    System.Net.IPAddress l_objAddress = null;

                    m_objLogger.logToMemory(string.Format("{0}: {1}: Checking the IP Address of the Projector", l_strFunctionName, projectorName), l_objStatus);
                    if (System.Net.IPAddress.TryParse(projectorIP, out l_objAddress))
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Connect to the projector", l_strFunctionName, projectorName), l_objStatus);

                        m_objProjectorConnection = new PJLinkConnection(projectorIP, "JBMIAProjectorLink");

                        l_objStatus = actionStatus.Success;
                        m_objLogger.logToMemory(string.Format("{0}: {1}: A Connection has been established!", l_strFunctionName, projectorName), l_objStatus);

                        l_objConnectionStatus = projectorConnectionStatus.Connected;
                    }
                    else
                    {
                        throw new Exception("Invalid IP Address Entered");
                    }
                }
                else
                {
                    l_objStatus = actionStatus.Success;
                    m_objLogger.logToMemory("Y", l_objStatus);
                    l_objConnectionStatus = projectorConnectionStatus.Connected;
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: Error Connecting to OR getting the status of the projector: {2}", l_strFunctionName, projectorName, e.ToString()), l_objStatus, true, true, l_strFunctionName);
                l_objConnectionStatus = projectorConnectionStatus.Error;
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
            return l_objConnectionStatus; 
        }

         public string getProjectorStatus()
         {
             if (m_objProjectorConnection != null)
                 return m_objProjectorConnection.powerQuery().ToString();
             else
                 return "No Status Available";
         }

         public string printProjectorStatus()
         {
             StringBuilder p_objProgress = new StringBuilder();
             var l_strFunctionName = "printProjectorStatus()";
             try
             {
                 if (m_objProjectorConnection == null) connectToProjector(); //Connect now if not connected
                 p_objProgress.AppendLine("Attempting to Print the Projector Status");

                 LampStatusCommand l_objLamp = new LampStatusCommand();
                 string l_strLampStatus = l_objLamp.getStatusOfLamp(1).ToString();

                 var l_objConnectionStatus = connectToProjector();

                 string l_strPower = m_objProjectorConnection.powerQuery().ToString();
                 ProjectorInfo l_objProjectorInfo = new ProjectorInfo();

                 p_objProgress.AppendLine("Connection Status: Connected");
                 p_objProgress.AppendLine("");
                 p_objProgress.AppendLine("Power Status: " + l_strPower);
                 p_objProgress.AppendLine("Fan Status: " + l_objProjectorInfo.FanStatus);
                 p_objProgress.AppendLine("Lamp Status: " + l_objProjectorInfo.LampStatus);
                 p_objProgress.AppendLine("Current Source Input: " + l_objProjectorInfo.Input);
                 p_objProgress.AppendLine("Cover Status: " + l_objProjectorInfo.CoverStatus);
                 p_objProgress.AppendLine("Filter Status: " + l_objProjectorInfo.FilterStatus);
                 p_objProgress.AppendLine("Current Lamp Status: " + l_strLampStatus);
                 p_objProgress.AppendLine("");
             }
             catch (Exception e)
             {
                 //Writes the error to the DB then throws the error up.
                 throw new Exception(m_objLogger.logToDB(string.Format("{1}: {2}: Error Printing the Projector Status: {2}.", l_strFunctionName, projectorName, e.ToString()), actionStatus.Error, true, p_strFunctionName: "printProjectorStatus()"));
             }
             finally { m_objLogger.writePendingToDB(p_strFunctionName: l_strFunctionName); }

             return p_objProgress.ToString();
         }

         public actionStatus turnOnProjector()
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "turnOnProjector()"; 
             m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to turn the projector On", l_strFunctionName, projectorName));

             try
             {
                 if (m_objProjectorConnection == null) connectToProjector(); //Connect now if not connected
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Turning Projector On", l_strFunctionName, projectorName));
                 m_objProjectorConnection.turnOn();
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Projector is now: {2}", l_strFunctionName, projectorName, m_objProjectorConnection.powerQuery().ToString()));
                 l_objStatus = actionStatus.Success;
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToDB(string.Format("{0}: {1}: Error Turning the Projector On: {2}", l_strFunctionName, projectorName, e.ToString()), l_objStatus);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }


             return l_objStatus;
         }

         public actionStatus turnOffProjector()
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "turnOffProjector()"; 

             m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to turn the Projector Off......", l_strFunctionName, projectorName));
             try
             {
                 if (m_objProjectorConnection == null) connectToProjector(); //Connect now if not connected

                 //TODO:create a message bus so that these messages can be displayed real time
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Turning Projector Off", l_strFunctionName, projectorName));
                 m_objProjectorConnection.turnOff();
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Projector is now: {2}", l_strFunctionName, projectorName, m_objProjectorConnection.powerQuery().ToString()));
                 l_objStatus = actionStatus.Success;
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Error Turning Off the Projector: {2}", l_strFunctionName, projectorName, e.ToString()), l_objStatus);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }

         private actionStatus change_Input(rv.InputCommand.InputType p_objInputType, int p_intPort, string p_strInputName)
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "change_Input()"; 

             try
             {
                 if (m_objProjectorConnection == null) connectToProjector(); //Connect now if not connected
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Change the Projector Input to {2}", l_strFunctionName, projectorName, p_strInputName));

                 InputCommand l_objInputCommand = new InputCommand(p_objInputType, p_intPort);

                 if (m_objProjectorConnection.sendCommand(l_objInputCommand) == Command.Response.SUCCESS)
                 {
                     l_objStatus = actionStatus.Success;
                     m_objLogger.logToMemory(l_objInputCommand.dumpToString());
                 }
                 else
                 {
                     throw new Exception("Communication Error");
                 }
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Error Changing the Projector Input to {2}. Error: {3}", l_strFunctionName, projectorName, p_strInputName, e.ToString()), l_objStatus);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }

         public actionStatus changeProjectorToHDMI()
         {
             return change_Input(InputCommand.InputType.DIGITAL, 1, "HDMI");
         }

         public actionStatus changeProjectorToBlank()
         {
             return change_Input(InputCommand.InputType.UNKNOWN, 1, "BLANK");
         }

         public actionStatus changeProjectorToVGA()
         {
             return change_Input(InputCommand.InputType.RGB, 1, "VGA");
         }

         public string checkProjectorPowerStatus()
         {
             var l_strFunctionName = "checkProjectorPowerStatus()"; 
             try
             {
                 connectToProjector();
                 m_objLogger.logToMemory(string.Format("{0}: {1}: The Power Status of the Projector is: {2}", l_strFunctionName, projectorName, m_objProjectorConnection.powerQuery().ToString()));
                 return m_objProjectorConnection.powerQuery().ToString();
             }
             catch (Exception e) { throw e; }
             finally { m_objLogger.writePendingToDB(actionStatus.None, p_strFunctionName: l_strFunctionName); }
         }

         public List<string> getCommandNames()
         {
             List<string> l_lstrCommands = new List<string>();
             //TODO: Add Safer Logic so that a list is not provided if the projector is not available.
             l_lstrCommands.Add("Power On");
             l_lstrCommands.Add("Power Off");
             l_lstrCommands.Add("HDMI");
             l_lstrCommands.Add("VGA");
             //l_lstrCommands.Add("BLANK"); 
             return l_lstrCommands;
         }

         /// <summary>
         /// Finds the value of the Command Name passed in and then sends the command value to the Projector to execute.
         /// </summary>
         /// <param name="p_strCommandName"></param>
         /// <returns></returns>
         public actionStatus SendCommandByName(string p_strCommandName)
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "SendCommandByName()";
             try
             {
                 //Determine if this is aspecial functionality command
                 switch (p_strCommandName)
                 {
                     case "HDMI":
                         m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, projectorName, "changeProjectorToHDMI()", p_strCommandName));
                         l_objStatus = changeProjectorToHDMI();
                         m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, projectorName, "changeProjectorToHDMI()", l_objStatus), l_objStatus);
                         break;
                     case "VGA":
                         m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, projectorName, "changeProjectorToVGA()", p_strCommandName));
                         l_objStatus = changeProjectorToVGA();
                         m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, projectorName, "changeProjectorToVGA()", l_objStatus), l_objStatus);
                         break;
                     case "BLANK":
                         m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, projectorName, "changeProjectorToBlank()", p_strCommandName));
                         l_objStatus = changeProjectorToBlank();
                         m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, projectorName, "changeProjectorToBlank()", l_objStatus), l_objStatus);
                         break;
                     case "Power On":
                         m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, projectorName, "turnOnProjector()", p_strCommandName));
                         l_objStatus = turnOnProjector();
                         m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, projectorName, "turnOnProjector()", l_objStatus), l_objStatus);
                         
                         break;
                     case "Power Off":
                         m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to call {2} based on receiving the following command name: {3}.", l_strFunctionName, projectorName, "turnOffProjector()", p_strCommandName));
                         l_objStatus = turnOffProjector();
                         m_objLogger.logToMemory(string.Format("{0}: {1}: {2} completed with a status of : {3}.", l_strFunctionName, projectorName, "turnOffProjector()", l_objStatus), l_objStatus);
                         break;
                     default:
                         throw new Exception(string.Format("{0}: {1}: {2} Command NOT Found.", l_strFunctionName, projectorName, p_strCommandName));
                 }
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command By Name: {2}", l_strFunctionName, projectorName, e.ToString()), l_objStatus);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }

         public actionStatus SendCommandByValue(string p_strCommand)
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "SendCommandByValue()";

             try
             {
                 //The Projector Commands are property specific and therefore may not be able to be generalized at this time.
                 throw new Exception("This functionality is not implemented at this time.");
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToMemory(string.Format("{0}: {1}: Error Sending Command to the Projector Lift: {2}.", l_strFunctionName, projectorName, e.ToString()), l_objStatus);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }



         public string GetDeviceInfo(bool l_blnBasic = false)
         {
             StringBuilder l_objDeviceInfo = new StringBuilder();
             l_objDeviceInfo.AppendLine("  Name:             " + projectorName);

             if (!l_blnBasic)
             {
                 l_objDeviceInfo.AppendLine("  IP Address:       " + projectorIP);
                 l_objDeviceInfo.AppendLine("  Lift Association: " + projectorLiftAssociation);
             }

             return l_objDeviceInfo.ToString();
         }

         public bool isDeviceConfigured()
         {
             return projectorIP.Trim().Length > 0;
         }

        #endregion
    }
}
