using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    internal class QdBarrier : DirectComponent 
    {
        public Polyline2d BasePolyline
        {
            get
            {

                return (Polyline2d)this.BaseCurve;
            }
            set
            {
                this.BaseCurve = value;
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
                this.Properties.SetValue("Material", value);
            }
        }

        public double BarrierWidth
        {
            get
            {
                return Convert.ToDouble(this.Properties["BarrierWidth"]);
            }
            set
            {
                this.Properties["BarrierWidth"] = value;
            }
        }
        public double BarrierHeight
        {
            get
            {
                return Convert.ToDouble(this.Properties["BarrierHeight"]);
            }
            set
            {
                this.Properties["BarrierHeight"] = value;
            }
        }

        public QdBarrier(QdBarrierDef qdBarrierDef) : base(qdBarrierDef)
        {
            this.Type = LayoutElementType.Barrier;
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

        public void set(int index, Vector2 newPoint)
        {
            this.Points[index] = newPoint;
        }

        public override LcElement Clone()
        {
            var clone = new QdFence(this.Definition as QdFenceDef);
            clone.Copy(this);
            //clone.Initilize(this.Document);
            return clone;
        }

        public static bool CheckParallel(double angle)
        {
            var i = Math.Abs(angle) % Math.PI;
            return Math.Round(i, 3) == 0 || Math.Round(i, 3) == Math.Round(Math.PI, 3);
        }

 

        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(this.BaseCurve.GetPoints());
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
