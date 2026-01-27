using LightCAD.Drawing.Actions.Action;
using netDxf.Collections;
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
    public class OpenLineAction : LineAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        internal static void Initilize()
        {
            ElementActions.Line = new OpenLineAction();
            LcDocument.ElementActions.Add(BuiltinElementType.Line, ElementActions.Line);

        }
        public OpenLineAction()
        {

        }

        public OpenLineAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：OpenLine");
        }
        static OpenLineAction()
        {
            //CreateMethods = new LcCreateMethod[1];
            //CreateMethods[0] = new LcCreateMethod()
            //{
            //    Name = "CreateOpenLine",
            //    Description = "创建用地红线",
            //    Steps = new LcCreateStep[]
            //    {
            //        new LcCreateStep { Name = "Step0", Options = "指定轮廓第一个点:" },
            //        new LcCreateStep { Name = "Step1", Options = "指定轮廓下一点或 [结束(E)]" },
            //    }
            //};
        }
        public async void ExecCreateOpenLine(string[] args = null)
        {
            await this.StartCreating();
            var elementInputerOnce = new ElementInputerOnce(this.docEditor);
            var result0 = await elementInputerOnce.Execute("请选择用地红线的开门边线:");
            if (elementInputerOnce.isCancelled)
            {
                this.Cancel();
                goto End;
            }
            if (result0 == null || result0.ValueX is not QdPropertyLine)
            {
                this.Cancel();
                goto End;
            }
            var inputP = elementInputerOnce.InputP?.Clone();
            var pL = result0.ValueX as QdPropertyLine;
            var res = pL.Curve2ds.Select(n =>
            {
                if (n is Arc2d arc)
                {
                    return new Tuple<Curve2d, double>(arc, arc.GetPoints().Min(m => m.DistanceToSquared(inputP)));
                }
                else if (n is Line2d pl)
                {
                    return new Tuple<Curve2d, double>(pl, pl.PointProjection(inputP).DistanceToSquared(inputP));
                }
                return new Tuple<Curve2d, double>(n, 0);
            }
);
            var minDisL = res.OrderBy(n => n.Item2).First().Item1;
            if (minDisL is Arc2d)
            {
                goto End;
            }
            if (pL.OpenLine!=null)
            {
                this.vportRt.ActiveElementSet.RemoveElement(pL.OpenLine);
            }
            var line = minDisL as Line2d;
            var yellowL = new QdOpenLine();
            yellowL.Initilize(docRt.Document);
            yellowL.Curve = line.Clone();
            yellowL.Layer = GetLayer().Name;
            yellowL.PropertyLine= pL;
            pL.OpenLine = yellowL;
            this.vportRt.ActiveElementSet.InsertElement(yellowL);

        End:
            elementInputerOnce = null;
            this.EndCreating();

        }

        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }




        //public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        //{
        //    var propertyLine = element as QdOpenLine;
        //    var pen = GetDrawPen(propertyLine);
        //    DrawOpenLine(canvas, propertyLine, matrix, pen);
        //}
        //public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        //{
        //    var propertyLine = element as QdOpenLine;
        //    var pen = GetDrawPen(propertyLine);
        //    var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
        //    DrawOpenLine(canvas, propertyLine, matrix, pen);
        //}


        //public void DrawOpenLine(LcCanvas2d canvas, QdOpenLine propertyLine, Matrix3 matrix, LcPaint pen)
        //{
        //    canvas.DrawCurve(pen, propertyLine.Curve, matrix);
        //    LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
        //    textPaint.WidthFactor = 1;
        //    string text = "开门边线";
        //    textPaint.Position = propertyLine.BoundingBox.Center;
        //    canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        //}

        private QdOpenLine _propertyLine;
        private string _gripName;
        private Vector2 _position;

        //public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        //{
        //    base.SetDragGrip(element,grip,position,isEnd);
        //}
        //public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn)
        //{
        //    //var qdOpenLine = element as QdOpenLine;
        //    //var sscur = SnapSettings.Current;
        //    //var result = new SnapPointResult { Element = element };
        //    //if (sscur.ObjectOn && (qdOpenLine.Curve as Polyline2d).Curve2ds.Count > 0)
        //    //{
        //    //    foreach (var curve in (qdOpenLine.Curve as Polyline2d).Curve2ds)
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
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_OpenLine");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_OpenLine";
                layer.Color = 0xFFFF00;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }

    }
}
