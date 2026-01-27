namespace QdLayout
{
    partial class RoomConfigControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            checkBox1 = new CheckBox();
            lbRoomConfigName = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(194, 111);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += RoomConfigControl_MouseClick;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(60, 143);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(51, 21);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "选择";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.MouseClick += RoomConfigControl_MouseClick;
            // 
            // lbRoomConfigName
            // 
            lbRoomConfigName.AutoSize = true;
            lbRoomConfigName.Location = new Point(60, 117);
            lbRoomConfigName.Name = "lbRoomConfigName";
            lbRoomConfigName.Size = new Size(43, 17);
            lbRoomConfigName.TabIndex = 2;
            lbRoomConfigName.Text = "label1";
            lbRoomConfigName.MouseClick += RoomConfigControl_MouseClick;
            // 
            // RoomConfigControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lbRoomConfigName);
            Controls.Add(checkBox1);
            Controls.Add(pictureBox1);
            Name = "RoomConfigControl";
            Size = new Size(198, 165);
            MouseClick += RoomConfigControl_MouseClick;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private CheckBox checkBox1;
        private Label lbRoomConfigName;
    }
}
