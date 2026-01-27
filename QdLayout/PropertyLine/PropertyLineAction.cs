using LightCAD.Drawing;
using LightCAD.Drawing.Actions.Action;
using netDxf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;

namespace QdLayout
{
    public class PropertyLineAction : PolyLineAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        internal static void Initilize()
        {
            ElementActions.PolyLine = new PropertyLineAction();
            LcDocument.ElementActions.Add(BuiltinElementType.PloyLine, ElementActions.PolyLine);

        }
        public PropertyLineAction()
        {

        }
        public PropertyLineAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：PropertyLine");
        }
        static PropertyLineAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreatePropertyLine",
                Description = "创建用地红线",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定轮廓第一个点:" },
                    new LcCreateStep { Name = "Step1", Options = "指定轮廓下一点或 [结束(E)]" },
                }
            };
        } 
        public async void ExecCreatePoly(string[] args = null)
        {
            commandCtrl.WriteInfo("绘制用地红线轮廓中...");
            var curMethod = CreateMethods[0]; 
            CurrentPoly = null;
            PolyLine = 1;
            await this.StartCreating();
            await CreatePoly();
            if (CurrentPoly == null || CurrentPoly.Curve == null|| CurrentPoly.Curve2ds.Count==0)
            {
                return;
            }
            if (!CurrentPoly.IsClosed)
            {
                var sp = CurrentPoly.Curve2ds.Last().GetPoints(1)[1];
                var ep = CurrentPoly.Curve2ds.First().GetPoints(1)[0];
                CurrentPoly.Curve2ds.Add(new Line2d(sp, ep));
            }
            CreatePropertyLine(); 
            this.EndCreating();
        }
        public async void ExecCreateRec(string[] args = null)
        { 
            commandCtrl.WriteInfo("绘制用地红线轮廓中...");
            await this.StartCreating();
            await CreateRectPoly();
            if (CurrentPoly == null || CurrentPoly.Curve == null || CurrentPoly.Curve2ds.Count == 0)
            {
                return;
            }
            CreatePropertyLine();
            this.EndCreating();
        }
        public async void ExecCreate(string[] args = null)
        {
            var elementInputerInGroup = new ElementSetInputer(this.docEditor);
        Step0:
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建用地红线:");
            if (elementInputerInGroup.isCancelled)
            {
                this.Cancel();
                goto End;
            }
            if (result0 == null)
            {
                this.Cancel();
                goto End;
            }
            if (result0.ValueX != null)
            {
                var eles = result0.ValueX as List<LcElement>;
                var lines = new List<LcCurve2d>();
                foreach (var ele in eles)
                {
                    if (ele is LcLine line)
                    {
                        lines.Add(line);
                    }
                    else if (ele is LcPolyLine polyLine)
                    {
                        lines.Add(polyLine);
                    }
                    else if (ele is LcArc arc)
                    {
                        lines.Add(arc);
                    }
                }
                var polys = LcCurveChangeLoop.CheckLoops(lines);
                foreach (var line in polys)
                {
                    CurrentPoly = line;
                    CreatePropertyLine();
                }
            }
            else if (result0.Option != null)
            {
                goto End;
            }
            else
            {
                goto Step0;
            }
        End:
            elementInputerInGroup = null;
        }
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }


        public void CreatePropertyLine()
        {
            var doc = docRt.Document;
            var propertyLine = new QdPropertyLine();
            propertyLine.Initilize(doc);
            propertyLine.Curve = CurrentPoly.Curve.Clone() as Polyline2d;
            propertyLine.Curve2ds = propertyLine.PolyLine.Curve2ds.ToLcListCurve();
            propertyLine.ResetBoundingBox();
            propertyLine.Layer = GetLayer().Name; 
            vportRt.ActiveElementSet.InsertElement(propertyLine);
            vportRt.ActiveElementSet.RemoveElement(CurrentPoly);
            docRt.Action.ClearSelects();
        }

        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var propertyLine = element as QdPropertyLine;
            var pen = GetDrawPen(propertyLine);
            DrawPropertyLine(canvas, propertyLine, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var propertyLine = element as QdPropertyLine;
            var pen = GetDrawPen(propertyLine);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawPropertyLine(canvas, propertyLine, matrix, pen);
        }


        public void DrawPropertyLine(LcCanvas2d canvas, QdPropertyLine propertyLine, Matrix3 matrix, LcPaint pen)
        {
            canvas.DrawCurve(pen, propertyLine.Curve, matrix);
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            //string text = "用地红线";
            //textPaint.Position = propertyLine.BoundingBox.Center;
            //canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }
 
        private QdPropertyLine _propertyLine;
        private string _gripName;
        private Vector2 _position;

        //public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        //{
        //    base.SetDragGrip(element,grip,position,isEnd);
        //}
        //public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn)
        //{
        //    //var qdPropertyLine = element as QdPropertyLine;
        //    //var sscur = SnapSettings.Current;
        //    //var result = new SnapPointResult { Element = element };
        //    //if (sscur.ObjectOn && (qdPropertyLine.Curve as Polyline2d).Curve2ds.Count > 0)
        //    //{
        //    //    foreach (var curve in (qdPropertyLine.Curve as Polyline2d).Curve2ds)
        //    //    {
        //    //        foreach (var firstP in curve.GetPoints())
        //    //        {
        //    //            if (GeoUtil.Vec2EQ(firstP, point, maxDistance))
        //    //            {
        //    //                result.Point = firstP;
        //    //                result.Name = "Start";
        //    //                result.Curves.Add(new SnapRefCurve(SnapPointType.Endpoint, curve.Clone()));
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //if (result.Point != null)
        //    //    return result;
        //    //else
        //    //    return null;
        //}
        //public override void DrawDragGrip(LcCanvas2d canvas)
        //{
        //    if (_propertyLine == null)
        //        return;


        //}
        
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_PropertyLine");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_PropertyLine";
                layer.Color = 0xA81C07;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }

    }
}
