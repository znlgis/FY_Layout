using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace QdLayout
{
    public partial class RoomConfigControl : UserControl
    {
        public RoomConfig RoomConfig;
        public Image Image
        {
            get
            {
                return this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        public bool Checked
        {
            get
            {
                return this.checkBox1.Checked;
            }
            set
            {
                this.checkBox1.Checked = value;
            }
        }

        public string RoomConfigName
        {
            get
            {
                return this.lbRoomConfigName.Text;
            }
            set
            {
                this.lbRoomConfigName.Text = value;
            }
        }

        public Action<RoomConfig> SelectedAction;
        public RoomConfigControl()
        {
            InitializeComponent();
        }

        private void RoomConfigControl_MouseClick(object sender, MouseEventArgs e)
        {
            this.Checked = !this.Checked;

            var parentControl = this.Parent;
            foreach (var chileCtrl in this.Parent.Controls)
            {
                if (chileCtrl is RoomConfigControl roomConfigCtrl)
                {
                    if (roomConfigCtrl != this)
                    {
                        roomConfigCtrl.Checked = false;
                    }
                }
            }
            if (this.Checked)
            {
                SelectedAction?.Invoke(this.RoomConfig);
            }
            else
            {
                SelectedAction?.Invoke(null); //当前配置取消选中, 清除当前房间的配置
            }
        }
    }
}
