namespace MIDIPlayback {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.browse = new System.Windows.Forms.Button();
            this.fileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mapRemove = new System.Windows.Forms.Button();
            this.play = new System.Windows.Forms.Button();
            this.elapsed = new System.Windows.Forms.Label();
            this.scrub = new System.Windows.Forms.TrackBar();
            this.total = new System.Windows.Forms.Label();
            this.listChannels = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mapInst = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mapOctave = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.mapStrat = new System.Windows.Forms.ListBox();
            this.mapPreviewLocal = new System.Windows.Forms.Button();
            this.mapAdd = new System.Windows.Forms.Button();
            this.mapStaccatoRepeats = new System.Windows.Forms.CheckBox();
            this.listMappings = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comPort = new System.Windows.Forms.ComboBox();
            this.comOpen = new System.Windows.Forms.Button();
            this.mapToggle = new System.Windows.Forms.Button();
            this.allOff = new System.Windows.Forms.Button();
            this.home0 = new System.Windows.Forms.Button();
            this.home1 = new System.Windows.Forms.Button();
            this.home2 = new System.Windows.Forms.Button();
            this.notePreview = new System.Windows.Forms.NumericUpDown();
            this.noteOff = new System.Windows.Forms.Button();
            this.home3 = new System.Windows.Forms.Button();
            this.skipTo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.embedPlay = new System.Windows.Forms.Button();
            this.embedStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scrub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapInst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapOctave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(13, 13);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 0;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.Location = new System.Drawing.Point(94, 18);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(48, 13);
            this.fileName.TabIndex = 1;
            this.fileName.Text = "fileName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(296, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Mappings";
            // 
            // mapRemove
            // 
            this.mapRemove.Location = new System.Drawing.Point(475, 167);
            this.mapRemove.Name = "mapRemove";
            this.mapRemove.Size = new System.Drawing.Size(89, 23);
            this.mapRemove.TabIndex = 7;
            this.mapRemove.Text = "Remove";
            this.mapRemove.UseVisualStyleBackColor = true;
            this.mapRemove.Click += new System.EventHandler(this.mapRemove_Click);
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(171, 13);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(75, 23);
            this.play.TabIndex = 8;
            this.play.Text = "Play";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // elapsed
            // 
            this.elapsed.AutoSize = true;
            this.elapsed.Location = new System.Drawing.Point(17, 56);
            this.elapsed.Name = "elapsed";
            this.elapsed.Size = new System.Drawing.Size(28, 13);
            this.elapsed.TabIndex = 11;
            this.elapsed.Text = "0:00";
            // 
            // scrub
            // 
            this.scrub.Location = new System.Drawing.Point(51, 52);
            this.scrub.Name = "scrub";
            this.scrub.Size = new System.Drawing.Size(322, 45);
            this.scrub.TabIndex = 12;
            this.scrub.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // total
            // 
            this.total.AutoSize = true;
            this.total.Location = new System.Drawing.Point(380, 56);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(28, 13);
            this.total.TabIndex = 13;
            this.total.Text = "0:00";
            // 
            // listChannels
            // 
            this.listChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listChannels.Location = new System.Drawing.Point(12, 103);
            this.listChannels.MultiSelect = false;
            this.listChannels.Name = "listChannels";
            this.listChannels.Size = new System.Drawing.Size(269, 355);
            this.listChannels.TabIndex = 16;
            this.listChannels.UseCompatibleStateImageBehavior = false;
            this.listChannels.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 20;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Notes";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Range";
            this.columnHeader4.Width = 105;
            // 
            // mapInst
            // 
            this.mapInst.Location = new System.Drawing.Point(84, 494);
            this.mapInst.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.mapInst.Name = "mapInst";
            this.mapInst.Size = new System.Drawing.Size(72, 20);
            this.mapInst.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 496);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Instrument";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 531);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Octave";
            // 
            // mapOctave
            // 
            this.mapOctave.Location = new System.Drawing.Point(84, 529);
            this.mapOctave.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.mapOctave.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.mapOctave.Name = "mapOctave";
            this.mapOctave.Size = new System.Drawing.Size(72, 20);
            this.mapOctave.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 465);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Mapping";
            // 
            // mapStrat
            // 
            this.mapStrat.FormattingEnabled = true;
            this.mapStrat.Items.AddRange(new object[] {
            "Most Recent",
            "Highest Note",
            "Lowest Note",
            "Arpeggio"});
            this.mapStrat.Location = new System.Drawing.Point(196, 494);
            this.mapStrat.Name = "mapStrat";
            this.mapStrat.Size = new System.Drawing.Size(120, 56);
            this.mapStrat.TabIndex = 23;
            // 
            // mapPreviewLocal
            // 
            this.mapPreviewLocal.Location = new System.Drawing.Point(333, 491);
            this.mapPreviewLocal.Name = "mapPreviewLocal";
            this.mapPreviewLocal.Size = new System.Drawing.Size(89, 23);
            this.mapPreviewLocal.TabIndex = 24;
            this.mapPreviewLocal.Text = "Preview";
            this.mapPreviewLocal.UseVisualStyleBackColor = true;
            this.mapPreviewLocal.Click += new System.EventHandler(this.mapPreviewLocal_Click);
            // 
            // mapAdd
            // 
            this.mapAdd.Location = new System.Drawing.Point(475, 491);
            this.mapAdd.Name = "mapAdd";
            this.mapAdd.Size = new System.Drawing.Size(89, 23);
            this.mapAdd.TabIndex = 26;
            this.mapAdd.Text = "Add Mapping";
            this.mapAdd.UseVisualStyleBackColor = true;
            this.mapAdd.Click += new System.EventHandler(this.mapAdd_Click);
            // 
            // mapStaccatoRepeats
            // 
            this.mapStaccatoRepeats.AutoSize = true;
            this.mapStaccatoRepeats.Location = new System.Drawing.Point(13, 558);
            this.mapStaccatoRepeats.Name = "mapStaccatoRepeats";
            this.mapStaccatoRepeats.Size = new System.Drawing.Size(107, 17);
            this.mapStaccatoRepeats.TabIndex = 27;
            this.mapStaccatoRepeats.Text = "Staccato repeats";
            this.mapStaccatoRepeats.UseVisualStyleBackColor = true;
            // 
            // listMappings
            // 
            this.listMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listMappings.Location = new System.Drawing.Point(288, 103);
            this.listMappings.MultiSelect = false;
            this.listMappings.Name = "listMappings";
            this.listMappings.Size = new System.Drawing.Size(168, 355);
            this.listMappings.TabIndex = 28;
            this.listMappings.UseCompatibleStateImageBehavior = false;
            this.listMappings.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Inst";
            this.columnHeader5.Width = 30;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Ch";
            this.columnHeader6.Width = 30;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Map";
            this.columnHeader7.Width = 103;
            // 
            // comPort
            // 
            this.comPort.FormattingEnabled = true;
            this.comPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4"});
            this.comPort.Location = new System.Drawing.Point(12, 601);
            this.comPort.Name = "comPort";
            this.comPort.Size = new System.Drawing.Size(121, 21);
            this.comPort.TabIndex = 29;
            // 
            // comOpen
            // 
            this.comOpen.Location = new System.Drawing.Point(140, 601);
            this.comOpen.Name = "comOpen";
            this.comOpen.Size = new System.Drawing.Size(75, 23);
            this.comOpen.TabIndex = 30;
            this.comOpen.Text = "Open COM";
            this.comOpen.UseVisualStyleBackColor = true;
            this.comOpen.Click += new System.EventHandler(this.comOpen_Click);
            // 
            // mapToggle
            // 
            this.mapToggle.Location = new System.Drawing.Point(475, 118);
            this.mapToggle.Name = "mapToggle";
            this.mapToggle.Size = new System.Drawing.Size(89, 23);
            this.mapToggle.TabIndex = 34;
            this.mapToggle.Text = "Enable/Disable";
            this.mapToggle.UseVisualStyleBackColor = true;
            this.mapToggle.Click += new System.EventHandler(this.mapToggle_Click);
            // 
            // allOff
            // 
            this.allOff.Location = new System.Drawing.Point(489, 13);
            this.allOff.Name = "allOff";
            this.allOff.Size = new System.Drawing.Size(75, 23);
            this.allOff.TabIndex = 35;
            this.allOff.Text = "All Off";
            this.allOff.UseVisualStyleBackColor = true;
            this.allOff.Click += new System.EventHandler(this.allOff_Click);
            // 
            // home0
            // 
            this.home0.Location = new System.Drawing.Point(489, 265);
            this.home0.Name = "home0";
            this.home0.Size = new System.Drawing.Size(75, 23);
            this.home0.TabIndex = 36;
            this.home0.Tag = "0";
            this.home0.Text = "Home 0";
            this.home0.UseVisualStyleBackColor = true;
            this.home0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.home_MouseDown);
            this.home0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.home_MouseUp);
            // 
            // home1
            // 
            this.home1.Location = new System.Drawing.Point(489, 294);
            this.home1.Name = "home1";
            this.home1.Size = new System.Drawing.Size(75, 23);
            this.home1.TabIndex = 37;
            this.home1.Tag = "1";
            this.home1.Text = "Home 1";
            this.home1.UseVisualStyleBackColor = true;
            this.home1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.home_MouseDown);
            this.home1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.home_MouseUp);
            // 
            // home2
            // 
            this.home2.Location = new System.Drawing.Point(489, 323);
            this.home2.Name = "home2";
            this.home2.Size = new System.Drawing.Size(75, 23);
            this.home2.TabIndex = 38;
            this.home2.Tag = "2";
            this.home2.Text = "Home 2";
            this.home2.UseVisualStyleBackColor = true;
            this.home2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.home_MouseDown);
            this.home2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.home_MouseUp);
            // 
            // notePreview
            // 
            this.notePreview.Location = new System.Drawing.Point(334, 524);
            this.notePreview.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.notePreview.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.notePreview.Name = "notePreview";
            this.notePreview.Size = new System.Drawing.Size(39, 20);
            this.notePreview.TabIndex = 39;
            this.notePreview.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.notePreview.ValueChanged += new System.EventHandler(this.notePreview_ValueChanged);
            // 
            // noteOff
            // 
            this.noteOff.Location = new System.Drawing.Point(379, 521);
            this.noteOff.Name = "noteOff";
            this.noteOff.Size = new System.Drawing.Size(43, 23);
            this.noteOff.TabIndex = 40;
            this.noteOff.Text = "Off";
            this.noteOff.UseVisualStyleBackColor = true;
            this.noteOff.Click += new System.EventHandler(this.noteOff_Click);
            // 
            // home3
            // 
            this.home3.Location = new System.Drawing.Point(489, 352);
            this.home3.Name = "home3";
            this.home3.Size = new System.Drawing.Size(75, 23);
            this.home3.TabIndex = 41;
            this.home3.Tag = "3";
            this.home3.Text = "Home 3";
            this.home3.UseVisualStyleBackColor = true;
            this.home3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.home_MouseDown);
            this.home3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.home_MouseUp);
            // 
            // skipTo
            // 
            this.skipTo.Location = new System.Drawing.Point(308, 13);
            this.skipTo.Name = "skipTo";
            this.skipTo.Size = new System.Drawing.Size(74, 20);
            this.skipTo.TabIndex = 42;
            this.skipTo.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(260, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "start at";
            // 
            // embedPlay
            // 
            this.embedPlay.Location = new System.Drawing.Point(224, 601);
            this.embedPlay.Name = "embedPlay";
            this.embedPlay.Size = new System.Drawing.Size(75, 23);
            this.embedPlay.TabIndex = 44;
            this.embedPlay.Text = "Play";
            this.embedPlay.UseVisualStyleBackColor = true;
            this.embedPlay.Click += new System.EventHandler(this.embedPlay_Click);
            // 
            // embedStop
            // 
            this.embedStop.Location = new System.Drawing.Point(308, 601);
            this.embedStop.Name = "embedStop";
            this.embedStop.Size = new System.Drawing.Size(75, 23);
            this.embedStop.TabIndex = 45;
            this.embedStop.Text = "Stop";
            this.embedStop.UseVisualStyleBackColor = true;
            this.embedStop.Click += new System.EventHandler(this.embedStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 648);
            this.Controls.Add(this.embedStop);
            this.Controls.Add(this.embedPlay);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.skipTo);
            this.Controls.Add(this.home3);
            this.Controls.Add(this.noteOff);
            this.Controls.Add(this.notePreview);
            this.Controls.Add(this.home2);
            this.Controls.Add(this.home1);
            this.Controls.Add(this.home0);
            this.Controls.Add(this.allOff);
            this.Controls.Add(this.mapToggle);
            this.Controls.Add(this.comOpen);
            this.Controls.Add(this.comPort);
            this.Controls.Add(this.listMappings);
            this.Controls.Add(this.mapStaccatoRepeats);
            this.Controls.Add(this.mapAdd);
            this.Controls.Add(this.mapPreviewLocal);
            this.Controls.Add(this.mapStrat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mapOctave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapInst);
            this.Controls.Add(this.listChannels);
            this.Controls.Add(this.total);
            this.Controls.Add(this.scrub);
            this.Controls.Add(this.elapsed);
            this.Controls.Add(this.play);
            this.Controls.Add(this.mapRemove);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.browse);
            this.Name = "Form1";
            this.Text = "MIDI Playback";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.scrub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapInst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapOctave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button mapRemove;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Label elapsed;
        private System.Windows.Forms.TrackBar scrub;
        private System.Windows.Forms.Label total;
        private System.Windows.Forms.ListView listChannels;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.NumericUpDown mapInst;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown mapOctave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox mapStrat;
        private System.Windows.Forms.Button mapPreviewLocal;
        private System.Windows.Forms.Button mapAdd;
        private System.Windows.Forms.CheckBox mapStaccatoRepeats;
        private System.Windows.Forms.ListView listMappings;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ComboBox comPort;
        private System.Windows.Forms.Button comOpen;
        private System.Windows.Forms.Button mapToggle;
        private System.Windows.Forms.Button allOff;
        private System.Windows.Forms.Button home0;
        private System.Windows.Forms.Button home1;
        private System.Windows.Forms.Button home2;
        private System.Windows.Forms.NumericUpDown notePreview;
        private System.Windows.Forms.Button noteOff;
        private System.Windows.Forms.Button home3;
        private System.Windows.Forms.TextBox skipTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button embedPlay;
        private System.Windows.Forms.Button embedStop;
    }
}

