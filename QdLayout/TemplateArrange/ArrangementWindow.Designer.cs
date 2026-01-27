namespace QdLayout
{
    partial class ArrangementWindow
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            dgArrange = new DataGridView();
            Building = new DataGridViewTextBoxColumn();
            Room = new DataGridViewComboBoxColumn();
            Size = new DataGridViewComboBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            ArrangementMode = new DataGridViewComboBoxColumn();
            tgSemiAutoTemplate = new TabPage();
            panel1 = new Panel();
            p1 = new PictureBox();
            p2 = new PictureBox();
            p3 = new PictureBox();
            tabControl1 = new TabControl();
            tgAutoTemplate = new TabPage();
            btnSave = new Button();
            btnInsertCad = new Button();
            cb1 = new RadioButton();
            cb2 = new RadioButton();
            cb3 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)dgArrange).BeginInit();
            tgSemiAutoTemplate.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)p1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)p2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)p3).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // dgArrange
            // 
            dgArrange.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgArrange.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgArrange.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgArrange.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgArrange.Columns.AddRange(new DataGridViewColumn[] { Building, Room, Size, Data, ArrangementMode });
            dgArrange.Location = new Point(16, 12);
            dgArrange.Name = "dgArrange";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgArrange.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgArrange.RowsDefaultCellStyle = dataGridViewCellStyle6;
            dgArrange.Size = new Size(779, 125);
            dgArrange.TabIndex = 0;
            dgArrange.DoubleClick += dgArrange_DoubleClick;
            // 
            // Building
            // 
            Building.HeaderText = "楼栋";
            Building.Name = "Building";
            // 
            // Room
            // 
            Room.HeaderText = "房型";
            Room.Items.AddRange(new object[] { "K式房", "箱式房" });
            Room.Name = "Room";
            // 
            // Size
            // 
            Size.HeaderText = "规格";
            Size.Items.AddRange(new object[] { "2400*3200", "3200*3600", "3600*3900" });
            Size.Name = "Size";
            Size.Resizable = DataGridViewTriState.True;
            Size.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Data
            // 
            Data.HeaderText = "数量";
            Data.Name = "Data";
            // 
            // ArrangementMode
            // 
            ArrangementMode.HeaderText = "排布方式";
            ArrangementMode.Items.AddRange(new object[] { "一", "U", "L" });
            ArrangementMode.Name = "ArrangementMode";
            ArrangementMode.Resizable = DataGridViewTriState.True;
            ArrangementMode.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // tgSemiAutoTemplate
            // 
            tgSemiAutoTemplate.Controls.Add(panel1);
            tgSemiAutoTemplate.Location = new Point(4, 26);
            tgSemiAutoTemplate.Name = "tgSemiAutoTemplate";
            tgSemiAutoTemplate.Padding = new Padding(3);
            tgSemiAutoTemplate.Size = new Size(775, 418);
            tgSemiAutoTemplate.TabIndex = 0;
            tgSemiAutoTemplate.Text = "半自动模板";
            tgSemiAutoTemplate.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(cb3);
            panel1.Controls.Add(cb2);
            panel1.Controls.Add(cb1);
            panel1.Controls.Add(p1);
            panel1.Controls.Add(p2);
            panel1.Controls.Add(p3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(769, 412);
            panel1.TabIndex = 0;
            // 
            // p1
            // 
            p1.Image = Properties.Resources.办1;
            p1.Location = new Point(15, 27);
            p1.Name = "p1";
            p1.Size = new Size(240, 212);
            p1.SizeMode = PictureBoxSizeMode.StretchImage;
            p1.TabIndex = 4;
            p1.TabStop = false;
            // 
            // p2
            // 
            p2.Image = Properties.Resources.办2;
            p2.Location = new Point(273, 27);
            p2.Name = "p2";
            p2.Size = new Size(240, 212);
            p2.SizeMode = PictureBoxSizeMode.StretchImage;
            p2.TabIndex = 3;
            p2.TabStop = false;
            // 
            // p3
            // 
            p3.Image = Properties.Resources.办3;
            p3.Location = new Point(519, 27);
            p3.Name = "p3";
            p3.Size = new Size(240, 212);
            p3.SizeMode = PictureBoxSizeMode.StretchImage;
            p3.TabIndex = 2;
            p3.TabStop = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tgAutoTemplate);
            tabControl1.Controls.Add(tgSemiAutoTemplate);
            tabControl1.Location = new Point(16, 144);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(783, 448);
            tabControl1.TabIndex = 1;
            // 
            // tgAutoTemplate
            // 
            tgAutoTemplate.Location = new Point(4, 26);
            tgAutoTemplate.Name = "tgAutoTemplate";
            tgAutoTemplate.Size = new Size(775, 418);
            tgAutoTemplate.TabIndex = 1;
            tgAutoTemplate.Text = "自动模板";
            tgAutoTemplate.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(587, 637);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(85, 25);
            btnSave.TabIndex = 2;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnInsertCad
            // 
            btnInsertCad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnInsertCad.Location = new Point(678, 637);
            btnInsertCad.Name = "btnInsertCad";
            btnInsertCad.Size = new Size(151, 25);
            btnInsertCad.TabIndex = 3;
            btnInsertCad.Text = "生成选中方案，插入CAD";
            btnInsertCad.UseVisualStyleBackColor = true;
            btnInsertCad.Click += btnInsertCad_Click;
            // 
            // cb1
            // 
            cb1.AutoSize = true;
            cb1.Location = new Point(96, 274);
            cb1.Name = "cb1";
            cb1.Size = new Size(50, 21);
            cb1.TabIndex = 5;
            cb1.TabStop = true;
            cb1.Text = "选择";
            cb1.UseVisualStyleBackColor = true;
            // 
            // cb2
            // 
            cb2.AutoSize = true;
            cb2.Location = new Point(364, 274);
            cb2.Name = "cb2";
            cb2.Size = new Size(50, 21);
            cb2.TabIndex = 6;
            cb2.TabStop = true;
            cb2.Text = "选择";
            cb2.UseVisualStyleBackColor = true;
            // 
            // cb3
            // 
            cb3.AutoSize = true;
            cb3.Location = new Point(615, 274);
            cb3.Name = "cb3";
            cb3.Size = new Size(50, 21);
            cb3.TabIndex = 7;
            cb3.TabStop = true;
            cb3.Text = "选择";
            cb3.UseVisualStyleBackColor = true;
            // 
            // ArrangementWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 674);
            Controls.Add(btnInsertCad);
            Controls.Add(btnSave);
            Controls.Add(tabControl1);
            Controls.Add(dgArrange);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "ArrangementWindow";
            ShowIcon = false;
            Text = "选择模板生成方案";
            Load += ArrangementWindow_Load;
            ((System.ComponentModel.ISupportInitialize)dgArrange).EndInit();
            tgSemiAutoTemplate.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)p1).EndInit();
            ((System.ComponentModel.ISupportInitialize)p2).EndInit();
            ((System.ComponentModel.ISupportInitialize)p3).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgArrange;
        private TabPage tgSemiAutoTemplate;
        private TabControl tabControl1;
        private Panel panel1;
        private Button btnSave;
        private Button btnInsertCad;
        private DataGridViewTextBoxColumn Building;
        private DataGridViewComboBoxColumn Room;
        private DataGridViewComboBoxColumn Size;
        private DataGridViewTextBoxColumn Data;
        private DataGridViewComboBoxColumn ArrangementMode;
        private PictureBox p1;
        private PictureBox p2;
        private PictureBox p3;
        private TabPage tgAutoTemplate;
        private RadioButton cb3;
        private RadioButton cb2;
        private RadioButton cb1;
    }
}