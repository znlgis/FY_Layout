using LightCAD.Drawing.Actions.Action;
using Svg.ExCSS;
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
    public class BermAction : ComponentInstance2dAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private Vector2 StartPoint;
        private QdBerm Berm;
        public BermAction() { }

        public BermAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Berm");
        }
        static BermAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateBerm",
                Description = "创建出土道路",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定出土道路起始点:" },
                    new LcCreateStep { Name = "Step1", Options = "指定出土道路结束点" }, 
                }
            };
        }
 
        public async void ExecCreatePoly(string[] args = null)
        {
            this.pointInputer = new PointInputer(docEditor);
            var curMethod = CreateMethods[0];
            var doc = docRt.Document;
            await this.StartCreating();
            var bermDef = docRt.GetUseComDef($"{NamespaceKey}.土方基坑", "出土道路", null) as QdBermDef;
            Berm = new QdBerm(bermDef) { };
            Berm.Initilize(doc);
            Berm.Baseline = new Line2d(new Vector2(),new Vector2());
            Berm.Layer = GetLayer().Name;
            Berm.Bottom = -4000;
            Berm.Factor = 0.4;
            Berm.ElevationStart = 0;
            Berm.ElevationEnd = -4000;
            Berm.Material = MaterialManager.GetMaterial(MaterialManager.EarthworkUuid);
        Step1:
            var plAc = new PolyLineAction(docEditor);
            var result = await this.pointInputer.Execute(curMethod.Steps[0].Options);
            if (result == null || this.pointInputer.isCancelled)
            {
                this.Cancel();
                goto End;
            }
            if (result.ValueX != null && result.ValueX is Vector2)
            {
                StartPoint = result.ValueX as Vector2;
                Berm.Baseline.Start = StartPoint;
                var result2 = await this.pointInputer.Execute(curMethod.Steps[1].Options);
                if (result2 == null || result2.ValueX == null || !(result2.ValueX is Vector2) || this.pointInputer.isCancelled)
                {
                    this.Cancel();
                    goto End;
                }
                Berm.Baseline.End = result2.ValueX as Vector2;
            }
            else
            {
                goto Step1;
            }
            CreateBerm(Berm);

        End:
            pointInputer = null;
            Berm = null;
            EndCreating();
        }
        
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }
 

        public void CreateBerm(QdBerm berm)
        {
            vportRt.ActiveElementSet.InsertElement(berm);
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var berm = element as QdBerm;
            var pen = GetDrawPen(berm);
            DrawBerm(canvas, berm, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var berm = element as QdBerm;
            var pen = GetDrawPen(berm);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawBerm(canvas, berm, matrix, pen);
        }

        public override void DrawAuxLines(LcCanvas2d canvas)
        {
            if (StartPoint!=null&& this.Berm!=null&& this.pointInputer?.InputP!=null)
            {
                var berm = Berm.Clone() as QdBerm;
                berm.Baseline = new Line2d(StartPoint.Clone(),this.pointInputer.InputP);
                DrawBerm(canvas,berm,new Matrix3(),new LcPaint() { Color=  Color.Orange, IsScrDashLine = true });
            }
        }
        public void DrawBerm(LcCanvas2d canvas, QdBerm berm, Matrix3 matrix, LcPaint pen)
        {
            foreach (var curve in berm.GetShapes().FirstOrDefault()?.Curve2ds)
            {
                canvas.DrawCurve(pen,curve,matrix);
            }
            pen.IsScrDashLine = true;
            var normal = berm.Baseline.Dir;
            var center = berm.Baseline.Mid;
            var cl = center.Clone().AddScaledVector(normal.Clone().RotateAround(new Vector2(), Math.PI / 5), -2000);
            var cr = center.Clone().AddScaledVector(normal.Clone().RotateAround(new Vector2(), -Math.PI / 5), -2000);
            canvas.DrawLine(pen, center, cl, matrix);
            canvas.DrawLine(pen, center, cr, matrix);
            canvas.DrawCurve(pen, berm.Baseline, matrix);
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "出土道路";
            textPaint.Position = berm.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var berm = element as QdBerm;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = berm,
                Name = "Center",
                Position = berm.Baseline.Mid,
            };
            var gripS= new ControlGrip
            {
                Element = berm,
                Name = "Start",
                Position = berm.Baseline.Start.Clone(),
            };
            var gripE = new ControlGrip
            {
                Element = berm,
                Name = "End",
                Position = berm.Baseline.End.Clone(),
            };
            grips.Add(gripCenter);
            grips.Add(gripS);
            grips.Add(gripE);

            return grips.ToArray();
        }

        private QdBerm _berm;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var berm = element as QdBerm;
            _berm = berm;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                if (grip.Name == "Center")
                {
                    var offset = position - berm.Baseline.Mid;
                    berm.Translate(offset);
                }
                if (grip.Name == "Start")
                {
                    var line = berm.Baseline.Clone() as Line2d;
                    line.Start = position.Clone();
                    berm.BaseCurve = line;
                }
                if (grip.Name == "End")
                {
                    var line = berm.Baseline.Clone() as Line2d;
                    line.End = position.Clone();
                    berm.BaseCurve = line;
                }
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdBerm = element as QdBerm;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            foreach (var firstP in qdBerm.Baseline.GetPoints())
            {
                if (GeoUtil.Vec2EQ(firstP, point, maxDistance))
                {
                    result.Point = firstP;
                    result.Name = "Start";
                    result.Curves.Add(new SnapRefCurve(SnapPointType.Endpoint, qdBerm.Baseline.Clone()));
                }
            }
            if (result.Point != null)
                return result;
            else
                return null;
        }
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_berm == null)
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
                Getter = (ele) => (ele as QdBerm).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var berm = (ele as QdBerm);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    berm.OnPropertyChangedBefore("Bottom",berm.Properties.GetValue<double>("Bottom"),bottom);
                    berm.Properties.SetValue("Bottom",bottom );
                    berm.ResetCache();
                    berm.OnPropertyChangedAfter("Bottom",berm.Properties.GetValue<double>("Bottom"),bottom);
                }
            }, 
            new PropertyObserver()
            {
                Name = "Width",
                DisplayName = "道路宽度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBerm).Properties.GetValue<double>("Width"),
                Setter = (ele, value) =>
                {
                    var berm = (ele as QdBerm);
                    if (!double.TryParse(value.ToString(),out var width))
                        return;
                    berm.OnPropertyChangedBefore("Width",berm.Properties.GetValue<double>("Width"),width);
                    berm.Properties.SetValue("Width",width );
                    berm.ResetCache();
                    berm.OnPropertyChangedAfter("Width",berm.Properties.GetValue<double>("Width"),width);
                }
            },
            new PropertyObserver()
            {
                Name = "Factor",
                DisplayName = "放坡系数",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBerm).Factor,
                Setter = (ele, value) =>
                {
                    var berm = (ele as QdBerm);
                    if (!double.TryParse(value.ToString(),out var factor))
                        return;
                    berm.OnPropertyChangedBefore("Factor",berm.Properties.GetValue<double>("Factor"),factor);
                    berm.Properties.SetValue("Factor",factor );
                    berm.ResetCache();
                    berm.OnPropertyChangedAfter("Factor",berm.Properties.GetValue<double>("Factor"),factor);
                }
            },
              new PropertyObserver()
            {
                Name = "ElevationStart",
                DisplayName = "起始标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBerm).ElevationStart,
                Setter = (ele, value) =>
                {
                    var berm = (ele as QdBerm);
                    if (!double.TryParse(value.ToString(),out var height))
                        return;
                    berm.OnPropertyChangedBefore("ElevationStart",berm.Properties.GetValue<double>("ElevationStart"),height);
                    berm.Properties.SetValue("ElevationStart",height );
                    berm.ResetCache();
                    berm.OnPropertyChangedAfter("ElevationStart",berm.Properties.GetValue<double>("ElevationStart"),height);
                }
            },
              new PropertyObserver()
            {
                Name = "ElevationEnd",
                DisplayName = "终点标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBerm).ElevationEnd,
                Setter = (ele, value) =>
                {
                    var berm = (ele as QdBerm);
                    if (!double.TryParse(value.ToString(),out var height))
                        return;
                    berm.OnPropertyChangedBefore("ElevationEnd",berm.Properties.GetValue<double>("ElevationEnd"),height);
                    berm.Properties.SetValue("ElevationEnd",height );
                    berm.ResetCache();
                    berm.OnPropertyChangedAfter("ElevationEnd",berm.Properties.GetValue<double>("ElevationEnd"),height);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Berm");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Berm";
                layer.Color = 0x00FF00;
                layer.SetLineType(new LcLineType("ByLayer"));
                layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
