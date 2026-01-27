namespace QdLayout
{
    partial class PlateGroupSet
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
            components = new System.ComponentModel.Container();
            DrawPlan = new Panel();
            contextMenuStrip1 = new ContextMenuStrip(components);
            插入房间前ToolStripMenuItem = new ToolStripMenuItem();
            插入房间后ToolStripMenuItem = new ToolStripMenuItem();
            交换房间ToolStripMenuItem = new ToolStripMenuItem();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            button4 = new Button();
            label2 = new Label();
            comboBox2 = new ComboBox();
            label3 = new Label();
            comboBox3 = new ComboBox();
            cbxBuildGroups = new CheckedListBox();
            btnImportRoomConfig = new Button();
            flpRoomConfigs = new FlowLayoutPanel();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // DrawPlan
            // 
            DrawPlan.BackColor = System.Drawing.Color.Black;
            DrawPlan.ContextMenuStrip = contextMenuStrip1;
            DrawPlan.Location = new Point(121, 50);
            DrawPlan.Name = "DrawPlan";
            DrawPlan.Size = new Size(1117, 850);
            DrawPlan.TabIndex = 0;
            DrawPlan.Click += DrawPlan_Click;
            DrawPlan.Paint += DrawPlan_Paint;
            DrawPlan.MouseDown += DrawPlan_MouseDown;
            DrawPlan.MouseMove += DrawPlan_MouseMove;
            DrawPlan.MouseUp += DrawPlan_MouseUp;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 插入房间前ToolStripMenuItem, 插入房间后ToolStripMenuItem, 交换房间ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(161, 70);
            // 
            // 插入房间前ToolStripMenuItem
            // 
            插入房间前ToolStripMenuItem.Name = "插入房间前ToolStripMenuItem";
            插入房间前ToolStripMenuItem.Size = new Size(160, 22);
            插入房间前ToolStripMenuItem.Text = "插入绿色房间前";
            插入房间前ToolStripMenuItem.Click += 插入房间前ToolStripMenuItem_Click;
            // 
            // 插入房间后ToolStripMenuItem
            // 
            插入房间后ToolStripMenuItem.Name = "插入房间后ToolStripMenuItem";
            插入房间后ToolStripMenuItem.Size = new Size(160, 22);
            插入房间后ToolStripMenuItem.Text = "插入绿色房间后";
            插入房间后ToolStripMenuItem.Click += 插入房间后ToolStripMenuItem_Click;
            // 
            // 交换房间ToolStripMenuItem
            // 
            交换房间ToolStripMenuItem.Name = "交换房间ToolStripMenuItem";
            交换房间ToolStripMenuItem.Size = new Size(160, 22);
            交换房间ToolStripMenuItem.Text = "交换房间";
            交换房间ToolStripMenuItem.Click += 交换房间ToolStripMenuItem_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 13);
            button1.Name = "button1";
            button1.Size = new Size(122, 23);
            button1.TabIndex = 1;
            button1.Text = "设置人员房间参数";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(1124, 12);
            button2.Name = "button2";
            button2.Size = new Size(97, 23);
            button2.TabIndex = 2;
            button2.Text = "确认";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1, 50);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 8;
            label1.Text = "选择编辑楼层";
            // 
            // button4
            // 
            button4.Location = new Point(333, 11);
            button4.Name = "button4";
            button4.Size = new Size(97, 23);
            button4.TabIndex = 39;
            button4.Text = "自动布置";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(140, 16);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 40;
            label2.Text = "排布规则：";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "U型默认排布规则" });
            comboBox2.Location = new Point(205, 11);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(122, 25);
            comboBox2.TabIndex = 41;
            comboBox2.Text = "U型默认排布规则";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(463, 13);
            label3.Name = "label3";
            label3.Size = new Size(116, 17);
            label3.TabIndex = 42;
            label3.Text = "选择图纸显示的楼层";
            label3.Click += label3_Click;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "1", "2" });
            comboBox3.Location = new Point(585, 10);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(41, 25);
            comboBox3.TabIndex = 43;
            comboBox3.Text = "1";
            // 
            // cbxBuildGroups
            // 
            cbxBuildGroups.FormattingEnabled = true;
            cbxBuildGroups.Location = new Point(1, 71);
            cbxBuildGroups.Name = "cbxBuildGroups";
            cbxBuildGroups.Size = new Size(114, 832);
            cbxBuildGroups.TabIndex = 44;
            cbxBuildGroups.ItemCheck += cbxBuildGroups_ItemCheck;
            cbxBuildGroups.SelectedIndexChanged += cbxBuildGroups_SelectedIndexChanged;
            // 
            // btnImportRoomConfig
            // 
            btnImportRoomConfig.Location = new Point(1294, 50);
            btnImportRoomConfig.Name = "btnImportRoomConfig";
            btnImportRoomConfig.Size = new Size(97, 23);
            btnImportRoomConfig.TabIndex = 45;
            btnImportRoomConfig.Text = "导入房间配置";
            btnImportRoomConfig.UseVisualStyleBackColor = true;
            btnImportRoomConfig.Click += btnImportRoomConfig_Click;
            // 
            // flpRoomConfigs
            // 
            flpRoomConfigs.BorderStyle = BorderStyle.FixedSingle;
            flpRoomConfigs.FlowDirection = FlowDirection.TopDown;
            flpRoomConfigs.Location = new Point(1244, 79);
            flpRoomConfigs.Name = "flpRoomConfigs";
            flpRoomConfigs.Size = new Size(200, 821);
            flpRoomConfigs.TabIndex = 46;
            // 
            // PlateGroupSet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1453, 913);
            Controls.Add(flpRoomConfigs);
            Controls.Add(btnImportRoomConfig);
            Controls.Add(cbxBuildGroups);
            Controls.Add(comboBox3);
            Controls.Add(label3);
            Controls.Add(comboBox2);
            Controls.Add(label2);
            Controls.Add(button4);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(DrawPlan);
            Name = "PlateGroupSet";
            Text = "编辑板房排布";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel DrawPlan;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 插入房间前ToolStripMenuItem;
        private ToolStripMenuItem 插入房间后ToolStripMenuItem;
        private ToolStripMenuItem 交换房间ToolStripMenuItem;
        private Button button1;
        private Button button2;
        private Label label1;
        private Button button4;
        private Label label2;
        private ComboBox comboBox2;
        private Label label3;
        private ComboBox comboBox3;
        private CheckedListBox cbxBuildGroups;
        private Button btnImportRoomConfig;
        private FlowLayoutPanel flpRoomConfigs;
    }
}