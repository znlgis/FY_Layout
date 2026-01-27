namespace QdLayout
{
    partial class AppearanceEditWindow
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
            lRoomType = new Label();
            combRoomType = new ComboBox();
            lRoomSize = new Label();
            combRoomSize = new ComboBox();
            ArrangeScheme = new Label();
            rb1Shape = new RadioButton();
            rbLshape = new RadioButton();
            rbUshape = new RadioButton();
            lLevel = new Label();
            panel1 = new Panel();
            pU = new Panel();
            cbUB4 = new CheckBox();
            cbUB3 = new CheckBox();
            cbUB2 = new CheckBox();
            rbBShape = new RadioButton();
            rbAShape = new RadioButton();
            panelB2 = new Panel();
            panelB0 = new Panel();
            panelB1 = new Panel();
            cbUB1 = new CheckBox();
            panelA2 = new Panel();
            cbUA3 = new CheckBox();
            panelA0 = new Panel();
            cbUA4 = new CheckBox();
            panelA1 = new Panel();
            cbUA1 = new CheckBox();
            cbUA2 = new CheckBox();
            pL = new Panel();
            rbLDShape = new RadioButton();
            rbLCShape = new RadioButton();
            rbLBShape = new RadioButton();
            rbLAShape = new RadioButton();
            cbLD2 = new CheckBox();
            cbLD3 = new CheckBox();
            panel4 = new Panel();
            panel8 = new Panel();
            cbLD1 = new CheckBox();
            cbLC3 = new CheckBox();
            panel5 = new Panel();
            panel7 = new Panel();
            cbLC1 = new CheckBox();
            cbLC2 = new CheckBox();
            cbLB2 = new CheckBox();
            cbLB3 = new CheckBox();
            panel3 = new Panel();
            panel9 = new Panel();
            cbLA3 = new CheckBox();
            panel10 = new Panel();
            cbLB1 = new CheckBox();
            panel12 = new Panel();
            cbLA1 = new CheckBox();
            cbLA2 = new CheckBox();
            p1 = new Panel();
            cb一A2 = new CheckBox();
            pl1 = new Panel();
            cb一A1 = new CheckBox();
            panel2 = new Panel();
            panel11 = new Panel();
            btnCancle = new Button();
            btnSave = new Button();
            combLevel = new TextBox();
            panel1.SuspendLayout();
            pU.SuspendLayout();
            pL.SuspendLayout();
            p1.SuspendLayout();
            panel2.SuspendLayout();
            panel11.SuspendLayout();
            SuspendLayout();
            // 
            // lRoomType
            // 
            lRoomType.AutoSize = true;
            lRoomType.Location = new Point(34, 15);
            lRoomType.Name = "lRoomType";
            lRoomType.Size = new Size(32, 17);
            lRoomType.TabIndex = 0;
            lRoomType.Text = "房型";
            // 
            // combRoomType
            // 
            combRoomType.FormattingEnabled = true;
            combRoomType.Items.AddRange(new object[] { "K式房", "箱式房" });
            combRoomType.Location = new Point(72, 12);
            combRoomType.Name = "combRoomType";
            combRoomType.Size = new Size(98, 25);
            combRoomType.TabIndex = 1;
            combRoomType.SelectedIndexChanged += combRoomType_SelectedIndexChanged;
            // 
            // lRoomSize
            // 
            lRoomSize.AutoSize = true;
            lRoomSize.Location = new Point(176, 15);
            lRoomSize.Name = "lRoomSize";
            lRoomSize.Size = new Size(56, 17);
            lRoomSize.TabIndex = 2;
            lRoomSize.Text = "房间规格";
            // 
            // combRoomSize
            // 
            combRoomSize.FormattingEnabled = true;
            combRoomSize.Items.AddRange(new object[] { "3000*3000", "3000*6000", "3300*3300", "3240*6260" });
            combRoomSize.Location = new Point(238, 12);
            combRoomSize.Name = "combRoomSize";
            combRoomSize.Size = new Size(98, 25);
            combRoomSize.TabIndex = 3;
            // 
            // ArrangeScheme
            // 
            ArrangeScheme.AccessibleDescription = "";
            ArrangeScheme.AutoSize = true;
            ArrangeScheme.Location = new Point(10, 53);
            ArrangeScheme.Name = "ArrangeScheme";
            ArrangeScheme.Size = new Size(56, 17);
            ArrangeScheme.TabIndex = 4;
            ArrangeScheme.Text = "排布方案";
            // 
            // rb1Shape
            // 
            rb1Shape.AutoSize = true;
            rb1Shape.Location = new Point(72, 49);
            rb1Shape.Name = "rb1Shape";
            rb1Shape.Size = new Size(62, 21);
            rb1Shape.TabIndex = 5;
            rb1Shape.Text = "一字型";
            rb1Shape.UseVisualStyleBackColor = true;
            rb1Shape.CheckedChanged += rb1Shape_CheckedChanged;
            // 
            // rbLshape
            // 
            rbLshape.AutoSize = true;
            rbLshape.Checked = true;
            rbLshape.Location = new Point(238, 51);
            rbLshape.Name = "rbLshape";
            rbLshape.Size = new Size(44, 21);
            rbLshape.TabIndex = 6;
            rbLshape.TabStop = true;
            rbLshape.Text = "L型";
            rbLshape.UseVisualStyleBackColor = true;
            rbLshape.CheckedChanged += rbLshape_CheckedChanged;
            // 
            // rbUshape
            // 
            rbUshape.AutoSize = true;
            rbUshape.Location = new Point(380, 51);
            rbUshape.Name = "rbUshape";
            rbUshape.Size = new Size(47, 21);
            rbUshape.TabIndex = 7;
            rbUshape.Text = "U型";
            rbUshape.UseVisualStyleBackColor = true;
            rbUshape.CheckedChanged += rbUshape_CheckedChanged;
            // 
            // lLevel
            // 
            lLevel.AutoSize = true;
            lLevel.Location = new Point(342, 15);
            lLevel.Name = "lLevel";
            lLevel.Size = new Size(32, 17);
            lLevel.TabIndex = 8;
            lLevel.Text = "楼层";
            // 
            // panel1
            // 
            panel1.Controls.Add(pU);
            panel1.Controls.Add(pL);
            panel1.Controls.Add(p1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 78);
            panel1.Name = "panel1";
            panel1.Size = new Size(607, 443);
            panel1.TabIndex = 10;
            // 
            // pU
            // 
            pU.Controls.Add(cbUB4);
            pU.Controls.Add(cbUB3);
            pU.Controls.Add(cbUB2);
            pU.Controls.Add(rbBShape);
            pU.Controls.Add(rbAShape);
            pU.Controls.Add(panelB2);
            pU.Controls.Add(panelB0);
            pU.Controls.Add(panelB1);
            pU.Controls.Add(cbUB1);
            pU.Controls.Add(panelA2);
            pU.Controls.Add(cbUA3);
            pU.Controls.Add(panelA0);
            pU.Controls.Add(cbUA4);
            pU.Controls.Add(panelA1);
            pU.Controls.Add(cbUA1);
            pU.Controls.Add(cbUA2);
            pU.Dock = DockStyle.Fill;
            pU.Location = new Point(0, 0);
            pU.Name = "pU";
            pU.Size = new Size(607, 443);
            pU.TabIndex = 20;
            // 
            // cbUB4
            // 
            cbUB4.AutoSize = true;
            cbUB4.Location = new Point(452, 411);
            cbUB4.Name = "cbUB4";
            cbUB4.Size = new Size(51, 21);
            cbUB4.TabIndex = 41;
            cbUB4.Text = "楼梯";
            cbUB4.UseVisualStyleBackColor = true;
            // 
            // cbUB3
            // 
            cbUB3.CheckAlign = ContentAlignment.TopCenter;
            cbUB3.ImageAlign = ContentAlignment.TopLeft;
            cbUB3.Location = new Point(501, 251);
            cbUB3.Name = "cbUB3";
            cbUB3.Size = new Size(23, 57);
            cbUB3.TabIndex = 40;
            cbUB3.Text = "楼梯";
            cbUB3.TextAlign = ContentAlignment.MiddleCenter;
            cbUB3.UseVisualStyleBackColor = true;
            // 
            // cbUB2
            // 
            cbUB2.CheckAlign = ContentAlignment.TopCenter;
            cbUB2.ImageAlign = ContentAlignment.TopLeft;
            cbUB2.Location = new Point(99, 251);
            cbUB2.Name = "cbUB2";
            cbUB2.Size = new Size(23, 57);
            cbUB2.TabIndex = 39;
            cbUB2.Text = "楼梯";
            cbUB2.TextAlign = ContentAlignment.MiddleCenter;
            cbUB2.UseVisualStyleBackColor = true;
            // 
            // rbBShape
            // 
            rbBShape.AutoSize = true;
            rbBShape.Location = new Point(27, 326);
            rbBShape.Name = "rbBShape";
            rbBShape.Size = new Size(46, 21);
            rbBShape.TabIndex = 38;
            rbBShape.Text = "B型";
            rbBShape.UseVisualStyleBackColor = true;
            // 
            // rbAShape
            // 
            rbAShape.AutoSize = true;
            rbAShape.Checked = true;
            rbAShape.Location = new Point(27, 86);
            rbAShape.Name = "rbAShape";
            rbAShape.Size = new Size(46, 21);
            rbAShape.TabIndex = 37;
            rbAShape.TabStop = true;
            rbAShape.Text = "A型";
            rbAShape.UseVisualStyleBackColor = true;
            // 
            // panelB2
            // 
            panelB2.BorderStyle = BorderStyle.FixedSingle;
            panelB2.Location = new Point(452, 24);
            panelB2.Margin = new Padding(0);
            panelB2.Name = "panelB2";
            panelB2.Size = new Size(46, 150);
            panelB2.TabIndex = 28;
            // 
            // panelB0
            // 
            panelB0.BorderStyle = BorderStyle.FixedSingle;
            panelB0.Location = new Point(126, 24);
            panelB0.Margin = new Padding(0);
            panelB0.Name = "panelB0";
            panelB0.Size = new Size(50, 150);
            panelB0.TabIndex = 27;
            // 
            // panelB1
            // 
            panelB1.BorderStyle = BorderStyle.FixedSingle;
            panelB1.Location = new Point(176, 24);
            panelB1.Margin = new Padding(0);
            panelB1.Name = "panelB1";
            panelB1.Size = new Size(276, 50);
            panelB1.TabIndex = 29;
            // 
            // cbUB1
            // 
            cbUB1.AutoSize = true;
            cbUB1.Location = new Point(126, 411);
            cbUB1.Name = "cbUB1";
            cbUB1.Size = new Size(51, 21);
            cbUB1.TabIndex = 31;
            cbUB1.Text = "楼梯";
            cbUB1.UseVisualStyleBackColor = true;
            // 
            // panelA2
            // 
            panelA2.BorderStyle = BorderStyle.FixedSingle;
            panelA2.Location = new Point(452, 301);
            panelA2.Margin = new Padding(0);
            panelA2.Name = "panelA2";
            panelA2.Size = new Size(46, 100);
            panelA2.TabIndex = 20;
            // 
            // cbUA3
            // 
            cbUA3.CheckAlign = ContentAlignment.TopCenter;
            cbUA3.ImageAlign = ContentAlignment.TopLeft;
            cbUA3.Location = new Point(500, 25);
            cbUA3.Name = "cbUA3";
            cbUA3.Size = new Size(23, 57);
            cbUA3.TabIndex = 25;
            cbUA3.Text = "楼梯";
            cbUA3.TextAlign = ContentAlignment.MiddleCenter;
            cbUA3.UseVisualStyleBackColor = true;
            // 
            // panelA0
            // 
            panelA0.BorderStyle = BorderStyle.FixedSingle;
            panelA0.Location = new Point(126, 301);
            panelA0.Margin = new Padding(0);
            panelA0.Name = "panelA0";
            panelA0.Size = new Size(50, 100);
            panelA0.TabIndex = 19;
            // 
            // cbUA4
            // 
            cbUA4.AutoSize = true;
            cbUA4.Location = new Point(453, 178);
            cbUA4.Name = "cbUA4";
            cbUA4.Size = new Size(51, 21);
            cbUA4.TabIndex = 24;
            cbUA4.Text = "楼梯";
            cbUA4.UseVisualStyleBackColor = true;
            // 
            // panelA1
            // 
            panelA1.BorderStyle = BorderStyle.FixedSingle;
            panelA1.Location = new Point(126, 251);
            panelA1.Margin = new Padding(0);
            panelA1.Name = "panelA1";
            panelA1.Size = new Size(372, 50);
            panelA1.TabIndex = 21;
            // 
            // cbUA1
            // 
            cbUA1.AutoSize = true;
            cbUA1.Location = new Point(127, 178);
            cbUA1.Name = "cbUA1";
            cbUA1.Size = new Size(51, 21);
            cbUA1.TabIndex = 23;
            cbUA1.Text = "楼梯";
            cbUA1.UseVisualStyleBackColor = true;
            // 
            // cbUA2
            // 
            cbUA2.CheckAlign = ContentAlignment.TopCenter;
            cbUA2.ImageAlign = ContentAlignment.TopLeft;
            cbUA2.Location = new Point(99, 25);
            cbUA2.Name = "cbUA2";
            cbUA2.Size = new Size(23, 57);
            cbUA2.TabIndex = 22;
            cbUA2.Text = "楼梯";
            cbUA2.TextAlign = ContentAlignment.MiddleCenter;
            cbUA2.UseVisualStyleBackColor = true;
            // 
            // pL
            // 
            pL.Controls.Add(rbLDShape);
            pL.Controls.Add(rbLCShape);
            pL.Controls.Add(rbLBShape);
            pL.Controls.Add(rbLAShape);
            pL.Controls.Add(cbLD2);
            pL.Controls.Add(cbLD3);
            pL.Controls.Add(panel4);
            pL.Controls.Add(panel8);
            pL.Controls.Add(cbLD1);
            pL.Controls.Add(cbLC3);
            pL.Controls.Add(panel5);
            pL.Controls.Add(panel7);
            pL.Controls.Add(cbLC1);
            pL.Controls.Add(cbLC2);
            pL.Controls.Add(cbLB2);
            pL.Controls.Add(cbLB3);
            pL.Controls.Add(panel3);
            pL.Controls.Add(panel9);
            pL.Controls.Add(cbLA3);
            pL.Controls.Add(panel10);
            pL.Controls.Add(cbLB1);
            pL.Controls.Add(panel12);
            pL.Controls.Add(cbLA1);
            pL.Controls.Add(cbLA2);
            pL.Dock = DockStyle.Fill;
            pL.Location = new Point(0, 0);
            pL.Name = "pL";
            pL.Size = new Size(607, 443);
            pL.TabIndex = 37;
            // 
            // rbLDShape
            // 
            rbLDShape.AutoSize = true;
            rbLDShape.Location = new Point(390, 416);
            rbLDShape.Name = "rbLDShape";
            rbLDShape.Size = new Size(47, 21);
            rbLDShape.TabIndex = 54;
            rbLDShape.Text = "D型";
            rbLDShape.UseVisualStyleBackColor = true;
            // 
            // rbLCShape
            // 
            rbLCShape.AutoSize = true;
            rbLCShape.Location = new Point(122, 416);
            rbLCShape.Name = "rbLCShape";
            rbLCShape.Size = new Size(46, 21);
            rbLCShape.TabIndex = 53;
            rbLCShape.Text = "C型";
            rbLCShape.UseVisualStyleBackColor = true;
            // 
            // rbLBShape
            // 
            rbLBShape.AutoSize = true;
            rbLBShape.Location = new Point(395, 196);
            rbLBShape.Name = "rbLBShape";
            rbLBShape.Size = new Size(46, 21);
            rbLBShape.TabIndex = 52;
            rbLBShape.Text = "B型";
            rbLBShape.UseVisualStyleBackColor = true;
            // 
            // rbLAShape
            // 
            rbLAShape.AutoSize = true;
            rbLAShape.Checked = true;
            rbLAShape.Location = new Point(106, 196);
            rbLAShape.Name = "rbLAShape";
            rbLAShape.Size = new Size(46, 21);
            rbLAShape.TabIndex = 51;
            rbLAShape.TabStop = true;
            rbLAShape.Text = "A型";
            rbLAShape.UseVisualStyleBackColor = true;
            // 
            // cbLD2
            // 
            cbLD2.CheckAlign = ContentAlignment.TopCenter;
            cbLD2.ImageAlign = ContentAlignment.TopLeft;
            cbLD2.Location = new Point(489, 252);
            cbLD2.Name = "cbLD2";
            cbLD2.Size = new Size(31, 57);
            cbLD2.TabIndex = 50;
            cbLD2.Text = "楼梯";
            cbLD2.TextAlign = ContentAlignment.MiddleCenter;
            cbLD2.UseVisualStyleBackColor = true;
            // 
            // cbLD3
            // 
            cbLD3.CheckAlign = ContentAlignment.TopCenter;
            cbLD3.ImageAlign = ContentAlignment.TopLeft;
            cbLD3.Location = new Point(295, 252);
            cbLD3.Name = "cbLD3";
            cbLD3.Size = new Size(34, 57);
            cbLD3.TabIndex = 49;
            cbLD3.Text = "楼梯";
            cbLD3.TextAlign = ContentAlignment.MiddleCenter;
            cbLD3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Location = new Point(332, 253);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Size = new Size(154, 50);
            panel4.TabIndex = 47;
            // 
            // panel8
            // 
            panel8.BorderStyle = BorderStyle.FixedSingle;
            panel8.Location = new Point(440, 303);
            panel8.Margin = new Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new Size(46, 100);
            panel8.TabIndex = 46;
            // 
            // cbLD1
            // 
            cbLD1.AutoSize = true;
            cbLD1.Location = new Point(442, 406);
            cbLD1.Name = "cbLD1";
            cbLD1.Size = new Size(51, 21);
            cbLD1.TabIndex = 48;
            cbLD1.Text = "楼梯";
            cbLD1.UseVisualStyleBackColor = true;
            // 
            // cbLC3
            // 
            cbLC3.CheckAlign = ContentAlignment.TopCenter;
            cbLC3.ImageAlign = ContentAlignment.TopLeft;
            cbLC3.Location = new Point(214, 253);
            cbLC3.Name = "cbLC3";
            cbLC3.Size = new Size(29, 57);
            cbLC3.TabIndex = 45;
            cbLC3.Text = "楼梯";
            cbLC3.TextAlign = ContentAlignment.MiddleCenter;
            cbLC3.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel5.Location = new Point(53, 303);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(50, 100);
            panel5.TabIndex = 41;
            // 
            // panel7
            // 
            panel7.BorderStyle = BorderStyle.FixedSingle;
            panel7.Location = new Point(53, 253);
            panel7.Margin = new Padding(0);
            panel7.Name = "panel7";
            panel7.Size = new Size(158, 50);
            panel7.TabIndex = 42;
            // 
            // cbLC1
            // 
            cbLC1.AutoSize = true;
            cbLC1.Location = new Point(55, 406);
            cbLC1.Name = "cbLC1";
            cbLC1.Size = new Size(51, 21);
            cbLC1.TabIndex = 44;
            cbLC1.Text = "楼梯";
            cbLC1.UseVisualStyleBackColor = true;
            // 
            // cbLC2
            // 
            cbLC2.CheckAlign = ContentAlignment.TopCenter;
            cbLC2.ImageAlign = ContentAlignment.TopLeft;
            cbLC2.Location = new Point(27, 253);
            cbLC2.Name = "cbLC2";
            cbLC2.Size = new Size(23, 57);
            cbLC2.TabIndex = 43;
            cbLC2.Text = "楼梯";
            cbLC2.TextAlign = ContentAlignment.MiddleCenter;
            cbLC2.UseVisualStyleBackColor = true;
            // 
            // cbLB2
            // 
            cbLB2.CheckAlign = ContentAlignment.TopCenter;
            cbLB2.ImageAlign = ContentAlignment.TopLeft;
            cbLB2.Location = new Point(490, 16);
            cbLB2.Name = "cbLB2";
            cbLB2.Size = new Size(30, 57);
            cbLB2.TabIndex = 40;
            cbLB2.Text = "楼梯";
            cbLB2.TextAlign = ContentAlignment.MiddleCenter;
            cbLB2.UseVisualStyleBackColor = true;
            // 
            // cbLB3
            // 
            cbLB3.CheckAlign = ContentAlignment.TopCenter;
            cbLB3.ImageAlign = ContentAlignment.TopLeft;
            cbLB3.Location = new Point(295, 16);
            cbLB3.Name = "cbLB3";
            cbLB3.Size = new Size(35, 57);
            cbLB3.TabIndex = 39;
            cbLB3.Text = "楼梯";
            cbLB3.TextAlign = ContentAlignment.MiddleCenter;
            cbLB3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Location = new Point(333, 17);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(108, 50);
            panel3.TabIndex = 22;
            // 
            // panel9
            // 
            panel9.BorderStyle = BorderStyle.FixedSingle;
            panel9.Location = new Point(441, 16);
            panel9.Margin = new Padding(0);
            panel9.Name = "panel9";
            panel9.Size = new Size(46, 151);
            panel9.TabIndex = 20;
            // 
            // cbLA3
            // 
            cbLA3.CheckAlign = ContentAlignment.TopCenter;
            cbLA3.ImageAlign = ContentAlignment.TopLeft;
            cbLA3.Location = new Point(214, 16);
            cbLA3.Name = "cbLA3";
            cbLA3.Size = new Size(29, 57);
            cbLA3.TabIndex = 25;
            cbLA3.Text = "楼梯";
            cbLA3.TextAlign = ContentAlignment.MiddleCenter;
            cbLA3.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            panel10.BorderStyle = BorderStyle.FixedSingle;
            panel10.Location = new Point(53, 17);
            panel10.Margin = new Padding(0);
            panel10.Name = "panel10";
            panel10.Size = new Size(50, 149);
            panel10.TabIndex = 19;
            // 
            // cbLB1
            // 
            cbLB1.AutoSize = true;
            cbLB1.Location = new Point(443, 170);
            cbLB1.Name = "cbLB1";
            cbLB1.Size = new Size(51, 21);
            cbLB1.TabIndex = 24;
            cbLB1.Text = "楼梯";
            cbLB1.UseVisualStyleBackColor = true;
            // 
            // panel12
            // 
            panel12.BorderStyle = BorderStyle.FixedSingle;
            panel12.Location = new Point(103, 16);
            panel12.Margin = new Padding(0);
            panel12.Name = "panel12";
            panel12.Size = new Size(108, 50);
            panel12.TabIndex = 21;
            // 
            // cbLA1
            // 
            cbLA1.AutoSize = true;
            cbLA1.Location = new Point(55, 169);
            cbLA1.Name = "cbLA1";
            cbLA1.Size = new Size(51, 21);
            cbLA1.TabIndex = 23;
            cbLA1.Text = "楼梯";
            cbLA1.UseVisualStyleBackColor = true;
            // 
            // cbLA2
            // 
            cbLA2.CheckAlign = ContentAlignment.TopCenter;
            cbLA2.ImageAlign = ContentAlignment.TopLeft;
            cbLA2.Location = new Point(27, 16);
            cbLA2.Name = "cbLA2";
            cbLA2.Size = new Size(23, 57);
            cbLA2.TabIndex = 22;
            cbLA2.Text = "楼梯";
            cbLA2.TextAlign = ContentAlignment.MiddleCenter;
            cbLA2.UseVisualStyleBackColor = true;
            // 
            // p1
            // 
            p1.Controls.Add(cb一A2);
            p1.Controls.Add(pl1);
            p1.Controls.Add(cb一A1);
            p1.Dock = DockStyle.Fill;
            p1.Location = new Point(0, 0);
            p1.Name = "p1";
            p1.Size = new Size(607, 443);
            p1.TabIndex = 38;
            // 
            // cb一A2
            // 
            cb一A2.CheckAlign = ContentAlignment.TopCenter;
            cb一A2.ImageAlign = ContentAlignment.TopLeft;
            cb一A2.Location = new Point(481, 23);
            cb一A2.Name = "cb一A2";
            cb一A2.Size = new Size(23, 57);
            cb一A2.TabIndex = 25;
            cb一A2.Text = "楼梯";
            cb一A2.TextAlign = ContentAlignment.MiddleCenter;
            cb一A2.UseVisualStyleBackColor = true;
            // 
            // pl1
            // 
            pl1.BorderStyle = BorderStyle.FixedSingle;
            pl1.Location = new Point(106, 23);
            pl1.Margin = new Padding(0);
            pl1.Name = "pl1";
            pl1.Size = new Size(372, 50);
            pl1.TabIndex = 21;
            // 
            // cb一A1
            // 
            cb一A1.CheckAlign = ContentAlignment.TopCenter;
            cb一A1.ImageAlign = ContentAlignment.TopLeft;
            cb一A1.Location = new Point(80, 23);
            cb一A1.Name = "cb一A1";
            cb一A1.Size = new Size(23, 57);
            cb一A1.TabIndex = 22;
            cb一A1.Text = "楼梯";
            cb一A1.TextAlign = ContentAlignment.MiddleCenter;
            cb一A1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(combLevel);
            panel2.Controls.Add(combRoomType);
            panel2.Controls.Add(rb1Shape);
            panel2.Controls.Add(lRoomType);
            panel2.Controls.Add(ArrangeScheme);
            panel2.Controls.Add(lLevel);
            panel2.Controls.Add(rbLshape);
            panel2.Controls.Add(lRoomSize);
            panel2.Controls.Add(combRoomSize);
            panel2.Controls.Add(rbUshape);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(607, 78);
            panel2.TabIndex = 11;
            // 
            // panel11
            // 
            panel11.Controls.Add(btnCancle);
            panel11.Controls.Add(btnSave);
            panel11.Dock = DockStyle.Bottom;
            panel11.Location = new Point(0, 521);
            panel11.Name = "panel11";
            panel11.Size = new Size(607, 50);
            panel11.TabIndex = 12;
            // 
            // btnCancle
            // 
            btnCancle.Location = new Point(510, 13);
            btnCancle.Name = "btnCancle";
            btnCancle.Size = new Size(85, 25);
            btnCancle.TabIndex = 4;
            btnCancle.Text = "取消";
            btnCancle.UseVisualStyleBackColor = true;
            btnCancle.Click += btnCancle_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(419, 13);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(85, 25);
            btnSave.TabIndex = 3;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // combLevel
            // 
            combLevel.Location = new Point(380, 12);
            combLevel.Name = "combLevel";
            combLevel.Size = new Size(100, 23);
            combLevel.TabIndex = 9;
            // 
            // AppearanceEditWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(607, 571);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(panel11);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "AppearanceEditWindow";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "编辑外形";
            Load += AppearanceEditWindow_Load;
            panel1.ResumeLayout(false);
            pU.ResumeLayout(false);
            pU.PerformLayout();
            pL.ResumeLayout(false);
            pL.PerformLayout();
            p1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel11.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lRoomType;
        private ComboBox combRoomType;
        private Label lRoomSize;
        private ComboBox combRoomSize;
        private Label ArrangeScheme;
        private RadioButton rb1Shape;
        private RadioButton rbLshape;
        private RadioButton rbUshape;
        private Label lLevel;
        private Panel panel1;
        private Panel panel2;
        private Panel pU;
        private CheckBox cbStair5A;
        private CheckBox cbUA3;
        private CheckBox cbUA4;
        private CheckBox cbUA1;
        private CheckBox cbUA2;
        private Panel panelA2;
        private Panel panelA1;
        private Panel panelA0;
        private CheckBox cbStair3B;
        private CheckBox cbStair2B;
        private Panel panelB2;
        private Panel panelB0;
        private CheckBox cbStair4B;
        private Panel panelB1;
        private CheckBox cbUB1;
        private Panel panel11;
        private Button btnCancle;
        private Button btnSave;
        private Panel pL;
        private Panel panel9;
        private CheckBox cbLA3;
        private Panel panel10;
        private CheckBox cbLB1;
        private Panel panel12;
        private CheckBox cbLA1;
        private CheckBox cbLA2;
        private Panel p1;
        private CheckBox cb一A2;
        private Panel pl1;
        private CheckBox cb一A1;
        private Panel panel3;
        private CheckBox cbLB3;
        private CheckBox cbLB2;
        private CheckBox cbLD2;
        private CheckBox cbLD3;
        private Panel panel4;
        private Panel panel8;
        private CheckBox cbLD1;
        private CheckBox cbLC3;
        private Panel panel5;
        private Panel panel7;
        private CheckBox cbLC1;
        private CheckBox cbLC2;
        private RadioButton rbBShape;
        private RadioButton rbAShape;
        private RadioButton rbLBShape;
        private RadioButton rbLAShape;
        private RadioButton rbLDShape;
        private RadioButton rbLCShape;
        private CheckBox cbUB3;
        private CheckBox cbUB2;
        private CheckBox cbUB4;
        private TextBox combLevel;
    }
}