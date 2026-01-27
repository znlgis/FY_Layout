
using LightCAD.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public  class TemComAttriAction : ElementAction//选择红线进行排布
    {
        public TemComAttriAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：选择红线进行排布");
        }
        public async void ExecCreate(string[] args = null)
        {
            //需加一个选择红线和门口线判断
            TemComAttriRuntime temComAttriWindow = new TemComAttriRuntime(docEditor.DocRt);
            
            var dt = SQLiteUtility.GetPartitionRoom();
            temComAttriWindow.ShowWindow(dt);//读区
        }
    }
}
