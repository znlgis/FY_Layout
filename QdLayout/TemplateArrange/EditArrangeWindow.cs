using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QdLayout
{
    public partial class EditArrangeWindow : Form, IEditArrangeWindow
    {
        private PlateArrangeGroup arrangeGroup;

        public bool IsActive {  get; set; }

        public Type WinType => this.GetType();

        public event Action<PlateArrangeGroup> Save;
        public EditArrangeWindow()
        {
            InitializeComponent();
        }
        public EditArrangeWindow(PlateArrangeGroup arrangeGroup) : this()
        {
            this.arrangeGroup = arrangeGroup;
            InitUI();
        }
        void InitUI()
        {
            switch (this.arrangeGroup.GroupType)
            {
                case PlateBuildGroupType.一:
                    this.rb1Shape.Checked = true;
                    break;
                case PlateBuildGroupType.L:
                    this.rbLshape.Checked = true;
                    break;
                case PlateBuildGroupType.U:
                    this.rbUshape.Checked = true;
                    break;
                default:
                    this.rbUshape.Checked = true;
                    break;
            }

            int index = 0;
            switch (this.arrangeGroup.DetailRoomType)
            {
                case DetailRoomType.KTypeRoom:
                    index = this.combRoomType.Items.IndexOf("K式房");
                    break;
                case DetailRoomType.BoxTypeRoom:
                    index = this.combRoomType.Items.IndexOf("箱式房");
                    break;
                default:
                    index = 0;
                    break;
            }
            this.combRoomType.SelectedIndex = index;
            //标准间规格
            var length = this.arrangeGroup.RoomSizeLength;
            var width = this.arrangeGroup.RoomSizeWidth;
            var size = $"{length}*{width}";
            if (!this.combRoomSize.Items.Contains(size))
            {
                this.combRoomSize.Items.Add(size);
            }
            var inde = this.combRoomSize.Items.IndexOf(size);
            this.combRoomSize.SelectedIndex = inde;


            this.Text= "编辑"+"("+ this.arrangeGroup.name+")";
        }
 
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            RefreshArrangeGroup();
            Save?.Invoke(arrangeGroup);
            this.Close();
        }
       void RefreshArrangeGroup()
       {
            //排布方式
            if (this.rb1Shape.Checked)
            {
                this.arrangeGroup.GroupType = PlateBuildGroupType.一;
            }
            else if (this.rbLshape.Checked)
            {
                this.arrangeGroup.GroupType = PlateBuildGroupType.L;
            }
            else if (this.rbUshape.Checked)
            {
                this.arrangeGroup.GroupType = PlateBuildGroupType.U;
            }
            //房间类型
            var text = combRoomType.Text.ToString();
            if (text.Contains("K式"))
            {
                arrangeGroup.DetailRoomType = DetailRoomType.KTypeRoom;
            }
            else if (text.Contains("箱式"))
            {
                arrangeGroup.DetailRoomType = DetailRoomType.BoxTypeRoom;
            }
            //房间规格
            var text1 = this.combRoomSize.Text.ToString()?.Split("*");
            if (text1.Length == 2)
            {
                this.arrangeGroup.RoomSizeWidth = Convert.ToDouble(text1[1]);
                this.arrangeGroup.RoomSizeLength = Convert.ToDouble(text1[0]);
            }
        }

        private void btnCancle_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
