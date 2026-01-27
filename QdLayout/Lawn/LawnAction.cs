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
    public class LawnAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public LawnAction() { }

        public LawnAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Lawn");
        }
        static LawnAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateLawn",
                Description = "创建草坪",
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
            commandCtrl.WriteInfo("绘制草坪轮廓中...");
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
            CreateLawn();

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
            this.EndCreating();
            OutLoop = null;
            commandCtrl.WriteInfo("绘制草坪轮廓中...");
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
            CreateLawn();

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
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建草坪:");
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
                    CreateLawn();
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
 

        public void CreateLawn()
        {
            var doc = docRt.Document;
            var lawnDef = docRt.GetUseComDef($"{NamespaceKey}.绿色文明", "草坪", null) as QdLawnDef;
            var lawn = new QdLawn(lawnDef);
            lawn.Initilize(doc);
            var poly = OutLoop.Clone() as LcPolyLine; 
            lawn.Outline = poly.Curve.Clone() as Polyline2d;
            lawn.ResetBoundingBox();
            lawn.Layer = GetLayer().Name;
            lawn.Bottom = 0;
            lawn.Material = MaterialManager.GetMaterial(MaterialManager.LawnUuid);
            vportRt.ActiveElementSet.InsertElement(lawn);
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var lawn = element as QdLawn;
            var pen = GetDrawPen(lawn);
            DrawLawn(canvas, lawn, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var lawn = element as QdLawn;
            var pen = GetDrawPen(lawn);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawLawn(canvas, lawn, matrix, pen);
        }


        public void DrawLawn(LcCanvas2d canvas, QdLawn lawn, Matrix3 matrix, LcPaint pen)
        {
            for (int i = 0; i < lawn.GetShapes()[0].Curve2ds.Count; i++)
            {
                 canvas.DrawCurve(pen, lawn.GetShapes()[0].Curve2ds[i], matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "草坪";
            textPaint.Position = lawn.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var lawn = element as QdLawn;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = lawn,
                Name = "Center",
                Position = lawn.BoundingBox.Center.Clone()
            };
            grips.Add(gripCenter);
            var points = lawn.GetShapes()[0].Curve2ds.Select(n => n.GetPoints(1)[0]).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var grip = new ControlGrip
                {
                    Element = lawn,
                    Name = $"Outline_{i}",
                    Position = points[i]
                };
                grips.Add(grip);
            }
            return grips.ToArray();
        }

        private QdLawn _lawn;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var lawn = element as QdLawn;
            _lawn = lawn;
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
                    lawn.Translate(offset);
                }
                else if (_gripName.StartsWith("Outline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = lawn.Outline.Clone() as Polyline2d;
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
                    lawn.Outline = poly;
                }              
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdLawn = element as QdLawn;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn && qdLawn.GetShapes()[0].Curve2ds.Count > 0)
            {
                foreach (var curve in qdLawn.GetShapes()[0].Curve2ds)
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
            if (_lawn == null)
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
                Getter = (ele) => (ele as QdLawn).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var lawn = (ele as QdLawn);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    lawn.OnPropertyChangedBefore("Bottom",lawn.Properties.GetValue<double>("Bottom"),bottom);
                    lawn.Properties.SetValue("Bottom",bottom );
                    lawn.ResetCache();
                    lawn.OnPropertyChangedAfter("Bottom",lawn.Properties.GetValue<double>("Bottom"),bottom);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Lawn");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Lawn";
                layer.Color = 0x00FF00;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
 