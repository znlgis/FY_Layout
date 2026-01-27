using LightCAD.MathLib;
using netDxf.Collections;
using Newtonsoft.Json.Linq;
using OpenTK.Graphics.OpenGL;
using QdLayout.Fence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThreeJs4Net;
using static LightCAD.Core.Elements.LcPolyLine;
using static netDxf.Entities.HatchBoundaryPath;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public class PlanBuildAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods;

        private PointInputer PointInputer { get; set; }
        private ElementSetInputer ElementSetInputer { get; set; }
        

        private LcDocument LcDocument { get; set; }

        /// <summary>
        /// 基础多段线
        /// </summary>
        private Polyline2d BasePolyLine2D { get; set; }

        /// <summary>
        /// 点击的第一点
        /// </summary>
        private Vector2 FirstPoint { get; set; }

        /// <summary>
        /// 点击的上一个点
        /// </summary>
        private Vector2 BeforePoint { get; set; }

        /// <summary>
        /// 圆弧第二个点
        /// </summary>
        private Vector2 ArcSecondPoint { get; set; }

        /// <summary>
        /// 当前绘制图形状态
        /// </summary>
        private PlineSegmentType PolyLineType { get; set; }

        /// <summary>
        /// 拖拽点的名字
        /// </summary>
        private string _gripName;
        /// <summary>
        /// 拖拽点的位置
        /// </summary>
        private Vector2 _position;
        private QdPlanBuild _qdPlanBuild;


        private List<LcElement> PickLineLcELements { get; set; }

        public PlanBuildAction() { }

        public PlanBuildAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：PlanBuild");
            this.PointInputer = new PointInputer(this.docEditor);
            this.ElementSetInputer = new ElementSetInputer(this.docEditor);
            this.LcDocument = this.docRt.Document;
            this.BasePolyLine2D = new Polyline2d();
            this.BasePolyLine2D.Curve2ds = new List<Curve2d>();
            this.PolyLineType = PlineSegmentType.Line;
            this.PickLineLcELements = new List<LcElement>();

        }

        static PlanBuildAction()
        {
            CreateMethods = new LcCreateMethod[3];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreatePlanBuild",
                Description = "创建拟建建筑",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "请拾取起点: " },
                    new LcCreateStep { Name = "Step1", Options = "下一点[闭合(C) 放弃(U)]<回车结束>: " },
                    new LcCreateStep { Name = "Step2", Options = "请拾取中间点[回退(U)]: " },
                    new LcCreateStep { Name = "Step3", Options = "请输入下一点: " },
                }
            }; 
            CreateMethods[1] = new LcCreateMethod()
            {
                Name = "PickLineCreatePlanBuild",
                Description = "拾取线创建拟建建筑",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "请拾取封闭轮廓: " },
                }
            };
        }

        public async void ExecCreatePlanBuild(string[] args = null)
        {
            await this.StartCreating();
            string method = "PlanBuild";
            if (args != null && args.Length > 0) { method = args[0]; }
            LcCreateMethod curMethod = CreateMethods[0];

        Step0:
            var step0 = curMethod.Steps[0];
            var result0 = await this.PointInputer.Execute(step0.Options);
            if (this.PointInputer.isCancelled) { this.Cancel(); return; }
            if (result0.ValueX != null)
            {
                this.FirstPoint = (Vector2)result0.ValueX;
                this.BeforePoint = this.FirstPoint.Clone();
                goto Step1;
            }
            else if (result0.ValueX == null)
            {

            }

        Step1:
            var step1 = curMethod.Steps[1];
            var result1 = await this.PointInputer.Execute(step1.Options);
            if (this.PointInputer.isCancelled)
            {
                if (CurrentPointIsConnected(this.FirstPoint.Clone(), this.BasePolyLine2D.Curve2ds.Clone(), true)) goto Step1;
                this.Cancel(); goto End; 
            }
            if (result1.ValueX != null)
            {
                Vector2 otherPoint = (Vector2)result1.ValueX;
                if(CurrentPointIsConnected(otherPoint.Clone(), this.BasePolyLine2D.Curve2ds.Clone(), false)) goto Step1;

                this.BasePolyLine2D.Curve2ds.Add(new Line2d(this.BeforePoint.Clone(), otherPoint.Clone()));
                this.BeforePoint = otherPoint.Clone();
                goto Step1;
            }
            else if (result1.ValueX == null)
            {
                if (result1.Option == null)
                {
                    if (CurrentPointIsConnected(this.FirstPoint.Clone(), this.BasePolyLine2D.Curve2ds.Clone(), true)) goto Step1;
                    this.Cancel(); goto End;
                }
                else if (result1.Option.ToUpper() == "C")
                {
                    if (this.BasePolyLine2D.Curve2ds.Count < 2)
                    {
                        this.Cancel(); return;
                    }

                    if (CurrentPointIsConnected(this.FirstPoint.Clone(), this.BasePolyLine2D.Curve2ds.Clone(), true)) goto Step1;

                    goto End;
                }
                else if (result1.Option.ToUpper() == "U")
                {
                    if (this.BasePolyLine2D.Curve2ds.Count >= 1)
                    {
                        Curve2d curve2D = this.BasePolyLine2D.Curve2ds.Last();
                        if (curve2D is Line2d line2d) 
                        {
                            this.BeforePoint = line2d.Start.Clone();
                        }
                        if(curve2D is Arc2d arc2d)
                        { 
                            this.BeforePoint = arc2d.Startp.Clone();
                        }
                        this.BasePolyLine2D.Curve2ds.Remove(curve2D);
                    }
                    goto Step1;
                }
            }
        Step3:
            var step3 = curMethod.Steps[1];
            var result3 = await this.PointInputer.Execute(step0.Options);
            if (this.PointInputer.isCancelled) { this.Cancel(); return; }


        End:
            if (this.BasePolyLine2D.Curve2ds.Count() > 0)
            {
                QdPlanBuildDef planBuildDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "拟建建筑", null) as QdPlanBuildDef;
                QdPlanBuild planBuild = new QdPlanBuild(planBuildDef);
                planBuild.Initilize(LcDocument);
                this.BasePolyLine2D.Curve2ds.Add(new Line2d(this.BeforePoint.Clone(), this.FirstPoint.Clone()));
                planBuild.BasePolyline = new Polyline2d();
                planBuild.BasePolyline.Curve2ds = this.BasePolyLine2D.Curve2ds;
                planBuild.BaseCurve = this.BasePolyLine2D;
                LcPolyLineCreateQdPlanBuild(planBuild);
            }

            this.EndCreating();
        }

        public async void ExecPickLineCreatePlanBuild(string[] args = null)
        {
            await this.StartCreating();
            string method = "PlanBuild";
            if (args != null && args.Length > 0) { method = args[0]; }
            LcCreateMethod curMethod = CreateMethods[1];
            if (this.docRt.Action.SelectedElements.Count > 0)
            {
                this.PickLineLcELements = this.docRt.Action.SelectedElements;
                goto End;
            }
            else
            {
                goto Step0;
            }

        Step0:
            var step0 = curMethod.Steps[0];
            var result0 = await this.ElementSetInputer.Execute(step0.Options);
            if (this.ElementSetInputer.isCancelled) { this.Cancel(); return; }

            if (((List<LcElement>)result0.ValueX).Count != 0)
            {
                this.PickLineLcELements = this.docRt.Action.SelectedElements;
                goto End;
            }
            else
            {
                goto Step0;
            }

        End:
            if (this.PickLineLcELements.Count > 0)
            {         
                //List<Line2d> lineCurves = new List<Line2d>();
                foreach (var item in this.PickLineLcELements)
                {
                    //if (item is LcLine) 
                    //{
                    //    LcLine lcLine = (LcLine)item;        
                    //    lineCurves.Add(lcLine.Line);
                    //}

                    if (item is LcPolyLine && (item as LcPolyLine).IsClosed) 
                    {
                        QdPlanBuildDef planBuildDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "拟建建筑", null) as QdPlanBuildDef;
                        QdPlanBuild planBuild = new QdPlanBuild(planBuildDef);
                        planBuild.Initilize(LcDocument);

                        planBuild.BasePolyline = new Polyline2d();
                        planBuild.BasePolyline.Curve2ds = (item as LcPolyLine).Curve2ds.ToList();
                        planBuild.BaseCurve = planBuild.BasePolyline;
                        LcPolyLineCreateQdPlanBuild(planBuild);
                    }      
                    
                }

                //List<List<Line2d>> connectedLineGroups = GetConnectedLineGroups(lineCurves);
   
                //foreach (var group in connectedLineGroups)
                //{
                //    if (IsPolylineClosed(group))
                //    {
                //        List<Curve2d> newCurves = new List<Curve2d>();
                //        foreach (var item in group)
                //        {
                //            newCurves.Add(item);
                //        }

                //        QdPlanBuildDef planBuildDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "拟建建筑", null) as QdPlanBuildDef;
                //        QdPlanBuild planBuild = new QdPlanBuild(planBuildDef);
                //        planBuild.Initilize(LcDocument);

                //        planBuild.BasePolyline = new Polyline2d();
                //        planBuild.BasePolyline.Curve2ds = newCurves;
                //        planBuild.BaseCurve = planBuild.BasePolyline;
                //        LcPolyLineCreateQdPlanBuild(planBuild);
                //    }
                //    else
                //    {
                //        Console.WriteLine("找到一组相连但未闭合的线条集合");

                //    }
                //}
            }
            this.EndCreating();
        }


        private void LcPolyLineCreateQdPlanBuild(QdPlanBuild planBuild) 
        {
            double signedArea = planBuild.UpdatePolylineReverseSide();

            planBuild.OneBottomLevel = "自然设计地坪(0)";
            planBuild.GroundUpStorey = 5;
            planBuild.GroundDownStorey = 0;
            planBuild.StoreyHeight = 3000;
            planBuild.SignedArea = signedArea;

            this.vportRt.ActiveElementSet.InsertElement(planBuild);
        }

        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            LcPaint auxPen = GetAuxDrawPen();
            QdPlanBuild qdPlanBuild = element as QdPlanBuild; 
            foreach (var ele in qdPlanBuild.BasePolyline.Curve2ds)
            {
                if (ele.Type == Curve2dType.Line2d)
                {
                    Line2d line2d = ele as Line2d;
                    canvas.DrawLine(auxPen, line2d.Start, line2d.End);
                }
                else if (ele.Type == Curve2dType.Arc2d)
                {
                    Arc2d arc2d = ele as Arc2d;
                    canvas.DrawArc(auxPen, arc2d.Center, arc2d.Radius, arc2d.StartAngle, arc2d.EndAngle, arc2d.IsClockwise);
                }
            }
            //LcText lcText = new LcText();
            //lcText.Position = new Vector2(0, 0);
            //lcText.TextString = "拟建建筑";
        }

        public override void DrawTemp(LcCanvas2d canvas)
        {
            if (this.FirstPoint == null) { return; }
            LcPaint auxPen = GetAuxDrawPen();
            Vector2 mousePoint = this.PointInputer?.InputP;


            if (this.BasePolyLine2D.Curve2ds.Count() > 0)
            {
                foreach (var ele in this.BasePolyLine2D.Curve2ds)
                {
                    Vector2 start = new Vector2();
                    Vector2 end = new Vector2();
                    if (ele.Type == Curve2dType.Line2d)
                    {
                        start = (ele as Line2d).Start;
                        end = (ele as Line2d).End;
                        canvas.DrawLine(auxPen, start, end);
                    }
                    else if (ele.Type == Curve2dType.Arc2d)
                    {
                        start = (ele as Arc2d).Startp;
                        end = (ele as Arc2d).Endp;
                        Arc2d arc = ele as Arc2d;
                        Matrix3 matrix = Matrix3.GetTranslate(new Vector2(0, 0));
                        canvas.DrawArc(auxPen, arc.Center, arc.Radius, arc.StartAngle, arc.EndAngle, arc.IsClockwise);
                    }
                }

                if (this.PolyLineType == PlineSegmentType.Arc)
                {
                    Arc2d arc2d = Arc2d.CreateARC(this.FirstPoint, mousePoint, this.ArcSecondPoint);
                    canvas.DrawArc(auxPen, arc2d.Center, arc2d.Radius, arc2d.StartAngle, arc2d.EndAngle, arc2d.IsClockwise);
                    canvas.DrawLine(auxPen, this.FirstPoint, mousePoint);
                }
                else
                {
                    canvas.DrawLine(auxPen, this.BeforePoint, mousePoint);
                    canvas.DrawLine(auxPen, this.FirstPoint, mousePoint);
                }
            }
            else
            {
                if (this.FirstPoint != null)
                {
                    canvas.DrawLine(auxPen, this.FirstPoint, mousePoint);
                }
            }
        }

        public override void DrawAuxLines(LcCanvas2d canvas)
        {
            Vector2 mp = this.PointInputer?.InputP;

            if (this.FirstPoint != null)
            {
                DrawingGauxiliaryLine(canvas, 60 / this.vportRt.Viewport.Scale, this.FirstPoint, mp);
                DrawingGauxiliaryAngle(canvas, this.FirstPoint, mp);
            }
        }

        /// <summary>
        /// 属性栏
        /// </summary>
        /// <returns></returns>
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>()
            {
                //new PropertyObserver()
                //{
                //    Name = "ComponentStyle",
                //    DisplayName = "构件样式",
                //    PropType = PropertyType.Array,
                //    Source = (ele) =>
                //    {
                //        List<string> componentStyles = ["拟建建筑", "现有建筑"];
                //        return componentStyles.ToArray();
                //    },
                //    Getter = (ele) => (ele as QdPlanBuild).ComponentStyle,
                //    Setter = (ele, value) =>
                //    {
                //        (ele as QdPlanBuild).Set(componentStyle:Convert.ToString(value));
                //    }
                //},
                new PropertyObserver()
                {
                    Name = "OneBottomLevel",
                    DisplayName = "1层底标高",
                    Source = (ele) =>
                    {
                        List<string> oneBottomLevels = ["自然设计地坪(0)", "-0.150", "-0.300", "-0.450"];
                        return oneBottomLevels.ToArray();
                    },
                    Getter = (ele) => (ele as QdPlanBuild).OneBottomLevel,
                    Setter = (ele, value) =>
                    {
                        (ele as QdPlanBuild).OneBottomLevel=Convert.ToString(value);
                    }
                },
                new PropertyObserver()
                {
                    Name = "GroundUpStorey",
                    DisplayName = "地上层数",
                    Getter = (ele) => (ele as QdPlanBuild).GroundUpStorey,
                    Setter = (ele, value) =>
                    {
                        (ele as QdPlanBuild).GroundUpStorey=Convert.ToDouble(value);

                    }
                },
                new PropertyObserver()
                {
                    Name = "GroundDownStorey",
                    DisplayName = "地下层数",
                    Getter = (ele) => (ele as QdPlanBuild).GroundDownStorey,
                    Setter = (ele, value) =>
                    {
                        (ele as QdPlanBuild).GroundDownStorey=Convert.ToDouble(value);
                    }
                },
                new PropertyObserver()
                {
                    Name = "StoreyHeight",
                    DisplayName = "层高",
                    Getter = (ele) => (ele as QdPlanBuild).StoreyHeight,
                    Setter = (ele, value) =>
                    {
                        (ele as QdPlanBuild).StoreyHeight=Convert.ToDouble(value);
                    }
                },
                //new PropertyObserver()
                //{
                //    Name = "BuildType",
                //    DisplayName = "建筑类型",
                //    Source = (ele) =>
                //    {
                //        List<string> buildTypes = ["现浇混凝土", "装配式"];
                //        return buildTypes.ToArray();
                //    },
                //    Getter = (ele) => (ele as QdPlanBuild).BuildType,
                //    Setter = (ele, value) =>
                //    {
                //        (ele as QdPlanBuild).Set(buildType:Convert.ToString(value));
                //    }
                //}
            };
        }

        /// <summary>
        /// 多段线可拖拽的点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var qdPlanBuild = element as QdPlanBuild;
            var grips = new List<ControlGrip>();

            var gripCenter = new ControlGrip
            {
                Element = qdPlanBuild,
                Name = "Center",
                Position = qdPlanBuild.BoundingBox.Center
            };
            grips.Add(gripCenter);
            var points = qdPlanBuild.BasePolyline.Curve2ds.Select(n => n.GetPoints(1)[0]).ToList();

            //List<Vector2> newPoints = new List<Vector2>();
            //newPoints.Add(points.Last());
            //points.Remove(points.Last());
            //foreach (var item in points)
            //{
            //    newPoints.Add(item);
            //}
            for (int i = 0; i < points.Count; i++)
            {
                var grip = new ControlGrip
                {
                    Element = qdPlanBuild,
                    Name = $"BasePolyline_{i}",
                    Position = points[i]
                };
                grips.Add(grip);
            }
            return grips.ToArray();

        }

        /// <summary>
        /// 创建拖拽后的多段线
        /// </summary>
        /// <param name="element"></param>
        /// <param name="grip"></param>
        /// <param name="position"></param>
        /// <param name="isEnd"></param>
        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var qdPlanBuild = element as QdPlanBuild;
            _qdPlanBuild = qdPlanBuild;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                if (grip.Name == "Center")
                {
                    var offset = position - qdPlanBuild.BoundingBox.Center;
                    qdPlanBuild.Translate(offset);
                }
                else if (_gripName.StartsWith("BasePolyline"))
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var poly = qdPlanBuild.BasePolyline.Clone() as Polyline2d;
                    qdPlanBuild.OnPropertyChangedBefore(nameof(qdPlanBuild.BasePolyline), qdPlanBuild.BasePolyline, qdPlanBuild.BasePolyline);
                    (poly.Curve2ds[idx] as Line2d).Start = position.Clone();
                    if (qdPlanBuild.SignedArea > 0) 
                    {
                        if (idx == 0)
                        {
                            (poly.Curve2ds[idx + 1] as Line2d).End = position.Clone();
                        }
                        else
                        {
                            if (idx == poly.Curve2ds.Count - 1) 
                            {
                                (poly.Curve2ds[0] as Line2d).End = position.Clone();
                            }
                            else
                            {
                                (poly.Curve2ds[idx + 1] as Line2d).End = position.Clone();
                            }
                        }
                    }
                    else
                    {
                        if (idx == 0)
                        {
                            (poly.Curve2ds.Last() as Line2d).End = position.Clone();
                        }
                        else
                        {
                            (poly.Curve2ds[idx - 1] as Line2d).End = position.Clone();
                        }
                    }

                    qdPlanBuild.BasePolyline = poly;
                    qdPlanBuild.ResetCache();
                    qdPlanBuild.OnPropertyChangedAfter(nameof(qdPlanBuild.BasePolyline), poly, qdPlanBuild.BasePolyline);
                }
            }
        }
        /// <summary>
        /// 拖拽渲染
        /// </summary>
        /// <param name="canvas"></param>
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_qdPlanBuild == null)
                return;
        }
        void RenderCurves(LcPolyLine drawPolyline, int oriNum, int influenceNum, Vector2 vector, List<Line2d> drawlines, List<Arc2d> drawarcs)
        {
            Vector2 temporaryPoint = new Vector2();
            if (drawPolyline.Curve2ds[influenceNum].Type == Curve2dType.Line2d)
            {
                Line2d line1 = new Line2d();
                if (drawPolyline.IsClosed && influenceNum == drawPolyline.Curve2ds.Count - 1)
                {
                    line1.Start = GetStart(drawPolyline.Curve2ds[influenceNum]);
                    line1.End = GetEnd(drawPolyline.Curve2ds[influenceNum]) + vector;
                }
                else
                {
                    line1.Start = GetStart(drawPolyline.Curve2ds[influenceNum]) + vector;
                    line1.End = GetEnd(drawPolyline.Curve2ds[influenceNum]);
                }
                drawlines.Add(line1);
            }
            else
            {
                var arc = drawPolyline.Curve2ds[influenceNum] as Arc2d;
                var mid1 = arc.GetMidPoint(arc.IsClockwise);
                Arc2d arc1 = new Arc2d();
                if (GetStart(drawPolyline.Curve2ds[influenceNum]) == GetEnd(drawPolyline.Curve2ds[oriNum]))
                {
                    temporaryPoint = GetStart(drawPolyline.Curve2ds[influenceNum]) + vector;
                    arc1 = Arc2d.CreateARC(temporaryPoint, mid1, GetEnd(drawPolyline.Curve2ds[influenceNum]));
                }
                else
                {
                    temporaryPoint = GetEnd(drawPolyline.Curve2ds[influenceNum]) + vector;
                    arc1 = Arc2d.CreateARC(GetStart(drawPolyline.Curve2ds[influenceNum]), mid1, temporaryPoint);
                }
                drawarcs.Add(arc1);
            }
        }
        Arc2d CreateArc(LcPolyLine polyline, int oriNum, int influNum, Vector2 vector)
        {
            var mid1 = (polyline.Curve2ds[influNum] as Arc2d).GetMidPoint((polyline.Curve2ds[influNum] as Arc2d).IsClockwise);
            Arc2d arc1 = new Arc2d();
            Vector2 temporaryPoint1 = new Vector2();
            if (GetEnd(polyline.Curve2ds[influNum]) == GetStart(polyline.Curve2ds[oriNum]))
            {
                temporaryPoint1 = GetEnd(polyline.Curve2ds[influNum]) + vector;
                arc1 = Arc2d.CreateARC(GetStart(polyline.Curve2ds[influNum]), mid1, temporaryPoint1);

            }
            else
            {
                temporaryPoint1 = GetStart(polyline.Curve2ds[influNum]) + vector;
                arc1 = Arc2d.CreateARC(temporaryPoint1, mid1, GetEnd(polyline.Curve2ds[influNum]));
            }
            return arc1;
        }
        public override SnapPointResult SnapInteraction(SnapRuntime snapRt, List<LcElement> elements, Vector2 point, double maxDistance, bool IsReturn)
        {
            return base.SnapInteraction(snapRt, elements, point, maxDistance, IsReturn);
        }

        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }

        public bool CurrentPointIsConnected(Vector2 otherPoint, List<Curve2d> curve2ds, bool isClosed)
        {
            if (curve2ds.Count == 0) return false;
            if (isClosed)
            {
                curve2ds.Remove(curve2ds.First());
                curve2ds.Remove(curve2ds.Last());
            }
            else
            {
                curve2ds.Remove(curve2ds.Last());
            }

            if (this.PolyLineType == PlineSegmentType.Line)
            {
                Line2d tempLine2d = new Line2d(this.BeforePoint.Clone(), otherPoint);
                foreach (var item in curve2ds)
                {
                    if (item is Line2d oldLind2d)
                    {
                        if (Intersect2d.IsLineWithLine(oldLind2d, tempLine2d))
                        {
                            return true;
                        }
                    }
                    else if (item is Arc2d oldArc2d)
                    {
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else if (this.PolyLineType == PlineSegmentType.Arc)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
            
        
        /// <summary>
        /// 得到连接在一起的线条组
        /// </summary>
        /// <param name="lineCurves"></param>
        /// <returns></returns>
        //public List<List<Line2d>> GetConnectedLineGroups(List<Line2d> lineCurves)
        //{
        //    List<List<Line2d>> connectedGroups = new List<List<Line2d>>();
        //    HashSet<Line2d> usedLines = new HashSet<Line2d>();

        //    foreach (var line in lineCurves)
        //    {
        //        if (usedLines.Contains(line))
        //            continue;

        //        List<Line2d> group = new List<Line2d> { line };
        //        usedLines.Add(line);

        //        FindConnectedLines(lineCurves, group, usedLines);

        //        connectedGroups.Add(group);
        //    }

        //    return connectedGroups;
        //}
        /// <summary>
        /// 查找连接的线条
        /// </summary>
        /// <param name="lineCurves"></param>
        /// <param name="group"></param>
        /// <param name="usedLines"></param>
        //public void FindConnectedLines(List<Line2d> lineCurves, List<Line2d> group, HashSet<Line2d> usedLines)
        //{
        //    Line2d currentLine = group.Last();

        //    foreach (var line in lineCurves)
        //    {
        //        if (usedLines.Contains(line))
        //            continue;

        //        if (AreLinesConnected(currentLine, line))
        //        {
        //            group.Add(line);
        //            usedLines.Add(line);
        //            FindConnectedLines(lineCurves, group, usedLines);
        //        }
        //    }
        //}
        /// <summary>
        /// 判断两个线条是否有连接
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        //public bool AreLinesConnected(Line2d line1, Line2d line2)
        //{
        //    return line1.End.Equals(line2.Start) || line1.Start.Equals(line2.End);
        //}
        ///// <summary>
        ///// 判断线条是否闭合
        ///// </summary>
        ///// <param name="lineGroup"></param>
        ///// <returns></returns>
        //public bool IsPolylineClosed(List<Line2d> lineGroup)
        //{
        //    if (lineGroup.Count < 2) return false;

        //    return lineGroup[0].Start.Equals(lineGroup.Last().End) ||
        //           lineGroup[0].End.Equals(lineGroup.Last().Start);
        //}

    }
}
