namespace QdLayout
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
            dataGridView1 = new DataGridView();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox5 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            textBox6 = new TextBox();
            label7 = new Label();
            label8 = new Label();
            txtBuildNum = new TextBox();
            button1 = new Button();
            label4 = new Label();
            comboBox2 = new ComboBox();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dataGridView1.Location = new Point(174, 47);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(449, 282);
            dataGridView1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(63, 145);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(105, 23);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(63, 174);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(105, 23);
            textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(60, 215);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(108, 23);
            textBox3.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-2, 148);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 4;
            label1.Text = "房间长度";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(-2, 177);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 5;
            label2.Text = "房间宽度";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(-2, 218);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 6;
            label3.Text = "走廊宽度";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(60, 289);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(108, 23);
            textBox5.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(-2, 292);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 11;
            label5.Text = "楼梯宽度";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(-2, 260);
            label6.Name = "label6";
            label6.Size = new Size(56, 17);
            label6.TabIndex = 10;
            label6.Text = "楼梯长度";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(60, 260);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(108, 23);
            textBox6.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(-2, 98);
            label7.Name = "label7";
            label7.Size = new Size(56, 17);
            label7.TabIndex = 13;
            label7.Text = "排布方式";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(-2, 60);
            label8.Name = "label8";
            label8.Size = new Size(56, 17);
            label8.TabIndex = 15;
            label8.Text = "楼层数量";
            // 
            // txtBuildNum
            // 
            txtBuildNum.Location = new Point(60, 57);
            txtBuildNum.Name = "txtBuildNum";
            txtBuildNum.Size = new Size(108, 23);
            txtBuildNum.TabIndex = 16;
            // 
            // button1
            // 
            button1.Location = new Point(548, 335);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 17;
            button1.Text = "确定";
            button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(174, 18);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 18;
            label4.Text = "房间列表";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(60, 98);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(108, 25);
            comboBox2.TabIndex = 19;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 366);
            Controls.Add(comboBox2);
            Controls.Add(label4);
            Controls.Add(button1);
            Controls.Add(txtBuildNum);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textBox5);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(textBox6);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "定义建筑信息";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox5;
        private Label label5;
        private Label label6;
        private TextBox textBox6;
        private Label label7;
        private ComboBox comboBox1;
        private Label label8;
        private TextBox txtBuildNum;
        private Button button1;
        private Label label4;
        private ComboBox comboBox2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
    }
}