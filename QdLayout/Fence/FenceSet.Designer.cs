namespace QdLayout.Fence
{
    partial class FenceSet
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
            lblFenceStyle = new Label();
            lblFenceWeight = new Label();
            lblFenceHeight = new Label();
            lblFenceColumnInterval = new Label();
            lblFenceColumnStyle = new Label();
            lblFenceTopStyle = new Label();
            panel1 = new Panel();
            btFence = new Button();
            cboFenceStyle = new ComboBox();
            txtFenceWeight = new TextBox();
            txtFenceHeight = new TextBox();
            txtFenceColumnInterval = new TextBox();
            cboFenceColumnStyle = new ComboBox();
            cboFenceTopStyle = new ComboBox();
            txtFenceColumnHeight = new TextBox();
            lblFenceColumnHeight = new Label();
            lblFenceColor = new Label();
            lblFenceColumnColor = new Label();
            cboFenceColor = new ComboBox();
            cboFenceColumnColor = new ComboBox();
            SuspendLayout();
            // 
            // lblFenceStyle
            // 
            lblFenceStyle.AutoSize = true;
            lblFenceStyle.Location = new Point(76, 279);
            lblFenceStyle.Name = "lblFenceStyle";
            lblFenceStyle.Size = new Size(68, 17);
            lblFenceStyle.TabIndex = 0;
            lblFenceStyle.Text = "围墙样式：";
            lblFenceStyle.Visible = false;
            // 
            // lblFenceWeight
            // 
            lblFenceWeight.AutoSize = true;
            lblFenceWeight.Location = new Point(76, 58);
            lblFenceWeight.Name = "lblFenceWeight";
            lblFenceWeight.Size = new Size(92, 17);
            lblFenceWeight.TabIndex = 1;
            lblFenceWeight.Text = "墙厚（MM）：";
            // 
            // lblFenceHeight
            // 
            lblFenceHeight.AutoSize = true;
            lblFenceHeight.Location = new Point(76, 92);
            lblFenceHeight.Name = "lblFenceHeight";
            lblFenceHeight.Size = new Size(92, 17);
            lblFenceHeight.TabIndex = 2;
            lblFenceHeight.Text = "墙高（MM）：";
            // 
            // lblFenceColumnInterval
            // 
            lblFenceColumnInterval.AutoSize = true;
            lblFenceColumnInterval.Location = new Point(64, 157);
            lblFenceColumnInterval.Name = "lblFenceColumnInterval";
            lblFenceColumnInterval.Size = new Size(104, 17);
            lblFenceColumnInterval.TabIndex = 3;
            lblFenceColumnInterval.Text = "柱间距（MM）：";
            // 
            // lblFenceColumnStyle
            // 
            lblFenceColumnStyle.AutoSize = true;
            lblFenceColumnStyle.Location = new Point(76, 313);
            lblFenceColumnStyle.Name = "lblFenceColumnStyle";
            lblFenceColumnStyle.Size = new Size(56, 17);
            lblFenceColumnStyle.TabIndex = 4;
            lblFenceColumnStyle.Text = "柱样式：";
            lblFenceColumnStyle.Visible = false;
            // 
            // lblFenceTopStyle
            // 
            lblFenceTopStyle.AutoSize = true;
            lblFenceTopStyle.Location = new Point(76, 251);
            lblFenceTopStyle.Name = "lblFenceTopStyle";
            lblFenceTopStyle.Size = new Size(68, 17);
            lblFenceTopStyle.TabIndex = 5;
            lblFenceTopStyle.Text = "压顶样式：";
            lblFenceTopStyle.Visible = false;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Black;
            panel1.Location = new Point(359, 58);
            panel1.Name = "panel1";
            panel1.Size = new Size(405, 250);
            panel1.TabIndex = 6;
            panel1.Visible = false;
            // 
            // btFence
            // 
            btFence.Location = new Point(27, 322);
            btFence.Name = "btFence";
            btFence.Size = new Size(312, 36);
            btFence.TabIndex = 7;
            btFence.Text = "绘制围墙";
            btFence.UseVisualStyleBackColor = true;
            btFence.Click += btFence_Click;
            // 
            // cboFenceStyle
            // 
            cboFenceStyle.FormattingEnabled = true;
            cboFenceStyle.Items.AddRange(new object[] { "默认" });
            cboFenceStyle.Location = new Point(161, 279);
            cboFenceStyle.Name = "cboFenceStyle";
            cboFenceStyle.Size = new Size(121, 25);
            cboFenceStyle.TabIndex = 8;
            cboFenceStyle.Visible = false;
            // 
            // txtFenceWeight
            // 
            txtFenceWeight.Location = new Point(186, 59);
            txtFenceWeight.Name = "txtFenceWeight";
            txtFenceWeight.Size = new Size(100, 23);
            txtFenceWeight.TabIndex = 9;
            txtFenceWeight.Text = "240";
            // 
            // txtFenceHeight
            // 
            txtFenceHeight.Location = new Point(185, 92);
            txtFenceHeight.Name = "txtFenceHeight";
            txtFenceHeight.Size = new Size(100, 23);
            txtFenceHeight.TabIndex = 10;
            txtFenceHeight.Text = "2500";
            // 
            // txtFenceColumnInterval
            // 
            txtFenceColumnInterval.Location = new Point(185, 157);
            txtFenceColumnInterval.Name = "txtFenceColumnInterval";
            txtFenceColumnInterval.Size = new Size(100, 23);
            txtFenceColumnInterval.TabIndex = 11;
            txtFenceColumnInterval.Text = "6000";
            // 
            // cboFenceColumnStyle
            // 
            cboFenceColumnStyle.FormattingEnabled = true;
            cboFenceColumnStyle.Items.AddRange(new object[] { "矩形柱", "圆形柱" });
            cboFenceColumnStyle.Location = new Point(140, 313);
            cboFenceColumnStyle.Name = "cboFenceColumnStyle";
            cboFenceColumnStyle.Size = new Size(121, 25);
            cboFenceColumnStyle.TabIndex = 12;
            cboFenceColumnStyle.Visible = false;
            // 
            // cboFenceTopStyle
            // 
            cboFenceTopStyle.FormattingEnabled = true;
            cboFenceTopStyle.Items.AddRange(new object[] { "琉璃瓦压顶" });
            cboFenceTopStyle.Location = new Point(162, 251);
            cboFenceTopStyle.Name = "cboFenceTopStyle";
            cboFenceTopStyle.Size = new Size(121, 25);
            cboFenceTopStyle.TabIndex = 13;
            cboFenceTopStyle.Visible = false;
            // 
            // txtFenceColumnHeight
            // 
            txtFenceColumnHeight.Location = new Point(185, 126);
            txtFenceColumnHeight.Name = "txtFenceColumnHeight";
            txtFenceColumnHeight.Size = new Size(100, 23);
            txtFenceColumnHeight.TabIndex = 15;
            txtFenceColumnHeight.Text = "2500";
            txtFenceColumnHeight.TextChanged += textBox1_TextChanged;
            // 
            // lblFenceColumnHeight
            // 
            lblFenceColumnHeight.AutoSize = true;
            lblFenceColumnHeight.Location = new Point(64, 126);
            lblFenceColumnHeight.Name = "lblFenceColumnHeight";
            lblFenceColumnHeight.Size = new Size(104, 17);
            lblFenceColumnHeight.TabIndex = 14;
            lblFenceColumnHeight.Text = "柱高度（MM）：";
            // 
            // lblFenceColor
            // 
            lblFenceColor.AutoSize = true;
            lblFenceColor.Location = new Point(100, 188);
            lblFenceColor.Name = "lblFenceColor";
            lblFenceColor.Size = new Size(68, 17);
            lblFenceColor.TabIndex = 16;
            lblFenceColor.Text = "围墙颜色：";
            // 
            // lblFenceColumnColor
            // 
            lblFenceColumnColor.AutoSize = true;
            lblFenceColumnColor.Location = new Point(100, 221);
            lblFenceColumnColor.Name = "lblFenceColumnColor";
            lblFenceColumnColor.Size = new Size(68, 17);
            lblFenceColumnColor.TabIndex = 17;
            lblFenceColumnColor.Text = "柱子颜色：";
            // 
            // cboFenceColor
            // 
            cboFenceColor.FormattingEnabled = true;
            cboFenceColor.Items.AddRange(new object[] { "琉璃瓦压顶" });
            cboFenceColor.Location = new Point(185, 188);
            cboFenceColor.Name = "cboFenceColor";
            cboFenceColor.Size = new Size(121, 25);
            cboFenceColor.TabIndex = 18;
            cboFenceColor.SelectedIndexChanged += cboFenceColor_SelectedIndexChanged;
            // 
            // cboFenceColumnColor
            // 
            cboFenceColumnColor.FormattingEnabled = true;
            cboFenceColumnColor.Items.AddRange(new object[] { "琉璃瓦压顶" });
            cboFenceColumnColor.Location = new Point(185, 220);
            cboFenceColumnColor.Name = "cboFenceColumnColor";
            cboFenceColumnColor.Size = new Size(121, 25);
            cboFenceColumnColor.TabIndex = 19;
            cboFenceColumnColor.SelectedIndexChanged += cboFenceColumnColor_SelectedIndexChanged;
            // 
            // FenceSet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(399, 379);
            Controls.Add(cboFenceColumnColor);
            Controls.Add(cboFenceColor);
            Controls.Add(lblFenceColumnColor);
            Controls.Add(lblFenceColor);
            Controls.Add(txtFenceColumnHeight);
            Controls.Add(lblFenceColumnHeight);
            Controls.Add(cboFenceTopStyle);
            Controls.Add(cboFenceColumnStyle);
            Controls.Add(txtFenceColumnInterval);
            Controls.Add(txtFenceHeight);
            Controls.Add(txtFenceWeight);
            Controls.Add(cboFenceStyle);
            Controls.Add(btFence);
            Controls.Add(panel1);
            Controls.Add(lblFenceTopStyle);
            Controls.Add(lblFenceColumnStyle);
            Controls.Add(lblFenceColumnInterval);
            Controls.Add(lblFenceHeight);
            Controls.Add(lblFenceWeight);
            Controls.Add(lblFenceStyle);
            Name = "FenceSet";
            Text = "围墙";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFenceStyle;
        private Label lblFenceWeight;
        private Label lblFenceHeight;
        private Label lblFenceColumnInterval;
        private Label lblFenceColumnStyle;
        private Label lblFenceTopStyle;
        private Panel panel1;
        private Button btFence;
        private ComboBox cboFenceStyle;
        private TextBox txtFenceWeight;
        private TextBox txtFenceHeight;
        private TextBox txtFenceColumnInterval;
        private ComboBox cboFenceColumnStyle;
        private ComboBox cboFenceTopStyle;
        private TextBox txtFenceColumnHeight;
        private Label lblFenceColumnHeight;
        private Label lblFenceColor;
        private Label lblFenceColumnColor;
        private ComboBox cboFenceColor;
        private ComboBox cboFenceColumnColor;
    }
}