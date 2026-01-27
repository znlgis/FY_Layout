using LightCAD.Core;
using LightCAD.Drawing;
using LightCAD.Drawing.Actions;
using LightCAD.MathLib;
using LightCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    internal class LayoutEquipmentAction : ComponentInstance2dAction
    {
        public LayoutEquipmentAction() { }
        public LayoutEquipmentAction(IDocumentEditor docEditor) :base(docEditor)
        { }
        static LcCreateMethod[] CreateMethods { get; set; }
        static LayoutEquipmentAction()
        {
            CreateMethods = new LcCreateMethod[]
            {
                new LcCreateMethod{
                    Name="CreateFan",
                    Description="创建场布设备",
                    Steps=new LcCreateStep[]{ new LcCreateStep {Name="Step0",Options="请点取位置"
                    } } }
            };
        }

        private PointInputer pointInputer;
        private QdLayoutEquipment tempLayoutEquipment; 

        public async void ExecCreate(string[] args = null)
        {
            this.StartCreating();
            pointInputer = new PointInputer(this.docEditor);
            Vector2 insertP = null;
            var cm = CreateMethods[0];
            string msg = "在风管上点选位置";
        Step0:
            //    tempLayoutEquipment = CreateFan();
            //    tempLayoutEquipment.Initilize(this.docRt.Document);
            //    var result = await pointInputer.Execute(msg);
            //    if (result.IsCancelled || result.ValueX == null)
            //        goto CancelX;
            //    if (result.ValueX is Vector2 p)
            //    {
            //        //tempLayoutEquipment.Position.Copy(p);
            //        //this.docRt.Document.ModelSpace.InsertElement(tempLayoutEquipment);
            //    }
            //    else
            //        commandCtrl.Prompt(msg);
            //    tempLayoutEquipment = null;
            goto Step0;
        CancelX:
        End:
            tempLayoutEquipment = null;
            EndCreating();
        }
        //private QdLayoutEquipment CreateFan()
        //{
        //    var def = docRt.GetUseComDef(LcCategoryManager.GetCategory(FanKey), "柜式离心风机", "柜式离心风机") as QdFanDef;
        //    var fan = new QdLayoutEquipment(def) { ShapeName = "柜式离心风机", SolidName = "柜式离心风机" };
        //    return fan;
        //}
        //public override void DrawTemp(LcCanvas2d canvas)
        //{
        //    base.DrawTemp(canvas);
        //    if (tempLayoutEquipment == null)
        //        return;
        //    var p = pointInputer.InputP;
        //    if (p != null)
        //    {
        //        tempLayoutEquipment.ResetCache();
        //        tempLayoutEquipment.Position.Copy(p);
        //        this.Draw(canvas, tempLayoutEquipment, new Vector2());
        //    }
        //}
    }
}
