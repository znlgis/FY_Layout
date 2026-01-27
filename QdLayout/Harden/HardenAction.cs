using LightCAD.Drawing.Actions.Action;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static System.Windows.Forms.DataFormats;

namespace QdLayout
{
    public class HardenAction : ComponentInstance2dAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public HardenAction() { }

        public HardenAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Harden");
        }
        static HardenAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateHarden",
                Description = "创建路面硬化",
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
            commandCtrl.WriteInfo("绘制路面硬化轮廓中...");
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
            CreateHarden(OutLoop.Curve.Clone() as Polyline2d);

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
            commandCtrl.WriteInfo("绘制路面硬化轮廓中...");
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
            CreateHarden(OutLoop.Curve.Clone() as Polyline2d);

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
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建路面硬化:");
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
                var polys= LcCurveChangeLoop.CheckLoops(lines);
                foreach (var line in polys)
                {
                    CreateHarden(line.PolyLine);
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

        public void CreateHarden(Polyline2d polyline)
        {
            var doc = docRt.Document;
            var roadDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "路面硬化", null) as QdHardenDef;
            var road = new QdHarden(roadDef) { ShapeName = "路面硬化" };
            road.Initilize(doc);
            var box = new Box2().ExpandByPoints(polyline.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
            var poly = polyline.Clone() as Polyline2d;
            poly.Translate(-box.Center);
            road.Position = box.Center.ToVector3();
            road.Outline = poly;
            road.ResetBoundingBox();
            road.Layer = GetLayer().Name;
            road.Bottom = 2;
            road.Thickness = 200;
            road.Material = MaterialManager.GetMaterial(MaterialManager.AsphaltUuid);
            vportRt.ActiveElementSet.InsertElement(road);
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var road = element as QdHarden;
            var pen = GetDrawPen(road);
            DrawHarden(canvas, road, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var road = element as QdHarden;
            var pen = GetDrawPen(road);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawHarden(canvas, road, matrix, pen);
        }


        public void DrawHarden(LcCanvas2d canvas, QdHarden road, Matrix3 matrix, LcPaint pen)
        {
            for (int i = 0; i < road.GetShapes()[0].Curve2ds.Count; i++)
            {
                 canvas.DrawCurve(pen, road.GetShapes()[0].Curve2ds[i], matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "路面硬化";
            textPaint.Position = road.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var road = element as QdHarden;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = road,
                Name = "Center",
                Position = road.Position.ToVector2(),
            };
            grips.Add(gripCenter);
            var points = road.GetShapes()[0].Curve2ds.Select(n => n.GetPoints(1)[0]).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var grip = new ControlGrip
                {
                    Element = road,
                    Name = $"Outline_{i}",
                    Position = points[i]
                };
                grips.Add(grip);
            }
            return grips.ToArray();
        }

        private QdHarden _road;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var road = element as QdHarden;
            _road = road;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                if (grip.Name == "Center")
                {
                    var offset = position - road.Position.ToVector2();
                    road.Translate(offset);
                }
                else if (_gripName.StartsWith("Outline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = road.Outline.Clone() as Polyline2d;
                    var offset = position - grip.Position;
                    (poly.Curve2ds[idx] as Line2d).Start.Add(offset);
                    if (idx == 0)
                    {
                        (poly.Curve2ds.Last() as Line2d).End.Add(offset);
                    }
                    else
                    {
                        (poly.Curve2ds[idx - 1] as Line2d).End.Add(offset);
                    }
                    road.Outline = poly;
                }              
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdHarden = element as QdHarden;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn && qdHarden.GetShapes()[0].Curve2ds.Count > 0)
            {
                foreach (var curve in qdHarden.GetShapes()[0].Curve2ds)
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
            if (_road == null)
                return;

        
        }
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
             new PropertyObserver()
            {
                Name = "Bottom",
                DisplayName = "底高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdHarden).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var road = (ele as QdHarden);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    road.OnPropertyChangedBefore("Bottom",road.Properties.GetValue<double>("Bottom"),bottom);
                    road.Properties.SetValue("Bottom",bottom );
                    road.ResetCache();
                    road.OnPropertyChangedAfter("Bottom",road.Properties.GetValue<double>("Bottom"),bottom);
                }
            },
             new PropertyObserver()
            {
                Name = "Thickness",
                DisplayName = "厚度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdHarden).Properties.GetValue<double>("Thickness"),
                Setter = (ele, value) =>
                {
                    var road = (ele as QdHarden);
                    if (!double.TryParse(value.ToString(),out var thickness)&&thickness<=0)
                        return;
                    road.OnPropertyChangedBefore("Thickness",road.Properties.GetValue<double>("Thickness"),thickness);
                    road.Properties.SetValue("Thickness",thickness );
                    road.ResetCache();
                    road.OnPropertyChangedAfter("Thickness",road.Properties.GetValue<double>("Thickness"),thickness);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Harden");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Harden";
                layer.Color = 0xFFFF00;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
