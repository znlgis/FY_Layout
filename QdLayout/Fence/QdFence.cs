using LightCAD.MathLib.Hatch;
using System.Drawing;
using System.Xml.Linq;
using ThreeJs4Net;
using static LightCAD.Drawing.Actions.HatchAction;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public enum FenceColumnStyle
    {
        None = 0,
        Rectangle,
        Cricle,

    }
    public class QdFence : DirectComponent, IComponentEdit
    {
        public Polyline2d BasePolyline
        {

            get
            {
                return (Polyline2d)this.BaseCurve;
            }
        }

        public override Curve2dGroupCollection GetShapes()
        {
            if (shapes == null)
            {
                shapes = new Curve2dGroupCollection() { new Curve2dGroup() { Curve2ds = new ListEx<Curve2d>() } };
            }
            return shapes;
        }

        public MaterialInfo Material
        {
            get
            {
                return this.Properties.GetValue<MaterialInfo>("Material");
            }
            set
            {
                SetProps((GetPropId(nameof(Material)), value));
            }
        }

        public double FenceColumnInterval
        {
            get
            {
                return Convert.ToDouble(this.Properties["FenceColumnInterval"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceColumnInterval)), value));
                //this.Properties["FenceColumnInterval"] = value;
            }
        }
        public double FenceWidth
        {
            get
            {
                return Convert.ToDouble(this.Properties["FenceWidth"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceWidth)), value));
                //this.Properties["FenceWidth"] = value;
            }
        }
        public double FenceHeight
        {
            get
            {
                return Convert.ToDouble(this.Properties["FenceHeight"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceHeight)), value));
                //this.Properties["FenceHeight"] = value;
            }
        }
        public double FenceColumnHeight
        {
            get
            {
                return Convert.ToDouble(this.Properties["FenceColumnHeight"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceColumnHeight)), value));
                //this.Properties["FenceColumnHeight"] = value;
            }
        }
        public string FenceColumnColor
        {
            get
            {
                return Convert.ToString(this.Properties["FenceColumnColor"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceColumnColor)), value));
                //this.Properties["FenceColumnColor"] = value;
            }
        }
        public string FenceColor
        {
            get
            {
                return Convert.ToString(this.Properties["FenceColor"]);
            }
            set
            {
                SetProps((GetPropId(nameof(FenceColor)), value));
                //this.Properties["FenceColor"] = value;
            }
        }
        public double Bottom
        {
            get
            {
                return Convert.ToDouble(this.Properties["Bottom"]);
            }
            set
            {
                SetProps((GetPropId(nameof(Bottom)), value));
                //this.Properties["Bottom"] = value;
            }
        }
        public string HatchType
        {
            get
            {
                return this.Properties.GetValue<string>("HatchType");
            }
            set
            {
                SetProps((GetPropId(nameof(HatchType)), value));
                //this.Properties["HatchType"] = value;
            }
        }
        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var fence = (QdFence)src;
            fence.BaseCurve = this.BaseCurve.Clone() as Polyline2d;
            fence.Material = this.Material;
            fence.FenceColumnInterval = this.FenceColumnInterval;
            fence.FenceWidth = this.FenceWidth;
            fence.FenceColor = this.FenceColor;
            fence.FenceHeight = this.FenceHeight;
            fence.FenceColumnHeight = this.FenceColumnHeight;
            fence.FenceColumnColor = this.FenceColumnColor;
            fence.Bottom = this.Bottom;
            fence.HatchType = this.HatchType;

        }
        public override void Translate(double dx, double dy)
        {
            Polyline2d curve2D = this.BasePolyline.Clone() as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Translate(dx, dy);
            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            ResetBoundingBox();
        }
        //public long Hatch
        //{
        //    get
        //    {
        //        return this.Properties.GetValue<long>("Hatch");
        //    }
        //    set
        //    {
        //        this.Properties["Hatch"] = value;
        //    }
        //}
 
        public QdFence(QdFenceDef qdFenceDef) : base(qdFenceDef)
        {
            this.Type = LayoutElementType.Fence;
        }
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(this.BaseCurve.GetPoints());
        }

        public override void OnTransformAfter()
        {
            this.ResetCache();
        }
        public static List<Line2d> GetShapeCurves(Line2d line, double width, double centerOffset = 0)
        {



            List<Line2d> line2Ds = new List<Line2d>();

            return line2Ds;
        }

        public override LcElement Clone()
        {
            var clone = new QdFence(this.Definition as QdFenceDef);
            clone.Copy(this);
            //clone.Initilize(this.Document);
            return clone;

        }


        public Profile2[] GetEmbedHoles()
        {
            throw new NotImplementedException();
        }

        public LcElement Stretch(Box2 box, Vector2 vector)
        {
            throw new NotImplementedException();
        }


        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            foreach (var curve in this.BasePolyline.Curve2ds)
            {
                if (curve is Line2d line)
                {
                    if (Intersect2d.IsPolygonWithLine(testPoly.Points, line.Start, line.End))
                    {
                        return true;
                    }
                }
                else if (curve is Arc2d arc)
                {
                    if (Intersect2d.IsPolygonWithArc(testPoly.Points, arc))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public override void Move(Vector2 startPoint, Vector2 endPoint)
        {
            Polyline2d curve2D = this.BasePolyline.Clone() as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.BaseCurve = curve2D.Translate(endPoint.X- startPoint.X, endPoint.Y - startPoint.Y);
 
            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            ResetBoundingBox();
            
        }
        public override bool IncludedByBox(Polygon2d testPoly, List<RefChildElement> includedChildren = null)
        {
            var thisBox = BoundingBox;
            if (!testPoly.BoundingBox.ContainsBox(thisBox))
                return false;
            if (testPoly.BoundingBox.ContainsBox(thisBox))
                return true;
            foreach (var curve in this.BasePolyline.Curve2ds)
            {
                if (curve is Line2d line)
                {
                    if (Intersect2d.IsPolygonWithLine(testPoly.Points, line.Start, line.End))
                    {
                        return true;
                    }
                }
                else if (curve is Arc2d arc)
                {
                    if (Intersect2d.IsPolygonWithArc(testPoly.Points, arc))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}