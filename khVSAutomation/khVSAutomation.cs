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
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using Nito.AsyncEx;
//using khVSAutomation.HelperClass;
using SonyAPILib;
using rv;

namespace khVSAutomation
{

    public enum DeviceTypes
    {
        None = 0,
        Projector = 1,
        ProjectorLift = 2,
        Television = 3,
        MatrixSwitcher = 4
    }

    public enum actionStatus
    {
        None = 0,
        Success = 2,
        Error = -1,
        PartialError = -2
    }

    public enum logLevel
    {
        None = 0,
        ErrorOnly = 1,
        All = 5
    }

    public enum SwitcherOutput
    {
        Output1 = 1,
        Output2 = 2,
        Output3 = 3,
        Output4 = 4
    }

    public enum SwitcherAction
    {
        Source1 = 1,
        Source2 = 2,
        Source3 = 3,
        Source4 = 4,
        Left = 5,
        Right = 6
    }

    public enum LiftAction
    {
        Cancel = 0,
        Extend = 1,
        Retract = 2
    }

    public class Automation
    {
        //static DateTime StartTime;
        
        //private static PJLinkConnection c = null;
        private static List<Projector> m_objProjectors = null;
        private static List<Television> m_objTelevisions = null;
        private static List<ProjectorLift> m_objProjectorLifts = null;
        private static List<MatrixSwitcher> m_objMatrixSwitchers = null;
        
        //private static string m_strPublicTelevisionInfo;
        //private static string m_strPublicProjectorInfo;
        //private static string m_strPublicSwitcherInfo;

        //DB Object - Allow Save/Load from DB instead of from File
        private static AutomationsEntities myDB = null; 
        private static string m_cstrSessionID = "";
        private static logLevel m_objLogLevel;
        private static Logger m_objLogger = null;
        //private static SonyAPI_Lib m_objSonyAPI = null;

        #region Configuration/Information

        public int NumberOfProjectors { get{ return m_objProjectors.Count(); } }
        public int NumberOfProjectorLifts { get { return m_objProjectorLifts.Count(); } }
        public int NumberOfTelevisions { get { return m_objTelevisions.Count(); } }
        public int NumberOfSwitchers { get { return m_objMatrixSwitchers.Count(); } }

        //Properties Carry General Information to the UI: [ID;Name|ID;Name]
        public List<string> AllTelevisions { get { return m_lstPublicTelevisionInfo; } }
        public List<string> AllProjectors { get { return m_lstPublicProjectorInfo; } }
        public List<string> AllProjectorLifts { get { return m_lstPublicProjectorLiftInfo; } }
        public List<string> AllSwitchers { get { return m_lstPublicSwitcherInfo; } }
        private static List<string> m_lstPublicTelevisionInfo;
        private static List<string> m_lstPublicProjectorInfo;
        private static List<string> m_lstPublicProjectorLiftInfo;
        private static List<string> m_lstPublicSwitcherInfo;

        public Automation(bool p_blnWinForm = false, logLevel p_objLogLevel = logLevel.ErrorOnly)
        {
            loadConfigurations(p_objLogLevel);
        }

        /// <summary>
        /// Load the Configurations from the DB
        /// </summary>
        private static void loadConfigurations(logLevel p_objLogLevel)
        {
            m_cstrSessionID = Guid.NewGuid().ToString();
            m_objLogLevel = p_objLogLevel;
            myDB = new AutomationsEntities();
            m_objLogger = new Logger(ref myDB, m_cstrSessionID, m_objLogLevel);
            actionStatus l_objStatus = actionStatus.None;

            var l_strFunctionName = "loadConfigurations()";
            try
            {
                m_objLogger.logToMemory(string.Format("{0}: New Session Started: {1}.", l_strFunctionName, m_cstrSessionID));
                m_objLogger.logToMemory("Loading Configurations");

                //Initialize the Modular Lists
                m_objTelevisions = new List<Television>();
                m_objProjectors = new List<Projector>();
                m_objProjectorLifts = new List<ProjectorLift>();
                m_objMatrixSwitchers = new List<MatrixSwitcher>();
                m_lstPublicProjectorInfo = new List<string>();
                m_lstPublicProjectorLiftInfo = new List<string>();
                m_lstPublicSwitcherInfo = new List<string>();
                m_lstPublicTelevisionInfo = new List<string>();

                //Get the items from the DB Televisions from the DB and add them to the List
                var l_objTelevisions = (from tv in myDB.tblTelevisions
                                        orderby tv.TelevisionID
                                        select tv).ToList();
                m_objLogger.logToMemory(string.Format("{0}: Television Count: {1}", l_strFunctionName, l_objTelevisions.Count()));

                var l_objProjectors = (from proj in myDB.tblProjectors
                                       orderby proj.ProjectorID
                                       select proj).ToList();
                m_objLogger.logToMemory(string.Format("{0}: Projector Count: {1}", l_strFunctionName, l_objProjectors.Count()));

                var l_objProjectorLifts = (from lifts in myDB.tblProjectorLifts
                                           orderby lifts.ProjectorLiftID
                                           select lifts).ToList();
                m_objLogger.logToMemory(string.Format("{0}: Projector Lift Count: {1}", l_strFunctionName, l_objProjectorLifts.Count()));

                var l_objMatrixSwitchers = (from switchers in myDB.tblMatrixSwitchers
                                           orderby switchers.MatrixSwitcherID
                                           select switchers).ToList();
                m_objLogger.logToMemory(string.Format("{0}: Matrix Switcher Count: {1}", l_strFunctionName, l_objMatrixSwitchers.Count()));

                //TVs
                foreach (var tv in l_objTelevisions)
                {
                    m_objTelevisions.Add(new Television(tv, m_cstrSessionID, m_objLogLevel)); //m_objTelevisions.Add(new Television(tv.Name, tv.IPAddress, tv.MACAddress, m_cstrSessionID, tv.CookieData, tv.CommandList, tv, m_objLogLevel));
                    m_lstPublicTelevisionInfo.Add(string.Format("{0};{1}", tv.Name, tv.TelevisionID));
                }
                
                //Projectors
                foreach (var proj in l_objProjectors)
                {
                    var l_objLiftAssociations = (from relate in myDB.tblReleateProjectorAndLifts
                                            join lifts in myDB.tblProjectorLifts on relate.ProjectorLiftID equals lifts.ProjectorLiftID
                                            where relate.ProjectorID == proj.ProjectorID
                                            select new { ProjectorID = proj.ProjectorID, LiftID = lifts.ProjectorLiftID, LiftAssociation = lifts.Name }).ToList();
                    m_objProjectors.Add(new Projector(proj.Name, proj.IPAddress, (l_objLiftAssociations.Count() >= 1? l_objLiftAssociations[0].LiftAssociation : ""), m_cstrSessionID, m_objLogLevel));
                    m_lstPublicProjectorInfo.Add(string.Format("{0};{1}", proj.Name, proj.ProjectorID));
                }

                //Projector Lifts
                //TODO: MoveTime was left out of the DB - This may need to be added back - for Now it's hardcoded
                foreach (var lifts in l_objProjectorLifts)
                {
                    m_objProjectorLifts.Add(new ProjectorLift(lifts.Name, lifts.COMPort, 15000, m_cstrSessionID, m_objLogLevel));
                    m_lstPublicProjectorLiftInfo.Add(string.Format("{0};{1}", lifts.Name, lifts.ProjectorLiftID));
                }

                //Matrix Switchers
                foreach (var switchers in l_objMatrixSwitchers)
                {
                    m_objMatrixSwitchers.Add(new MatrixSwitcher(switchers.Name, switchers.COMPort, switchers.IPAddress, m_cstrSessionID, m_objLogLevel));
                    m_lstPublicSwitcherInfo.Add(string.Format("{0};{1}", switchers.Name, switchers.MatrixSwitcherID));
                }
            }
            catch (System.Exception ex)
            {
                l_objStatus = actionStatus.Error;
                string strFullException = ex.ToString();
                m_objLogger.logToMemory(string.Format("{0}: Error: {1}", l_strFunctionName, ex.ToString()), l_objStatus);
                throw new System.Exception("loadConfigurations() ERROR: " + ex.ToString());
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }
        }

