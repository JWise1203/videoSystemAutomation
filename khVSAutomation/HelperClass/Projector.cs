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

        private static AutomationsEntities myDB; 
        private static Logger m_objLogger;
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
                m_objLogger.logToMemory("Is there already a connection to the projector? ", l_objStatus, false);
                if (m_objProjectorConnection == null)
                {
                    m_objLogger.logToMemory("N", l_objStatus);
                    m_objLogger.logToMemory("Attempting to Connect to the projector", l_objStatus);

                    System.Net.IPAddress l_objAddress = null;

                    m_objLogger.logToMemory("Checking the IP Address of the Projector", l_objStatus);
                    if (System.Net.IPAddress.TryParse(projectorIP, out l_objAddress))
                    {
                        m_objLogger.logToMemory("Attempting to Connect to the projector", l_objStatus);

                        m_objProjectorConnection = new PJLinkConnection(projectorIP, "JBMIAProjectorLink");

                        l_objStatus = actionStatus.Success;
                        m_objLogger.logToMemory("A Connection has been established!", l_objStatus);

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
                m_objLogger.logToMemory("Error Connecting to OR getting the status of the projector: " + e.ToString(), l_objStatus, true, true, l_strFunctionName);
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

             try
             {
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
                 throw new Exception(m_objLogger.logToDB("Error Printing the Projector Status: " + e.ToString(), actionStatus.Error, true, p_strFunctionName: "printProjectorStatus()"));
             }

             return p_objProgress.ToString();
         }

         public actionStatus turnOnProjector()
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "turnOnProjector()"; 
             m_objLogger.logToMemory("Attempting to turn the projector On");

             try
             {
                 connectToProjector();

                 //TODO:create a message bus so that these messages can be placed back in the functions
                 m_objLogger.logToMemory("Turning Projector On");
                 m_objProjectorConnection.turnOn();
                 m_objLogger.logToMemory("Projector is now:" + m_objProjectorConnection.powerQuery().ToString());
                 l_objStatus = actionStatus.Success;
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToDB("Error Turning the Projector On: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }


             return l_objStatus;
         }

         public actionStatus turnOffProjector()
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "turnOffProjector()"; 

             m_objLogger.logToMemory("Attempting to turn the Projector Off......");
             try
             {
                 connectToProjector();

                 //TODO:create a message bus so that these messages can be displayed real time
                 m_objLogger.logToMemory("Turning Projector Off");
                 m_objProjectorConnection.turnOff();
                 m_objLogger.logToMemory("Projector is now:" + m_objProjectorConnection.powerQuery().ToString());
                 l_objStatus = actionStatus.Success;
             }
             catch (Exception e)
             {
                 l_objStatus = actionStatus.Error;
                 m_objLogger.logToDB("Error Turning Off the Projector: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }

         public actionStatus change_Input(rv.InputCommand.InputType p_objInputType, int p_intPort, string p_strInputName)
         {
             var l_objStatus = actionStatus.None;
             var l_strFunctionName = "turnOffProjector()"; 

             try
             {
                 m_objLogger.logToMemory("Attempting to Change the Projector Input to " + p_strInputName);

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
                 m_objLogger.logToDB("Error Changing the Projector Input to " + p_strInputName + ": " + e.ToString(), l_objStatus, true, p_strFunctionName: "change_Input()");
             }
             finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

             return l_objStatus;
         }

         public actionStatus changeProjectorToHDMI()
         {
             return change_Input(InputCommand.InputType.DIGITAL, 1, "HDMI");
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
                 m_objLogger.logToMemory("The Power Status of the Projector is: " + m_objProjectorConnection.powerQuery().ToString());
                 return m_objProjectorConnection.powerQuery().ToString();
             }
             catch (Exception e) { throw e; }
             finally { m_objLogger.writePendingToDB(actionStatus.None, p_strFunctionName: l_strFunctionName); }
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
