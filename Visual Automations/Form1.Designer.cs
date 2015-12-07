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
            System.Windows.Forms.Label lblRegistrationStatus;
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbQuick = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbCommands = new System.Windows.Forms.GroupBox();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.gbProjectors = new System.Windows.Forms.GroupBox();
            this.cboProj4 = new System.Windows.Forms.CheckBox();
            this.cboProj1 = new System.Windows.Forms.CheckBox();
            this.cboProj2 = new System.Windows.Forms.CheckBox();
            this.cboProj3 = new System.Windows.Forms.CheckBox();
            this.gbTVs = new System.Windows.Forms.GroupBox();
            this.cboTV1 = new System.Windows.Forms.CheckBox();
            this.cboTV2 = new System.Windows.Forms.CheckBox();
            this.cboTV3 = new System.Windows.Forms.CheckBox();
            this.cboTV4 = new System.Windows.Forms.CheckBox();
            this.cboTV5 = new System.Windows.Forms.CheckBox();
            this.cboTV6 = new System.Windows.Forms.CheckBox();
            this.cboTV7 = new System.Windows.Forms.CheckBox();
            this.cboTV8 = new System.Windows.Forms.CheckBox();
            this.lblQuickInstructions = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTelevisions = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlTelevisions = new System.Windows.Forms.ComboBox();
            this.gbTVRegistration = new System.Windows.Forms.GroupBox();
            this.gbTVAuth = new System.Windows.Forms.GroupBox();
            this.lblPIN = new System.Windows.Forms.Label();
            this.btnSendAuth = new System.Windows.Forms.Button();
            this.txtPIN = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTVRegistration = new System.Windows.Forms.Button();
            this.gbTVCommands = new System.Windows.Forms.GroupBox();
            this.btnExecuteCommand = new System.Windows.Forms.Button();
            this.lstTVCommands = new System.Windows.Forms.ListBox();
            this.tbProjectors = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tbErrors = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Feedback = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRawCommand = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            lblRegistrationStatus = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tbQuick.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbCommands.SuspendLayout();
            this.gbProjectors.SuspendLayout();
            this.gbTVs.SuspendLayout();
            this.tbTelevisions.SuspendLayout();
            this.gbTVRegistration.SuspendLayout();
            this.gbTVAuth.SuspendLayout();
            this.gbTVCommands.SuspendLayout();
            this.tbProjectors.SuspendLayout();
            this.tbErrors.SuspendLayout();
            this.Feedback.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRegistrationStatus
            // 
            lblRegistrationStatus.Location = new System.Drawing.Point(6, 99);
            lblRegistrationStatus.Name = "lblRegistrationStatus";
            lblRegistrationStatus.Size = new System.Drawing.Size(265, 70);
            lblRegistrationStatus.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbQuick);
            this.tabControl1.Controls.Add(this.tbTelevisions);
            this.tabControl1.Controls.Add(this.tbProjectors);
            this.tabControl1.Controls.Add(this.tbErrors);
            this.tabControl1.Controls.Add(this.Feedback);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(694, 504);
            this.tabControl1.TabIndex = 0;
            // 
            // tbQuick
            // 
            this.tbQuick.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbQuick.Controls.Add(this.groupBox1);
            this.tbQuick.Controls.Add(this.label1);
            this.tbQuick.Location = new System.Drawing.Point(4, 22);
            this.tbQuick.Name = "tbQuick";
            this.tbQuick.Padding = new System.Windows.Forms.Padding(3);
            this.tbQuick.Size = new System.Drawing.Size(686, 478);
            this.tbQuick.TabIndex = 0;
            this.tbQuick.Text = "Quick Commands";
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(670, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome Brothers! This tool is intended to make everyone\'s lives a little easier." +
    " Please take time to note any issues/enhancements that you may find in the Feedb" +
    "ack Section.";
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
            this.tbTelevisions.Text = "Television Commands";
            this.tbTelevisions.UseVisualStyleBackColor = true;
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
            // ddlTelevisions
            // 
            this.ddlTelevisions.FormattingEnabled = true;
            this.ddlTelevisions.Location = new System.Drawing.Point(6, 63);
            this.ddlTelevisions.Name = "ddlTelevisions";
            this.ddlTelevisions.Size = new System.Drawing.Size(265, 21);
            this.ddlTelevisions.TabIndex = 7;
            this.ddlTelevisions.SelectedIndexChanged += new System.EventHandler(this.ddlTelevisions_SelectedIndexChanged);
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
            // lblPIN
            // 
            this.lblPIN.AutoSize = true;
            this.lblPIN.Location = new System.Drawing.Point(8, 22);
            this.lblPIN.Name = "lblPIN";
            this.lblPIN.Size = new System.Drawing.Size(319, 20);
            this.lblPIN.TabIndex = 2;
            this.lblPIN.Text = "Please Enter The Code Displayed on the TV";
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
            // txtPIN
            // 
            this.txtPIN.Location = new System.Drawing.Point(12, 45);
            this.txtPIN.Name = "txtPIN";
            this.txtPIN.Size = new System.Drawing.Size(114, 26);
            this.txtPIN.TabIndex = 1;
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
            // gbTVCommands
            // 
            this.gbTVCommands.Controls.Add(this.label7);
            this.gbTVCommands.Controls.Add(this.button4);
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
            // btnExecuteCommand
            // 
            this.btnExecuteCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecuteCommand.Location = new System.Drawing.Point(210, 122);
            this.btnExecuteCommand.Name = "btnExecuteCommand";
            this.btnExecuteCommand.Size = new System.Drawing.Size(159, 72);
            this.btnExecuteCommand.TabIndex = 4;
            this.btnExecuteCommand.Text = "Execute Command";
            this.btnExecuteCommand.UseVisualStyleBackColor = true;
            this.btnExecuteCommand.Click += new System.EventHandler(this.btnExecuteCommand_Click);
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
            // tbProjectors
            // 
            this.tbProjectors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbProjectors.Controls.Add(this.label4);
            this.tbProjectors.Location = new System.Drawing.Point(4, 22);
            this.tbProjectors.Name = "tbProjectors";
            this.tbProjectors.Padding = new System.Windows.Forms.Padding(3);
            this.tbProjectors.Size = new System.Drawing.Size(686, 478);
            this.tbProjectors.TabIndex = 3;
            this.tbProjectors.Text = "Projector Commands";
            this.tbProjectors.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(82, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(519, 73);
            this.label4.TabIndex = 1;
            this.label4.Text = "COMING SOON!";
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
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(184, 389);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Save List";
            this.button3.UseVisualStyleBackColor = true;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Refresh List";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Feedback
            // 
            this.Feedback.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Feedback.Controls.Add(this.label2);
            this.Feedback.Location = new System.Drawing.Point(4, 22);
            this.Feedback.Name = "Feedback";
            this.Feedback.Size = new System.Drawing.Size(686, 478);
            this.Feedback.TabIndex = 4;
            this.Feedback.Text = "Feedback";
            this.Feedback.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(83, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(519, 73);
            this.label2.TabIndex = 0;
            this.label2.Text = "COMING SOON!";
            // 
            // txtRawCommand
            // 
            this.txtRawCommand.Location = new System.Drawing.Point(6, 250);
            this.txtRawCommand.Name = "txtRawCommand";
            this.txtRawCommand.Size = new System.Drawing.Size(198, 26);
            this.txtRawCommand.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(210, 244);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(123, 38);
            this.button4.TabIndex = 6;
            this.button4.Text = "Send Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(718, 525);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Kingdom Hall Automations";
            this.tabControl1.ResumeLayout(false);
            this.tbQuick.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbCommands.ResumeLayout(false);
            this.gbProjectors.ResumeLayout(false);
            this.gbProjectors.PerformLayout();
            this.gbTVs.ResumeLayout(false);
            this.gbTVs.PerformLayout();
            this.tbTelevisions.ResumeLayout(false);
            this.tbTelevisions.PerformLayout();
            this.gbTVRegistration.ResumeLayout(false);
            this.gbTVAuth.ResumeLayout(false);
            this.gbTVAuth.PerformLayout();
            this.gbTVCommands.ResumeLayout(false);
            this.gbTVCommands.PerformLayout();
            this.tbProjectors.ResumeLayout(false);
            this.tbProjectors.PerformLayout();
            this.tbErrors.ResumeLayout(false);
            this.tbErrors.PerformLayout();
            this.Feedback.ResumeLayout(false);
            this.Feedback.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbQuick;
        private System.Windows.Forms.TabPage tbErrors;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbCommands;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.GroupBox gbProjectors;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tbTelevisions;
        private System.Windows.Forms.TabPage tbProjectors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage Feedback;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cboProj4;
        private System.Windows.Forms.ComboBox ddlTelevisions;
        private System.Windows.Forms.GroupBox gbTVRegistration;
        private System.Windows.Forms.Button btnSendAuth;
        private System.Windows.Forms.Label lblPIN;
        private System.Windows.Forms.TextBox txtPIN;
        private System.Windows.Forms.Button btnTVRegistration;
        private System.Windows.Forms.GroupBox gbTVCommands;
        private System.Windows.Forms.Button btnExecuteCommand;
        private System.Windows.Forms.ListBox lstTVCommands;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbTVAuth;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtRawCommand;
        private System.Windows.Forms.Label label7;
    }
}

