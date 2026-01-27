using LightCAD.Drawing.Actions.Action;
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
    public class IntersectionAction : ComponentInstance2dAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public IntersectionAction() { }

        public IntersectionAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Intersection");
        }
        static IntersectionAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateIntersection",
                Description = "创建路口",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定轮廓第一个点:" },
                    new LcCreateStep { Name = "Step1", Options = "指定轮廓下一点或 [结束(E)]" }, 
                }
            };
        }
 
        public async void ExecCreatePoly(string[] args = null)
        {
             
            OutLoop = null; 
            commandCtrl.WriteInfo("绘制路口轮廓中...");
            var curMethod = CreateMethods[0];
            var doc = docRt.Document;
            cmdTextInputer = new CmdTextInputer(docEditor);
        Step1: 
            var plAc = new PolyLineAction(docEditor);
            await plAc.StartCreating();
            if (!await plAc.OtherActionCreating())
            {
                goto End;
            }
            else
            {
                OutLoop = plAc.CurrentPoly;
            }
            CreateIntersection();

        End:
            if (OutLoop != null)
            {
                vportRt.ActiveElementSet.RemoveElement(OutLoop);
            }
            pointInputer = null;
            cmdTextInputer = null;
            plAc.EndCreating();
            EndCreating();
        }
        public async void ExecCreateRec(string[] args = null)
        {

            OutLoop = null;
            commandCtrl.WriteInfo("绘制路口轮廓中...");
            var curMethod = CreateMethods[0];
            var doc = docRt.Document;
        Step1:
            var plAc = new PolyLineAction(docEditor);
            await plAc.StartCreating();
            if (!await plAc.OtherActionCreatingRect())
            {
                goto End;
            }
            else
            {
                OutLoop = plAc.CurrentPoly;
            }
            CreateIntersection();

        End:
            if (OutLoop != null)
            {
                vportRt.ActiveElementSet.RemoveElement(OutLoop);
            }
            pointInputer = null;
            cmdTextInputer = null;
            plAc.EndCreating();
            EndCreating();
        }
        public async void ExecCreate(string[] args = null)
        {
            var elementInputerInGroup = new ElementSetInputer(this.docEditor);
        Step0:
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建路口:");
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
                    OutLoop = line;
                    CreateIntersection();
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
 

        public void CreateIntersection()
        {
            var doc = docRt.Document;
            var siteDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "路口", null) as QdIntersectionDef;
            var site = new QdIntersection(siteDef);
            site.Initilize(doc);
            site.Outline = OutLoop.Curve.Clone() as Polyline2d;
            site.ResetBoundingBox();
            site.Layer = GetLayer().Name;
            site.Bottom = 0;
            site.DragPoint = site.BoundingBox.Center;
            site.Material = MaterialManager.GetMaterial(MaterialManager.AsphaltUuid);
            vportRt.ActiveElementSet.InsertElement(site);
            site.CheckHasFoundationPit();
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var site = element as QdIntersection;
            var pen = GetDrawPen(site);
            DrawIntersection(canvas, site, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var site = element as QdIntersection;
            var pen = GetDrawPen(site);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawIntersection(canvas, site, matrix, pen);
        }


        public void DrawIntersection(LcCanvas2d canvas, QdIntersection site, Matrix3 matrix, LcPaint pen)
        {
            for (int i = 0; i < site.Outline.Curve2ds.Count; i++)
            {
                 canvas.DrawCurve(pen, site.Outline.Curve2ds[i], matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "路口";
            textPaint.Position = site.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var site = element as QdIntersection;
            var grips = new List<ControlGrip>();

            //var gripCenter = new ControlGrip
            //{
            //    Element = site,
            //    Name = "Center",
            //    Position = site.DragPoint
            //};
            //grips.Add(gripCenter);
            //var points = site.Outline.Curve2ds.Select(n => n.GetPoints(1)[0]).ToList();
            //for (int i = 0; i < points.Count; i++)
            //{
            //    var grip = new ControlGrip
            //    {
            //        Element = site,
            //        Name = $"Outline_{i}",
            //        Position = points[i]
            //    };
            //    grips.Add(grip);
            //}
            return grips.ToArray();
        }

        private QdIntersection _site;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var site = element as QdIntersection;
            _site = site;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                //if (grip.Name == "Center")
                //{
                //    site.OnPropertyChangedBefore(nameof(site.DragPoint), site.DragPoint, position);
                //    var offset = position - site.DragPoint;
                //    site.Translate(offset);
                //    site.ResetCache();
                //    site.OnPropertyChangedAfter(nameof(site.DragPoint), site.DragPoint, position);
                //}
                //else if (_gripName.StartsWith("Outline"))
                //{
                //    var idx = int.Parse(_gripName.Split('_')[1]);
                //    site.OnPropertyChangedBefore(nameof(site.Outline), site.Outline, site.Outline);
                //    (site.Outline.Curve2ds[idx] as Line2d).Start = position.Clone();
                //    if (idx == 0)
                //    {
                //        (site.Outline.Curve2ds.Last() as Line2d).End = position.Clone();
                //    }
                //    else
                //    {
                //        (site.Outline.Curve2ds[idx - 1] as Line2d).End = position.Clone();
                //    }
                //    site.ResetCache();
                //    site.OnPropertyChangedAfter(nameof(site.Outline), site.Outline, site.Outline);
                //}
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdIntersection = element as QdIntersection;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn && qdIntersection.Outline.Curve2ds.Count > 0)
            {
                foreach (var curve in qdIntersection.Outline.Curve2ds)
                {
                    foreach (var firstP in curve.GetPoints())
                    {
                        if (GeoUtil.Vec2EQ(firstP, point, maxDistance))
                        {
                            result.Point = firstP;
                            result.Name = "Start";
                            result.Curves.Add(new SnapRefCurve(SnapPointType.Endpoint, curve.Clone()));
                        }
                    }
                }            
            }
            if (result.Point != null)
                return result;
            else
                return null;
        }
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_site == null)
                return;

        
        }
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
            // new PropertyObserver()
            //{
            //    Name = "Bottom",
            //    DisplayName = "底高",
            //    CategoryName = "Geometry",
            //    CategoryDisplayName = "几何图形",
            //    PropType=PropertyType.Double,
            //    Getter = (ele) => (ele as QdIntersection).Properties.GetValue<double>("Bottom"),
            //    Setter = (ele, value) =>
            //    {
            //        var site = (ele as QdIntersection);
            //        if (!double.TryParse(value.ToString(),out var bottom))
            //            return;
            //        site.OnPropertyChangedBefore("Bottom",site.Properties.GetValue<double>("Bottom"),bottom);
            //        site.Properties.SetValue("Bottom",bottom );
            //        site.ResetCache();
            //        site.OnPropertyChangedAfter("Bottom",site.Properties.GetValue<double>("Bottom"),bottom);
            //    }
            //}
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Intersection");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Intersection";
                layer.Color = 0xFF0000;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
