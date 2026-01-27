namespace QdLayout
{
    partial class TemComAttriWindow
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
            combArea = new ComboBox();
            btnOk = new Button();
            btnCancle = new Button();
            SuspendLayout();
            // 
            // combArea
            // 
            combArea.FormattingEnabled = true;
            combArea.Location = new Point(12, 12);
            combArea.Name = "combArea";
            combArea.Size = new Size(188, 25);
            combArea.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(34, 56);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(80, 25);
            btnOk.TabIndex = 3;
            btnOk.Text = "确定";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancle
            // 
            btnCancle.Location = new Point(120, 56);
            btnCancle.Name = "btnCancle";
            btnCancle.Size = new Size(80, 25);
            btnCancle.TabIndex = 4;
            btnCancle.Text = "取消";
            btnCancle.UseVisualStyleBackColor = true;
            btnCancle.Click += btnCancle_Click;
            // 
            // TemComAttriWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(212, 93);
            Controls.Add(btnCancle);
            Controls.Add(btnOk);
            Controls.Add(combArea);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "TemComAttriWindow";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "功能区选择";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox1;
        private Button btnOk;
        private Button btnCancle;
        private ComboBox combArea;
    }
}