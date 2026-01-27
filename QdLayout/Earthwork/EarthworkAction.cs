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
    public class EarthworkAction : ComponentInstance2dAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;  
        public EarthworkAction() { }

        public EarthworkAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Earthwork");
        }
        static EarthworkAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateEarthwork",
                Description = "创建土方回填",
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
            CreateEarthwork();

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
                    CreateEarthwork();
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
            commandCtrl.WriteInfo("绘制土方回填轮廓中...");
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
            CreateEarthwork();

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
 

        public void CreateEarthwork()
        {
            var doc = docRt.Document;
            var earthworkDef = docRt.GetUseComDef($"{NamespaceKey}.土方基坑", "土方回填", null) as QdEarthworkDef;
            var earthwork = new QdEarthwork(earthworkDef);
            earthwork.Initilize(doc);
            var poly = OutLoop.Clone() as LcPolyLine; 
            earthwork.Outline = poly.Curve.Clone() as Polyline2d;
            earthwork.ResetBoundingBox();
            earthwork.Layer = GetLayer().Name;
            earthwork.ElevationBottom = -4000;
            earthwork.ElevationTop = 0 ;
            earthwork.Material = MaterialManager.GetMaterial(MaterialManager.EarthworkUuid);
            vportRt.ActiveElementSet.InsertElement(earthwork);
            docRt.Action.ClearSelects();
        }
 
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var earthwork = element as QdEarthwork;
            var pen = GetDrawPen(earthwork);
            DrawEarthwork(canvas, earthwork, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var earthwork = element as QdEarthwork;
            var pen = GetDrawPen(earthwork);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawEarthwork(canvas, earthwork, matrix, pen);
        }


        public void DrawEarthwork(LcCanvas2d canvas, QdEarthwork earthwork, Matrix3 matrix, LcPaint pen)
        {
            var shape = earthwork.GetShapes()[0].Curve2ds;
            for (int i = 0; i < shape.Count; i++)
            {
                 canvas.DrawCurve(pen, shape[i], matrix);
            }
            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "土方回填";
            textPaint.Position = earthwork.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var earthwork = element as QdEarthwork;
            var grips = new List<ControlGrip>();
            var gripCenter = new ControlGrip
            {
                Element = earthwork,
                Name = "Center",
                Position = earthwork.BoundingBox.Center.Clone(),
            };
            grips.Add(gripCenter);
            var points = (earthwork.BaseCurve as Polyline2d).Curve2ds.Select(n => n.GetPoints(1)[0]).ToListEx();
            for (int i = 0; i < points.Count; i++)
            {
                var grip = new ControlGrip
                {
                    Element = earthwork,
                    Name = $"Outline_{i}",
                    Position = points[i]
                };
                grips.Add(grip);
            }
            return grips.ToArray();
        }

        private QdEarthwork _earthwork;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var earthwork = element as QdEarthwork;
            _earthwork = earthwork;
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
                    earthwork.Translate(offset);
                }
                else if (_gripName.StartsWith("Outline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = earthwork.Outline.Clone() as Polyline2d;
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
                    earthwork.Outline = poly;
                 }              
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdEarthwork = element as QdEarthwork;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn && qdEarthwork.GetShapes()[0].Curve2ds.Count > 0)
            {
                foreach (var curve in qdEarthwork.GetShapes()[0].Curve2ds)
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
            if (_earthwork == null)
                return;

        
        }
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
             new PropertyObserver()
            {
                Name = "ElevationBottom",
                DisplayName = "分段底标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdEarthwork).Properties.GetValue<double>("ElevationBottom"),
                Setter = (ele, value) =>
                {
                    var earthwork = (ele as QdEarthwork);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                    earthwork.OnPropertyChangedBefore("ElevationBottom",earthwork.Properties.GetValue<double>("Bottom"),bottom);
                    earthwork.Properties.SetValue("ElevationBottom",bottom );
                    earthwork.ResetCache();
                    earthwork.OnPropertyChangedAfter("ElevationBottom",earthwork.Properties.GetValue<double>("Bottom"),bottom);
                }
            },
             new PropertyObserver()
            {
                Name = "ElevationTop",
                DisplayName = "分段顶标高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdEarthwork).Properties.GetValue<double>("ElevationTop"),
                Setter = (ele, value) =>
                {
                    var earthwork = (ele as QdEarthwork);
                    if (!double.TryParse(value.ToString(),out var height))
                        return;
                    earthwork.OnPropertyChangedBefore("ElevationTop",earthwork.Properties.GetValue<double>("ElevationTop"),height);
                    earthwork.Properties.SetValue("ElevationTop",height );
                    earthwork.ResetCache();
                    earthwork.OnPropertyChangedAfter("ElevationTop",earthwork.Properties.GetValue<double>("ElevationTop"),height);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Earthwork");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Earthwork";
                layer.Color = 0xFFFF00;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
