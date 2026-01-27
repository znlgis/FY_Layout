namespace QdLayout
{
    partial class EditArrangeWindow
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
            combRoomType = new ComboBox();
            rb1Shape = new RadioButton();
            lRoomType = new Label();
            ArrangeScheme = new Label();
            rbLshape = new RadioButton();
            lRoomSize = new Label();
            combRoomSize = new ComboBox();
            rbUshape = new RadioButton();
            btnCancle = new Button();
            btnSave = new Button();
            SuspendLayout();
            // 
            // combRoomType
            // 
            combRoomType.FormattingEnabled = true;
            combRoomType.Items.AddRange(new object[] { "K式房", "箱式房" });
            combRoomType.Location = new Point(69, 12);
            combRoomType.Name = "combRoomType";
            combRoomType.Size = new Size(98, 25);
            combRoomType.TabIndex = 9;
            // 
            // rb1Shape
            // 
            rb1Shape.AutoSize = true;
            rb1Shape.Location = new Point(69, 49);
            rb1Shape.Name = "rb1Shape";
            rb1Shape.Size = new Size(62, 21);
            rb1Shape.TabIndex = 13;
            rb1Shape.Text = "一字型";
            rb1Shape.UseVisualStyleBackColor = true;
            // 
            // lRoomType
            // 
            lRoomType.AutoSize = true;
            lRoomType.Location = new Point(31, 15);
            lRoomType.Name = "lRoomType";
            lRoomType.Size = new Size(32, 17);
            lRoomType.TabIndex = 8;
            lRoomType.Text = "房型";
            // 
            // ArrangeScheme
            // 
            ArrangeScheme.AccessibleDescription = "";
            ArrangeScheme.AutoSize = true;
            ArrangeScheme.Location = new Point(7, 53);
            ArrangeScheme.Name = "ArrangeScheme";
            ArrangeScheme.Size = new Size(56, 17);
            ArrangeScheme.TabIndex = 12;
            ArrangeScheme.Text = "排布方案";
            // 
            // rbLshape
            // 
            rbLshape.AutoSize = true;
            rbLshape.Checked = true;
            rbLshape.Location = new Point(160, 49);
            rbLshape.Name = "rbLshape";
            rbLshape.Size = new Size(44, 21);
            rbLshape.TabIndex = 14;
            rbLshape.TabStop = true;
            rbLshape.Text = "L型";
            rbLshape.UseVisualStyleBackColor = true;
            // 
            // lRoomSize
            // 
            lRoomSize.AutoSize = true;
            lRoomSize.Location = new Point(173, 15);
            lRoomSize.Name = "lRoomSize";
            lRoomSize.Size = new Size(56, 17);
            lRoomSize.TabIndex = 10;
            lRoomSize.Text = "房间规格";
            // 
            // combRoomSize
            // 
            combRoomSize.FormattingEnabled = true;
            combRoomSize.Items.AddRange(new object[] { "3.3*3.3", "3.24*6.26" });
            combRoomSize.Location = new Point(235, 12);
            combRoomSize.Name = "combRoomSize";
            combRoomSize.Size = new Size(98, 25);
            combRoomSize.TabIndex = 11;
            // 
            // rbUshape
            // 
            rbUshape.AutoSize = true;
            rbUshape.Location = new Point(235, 49);
            rbUshape.Name = "rbUshape";
            rbUshape.Size = new Size(47, 21);
            rbUshape.TabIndex = 15;
            rbUshape.Text = "U型";
            rbUshape.UseVisualStyleBackColor = true;
            // 
            // btnCancle
            // 
            btnCancle.Location = new Point(248, 88);
            btnCancle.Name = "btnCancle";
            btnCancle.Size = new Size(85, 25);
            btnCancle.TabIndex = 17;
            btnCancle.Text = "取消";
            btnCancle.UseVisualStyleBackColor = true;
            btnCancle.Click += btnCancle_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(157, 88);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(85, 25);
            btnSave.TabIndex = 16;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // EditArrangeWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 124);
            Controls.Add(btnCancle);
            Controls.Add(btnSave);
            Controls.Add(combRoomType);
            Controls.Add(rb1Shape);
            Controls.Add(lRoomType);
            Controls.Add(ArrangeScheme);
            Controls.Add(rbLshape);
            Controls.Add(lRoomSize);
            Controls.Add(combRoomSize);
            Controls.Add(rbUshape);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "EditArrangeWindow";
            ShowIcon = false;
            Text = "编辑";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox combRoomType;
        private RadioButton rb1Shape;
        private Label lRoomType;
        private Label ArrangeScheme;
        private RadioButton rbLshape;
        private Label lRoomSize;
        private ComboBox combRoomSize;
        private RadioButton rbUshape;
        private Button btnCancle;
        private Button btnSave;
    }
}