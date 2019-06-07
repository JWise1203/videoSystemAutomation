using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using khVSAutomation;
using SplashScreen;

using rv;

namespace Visual_Automations
{
    public partial class Form1 : Form
    {
        private static readonly Dictionary<string,string> SwitcherSource = new Dictionary<string,string>() { { "Computer", "1" }, { "Stage HDMI", "2" }, { "Apple TV", "3" }, { "Roku", "4" } };
        private static readonly Dictionary<string, string> SwitcherDevice = new Dictionary<string, string>() { { "Projector", "1" }, { "Library TV", "2" }, { "Office TV", "3" }, { "Auditorium TVs", "4" } };
        private static Automation hallAutomations = null;
        static Thread ms_oThread = null;
        private static Boolean m_blnFrmLoading = false;

        private enum OFFOn
        {
            Off = 0,
            On = 1
        }

        private enum SourceChange
        {
            None = 0,
            AuditoriumTV = 1,
            AuditoriumProjector = 2,
            BackRoomTV = 3,
            All = 99
        }

        private enum PowerChange
        {
            None = 0,
            On = 1,
            Off = 2,
        }

        public Form1()
        {
            try
            {
                InitializeComponent();

                SplashScreen.SplashScreen.ShowSplashScreen();
                Application.DoEvents();
                SplashScreen.SplashScreen.SetStatus("Loading Automation Assemblies and checking DB.");
                System.Threading.Thread.Sleep(100);
                //Set the LogLevel To All For Testing Purposes - We can back it down later
                hallAutomations = new Automation(false, logLevel.All);

                //Wire up the tab control event handler
                tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

                SplashScreen.SplashScreen.SetStatus("Setting Controls for Startup");
                System.Threading.Thread.Sleep(100);
                ConfigureBasic(true);
                //RF Changed to Basic 8/13/2016
                //ConfigureStartUp(true);
                //DisplayMainUserInterface();

                SplashScreen.SplashScreen.SetStatus("Complete");
                System.Threading.Thread.Sleep(100);

                SplashScreen.SplashScreen.CloseForm();
            }
            catch (Exception e)
            {
                MessageBox.Show("An Exception: " + e.ToString());
            }
        }

        #region shared functions and events

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;
            //validate the current page, to cancel the select use:
            switch (current.Name)
            {
                case "tbBasic":
                    ConfigureBasic();
                    break;
                case "tbAdvanced":
                    //Startup is decieving - but the Startup commands were created when this used to be the first tab displayed
                    ConfigureStartUp();
                    break;
                case "tbQuick":
                    ConfigureQuickTab();
                    break;
                case "tbTelevisions":
                    ConfigureTelevisionsTab();
                    break;
                case "tbProjectors":
                    ConfigureProjectorsTab();
                    break;
                case "tbLifts":
                    ConfigureProjectorLiftTab();
                    break;
                case "tbMatrix":
                    ConfigureMatrixTab();
                    break;
                case "tbFeedBack":
                    ConfigureFeedbackTab();
                    break;
                case "tbErrors":
                    ConfigureErrorsTab();
                    break;
            }
        }

        private void LoadMatrixSourceDropDowns()
        {
            var l_selectedTab = tabControl1.SelectedTab.Name;
            var l_objTempDDLItems = new string[SwitcherSource.Keys.Count];
            SwitcherSource.Keys.CopyTo(l_objTempDDLItems, 0);
            string l_strTempSource = "";

            switch (l_selectedTab)
            {
                case "tbBasic":
                    l_strTempSource = ddlBasic_TV_Source.SelectedIndex > -1 ? ddlBasic_TV_Source.SelectedItem.ToString() : "";
                    ddlBasic_TV_Source.Items.Clear();
                    ddlBasic_TV_Source.Items.AddRange(l_objTempDDLItems);
                    if (l_strTempSource.Length > 0)
                    {
                        try { ddlBasic_TV_Source.SelectedItem = l_strTempSource; }
                        catch { ddlBasic_TV_Source.SelectedIndex = -1; }
                    }
                    if (ddlBasic_TV_Source.SelectedIndex == -1)
                    {
                        try { ddlBasic_TV_Source.SelectedItem = "Computer"; }
                        catch { ddlBasic_TV_Source.SelectedIndex = -1; }
                    }
                    break;


                case "tbAdvanced":        
                    l_strTempSource = ddlStart_All_Source.SelectedIndex > -1 ? ddlStart_All_Source.SelectedItem.ToString() : "";
                    ddlStart_All_Source.Items.Clear();
                    ddlStart_All_Source.Items.AddRange(l_objTempDDLItems);
                    if (l_strTempSource.Length > 0)
                    {
                        try { ddlStart_All_Source.SelectedItem = l_strTempSource; }
                        catch { ddlStart_All_Source.SelectedIndex = -1; }
                    }
                    if (ddlStart_All_Source.SelectedIndex == -1)
                    {
                        try { ddlStart_All_Source.SelectedItem = "Computer"; }
                        catch { ddlStart_All_Source.SelectedIndex = -1; }
                    }


                    l_strTempSource = ddlStart_BR_TV_Source.SelectedIndex > -1 ? ddlStart_BR_TV_Source.SelectedItem.ToString() : "";
                    ddlStart_BR_TV_Source.Items.Clear();
                    ddlStart_BR_TV_Source.Items.AddRange(l_objTempDDLItems);
                    if (l_strTempSource.Length > 0)
                    {
                        try { ddlStart_BR_TV_Source.SelectedItem = l_strTempSource; }
                        catch { ddlStart_BR_TV_Source.SelectedIndex = -1; }
                    }
                    if (ddlStart_BR_TV_Source.SelectedIndex == -1)
                    {
                        try { ddlStart_BR_TV_Source.SelectedItem = "Computer"; }
                        catch { ddlStart_BR_TV_Source.SelectedIndex = -1; }
                    }

                    l_strTempSource = ddlStart_MA_Proj_Source.SelectedIndex > -1 ? ddlStart_MA_Proj_Source.SelectedItem.ToString() : "";
                    ddlStart_MA_Proj_Source.Items.Clear();
                    ddlStart_MA_Proj_Source.Items.AddRange(l_objTempDDLItems);
                    if (l_strTempSource.Length > 0)
                    {
                        try { ddlStart_MA_Proj_Source.SelectedItem = l_strTempSource; }
                        catch { ddlStart_MA_Proj_Source.SelectedIndex = -1; }
                    }
                    if (ddlStart_MA_Proj_Source.SelectedIndex == -1)
                    {
                        try { ddlStart_MA_Proj_Source.SelectedItem = "Computer"; }
                        catch { ddlStart_MA_Proj_Source.SelectedIndex = -1; }
                    }

                    l_strTempSource = ddlStart_MA_TV_Source.SelectedIndex > -1 ? ddlStart_MA_TV_Source.SelectedItem.ToString() : "";
                    ddlStart_MA_TV_Source.Items.Clear();
                    ddlStart_MA_TV_Source.Items.AddRange(l_objTempDDLItems);
                    if (l_strTempSource.Length > 0)
                    {
                        try { ddlStart_MA_TV_Source.SelectedItem = l_strTempSource; }
                        catch { ddlStart_MA_TV_Source.SelectedIndex = -1; }
                    }
                    if (ddlStart_MA_TV_Source.SelectedIndex == -1)
                    {
                        try { ddlStart_MA_TV_Source.SelectedItem = "Computer"; }
                        catch { ddlStart_MA_TV_Source.SelectedIndex = -1; }
                    }
                    break;
            }
        }

