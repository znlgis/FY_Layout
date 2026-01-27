using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QdLayout.PlateHouse
{
    public partial class InputerBox : Form
    {
        public InputerBox()
        {
            InitializeComponent();
        }
        public static string inputerRestul="";
 
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                inputerRestul= this.textBox1.Text;
                this.Close();
            }
        }
    }
}