        public string displaySettings()
        {
            m_objLogger.logToMemory("==============CURRENT APPLICATION SETTINGS==============");
            m_objLogger.logToMemory("-----Televisions-----");
            int i = 0;
            foreach (Television objTempTV in m_objTelevisions)
            {
                i++;
                m_objLogger.logToMemory("TV #" + i.ToString() + " Information");
                m_objLogger.logToMemory(objTempTV.GetDeviceInfo());
            }

            i = 0;
            m_objLogger.logToMemory("-----Projectors-----");
            foreach (Projector objTempProj in m_objProjectors)
            {
                i++;
                m_objLogger.logToMemory("Projector #" + i.ToString() + " Information");
                m_objLogger.logToMemory(objTempProj.GetDeviceInfo());
            }

            i = 0;
            m_objLogger.logToMemory("-----Projector Lifts-----");
            foreach (ProjectorLift objTempLift in m_objProjectorLifts)
            {
                i++;
                m_objLogger.logToMemory("Lift #" + i.ToString() + " Information");
                m_objLogger.logToMemory(objTempLift.GetDeviceInfo());
            }
            m_objLogger.logToMemory("==============END OF APPLICATION SETTINGS===============");

            return m_objLogger.writePendingToDB(actionStatus.Success, true);
        }

        public List<string> getAvailableDeviceInfo(DeviceTypes p_objdeviceType)
        {
            List<string> l_strDevices = new List<string>();
            var i = 1;

            switch (p_objdeviceType)
            {
                case DeviceTypes.Projector:
                    foreach (Projector l_objProj in m_objProjectors)
                    {
                        l_strDevices.Add(i + "|" + l_objProj.GetDeviceInfo(true));
                        i++;
                    }
                    break;
                case DeviceTypes.ProjectorLift:
                    foreach (ProjectorLift l_objLift in m_objProjectorLifts)
                    {
                        l_strDevices.Add(i + "|" + l_objLift.GetDeviceInfo(true));
                        i++;
                    }
                    break;
                case DeviceTypes.Television:
                    foreach (Television l_objTV in m_objTelevisions)
                    {
                        l_strDevices.Add(i + "|" + l_objTV.GetDeviceInfo(true));
                        i++;
                    }
                    break;
                case DeviceTypes.MatrixSwitcher:
                    foreach (MatrixSwitcher l_objSW in m_objMatrixSwitchers)
                    {
                        l_strDevices.Add(i + "|" + l_objSW.GetDeviceInfo(true));
                        i++;
                    }
                    break;
            }
            return l_strDevices;
        }        

        #endregion

        #region Device Functionality Methods

        public string printProjectorStatus(int p_intProjID = 1)
        {
            var l_strStatus = "Status cannot be attained at this time. Projector #" + p_intProjID + " is not configured properly";
            actionStatus l_objActionStatus = actionStatus.Error;

            if (isDeviceConfigured(p_intProjID, DeviceTypes.Projector))
            {
                l_strStatus = m_objProjectors[p_intProjID - 1].printProjectorStatus();
                l_objActionStatus = actionStatus.Success;
            }

            return m_objLogger.logToDB(l_strStatus, l_objActionStatus, true);
        }

        private void setStartStopDeviceLoopValues(ref int p_intStartDevice, ref int p_intStopDevice, int p_intDeviceCount, int p_intDesiredDevice)
        {
            //99 = All of the particular devices
            if (p_intDesiredDevice == 99)
            {
                p_intStopDevice = p_intDeviceCount;
                p_intStartDevice = p_intDeviceCount > 0 ? 1 : 0;
            }
            else
                p_intStartDevice = p_intStopDevice = p_intDesiredDevice;


            //Let's not even go through the process
            if (p_intStartDevice <= 0) p_intStopDevice = -1;
        }

        private void DetermineOverallStatus(ref actionStatus p_objMainStatus, actionStatus p_objTempStatus)
        {
            switch (p_objTempStatus)
            {
                case actionStatus.Success:
                    if (p_objMainStatus == actionStatus.Error || p_objMainStatus == actionStatus.PartialError) p_objMainStatus = actionStatus.PartialError;
                    else p_objMainStatus = actionStatus.Success;
                    break;
                case actionStatus.Error:
                    if (p_objMainStatus == actionStatus.Success || p_objMainStatus == actionStatus.PartialError) p_objMainStatus = actionStatus.PartialError;
                    else p_objMainStatus = actionStatus.Error;
                    break;
                case actionStatus.None:
                    //Do Nothing - No Changes to return
                    break;
                default:
                    p_objMainStatus = p_objTempStatus;
                    break;
            }
        }

        /// <summary>
        /// for the console App - Quickly just turns them all on.
        /// </summary>
        /// <param name="p_intProjID"></param>
        /// <param name="p_intLiftID"></param>
        /// <param name="p_intTVID"></param>
        /// <param name="p_objProgress"></param>
        /// <returns></returns>
        public actionStatus turnSystemOn(int p_intProjID = 1, int p_intLiftID = 1, int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            //TODO: Need to switch these to all DB logging
            var l_objOverallStatus = actionStatus.None;
            var l_strFunctionName = "turnSystemOn";
            bool l_blnLoggingProgress = (p_objProgress != null);

            try
            {
                int l_intStartDevice, l_intStopDevice;
                actionStatus l_objTVStatus, l_objProjStatus, l_objLiftStatus;
                l_objLiftStatus = l_objTVStatus = l_objProjStatus = actionStatus.None;
                l_intStartDevice = l_intStopDevice = 0;
                
                //TVs
                setStartStopDeviceLoopValues(ref l_intStartDevice, ref l_intStopDevice, m_objTelevisions.Count(), p_intTVID);
                for (int l_intIterator = l_intStartDevice; l_intIterator <= l_intStopDevice; l_intIterator++)
                {
                    if (isDeviceConfigured(l_intIterator, DeviceTypes.Television))
                    {
                        //Attempt to wake up the TV and set the TV Master Status
                        m_objLogger.logToMemory("Waking up TV " + l_intIterator, l_objTVStatus);
                        DetermineOverallStatus(ref l_objTVStatus, (m_objTelevisions[l_intIterator - 1].WakeupTV()));
                        m_objLogger.logToMemory("Call to Wake up TV Completed", l_objTVStatus);
                        if (l_objTVStatus != actionStatus.Error)
                        {
                            if (!m_objTelevisions[l_intIterator - 1].TelevisionRegistered)
                            {
                                DetermineOverallStatus(ref l_objTVStatus, actionStatus.PartialError);
                                m_objLogger.logToMemory("The TV Needs to be Registered before any other commands will work.", l_objTVStatus);
                            }
                            else { DetermineOverallStatus(ref l_objTVStatus, actionStatus.Success); }
                        }                        
                    }
                    else
                    {
                        l_objTVStatus = actionStatus.Error;
                        
                        /* TODO: STOPPED HERE WITH LOGGER FUNCTIONALITY  - Need to finish so that logs are written as expected */
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Television #" + l_intIterator + " is not configured. Please check the configuration settings.");
                    }
                }

                //Projectors
                setStartStopDeviceLoopValues(ref l_intStartDevice, ref l_intStopDevice, m_objProjectors.Count(), p_intProjID);
                for (int l_intIterator = l_intStartDevice; l_intIterator <= l_intStopDevice; l_intIterator++)
                {
                    //Start Here Next Time
                    if (isDeviceConfigured(l_intIterator, DeviceTypes.Projector))
                    {
                        switch (m_objProjectors[l_intIterator - 1].checkProjectorPowerStatus())
                        {
                            case "OFF":
                            case "UNKNOWN":
                                if (l_blnLoggingProgress) 
                                    p_objProgress.AppendLine("Lowering projector lift...");
                                
                                //TODO Make a function to use the logic of the projector class settings to find the proper lift by name instead of using the same iterator.
                                //Task.WaitAll(m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Extend));
                                AsyncContext.Run(() => m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Extend));
                                
                                DetermineOverallStatus(ref l_objProjStatus, (m_objProjectors[l_intIterator - 1].turnOnProjector()));
                                break;

                            case "ON":
                                //Even though the projector is already on, there may be cases where the projector lift was not extended before hand 
                                //(e.g. Manual operations), so let's extend it now for good measure
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("Projector # " + l_intIterator + " was already on!..... Attempting to Extend Lift Now.");
                                AsyncContext.Run(() => m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Extend));
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);

                                break;
                        }

                        //Wait until projector is fully powered on
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Waiting for projector to power on and warmup...");
                        
                        int l_intProjWarmUpTime = 0;
                        while ((m_objProjectors[l_intIterator - 1].checkProjectorPowerStatus() != "ON") && (l_intProjWarmUpTime < 120000))
                        {
                            System.Threading.Thread.Sleep(750); // pause for 3/4 second;
                            l_intProjWarmUpTime += 750;
                            //Console.WriteLine("Current Projector Status: " + hallAutomations.getProjectorStatus());

                            if (l_intProjWarmUpTime >= 120000)
                            {
                                if (l_blnLoggingProgress) 
                                    p_objProgress.AppendLine("It's taking the projector #" + l_intIterator + " too long to Turn On/Warm Up. Moving on. Please check the projector settings");
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.PartialError);
                            }
                        }

