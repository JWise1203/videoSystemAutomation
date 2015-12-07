using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

using rv;

namespace Visual_Automations
{
    public partial class Form1 : Form
    {
        static DateTime StartTime;

        private static PJLinkConnection c = null;
        private static Automation hallAutomations = null;

        public Form1()
        {
            InitializeComponent();

            StartTime = DateTime.Now;

            //Set the LogLevel To All For Testing Purposes - We can back it down later
            hallAutomations = new Automation(false, logLevel.All);

            //tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

            ConfigureQuickTab();
            //DisplayMainUserInterface();
        }

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;
            //validate the current page, to cancel the select use:
            switch (current.Name)
            {
                case "tbQuick":
                    ConfigureQuickTab();
                    break;
                case "tbTelevisions":
                    ConfigureTelevisionsTab();
                    break;
                case "tbProjectors":
                    ConfigureProjectorsTab();
                    break;
                case "tbFeedBack":
                    ConfigureFeedbackTab();
                    break;
                case "tbErrors":
                    ConfigureErrorsTab();
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

        private void ConfigureProjectorsTab()
        {
            //throw new NotImplementedException();
        }

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

            ConfigureTelevisions();
        }

        //private static int m_intPreviousSelection
        private void ddlTelevisions_SelectedIndexChanged(object sender, EventArgs e) { ConfigureTelevisionCommands(); }
        private void ConfigureTelevisions()
        {
            if (hallAutomations.NumberOfTelevisions > 0)
            {
                foreach (var strTV in hallAutomations.AllTelevisions.Split('|'))
                {
                    string[] aryProps = strTV.Split(';');
                    ddlTelevisions.Items.Add(aryProps[0]);
                }
                ConfigureTelevisionCommands();
            }
        }
        private void ConfigureTelevisionCommands()
        {
            //Don't care about deselect
            if (ddlTelevisions.SelectedIndex > -1)
            {
                //TODO: GET THE LIST OF COMMANDS THAT CAN BE EXECUTED HERE
                gbTVCommands.Visible = true;
                gbTVRegistration.Visible = true;
                btnTVRegistration.Enabled = true;

                //fill in the List of commands
                lstTVCommands.SelectedIndex = -1;
                lstTVCommands.Items.Clear();

                //Power Commands will always be available since these have specific Functions
                lstTVCommands.Items.Add("Power On");
                lstTVCommands.Items.Add("Power Off");

                //Fill in the other commands via the availability list.
            }
            else { btnTVRegistration.Enabled = false; }

            btnExecuteCommand.Enabled = false;
            gbTVAuth.Visible = false;
            txtPIN.Text = "";
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
                        case actionStatus.Error:
                            MessageBox.Show("An error occurred while registering the TV. If the TV does not function as expected, please check the logs OR try again.");
                            break;
                        case actionStatus.Success:
                            MessageBox.Show("Registration Completed!");
                            break;
                    }
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
                else { gbTVAuth.Visible = false; MessageBox.Show("Registration Completed!"); }
            }
            else { MessageBox.Show("There was an issue sending the registration command for the selected Television"); }
        }
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

                foreach (var strTV in hallAutomations.AllTelevisions.Split('|'))
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
                foreach (var strProj in hallAutomations.AllProjectors.Split('|'))
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
                    case "Power On":
                        l_objStatus = hallAutomations.PowerOnSingleTV(l_strTVName);
                        break;
                    case "Power Off":
                        l_objStatus = hallAutomations.PowerOffSingleTV(l_strTVName);
                        break;
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
        private void turnOnSystem()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            //TODO: NEED TO CREATE A MESSAGEBUS OR SOMETHING TO GIVE THE USER REALTIME UPDATES
            StringBuilder l_objProgress = new StringBuilder();
            actionStatus l_objStatus = actionStatus.None;

            var strTVsOn = "";
            foreach (Control c in gbTVs.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strTVsOn += cboDynamic.Text + "|";
                }
            }
            strTVsOn = (strTVsOn.Length > 0 ? strTVsOn.Substring(0, strTVsOn.Length - 1) : "");

            var strProjsOn = "";
            foreach (Control c in gbProjectors.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strProjsOn += cboDynamic.Text + "|";
                }
            }
            strProjsOn = (strProjsOn.Length > 0 ? strProjsOn.Substring(0, strProjsOn.Length - 1) : "");

            if (strProjsOn == "" && strTVsOn == "") MessageBox.Show("No devices were selected and therefore This action will not be performed");
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

            var strTVsOff = "";
            foreach (Control c in gbTVs.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strTVsOff += cboDynamic.Text + "|";
                }
            }
            strTVsOff = (strTVsOff.Length > 0 ? strTVsOff.Substring(0, strTVsOff.Length - 1) : "");

            var strProjsOff = "";
            foreach (Control c in gbProjectors.Controls)
            {
                if (c is CheckBox)
                {
                    var cboDynamic = (CheckBox)c;
                    if (cboDynamic.Checked) strProjsOff += cboDynamic.Text + "|";
                }
            }
            strProjsOff = (strProjsOff.Length > 0 ? strProjsOff.Substring(0, strProjsOff.Length - 1) : "");

            if (strProjsOff == "" && strTVsOff == "") MessageBox.Show("No devices were selected and therefore This action will not be performed");
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

        private static void switchMatrixSource(SwitcherOutput p_objOutput, SwitcherAction p_objSource)
        {
            StringBuilder l_objProgress = new StringBuilder();
            MessageBox.Show("Attempting to Switch Source");
            if (hallAutomations.NumberOfSwitchers > 0)
            {
                var l_objStatus = hallAutomations.changeMatrixSource(1, p_objOutput, p_objSource, l_objProgress);
                Console.Write(l_objProgress.ToString());

                switch (l_objStatus)
                {
                    case actionStatus.None:
                    case actionStatus.Success:
                        MessageBox.Show("The Source has been successfully Been Switched.");
                        break;
                    default:
                        MessageBox.Show("Something went wrong. Please check the logs for more details.\nIf this issue persists, please contact the system administrator");
                        break;
                }
            }
            else
                MessageBox.Show("Action Cannot be Performed: There are No Matrix Switchers Configured!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtRawCommand.Text.Length > 0) hallAutomations.TestCommands(ddlTelevisions.SelectedItem.ToString(), txtRawCommand.Text);
        }
        
        /*
         private static void OperationLogicProcessor(String operation)
         {
             try
             {
                 switch (operation)
                 {
                     case "C":
                         Console.Write(hallAutomations.printProjectorStatus());

                         RunAnotherOperation();
                         break;
                     case "H":
                         List<string> l_strAvailTVs3 = hallAutomations.getAvailableDeviceInfo(DeviceTypes.Television);

                         if (l_strAvailTVs3.Count > 0)
                         {
                             foreach (string l_strTVInfo in l_strAvailTVs3)
                             {
                                 string[] l_astrTVInfo = l_strTVInfo.Split('|');
                                 Console.WriteLine("Enter {0} for {1}", l_astrTVInfo[0], l_astrTVInfo[1]);
                             }
                             Console.WriteLine("");
                             int iTVID = Int32.Parse(Console.ReadLine());
                             Console.WriteLine("Please Paste Command");
                             string strCommand = Console.ReadLine();
                             StringBuilder l_objProgress = new StringBuilder();

                             var l_objStatus = hallAutomations.TestCommands(strCommand, iTVID,l_objProgress);
                             Console.Write(l_objProgress.ToString());
                         }
                         else Console.WriteLine("Registration Cancelled - There are currently no Televisions setup in the configuration file.");

                         RunAnotherOperation();


                          for (int i = 1; i <= hallAutomations.NumberOfTelevisions; i++)
                         {
                             if (i == 1)
                             {
                                 Console.Write("Attempting to power off tv " + i);
                                 Console.Write(hallAutomations.PowerOffTV(i));
                             }
                         }
                         RunAnotherOperation();
                         break;
                     case "X" :
                         var l_intOutput = -1;
                         do
                         {
                             Console.Write("Please select an Output # (1-4): ");
                             try { l_intOutput = int.Parse(Console.ReadLine()); }
                             catch { l_intOutput = -1; }
                         } while (l_intOutput < 1 || l_intOutput > 4);

                         var l_intSource = -1;
                         do
                         {
                             Console.Write("Please select a Source # (1-4): ");
                             try { l_intSource = int.Parse(Console.ReadLine()); }
                             catch { l_intSource = -1; }
                         } while (l_intSource < 1 || l_intSource > 4);

                         switchMatrixSource((SwitcherOutput)l_intOutput, (SwitcherAction)l_intSource);

                         RunAnotherOperation();
                         break;
                     case "Y" :
                         Console.Write(hallAutomations.displaySettings());

                         RunAnotherOperation();
                         break;
                     default:
                         Console.WriteLine("Error! Unhandled operation selected.");

                         System.Environment.Exit(1);

                         break;
                 }

                 TimeSpan ts = DateTime.Now - StartTime;
                 Console.WriteLine("Operation completed in: " + ts.Hours + " Hours " + ts.Minutes + " Minutes " + ts.Seconds + " Seconds");
             }

             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
             }
         }    
             
              */

    }
}
