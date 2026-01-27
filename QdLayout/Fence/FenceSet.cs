using LightCAD.Core;
using LightCAD.MathLib;
using LightCAD.Runtime.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QdLayout.Fence
{
    public partial class FenceSet : Form
    {
        private List<string> colorList = new List<string>() { ValueFrom.ByLayer, ValueFrom.ByBlock, "红", "黄", "绿", "青", "蓝", "洋红", "白" };
        public static string FenceWidth = "240";
        public static string FenceHeight = "2500";
        public static string FenceColumnInterval = "2500";
        public static string FenceColumnHeight = "2500";
        public static string FenceColumnColor = "0x1b7fdf";
        public static string FenceColor = "0xffffff";
        public static FenceColumnStyle fenceColumnStyle = FenceColumnStyle.Rectangle;
        public FenceSet()
        {
            InitializeComponent();
            foreach (var color in colorList)
            {
                cboFenceColor.Items.Add(color);
                cboFenceColumnColor.Items.Add(color);
            }
        //    cboFenceColor.Items.Add("选择颜色...");
            cboFenceColor.SelectedItem="白";
            cboFenceColumnColor.SelectedItem = "蓝";
            cboFenceColumnColor.Items.Add("选择颜色...");
 
            FenceColumnColor = "0x1b7fdf";


            FenceColor = "0xffffff";
        }

        private void btFence_Click(object sender, System.EventArgs e)
        {
            FenceWidth = txtFenceWeight.Text;
            FenceHeight = txtFenceHeight.Text;
            FenceColumnInterval = txtFenceColumnInterval.Text;
            FenceColumnHeight = txtFenceColumnHeight.Text;
            fenceColumnStyle = FenceColumnStyle.Rectangle;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }
        public static uint RGBFromSystem(System.Drawing.Color color)
        {
            return (uint)(color.A << 24 | (color.R << 16) | (color.G << 8) | (color.B));
        }
 
        public static Color GetColorByUint(uint uintValue)
        {
           Color colorConverted =new Color((ushort)((uintValue >> 16) & 255), (ushort)((uintValue >> 8) & 255), (ushort)(uintValue & 255));
            return colorConverted;
        }
        private void cboFenceColumnColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
 
            switch (cboFenceColumnColor.SelectedItem.ToString())
            {

                case "红":
                    FenceColumnColor = "0xf21010";
                    break;
                case "黄":
                    FenceColumnColor = "fcff00";
                    break;
                case "绿":
                    FenceColumnColor = "0x00ff00";
                    break;
                case "青":
                    FenceColumnColor = "0x1fd3de";
                    break;
                case "蓝":
                    FenceColumnColor = "0x1b7fdf";
                    break;
                case "洋红":
                    FenceColumnColor = "0xd74949";
                    break;
                case "白":
                    FenceColumnColor = "0xffffff";
                    break;
                case "选择颜色...":
                    //下拉框触发颜色选择器 并重新刷新下拉框列表值
                    //ColorDialog colorDialog = new ColorDialog();
                    //if (System.Drawing.ColorDialog.ShowDialog() == DialogResult.OK)
                    //{
                    //    FenceColumnColor = RGBFromSystem(System.Drawing.ColorDialog.Color).ToString();
                    //    e = colorDialog.Color.R + "," + colorDialog.Color.G + "," + colorDialog.Color.B;
                    //    colorList.Add(e);
                    //}
                    //InitColors(lcDocument);
                    break;
                default: break;

            }
        }

        private void cboFenceColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
 
            switch (cboFenceColor.SelectedItem.ToString())
            {
                case "红":
                    FenceColor = "0xf21010";
                    break;
                case "黄":
                    FenceColor = "fcff00";
                    break;
                case "绿":
                    FenceColor = "0x00ff00";
                    break;
                case "青":
                    FenceColor = "0x1fd3de";
                    break;
                case "蓝":
                    FenceColor = "0x1b7fdf";
                    break;
                case "洋红":
                    FenceColor = "0xd74949";
                    break;
                case "白":
                    FenceColor = "0xffffff";
                 
                    break;
                case "选择颜色...":
                    //下拉框触发颜色选择器 并重新刷新下拉框列表值
                    //ColorDialog colorDialog = new ColorDialog();
                    //if (System.Drawing.ColorDialog.ShowDialog() == DialogResult.OK)
                    //{
                    //    FenceColor = RGBFromSystem(System.Drawing.ColorDialog.Color).ToString();
                    //    e = colorDialog.Color.R + "," + colorDialog.Color.G + "," + colorDialog.Color.B;
                    //    colorList.Add(e);
                    //}
                    //InitColors(lcDocument);
                    break;
                default: break;

            }
        }
    }
}
