using LightCAD.Drawing;
using LightCAD.Drawing.Actions.Action;
using LightCAD.MathLib;
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
    public class RoadAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods; 
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private Vector2 firstPoint;
        private Vector2 secondPoint;
        private static double RoadWidth = 12000; 
        public RoadAction() { }

        public RoadAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Road");
        }
 
        static RoadAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateRoad",
                Description = "创建城市道路",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定第一个点或[选择已有线段创建城市道路(C)]:" },
                    new LcCreateStep { Name = "Step1", Options = "直城市道路下一点或 [弧线(A)/闭合(C)/回退(U)]<另一段>:" },
                    new LcCreateStep { Name = "Step2", Options = "指定下一个点或[放弃(U)]:" },
                    new LcCreateStep { Name = "Step3", Options = "选择绘制城市道路线段或[放弃(U)]:" },
                }
            };
        }

 

        public async Task ExecCreateByLine(string[] args = null)
        {
            var elementInputerInGroup = new ElementSetInputer(this.docEditor);
            var curMethod = CreateMethods[0];
        Step0:
            var step0 = curMethod.Steps[3];
            var result0 = await elementInputerInGroup.Execute(step0.Options);
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
                foreach (var ele in eles)
                {
                    if (ele is LcLine line)
                    {
                        CreateRoad(line.Curve.Clone());
                    }
                    else if (ele is LcPolyLine polyLine)
                    {
                        foreach (var curve in polyLine.Curve2ds)
                        {
                            CreateRoad(curve);
                        }
                    }
                    else if (ele is LcArc arc)
                    {

                    }
                    else
                    {
                        continue;
                    }
                    this.vportRt.ActiveElementSet.RemoveElement(ele);
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
        public async void ExecCreate(string[] args = null)
        {
            await this.StartCreating();
            pointInputer = new PointInputer(this.docEditor);
            var curMethod = CreateMethods[0];
        Step0:
            this.secondPoint = null;
            var step0 = curMethod.Steps[0];
            var result0 = await pointInputer.Execute(step0.Options);
            if (pointInputer.isCancelled|| result0 == null)
            {
                this.Cancel();
                goto End;
            }   
            if (result0.ValueX == null)
            {
                if (result0.Option != null)
                {
                    if (result0.Option.ToUpper() == "C")
                    {
                        await ExecCreateByLine();
                        goto End;
                    }
                    else
                        goto Step0;
                }
                else
                {
                    goto Step0;
                }
            }
            else
            {
                this.firstPoint = (Vector2)result0.ValueX;
            }
        Step1:
            var step1 = curMethod.Steps[1];
            var result1 = await pointInputer.Execute(step1.Options);
            if (pointInputer.isCancelled || result1 == null)
            {
                this.Cancel();
                goto End;
            }


            if (result1.ValueX == null)
            {
                if (result1.Extent != null)
                {
                    this.secondPoint = (Vector2)result1.Extent;
                }
                else if (result1.Option != null)
                {
                    if (result1.Option == "U") { }
                    else if (result1.Option == "C")
                    {
                        goto End;
                    }
                    else if (result1.Option == " ")
                    {
                        goto End;
                    }
                    else if (result1.ValueX != null)
                    {
                        Vector2 pointB = (Vector2)result1.ValueX;
                        Vector2 direction = (pointB - firstPoint).Normalize();
                        result1.ValueX = firstPoint + direction * double.Parse(result1.Option);
                    }
                    else
                    {
                        goto Step1;
                    }
                }
            }
            else
                this.secondPoint = (Vector2)result1.ValueX;

            CreateRoad(new Line2d(firstPoint.Clone(), secondPoint.Clone()));
            firstPoint = secondPoint.Clone();
            secondPoint = null;
            goto Step1;

        End:
            this.pointInputer = null;
            this.firstPoint = null;
            this.EndCreating();
        }
       
    
        //public async void ExecCreate(string[] args = null)
        //{
        //    var elementInputerInGroup = new ElementSetInputer(this.docEditor);
        //Step0:
        //    var result0 = await elementInputerInGroup.Execute("请选择已有线段创建城市道路:");
        //    if (elementInputerInGroup.isCancelled)
        //    {
        //        this.Cancel();
        //        goto End;
        //    }
        //    if (result0 == null)
        //    {
        //        this.Cancel();
        //        goto End;
        //    }
        //    if (result0.ValueX != null)
        //    {
        //        var eles = result0.ValueX as List<LcElement>;
        //        var lines = new List<LcCurve2d>();
        //        foreach (var ele in eles)
        //        {
        //            if (ele is LcLine line)
        //            {
        //                lines.Add(line);
        //            }
        //            else if (ele is LcPolyLine polyLine)
        //            {
        //                lines.Add(polyLine);
        //            }
        //            else if (ele is LcArc arc)
        //            {
        //                lines.Add(arc);
        //            }
        //        }
               
        //    }
        //    else if (result0.Option != null)
        //    {
        //        goto End;
        //    }
        //    else
        //    {
        //        goto Step0;
        //    }
        //End:
        //    elementInputerInGroup = null;
        //}
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }
 

        public void CreateRoad(Curve2d baseLine)
        {
            var doc = docRt.Document;
            var roadDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "城市道路", null) as QdRoadDef;
            var road = new QdRoad(roadDef);
            road.Initilize(doc);
            road.BaseCurve = baseLine;
            road.ResetBoundingBox();
            road.Layer = GetLayer().Name;
            road.Bottom = 5;
            road.Width = RoadAction.RoadWidth;
            road.Thickness = 200;
            road.Material = MaterialManager.GetMaterial(MaterialManager.AsphaltUuid);
            vportRt.ActiveElementSet.InsertElement(road);
            road.DisposeConnectRoad(true);
            road.ResetCache();
            RefreshConnect(road);
            docRt.Action.ClearSelects(); 
        }

        public void CreateRoad(Curve2d baseLine,double width)
        {
            var doc = docRt.Document;
            var roadDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "城市道路", null) as QdRoadDef;
            var road = new QdRoad(roadDef);
            road.Initilize(doc);
            road.BaseCurve = baseLine;
            road.ResetBoundingBox();
            road.Layer = GetLayer().Name;
            road.Bottom = 0;
            road.Width = width;
            road.Thickness = 200;
            road.Material = MaterialManager.GetMaterial(MaterialManager.AsphaltUuid);
            vportRt.ActiveElementSet.InsertElement(road);
            road.DisposeConnectRoad(true);
            road.ResetCache();
            RefreshConnect(road);
            docRt.Action.ClearSelects();
        }
        public void RefreshConnect(QdRoad road)
        {
            var roadDic = road.GetConnectRoads();
            foreach (var kvp in roadDic)
            {
                kvp.Value.ForEach(n => { n.DisposeConnectRoad(false);});
            } 
        }
        public override void DrawAuxLines(LcCanvas2d canvas)
        {
            if (this.pointInputer!=null&&this.firstPoint!=null)
            {
                var endP = this.pointInputer?.InputP;
                var line = new Line2d(this.firstPoint.Clone(),endP.Clone());
                canvas.DrawLine(LcPaint.Default ,line.Start,line.End);
                var leftLine = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(-RoadWidth/2)) as Line2d;
                canvas.DrawLine(LcPaint.Default, leftLine.Start, leftLine.End);
                var rightLine = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(RoadWidth / 2)) as Line2d;
                canvas.DrawLine(LcPaint.Default, rightLine.Start, rightLine.End);
            }

        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var road = element as QdRoad;
            var pen = GetDrawPen(road);
            DrawRoad(canvas, road, matrix, pen);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var road = element as QdRoad;
            var pen = GetDrawPen(road);
            var matrix = new Matrix3().MakeTranslation(offset.X, offset.Y);
            DrawRoad(canvas, road, matrix, pen);
        }


        public void DrawRoad(LcCanvas2d canvas, QdRoad road, Matrix3 matrix, LcPaint pen)
        {
            foreach (var mdiL in road.MdiLines)
            {
                foreach (var curve in road.CreateShape(mdiL,road.Width)) {
                    //pen.PathEffect = LcPathEffect.CreateSolid();
                    pen.IsScrDashLine = false;
                    canvas.DrawCurve(pen, curve, matrix);
                }
                pen.IsScrDashLine = true;
                canvas.DrawCurve(pen, mdiL, matrix);
            }
            foreach (var intersection in road.EmbedAssociations)
            {
                if (intersection.Host == road)
                {
                    foreach (var cur in (intersection.HostTag as List<Curve2d>))
                    {
                        pen.IsScrDashLine = true;
                        canvas.DrawCurve(pen, cur, matrix);
                    }
                }
            }

            LcTextPaint textPaint = new LcTextPaint { Color = new Color().Set(this.GetLayer().Color), FontName = "仿宋", FontName2 = "仿宋", Size = 800, WordSpace = 50 / 10 };
            textPaint.WidthFactor = 1;
            string text = "城市道路";
            textPaint.Position = road.BoundingBox.Center;
            canvas.DrawText(textPaint, text, new Matrix3(), out var charBoxs);
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var road = element as QdRoad;
            var grips = new List<ControlGrip>();
            var baseLine = road.BaseCurve as Line2d;
            var grip = new ControlGrip
            {
                Element = road,
                Name = $"Start",
                Position = baseLine.Start.Clone(),
            };
            var gripEnd = new ControlGrip
            {
                Element = road,
                Name = $"End",
                Position = baseLine.End.Clone(),
            };
            var gripC = new ControlGrip
            {
                Element = road,
                Name = $"Center",
                Position = baseLine.Mid.Clone(),
            };
            grips.Add(grip);
            grips.Add(gripEnd);
            grips.Add(gripC);
            return grips.ToArray();
        }

        private QdRoad _road;
        private string _gripName;
        private Vector2 _position;

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var road = element as QdRoad;
            _road = road;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                //var oldCrossDic = road.GetConnectRoads();
                var oldC = road.EmbedAssociations.Select(n=>(n.Embed as QdRoad).Id==road.Id? n.Host as QdRoad: n.Embed as QdRoad).ToList();
                if (grip.Name == "Center")
                {
                    var offset = position - (road.BaseCurve as Line2d).Mid;
                    road.Translate(offset);
                    road.DisposeConnectRoad(true);
                }
                else if (_gripName.StartsWith("Start"))
                {
                    (road.BaseCurve as Line2d).Start = position.Clone();
                    road.DisposeConnectRoad(true);
                }
                else if (_gripName.StartsWith("End"))
                {
                    (road.BaseCurve as Line2d).End = position.Clone();
                    road.DisposeConnectRoad(true);
                }
                var newC = road.EmbedAssociations.Select(n => (n.Embed as QdRoad).Id == road.Id ? n.Host as QdRoad : n.Embed as QdRoad).ToList();
                oldC.AddRange(newC);
                oldC = oldC.Distinct().ToList();
                foreach (var oc in oldC)
                {
                    oc.DisposeConnectRoad(false);
                }
                //var newCrossDic = road.GetConnectRoads();
                //foreach (var kvp in newCrossDic)
                //{
                //    oldCrossDic[kvp.Key].AddRange(kvp.Value.Where(n => oldCrossDic[kvp.Key].Any(m => n.Id != m.Id)));
                //}
                //RefreshConnect(road);
            }
        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            var qdRoad = element as QdRoad;
            var sscur = SnapSettings.Current;
            var result = new SnapPointResult { Element = element };
            if (sscur.ObjectOn )
            {
                var baseLine = qdRoad.BaseCurve as Line2d;

                if (GeoUtil.Vec2EQ(baseLine.Start, point, maxDistance))
                {
                    result.Point = baseLine.Start.Clone();
                    result.Name = "Start";
                    result.Curves.Add(new SnapRefCurve(SnapPointType.Endpoint, baseLine.Clone()));
                }

                if (GeoUtil.Vec2EQ(baseLine.End, point, maxDistance))
                {
                    result.Point = baseLine.End.Clone();
                    result.Name = "End";
                    result.Curves.Add(new SnapRefCurve(SnapPointType.Endpoint, baseLine.Clone()));
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
                Getter = (ele) => (ele as QdRoad).Properties.GetValue<double>("Bottom"),
                Setter = (ele, value) =>
                {
                    var road = (ele as QdRoad);
                    if (!double.TryParse(value.ToString(),out var bottom))
                        return;
                   road.Bottom=bottom;
                }
            },
             new PropertyObserver()
            {
                Name = "Thickness",
                DisplayName = "厚度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdRoad).Properties.GetValue<double>("Thickness"),
                Setter = (ele, value) =>
                {
                    var road = (ele as QdRoad);
                    if (!double.TryParse(value.ToString(),out var thickness)&&thickness<=0)
                        return;
                    road.Thickness= thickness;
                }
            },
             new PropertyObserver()
            {
                Name = "Width",
                DisplayName = "宽度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdRoad).Properties.GetValue<double>("Width"),
                Setter = (ele, value) =>
                {
                    var road = (ele as QdRoad);
                    if (!double.TryParse(value.ToString(),out var width)&&width<=0)
                        return;
                    road.Width=width;
                    road.DisposeConnectRoad(true);
                    RoadAction.RoadWidth=width;
                    RefreshConnect(road);
                }
            },
             new PropertyObserver()
            {
                Name = "Radius",
                DisplayName = "转弯半径",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdRoad).Radius ,
                Setter = (ele, value) =>
                {
                    var road = (ele as QdRoad);
                    if (!double.TryParse(value.ToString(),out var radius)&&radius<=0)
                        return;
                    road.Radius = radius;
                    road.DisposeConnectRoad(true);
                    RefreshConnect(road);
                }
            }
            };
        }
        private LcLayer GetLayer()
        {
            var layer = docRt.Document.Layers.FirstOrDefault(n => n.Name == "Layout_Road");
            if (layer == null)
            {
                layer = docRt.Document.CreateObject<LcLayer>();
                layer.Name = "Layout_Road";
                layer.Color = 0x00BFFF;
                
                layer.SetLineType(new LcLineType("ByLayer"));layer.Transparency = 0;
                docRt.Document.Layers.Add(layer);
            }
            return layer;
        }
        
    }
}
