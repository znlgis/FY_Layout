
using LightCAD.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public interface ITemComAttriWindow : IWindow
    {
        public event Action<string> Save;
    }
    public class TemComAttriRuntime()
    {
        public ITemComAttriWindow Control { get; set; }
        private ArrangementRuntime arrangeRuntime;
        private readonly DocumentRuntime docRt;

        public TemComAttriRuntime(DocumentRuntime docRt): this()
        {
           this.docRt = docRt;
        }
        public bool ShowWindow(DataTable dataTable)
        {
            if (Control == null)
            {
                Control = new TemComAttriWindow(dataTable);
                Control.Save += Control_Save;
                AppRuntime.UISystem.ShowWindow(Control, AppRuntime.App.ActiveWindow);
                return true;
            }
            return false;
        }

        private void Control_Save(string type)
        {
            var ty = ArrangementArea.WorkArea;
            //弹出方案生成界面
            if (type.Contains("工人"))
            {
                ty = ArrangementArea.WorkerArea;
            }
            ArrangementRuntime arrangeRuntime = new ArrangementRuntime();
            DataTable dt = new DataTable();
            //if (BuildiType == ArrangementArea.WorkArea)
            //{
           
           string id =     SQLiteUtility.GetIdByPartitionName(type);
            dt = SQLiteUtility.GetBuildingRoom(id);
           arrangeRuntime.ShowWindow(ty, dt);
        }
    }
}
