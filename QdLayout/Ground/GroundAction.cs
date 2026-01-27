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
    public class GroundAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public GroundAction() { }

        public GroundAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Ground");
        }
        static GroundAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateGround",
                Description = "创建硬化地面",
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
            commandCtrl.WriteInfo("绘制硬化地面轮廓中...");
            var curMethod = CreateMethods[0];
            var doc = docRt.Document;
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
            CreateGround();

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
            commandCtrl.WriteInfo("绘制硬化地面轮廓中...");
            var curMethod = CreateMethods[0];
            var doc = docRt.Document;
            cmdTextInputer = new CmdTextInputer(docEditor);
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
            CreateGround();

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
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建硬化地面:");
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
                    CreateGround();
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
 

        public void CreateGround()
        {
            var doc = docRt.Document;
            var groundDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "硬化地面", null) as QdGroundDef;
            var ground = new QdGround(groundDef);
            ground.Initilize(doc);
            var poly = OutLoop.Clone() as LcPolyLine;
            ground.Outline = poly.Curve.Clone() as Polyline2d;
            ground.Layer = GetLayer().Name;
            ground.Bottom = -5;
            ground.Thickness = 200;
            ground.Material = MaterialManager.GetMaterial(MaterialManager.RoadUuid);
            vportRt.ActiveElementSet.InsertElement(ground);
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var ground = element as QdGround;
            var pen = GetDrawPen(ground);
            DrawGround(canvas, ground, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var ground = element as QdGround;
            var pen = GetDrawPen(ground);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawGround(canvas, ground, matrix, pen);
        }


        public void DrawGround(LcCanvas2d canvas, QdGround ground, Matrix3 matrix, LcPaint pen)
        {
            for (int i = 0; i < ground.GetShapes()[0].Curve2ds.Count; i++)
            {
                 canvas.DrawCurve(pen, ground.GetShapes()[0].Curve2ds[i], matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "硬化地面";
            textPaint.Position = ground.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var road = element as QdGround;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = road,
                Name = "Center",
                Position = road.BoundingBox.Center.Clone(),
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

        private QdGround _ground;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var ground = element as QdGround;
            _ground = ground;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                if (grip.Name == "Center")
                {
                    var offset = position - grip.Position;
                    ground.Translate(offset);
                }
                else if (_gripName.StartsWith("Outline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = ground.Outline.Clone() as Polyline2d;
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
                    ground.Outline = poly;
                }              
            }
        }
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_ground == null)
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
                Getter = (ele) => (ele as QdGround).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var ground = (ele as QdGround);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    ground.OnPropertyChangedBefore("Bottom",ground.Properties.GetValue<double>("Bottom"),bottom);
                    ground.Properties.SetValue("Bottom",bottom );
                    ground.ResetCache();
                    ground.OnPropertyChangedAfter("Bottom",ground.Properties.GetValue<double>("Bottom"),bottom);
                }
            },
             new PropertyObserver()
            {
                Name = "Thickness",
                DisplayName = "厚度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdGround).Properties.GetValue<double>("Thickness"),
                Setter = (ele, value) =>
                {
                    var ground = (ele as QdGround);
                    if (!double.TryParse(value.ToString(),out var thickness)&&thickness<=0)
                        return;
                    ground.OnPropertyChangedBefore("Thickness",ground.Properties.GetValue<double>("Thickness"),thickness);
                    ground.Properties.SetValue("Thickness",thickness );
                    ground.ResetCache();
                    ground.OnPropertyChangedAfter("Thickness",ground.Properties.GetValue<double>("Thickness"),thickness);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Ground");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Ground";
                layer.Color = 0x00BFFF;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