        private void ConfigureErrorsTab()
        {
            //throw new NotImplementedException();
        }

        private void ConfigureFeedbackTab()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Basic Start Stuff
        private void ConfigureBasic(bool p_blnFirstLoad = false)
        {
            m_blnFrmLoading = true;
            //Not Needed for Basic
            //chkStart_MA_TV_SourceAudio.Checked = p_blnFirstLoad ? p_blnFirstLoad : chkStart_MA_TV_SourceAudio.Checked;
            LoadMatrixSourceDropDowns();
            m_blnFrmLoading = false;
        }

        private void btnBasic_TV_On_Click(object sender, EventArgs e) { basicTV_ONOFF(OFFOn.On); }
        private void btnBasic_TV_Off_Click(object sender, EventArgs e) { basicTV_ONOFF(OFFOn.Off); }
        private void basicTV_ONOFF(OFFOn p_enOffOn)
        {
            SplashScreen.SplashScreen.ShowSplashScreen();
            Application.DoEvents();
            SplashScreen.SplashScreen.SetStatus(string.Format("Turning TV's {0}", (p_enOffOn == OFFOn.On ? "On" : "Off")));
            System.Threading.Thread.Sleep(100);
            var l_strTVNames = new List<string>();
            var l_objProgress = new StringBuilder();
            foreach (var strTV in hallAutomations.AllTelevisions)
            {
                var l_strTempName = strTV.Split(';')[0];
                //For Basic Operations for now we are leaving out the Office TV.
                if (!l_strTempName.Contains("Office")) { l_strTVNames.Add(l_strTempName); }
            }
            SplashScreen.SplashScreen.SetStatus("Sending Command to TVs");
            System.Threading.Thread.Sleep(100);
            if (l_strTVNames.Count > 0)
            {
                if (p_enOffOn == OFFOn.Off)
                {
                    hallAutomations.turnSystemOff(l_strTVNames, new List<string>(), l_objProgress);
                }
                else
                {
                    hallAutomations.turnSystemOn(l_strTVNames, new List<string>(), l_objProgress);
                    basicSourceChange();
                }                                
            }
            SplashScreen.SplashScreen.SetStatus("Complete");
            System.Threading.Thread.Sleep(100);
            SplashScreen.SplashScreen.CloseForm();
        }

        private void ddlBasic_TV_Source_SelectedIndexChanged(object sender, EventArgs e) { basicSourceChange(true); }
        private void basicSourceChange(bool p_blnFromDDL = false)
        {
            if (ddlBasic_TV_Source.SelectedIndex > -1 && !m_blnFrmLoading)
            {
                if (p_blnFromDDL)
                {
                    SplashScreen.SplashScreen.ShowSplashScreen();
                    Application.DoEvents();
                }
                SplashScreen.SplashScreen.SetStatus("Switching TV and Audio Source.");
                System.Threading.Thread.Sleep(100);

                //TODO: There has to be a better way
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Projector"], SwitcherSource[ddlBasic_TV_Source.SelectedItem.ToString()]);
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Auditorium TVs"], SwitcherSource[ddlBasic_TV_Source.SelectedItem.ToString()]);
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Library TV"], SwitcherSource[ddlBasic_TV_Source.SelectedItem.ToString()]);

                SplashScreen.SplashScreen.SetStatus("Done Source Switch");
                System.Threading.Thread.Sleep(100);

                if (p_blnFromDDL) { SplashScreen.SplashScreen.CloseForm(); }
            }
        }

        #endregion
 

