using LightCAD.Core.Elements;
using LightCAD.Drawing;
using LightCAD.Drawing.Actions.Action;
using QdLayout.Fence;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using ThreeJs4Net;
using static netDxf.Entities.HatchBoundaryPath;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public class FenceAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods;
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;
        private Polyline2d polyline2D = new Polyline2d();
        private Vector2 firstPoint = null;
        public FenceAction() { }

        public FenceAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Fence");
        }
        static FenceAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateLawn",
                Description = "创建围栏",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定围栏第一个点:" },
                    new LcCreateStep { Name = "Step1", Options = "下一点[圆弧(A)/闭合(C)/放弃(U)]:" },
                    new LcCreateStep { Name = "Step2", Options = "下一点[圆弧(A)/闭合(C)/放弃(U)]:" },
                }
            };
        }
        public async void ExecCreate(string[] args = null)
        {
            polyline2D.Curve2ds = new List<Curve2d>();


            if (args.Length > 0)
            {

            }
            //this.points = new List<Vector2>();
            await this.StartCreating();
            this.pointInputer = new PointInputer(this.docEditor);
            var curMethod = CreateMethods[0];

            FenceSet fenceSet = new FenceSet();

            //  wallSetting.InitWallSetting();
            fenceSet.Show();
            fenceSet.TopMost = true;


            var doc = docRt.Document;

        Step1:
            var step0 = curMethod.Steps[0];
            var result0 = await pointInputer.Execute(step0.Options);
            if (pointInputer.isCancelled) { this.Cancel(); return; }
            if (result0.ValueX != null)
            {
                firstPoint = (Vector2)result0.ValueX;
                goto Step2;
            }

            else if (result0.ValueX == null)
            {
                if (result0.Option == null)
                {
                    goto End;
                }
                if (result0.Option.ToUpper() == "C")
                {

                }
                else
                if (result0.Option.ToUpper() == "A")
                {

                }
            }

        Step2:

            Vector2 secondPoint = new Vector2();
            var step2 = curMethod.Steps[1];
            var result2 = await pointInputer.Execute(step0.Options);
            if (pointInputer.isCancelled) { this.Cancel(); goto End; }
            if (result2.ValueX != null)
            {
                secondPoint = (Vector2)result2.ValueX;
                polyline2D.Curve2ds.Add(new Line2d(firstPoint.Clone(), secondPoint.Clone()));
                firstPoint = secondPoint.Clone();
                goto Step2;
            }
            else if (result2.ValueX == null)
            {
                if (result2.Option == null)
                {
                    goto End;
                }
                if (result2.Option.ToUpper() == "C")
                {

                }
            }
        Step3:
            var step3 = curMethod.Steps[1];
            var result3 = await pointInputer.Execute(step0.Options);
            if (pointInputer.isCancelled) { this.Cancel(); return; }
        End:
            if (polyline2D.Curve2ds.Count() > 0)
            {
                var FenceDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "围墙", null) as QdFenceDef;
                QdFence qdFence = new QdFence(FenceDef);
                qdFence.Initilize(doc);
                qdFence.BaseCurve = polyline2D;
                qdFence.FenceWidth = Convert.ToDouble(FenceSet.FenceWidth);
                qdFence.FenceHeight = Convert.ToDouble(FenceSet.FenceHeight);
                qdFence.FenceColumnHeight = Convert.ToDouble(FenceSet.FenceColumnHeight);
                qdFence.FenceColumnInterval = Convert.ToDouble(FenceSet.FenceColumnInterval);
                qdFence.FenceColor = FenceSet.FenceColor;
                qdFence.FenceColumnColor = FenceSet.FenceColumnColor;
                qdFence.Bottom = 0;
                this.vportRt.ActiveElementSet.InsertElement(qdFence);
            }

            this.EndCreating();
        }
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }

        public Dictionary<string, string> colors = new Dictionary<string, string>
            {

                { "红", "4294901760" },
                { "黄", "4294967040" },
                { "绿", "4278255360" },
                { "青", "4278255615" },
                { "蓝", "4278190335" },
                { "洋红", "4294902015" },
                { "白", "4294967295" },
            };

        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            LcPaint auxPen = GetAuxDrawPen();
            if (element.IsSelected)
                auxPen = Constants.SelectedPen;
            bool firstLine = true;
            List<Line2d> lastColLines = new List<Line2d>();
            Vector2 lastEndP = new Vector2();
            var FenceWidth = (element as QdFence).FenceWidth;
            var FenceColumnInterval = (element as QdFence).FenceColumnInterval;
            Polyline2d polyline2D = ((element as QdFence).BasePolyline).Clone() as Polyline2d;
            polyline2D= polyline2D.ApplyMatrix(matrix);
            foreach (var ele in polyline2D.Curve2ds)
            {
                Vector2 start = new Vector2();
                Vector2 end = new Vector2();
                if (ele.Type == Curve2dType.Line2d)
                {
                    start = (ele as Line2d).Start.Clone();
                    end = (ele as Line2d).End.Clone();

                    if (firstLine)
                    {
                        Vector2 direction = (end - start).Normalize();
                        lastEndP = start + direction * FenceWidth;
                   
                    }
                    else
                    {
                        lastEndP = null;
                        foreach (var item in lastColLines)
                        {
                            lastEndP = Intersect2d.LineWithLine((ele as Line2d), item);
                            if (lastEndP != null)
                            {
                                break;

                            }
                        }
                        if (lastEndP == null)
                        {
                            Vector2 direction = (end - start).Normalize();
                            lastEndP = start + direction * FenceWidth;
                        }
                    }
                    lastColLines = DrawColumn(canvas, auxPen, start, end, FenceWidth, FenceColumnInterval, firstLine, lastEndP);
                    firstLine = false;
                }
            }
        }

        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            this.Draw(canvas, element, Matrix3.GetTranslate(offset));
        }
        public override void DrawTemp(LcCanvas2d canvas)
        {
            if (this.firstPoint == null)
            {
                return;
            }
            LcPaint auxPen = GetAuxDrawPen();
            Vector2 mp = this.pointInputer?.InputP;
            if (polyline2D.Curve2ds.Count() > 0)
            {


                //下一段线绘制的时候 渲染上一段线
                bool firstLine = true;
                List<Line2d> lastColLines = new List<Line2d>();
                Vector2 lastEndP = new Vector2();
                foreach (var ele in this.polyline2D.Curve2ds)
                {
                    Vector2 start = new Vector2();
                    Vector2 end = new Vector2();
                    if (ele.Type == Curve2dType.Line2d)
                    {
                        start = (ele as Line2d).Start;
                        end = (ele as Line2d).End;

                        if (firstLine)
                        {
                            Vector2 direction = (end - start).Normalize();
                            lastEndP = start + direction * Convert.ToDouble(FenceSet.FenceWidth);

                        }
                        else
                        {
                            lastEndP = null;
                            foreach (var item in lastColLines)
                            {
                                lastEndP = Intersect2d.LineWithLine((ele as Line2d), item);
                                if (lastEndP != null)
                                {
                                    break;

                                }
                            }
                            if (lastEndP == null)
                            {
                                Vector2 direction = (end - start).Normalize();
                                lastEndP = start + direction * Convert.ToDouble(FenceSet.FenceWidth);
                            }
                        }
                        lastColLines = DrawColumn(canvas, auxPen, start, end, Convert.ToDouble(FenceSet.FenceWidth), Convert.ToDouble(FenceSet.FenceColumnInterval), firstLine, lastEndP);
                        firstLine = false;
                    }
                }

                lastEndP = null;
                foreach (var item in lastColLines)
                {
                    lastEndP = Intersect2d.LineWithLine(new Line2d(this.firstPoint, mp), item);
                    if (lastEndP != null)
                    {
                        break;

                    }
                }
                if (lastEndP == null)
                {
                    Vector2 direction = (mp - this.firstPoint).Normalize();
                    lastEndP = this.firstPoint + direction * Convert.ToDouble(FenceSet.FenceWidth);
                }
                DrawColumn(canvas, auxPen, this.firstPoint, mp, Convert.ToDouble(FenceSet.FenceWidth), Convert.ToDouble(FenceSet.FenceColumnInterval), false, lastEndP);

            }
            else
            {
                Vector2 direction = (mp - this.firstPoint).Normalize();
                Vector2 lastEndP = this.firstPoint + direction * Convert.ToDouble(FenceSet.FenceWidth);
                DrawColumn(canvas, auxPen, this.firstPoint, mp, Convert.ToDouble(FenceSet.FenceWidth), Convert.ToDouble(FenceSet.FenceColumnInterval), true, lastEndP);
            }
        }
        private QdFence _Fence;
        private string _gripName;
        private Vector2 _position;
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_gripName == null || _Fence == null)
            {
                return;
            }
            if (_gripName.StartsWith("FenceBaseLine_"))
            {
                if (_gripName.Contains("Start"))
                {
                    Vector2 e = (_Fence.BasePolyline.Curve2ds.First() as Line2d).End;
                    Vector2 direction = (e - _position).Normalize();
                    var lastEndP = _position + direction * _Fence.FenceWidth;
                    DrawColumn(canvas, Constants.AuxOrangePaint, _position, e, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);
                    if (Vector2.Distance((_Fence.BasePolyline.Curve2ds.Last() as Line2d).End, (_Fence.BasePolyline.Curve2ds.First() as Line2d).Start) < 0.01)
                    {
                        Vector2 ls = (_Fence.BasePolyline.Curve2ds.Last() as Line2d).Start;

                        direction = (_position - ls).Normalize();
                        lastEndP = ls + direction * _Fence.FenceWidth;
                        DrawColumn(canvas, Constants.AuxOrangePaint, ls, _position, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);
                    }

                }
                else
                    if ((_gripName.Contains("end")))
                {
                    Vector2 s = (_Fence.BasePolyline.Curve2ds.Last() as Line2d).Start;
                    Vector2 direction = (_position - s).Normalize();
                    var lastEndP = s + direction * _Fence.FenceWidth;
                    DrawColumn(canvas, Constants.AuxOrangePaint, s, _position, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);
                    if (Vector2.Distance((_Fence.BasePolyline.Curve2ds.Last() as Line2d).End, (_Fence.BasePolyline.Curve2ds.First() as Line2d).Start) < 0.01)
                    {
                        Vector2 le = (_Fence.BasePolyline.Curve2ds.First() as Line2d).End;

                        direction = (le - _position).Normalize();
                        DrawColumn(canvas, Constants.AuxOrangePaint, _position, le, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);
                    }
                }
                else
                {
                    var idx = int.Parse(_gripName.Split('_')[1]);
                    var end = (_Fence.BasePolyline.Curve2ds[idx] as Line2d).End;
                    Vector2 direction = (end - _position).Normalize();
                    var lastEndP = _position + direction * _Fence.FenceWidth;
                    DrawColumn(canvas, Constants.AuxOrangePaint, _position, end, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);
                    var strat = (_Fence.BasePolyline.Curve2ds[idx - 1] as Line2d).Start;
                    direction = (_position - strat).Normalize();
                    lastEndP = strat + direction * _Fence.FenceWidth;
                    DrawColumn(canvas, Constants.AuxOrangePaint, strat, _position, _Fence.FenceWidth, _Fence.FenceColumnInterval, true, lastEndP);


                }
            }
        }
        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var Fence = element as QdFence;
            var grips = new List<ControlGrip>();
            if (Fence.BasePolyline.Curve2ds.Count == 0)
            {
                return null;
            }
            //var gripCenter = new ControlGrip
            //{
            //    Element = Fence,
            //    Name = "Center",
            //    Position = Fence.
            //};
            //  grips.Add(gripCenter);
            var grip = new ControlGrip
            {
                Element = Fence,
                Name = $"FenceBaseLine_Start",
                Position = (Fence.BasePolyline.Curve2ds.FirstOrDefault() as Line2d).Start,
            };
            grips.Add(grip);
            int i = 0;
            if (Fence.BasePolyline.Curve2ds.Count > 1)
            {
                foreach (var item in Fence.BasePolyline.Curve2ds)
                {
                    if (i > 0)
                    {
                        var grip_ = new ControlGrip
                        {
                            Element = Fence,
                            Name = $"FenceBaseLine_{i}",
                            Position = (item as Line2d).Start
                        };
                        grips.Add(grip_);
                    }
                    i++;

                }
            }
            var endgrip = new ControlGrip
            {
                Element = Fence,
                Name = $"FenceBaseLine_end",
                Position = (Fence.BasePolyline.Curve2ds.Last() as Line2d).End,
            };
            grips.Add(endgrip);

            return grips.ToArray();
        }

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var Fence = element as QdFence;
            _Fence = Fence;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                if (_gripName.StartsWith("FenceBaseLine_"))
                {
                    Polyline2d pl = Fence.BasePolyline.Clone() as Polyline2d;
                    if (_gripName.Contains("Start"))
                    {

                        if (Vector2.Distance((pl.Curve2ds.Last() as Line2d).End, (pl.Curve2ds.First() as Line2d).Start) < 0.01)
                        {
                            (pl.Curve2ds.Last() as Line2d).End = position.Clone();
                        }
                        (pl.Curve2ds[0] as Line2d).Start = position.Clone();


                    }
                    else
                        if ((_gripName.Contains("end")))
                    {

                        if (Vector2.Distance((pl.Curve2ds.Last() as Line2d).End, (pl.Curve2ds.First() as Line2d).Start) < 0.01)
                        {
                            (pl.Curve2ds[0] as Line2d).Start = position.Clone();
                        }
                        (pl.Curve2ds.Last() as Line2d).End = position.Clone();

                    }
                    else
                    {
                        var idx = int.Parse(_gripName.Split('_')[1]);

                        (pl.Curve2ds[idx] as Line2d).Start = position.Clone();
                        (pl.Curve2ds[idx - 1] as Line2d).End = position.Clone();

                    }

                    Fence.BaseCurve = pl;
                }
            }
        }

        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
             new PropertyObserver()
            {
                Name = "FenceWidth",
                DisplayName = "墙宽",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFence).Properties.GetValue<double>("FenceWidth"),
                Setter = (ele, value) =>
                {
                    var fence = (ele as QdFence);
                    if (!double.TryParse(value.ToString(),out var FenceWidth))
                        return;
                    fence.OnPropertyChangedBefore("FenceWidth",fence.Properties.GetValue<double>("FenceWidth"),FenceWidth);
                    fence.Properties.SetValue("FenceWidth",FenceWidth );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceWidth",fence.Properties.GetValue<double>("FenceWidth"),FenceWidth);
                }
            },new PropertyObserver()
            {
                Name = "FenceHeight",
                DisplayName = "墙高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFence).Properties.GetValue<double>("FenceHeight"),
                Setter = (ele, value) =>
                {
                    var fence = (ele as QdFence);
                    if (!double.TryParse(value.ToString(),out var FenceHeight))
                        return;
                    fence.OnPropertyChangedBefore("FenceHeight",fence.Properties.GetValue<double>("FenceHeight"),FenceHeight);
                    fence.Properties.SetValue("FenceHeight",FenceHeight );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceHeight",fence.Properties.GetValue<double>("FenceHeight"),FenceHeight);
                }
            },new PropertyObserver()
            {
                Name = "FenceColumnHeight",
                DisplayName = "柱子高度",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFence).Properties.GetValue<double>("FenceColumnHeight"),
                Setter = (ele, value) =>
                {
                    var fence = (ele as QdFence);
                    if (!double.TryParse(value.ToString(),out var FenceColumnHeight))
                        return;
                    fence.OnPropertyChangedBefore("FenceColumnHeight",fence.Properties.GetValue<double>("FenceColumnHeight"),FenceColumnHeight);
                    fence.Properties.SetValue("FenceColumnHeight",FenceColumnHeight );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceColumnHeight",fence.Properties.GetValue<double>("FenceColumnHeight"),FenceColumnHeight);
                }
            },new PropertyObserver()
            {
                Name = "FenceColumnInterval",
                DisplayName = "柱子间距",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdFence).Properties.GetValue<double>("FenceColumnInterval"),
                Setter = (ele, value) =>
                {
                    var fence = (ele as QdFence);
                    if (!double.TryParse(value.ToString(),out var FenceColumnInterval))
                        return;
                    fence.OnPropertyChangedBefore("FenceColumnInterval",fence.Properties.GetValue<double>("FenceColumnInterval"),FenceColumnInterval);
                    fence.Properties.SetValue("FenceColumnInterval",FenceColumnInterval );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceColumnInterval",fence.Properties.GetValue<double>("FenceColumnInterval"),FenceColumnInterval);
                }
            },
            new PropertyObserver()
                {
                    Name = "FenceColor",
                    DisplayName = "围墙颜色",
                    CategoryName = "Geometry",
                    CategoryDisplayName = "几何图形",
                    PropType = PropertyType.Array,
                    Source = (ele) => colors.Keys.ToArray(),
                    Getter = (ele) =>
                    {
                        var col=(ele as QdFence).Properties.GetValue<string>("FenceColor");
                       return colors.FirstOrDefault(c => c.Value == col).Key;

                    },
                    Setter = (ele, value) => {
                            var fence = (ele as QdFence);
                      //  MessageBox.Show(value.ToString());
                 //    MessageBox.Show(colors.FirstOrDefault(c => c.Key == value).Value.ToString());
                    var FenceColor=colors.FirstOrDefault(c => c.Key == value).Value;

                    fence.OnPropertyChangedBefore("FenceColor",fence.Properties.GetValue<string>("FenceColor"),FenceColor);
                    fence.Properties.SetValue("FenceColor",FenceColor );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceColor",fence.Properties.GetValue<string>("FenceColor"),FenceColor);
                    }
                },  new PropertyObserver()
                {
                    Name = "FenceColumnColor",
                    DisplayName = "柱子颜色",
                    CategoryName = "Geometry",
                    CategoryDisplayName = "几何图形",
                    PropType = PropertyType.Array,
                    Source = (ele) => colors.Keys.ToArray(),
                    Getter = (ele) =>
                    {
                        var col=(ele as QdFence).Properties.GetValue<string>("FenceColumnColor");
                       return colors.FirstOrDefault(c => c.Value == col).Key;

                    },
                    Setter = (ele, value) => {
                            var fence = (ele as QdFence);
                      //  MessageBox.Show(value.ToString());
                 //    MessageBox.Show(colors.FirstOrDefault(c => c.Key == value).Value.ToString());
                    var FenceColumnColor=colors.FirstOrDefault(c => c.Key == value).Value;
                    fence.OnPropertyChangedBefore("FenceColumnColor",fence.Properties.GetValue<string>("FenceColumnColor"),FenceColumnColor);
                    fence.Properties.SetValue("FenceColumnColor",FenceColumnColor );
                    fence.ResetCache();
                    fence.OnPropertyChangedAfter("FenceColumnColor",fence.Properties.GetValue<string>("FenceColumnColor"),FenceColumnColor);
                    }
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="auxPen"></param>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <param name="Interval"></param>
        /// <param name="startDraw"></param>
        /// <param name="lastEndp"></param>
        /// <returns>最后一个柱子的竖线</returns>
        public List<Line2d> DrawColumn(LcCanvas2d canvas, LcPaint auxPen, Vector2 sp, Vector2 ep, double FenceWidth, double Interval, bool startDraw, Vector2 lastEndp)
        {

            var Angle = Vector2.GetAngle(sp, ep);
            Matrix3 matrix3 = Matrix3.RotateInRadian(Angle, sp);
            // Vector2 UpStart = matrix3.MultiplyPoint(new Vector2(sp.X,sp.Y +Convert.ToDouble(FenceWidth) / 2));
            //  Vector2 DownStart = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y - Convert.ToDouble(FenceWidth) / 2));

            // Line2d upline = new Line2d(sp, ep).Move(sp, UpStart) as Line2d;
            // Line2d downline = new Line2d(sp, ep).Move(sp, DownStart) as Line2d;

            // canvas.DrawLine(auxPen, sp, ep);
            //canvas.DrawLine(auxPen, upline.Start, upline.End);
            //canvas.DrawLine(auxPen, downline.Start, downline.End);
            //canvas.DrawLine(auxPen, downline.Start, upline.Start);
            //canvas.DrawLine(auxPen, downline.End, upline.End);

            //第一个柱子
            Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X - Convert.ToDouble(FenceWidth), sp.Y - Convert.ToDouble(FenceWidth)));
            Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + Convert.ToDouble(FenceWidth), sp.Y - Convert.ToDouble(FenceWidth)));
            Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + Convert.ToDouble(FenceWidth), sp.Y + Convert.ToDouble(FenceWidth)));
            Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X - Convert.ToDouble(FenceWidth), sp.Y + Convert.ToDouble(FenceWidth)));
            Vector2 direction = (ep - sp).Normalize();
            Vector2 NextstartP = lastEndp.Clone();
            if (startDraw)
            {
                canvas.DrawLine(auxPen, p1.Clone(), p2.Clone());
                canvas.DrawLine(auxPen, p2.Clone(), p3.Clone());
                canvas.DrawLine(auxPen, p3.Clone(), p4.Clone());
                canvas.DrawLine(auxPen, p4.Clone(), p1.Clone());
            }
            //------[]------[*3
            while (((ep - NextstartP).Length() >= (2 * Interval + Convert.ToDouble(FenceWidth) * 3)))
            {
                Vector2 NextColCenter = NextstartP + direction * Interval;
                Vector2 NextendP = NextColCenter + direction * Convert.ToDouble(FenceWidth);
                canvas.DrawLine(auxPen, NextstartP, NextendP);

                Vector2 NextColEnd = NextendP + direction * Convert.ToDouble(FenceWidth) * 2;

                p1 = p1 + (NextColEnd - NextstartP);
                p2 = p2 + (NextColEnd - NextstartP);
                p3 = p3 + (NextColEnd - NextstartP);
                p4 = p4 + (NextColEnd - NextstartP);
                canvas.DrawLine(auxPen, p1.Clone(), p2.Clone());
                canvas.DrawLine(auxPen, p2.Clone(), p3.Clone());
                canvas.DrawLine(auxPen, p3.Clone(), p4.Clone());
                canvas.DrawLine(auxPen, p4.Clone(), p1.Clone());
                NextstartP = NextColEnd.Clone();
            }
            if ((ep - NextstartP).Length() > Interval + Convert.ToDouble(FenceWidth))
            {
                var centerLenth = ((ep - NextstartP).Length() - Convert.ToDouble(FenceWidth)) / 2;
                Vector2 NextColCenter = NextstartP + direction * centerLenth;
                Vector2 NextendP = NextColCenter + direction * Convert.ToDouble(FenceWidth);
                canvas.DrawLine(auxPen, NextstartP, NextendP);

                Vector2 NextColEnd = NextendP + direction * Convert.ToDouble(FenceWidth) * 2;

                p1 = p1 + (NextColEnd - NextstartP);
                p2 = p2 + (NextColEnd - NextstartP);
                p3 = p3 + (NextColEnd - NextstartP);
                p4 = p4 + (NextColEnd - NextstartP);
                canvas.DrawLine(auxPen, p1.Clone(), p2.Clone());
                canvas.DrawLine(auxPen, p2.Clone(), p3.Clone());
                canvas.DrawLine(auxPen, p3.Clone(), p4.Clone());
                canvas.DrawLine(auxPen, p4.Clone(), p1.Clone());
                NextstartP = NextColEnd.Clone();
            }

            Vector2 endP = ep - direction * Convert.ToDouble(FenceWidth);
            canvas.DrawLine(auxPen, NextstartP, endP);
            p1 = p1 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p2 = p2 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p3 = p3 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p4 = p4 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            canvas.DrawLine(auxPen, p1.Clone(), p2.Clone());
            canvas.DrawLine(auxPen, p2.Clone(), p3.Clone());
            canvas.DrawLine(auxPen, p3.Clone(), p4.Clone());
            canvas.DrawLine(auxPen, p4.Clone(), p1.Clone());
            List<Line2d> line2Ds = new List<Line2d>();
            line2Ds.Add(new Line2d(p1, p2));
            line2Ds.Add(new Line2d(p2, p3));
            line2Ds.Add(new Line2d(p3, p4));
            line2Ds.Add(new Line2d(p4, p1));
            return line2Ds;





            //this.extInputp = sp + direction * Interval;
            //Vector2 p1 =
            //   for ()

        }
        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            return null;
        }
  
    }
}