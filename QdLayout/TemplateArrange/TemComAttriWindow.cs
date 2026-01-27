using LightCAD.DBUtility;
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
    public partial class TemComAttriWindow : Form, ITemComAttriWindow
    {

        public bool IsActive { get; set; }

        public Type WinType => this.GetType();

        public TemComAttriWindow()
        {
            InitializeComponent();
           
        }
        public TemComAttriWindow(DataTable dataTable) :this()
        {
            foreach (var dr in dataTable.Rows)
            {
                var text = (dr as DataRow).ItemArray.LastOrDefault().ToString();
                this.combArea.Items.Add(text);
            }
            this.combArea.SelectedIndex = 0;
        }

        public event Action<string> Save;

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Save?.Invoke(combArea.Text.ToString());
            this.Close();
        }

        private void btnCancle_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
