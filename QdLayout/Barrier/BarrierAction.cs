using LightCAD.Core.Elements;
using QdLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;

namespace QdLayout
{
    internal class BarrierAction : DirectComponentAction
    {
        private static readonly LcCreateMethod[] CreateMethods;
        private PointInputer pointInputer;
        private CmdTextInputer cmdTextInputer;
        private LcPolyLine OutLoop;
        private Polyline2d polyline2D = new Polyline2d();
        private Vector2 firstPoint = null;
        public BarrierAction() { }

        public BarrierAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：Barrier");
        }
        static BarrierAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateLawn",
                Description = "创建防护栏杆",
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
                var barrierDef = docRt.GetUseComDef($"{NamespaceKey}.安全防护", "防护栏杆", null) as QdBarrierDef;
                QdBarrier qdBarrier = new QdBarrier(barrierDef);
                qdBarrier.Initilize(doc);
                qdBarrier.BasePolyline = polyline2D;
                qdBarrier.BarrierWidth = 2500;// 
                qdBarrier.BarrierHeight = 1200;//
                this.vportRt.ActiveElementSet.InsertElement(qdBarrier);
            }

            this.EndCreating();
        }
        public override void Cancel()
        {
            base.Cancel();
            vportRt.SetCreateDrawer(null);
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            LcPaint auxPen = GetAuxDrawPen();
            var curves = (element as QdBarrier).BasePolyline.Curve2ds;
            if (curves.Count() > 0)
            {
                foreach (var ele in curves)
                {
                    Vector2 start = new Vector2();
                    Vector2 end = new Vector2();

                    if (ele.Type == Curve2dType.Line2d)
                    {
                        start = (ele as Line2d).Start;
                        end = (ele as Line2d).End;
                        DrawBarrier(canvas,auxPen, start, end);
                    }
                }
            }
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
                foreach (var ele in this.polyline2D.Curve2ds)
                {
                    Vector2 start = new Vector2();
                    Vector2 end = new Vector2();
                    if (ele.Type == Curve2dType.Line2d)
                    {
                        start = (ele as Line2d).Start;
                        end = (ele as Line2d).End;

                        DrawBarrier(canvas, auxPen, start, end);
                        DrawAuxArrow(canvas, auxPen, start, end);
                    }
                }

                DrawBarrier(canvas, auxPen, (this.polyline2D.Curve2ds.Last() as Line2d).End, mp);
                DrawAuxArrow(canvas, auxPen, (this.polyline2D.Curve2ds.Last() as Line2d).End, mp);
            }
            else
            {
                if(this.firstPoint is not null)
                {
                    DrawBarrier(canvas,auxPen, this.firstPoint, mp);
                    DrawAuxArrow(canvas, auxPen, this.firstPoint, mp);
                }
            }
        }

        private void DrawBarrier(LcCanvas2d canvas, LcPaint pen, Vector2 sp, Vector2 ep, bool inverse = false)
        {
            canvas.DrawLine(pen, sp, ep);
            DrawBarrierText(canvas,"移动式围栏1",sp,ep, inverse);
            DrawBarrierArrow(canvas,pen,sp,ep, inverse);
        }

        private void DrawBarrierText(LcCanvas2d canvas,string str ,Vector2 sp, Vector2 ep, bool inverse = false)
        {
            double angle = Vector2.GetAngle(sp, ep);
            LcText text = new LcText();
            text.Location = (sp+ep) / 2;
            text.Height = 35;
            text.TextString = str;
            text.TextStyleName = "Standard";
            text.Rotation = angle * 180 / Math.PI;
            text.WidthFactor = 1;
            text.Oblique = 0;
            text.Initilize(this.docRt.Document);
            var paint = TextAction.GetPaint(text);
            canvas.DrawText(paint, text.TextString, Matrix3.Identity, out var charBoxs);
        }

        private void DrawBarrierArrow(LcCanvas2d canvas, LcPaint pen, Vector2 sp, Vector2 ep, bool inverse = false)
        {
            //
        }

        private void DrawAuxArrow(LcCanvas2d canvas, LcPaint pen, Vector2 sp, Vector2 ep, bool inverse = false)
        {
            // 
            Vector2 midp = (sp + ep) / 2;
            Vector2 direction = (ep - midp);
            Vector2 vert = direction.RotateAround(new Vector2(), -Math.PI / 2).Normalize();
            Vector2 startp = midp + vert * 100;
            Vector2 endp = midp + vert * 500;

            canvas.DrawLine(pen, startp, endp);
        }


        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>() {
             new PropertyObserver()
            {
                Name = "BarrierWidth",
                DisplayName = "墙宽",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBarrier).Properties.GetValue<double>("BarrierWidth"),
                Setter = (ele, value) =>
                {
                    var barrier = (ele as QdBarrier);
                    if (!double.TryParse(value.ToString(),out var BarrierWidth))
                        return;
                    barrier.OnPropertyChangedBefore("BarrierWidth",barrier.Properties.GetValue<double>("BarrierWidth"),BarrierWidth);
                    barrier.Properties.SetValue("BarrierWidth",BarrierWidth );
                    barrier.ResetCache();
                    barrier.OnPropertyChangedAfter("BarrierWidth",barrier.Properties.GetValue<double>("BarrierWidth"),BarrierWidth);
                }
            },new PropertyObserver()
            {
                Name = "BarrierHeight",
                DisplayName = "墙高",
                CategoryName = "Geometry",
                CategoryDisplayName = "几何图形",
                PropType=PropertyType.Double,
                Getter = (ele) => (ele as QdBarrier).Properties.GetValue<double>("BarrierHeight"),
                Setter = (ele, value) =>
                {
                    var barrier = (ele as QdBarrier);
                    if (!double.TryParse(value.ToString(),out var BarrierHeight))
                        return;
                    barrier.OnPropertyChangedBefore("BarrierHeight",barrier.Properties.GetValue<double>("BarrierHeight"),BarrierHeight);
                    barrier.Properties.SetValue("BarrierHeight",BarrierHeight );
                    barrier.ResetCache();
                    barrier.OnPropertyChangedAfter("BarrierHeight",barrier.Properties.GetValue<double>("BarrierHeight"),BarrierHeight);
                }
            }
            };
        }

        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var barrier = element as QdBarrier;
            var grips = new List<ControlGrip>();
            if (barrier.BasePolyline.Curve2ds.Count == 0)
            {
                return null;
            }

            var grip = new ControlGrip
            {
                Element = barrier,
                Name = $"FenceBaseLine_Start",
                Position = (barrier.BasePolyline.Curve2ds.FirstOrDefault() as Line2d).Start,
            };
            grips.Add(grip);
            int i = 0;
            if (barrier.BasePolyline.Curve2ds.Count > 1)
            {
                foreach (var item in barrier.BasePolyline.Curve2ds)
                {
                    if (i > 0)
                    {
                        var grip_ = new ControlGrip
                        {
                            Element = barrier,
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
                Element = barrier,
                Name = $"FenceBaseLine_end",
                Position = (barrier.BasePolyline.Curve2ds.Last() as Line2d).End,
            };
            grips.Add(endgrip);

            return grips.ToArray();
        }

        public override SnapPointResult SnapPoint(SnapRuntime snapRt, LcElement element, Vector2 point, double maxDistance, bool IsReturn, Matrix3 matrix3)
        {
            return null;
        }
    }
}
