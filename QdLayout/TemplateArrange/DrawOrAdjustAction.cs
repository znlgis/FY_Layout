using LightCAD.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class DrawOrAdjustAction : ElementAction
    {
        public DrawOrAdjustAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：编辑外型");
        }
        public async void ExecCreate(string[] args = null)
        {
            var eles = this.docRt.Action.SelectedElements;
            if (eles.Count == 1  && eles.FirstOrDefault() is PlateBuildGroup plateBuildGroup)
            {
                AppearanceEditRuntime appearanceEditRuntime = new AppearanceEditRuntime();
                appearanceEditRuntime.ShowWindow(plateBuildGroup, docEditor.DocRt);
            }
        }
    }
}