        #region Advanced Tab Stuff
        private void ConfigureStartUp(bool p_blnFirstLoad = false)
        {
            chkStart_MA_TV_SourceAudio.Checked = p_blnFirstLoad ? p_blnFirstLoad : chkStart_MA_TV_SourceAudio.Checked;
            LoadMatrixSourceDropDowns();
        }

        private void btnStart_MA_TV_On_Click(object sender, EventArgs e) { advancedTV_Auditorium_ONOFF(OFFOn.On); }
        private void btnStart_MA_TV_Off_Click(object sender, EventArgs e) { advancedTV_Auditorium_ONOFF(OFFOn.Off); }
        private void advancedTV_Auditorium_ONOFF(OFFOn p_enOffOn)
        {
            var l_strTVNames = new List<string>();
            var l_objProgress = new StringBuilder();
            foreach (var strTV in hallAutomations.AllTelevisions)
            {
                var l_strTVName = strTV.Split(';')[0];
                if (l_strTVName.Contains("Auditorium"))
                {
                    l_strTVNames.Add(l_strTVName);
                }
            }

            if (l_strTVNames.Count > 0)
            {
                if (p_enOffOn == OFFOn.Off)
                {
                    hallAutomations.turnSystemOff(l_strTVNames, new List<string>(), l_objProgress);
                }
                else
                {
                    hallAutomations.turnSystemOn(l_strTVNames, new List<string>(), l_objProgress);
                    //Switch the Source
                    if (ddlStart_MA_TV_Source.SelectedIndex > -1)
                        hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Auditorium TVs"], SwitcherSource[ddlStart_MA_TV_Source.SelectedItem.ToString()]);
                }
            }
        }

        //not needed at this time
        private void ddlStart_MA_TV_Source_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnStart_MA_TV_Source_Click(object sender, EventArgs e)
        {
            if (ddlStart_MA_TV_Source.SelectedIndex > -1)
            {
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Auditorium TVs"], SwitcherSource[ddlStart_MA_TV_Source.SelectedItem.ToString()]);

                if (chkStart_MA_TV_SourceAudio.Checked) //Switch the Projector Source so that the Audio can play on the TVs.
                {
                    ddlStart_MA_Proj_Source.SelectedIndex = ddlStart_MA_TV_Source.SelectedIndex;
                    hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Projector"], SwitcherSource[ddlStart_MA_TV_Source.SelectedItem.ToString()]);
                }
            }
        }

        private void btnStart_MA_Proj_On_Click(object sender, EventArgs e) { advancedProj_ONOFF(OFFOn.On); }
        private void btnStart_MA_Proj_Off_Click(object sender, EventArgs e) { advancedProj_ONOFF(OFFOn.Off); }
        private void advancedProj_ONOFF(OFFOn p_enOffOn)
        {
            var l_strProjNames = new List<string>();
            foreach (var strProj in hallAutomations.AllProjectors) l_strProjNames.Add(strProj.Split(';')[0]);
            var l_objProgress = new StringBuilder();
            if (p_enOffOn == OFFOn.Off)
            {
                var l_objStatus = hallAutomations.turnSystemOff(new List<string>(), l_strProjNames, l_objProgress);
            }
            else
            {
                var l_objStatus = hallAutomations.turnSystemOn(new List<string>(), l_strProjNames, l_objProgress);
                if (ddlStart_MA_TV_Source.SelectedIndex > -1)
                {
                    hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Projector"], SwitcherSource[ddlStart_MA_Proj_Source.SelectedItem.ToString()]);
                }                
            }
        }

