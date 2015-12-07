using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace khVSAutomation
{
    class ProjectorLift
    {
        public string LiftName { get; set; }
        public string LiftCOMPort { get; set; }
        public int LiftMoveTime { get; set; }

        private const string m_cstrCancel = ">0500037D\r";
        private const string m_cstrExtend = ">0500107B\r";
        private const string m_cstrRetract = ">0500127D\r";

        private static AutomationsEntities myDB;
        private static Logger m_objLogger;

        public ProjectorLift(string p_strName, string p_strCOM, int p_strMoveTime, string p_strCurrentSession, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            LiftName = p_strName;
            LiftCOMPort = p_strCOM;
            LiftMoveTime = p_strMoveTime;
            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, p_strCurrentSession, p_objLogLevel, "ProjectorLift()");
        }

        private void sendSerialData(string p_strCOMPort, int p_intBaudRate, System.IO.Ports.Parity p_objParitySetting, int p_intDataBits, System.IO.Ports.StopBits p_objStopBits, string p_strDataToSend)
        {
            System.IO.Ports.SerialPort l_objLiftSerialPort = new System.IO.Ports.SerialPort(p_strCOMPort, p_intBaudRate, p_objParitySetting, p_intDataBits, p_objStopBits);

            try
            {
                l_objLiftSerialPort.Open();
                l_objLiftSerialPort.WriteLine(p_strDataToSend);
            }
            catch (Exception e) { throw e; }
            finally { l_objLiftSerialPort.Close(); }
        }

        public async Task<actionStatus> doLiftAction(LiftAction p_objLiftAction)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "doLiftAction()";
            var l_blnPerformAwait = false;

            try
            {
                m_objLogger.logToMemory("Called doLiftAction with Action Set to: " + p_objLiftAction, l_objStatus);

                var l_strCurrentAction = "";
                switch (p_objLiftAction)
                {
                    case LiftAction.Cancel: l_strCurrentAction = m_cstrCancel; break;
                    case LiftAction.Extend: l_strCurrentAction = m_cstrExtend; l_blnPerformAwait = true; break;
                    case LiftAction.Retract: l_strCurrentAction = m_cstrRetract; l_blnPerformAwait = true; break;
                    default:
                        throw new Exception("Lift Action: " + p_objLiftAction + "Not Found!");
                }

                m_objLogger.logToMemory("Lift Action Found: Attempting to Send Message through Serial Port " + LiftCOMPort, l_objStatus);

                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, l_strCurrentAction);
                if (l_blnPerformAwait) await Task.Delay(LiftMoveTime);

                l_objStatus = actionStatus.Success;
                m_objLogger.logToMemory("Lift Action Message Sent: " + l_strCurrentAction, l_objStatus);
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToDB("Error Performing Lift Action: " + e.ToString(), l_objStatus, true, p_strFunctionName: l_strFunctionName);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
            return l_objStatus;
        }
/*
        /// <summary>
        /// cancels the Projector Lift Movement
        /// </summary>
        /// <returns></returns>
        public actionStatus cancelProjectorLiftMove(ref Logger p_objLogger, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;
            try
            {
                SharedFunctions.writeProgress("Attempting to Cancel the Current Life Action", l_objStatus);
                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500037D\r");
                SharedFunctions.writeProgress("Lift Action Cancelled", l_objStatus);
                l_objStatus = actionStatus.Success;
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                SharedFunctions.writeProgress("cancelProjectorLiftMove: Error Cancelling Lift Action: " + e.ToString(), l_objStatus);
            }
            finally { p_objLogger.writePendingToDB(l_objStatus); }
            return l_objStatus;
        }

        public async void extendProjectorLift()
        {
            try
            {
                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500107B\r");
                await Task.Delay(LiftMoveTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void retractProjectorLift(int p_intProjID = 1)
        {
            try
            {
                sendSerialData(LiftCOMPort, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, ">0500127D\r");
                await Task.Delay(LiftMoveTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
 */

        public string GetDeviceInfo(bool l_blnBasic = false)
        {
            StringBuilder l_objDeviceInfo = new StringBuilder();
            l_objDeviceInfo.AppendLine("  Name:      " + LiftName);

            if (!l_blnBasic)
            {
                l_objDeviceInfo.AppendLine("  COM Port:  " + LiftCOMPort);
                l_objDeviceInfo.AppendLine("  Move Time: " + LiftMoveTime);
            }

            return l_objDeviceInfo.ToString();
        }

        public bool isDeviceConfigured()
        {
            return ((LiftCOMPort.Trim().Length > 0) && (LiftMoveTime > 0));
        }

    }
}
