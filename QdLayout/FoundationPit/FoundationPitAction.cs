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
    public class FoundationPitAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public FoundationPitAction() { }

        public FoundationPitAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：FoundationPit");
        }
        static FoundationPitAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateFoundationPit",
                Description = "创建基坑",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定轮廓第一个点:" },
                    new LcCreateStep { Name = "Step1", Options = "指定轮廓下一点或 [结束(E)]" }, 
                }
            };
        }
        public async void ExecCreateRec(string[] args = null)
        {

            OutLoop = null;
            commandCtrl.WriteInfo("绘制基坑轮廓中...");
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
            CreateFoundationPit();

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
            var result0 = await elementInputerInGroup.Execute("请选择已有闭合线段创建基坑:");
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
                    CreateFoundationPit();
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
        public async void ExecCreatePoly(string[] args = null)
        {
             
            OutLoop = null; 
            commandCtrl.WriteInfo("绘制基坑轮廓中...");
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
            CreateFoundationPit();

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
        
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }

        public void CreateFoundationPit()
        {
            var doc = docRt.Document;
            var fdPitDef = docRt.GetUseComDef($"{NamespaceKey}.土方基坑", "基坑", null) as QdFoundationPitDef;
            var fdPit = new QdFoundationPit(fdPitDef);
            fdPit.Initilize(doc);
            var box = new Box2().ExpandByPoints(OutLoop.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
            var poly = OutLoop.Clone() as LcPolyLine; 
            fdPit.Outline = poly.Curve.Clone() as Polyline2d;
            fdPit.Pattern = 0;
            fdPit.Bottom = -4000;
            fdPit.Elevation = 0;
            fdPit.Factor = 0.4;
            fdPit.ResetBoundingBox();
            fdPit.Layer = GetLayer().Name; 
            vportRt.ActiveElementSet.InsertElement(fdPit);
            //fdPit.CheckInSite();
            docRt.Action.ClearSelects();
        }

        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var fdPit = element as QdFoundationPit;
            var pen = GetDrawPen(fdPit);
            DrawFoundationPit(canvas, fdPit, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var fdPit = element as QdFoundationPit;
            var pen = GetDrawPen(fdPit);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawFoundationPit(canvas, fdPit, matrix, pen);
        }


        public void DrawFoundationPit(LcCanvas2d canvas, QdFoundationPit fdPit, Matrix3 matrix, LcPaint pen)
        {
            foreach (var shape in fdPit.GetShapes())
            {
                 canvas.DrawCurves(pen, shape.Curve2ds.ToArray(), matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "基坑";
            textPaint.Position = fdPit.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);

        }
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
             new PropertyObserver()
            {
                Name = "Bottom",
                DisplayName = "土方底绝对标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFoundationPit).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var fdPit = (ele as QdFoundationPit);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    fdPit.Bottom=bottom;
                    //fdPit.OnPropertyChangedBefore("Bottom",fdPit.Properties.GetValue<double>("Bottom"),bottom);
                    //fdPit.Properties.SetValue("Bottom",bottom );
                    //fdPit.ResetCache();
                    //fdPit.OnPropertyChangedAfter("Bottom",fdPit.Properties.GetValue<double>("Bottom"),bottom);
                }
            },
               new PropertyObserver()
            {
                Name = "Elevation",
                DisplayName = "土方顶绝对标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFoundationPit).Elevation,
                Setter = (ele, value) =>
                {
                    var fdPit = (ele as QdFoundationPit);
                    if (!double.TryParse(value.ToString(),out var elevation))
                        return;
                    fdPit.Elevation=elevation;
                    //fdPit.OnPropertyChangedBefore("Elevation",fdPit.Properties.GetValue<double>("Elevation"),elevation);
                    //fdPit.Properties.SetValue("Elevation",elevation );
                    //fdPit.ResetCache();
                    //fdPit.OnPropertyChangedAfter("Elevation",fdPit.Properties.GetValue<double>("Elevation"),elevation);
                }
            },
              new PropertyObserver()
            {
                Name = "Factor",
                DisplayName = "放坡系数",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFoundationPit).Factor,
                Setter = (ele, value) =>
                {
                    var fdPit = (ele as QdFoundationPit);
                    if (!double.TryParse(value.ToString(),out var factor))
                        return;
                    fdPit.Factor=factor;
                    //fdPit.OnPropertyChangedBefore("Factor",fdPit.Properties.GetValue<double>("Factor"),factor);
                    //fdPit.Properties.SetValue("Factor",factor );
                    //fdPit.ResetCache();
                    //fdPit.OnPropertyChangedAfter("Factor",fdPit.Properties.GetValue<double>("Factor"),factor);
                }
            },
              new PropertyObserver()
            {
                Name = "Pattern",
                DisplayName = "放坡方式",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Array,
                Source= (ele)=> new string[]{"向外放坡","向内放坡" },
                Getter = (ele) => (ele as QdFoundationPit).Pattern==0?"向外放坡":"向内放坡" ,
                Setter = (ele, value) =>
                {
                    var fdPit = (ele as QdFoundationPit);
                    var pattern=value?.ToString()=="向外放坡"?0:1;
                    fdPit.Pattern=pattern;
                    //fdPit.OnPropertyChangedBefore("Pattern",fdPit.Properties.GetValue<int>("Pattern"),pattern);
                    //fdPit.Properties.SetValue("Pattern",pattern);
                    //fdPit.ResetCache();
                    //fdPit.OnPropertyChangedAfter("Pattern",fdPit.Properties.GetValue<int>("Pattern"),pattern);
                }
            } 
            };
        }
        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var fdPit = element as QdFoundationPit;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = fdPit,
                Name = "Center",
                Position = fdPit.BoundingBox.Center.Clone()
            };
            grips.Add(gripCenter);
            var points = (fdPit.BaseCurve as Polyline2d).Curve2ds.Select(n => n.GetPoints(1)[0]).ToListEx();
            for (int i = 0; i < points.Count; i++)
            {
                var grip = new ControlGrip
                {
                    Element = fdPit,
                    Name = $"Outline_{i}",
                    Position = points[i]
                };
                grips.Add(grip);
            }
            return grips.ToArray();
        }

        private QdFoundationPit _fdPit;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var fdPit = element as QdFoundationPit;
            _fdPit = fdPit;
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
                    fdPit.Translate(offset);
                }
                else if (_gripName.StartsWith("Outline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = fdPit.Outline.Clone() as Polyline2d;
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
                    fdPit.Outline = poly;
                }
            }     
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdFoundationPit = element as QdFoundationPit;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn && qdFoundationPit.GetShapes()[0].Curve2ds.Count > 0)
            {
                foreach (var curve in qdFoundationPit.GetShapes()[0].Curve2ds)
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
            if (_fdPit == null)
                return;

        
        }
      
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_FoundationPit");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_FoundationPit";
                layer.Color = 0xFFFF00;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