        private void ddlStart_MA_Proj_Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Not Needed at this time
        }

        private void btnStart_MA_Proj_Source_Click(object sender, EventArgs e)
        {
            if (ddlStart_MA_Proj_Source.SelectedIndex > -1)
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Projector"], SwitcherSource[ddlStart_MA_Proj_Source.SelectedItem.ToString()]);
        }

        private void btnStart_BR_TV_On_Click(object sender, EventArgs e)
        {
            var l_strTVNames = new List<string>();
            var l_objProgress = new StringBuilder();
            foreach (var strTV in hallAutomations.AllTelevisions)
            {
                var l_strTempName = strTV.Split(';')[0];
                if (!l_strTempName.Contains("Auditorium"))
                {
                    if (l_strTempName.Contains("Office")) { if (chkStart_BR_IncOffice.Checked) l_strTVNames.Add(l_strTempName); }
                    else l_strTVNames.Add(l_strTempName);
                }
            }
            if (l_strTVNames.Count > 0)
            {
                hallAutomations.turnSystemOn(l_strTVNames, new List<string>(), l_objProgress);
                changeBRTVSource();
            }
        }

        private void changeBRTVSource()
        {
            if (ddlStart_BR_TV_Source.SelectedIndex > -1)
            {
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Library TV"], SwitcherSource[ddlStart_BR_TV_Source.SelectedItem.ToString()]);
                if (chkStart_BR_IncOffice.Checked)
                    hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Office TV"], SwitcherSource[ddlStart_BR_TV_Source.SelectedItem.ToString()]);
            }
        }

        private void btnStart_BR_TV_Off_Click(object sender, EventArgs e)
        {
            var l_strTVNames = new List<string>();
            var l_objProgress = new StringBuilder();
            foreach (var strTV in hallAutomations.AllTelevisions)
            {
                var l_strTempName = strTV.Split(';')[0];
                if (!l_strTempName.Contains("Auditorium"))
                {
                    if (l_strTempName.Contains("Office")) { if (chkStart_BR_IncOffice.Checked) l_strTVNames.Add(l_strTempName); }
                    else l_strTVNames.Add(l_strTempName);
                }
            }
            if (l_strTVNames.Count > 0)
                hallAutomations.turnSystemOff(l_strTVNames, new List<string>(), l_objProgress);
        }

        private void ddlStart_BR_TV_Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Not Needed at this time
        }

        private void btnStart_BR_TV_Source_Click(object sender, EventArgs e)
        {
            changeBRTVSource();
        }

        private void btnStart_All_On_Click(object sender, EventArgs e)
        {
            var l_strTVNames = new List<string>();
            foreach (var strTV in hallAutomations.AllTelevisions) l_strTVNames.Add(strTV.Split(';')[0]);
            var l_strProjNames = new List<string>();
            foreach (var strProj in hallAutomations.AllProjectors) l_strProjNames.Add(strProj.Split(';')[0]);
            var l_objProgress = new StringBuilder();
            hallAutomations.turnSystemOn(l_strTVNames, l_strProjNames, l_objProgress);
            changeAllSources();


            //using (var pleaseWait = new PleaseWait(DoChanges(l_strTVNames, l_strProjNames, PowerChange.On, SourceChange.All);))
            //   pleaseWait.ShowDialog(this);
        }

        private void DoChanges(List<string> p_strTV, List<string> p_strProj, PowerChange p_objPChange = PowerChange.None, SourceChange p_objSourceChange = SourceChange.None)
        {
            var l_sbOldLog = new StringBuilder();
            switch (p_objPChange)
            {
                case PowerChange.On :
                    hallAutomations.turnSystemOn(p_strTV, p_strProj, l_sbOldLog);
                    break;
                case PowerChange.Off :
                    hallAutomations.turnSystemOff(p_strTV, p_strProj, l_sbOldLog);
                    break;
            }
            switch (p_objSourceChange)
            {
                case SourceChange.All :
                    changeAllSources();
                    break;
                case SourceChange.AuditoriumTV:
                    break;
                case SourceChange.AuditoriumProjector:
                    break;
                case SourceChange.BackRoomTV:
                    break;
            }
        }

        private void changeAllSources()
        {
            if (ddlStart_All_Source.SelectedIndex > -1)
            {
                //Set them all the same - This can be done since the lists are all the same - If the lists differ in the future more logic will be needed.
                ddlStart_BR_TV_Source.SelectedIndex = ddlStart_MA_Proj_Source.SelectedIndex = ddlStart_MA_TV_Source.SelectedIndex = ddlStart_All_Source.SelectedIndex;

                var l_SourceNum = SwitcherSource[ddlStart_All_Source.SelectedItem.ToString()];
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Projector"], l_SourceNum);
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Library TV"], l_SourceNum);
                if (chkStart_BR_IncOffice.Checked)
                    hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Office TV"], l_SourceNum);
                hallAutomations.SendMatrixCommandByValue(hallAutomations.AllSwitchers[0].Split(';')[0], SwitcherDevice["Auditorium TVs"], l_SourceNum);
            }
        }

        private void btnStart_All_Off_Click(object sender, EventArgs e)
        {
            var l_strTVNames = new List<string>();
            foreach (var strTV in hallAutomations.AllTelevisions) l_strTVNames.Add(strTV.Split(';')[0]);
            var l_strProjNames = new List<string>();
            foreach (var strProj in hallAutomations.AllProjectors) l_strProjNames.Add(strProj.Split(';')[0]);
            var l_objProgress = new StringBuilder(); //Need to find another way.
            hallAutomations.turnSystemOff(l_strTVNames, l_strProjNames, l_objProgress);
        }

        private void ddlStart_All_Source_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnStart_All_Source_Click(object sender, EventArgs e) { changeAllSources(); }

        #endregion


        #region Matrix Switcher Stuff
        private void ConfigureMatrixTab()
        {
            ddlSwitchers.SelectedIndex = -1;
            ddlSwitchers.Items.Clear();
            lstSwitcherCommands.SelectedIndex = -1;
            lstSwitcherCommands.Items.Clear();
            btnExecuteSwitcherCommand.Enabled = false;
            gbSwitcherCommands.Visible = false;
            btnSendRawSwitcherCommand.Enabled = false;
            txtRawSwitcherCommand.Text = "";
            txtRawSwitcherCommand.Enabled = false;
            txtRawSwitcherDevice.Text = "";
            txtRawSwitcherDevice.Enabled = false;
            txtRawSwitcherSource.Text = "";
            txtRawSwitcherSource.Enabled = false;

            ConfigureSwitchers();
        }


        private void ConfigureSwitchers()
        {
            if (hallAutomations.NumberOfSwitchers > 0)
            {
                foreach (var strSW in hallAutomations.AllSwitchers) ddlSwitchers.Items.Add(strSW.Split(';')[0]);
                if (ddlSwitchers.Items.Count == 1)
                    ddlSwitchers.SelectedIndex = 0;
                ConfigureSwitcherCommands();
            }
        }

        private void ConfigureSwitcherCommands(bool p_blnActionListOnly = false)
        {
            //Don't care about deselect
            if (ddlSwitchers.SelectedIndex > -1)
            {
                if (!p_blnActionListOnly)
                {
                    txtRawSwitcherCommand.Enabled = true;
                    txtRawSwitcherDevice.Enabled = true;
                    txtRawSwitcherSource.Enabled = true;
                    btnSendRawSwitcherCommand.Enabled = true;
                }
                gbSwitcherCommands.Visible = true;

                //fill in the List of commands
                lstSwitcherCommands.SelectedIndex = -1;
                lstSwitcherCommands.Items.Clear();
                var l_lstrCommandNames = hallAutomations.ListAvailableMatrixCommandsByName(ddlSwitchers.SelectedItem.ToString());
                foreach (var l_strCommandDisplayName in l_lstrCommandNames) lstSwitcherCommands.Items.Add(l_strCommandDisplayName);
            }
            else
            {
                if (!p_blnActionListOnly)
                {
                    txtRawSwitcherCommand.Text = "";
                    txtRawSwitcherCommand.Enabled = false;
                    txtRawSwitcherDevice.Text = "";
                    txtRawSwitcherDevice.Enabled = false;
                    txtRawSwitcherSource.Text = "";
                    txtRawSwitcherSource.Enabled = false;
                    btnSendRawSwitcherCommand.Enabled = false;
                }
            }

            if (lstSwitcherCommands.SelectedIndex == -1)
                btnExecuteSwitcherCommand.Enabled = false;
        }

        private void executeSwitcherCommand()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            var l_objStatus = actionStatus.None;
            var l_strCommand = lstSwitcherCommands.SelectedItem.ToString();
            var l_strSwitcherName = ddlSwitchers.SelectedItem.ToString();

            try { l_objStatus = hallAutomations.SendMatrixCommand(l_strSwitcherName, l_strCommand); }
            catch { l_objStatus = actionStatus.Error; }
            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("Command: " + l_strCommand + " has completed.");
                    break;
                default:
                    MessageBox.Show("Command: " + l_strCommand + " has NOT completed Successfully. Please check the logs for more details OR try again");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
        }
        private void ddlSwitchers_SelectedIndexChanged(object sender, EventArgs e) { ConfigureSwitcherCommands(); }

        private void lstSwitcherCommands_SelectedIndexChanged(object sender, EventArgs e) { btnExecuteSwitcherCommand.Enabled = lstSwitcherCommands.SelectedIndex > -1 ? true : false; }

        private void btnExecuteSwitcherCommand_Click(object sender, EventArgs e) { executeSwitcherCommand(); }

        private void btnSendRawSwitcherCommand_Click(object sender, EventArgs e)
        {
            if (txtRawSwitcherCommand.Text.Length > 0 && txtRawSwitcherDevice.Text.Length > 0 && txtRawSwitcherSource.Text.Length > 0)
                hallAutomations.SendMatrixCommandByValue(ddlSwitchers.SelectedItem.ToString(), txtRawSwitcherDevice.Text, txtRawSwitcherSource.Text, txtRawSwitcherCommand.Text);
        }
        #endregion

        #region Projector Lift Command Stuff
        private void ConfigureProjectorLiftTab()
        {
            ddlLift.SelectedIndex = -1;
            ddlLift.Items.Clear();
            lstLiftCommands.SelectedIndex = -1;
            lstLiftCommands.Items.Clear();
            btnExecuteLiftCommand.Enabled = false;
            gbLiftCommands.Visible = false;
            btnSendRawLiftCommand.Enabled = false;
            txtRawLiftCommand.Text = "";
            txtRawLiftCommand.Enabled = false;

            ConfigureLifts();
        }

        private void ConfigureLifts()
        {
            if (hallAutomations.NumberOfProjectorLifts > 0)
            {
                foreach (var strLift in hallAutomations.AllProjectorLifts) ddlLift.Items.Add(strLift.Split(';')[0]);
                if (ddlLift.Items.Count == 1)
                    ddlLift.SelectedIndex = 0;
                ConfigureProjectorCommands();
            }
        }

        private void ConfigureLiftCommands(bool p_blnActionListOnly = false)
        {
            //Don't care about deselect
            if (ddlLift.SelectedIndex > -1)
            {
                if (!p_blnActionListOnly)
                {
                    txtRawLiftCommand.Enabled = true;
                    btnSendRawLiftCommand.Enabled = true;
                }
                gbLiftCommands.Visible = true;

                //fill in the List of commands
                lstLiftCommands.SelectedIndex = -1;
                lstLiftCommands.Items.Clear();
                var l_lstrCommandNames = hallAutomations.ListAvailableProjectorLiftCommandsByName(ddlLift.SelectedItem.ToString());
                foreach (var l_strCommandDisplayName in l_lstrCommandNames) lstLiftCommands.Items.Add(l_strCommandDisplayName);
            }
            else
            {
                if (!p_blnActionListOnly)
                {
                    txtRawLiftCommand.Text = "";
                    txtRawLiftCommand.Enabled = false;
                    btnSendRawLiftCommand.Enabled = false;
                }
            }

            if (lstLiftCommands.SelectedIndex == -1)
                btnExecuteLiftCommand.Enabled = false;
        }

        private void executeLiftCommand()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            var l_objStatus = actionStatus.None;
            var l_strCommand = lstLiftCommands.SelectedItem.ToString();
            var l_strLiftName = ddlLift.SelectedItem.ToString();

            try { l_objStatus = hallAutomations.SendLiftCommand(l_strLiftName, l_strCommand); }
            catch { l_objStatus = actionStatus.Error; }
            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("Command: " + l_strCommand + " has completed.");
                    break;
                default:
                    MessageBox.Show("Command: " + l_strCommand + " has NOT completed Successfully. Please check the logs for more details OR try again");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
        }

        private void ddlLift_SelectedIndexChanged(object sender, EventArgs e) { ConfigureLiftCommands(); }
        private void lstLiftCommands_SelectedIndexChanged(object sender, EventArgs e) { btnExecuteLiftCommand.Enabled = lstLiftCommands.SelectedIndex > -1 ? true : false; }
        private void btnExecuteLiftCommand_Click(object sender, EventArgs e) { executeLiftCommand(); }
        private void btnSendRawLiftCommand_Click(object sender, EventArgs e)
        {
            if (txtRawLiftCommand.Text.Length > 0) hallAutomations.SendLiftCommandByValue(ddlLift.SelectedItem.ToString(), txtRawLiftCommand.Text);
        }
        #endregion

        #region Projector Command Stuff

        private void ConfigureProjectorsTab()
        {
            ddlProjectors.SelectedIndex = -1;
            ddlProjectors.Items.Clear();
            lstProjectorCommands.SelectedIndex = -1;
            lstProjectorCommands.Items.Clear();
            btnExecuteProjectorCommand.Enabled = false;
            gbProjectorCommands.Visible = false;
            btnSendRawProjectorCommand.Enabled = false;
            txtRawProjectorCommand.Text = "";
            txtRawProjectorCommand.Enabled = false;

            ConfigureProjectors();
        }

        private void ConfigureProjectors()
        {
            if (hallAutomations.NumberOfProjectors > 0)
            {
                foreach (var strProj in hallAutomations.AllProjectors) ddlProjectors.Items.Add(strProj.Split(';')[0]);
                if (ddlProjectors.Items.Count == 1)
                    ddlProjectors.SelectedIndex = 0;
                ConfigureProjectorCommands();
            }
        }

        private void ConfigureProjectorCommands(bool p_blnActionListOnly = false)
        {
            //Don't care about deselect
            if (ddlProjectors.SelectedIndex > -1)
            {
                if (!p_blnActionListOnly)
                {
                    txtRawProjectorCommand.Enabled = true;
                    btnSendRawProjectorCommand.Enabled = true;
                }
                gbProjectorCommands.Visible = true;

                //fill in the List of commands
                lstProjectorCommands.SelectedIndex = -1;
                lstProjectorCommands.Items.Clear();
                var l_lstrCommandNames = hallAutomations.ListAvailableProjectorCommandsByName(ddlProjectors.SelectedItem.ToString());
                foreach (var l_strCommandDisplayName in l_lstrCommandNames) lstProjectorCommands.Items.Add(l_strCommandDisplayName);
            }
            else
            {
                if (!p_blnActionListOnly)
                {
                    txtRawProjectorCommand.Text = "";
                    txtRawProjectorCommand.Enabled = false;
                    btnSendRawProjectorCommand.Enabled = false;
                }
            }

            if (lstProjectorCommands.SelectedIndex == -1)
                btnExecuteProjectorCommand.Enabled = false;
        }

        private void executeProjectorCommand()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            var l_objStatus = actionStatus.None;
            var l_strCommand = lstProjectorCommands.SelectedItem.ToString();
            var l_strProjectorName = ddlProjectors.SelectedItem.ToString();

            try { l_objStatus = hallAutomations.SendProjectorCommand(l_strProjectorName, l_strCommand); }
            catch { l_objStatus = actionStatus.Error; }
            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("Command: " + l_strCommand + " has completed.");
                    break;
                default:
                    MessageBox.Show("Command: " + l_strCommand + " has NOT completed Successfully. Please check the logs for more details OR try again");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
        }

        private void ddlProjectors_SelectedIndexChanged(object sender, EventArgs e) { ConfigureProjectorCommands(); }

        private void btnExecuteProjectorCommand_Click(object sender, EventArgs e) { executeProjectorCommand(); }

        private void btnSendRawProjectorCommand_Click(object sender, EventArgs e)
        {
            if (txtRawProjectorCommand.Text.Length > 0) hallAutomations.SendProjectorCommandByValue(ddlProjectors.SelectedItem.ToString(), txtRawProjectorCommand.Text);
        }
        private void lstProjectorCommands_SelectedIndexChanged(object sender, EventArgs e) { btnExecuteProjectorCommand.Enabled = lstProjectorCommands.SelectedIndex > -1 ? true : false; }

        #endregion

        #region Television Commands Stuff
        private void ConfigureTelevisionsTab()
        {
            ddlTelevisions.SelectedIndex = -1;
            ddlTelevisions.Items.Clear();
            lstTVCommands.SelectedIndex = -1;
            lstTVCommands.Items.Clear();
            btnExecuteCommand.Enabled = false;
            btnTVRegistration.Enabled = false;
            gbTVRegistration.Visible = false;
            gbTVCommands.Visible = false;
            btnSendRawCommand.Enabled = false;
            txtRawCommand.Text = "";
            txtRawCommand.Enabled = false;

            ConfigureTelevisions();
        }

        //private static int m_intPreviousSelection
        private void ConfigureTelevisions()
        {
            if (hallAutomations.NumberOfTelevisions > 0)
            {
                foreach (var strTV in hallAutomations.AllTelevisions) ddlTelevisions.Items.Add(strTV.Split(';')[0]);
                ConfigureTelevisionCommands();
            }
        }
        private void ConfigureTelevisionCommands(bool p_blnActionListOnly = false)
        {
            //Don't care about deselect
            if (ddlTelevisions.SelectedIndex > -1)
            {
                if (!p_blnActionListOnly)
                {
                    gbTVRegistration.Visible = true;
                    btnTVRegistration.Enabled = true;
                    txtRawCommand.Enabled = true;
                    btnSendRawCommand.Enabled = true;
                }
                gbTVCommands.Visible = true;            

                //fill in the List of commands
                lstTVCommands.SelectedIndex = -1;
                lstTVCommands.Items.Clear();

                //These are now filled in the back end - we will take what we get.
                ////Power Commands will always be available since these have specific Functions
                //lstTVCommands.Items.Add("Power On");
                //lstTVCommands.Items.Add("Power Off");
                var l_lstrCommandNames = hallAutomations.ListAvailableCommandsByName(ddlTelevisions.SelectedItem.ToString());
                foreach (var l_strCommandDisplayName in l_lstrCommandNames) lstTVCommands.Items.Add(l_strCommandDisplayName);
            }
            else 
            { 
                if (!p_blnActionListOnly)
                {
                    btnTVRegistration.Enabled = false;
                    txtRawCommand.Text = "";
                    txtRawCommand.Enabled = false;
                    btnSendRawCommand.Enabled = false;
                }
            }

            if (lstTVCommands.SelectedIndex == -1)
                btnExecuteCommand.Enabled = false;
            if (!p_blnActionListOnly)
            {
                gbTVAuth.Visible = false;
                txtPIN.Text = "";
            }
        }

        private void lstTVCommands_SelectedIndexChanged(object sender, EventArgs e) { btnExecuteCommand.Enabled = lstTVCommands.SelectedIndex > -1 ? true : false; }
        private void btnTVRegistration_Click(object sender, EventArgs e) { if(ddlTelevisions.SelectedIndex > -1) RegisterTV(); }
        private void btnSendAuth_Click(object sender, EventArgs e) { SendAuthorization(); }
        private void SendAuthorization()
        {
            
            if (txtPIN.Text.Trim() == "")
            {
                MessageBox.Show("Cancelling The Submit PIN operation as no PIN was entered");
            }
            else
            {
                var l_strSelectedTV = ddlTelevisions.SelectedItem.ToString();
                if (l_strSelectedTV.Length > 0)
                {
                    StringBuilder l_objProgress = new StringBuilder();
                    var l_objStatus = hallAutomations.registerTV_Part2(txtPIN.Text.Trim(), l_strSelectedTV);

                    switch (l_objStatus)
                    {
                        case actionStatus.PartialError:
                            break;
                        case actionStatus.Error:
                            MessageBox.Show("An error occurred while registering the TV. If the TV does not function as expected, please check the logs OR try again.");
                            break;
                        case actionStatus.Success:
                            MessageBox.Show("Registration Completed!");
                            break;
                    }
                    ConfigureTelevisionCommands(true); //Update What's available now.
                    txtPIN.Text = "";
                    gbTVAuth.Visible = false;
                    //Console.Write(l_objProgress.ToString());
                }
                else { MessageBox.Show("There was an issue sending the registration command for the selected Television"); }
            }
            
        }
        private void RegisterTV()
        {
            var l_strSelectedTV = ddlTelevisions.SelectedItem.ToString();

            if (l_strSelectedTV.Length > 0)
            {
                var l_blnPinRequired = true;
                StringBuilder l_objProgress = new StringBuilder();
                var l_objStatus = hallAutomations.registerTV_Part1(ref l_blnPinRequired, l_strSelectedTV);

                if (l_objStatus == actionStatus.Success && l_blnPinRequired)
                {
                    l_objProgress.Clear();
                    txtPIN.Text = "";
                    //btnSendAuth.Enabled = false;
                    gbTVAuth.Visible = true;
                }
                else { 
                    gbTVAuth.Visible = false;
                    ConfigureTelevisionCommands(true); //Update What's available now.
                    MessageBox.Show("Registration Completed!"); 
                }
            }
            else { MessageBox.Show("There was an issue sending the registration command for the selected Television"); }
        }
        private void btnExecuteCommand_Click(object sender, EventArgs e) { executeTVCommand(); }
        private void executeTVCommand()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            var l_objStatus = actionStatus.None;
            var l_strCommand = lstTVCommands.SelectedItem.ToString();
            var l_strTVName = ddlTelevisions.SelectedItem.ToString();

            try
            {
                switch (l_strCommand)
                {
                    //should not be needed anymore as the Television Class should be able to handle all types of these commands
                    //case "Power On":
                    //    l_objStatus = hallAutomations.PowerOnSingleTV(l_strTVName);
                    //    break;
                    //case "Power Off":
                    //    l_objStatus = hallAutomations.PowerOffSingleTV(l_strTVName);
                    //    break;
                    default:
                        l_objStatus = hallAutomations.SendTVCommand(l_strTVName, l_strCommand);
                        break;
                }
            }
            catch
            {
                l_objStatus = actionStatus.Error;
            }
            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("Command: " + l_strCommand + " has completed.");
                    break;
                default:
                    MessageBox.Show("Command: " + l_strCommand + " has NOT completed Successfully. Please check the logs for more details OR try again");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
        }

        private void btnSendRawCommand_Click(object sender, EventArgs e)
        {
            if (txtRawCommand.Text.Length > 0) hallAutomations.SendCommandByValue(ddlTelevisions.SelectedItem.ToString(), txtRawCommand.Text);
        }

        private void ddlTelevisions_SelectedIndexChanged(object sender, EventArgs e) { ConfigureTelevisionCommands(); }
        
        #endregion

        #region Quick Tab Stuff
        private void ConfigureQuickTab()
        {
            //Clear the Selections first
            foreach (Control c in gbTVs.Controls) { if (c is CheckBox) { var cboDynamic = (CheckBox)c; cboDynamic.Checked = false; cboDynamic.Visible = false; } }
            foreach (Control c in gbProjectors.Controls) { if (c is CheckBox) {var cboDynamic = (CheckBox)c; cboDynamic.Checked = false; cboDynamic.Visible = false;} }
            gbTVs.Visible = false;
            gbProjectors.Visible = false;
            gbCommands.Visible = false;

            //Now set things back to visible
            var blnTelevisions = false;
            if (hallAutomations.NumberOfTelevisions > 0)
            {
                blnTelevisions = true;
                gbTVs.Visible = true;

                foreach (var strTV in hallAutomations.AllTelevisions)
                {
                    string[] aryProps = strTV.Split(';');
                    foreach (Control c in gbTVs.Controls)
                    {
                        if (c is CheckBox)
                        {
                            var cboDynamic = (CheckBox)c;
                            var strCBOName = cboDynamic.Name;
                            var strCBOID = strCBOName.Substring(strCBOName.Length - 1);
                            if (strCBOID == aryProps[1])
                            {
                                cboDynamic.Text = aryProps[0];
                                cboDynamic.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
            
            var blnProjectors = false;
            if(hallAutomations.NumberOfProjectors > 0)
            {
                blnProjectors = true;
                gbProjectors.Visible = true;
                foreach (var strProj in hallAutomations.AllProjectors)
                {
                    string[] aryProps = strProj.Split(';');
                    foreach (Control c in gbProjectors.Controls)
                    {
                        if (c is CheckBox)
                        {
                            var cboDynamic = (CheckBox)c;
                            var strCBOName = cboDynamic.Name;
                            var strCBOID = strCBOName.Substring(strCBOName.Length - 1);
                            if (strCBOID == aryProps[1])
                            {
                                cboDynamic.Text = aryProps[0];
                                cboDynamic.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (blnTelevisions || blnProjectors) gbCommands.Visible = true;
        }

        private void btnOn_Click(object sender, EventArgs e) { turnOnSystem(); }
        private void btnOff_Click(object sender, EventArgs e) { turnOffSystem(); }
        private void turnOnSystem()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            //TODO: NEED TO CREATE A MESSAGEBUS OR SOMETHING TO GIVE THE USER REALTIME UPDATES
            StringBuilder l_objProgress = new StringBuilder();
            actionStatus l_objStatus = actionStatus.None;

            var strTVsOn = new List<string>();
            foreach (Control c in gbTVs.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strTVsOn.Add(cboDynamic.Text);
                }
            }

            var strProjsOn = new List<string>();
            foreach (Control c in gbProjectors.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strProjsOn.Add(cboDynamic.Text);
                }
            }

            if (strProjsOn.Count == 0 && strTVsOn.Count == 0) MessageBox.Show("No devices were selected and therefore This action will not be performed");
            else l_objStatus = hallAutomations.turnSystemOn(strTVsOn, strProjsOn, l_objProgress);
            //Console.Write(l_objProgress.ToString());

            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("The System has been turned on successfully.");
                    break;
                default:
                    MessageBox.Show("Something went wrong. Please check the logs for more details.\nIf this issue persists, please contact the system administrator");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
        }
        private void turnOffSystem()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            //TODO: NEED TO CREATE A MESSAGEBUS OR SOMETHING TO GIVE THE USER REALTIME UPDATES
            System.Threading.Thread.Sleep(5000);
            StringBuilder l_objProgress = new StringBuilder();
            actionStatus l_objStatus = actionStatus.None;

            var strTVsOff = new List<string>();
            foreach (Control c in gbTVs.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strTVsOff.Add(cboDynamic.Text);
                }
            }

            var strProjsOff = new List<string>();
            foreach (Control c in gbProjectors.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strProjsOff.Add(cboDynamic.Text);
                }
            }

            if (strProjsOff.Count == 0 && strTVsOff.Count == 0) MessageBox.Show("No devices were selected and therefore This action will not be performed");
            else l_objStatus = hallAutomations.turnSystemOff(strTVsOff, strProjsOff, l_objProgress);
            //Console.Write(l_objProgress.ToString());

            switch (l_objStatus)
            {
                case actionStatus.None:
                case actionStatus.Success:
                    MessageBox.Show("The System has been successfully turned off.");
                    break;
                default:
                    MessageBox.Show("Something went wrong. Please check the logs for more details.\nIf this issue persists, please contact the system administrator");
                    break;
            }
            foreach (Control c in this.Controls) { c.Enabled = true; }
            
            Console.Write(l_objProgress.ToString());
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Matrix Commander Test Code - Working
        /*
        private void btnSendHTTPCommand_Click(object sender, EventArgs e)
        {
            if (txtSWCommand.Text.Length > 0 && txtSWOutput.Text.Length > 0 && txtSWSource.Text.Length > 0)
            {
                TestMatrixQuick(txtSWCommand.Text, txtSWOutput.Text, txtSWSource.Text);
            }
        }
        static void TestMatrixQuick(string p_strCommand, string p_strOutput, string p_strSource)
        {
            m_intCheckSum = Int32.Parse(p_strCommand) + Int32.Parse(p_strOutput) + Int32.Parse(p_strSource);
            // Create a request for the URL. 
            var l_strTestResult = "";
            try
            {
                WebRequest request = WebRequest.Create(string.Format(" http://192.168.1.88/switch.cgi?command={0}&data0={1}&data1={2}&checksum={3}", p_strCommand, p_strOutput, p_strSource, m_intCheckSum.ToString()));
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                l_strTestResult = ((HttpWebResponse)response).StatusDescription;
                response.Close();
            }
            catch (Exception e)
            {

            }
            if (l_strTestResult.Length > 0) { MessageBox.Show("Response received: " + l_strTestResult); }
            else { MessageBox.Show("No Response received"); }


        }
        */
        #endregion

        #region SplashScreen - Operations
        

        #endregion
    }
}
