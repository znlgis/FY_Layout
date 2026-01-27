namespace QdLayout
{
    partial class PlateSet
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
            btSetMinGrid = new Button();
            BtGetMinGrid = new Button();
            BtCombineGrid = new Button();
            button1 = new Button();
            comboBox1 = new ComboBox();
            label1 = new Label();
            buildstyle = new ComboBox();
            label7 = new Label();
            stairwidth = new TextBox();
            label5 = new Label();
            label6 = new Label();
            statirlength = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label4 = new Label();
            passwaywidth = new TextBox();
            rommwith = new TextBox();
            roomlength = new TextBox();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button3 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            tabPage2 = new TabPage();
            button10 = new Button();
            button5 = new Button();
            button6 = new Button();
            listBox2 = new ListBox();
            dataGridView2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            button11 = new Button();
            button7 = new Button();
            button8 = new Button();
            listBox3 = new ListBox();
            dataGridView3 = new DataGridView();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            label9 = new Label();
            textBox4 = new TextBox();
            button4 = new Button();
            button9 = new Button();
            button12 = new Button();
            button13 = new Button();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            SuspendLayout();
            // 
            // DrawPlan
            // 
            DrawPlan.BackColor = System.Drawing.Color.Black;
            DrawPlan.ContextMenuStrip = contextMenuStrip1;
            DrawPlan.Location = new Point(220, 247);
            DrawPlan.Name = "DrawPlan";
            DrawPlan.Size = new Size(817, 450);
            DrawPlan.TabIndex = 0;
            DrawPlan.Click += DrawPlan_Click;
            DrawPlan.Paint += DrawPlan_Paint;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 插入房间前ToolStripMenuItem, 插入房间后ToolStripMenuItem, 交换房间ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 92);
            // 
            // 插入房间前ToolStripMenuItem
            // 
            插入房间前ToolStripMenuItem.Name = "插入房间前ToolStripMenuItem";
            插入房间前ToolStripMenuItem.Size = new Size(180, 22);
            插入房间前ToolStripMenuItem.Text = "插入绿色房间前";
            插入房间前ToolStripMenuItem.Click += 插入房间前ToolStripMenuItem_Click;
            // 
            // 插入房间后ToolStripMenuItem
            // 
            插入房间后ToolStripMenuItem.Name = "插入房间后ToolStripMenuItem";
            插入房间后ToolStripMenuItem.Size = new Size(180, 22);
            插入房间后ToolStripMenuItem.Text = "插入绿色房间后";
            插入房间后ToolStripMenuItem.Click += 插入房间后ToolStripMenuItem_Click;
            // 
            // 交换房间ToolStripMenuItem
            // 
            交换房间ToolStripMenuItem.Name = "交换房间ToolStripMenuItem";
            交换房间ToolStripMenuItem.Size = new Size(180, 22);
            交换房间ToolStripMenuItem.Text = "交换房间";
            交换房间ToolStripMenuItem.Click += 交换房间ToolStripMenuItem_Click;
            // 
            // btSetMinGrid
            // 
            btSetMinGrid.Location = new Point(567, 725);
            btSetMinGrid.Name = "btSetMinGrid";
            btSetMinGrid.Size = new Size(159, 23);
            btSetMinGrid.TabIndex = 1;
            btSetMinGrid.Text = "设置最小网格大小";
            btSetMinGrid.UseVisualStyleBackColor = true;
            btSetMinGrid.Click += btSetMinGrid_Click;
            // 
            // BtGetMinGrid
            // 
            BtGetMinGrid.Location = new Point(758, 725);
            BtGetMinGrid.Name = "BtGetMinGrid";
            BtGetMinGrid.Size = new Size(80, 23);
            BtGetMinGrid.TabIndex = 2;
            BtGetMinGrid.Text = "网格化";
            BtGetMinGrid.UseVisualStyleBackColor = true;
            BtGetMinGrid.Click += BtGetMinGrid_Click;
            // 
            // BtCombineGrid
            // 
            BtCombineGrid.Location = new Point(866, 725);
            BtCombineGrid.Name = "BtCombineGrid";
            BtCombineGrid.Size = new Size(166, 23);
            BtCombineGrid.TabIndex = 3;
            BtCombineGrid.Text = "合并选中成为新房间";
            BtCombineGrid.UseVisualStyleBackColor = true;
            BtCombineGrid.Click += BtCombineGrid_Click;
            // 
            // button1
            // 
            button1.Location = new Point(906, 119);
            button1.Name = "button1";
            button1.Size = new Size(78, 59);
            button1.TabIndex = 4;
            button1.Text = "保存";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "1" });
            comboBox1.Location = new Point(22, 267);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(168, 25);
            comboBox1.TabIndex = 5;
            comboBox1.Text = "1";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 247);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 6;
            label1.Text = "选择编辑楼层";
            // 
            // buildstyle
            // 
            buildstyle.FormattingEnabled = true;
            buildstyle.Items.AddRange(new object[] { "一", "L", "U" });
            buildstyle.Location = new Point(84, 372);
            buildstyle.Name = "buildstyle";
            buildstyle.Size = new Size(108, 25);
            buildstyle.TabIndex = 31;
            buildstyle.Text = "一";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(22, 372);
            label7.Name = "label7";
            label7.Size = new Size(56, 17);
            label7.TabIndex = 30;
            label7.Text = "排布方式";
            // 
            // stairwidth
            // 
            stairwidth.Location = new Point(84, 563);
            stairwidth.Name = "stairwidth";
            stairwidth.Size = new Size(100, 23);
            stairwidth.TabIndex = 29;
            stairwidth.Text = "1000";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 566);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 28;
            label5.Text = "楼梯宽度";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(22, 534);
            label6.Name = "label6";
            label6.Size = new Size(56, 17);
            label6.TabIndex = 27;
            label6.Text = "楼梯长度";
            // 
            // statirlength
            // 
            statirlength.Location = new Point(84, 534);
            statirlength.Name = "statirlength";
            statirlength.Size = new Size(100, 23);
            statirlength.TabIndex = 26;
            statirlength.Text = "3000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(22, 492);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 25;
            label3.Text = "走廊宽度";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 456);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 24;
            label2.Text = "房间宽度";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 422);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 23;
            label4.Text = "房间长度";
            // 
            // passwaywidth
            // 
            passwaywidth.Location = new Point(84, 489);
            passwaywidth.Name = "passwaywidth";
            passwaywidth.Size = new Size(100, 23);
            passwaywidth.TabIndex = 22;
            passwaywidth.Text = "1000";
            // 
            // rommwith
            // 
            rommwith.Location = new Point(87, 453);
            rommwith.Name = "rommwith";
            rommwith.Size = new Size(100, 23);
            rommwith.TabIndex = 21;
            rommwith.Text = "3720";
            // 
            // roomlength
            // 
            roomlength.Location = new Point(87, 419);
            roomlength.Name = "roomlength";
            roomlength.Size = new Size(100, 23);
            roomlength.TabIndex = 20;
            roomlength.Text = "5620";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dataGridView1.Location = new Point(221, 6);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(661, 172);
            dataGridView1.TabIndex = 32;
            // 
            // Column1
            // 
            Column1.FillWeight = 200F;
            Column1.HeaderText = "房间名称";
            Column1.Name = "Column1";
            Column1.Width = 200;
            // 
            // Column2
            // 
            Column2.HeaderText = "房间数量";
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.HeaderText = "占用间数";
            Column3.Name = "Column3";
            // 
            // Column4
            // 
            Column4.HeaderText = "已排布";
            Column4.Name = "Column4";
            // 
            // Column5
            // 
            Column5.HeaderText = "未排布";
            Column5.Name = "Column5";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(20, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1012, 214);
            tabControl1.TabIndex = 35;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(listBox1);
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Controls.Add(button1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1004, 184);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "办公";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(115, 155);
            button3.Name = "button3";
            button3.Size = new Size(83, 23);
            button3.TabIndex = 35;
            button3.Text = "删除楼栋";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 155);
            button2.Name = "button2";
            button2.Size = new Size(83, 23);
            button2.TabIndex = 34;
            button2.Text = "添加楼栋";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(12, 13);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(186, 140);
            listBox1.TabIndex = 33;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button10);
            tabPage2.Controls.Add(button5);
            tabPage2.Controls.Add(button6);
            tabPage2.Controls.Add(listBox2);
            tabPage2.Controls.Add(dataGridView2);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1004, 184);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "宿舍";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Location = new Point(909, 119);
            button10.Name = "button10";
            button10.Size = new Size(78, 59);
            button10.TabIndex = 40;
            button10.Text = "保存";
            button10.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(134, 155);
            button5.Name = "button5";
            button5.Size = new Size(83, 23);
            button5.TabIndex = 39;
            button5.Text = "删除楼栋";
            button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(31, 155);
            button6.Name = "button6";
            button6.Size = new Size(83, 23);
            button6.TabIndex = 38;
            button6.Text = "添加楼栋";
            button6.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 17;
            listBox2.Location = new Point(31, 13);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(186, 140);
            listBox2.TabIndex = 37;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8 });
            dataGridView2.Location = new Point(240, 6);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(632, 172);
            dataGridView2.TabIndex = 36;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.FillWeight = 200F;
            dataGridViewTextBoxColumn1.HeaderText = "房间名称";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "房间数量";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "占用间数";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "已排布";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "未排布";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(button11);
            tabPage3.Controls.Add(button7);
            tabPage3.Controls.Add(button8);
            tabPage3.Controls.Add(listBox3);
            tabPage3.Controls.Add(dataGridView3);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1004, 184);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "餐厅";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Location = new Point(899, 116);
            button11.Name = "button11";
            button11.Size = new Size(78, 59);
            button11.TabIndex = 40;
            button11.Text = "保存";
            button11.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(112, 152);
            button7.Name = "button7";
            button7.Size = new Size(92, 23);
            button7.TabIndex = 39;
            button7.Text = "删除楼栋";
            button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Location = new Point(9, 152);
            button8.Name = "button8";
            button8.Size = new Size(92, 23);
            button8.TabIndex = 38;
            button8.Text = "添加楼栋";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 17;
            listBox3.Location = new Point(9, 10);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(195, 140);
            listBox3.TabIndex = 37;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10 });
            dataGridView3.Location = new Point(246, 3);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(624, 172);
            dataGridView3.TabIndex = 36;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.FillWeight = 200F;
            dataGridViewTextBoxColumn4.HeaderText = "房间名称";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "房间数量";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "占用间数";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "已排布";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "未排布";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(24, 600);
            label9.Name = "label9";
            label9.Size = new Size(56, 17);
            label9.TabIndex = 36;
            label9.Text = "标准间数";
            label9.Click += label9_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(82, 597);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 37;
            // 
            // button4
            // 
            button4.Location = new Point(107, 657);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 38;
            button4.Text = "自动布置";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button9
            // 
            button9.Location = new Point(368, 725);
            button9.Name = "button9";
            button9.Size = new Size(167, 23);
            button9.TabIndex = 40;
            button9.Text = "将当前楼栋插入图纸";
            button9.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Location = new Point(28, 309);
            button12.Name = "button12";
            button12.Size = new Size(75, 23);
            button12.TabIndex = 41;
            button12.Text = "添加楼层";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button13
            // 
            button13.Location = new Point(109, 309);
            button13.Name = "button13";
            button13.Size = new Size(75, 23);
            button13.TabIndex = 42;
            button13.Text = "减少楼层";
            button13.UseVisualStyleBackColor = true;
            // 
            // PlateSet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1049, 762);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button9);
            Controls.Add(button4);
            Controls.Add(textBox4);
            Controls.Add(label9);
            Controls.Add(tabControl1);
            Controls.Add(buildstyle);
            Controls.Add(label7);
            Controls.Add(stairwidth);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(statirlength);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(passwaywidth);
            Controls.Add(rommwith);
            Controls.Add(roomlength);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(BtCombineGrid);
            Controls.Add(BtGetMinGrid);
            Controls.Add(btSetMinGrid);
            Controls.Add(DrawPlan);
            Name = "PlateSet";
            Text = "PlateSet";
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel DrawPlan;
        private Button btSetMinGrid;
        private Button BtGetMinGrid;
        private Button BtCombineGrid;
        private Button button1;
        private ComboBox comboBox1;
        private Label label1;
        private ComboBox buildstyle;
        private Label label7;
        private TextBox stairwidth;
        private Label label5;
        private Label label6;
        private TextBox statirlength;
        private Label label3;
        private Label label2;
        private Label label4;
        private TextBox passwaywidth;
        private TextBox rommwith;
        private TextBox roomlength;
        private DataGridView dataGridView1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private Button button3;
        private Button button2;
        private ListBox listBox1;
        private Label label9;
        private TextBox textBox4;
        private Button button4;
        private Button button5;
        private Button button6;
        private ListBox listBox2;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private Button button7;
        private Button button8;
        private ListBox listBox3;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 插入房间前ToolStripMenuItem;
        private ToolStripMenuItem 插入房间后ToolStripMenuItem;
        private ToolStripMenuItem 交换房间ToolStripMenuItem;
    }
}