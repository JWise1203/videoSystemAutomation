namespace Visual_Automations
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.Label lblProjectorStatus;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			System.Windows.Forms.Label lblLiftStatus;
			System.Windows.Forms.Label lblRegistrationStatus;
			this.tbFeedback = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.tbErrors = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tbProjectors = new System.Windows.Forms.TabPage();
			this.gbProjectorCommands = new System.Windows.Forms.GroupBox();
			this.lstProjectorCommands = new System.Windows.Forms.ListBox();
			this.btnExecuteProjectorCommand = new System.Windows.Forms.Button();
			this.txtRawProjectorCommand = new System.Windows.Forms.TextBox();
			this.btnSendRawProjectorCommand = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.ddlProjectors = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tbLifts = new System.Windows.Forms.TabPage();
			this.gbLiftCommands = new System.Windows.Forms.GroupBox();
			this.lstLiftCommands = new System.Windows.Forms.ListBox();
			this.btnExecuteLiftCommand = new System.Windows.Forms.Button();
			this.txtRawLiftCommand = new System.Windows.Forms.TextBox();
			this.btnSendRawLiftCommand = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.ddlLift = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tbTelevisions = new System.Windows.Forms.TabPage();
			this.gbTVCommands = new System.Windows.Forms.GroupBox();
			this.lstTVCommands = new System.Windows.Forms.ListBox();
			this.btnExecuteCommand = new System.Windows.Forms.Button();
			this.txtRawCommand = new System.Windows.Forms.TextBox();
			this.btnSendRawCommand = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.gbTVRegistration = new System.Windows.Forms.GroupBox();
			this.btnTVRegistration = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.gbTVAuth = new System.Windows.Forms.GroupBox();
			this.txtPIN = new System.Windows.Forms.TextBox();
			this.btnSendAuth = new System.Windows.Forms.Button();
			this.lblPIN = new System.Windows.Forms.Label();
			this.ddlTelevisions = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbQuick = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblQuickInstructions = new System.Windows.Forms.Label();
			this.gbTVs = new System.Windows.Forms.GroupBox();
			this.cboTV8 = new System.Windows.Forms.CheckBox();
			this.cboTV7 = new System.Windows.Forms.CheckBox();
			this.cboTV6 = new System.Windows.Forms.CheckBox();
			this.cboTV5 = new System.Windows.Forms.CheckBox();
			this.cboTV4 = new System.Windows.Forms.CheckBox();
			this.cboTV3 = new System.Windows.Forms.CheckBox();
			this.cboTV2 = new System.Windows.Forms.CheckBox();
			this.cboTV1 = new System.Windows.Forms.CheckBox();
			this.gbProjectors = new System.Windows.Forms.GroupBox();
			this.cboProj3 = new System.Windows.Forms.CheckBox();
			this.cboProj2 = new System.Windows.Forms.CheckBox();
			this.cboProj1 = new System.Windows.Forms.CheckBox();
			this.cboProj4 = new System.Windows.Forms.CheckBox();
			this.gbCommands = new System.Windows.Forms.GroupBox();
			this.btnOn = new System.Windows.Forms.Button();
			this.btnOff = new System.Windows.Forms.Button();
			this.tbAdvanced = new System.Windows.Forms.TabPage();
			this.gbStart_MainAuditorium = new System.Windows.Forms.GroupBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.btnStart_MA_TV_On = new System.Windows.Forms.Button();
			this.btnStart_MA_TV_Off = new System.Windows.Forms.Button();
			this.btnStart_MA_Proj_On = new System.Windows.Forms.Button();
			this.btnStart_MA_Proj_Off = new System.Windows.Forms.Button();
			this.gbStart_BackRooms = new System.Windows.Forms.GroupBox();
			this.label23 = new System.Windows.Forms.Label();
			this.btnStart_BR_TV_On = new System.Windows.Forms.Button();
			this.btnStart_BR_TV_Off = new System.Windows.Forms.Button();
			this.chkStart_BR_IncOffice = new System.Windows.Forms.CheckBox();
			this.gbStart_All = new System.Windows.Forms.GroupBox();
			this.btnStart_All_On = new System.Windows.Forms.Button();
			this.btnStart_All_Off = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbBasic = new System.Windows.Forms.TabPage();
			this.gbBasic_Televisions = new System.Windows.Forms.GroupBox();
			this.btnBasic_TV_On = new System.Windows.Forms.Button();
			this.btnBasic_TV_Off = new System.Windows.Forms.Button();
			this.label32 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			lblProjectorStatus = new System.Windows.Forms.Label();
			lblLiftStatus = new System.Windows.Forms.Label();
			lblRegistrationStatus = new System.Windows.Forms.Label();
			this.tbFeedback.SuspendLayout();
			this.tbErrors.SuspendLayout();
			this.tbProjectors.SuspendLayout();
			this.gbProjectorCommands.SuspendLayout();
			this.tbLifts.SuspendLayout();
			this.gbLiftCommands.SuspendLayout();
			this.tbTelevisions.SuspendLayout();
			this.gbTVCommands.SuspendLayout();
			this.gbTVRegistration.SuspendLayout();
			this.gbTVAuth.SuspendLayout();
			this.tbQuick.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gbTVs.SuspendLayout();
			this.gbProjectors.SuspendLayout();
			this.gbCommands.SuspendLayout();
			this.tbAdvanced.SuspendLayout();
			this.gbStart_MainAuditorium.SuspendLayout();
			this.gbStart_BackRooms.SuspendLayout();
			this.gbStart_All.SuspendLayout();
			this.tbBasic.SuspendLayout();
			this.gbBasic_Televisions.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbFeedback
			// 
			this.tbFeedback.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbFeedback.Controls.Add(this.label2);
			this.tbFeedback.Location = new System.Drawing.Point(4, 22);
			this.tbFeedback.Name = "tbFeedback";
			this.tbFeedback.Padding = new System.Windows.Forms.Padding(3);
			this.tbFeedback.Size = new System.Drawing.Size(686, 478);
			this.tbFeedback.TabIndex = 4;
			this.tbFeedback.Text = "Feedback";
			this.tbFeedback.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(86, 174);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(519, 73);
			this.label2.TabIndex = 0;
			this.label2.Text = "COMING SOON!";
			// 
			// tbErrors
			// 
			this.tbErrors.BackColor = System.Drawing.Color.Transparent;
			this.tbErrors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbErrors.Controls.Add(this.label3);
			this.tbErrors.Controls.Add(this.button3);
			this.tbErrors.Controls.Add(this.button2);
			this.tbErrors.Controls.Add(this.button1);
			this.tbErrors.Location = new System.Drawing.Point(4, 22);
			this.tbErrors.Name = "tbErrors";
			this.tbErrors.Padding = new System.Windows.Forms.Padding(3);
			this.tbErrors.Size = new System.Drawing.Size(686, 478);
			this.tbErrors.TabIndex = 1;
			this.tbErrors.Text = "Errors";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(6, 389);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(91, 25);
			this.button1.TabIndex = 0;
			this.button1.Text = "Refresh List";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(103, 389);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Clear List";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(184, 389);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "Save List";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(82, 201);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(519, 73);
			this.label3.TabIndex = 3;
			this.label3.Text = "COMING SOON!";
			// 
			// tbProjectors
			// 
			this.tbProjectors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbProjectors.Controls.Add(this.label4);
			this.tbProjectors.Controls.Add(lblProjectorStatus);
			this.tbProjectors.Controls.Add(this.label8);
			this.tbProjectors.Controls.Add(this.ddlProjectors);
			this.tbProjectors.Controls.Add(this.gbProjectorCommands);
			this.tbProjectors.Location = new System.Drawing.Point(4, 22);
			this.tbProjectors.Name = "tbProjectors";
			this.tbProjectors.Padding = new System.Windows.Forms.Padding(3);
			this.tbProjectors.Size = new System.Drawing.Size(686, 478);
			this.tbProjectors.TabIndex = 3;
			this.tbProjectors.Text = "Projectors";
			this.tbProjectors.UseVisualStyleBackColor = true;
			// 
			// gbProjectorCommands
			// 
			this.gbProjectorCommands.Controls.Add(this.label11);
			this.gbProjectorCommands.Controls.Add(this.btnSendRawProjectorCommand);
			this.gbProjectorCommands.Controls.Add(this.txtRawProjectorCommand);
			this.gbProjectorCommands.Controls.Add(this.btnExecuteProjectorCommand);
			this.gbProjectorCommands.Controls.Add(this.lstProjectorCommands);
			this.gbProjectorCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbProjectorCommands.Location = new System.Drawing.Point(283, 115);
			this.gbProjectorCommands.Name = "gbProjectorCommands";
			this.gbProjectorCommands.Size = new System.Drawing.Size(393, 297);
			this.gbProjectorCommands.TabIndex = 10;
			this.gbProjectorCommands.TabStop = false;
			this.gbProjectorCommands.Text = "General Commands";
			this.gbProjectorCommands.Visible = false;
			// 
			// lstProjectorCommands
			// 
			this.lstProjectorCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstProjectorCommands.FormattingEnabled = true;
			this.lstProjectorCommands.ItemHeight = 20;
			this.lstProjectorCommands.Location = new System.Drawing.Point(6, 47);
			this.lstProjectorCommands.Name = "lstProjectorCommands";
			this.lstProjectorCommands.Size = new System.Drawing.Size(198, 164);
			this.lstProjectorCommands.TabIndex = 2;
			this.lstProjectorCommands.SelectedIndexChanged += new System.EventHandler(this.lstProjectorCommands_SelectedIndexChanged);
			// 
			// btnExecuteProjectorCommand
			// 
			this.btnExecuteProjectorCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExecuteProjectorCommand.Location = new System.Drawing.Point(210, 122);
			this.btnExecuteProjectorCommand.Name = "btnExecuteProjectorCommand";
			this.btnExecuteProjectorCommand.Size = new System.Drawing.Size(159, 72);
			this.btnExecuteProjectorCommand.TabIndex = 4;
			this.btnExecuteProjectorCommand.Text = "Execute Command";
			this.btnExecuteProjectorCommand.UseVisualStyleBackColor = true;
			this.btnExecuteProjectorCommand.Click += new System.EventHandler(this.btnExecuteProjectorCommand_Click);
			// 
			// txtRawProjectorCommand
			// 
			this.txtRawProjectorCommand.Location = new System.Drawing.Point(6, 250);
			this.txtRawProjectorCommand.Name = "txtRawProjectorCommand";
			this.txtRawProjectorCommand.Size = new System.Drawing.Size(198, 26);
			this.txtRawProjectorCommand.TabIndex = 5;
			// 
			// btnSendRawProjectorCommand
			// 
			this.btnSendRawProjectorCommand.Location = new System.Drawing.Point(210, 244);
			this.btnSendRawProjectorCommand.Name = "btnSendRawProjectorCommand";
			this.btnSendRawProjectorCommand.Size = new System.Drawing.Size(123, 38);
			this.btnSendRawProjectorCommand.TabIndex = 6;
			this.btnSendRawProjectorCommand.Text = "Send Test";
			this.btnSendRawProjectorCommand.UseVisualStyleBackColor = true;
			this.btnSendRawProjectorCommand.Click += new System.EventHandler(this.btnSendRawProjectorCommand_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 227);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(118, 20);
			this.label11.TabIndex = 7;
			this.label11.Text = "Raw Command";
			// 
			// ddlProjectors
			// 
			this.ddlProjectors.FormattingEnabled = true;
			this.ddlProjectors.Location = new System.Drawing.Point(6, 162);
			this.ddlProjectors.Name = "ddlProjectors";
			this.ddlProjectors.Size = new System.Drawing.Size(265, 21);
			this.ddlProjectors.TabIndex = 12;
			this.ddlProjectors.SelectedIndexChanged += new System.EventHandler(this.ddlProjectors_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(5, 139);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(134, 20);
			this.label8.TabIndex = 13;
			this.label8.Text = "Select a Projector";
			// 
			// lblProjectorStatus
			// 
			lblProjectorStatus.Location = new System.Drawing.Point(6, 198);
			lblProjectorStatus.Name = "lblProjectorStatus";
			lblProjectorStatus.Size = new System.Drawing.Size(265, 211);
			lblProjectorStatus.TabIndex = 14;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 31);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(670, 67);
			this.label4.TabIndex = 15;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// tbLifts
			// 
			this.tbLifts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbLifts.Controls.Add(this.label9);
			this.tbLifts.Controls.Add(lblLiftStatus);
			this.tbLifts.Controls.Add(this.label12);
			this.tbLifts.Controls.Add(this.ddlLift);
			this.tbLifts.Controls.Add(this.gbLiftCommands);
			this.tbLifts.Location = new System.Drawing.Point(4, 22);
			this.tbLifts.Name = "tbLifts";
			this.tbLifts.Padding = new System.Windows.Forms.Padding(3);
			this.tbLifts.Size = new System.Drawing.Size(686, 478);
			this.tbLifts.TabIndex = 5;
			this.tbLifts.Text = "Projector Lifts";
			this.tbLifts.UseVisualStyleBackColor = true;
			// 
			// gbLiftCommands
			// 
			this.gbLiftCommands.Controls.Add(this.label13);
			this.gbLiftCommands.Controls.Add(this.btnSendRawLiftCommand);
			this.gbLiftCommands.Controls.Add(this.txtRawLiftCommand);
			this.gbLiftCommands.Controls.Add(this.btnExecuteLiftCommand);
			this.gbLiftCommands.Controls.Add(this.lstLiftCommands);
			this.gbLiftCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbLiftCommands.Location = new System.Drawing.Point(283, 87);
			this.gbLiftCommands.Name = "gbLiftCommands";
			this.gbLiftCommands.Size = new System.Drawing.Size(393, 297);
			this.gbLiftCommands.TabIndex = 16;
			this.gbLiftCommands.TabStop = false;
			this.gbLiftCommands.Text = "General Commands";
			this.gbLiftCommands.Visible = false;
			// 
			// lstLiftCommands
			// 
			this.lstLiftCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstLiftCommands.FormattingEnabled = true;
			this.lstLiftCommands.ItemHeight = 20;
			this.lstLiftCommands.Location = new System.Drawing.Point(6, 47);
			this.lstLiftCommands.Name = "lstLiftCommands";
			this.lstLiftCommands.Size = new System.Drawing.Size(198, 164);
			this.lstLiftCommands.TabIndex = 2;
			this.lstLiftCommands.SelectedIndexChanged += new System.EventHandler(this.lstLiftCommands_SelectedIndexChanged);
			// 
			// btnExecuteLiftCommand
			// 
			this.btnExecuteLiftCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExecuteLiftCommand.Location = new System.Drawing.Point(210, 122);
			this.btnExecuteLiftCommand.Name = "btnExecuteLiftCommand";
			this.btnExecuteLiftCommand.Size = new System.Drawing.Size(159, 72);
			this.btnExecuteLiftCommand.TabIndex = 4;
			this.btnExecuteLiftCommand.Text = "Execute Command";
			this.btnExecuteLiftCommand.UseVisualStyleBackColor = true;
			this.btnExecuteLiftCommand.Click += new System.EventHandler(this.btnExecuteLiftCommand_Click);
			// 
			// txtRawLiftCommand
			// 
			this.txtRawLiftCommand.Location = new System.Drawing.Point(6, 250);
			this.txtRawLiftCommand.Name = "txtRawLiftCommand";
			this.txtRawLiftCommand.Size = new System.Drawing.Size(198, 26);
			this.txtRawLiftCommand.TabIndex = 5;
			// 
			// btnSendRawLiftCommand
			// 
			this.btnSendRawLiftCommand.Location = new System.Drawing.Point(210, 244);
			this.btnSendRawLiftCommand.Name = "btnSendRawLiftCommand";
			this.btnSendRawLiftCommand.Size = new System.Drawing.Size(123, 38);
			this.btnSendRawLiftCommand.TabIndex = 6;
			this.btnSendRawLiftCommand.Text = "Send Test";
			this.btnSendRawLiftCommand.UseVisualStyleBackColor = true;
			this.btnSendRawLiftCommand.Click += new System.EventHandler(this.btnSendRawLiftCommand_Click);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(6, 227);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(118, 20);
			this.label13.TabIndex = 7;
			this.label13.Text = "Raw Command";
			// 
			// ddlLift
			// 
			this.ddlLift.FormattingEnabled = true;
			this.ddlLift.Location = new System.Drawing.Point(6, 134);
			this.ddlLift.Name = "ddlLift";
			this.ddlLift.Size = new System.Drawing.Size(265, 21);
			this.ddlLift.TabIndex = 17;
			this.ddlLift.SelectedIndexChanged += new System.EventHandler(this.ddlLift_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(5, 111);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(160, 20);
			this.label12.TabIndex = 18;
			this.label12.Text = "Select a Projector Lift";
			// 
			// lblLiftStatus
			// 
			lblLiftStatus.Location = new System.Drawing.Point(6, 170);
			lblLiftStatus.Name = "lblLiftStatus";
			lblLiftStatus.Size = new System.Drawing.Size(265, 211);
			lblLiftStatus.TabIndex = 19;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(6, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(670, 22);
			this.label9.TabIndex = 20;
			this.label9.Text = "Please do not close the lift while the projector is still on.";
			// 
			// tbTelevisions
			// 
			this.tbTelevisions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbTelevisions.Controls.Add(lblRegistrationStatus);
			this.tbTelevisions.Controls.Add(this.label6);
			this.tbTelevisions.Controls.Add(this.ddlTelevisions);
			this.tbTelevisions.Controls.Add(this.gbTVRegistration);
			this.tbTelevisions.Controls.Add(this.gbTVCommands);
			this.tbTelevisions.Location = new System.Drawing.Point(4, 22);
			this.tbTelevisions.Name = "tbTelevisions";
			this.tbTelevisions.Padding = new System.Windows.Forms.Padding(3);
			this.tbTelevisions.Size = new System.Drawing.Size(686, 478);
			this.tbTelevisions.TabIndex = 2;
			this.tbTelevisions.Text = "Televisions";
			this.tbTelevisions.UseVisualStyleBackColor = true;
			// 
			// gbTVCommands
			// 
			this.gbTVCommands.Controls.Add(this.label7);
			this.gbTVCommands.Controls.Add(this.btnSendRawCommand);
			this.gbTVCommands.Controls.Add(this.txtRawCommand);
			this.gbTVCommands.Controls.Add(this.btnExecuteCommand);
			this.gbTVCommands.Controls.Add(this.lstTVCommands);
			this.gbTVCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTVCommands.Location = new System.Drawing.Point(283, 16);
			this.gbTVCommands.Name = "gbTVCommands";
			this.gbTVCommands.Size = new System.Drawing.Size(393, 297);
			this.gbTVCommands.TabIndex = 5;
			this.gbTVCommands.TabStop = false;
			this.gbTVCommands.Text = "General Commands";
			this.gbTVCommands.Visible = false;
			// 
			// lstTVCommands
			// 
			this.lstTVCommands.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstTVCommands.FormattingEnabled = true;
			this.lstTVCommands.ItemHeight = 20;
			this.lstTVCommands.Location = new System.Drawing.Point(6, 47);
			this.lstTVCommands.Name = "lstTVCommands";
			this.lstTVCommands.Size = new System.Drawing.Size(198, 164);
			this.lstTVCommands.TabIndex = 2;
			this.lstTVCommands.SelectedIndexChanged += new System.EventHandler(this.lstTVCommands_SelectedIndexChanged);
			// 
			// btnExecuteCommand
			// 
			this.btnExecuteCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExecuteCommand.Location = new System.Drawing.Point(210, 83);
			this.btnExecuteCommand.Name = "btnExecuteCommand";
			this.btnExecuteCommand.Size = new System.Drawing.Size(159, 72);
			this.btnExecuteCommand.TabIndex = 4;
			this.btnExecuteCommand.Text = "Execute Command";
			this.btnExecuteCommand.UseVisualStyleBackColor = true;
			this.btnExecuteCommand.Click += new System.EventHandler(this.btnExecuteCommand_Click);
			// 
			// txtRawCommand
			// 
			this.txtRawCommand.Location = new System.Drawing.Point(6, 250);
			this.txtRawCommand.Name = "txtRawCommand";
			this.txtRawCommand.Size = new System.Drawing.Size(198, 26);
			this.txtRawCommand.TabIndex = 5;
			// 
			// btnSendRawCommand
			// 
			this.btnSendRawCommand.Location = new System.Drawing.Point(210, 244);
			this.btnSendRawCommand.Name = "btnSendRawCommand";
			this.btnSendRawCommand.Size = new System.Drawing.Size(123, 38);
			this.btnSendRawCommand.TabIndex = 6;
			this.btnSendRawCommand.Text = "Send Test";
			this.btnSendRawCommand.UseVisualStyleBackColor = true;
			this.btnSendRawCommand.Click += new System.EventHandler(this.btnSendRawCommand_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 227);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(118, 20);
			this.label7.TabIndex = 7;
			this.label7.Text = "Raw Command";
			// 
			// gbTVRegistration
			// 
			this.gbTVRegistration.Controls.Add(this.gbTVAuth);
			this.gbTVRegistration.Controls.Add(this.label5);
			this.gbTVRegistration.Controls.Add(this.btnTVRegistration);
			this.gbTVRegistration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTVRegistration.Location = new System.Drawing.Point(6, 313);
			this.gbTVRegistration.Name = "gbTVRegistration";
			this.gbTVRegistration.Size = new System.Drawing.Size(670, 155);
			this.gbTVRegistration.TabIndex = 6;
			this.gbTVRegistration.TabStop = false;
			this.gbTVRegistration.Text = "TV Registration";
			this.gbTVRegistration.Visible = false;
			// 
			// btnTVRegistration
			// 
			this.btnTVRegistration.Location = new System.Drawing.Point(6, 85);
			this.btnTVRegistration.Name = "btnTVRegistration";
			this.btnTVRegistration.Size = new System.Drawing.Size(173, 64);
			this.btnTVRegistration.TabIndex = 0;
			this.btnTVRegistration.Text = "Register TV";
			this.btnTVRegistration.UseVisualStyleBackColor = true;
			this.btnTVRegistration.Click += new System.EventHandler(this.btnTVRegistration_Click);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(6, 33);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(658, 36);
			this.label5.TabIndex = 4;
			this.label5.Text = "Note: Registration should be checked/Perfomred before the meeting begins, as we d" +
    "o not want to distract anyone from paying attention during the meeting.";
			// 
			// gbTVAuth
			// 
			this.gbTVAuth.Controls.Add(this.lblPIN);
			this.gbTVAuth.Controls.Add(this.btnSendAuth);
			this.gbTVAuth.Controls.Add(this.txtPIN);
			this.gbTVAuth.Location = new System.Drawing.Point(283, 72);
			this.gbTVAuth.Name = "gbTVAuth";
			this.gbTVAuth.Size = new System.Drawing.Size(381, 77);
			this.gbTVAuth.TabIndex = 10;
			this.gbTVAuth.TabStop = false;
			this.gbTVAuth.Visible = false;
			// 
			// txtPIN
			// 
			this.txtPIN.Location = new System.Drawing.Point(12, 45);
			this.txtPIN.Name = "txtPIN";
			this.txtPIN.Size = new System.Drawing.Size(114, 26);
			this.txtPIN.TabIndex = 1;
			// 
			// btnSendAuth
			// 
			this.btnSendAuth.Location = new System.Drawing.Point(132, 45);
			this.btnSendAuth.Name = "btnSendAuth";
			this.btnSendAuth.Size = new System.Drawing.Size(101, 26);
			this.btnSendAuth.TabIndex = 3;
			this.btnSendAuth.Text = "Submit PIN";
			this.btnSendAuth.UseVisualStyleBackColor = true;
			this.btnSendAuth.Click += new System.EventHandler(this.btnSendAuth_Click);
			// 
			// lblPIN
			// 
			this.lblPIN.AutoSize = true;
			this.lblPIN.Location = new System.Drawing.Point(8, 22);
			this.lblPIN.Name = "lblPIN";
			this.lblPIN.Size = new System.Drawing.Size(319, 20);
			this.lblPIN.TabIndex = 2;
			this.lblPIN.Text = "Please Enter The Code Displayed on the TV";
			// 
			// ddlTelevisions
			// 
			this.ddlTelevisions.FormattingEnabled = true;
			this.ddlTelevisions.Location = new System.Drawing.Point(6, 63);
			this.ddlTelevisions.Name = "ddlTelevisions";
			this.ddlTelevisions.Size = new System.Drawing.Size(265, 21);
			this.ddlTelevisions.TabIndex = 7;
			this.ddlTelevisions.SelectedIndexChanged += new System.EventHandler(this.ddlTelevisions_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(5, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(91, 20);
			this.label6.TabIndex = 8;
			this.label6.Text = "Select a TV";
			// 
			// lblRegistrationStatus
			// 
			lblRegistrationStatus.Location = new System.Drawing.Point(6, 99);
			lblRegistrationStatus.Name = "lblRegistrationStatus";
			lblRegistrationStatus.Size = new System.Drawing.Size(265, 211);
			lblRegistrationStatus.TabIndex = 9;
			// 
			// tbQuick
			// 
			this.tbQuick.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbQuick.Controls.Add(this.groupBox1);
			this.tbQuick.Location = new System.Drawing.Point(4, 22);
			this.tbQuick.Name = "tbQuick";
			this.tbQuick.Padding = new System.Windows.Forms.Padding(3);
			this.tbQuick.Size = new System.Drawing.Size(686, 478);
			this.tbQuick.TabIndex = 0;
			this.tbQuick.Text = "Power Alternatives";
			this.tbQuick.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.gbCommands);
			this.groupBox1.Controls.Add(this.gbProjectors);
			this.groupBox1.Controls.Add(this.gbTVs);
			this.groupBox1.Controls.Add(this.lblQuickInstructions);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(9, 59);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(657, 406);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Quick Commands";
			// 
			// lblQuickInstructions
			// 
			this.lblQuickInstructions.AutoSize = true;
			this.lblQuickInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblQuickInstructions.Location = new System.Drawing.Point(158, 22);
			this.lblQuickInstructions.Name = "lblQuickInstructions";
			this.lblQuickInstructions.Size = new System.Drawing.Size(318, 80);
			this.lblQuickInstructions.TabIndex = 13;
			this.lblQuickInstructions.Text = "Instructions\r\n\r\nStep 1: Pick the devices that you wish to control\r\nStep 2: Select" +
    " the Command that you wish to execute\r\nStep 3: Watch it happen.";
			// 
			// gbTVs
			// 
			this.gbTVs.Controls.Add(this.cboTV1);
			this.gbTVs.Controls.Add(this.cboTV2);
			this.gbTVs.Controls.Add(this.cboTV3);
			this.gbTVs.Controls.Add(this.cboTV4);
			this.gbTVs.Controls.Add(this.cboTV5);
			this.gbTVs.Controls.Add(this.cboTV6);
			this.gbTVs.Controls.Add(this.cboTV7);
			this.gbTVs.Controls.Add(this.cboTV8);
			this.gbTVs.Location = new System.Drawing.Point(6, 183);
			this.gbTVs.Name = "gbTVs";
			this.gbTVs.Size = new System.Drawing.Size(642, 100);
			this.gbTVs.TabIndex = 17;
			this.gbTVs.TabStop = false;
			this.gbTVs.Text = "Televisions";
			this.gbTVs.Visible = false;
			// 
			// cboTV8
			// 
			this.cboTV8.AutoSize = true;
			this.cboTV8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV8.Location = new System.Drawing.Point(456, 63);
			this.cboTV8.Name = "cboTV8";
			this.cboTV8.Size = new System.Drawing.Size(97, 20);
			this.cboTV8.TabIndex = 9;
			this.cboTV8.Text = "Television8";
			this.cboTV8.UseVisualStyleBackColor = true;
			this.cboTV8.Visible = false;
			// 
			// cboTV7
			// 
			this.cboTV7.AutoSize = true;
			this.cboTV7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV7.Location = new System.Drawing.Point(306, 63);
			this.cboTV7.Name = "cboTV7";
			this.cboTV7.Size = new System.Drawing.Size(97, 20);
			this.cboTV7.TabIndex = 8;
			this.cboTV7.Text = "Television7";
			this.cboTV7.UseVisualStyleBackColor = true;
			this.cboTV7.Visible = false;
			// 
			// cboTV6
			// 
			this.cboTV6.AutoSize = true;
			this.cboTV6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV6.Location = new System.Drawing.Point(156, 63);
			this.cboTV6.Name = "cboTV6";
			this.cboTV6.Size = new System.Drawing.Size(97, 20);
			this.cboTV6.TabIndex = 7;
			this.cboTV6.Text = "Television6";
			this.cboTV6.UseVisualStyleBackColor = true;
			this.cboTV6.Visible = false;
			// 
			// cboTV5
			// 
			this.cboTV5.AutoSize = true;
			this.cboTV5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV5.Location = new System.Drawing.Point(6, 63);
			this.cboTV5.Name = "cboTV5";
			this.cboTV5.Size = new System.Drawing.Size(97, 20);
			this.cboTV5.TabIndex = 6;
			this.cboTV5.Text = "Television5";
			this.cboTV5.UseVisualStyleBackColor = true;
			this.cboTV5.Visible = false;
			// 
			// cboTV4
			// 
			this.cboTV4.AutoSize = true;
			this.cboTV4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV4.Location = new System.Drawing.Point(456, 28);
			this.cboTV4.Name = "cboTV4";
			this.cboTV4.Size = new System.Drawing.Size(97, 20);
			this.cboTV4.TabIndex = 5;
			this.cboTV4.Text = "Television4";
			this.cboTV4.UseVisualStyleBackColor = true;
			this.cboTV4.Visible = false;
			// 
			// cboTV3
			// 
			this.cboTV3.AutoSize = true;
			this.cboTV3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV3.Location = new System.Drawing.Point(306, 28);
			this.cboTV3.Name = "cboTV3";
			this.cboTV3.Size = new System.Drawing.Size(97, 20);
			this.cboTV3.TabIndex = 4;
			this.cboTV3.Text = "Television3";
			this.cboTV3.UseVisualStyleBackColor = true;
			this.cboTV3.Visible = false;
			// 
			// cboTV2
			// 
			this.cboTV2.AutoSize = true;
			this.cboTV2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV2.Location = new System.Drawing.Point(156, 28);
			this.cboTV2.Name = "cboTV2";
			this.cboTV2.Size = new System.Drawing.Size(97, 20);
			this.cboTV2.TabIndex = 3;
			this.cboTV2.Text = "Television2";
			this.cboTV2.UseVisualStyleBackColor = true;
			this.cboTV2.Visible = false;
			// 
			// cboTV1
			// 
			this.cboTV1.AutoSize = true;
			this.cboTV1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboTV1.Location = new System.Drawing.Point(6, 28);
			this.cboTV1.Name = "cboTV1";
			this.cboTV1.Size = new System.Drawing.Size(97, 20);
			this.cboTV1.TabIndex = 2;
			this.cboTV1.Text = "Television1";
			this.cboTV1.UseVisualStyleBackColor = true;
			this.cboTV1.Visible = false;
			// 
			// gbProjectors
			// 
			this.gbProjectors.Controls.Add(this.cboProj4);
			this.gbProjectors.Controls.Add(this.cboProj1);
			this.gbProjectors.Controls.Add(this.cboProj2);
			this.gbProjectors.Controls.Add(this.cboProj3);
			this.gbProjectors.Location = new System.Drawing.Point(6, 110);
			this.gbProjectors.Name = "gbProjectors";
			this.gbProjectors.Size = new System.Drawing.Size(645, 67);
			this.gbProjectors.TabIndex = 18;
			this.gbProjectors.TabStop = false;
			this.gbProjectors.Text = "Projectors";
			this.gbProjectors.Visible = false;
			// 
			// cboProj3
			// 
			this.cboProj3.AutoSize = true;
			this.cboProj3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboProj3.Location = new System.Drawing.Point(306, 28);
			this.cboProj3.Name = "cboProj3";
			this.cboProj3.Size = new System.Drawing.Size(88, 20);
			this.cboProj3.TabIndex = 12;
			this.cboProj3.Text = "Projector3";
			this.cboProj3.UseVisualStyleBackColor = true;
			this.cboProj3.Visible = false;
			// 
			// cboProj2
			// 
			this.cboProj2.AutoSize = true;
			this.cboProj2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboProj2.Location = new System.Drawing.Point(156, 28);
			this.cboProj2.Name = "cboProj2";
			this.cboProj2.Size = new System.Drawing.Size(88, 20);
			this.cboProj2.TabIndex = 11;
			this.cboProj2.Text = "Projector2";
			this.cboProj2.UseVisualStyleBackColor = true;
			this.cboProj2.Visible = false;
			// 
			// cboProj1
			// 
			this.cboProj1.AutoSize = true;
			this.cboProj1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboProj1.Location = new System.Drawing.Point(6, 28);
			this.cboProj1.Name = "cboProj1";
			this.cboProj1.Size = new System.Drawing.Size(88, 20);
			this.cboProj1.TabIndex = 10;
			this.cboProj1.Text = "Projector1";
			this.cboProj1.UseVisualStyleBackColor = true;
			this.cboProj1.Visible = false;
			// 
			// cboProj4
			// 
			this.cboProj4.AutoSize = true;
			this.cboProj4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboProj4.Location = new System.Drawing.Point(456, 28);
			this.cboProj4.Name = "cboProj4";
			this.cboProj4.Size = new System.Drawing.Size(88, 20);
			this.cboProj4.TabIndex = 13;
			this.cboProj4.Text = "Projector4";
			this.cboProj4.UseVisualStyleBackColor = true;
			this.cboProj4.Visible = false;
			// 
			// gbCommands
			// 
			this.gbCommands.Controls.Add(this.btnOff);
			this.gbCommands.Controls.Add(this.btnOn);
			this.gbCommands.Location = new System.Drawing.Point(6, 322);
			this.gbCommands.Name = "gbCommands";
			this.gbCommands.Size = new System.Drawing.Size(642, 70);
			this.gbCommands.TabIndex = 19;
			this.gbCommands.TabStop = false;
			this.gbCommands.Text = "Commands";
			this.gbCommands.Visible = false;
			// 
			// btnOn
			// 
			this.btnOn.Location = new System.Drawing.Point(39, 25);
			this.btnOn.Name = "btnOn";
			this.btnOn.Size = new System.Drawing.Size(238, 39);
			this.btnOn.TabIndex = 15;
			this.btnOn.Text = "Turn Selected Devices On";
			this.btnOn.UseVisualStyleBackColor = true;
			this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
			// 
			// btnOff
			// 
			this.btnOff.Location = new System.Drawing.Point(318, 25);
			this.btnOff.Name = "btnOff";
			this.btnOff.Size = new System.Drawing.Size(238, 39);
			this.btnOff.TabIndex = 16;
			this.btnOff.Text = "Turn Selected Devices Off";
			this.btnOff.UseVisualStyleBackColor = true;
			this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
			// 
			// tbAdvanced
			// 
			this.tbAdvanced.Controls.Add(this.label1);
			this.tbAdvanced.Controls.Add(this.gbStart_All);
			this.tbAdvanced.Controls.Add(this.gbStart_BackRooms);
			this.tbAdvanced.Controls.Add(this.gbStart_MainAuditorium);
			this.tbAdvanced.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbAdvanced.Location = new System.Drawing.Point(4, 22);
			this.tbAdvanced.Name = "tbAdvanced";
			this.tbAdvanced.Size = new System.Drawing.Size(686, 478);
			this.tbAdvanced.TabIndex = 7;
			this.tbAdvanced.Text = "Advanced";
			this.tbAdvanced.UseVisualStyleBackColor = true;
			// 
			// gbStart_MainAuditorium
			// 
			this.gbStart_MainAuditorium.Controls.Add(this.btnStart_MA_Proj_Off);
			this.gbStart_MainAuditorium.Controls.Add(this.btnStart_MA_Proj_On);
			this.gbStart_MainAuditorium.Controls.Add(this.btnStart_MA_TV_Off);
			this.gbStart_MainAuditorium.Controls.Add(this.btnStart_MA_TV_On);
			this.gbStart_MainAuditorium.Controls.Add(this.label18);
			this.gbStart_MainAuditorium.Controls.Add(this.label17);
			this.gbStart_MainAuditorium.Location = new System.Drawing.Point(7, 82);
			this.gbStart_MainAuditorium.Name = "gbStart_MainAuditorium";
			this.gbStart_MainAuditorium.Size = new System.Drawing.Size(666, 132);
			this.gbStart_MainAuditorium.TabIndex = 0;
			this.gbStart_MainAuditorium.TabStop = false;
			this.gbStart_MainAuditorium.Text = "Main Auditorium";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(15, 37);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(89, 16);
			this.label17.TabIndex = 0;
			this.label17.Text = "Televisions";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(357, 37);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(71, 16);
			this.label18.TabIndex = 1;
			this.label18.Text = "Projector";
			// 
			// btnStart_MA_TV_On
			// 
			this.btnStart_MA_TV_On.Location = new System.Drawing.Point(29, 65);
			this.btnStart_MA_TV_On.Name = "btnStart_MA_TV_On";
			this.btnStart_MA_TV_On.Size = new System.Drawing.Size(75, 29);
			this.btnStart_MA_TV_On.TabIndex = 3;
			this.btnStart_MA_TV_On.Text = "On";
			this.btnStart_MA_TV_On.UseVisualStyleBackColor = true;
			this.btnStart_MA_TV_On.Click += new System.EventHandler(this.btnStart_MA_TV_On_Click);
			// 
			// btnStart_MA_TV_Off
			// 
			this.btnStart_MA_TV_Off.Location = new System.Drawing.Point(132, 65);
			this.btnStart_MA_TV_Off.Name = "btnStart_MA_TV_Off";
			this.btnStart_MA_TV_Off.Size = new System.Drawing.Size(75, 29);
			this.btnStart_MA_TV_Off.TabIndex = 4;
			this.btnStart_MA_TV_Off.Text = "Off";
			this.btnStart_MA_TV_Off.UseVisualStyleBackColor = true;
			this.btnStart_MA_TV_Off.Click += new System.EventHandler(this.btnStart_MA_TV_Off_Click);
			// 
			// btnStart_MA_Proj_On
			// 
			this.btnStart_MA_Proj_On.Location = new System.Drawing.Point(379, 65);
			this.btnStart_MA_Proj_On.Name = "btnStart_MA_Proj_On";
			this.btnStart_MA_Proj_On.Size = new System.Drawing.Size(75, 29);
			this.btnStart_MA_Proj_On.TabIndex = 8;
			this.btnStart_MA_Proj_On.Text = "On";
			this.btnStart_MA_Proj_On.UseVisualStyleBackColor = true;
			this.btnStart_MA_Proj_On.Click += new System.EventHandler(this.btnStart_MA_Proj_On_Click);
			// 
			// btnStart_MA_Proj_Off
			// 
			this.btnStart_MA_Proj_Off.Location = new System.Drawing.Point(482, 65);
			this.btnStart_MA_Proj_Off.Name = "btnStart_MA_Proj_Off";
			this.btnStart_MA_Proj_Off.Size = new System.Drawing.Size(75, 29);
			this.btnStart_MA_Proj_Off.TabIndex = 9;
			this.btnStart_MA_Proj_Off.Text = "Off";
			this.btnStart_MA_Proj_Off.UseVisualStyleBackColor = true;
			this.btnStart_MA_Proj_Off.Click += new System.EventHandler(this.btnStart_MA_Proj_Off_Click);
			// 
			// gbStart_BackRooms
			// 
			this.gbStart_BackRooms.Controls.Add(this.chkStart_BR_IncOffice);
			this.gbStart_BackRooms.Controls.Add(this.btnStart_BR_TV_Off);
			this.gbStart_BackRooms.Controls.Add(this.btnStart_BR_TV_On);
			this.gbStart_BackRooms.Controls.Add(this.label23);
			this.gbStart_BackRooms.Location = new System.Drawing.Point(7, 229);
			this.gbStart_BackRooms.Name = "gbStart_BackRooms";
			this.gbStart_BackRooms.Size = new System.Drawing.Size(272, 113);
			this.gbStart_BackRooms.TabIndex = 1;
			this.gbStart_BackRooms.TabStop = false;
			this.gbStart_BackRooms.Text = "Back Rooms";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.Location = new System.Drawing.Point(15, 32);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(89, 16);
			this.label23.TabIndex = 12;
			this.label23.Text = "Televisions";
			// 
			// btnStart_BR_TV_On
			// 
			this.btnStart_BR_TV_On.Location = new System.Drawing.Point(29, 63);
			this.btnStart_BR_TV_On.Name = "btnStart_BR_TV_On";
			this.btnStart_BR_TV_On.Size = new System.Drawing.Size(75, 29);
			this.btnStart_BR_TV_On.TabIndex = 14;
			this.btnStart_BR_TV_On.Text = "On";
			this.btnStart_BR_TV_On.UseVisualStyleBackColor = true;
			this.btnStart_BR_TV_On.Click += new System.EventHandler(this.btnStart_BR_TV_On_Click);
			// 
			// btnStart_BR_TV_Off
			// 
			this.btnStart_BR_TV_Off.Location = new System.Drawing.Point(132, 63);
			this.btnStart_BR_TV_Off.Name = "btnStart_BR_TV_Off";
			this.btnStart_BR_TV_Off.Size = new System.Drawing.Size(75, 29);
			this.btnStart_BR_TV_Off.TabIndex = 15;
			this.btnStart_BR_TV_Off.Text = "Off";
			this.btnStart_BR_TV_Off.UseVisualStyleBackColor = true;
			this.btnStart_BR_TV_Off.Click += new System.EventHandler(this.btnStart_BR_TV_Off_Click);
			// 
			// chkStart_BR_IncOffice
			// 
			this.chkStart_BR_IncOffice.AutoSize = true;
			this.chkStart_BR_IncOffice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkStart_BR_IncOffice.Location = new System.Drawing.Point(127, 32);
			this.chkStart_BR_IncOffice.Name = "chkStart_BR_IncOffice";
			this.chkStart_BR_IncOffice.Size = new System.Drawing.Size(109, 17);
			this.chkStart_BR_IncOffice.TabIndex = 17;
			this.chkStart_BR_IncOffice.Text = "Include Office TV";
			this.chkStart_BR_IncOffice.UseVisualStyleBackColor = true;
			// 
			// gbStart_All
			// 
			this.gbStart_All.Controls.Add(this.btnStart_All_Off);
			this.gbStart_All.Controls.Add(this.btnStart_All_On);
			this.gbStart_All.Location = new System.Drawing.Point(367, 229);
			this.gbStart_All.Name = "gbStart_All";
			this.gbStart_All.Size = new System.Drawing.Size(306, 113);
			this.gbStart_All.TabIndex = 2;
			this.gbStart_All.TabStop = false;
			this.gbStart_All.Text = "All";
			// 
			// btnStart_All_On
			// 
			this.btnStart_All_On.Location = new System.Drawing.Point(19, 63);
			this.btnStart_All_On.Name = "btnStart_All_On";
			this.btnStart_All_On.Size = new System.Drawing.Size(75, 29);
			this.btnStart_All_On.TabIndex = 16;
			this.btnStart_All_On.Text = "On";
			this.btnStart_All_On.UseVisualStyleBackColor = true;
			this.btnStart_All_On.Click += new System.EventHandler(this.btnStart_All_On_Click);
			// 
			// btnStart_All_Off
			// 
			this.btnStart_All_Off.Location = new System.Drawing.Point(122, 63);
			this.btnStart_All_Off.Name = "btnStart_All_Off";
			this.btnStart_All_Off.Size = new System.Drawing.Size(75, 29);
			this.btnStart_All_Off.TabIndex = 17;
			this.btnStart_All_Off.Text = "Off";
			this.btnStart_All_Off.UseVisualStyleBackColor = true;
			this.btnStart_All_Off.Click += new System.EventHandler(this.btnStart_All_Off_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(670, 42);
			this.label1.TabIndex = 3;
			this.label1.Text = "Welcome Brothers! This tool is intended to make everyone\'s lives a little easier." +
    " Please take time to note any issues/enhancements that you may find in the Feedb" +
    "ack Section.";
			// 
			// tbBasic
			// 
			this.tbBasic.Controls.Add(this.label32);
			this.tbBasic.Controls.Add(this.gbBasic_Televisions);
			this.tbBasic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.tbBasic.Location = new System.Drawing.Point(4, 22);
			this.tbBasic.Name = "tbBasic";
			this.tbBasic.Size = new System.Drawing.Size(686, 478);
			this.tbBasic.TabIndex = 8;
			this.tbBasic.Text = "Start";
			this.tbBasic.UseVisualStyleBackColor = true;
			// 
			// gbBasic_Televisions
			// 
			this.gbBasic_Televisions.Controls.Add(this.btnBasic_TV_Off);
			this.gbBasic_Televisions.Controls.Add(this.btnBasic_TV_On);
			this.gbBasic_Televisions.Location = new System.Drawing.Point(7, 147);
			this.gbBasic_Televisions.Name = "gbBasic_Televisions";
			this.gbBasic_Televisions.Size = new System.Drawing.Size(425, 165);
			this.gbBasic_Televisions.TabIndex = 3;
			this.gbBasic_Televisions.TabStop = false;
			this.gbBasic_Televisions.Text = "TV Displays";
			// 
			// btnBasic_TV_On
			// 
			this.btnBasic_TV_On.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBasic_TV_On.Location = new System.Drawing.Point(15, 53);
			this.btnBasic_TV_On.Name = "btnBasic_TV_On";
			this.btnBasic_TV_On.Size = new System.Drawing.Size(197, 82);
			this.btnBasic_TV_On.TabIndex = 3;
			this.btnBasic_TV_On.Text = "On";
			this.btnBasic_TV_On.UseVisualStyleBackColor = true;
			this.btnBasic_TV_On.Click += new System.EventHandler(this.btnBasic_TV_On_Click);
			// 
			// btnBasic_TV_Off
			// 
			this.btnBasic_TV_Off.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBasic_TV_Off.Location = new System.Drawing.Point(218, 53);
			this.btnBasic_TV_Off.Name = "btnBasic_TV_Off";
			this.btnBasic_TV_Off.Size = new System.Drawing.Size(191, 82);
			this.btnBasic_TV_Off.TabIndex = 4;
			this.btnBasic_TV_Off.Text = "Off";
			this.btnBasic_TV_Off.UseVisualStyleBackColor = true;
			this.btnBasic_TV_Off.Click += new System.EventHandler(this.btnBasic_TV_Off_Click);
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label32.Location = new System.Drawing.Point(3, 19);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(670, 42);
			this.label32.TabIndex = 6;
			this.label32.Text = "Welcome Brothers! This tool is intended to make everyone\'s lives a little easier." +
    " Please take time to note any issues/enhancements that you may find in the Feedb" +
    "ack Section.";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tbBasic);
			this.tabControl1.Controls.Add(this.tbAdvanced);
			this.tabControl1.Controls.Add(this.tbQuick);
			this.tabControl1.Controls.Add(this.tbTelevisions);
			this.tabControl1.Controls.Add(this.tbLifts);
			this.tabControl1.Controls.Add(this.tbProjectors);
			this.tabControl1.Controls.Add(this.tbErrors);
			this.tabControl1.Controls.Add(this.tbFeedback);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(694, 504);
			this.tabControl1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(718, 525);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Kingdom Hall Automations";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tbFeedback.ResumeLayout(false);
			this.tbFeedback.PerformLayout();
			this.tbErrors.ResumeLayout(false);
			this.tbErrors.PerformLayout();
			this.tbProjectors.ResumeLayout(false);
			this.tbProjectors.PerformLayout();
			this.gbProjectorCommands.ResumeLayout(false);
			this.gbProjectorCommands.PerformLayout();
			this.tbLifts.ResumeLayout(false);
			this.tbLifts.PerformLayout();
			this.gbLiftCommands.ResumeLayout(false);
			this.gbLiftCommands.PerformLayout();
			this.tbTelevisions.ResumeLayout(false);
			this.tbTelevisions.PerformLayout();
			this.gbTVCommands.ResumeLayout(false);
			this.gbTVCommands.PerformLayout();
			this.gbTVRegistration.ResumeLayout(false);
			this.gbTVAuth.ResumeLayout(false);
			this.gbTVAuth.PerformLayout();
			this.tbQuick.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.gbTVs.ResumeLayout(false);
			this.gbTVs.PerformLayout();
			this.gbProjectors.ResumeLayout(false);
			this.gbProjectors.PerformLayout();
			this.gbCommands.ResumeLayout(false);
			this.tbAdvanced.ResumeLayout(false);
			this.gbStart_MainAuditorium.ResumeLayout(false);
			this.gbStart_MainAuditorium.PerformLayout();
			this.gbStart_BackRooms.ResumeLayout(false);
			this.gbStart_BackRooms.PerformLayout();
			this.gbStart_All.ResumeLayout(false);
			this.tbBasic.ResumeLayout(false);
			this.gbBasic_Televisions.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.TabPage tbFeedback;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tbErrors;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabPage tbProjectors;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox ddlProjectors;
		private System.Windows.Forms.GroupBox gbProjectorCommands;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button btnSendRawProjectorCommand;
		private System.Windows.Forms.TextBox txtRawProjectorCommand;
		private System.Windows.Forms.Button btnExecuteProjectorCommand;
		private System.Windows.Forms.ListBox lstProjectorCommands;
		private System.Windows.Forms.TabPage tbLifts;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox ddlLift;
		private System.Windows.Forms.GroupBox gbLiftCommands;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnSendRawLiftCommand;
		private System.Windows.Forms.TextBox txtRawLiftCommand;
		private System.Windows.Forms.Button btnExecuteLiftCommand;
		private System.Windows.Forms.ListBox lstLiftCommands;
		private System.Windows.Forms.TabPage tbTelevisions;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox ddlTelevisions;
		private System.Windows.Forms.GroupBox gbTVRegistration;
		private System.Windows.Forms.GroupBox gbTVAuth;
		private System.Windows.Forms.Label lblPIN;
		private System.Windows.Forms.Button btnSendAuth;
		private System.Windows.Forms.TextBox txtPIN;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnTVRegistration;
		private System.Windows.Forms.GroupBox gbTVCommands;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnSendRawCommand;
		private System.Windows.Forms.TextBox txtRawCommand;
		private System.Windows.Forms.Button btnExecuteCommand;
		private System.Windows.Forms.ListBox lstTVCommands;
		private System.Windows.Forms.TabPage tbQuick;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox gbCommands;
		private System.Windows.Forms.Button btnOff;
		private System.Windows.Forms.Button btnOn;
		private System.Windows.Forms.GroupBox gbProjectors;
		private System.Windows.Forms.CheckBox cboProj4;
		private System.Windows.Forms.CheckBox cboProj1;
		private System.Windows.Forms.CheckBox cboProj2;
		private System.Windows.Forms.CheckBox cboProj3;
		private System.Windows.Forms.GroupBox gbTVs;
		private System.Windows.Forms.CheckBox cboTV1;
		private System.Windows.Forms.CheckBox cboTV2;
		private System.Windows.Forms.CheckBox cboTV3;
		private System.Windows.Forms.CheckBox cboTV4;
		private System.Windows.Forms.CheckBox cboTV5;
		private System.Windows.Forms.CheckBox cboTV6;
		private System.Windows.Forms.CheckBox cboTV7;
		private System.Windows.Forms.CheckBox cboTV8;
		private System.Windows.Forms.Label lblQuickInstructions;
		private System.Windows.Forms.TabPage tbAdvanced;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gbStart_All;
		private System.Windows.Forms.Button btnStart_All_Off;
		private System.Windows.Forms.Button btnStart_All_On;
		private System.Windows.Forms.GroupBox gbStart_BackRooms;
		private System.Windows.Forms.CheckBox chkStart_BR_IncOffice;
		private System.Windows.Forms.Button btnStart_BR_TV_Off;
		private System.Windows.Forms.Button btnStart_BR_TV_On;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.GroupBox gbStart_MainAuditorium;
		private System.Windows.Forms.Button btnStart_MA_Proj_Off;
		private System.Windows.Forms.Button btnStart_MA_Proj_On;
		private System.Windows.Forms.Button btnStart_MA_TV_Off;
		private System.Windows.Forms.Button btnStart_MA_TV_On;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TabPage tbBasic;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.GroupBox gbBasic_Televisions;
		private System.Windows.Forms.Button btnBasic_TV_Off;
		private System.Windows.Forms.Button btnBasic_TV_On;
		private System.Windows.Forms.TabControl tabControl1;
	}
}

