using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LightCAD.Drawing.ElementAction;

namespace QdLayout
{
    public partial class AppearanceEditWindow : Form, IAppearanceEdit
    {
        private PlateBuildGroup plateBuildGroup;
        public List<PlateBuildingMath> PlateBuildingMaths;//每一个PlateBuildingGroup中的多个PlateBuilding
        private PlateArrangeGroup plateArrangeGroup;
        public bool IsActive { get; set; }

        public Type WinType => this.GetType();

        public AppearanceEditWindow()
        {
            InitializeComponent();
        }
        public AppearanceEditWindow(PlateBuildGroup plateBuildGroup) : this()
        {
            this.plateBuildGroup = plateBuildGroup;
        }
        //public event Action<PlateBuildingGroup, List<PlateBuildingMath>> Save;
        public event Action<PlateArrangeGroup, List<PlateBuildingMath>> Save;
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //更新组
            ResetPlateBuildingGroup();
            Save?.Invoke(plateArrangeGroup, PlateBuildingMaths);
            this.Close();
        }
        void ResetPlateBuildingGroup()
        {
            plateArrangeGroup = new PlateArrangeGroup();
            PlateBuildingMaths = new List<PlateBuildingMath>();
            var breforeType = this.plateBuildGroup.GroupType;
            var breforeDetailType = this.plateBuildGroup.DeatilPlateBuildType;
            int.TryParse(this.combLevel.Text, out int floorNum);
            plateArrangeGroup.num = this.plateBuildGroup.RoomCount;//跟代码确认
            plateArrangeGroup.FloorCount = floorNum;
            //排布方式
            if (this.rb1Shape.Checked)
            {
                //this.plateBuildGroup.GroupType = PlateBuildingGroupType.一;
                //this.plateBuildGroup.DeatilPlateBuildType = DeatilPlateBuildingGroupType.A;
                this.plateArrangeGroup.GroupType = PlateBuildGroupType.一;
                this.plateArrangeGroup.DeatilGroupType = DeatilPlateBuildGroupType.A;

                PlateBuildingMath buildingMath = new PlateBuildingMath();
                buildingMath.FloorCount = floorNum;
                buildingMath.StartStair = cb一A1.Checked;
                buildingMath.EndStair = cb一A2.Checked;
                PlateBuildingMaths.Add(buildingMath);

            }
            else if (this.rbLshape.Checked)
            {
                //this.plateBuildGroup.GroupType = PlateBuildingGroupType.L;
                //if (rbLAShape.Checked)
                //    this.plateBuildGroup.DeatilPlateBuildType = DeatilPlateBuildingGroupType.A;
                //if (rbLBShape.Checked)
                //    this.plateBuildGroup.DeatilPlateBuildType = DeatilPlateBuildingGroupType.B;
                //if (rbLCShape.Checked)
                //    this.plateBuildGroup.DeatilPlateBuildType = DeatilPlateBuildingGroupType.C;
                //if (rbLDShape.Checked)
                //    this.plateBuildGroup.DeatilPlateBuildType = DeatilPlateBuildingGroupType.D;
                this.plateArrangeGroup.GroupType = PlateBuildGroupType.L;
                if (rbLAShape.Checked)
                    this.plateArrangeGroup.DeatilGroupType = DeatilPlateBuildGroupType.A;
                if (rbLBShape.Checked)
                    this.plateArrangeGroup.DeatilGroupType = DeatilPlateBuildGroupType.B;
                if (rbLCShape.Checked)
                    this.plateArrangeGroup.DeatilGroupType = DeatilPlateBuildGroupType.C;
                if (rbLDShape.Checked)
                    this.plateArrangeGroup.DeatilGroupType = DeatilPlateBuildGroupType.D;


                PlateBuildingMath buildingMath = new PlateBuildingMath();
                buildingMath.FloorCount = floorNum;
                PlateBuildingMaths.Add(buildingMath.Clone());
                PlateBuildingMaths.Add(buildingMath.Clone());
                var ints = GetStairIndex(this.pL);
                InitStair(ints);
            }
            else if (this.rbUshape.Checked)
            {
                //this.plateBuildGroup.GroupType = PlateBuildingGroupType.U;
                //this.plateBuildGroup.DeatilPlateBuildType = rbAShape.Checked ? DeatilPlateBuildingGroupType.A : DeatilPlateBuildingGroupType.B;

                this.plateArrangeGroup.GroupType = PlateBuildGroupType.U;
                this.plateArrangeGroup.DeatilGroupType = rbAShape.Checked ? DeatilPlateBuildGroupType.A : DeatilPlateBuildGroupType.B;

                PlateBuildingMath buildingMath = new PlateBuildingMath();
                buildingMath.FloorCount = floorNum;
                PlateBuildingMaths.Add(buildingMath.Clone());
                PlateBuildingMaths.Add(buildingMath.Clone());
                PlateBuildingMaths.Add(buildingMath.Clone());
                var ints = GetStairIndex(this.pU);
                InitStair(ints);
            }
            //房间类型
            var text = combRoomType.Text.ToString();
            if (text.Contains("K式"))
            {
                this.plateArrangeGroup.DetailRoomType = DetailRoomType.KTypeRoom;
            }
            else if (text.Contains("箱式"))
            {
                this.plateArrangeGroup.DetailRoomType = DetailRoomType.BoxTypeRoom;
            }
            //房间规格
            var text1 = this.combRoomSize.Text.ToString()?.Split("*");
            if (text1.Length == 2)
            {
                this.plateArrangeGroup.RoomSizeWidth = Convert.ToDouble(text1[0]);
                this.plateArrangeGroup.RoomSizeLength = Convert.ToDouble(text1[1]);
            }


            void InitStair(List<int> ints)
            {
                for (int i = 0; i < PlateBuildingMaths.Count; i++)
                {
                    var pl = PlateBuildingMaths[i];
                    pl.StartStair = ints.Contains(i + 1);
                    pl.EndStair = ints.Contains(i + 2);
                }
            }

        }
        List<int> GetStairIndex(Panel panel)
        {
            List<int> ints = new List<int>();
            var text = this.plateArrangeGroup.DeatilGroupType.ToString();
            var type = this.plateArrangeGroup.GroupType.ToString();//
            var combineText = "cb" + type + text;
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                var name = panel.Controls[i].Name;
                if (name.StartsWith(combineText) && panel.Controls[i] is CheckBox checkBox && checkBox.Checked)
                {
                    var num = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
                    int.TryParse(num, out int result);
                    ints.Add(result);
                }
            }
            return ints;
        }
        private void btnCancle_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void AppearanceEditWindow_Load(object sender, System.EventArgs e)
        {
            if (this.plateBuildGroup != null)
            {
                CheckPlateBuildingGroupType();
                RoomSize();
                ResetDetailGroupType();
                ResetDetailRoomType();
                ResetStairs();
                RbShapeChecked();
                ResetFloorNum();
            }
        }
        void CheckPlateBuildingGroupType()
        {
            switch (this.plateBuildGroup.GroupType)
            {
                case PlateBuildGroupType.一:
                    this.rb1Shape.Checked = true; break;
                case PlateBuildGroupType.L:
                    this.rbLshape.Checked = true; break;
                case PlateBuildGroupType.U:
                    this.rbUshape.Checked = true; break;
            }
        }
        void RoomSize()
        {
            this.combRoomSize.Text = $"{this.plateBuildGroup.RoomSizeWidth}*{this.plateBuildGroup.RoomSizeLength}";
        }
        void ResetDetailGroupType()
        {
            if (plateBuildGroup.GroupType == PlateBuildGroupType.L)
            {
                switch (plateBuildGroup.DeatilPlateBuildType)
                {
                    case DeatilPlateBuildGroupType.A:
                        this.rbLAShape.Checked = true;
                        break;
                    case DeatilPlateBuildGroupType.B:
                        this.rbLBShape.Checked = true;
                        break;
                    case DeatilPlateBuildGroupType.C:
                        this.rbLCShape.Checked = true;
                        break;
                    case DeatilPlateBuildGroupType.D:
                        this.rbLDShape.Checked = true;
                        break;
                }
            }
            else if (plateBuildGroup.GroupType == PlateBuildGroupType.U)
            {
                switch (plateBuildGroup.DeatilPlateBuildType)
                {
                    case DeatilPlateBuildGroupType.A:
                        this.rbAShape.Checked = true;
                        break;
                    case DeatilPlateBuildGroupType.B:
                        this.rbBShape.Checked = true;
                        break;
                }
            }
        }
        void ResetDetailRoomType()
        {
            if (this.plateBuildGroup.DetailRoomType == DetailRoomType.KTypeRoom)
            {
                this.combRoomType.SelectedItem = "K式房";
            }
            else if (this.plateBuildGroup.DetailRoomType == DetailRoomType.BoxTypeRoom)
            {
                this.combRoomType.SelectedItem = "箱式房";
            }
        }
        void ResetStairs()
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < this.plateBuildGroup.Elements.Count; i++)
            {
                var pl = this.plateBuildGroup.Elements[i] as PlateBuilding;
                if (pl != null)
                {
                    if (pl.StartStair)
                        ints.Add(i + 1);
                    if (pl.EndStair)
                        ints.Add(i + 2);
                }
            }

            var text = this.plateBuildGroup.DeatilPlateBuildType.ToString();
            var type = this.plateBuildGroup.GroupType.ToString();
            var combineText = "cb" + type + text;
            Panel panel = new Panel();
            if (this.plateBuildGroup.GroupType == PlateBuildGroupType.一)
                panel = this.p1;
            else if (this.plateBuildGroup.GroupType == PlateBuildGroupType.U)
                panel = this.pU;
            else if (this.plateBuildGroup.GroupType == PlateBuildGroupType.L)
                panel = this.pL;
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                var name = panel.Controls[i].Name;
                if (name.StartsWith(combineText) && panel.Controls[i] is CheckBox checkBox )
                {
                    var num = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
                    int.TryParse(num, out int result);
                    if (ints.Contains(result))
                    {
                        checkBox.Checked = true;
                    }
                }
            }

        }
        void ResetFloorNum()
        {
            for (int i = 0; i < this.plateBuildGroup.Elements.Count; i++)
            {
                var ele = this.plateBuildGroup.Elements[i] as PlateBuilding;
                this.combLevel.Text = ele.BuildingFloor.ToString() ?? "";
                break;
            }
        }
        private void rb1Shape_CheckedChanged(object sender, System.EventArgs e)
        {
            RbShapeChecked();
        }

        private void rbLshape_CheckedChanged(object sender, System.EventArgs e)
        {
            RbShapeChecked();

        }

        private void rbUshape_CheckedChanged(object sender, System.EventArgs e)
        {
            RbShapeChecked();
        }

        void RbShapeChecked()
        {
            if (this.rb1Shape.Checked)
                this.p1.BringToFront();
            else if (this.rbLshape.Checked)
                this.pL.BringToFront();
            else if (this.rbUshape.Checked)
                this.pU.BringToFront();
        }

        private void combRoomType_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
    }
}