                        if (l_blnLoggingProgress) p_objProgress.AppendLine("Pausing for 5 seconds, then changing the projectors input to HDMI...");
                        System.Threading.Thread.Sleep(5000);
                        m_objProjectors[l_intIterator - 1].changeProjectorToHDMI();
                        DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);
                    }
                    else
                    {
                        l_objProjStatus = actionStatus.Error;
                        p_objProgress.AppendLine("Projector #" + l_intIterator + " is not configured. Please check the configuration settings.");
                    }
                }

                if (l_objProjStatus == actionStatus.Success && l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.Success;
                else if (l_objProjStatus == actionStatus.Success || l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.PartialError;
                else
                    l_objOverallStatus = actionStatus.Error;

                //TODO: Need to replace all of the stringbuilder references with the DB Logger (string reference)
                m_objLogger.logToDB(p_objProgress.ToString(), l_objOverallStatus);
            }
            catch (Exception e)
            {
                //throw e;
                l_objOverallStatus = actionStatus.Error;
                string exLog = "An Error Occured Turning the system on: " + e.ToString();
                m_objLogger.logToDB(exLog, actionStatus.Error, p_blnError_WriteMemoryToDB: true);
                p_objProgress.AppendLine(exLog);
            }
            finally { m_objLogger.writePendingToDB(l_objOverallStatus, p_strFunctionName: l_strFunctionName); }

            return l_objOverallStatus;
        }

        private bool isItemInList(string p_strCurrentItem, List<string> p_strList)
        {
            var l_blnIn = false;

            try
            {
                foreach(var strListItem in p_strList)
                {
                    if (strListItem.Equals(p_strCurrentItem, StringComparison.OrdinalIgnoreCase)) 
                    {
                        l_blnIn = true;
                        break;
                    }
                }
            }
            catch 
            { 
                //Just return False 
            }

            return l_blnIn;
        }

        public actionStatus PowerOnSingleTV(string p_strTVName)
        {
            actionStatus l_objTVStatus = actionStatus.None;
            var l_strFunctionName = "PowerOffSingleTV";
            try
            {
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if(l_objTelevision.TelevisionName == p_strTVName)
                    {
                        m_objLogger.logToMemory("Waking up TV: " + l_objTelevision.TelevisionName, l_objTVStatus);
                        l_objTVStatus = l_objTelevision.WakeupTV();
                        m_objLogger.logToMemory("Call to Wake up TV Completed", l_objTVStatus);
                        if (l_objTVStatus != actionStatus.Error)
                        {
                            if (!l_objTelevision.TelevisionRegistered)
                            {
                                l_objTVStatus = actionStatus.PartialError;
                                m_objLogger.logToMemory("The TV Needs to be Registered before any other commands will work.", l_objTVStatus);
                            }
                            else
                            {
                                l_objTVStatus = actionStatus.Success;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objTVStatus = actionStatus.Error;
                m_objLogger.logToMemory("An Error Occured Turning On " + p_strTVName + ": " + e.ToString(), l_objTVStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objTVStatus, p_strFunctionName: l_strFunctionName); }

            return l_objTVStatus;
        }

        public actionStatus PowerOffSingleTV(string p_strTVName)
        {
            actionStatus l_objTVStatus = actionStatus.None;
            var l_strFunctionName = "PowerOffSingleTV";
            try
            {
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if (l_objTelevision.TelevisionName == p_strTVName)
                    {
                        try
                        {
                            m_objLogger.logToMemory("Turning Off TV: " + l_objTelevision.TelevisionName, l_objTVStatus);
                            l_objTVStatus = l_objTelevision.PowerOffTV();
                            m_objLogger.logToMemory("TV Off", l_objTVStatus);
                        }
                        catch (Exception e)
                        {
                            l_objTVStatus = actionStatus.Error;
                            m_objLogger.logToMemory("Issue Turning off " + l_objTelevision.TelevisionName + ": " + e.ToString(), actionStatus.Error);
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objTVStatus = actionStatus.Error;
                m_objLogger.logToMemory("An Error Occured Turning Off " + p_strTVName + ": " + e.ToString(), l_objTVStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objTVStatus, p_strFunctionName: l_strFunctionName); }

            return l_objTVStatus;
        }

        //public actionStatus turnSystemOn(string p_strTVsOn, string p_strProjsOn, StringBuilder p_objProgress = null)
        public actionStatus turnSystemOn(List<string> p_strTVsOn, List<string> p_strProjsOn, StringBuilder p_objProgress = null)
        {
            //TODO: Need to switch these to all DB logging - Still TODO
            var l_objOverallStatus = actionStatus.None;
            var l_strFunctionName = "turnSystemOn";
            bool l_blnLoggingProgress = (p_objProgress != null);
            try
            {
                actionStatus l_objTVStatus, l_objProjStatus, l_objLiftStatus;
                l_objLiftStatus = l_objTVStatus = l_objProjStatus = actionStatus.None;

                //TVs
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if(isItemInList(l_objTelevision.TelevisionName, p_strTVsOn))
                    {
                        m_objLogger.logToMemory("Waking up TV: " + l_objTelevision.TelevisionName, l_objTVStatus);
                        DetermineOverallStatus(ref l_objTVStatus, (l_objTelevision.WakeupTV()));
                        m_objLogger.logToMemory("Call to Wake up TV Completed", l_objTVStatus);
                        if (l_objTVStatus != actionStatus.Error)
                        {
                            DetermineOverallStatus(ref l_objTVStatus, (l_objTelevision.TelevisionRegistered == false ? actionStatus.PartialError : actionStatus.Success));
                            m_objLogger.logToMemory("The TV Needs to be Registered before any other commands will work.", l_objTVStatus);
                        }
                    }
                    else
                    {
                        l_objTVStatus = actionStatus.Error;
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Television " + l_objTelevision.TelevisionName + " is not configured. Please check the configuration settings.");
                    }
                }

                //Projectors
                foreach (var l_objProjector in m_objProjectors)
                {
                    if(isItemInList(l_objProjector.projectorName, p_strProjsOn))
                    {
                        switch (l_objProjector.checkProjectorPowerStatus())
                        {
                            case "OFF":
                            case "UNKNOWN":
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("Lowering projector lift...");

                                
                                //Task.WaitAll(m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Extend));
                                foreach (var l_objLift in m_objProjectorLifts)
                                {
                                    if (l_objProjector.projectorLiftAssociation == l_objLift.LiftName)
                                    {
                                        AsyncContext.Run(() => l_objLift.doLiftAction(LiftAction.Extend));
                                        break;
                                    }
                                }
                                DetermineOverallStatus(ref l_objProjStatus, (l_objProjector.turnOnProjector()));
                                break;

                            case "ON":
                                //Even though the projector is already on, there may be cases where the projector lift was not extended before hand 
                                //(e.g. Manual operations), so let's extend it now for good measure
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("Projector: " + l_objProjector.projectorName + " was already on!..... Attempting to Extend Lift Now.");
                                foreach (var l_objLift in m_objProjectorLifts)
                                {
                                    if (l_objProjector.projectorLiftAssociation == l_objLift.LiftName)
                                    {
                                        AsyncContext.Run(() => l_objLift.doLiftAction(LiftAction.Extend));
                                        break;
                                    }
                                }
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);

                                break;
                        }

                        //Wait until projector is fully powered on
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Waiting for projector to power on and warmup...");

                        int l_intProjWarmUpTime = 0;
                        while ((l_objProjector.checkProjectorPowerStatus() != "ON") && (l_intProjWarmUpTime < 120000))
                        {
                            System.Threading.Thread.Sleep(750); // pause for 3/4 second;
                            l_intProjWarmUpTime += 750;
                            //Console.WriteLine("Current Projector Status: " + hallAutomations.getProjectorStatus());

                            if (l_intProjWarmUpTime >= 120000)
                            {
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("It's taking the projector #" + l_objProjector.projectorName + " too long to Turn On/Warm Up. Moving on. Please check the projector settings");
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.PartialError);
                            }
                        }

                        if (l_blnLoggingProgress) p_objProgress.AppendLine("Pausing for 5 seconds, then changing the projectors input to HDMI...");
                        System.Threading.Thread.Sleep(5000);
                        l_objProjector.changeProjectorToHDMI();
                        DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);
                    }
                    else
                    {
                        l_objProjStatus = actionStatus.Error;
                        p_objProgress.AppendLine("Projector " + l_objProjector.projectorName + " is not configured. Please check the configuration settings.");
                    }
                }

                if (l_objProjStatus == actionStatus.Success && l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.Success;
                else if (l_objProjStatus == actionStatus.Success || l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.PartialError;
                else
                    l_objOverallStatus = actionStatus.Error;

                //TODO: Need to replace all of the stringbuilder references with the DB Logger (string reference)
                m_objLogger.logToDB(p_objProgress.ToString(), l_objOverallStatus);
            }
            catch (Exception e)
            {
                //throw e;
                l_objOverallStatus = actionStatus.Error;
                string exLog = "An Error Occured Turning the system on: " + e.ToString();
                m_objLogger.logToDB(exLog, actionStatus.Error, p_blnError_WriteMemoryToDB: true);
                p_objProgress.AppendLine(exLog);
            }
            finally { m_objLogger.writePendingToDB(l_objOverallStatus, p_strFunctionName: l_strFunctionName); }

            return l_objOverallStatus;
        }

        public actionStatus turnSystemOff(int p_intProjID = 1, int p_intLiftID = 1, int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            
            ////TODO Figure out how to power off TVs
            //Console.WriteLine("Powering off TV...");
            ////Power off TV Here!

            //TODO: FIGURE OUT HOW TO POLL/TRACK WHAT IS ON SO THERE IS NO GUESSING
            var l_objOverallStatus = actionStatus.None;
            var l_strFunctionName = "turnSystemOff()";
            bool l_blnLoggingProgress = (p_objProgress != null);

            try
            {
                int l_intStartDevice, l_intStopDevice;
                actionStatus l_objTVStatus, l_objProjStatus, l_objLiftStatus;
                l_objLiftStatus = l_objTVStatus = l_objProjStatus = actionStatus.None;
                l_intStartDevice = l_intStopDevice = 0;

                //TVs
                setStartStopDeviceLoopValues(ref l_intStartDevice, ref l_intStopDevice, m_objTelevisions.Count(), p_intTVID);
                for (int l_intIterator = l_intStartDevice; l_intIterator <= l_intStopDevice; l_intIterator++)
                {
                    if (isDeviceConfigured(l_intIterator, DeviceTypes.Television))
                    {
                        //Attempt to wake up the TV and set the TV Master Status
                        //TODO: TURN OFF TV
                        //DetermineOverallStatus(ref l_objTVStatus, (m_objTelevisions[l_intIterator - 1].TurnOff(p_objProgress)));
                    }
                    else
                    {
                        l_objTVStatus = actionStatus.Error;
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Television #" + l_intIterator + " is not configured. Please check the configuration settings.");
                    }
                }

                //Projectors
                setStartStopDeviceLoopValues(ref l_intStartDevice, ref l_intStopDevice, m_objProjectors.Count(), p_intProjID);
                for (int l_intIterator = l_intStartDevice; l_intIterator <= l_intStopDevice; l_intIterator++)
                {
                    //Start Here Next Time
                    if (isDeviceConfigured(l_intIterator, DeviceTypes.Projector))
                    {
                        switch (m_objProjectors[l_intIterator - 1].checkProjectorPowerStatus())
                        {
                            case "OFF":
                                //Even though the projector is already off, there may be cases where the projector lift was not retracted before hand 
                                //(e.g. Manual operations), so let's retract it now for good measure
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("Projector # " + l_intIterator + " was already off!..... Attempting to Retract Lift Now.");
                                AsyncContext.Run(() => m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Retract));
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);
                                break;

                            case "ON":
                            case "UNKNOWN":
                                DetermineOverallStatus(ref l_objProjStatus, (m_objProjectors[l_intIterator - 1].turnOffProjector()));

                                //Wait until projector is fully powered off
                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Waiting for projector to power off and cooldown...");
                                int l_intProjCoolDownTime = 0;
                                while ((m_objProjectors[l_intIterator - 1].checkProjectorPowerStatus() != "UNKNOWN") && (l_intProjCoolDownTime < 120000))
                                {
                                    System.Threading.Thread.Sleep(750); // pause for 1/4 second;
                                    l_intProjCoolDownTime += 750;
                                    if (l_blnLoggingProgress) p_objProgress.AppendLine("Current Projector Status: " + m_objProjectors[l_intIterator - 1].getProjectorStatus());

                                    if (l_intProjCoolDownTime >= 120000)
                                    {
                                        if (l_blnLoggingProgress)
                                            p_objProgress.AppendLine("It's taking the projector #" + l_intIterator + " too long to Cool Down. Moving on. Please check the projector settings");
                                        DetermineOverallStatus(ref l_objProjStatus, actionStatus.PartialError);
                                    }
                                }

                                if (l_intProjCoolDownTime < 120000) DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success); //Cooled Down in Time

                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Retracting projector lift...");
                                //TODO Make a function to use the logic of the projector class settings to find the proper lift by name instead of using the same iterator.
                                AsyncContext.Run(() => m_objProjectorLifts[l_intIterator - 1].doLiftAction(LiftAction.Retract));
                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Completed Retracting projector lift...");
                                break;
                        }
                    }
                    else
                    {
                        l_objProjStatus = actionStatus.Error;
                        p_objProgress.AppendLine("Projector #" + l_intIterator + " is not configured. Please check the configuration settings.");
                    }
                }

                if (l_objProjStatus == actionStatus.Success && l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.Success;
                else if (l_objProjStatus == actionStatus.Success || l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.PartialError;
                else
                    l_objOverallStatus = actionStatus.Error;

                //TODO: Need to replace all of the stringbuilder references with the DB Logger (string reference)
                m_objLogger.logToDB(p_objProgress.ToString(), l_objOverallStatus);
            }
            catch (Exception e)
            {
                //throw e;
                l_objOverallStatus = actionStatus.Error;
                string exLog = "An Error Occured Turning the system off: " + e.ToString();
                m_objLogger.logToDB(exLog, actionStatus.Error, p_blnError_WriteMemoryToDB: true);
                p_objProgress.AppendLine(exLog);
            }
            finally { m_objLogger.writePendingToDB(l_objOverallStatus, p_strFunctionName: l_strFunctionName); }

            return l_objOverallStatus;
        }

        public actionStatus turnSystemOff(List<string> p_strTVsOff, List<string> p_strProjsOff, StringBuilder p_objProgress = null)
        {

            ////TODO Figure out how to power off TVs
            //Console.WriteLine("Powering off TV...");
            ////Power off TV Here!

            //TODO: FIGURE OUT HOW TO POLL/TRACK WHAT IS ON SO THERE IS NO GUESSING
            var l_objOverallStatus = actionStatus.None;
            var l_strFunctionName = "turnSystemOff()";
            bool l_blnLoggingProgress = (p_objProgress != null);

            try
            {
                actionStatus l_objTVStatus, l_objProjStatus, l_objLiftStatus;
                l_objLiftStatus = l_objTVStatus = l_objProjStatus = actionStatus.None;

                //TVs
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if (isItemInList(l_objTelevision.TelevisionName, p_strTVsOff))
                    {
                        try
                        {
                            DetermineOverallStatus(ref l_objTVStatus, l_objTelevision.PowerOffTV());
                        }
                        catch (Exception e)
                        {
                            l_objTVStatus = actionStatus.Error;
                            m_objLogger.logToMemory("Issue Turning off " + l_objTelevision.TelevisionName + ": " + e.ToString(), actionStatus.Error);
                        }
                    }
                    else
                    {
                        l_objTVStatus = actionStatus.Error;
                        if (l_blnLoggingProgress)
                            p_objProgress.AppendLine("Television " + l_objTelevision.TelevisionName + " is not configured. Please check the configuration settings.");
                    }
                }

                //Projectors
                foreach (var l_objProjector in m_objProjectors)
                {
                    if(isItemInList(l_objProjector.projectorName, p_strProjsOff))
                    {
                        switch (l_objProjector.checkProjectorPowerStatus())
                        {
                            case "OFF":
                                //Even though the projector is already off, there may be cases where the projector lift was not retracted before hand 
                                //(e.g. Manual operations), so let's retract it now for good measure
                                if (l_blnLoggingProgress)
                                    p_objProgress.AppendLine("Projector " + l_objProjector + " was already off!..... Attempting to Retract Lift Now.");
                                foreach (var l_objLift in m_objProjectorLifts)
                                {
                                    if (l_objProjector.projectorLiftAssociation == l_objLift.LiftName)
                                    {
                                        AsyncContext.Run(() => l_objLift.doLiftAction(LiftAction.Retract));
                                        break;
                                    }
                                }
                                DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success);
                                break;
                            case "ON":
                            case "UNKNOWN":
                                DetermineOverallStatus(ref l_objProjStatus, (l_objProjector.turnOffProjector()));

                                //Wait until projector is fully powered off
                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Waiting for projector to power off and cooldown...");
                                int l_intProjCoolDownTime = 0;
                                while ((l_objProjector.checkProjectorPowerStatus() != "UNKNOWN") && (l_intProjCoolDownTime < 120000))
                                {
                                    System.Threading.Thread.Sleep(750); // pause for 1/4 second;
                                    l_intProjCoolDownTime += 750;
                                    if (l_blnLoggingProgress) p_objProgress.AppendLine("Current Projector Status: " + l_objProjector.getProjectorStatus());

                                    if (l_intProjCoolDownTime >= 120000)
                                    {
                                        if (l_blnLoggingProgress)
                                            p_objProgress.AppendLine("It's taking the projector " + l_objProjector.projectorName + " too long to Cool Down. Moving on. Please check the projector settings");
                                        DetermineOverallStatus(ref l_objProjStatus, actionStatus.PartialError);
                                    }
                                }

                                if (l_intProjCoolDownTime < 120000) DetermineOverallStatus(ref l_objProjStatus, actionStatus.Success); //Cooled Down in Time

                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Retracting projector lift...");
                                
                                foreach (var l_objLift in m_objProjectorLifts)
                                {
                                    if (l_objProjector.projectorLiftAssociation == l_objLift.LiftName)
                                    {
                                        AsyncContext.Run(() => l_objLift.doLiftAction(LiftAction.Retract));
                                        break;
                                    }
                                }
                                if (l_blnLoggingProgress) p_objProgress.AppendLine("Completed Retracting projector lift...");
                                break;
                        }
                    }
                    else
                    {
                        l_objProjStatus = actionStatus.Error;
                        p_objProgress.AppendLine("Projector " + l_objProjector.projectorName + " is not configured. Please check the configuration settings.");
                    }
                }
                
                if (l_objProjStatus == actionStatus.Success && l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.Success;
                else if (l_objProjStatus == actionStatus.Success || l_objTVStatus == actionStatus.Success)
                    l_objOverallStatus = actionStatus.PartialError;
                else
                    l_objOverallStatus = actionStatus.Error;

                //TODO: Need to replace all of the stringbuilder references with the DB Logger (string reference)
                m_objLogger.logToDB(p_objProgress.ToString(), l_objOverallStatus);
            }
            catch (Exception e)
            {
                //throw e;
                l_objOverallStatus = actionStatus.Error;
                string exLog = "An Error Occured Turning the system off: " + e.ToString();
                m_objLogger.logToDB(exLog, actionStatus.Error, p_blnError_WriteMemoryToDB: true);
                p_objProgress.AppendLine(exLog);
            }
            finally { m_objLogger.writePendingToDB(l_objOverallStatus, p_strFunctionName: l_strFunctionName); }

            return l_objOverallStatus;
        }

        public actionStatus WakeupTV(int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].WakeupTV();

            return l_objStatus;
        }

        public actionStatus PowerOffTV(int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].PowerOffTV();

            return l_objStatus;
        }

        /// <summary>
        /// Send a Specific Command to the TV.
        /// Must have the exact command name for this operation to work.
        /// Use getCommandNames() to return the list of available commands for a specific TV ('|' Delimited)
        /// </summary>
        /// <param name="p_strTVName"></param>
        /// <param name="p_strCommandName"></param>
        /// <returns></returns>
        public actionStatus SendTVCommand(string p_strTVName, string p_strCommandName)
        {
            actionStatus l_objTVStatus = actionStatus.None;
            var l_strFunctionName = "SendTVCommand";
            try
            {
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if (l_objTelevision.TelevisionName == p_strTVName)
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Sending {2} to the TV.", l_strFunctionName , p_strTVName, p_strCommandName), l_objTVStatus);
                        l_objTVStatus = l_objTelevision.SendCommandByName(p_strCommandName);

                        if (l_objTVStatus == actionStatus.Success)
                            m_objLogger.logToMemory(string.Format("{0}: {1}: A status of: {2} was returned from the call to SendCommandByName({3})", l_strFunctionName, p_strTVName, l_objTVStatus.ToString(), p_strCommandName), l_objTVStatus);
                        else if (l_objTVStatus == actionStatus.Error)
                            throw new Exception(string.Format("An Error Status was returned from the call to SendCommandByName({0})", p_strCommandName));

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objTVStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: An Error Occurred sending {2} to the TV: {3}", l_strFunctionName, p_strTVName, p_strCommandName, e.ToString()), l_objTVStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objTVStatus, p_strFunctionName: l_strFunctionName); }

            return l_objTVStatus;
        }

        public actionStatus SendProjectorCommand(string p_strProjectorName, string p_strCommandName)
        {
            actionStatus l_objProjectorStatus = actionStatus.None;
            var l_strFunctionName = "SendProjectorCommand";
            try
            {
                foreach (var l_objProjector in m_objProjectors)
                {
                    if (l_objProjector.projectorName == p_strProjectorName)
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Sending {2} to the Projector.", l_strFunctionName, p_strProjectorName, p_strCommandName), l_objProjectorStatus);
                        l_objProjectorStatus = l_objProjector.SendCommandByName(p_strCommandName);

                        if (l_objProjectorStatus == actionStatus.Success)
                            m_objLogger.logToMemory(string.Format("{0}: {1}: A status of: {2} was returned from the call to SendCommandByName({3})", l_strFunctionName, p_strProjectorName, l_objProjectorStatus.ToString(), p_strCommandName), l_objProjectorStatus);
                        else if (l_objProjectorStatus == actionStatus.Error)
                            throw new Exception(string.Format("An Error Status was returned from the call to SendCommandByName({0})", p_strCommandName));

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objProjectorStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: An Error Occurred sending {2} to the Projector: {3}", l_strFunctionName, p_strProjectorName, p_strCommandName, e.ToString()), l_objProjectorStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objProjectorStatus, p_strFunctionName: l_strFunctionName); }

            return l_objProjectorStatus;
        }

        public actionStatus SendLiftCommand(string p_strLiftName, string p_strCommandName)
        {
            actionStatus l_objLiftStatus = actionStatus.None;
            var l_strFunctionName = "SendLiftCommand";
            try
            {
                foreach (var l_objLift in m_objProjectorLifts)
                {
                    if (l_objLift.LiftName == p_strLiftName)
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Sending {2} to the Projector Lift.", l_strFunctionName, p_strLiftName, p_strCommandName), l_objLiftStatus);
                        l_objLiftStatus = l_objLift.SendCommandByName(p_strCommandName);

                        if (l_objLiftStatus == actionStatus.Success)
                            m_objLogger.logToMemory(string.Format("{0}: {1}: A status of: {2} was returned from the call to SendCommandByName({3})", l_strFunctionName, p_strLiftName, l_objLiftStatus.ToString(), p_strCommandName), l_objLiftStatus);
                        else if (l_objLiftStatus == actionStatus.Error)
                            throw new Exception(string.Format("An Error Status was returned from the call to SendCommandByName({0})", p_strCommandName));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objLiftStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: An Error Occurred sending {2} to the Projector Lift: {3}", l_strFunctionName, p_strLiftName, p_strCommandName, e.ToString()), l_objLiftStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objLiftStatus, p_strFunctionName: l_strFunctionName); }

            return l_objLiftStatus;
        }

        public actionStatus SendMatrixCommand(string p_strMatrixName, string p_strCommandName)
        {
            actionStatus l_objMatrixStatus = actionStatus.None;
            var l_strFunctionName = "SendMatrixCommand";
            try
            {
                foreach (var l_objMatrix in m_objMatrixSwitchers)
                {
                    if (l_objMatrix.SwitcherName == p_strMatrixName)
                    {
                        m_objLogger.logToMemory(string.Format("{0}: {1}: Sending {2} to the Matrix Switcher.", l_strFunctionName, p_strMatrixName, p_strCommandName), l_objMatrixStatus);
                        l_objMatrixStatus = l_objMatrix.SendCommandByName(p_strCommandName);

                        if (l_objMatrixStatus == actionStatus.Success)
                            m_objLogger.logToMemory(string.Format("{0}: {1}: A status of: {2} was returned from the call to SendCommandByName({3})", l_strFunctionName, p_strMatrixName, l_objMatrixStatus.ToString(), p_strCommandName), l_objMatrixStatus);
                        else if (l_objMatrixStatus == actionStatus.Error)
                            throw new Exception(string.Format("An Error Status was returned from the call to SendCommandByName({0})", p_strCommandName));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                l_objMatrixStatus = actionStatus.Error;
                m_objLogger.logToMemory(string.Format("{0}: {1}: An Error Occurred sending {2} to the Matrix Switcher: {3}", l_strFunctionName, p_strMatrixName, p_strCommandName, e.ToString()), l_objMatrixStatus);
            }
            finally { m_objLogger.writePendingToDB(l_objMatrixStatus, p_strFunctionName: l_strFunctionName); }

            return l_objMatrixStatus;
        }


        /// <summary>
        /// Obsolete.
        /// This function was used for the original Console application.
        /// Use SendCommandByValue(string p_strSelectedTV, string p_strCommand) for any UI versions.
        /// </summary>
        /// <param name="p_strCommand"></param>
        /// <param name="p_intTVID"></param>
        /// <param name="p_objProgress"></param>
        /// <returns></returns>
        public actionStatus SendCommandByValue(string p_strCommand, int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].SendCommandByValue(p_strCommand);

            return l_objStatus;
        }

        /// <summary>
        /// Intended to allow advanced users to test/send specific commands to the Television
        /// </summary>
        /// <param name="p_strSelectedTV"></param>
        /// <param name="p_strCommand"></param>
        /// <returns></returns>
        public actionStatus SendCommandByValue(string p_strSelectedTV, string p_strCommand)
        {
            var l_strFunctionName = "SendCommandByValue()";
            var l_objStatus = actionStatus.None;
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to send the following Command: {2}", l_strFunctionName, p_strSelectedTV, p_strCommand));
            foreach (var l_objTelevision in m_objTelevisions)
            {
                if (l_objTelevision.TelevisionName == p_strSelectedTV)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Television Located.", l_strFunctionName, p_strSelectedTV));
                    l_objStatus = l_objTelevision.SendCommandByValue(p_strCommand);
                    break;
                }
            }

            switch(l_objStatus)
            {
                case actionStatus.Success:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Successfully Sent: {2}", l_strFunctionName, p_strSelectedTV, p_strCommand));
                    break;
                case actionStatus.PartialError:
                case actionStatus.Error:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Not Sent Successfully: {2}", l_strFunctionName, p_strSelectedTV, p_strCommand));
                    break;
                case actionStatus.None :
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Status Unknown: {2}", l_strFunctionName, p_strSelectedTV, p_strCommand));
                    break;
            }

            return l_objStatus;
        }

        /// <summary>
        /// Intended to allow advanced users to test/send specific commands to the Projector
        /// </summary>
        /// <param name="p_strSelectedTV"></param>
        /// <param name="p_strCommand"></param>
        /// <returns></returns>
        public actionStatus SendProjectorCommandByValue(string p_strSelectedProjector, string p_strCommand)
        {
            var l_strFunctionName = "SendProjectorCommandByValue()";
            var l_objStatus = actionStatus.None;
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to send the following Command: {2}", l_strFunctionName, p_strSelectedProjector, p_strCommand));
            foreach (var l_objProjector in m_objProjectors)
            {
                if (l_objProjector.projectorName == p_strSelectedProjector)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Projector Located.", l_strFunctionName, p_strSelectedProjector));
                    l_objStatus = l_objProjector.SendCommandByValue(p_strCommand);
                    break;
                }
            }

            switch (l_objStatus)
            {
                case actionStatus.Success:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Successfully Sent: {2}", l_strFunctionName, p_strSelectedProjector, p_strCommand));
                    break;
                case actionStatus.PartialError:
                case actionStatus.Error:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Not Sent Successfully: {2}", l_strFunctionName, p_strSelectedProjector, p_strCommand));
                    break;
                case actionStatus.None:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Status Unknown: {2}", l_strFunctionName, p_strSelectedProjector, p_strCommand));
                    break;
            }

            return l_objStatus;
        }

        public actionStatus SendLiftCommandByValue(string p_strSelectedLift, string p_strCommand)
        {
            var l_strFunctionName = "SendLiftCommandByValue()";
            var l_objStatus = actionStatus.None;
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to send the following Command: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
            foreach (var l_objLift in m_objProjectorLifts)
            {
                if (l_objLift.LiftName == p_strSelectedLift)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Lift Located.", l_strFunctionName, p_strSelectedLift));
                    l_objStatus = l_objLift.SendCommandByValue(p_strCommand);
                    break;
                }
            }

            switch (l_objStatus)
            {
                case actionStatus.Success:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Successfully Sent: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
                case actionStatus.PartialError:
                case actionStatus.Error:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Not Sent Successfully: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
                case actionStatus.None:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Status Unknown: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
            }

            return l_objStatus;
        }

        public actionStatus SendMatrixCommandByValue(string p_strSelectedLift, string p_strCommand, string p_strDevice, string p_strSource)
        {
            var l_strFunctionName = "SendMatrixCommandByValue()";
            var l_objStatus = actionStatus.None;
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to send the following Command: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
            foreach (var l_objSwitcher in m_objMatrixSwitchers)
            {
                if (l_objSwitcher.SwitcherName == p_strSelectedLift)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Matrix Switcher Located.", l_strFunctionName, p_strSelectedLift));
                    l_objStatus = l_objSwitcher.SendCommandByValueHTTP(p_strCommand, p_strDevice, p_strSource);
                    break;
                }
            }

            switch (l_objStatus)
            {
                case actionStatus.Success:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Successfully Sent: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
                case actionStatus.PartialError:
                case actionStatus.Error:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Not Sent Successfully: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
                case actionStatus.None:
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Command Status Unknown: {2}", l_strFunctionName, p_strSelectedLift, p_strCommand));
                    break;
            }

            return l_objStatus;
        }

        public List<string> ListAvailableCommandsByName(string p_strSelectedTV)
        {
            var l_lstrCommandList = new List<string>();
            var l_strFunctionName = "ListAvailableCommandsByName()";
            
            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Get the available commands for the Television.", l_strFunctionName, p_strSelectedTV));
            foreach (var l_objTelevision in m_objTelevisions)
            {
                if (l_objTelevision.TelevisionName == p_strSelectedTV)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Television Located. Retrieving Commands", l_strFunctionName, p_strSelectedTV));
                    l_lstrCommandList = l_objTelevision.getCommandNames();
                    break;
                }
            }
            return l_lstrCommandList;
        }

        public List<string> ListAvailableProjectorCommandsByName(string p_strSelectedProjector)
        {
            var l_lstrCommandList = new List<string>();
            var l_strFunctionName = "ListAvailableProjectorCommandsByName()";

            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Get the available commands for the Prjector.", l_strFunctionName, p_strSelectedProjector));
            foreach (var l_objProjector in m_objProjectors)
            {
                if (l_objProjector.projectorName == p_strSelectedProjector)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Projector Located. Retrieving Commands", l_strFunctionName, p_strSelectedProjector));
                    l_lstrCommandList = l_objProjector.getCommandNames();
                    break;
                }
            }
            return l_lstrCommandList;
        }

        public List<string> ListAvailableProjectorLiftCommandsByName(string p_strSelectedProjectorLift)
        {
            var l_lstrCommandList = new List<string>();
            var l_strFunctionName = "ListAvailableLiftCommandsByName()";

            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Get the available commands for the Prjector Lift.", l_strFunctionName, p_strSelectedProjectorLift));
            foreach (var l_objLift in m_objProjectorLifts)
            {
                if (l_objLift.LiftName == p_strSelectedProjectorLift)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Projector Lift Located. Retrieving Commands.", l_strFunctionName, p_strSelectedProjectorLift));
                    l_lstrCommandList = l_objLift.getCommandNames();
                    break;
                }
            }
            return l_lstrCommandList;
        }

        public List<string> ListAvailableMatrixCommandsByName(string p_strSelectedMatrixSwitcher)
        {
            var l_lstrCommandList = new List<string>();
            var l_strFunctionName = "ListAvailableMatrixCommandsByName()";

            m_objLogger.logToMemory(string.Format("{0}: {1}: Attempting to Get the available commands for the Matrix Switcher.", l_strFunctionName, p_strSelectedMatrixSwitcher));
            foreach (var l_objSwitcher in m_objMatrixSwitchers)
            {
                if (l_objSwitcher.SwitcherName == p_strSelectedMatrixSwitcher)
                {
                    m_objLogger.logToMemory(string.Format("{0}: {1}: Matrix Switcher Located. Retrieving Commands.", l_strFunctionName, p_strSelectedMatrixSwitcher));
                    l_lstrCommandList = l_objSwitcher.getCommandNames();
                    break;
                }
            }
            return l_lstrCommandList;
        }

        /// <summary>
        /// Obsolete.
        /// This function was used for the original Console application.
        /// use registerTV_Part1(ref bool p_blnPinRequired, string p_strSelectedTV) instead for UI operations.
        /// </summary>
        /// <param name="p_blnPinRequired"></param>
        /// <param name="p_intTVID"></param>
        /// <param name="p_objProgress"></param>
        /// <returns></returns>
        public actionStatus registerTV_Part1(ref bool p_blnPinRequired, int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].registerTV_Step1(ref p_blnPinRequired);

            return l_objStatus;
        }

        public actionStatus registerTV_Part1(ref bool p_blnPinRequired, string p_strSelectedTV)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctonname = "registerTV_Part1";
            try 
            {
                var l_blnTVFound = false;
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if (l_objTelevision.TelevisionName == p_strSelectedTV)
                    {
                        l_blnTVFound = true;
                        l_objStatus = l_objTelevision.registerTV_Step1(ref p_blnPinRequired);
                        break;
                    }
                }
                if (!l_blnTVFound)
                {
                    l_objStatus = actionStatus.Error;
                    m_objLogger.logToMemory("Television " + p_strSelectedTV + " is not configured. Please check the configuration settings.", l_objStatus);
                }
            }
            catch(Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory("Error Registering " + p_strSelectedTV + ": " + e.ToString(), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(p_objStatus: l_objStatus, p_strFunctionName: l_strFunctonname); }
            return l_objStatus;
        }

        /// <summary>
        /// Obsolete.
        /// This function was used for the original Console application.
        /// This Operation did not work during testing.
        /// </summary>
        /// <param name="p_intTVID"></param>
        /// <param name="p_objProgress"></param>
        /// <returns></returns>
        public actionStatus RegisterTV1Step(int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;
            var l_blnSuccess = false;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].RegisterTV1Step(ref l_blnSuccess);

            return l_objStatus;
        }

        /// <summary>
        /// Obsolete.
        /// This function was used for the original Console application.
        /// use registerTV_Part2(string l_strPIN, string p_strSelectedTV) for UI operations.
        /// </summary>
        /// <param name="l_strPIN"></param>
        /// <param name="p_intTVID"></param>
        /// <param name="p_objProgress"></param>
        /// <returns></returns>
        public actionStatus registerTV_Part2(string l_strPIN, int p_intTVID = 1, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;
            var l_blnSuccess = false;

            if (isDeviceConfigured(p_intTVID, DeviceTypes.Television))
                l_objStatus = m_objTelevisions[p_intTVID - 1].registerTV_Step2(l_strPIN, ref l_blnSuccess);

            return l_objStatus;
        }

        public actionStatus registerTV_Part2(string l_strPIN, string p_strSelectedTV)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctonname = "registerTV_Part2";
            var l_blnSuccess = false;

            try
            {
                var l_blnTVFound = false;
                foreach (var l_objTelevision in m_objTelevisions)
                {
                    if (l_objTelevision.TelevisionName == p_strSelectedTV)
                    {
                        l_blnTVFound = true;
                        l_objStatus = l_objTelevision.registerTV_Step2(l_strPIN, ref l_blnSuccess);
                        break;
                    }
                }
                if (!l_blnTVFound)
                {
                    l_objStatus = actionStatus.Error;
                    m_objLogger.logToMemory("Television " + p_strSelectedTV + " is not configured. Please check the configuration settings.", l_objStatus);
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                m_objLogger.logToMemory("Error Sending Auth Code for " + p_strSelectedTV + ": " + e.ToString(), l_objStatus);
            }
            finally { m_objLogger.writePendingToDB(p_objStatus: l_objStatus, p_strFunctionName: l_strFunctonname); }

            return l_objStatus;
        }


        private static bool isDeviceConfigured(int p_intDeviceID, DeviceTypes p_objDeviceType)
        {
            //Short Circuit
            if (p_intDeviceID < 1) return false;

            bool canPerform = false;
            
            //int l_intID = 1; --> Cannot Default because we cannot be sure that the methods calling this function are also going to default to that number

            switch (p_objDeviceType)
            {
                case DeviceTypes.Projector:
                    canPerform = ((m_objProjectors.Count() > 0) && (p_intDeviceID <= m_objProjectors.Count()) && 
                        m_objProjectors[p_intDeviceID - 1].isDeviceConfigured()) ? true : false;
                    break;
                case DeviceTypes.ProjectorLift:
                    canPerform = ((m_objProjectorLifts.Count() > 0) && (p_intDeviceID <= m_objProjectorLifts.Count()) &&
                        m_objProjectorLifts[p_intDeviceID - 1].isDeviceConfigured()) ? true : false;
                    break;
                case DeviceTypes.Television:
                    canPerform = ((m_objTelevisions.Count() > 0) && (p_intDeviceID <= m_objTelevisions.Count()) &&
                        m_objTelevisions[p_intDeviceID - 1].isDeviceConfigured()) ? true : false;
                    break;
                case DeviceTypes.MatrixSwitcher:
                    canPerform = ((m_objMatrixSwitchers.Count() > 0) && (p_intDeviceID <= m_objMatrixSwitchers.Count()) &&
                        m_objMatrixSwitchers[p_intDeviceID - 1].isDeviceConfigured()) ? true : false;
                    break;
            }

            return canPerform;
        }

        public actionStatus changeMatrixSource(int p_intMatrixID ,SwitcherOutput p_objOutput, SwitcherAction p_objAction, StringBuilder p_objProgress = null)
        {
            var l_objStatus = actionStatus.None;
            var l_strFunctionName = "changeMatrixSource";
            bool l_blnLoggingProgress = (p_objProgress != null);

            try
            {
                if (isDeviceConfigured(p_intMatrixID, DeviceTypes.MatrixSwitcher))
                {
                    if (l_blnLoggingProgress) p_objProgress.AppendFormat("Attempting to switch: Source = {0}, Input = {1}{2}", p_objAction, p_objOutput, Environment.NewLine);
                    l_objStatus = m_objMatrixSwitchers[p_intMatrixID - 1].doSwitcherAction(p_objOutput, p_objAction);
                    if (l_blnLoggingProgress) p_objProgress.AppendFormat("Switching Complete{0}", Environment.NewLine);
                    l_objStatus = actionStatus.Success;
                }
            }
            catch (Exception e)
            {
                l_objStatus = actionStatus.Error;
                string exLog = string.Format("An Error Occurred while switching Matrix Source {0} for input {1}: {2}", p_objAction, p_objOutput, e.ToString());
                m_objLogger.logToDB(exLog, actionStatus.Error, p_blnError_WriteMemoryToDB: true);
                p_objProgress.AppendLine(exLog);
            }
            finally { m_objLogger.writePendingToDB(l_objStatus, p_strFunctionName: l_strFunctionName); }

            return l_objStatus;
        }


        #endregion

        #region Old Code()
        /*        private static void loadConfigurations()
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
 
        public async void logToDB(string p_strDescription, actionStatus p_objStatus = actionStatus.None, string p_strUpdatedBy = "GeneralUser")
        {
            //Let's do the work here so as not to repeat ourselves throughout the project
            bool l_blnAddToLog = false;
            if ((m_objLogLevel == logLevel.All) || 
                (m_objLogLevel == logLevel.ErrorOnly && 
                (p_objStatus == actionStatus.PartialError || 
                p_objStatus == actionStatus.Error))) l_blnAddToLog = true;

            if (!(myDB == null) && l_blnAddToLog)
            {
                //Only Set Once - This will help in asyncronously getting logs later
                if (m_cstrSessionID == "") m_cstrSessionID = Guid.NewGuid().ToString();
                myDB.tblOperationStatus.Add(new tblOperationStatu {SessionID = m_cstrSessionID, 
                                                                    StatusID = (int)p_objStatus, 
                                                                    Description = p_strDescription, 
                                                                    UpdatedBy = p_strUpdatedBy, 
                                                                    UpdatedDateTime = DateTime.Now.ToUniversalTime()});
                int x = await myDB.SaveChangesAsync();
            }
        }
         * 
        public string displaySettings()
        {
            //NOTE Logging both to DB and returning the same thing via a string/stringbuilder
            //The Hopes is that we will be able to move to asynchronously reading/writing to the DB for realtime logs
            StringBuilder myOutput = new StringBuilder();
            myOutput.AppendLine("==============CURRENT APPLICATION SETTINGS==============");
            myOutput.AppendLine("-----Televisions-----");
            int i = 0;
            foreach (Television objTempTV in m_objTelevisions)
            {
                i++;
                myOutput.AppendLine("TV #" + i.ToString() + " Information");
                myOutput.AppendLine(objTempTV.GetDeviceInfo());
            }

            i = 0;
            myOutput.AppendLine("-----Projectors-----");
            foreach (Projector objTempProj in m_objProjectors)
            {
                i++;
                myOutput.AppendLine("Projector #" + i.ToString() + " Information");
                myOutput.AppendLine(objTempProj.GetDeviceInfo());
            }

            i = 0;
            myOutput.AppendLine("-----Projector Lifts-----");
            foreach (ProjectorLift objTempLift in m_objProjectorLifts)
            {
                i++;
                myOutput.AppendLine("Lift #" + i.ToString() + " Information");
                myOutput.AppendLine(objTempLift.GetDeviceInfo());
            }
            myOutput.AppendLine("==============END OF APPLICATION SETTINGS===============");

            logToDB(myOutput.ToString());
            return myOutput.ToString();
        }
    */
        #endregion

    }
}
