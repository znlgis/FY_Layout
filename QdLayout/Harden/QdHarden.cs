using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdHarden : LcComponentInstance
    {
        public double Bottom
        {
            get
            {
                return Properties.GetValue<double>("Bottom");
            }
            set
            {
                SetProps((GetPropId(nameof(Bottom)), value));
            }
        }
        public double Thickness
        {
            get
            {
                return Properties.GetValue<double>("Thickness");
            }
            set
            {
                SetProps((GetPropId(nameof(Thickness)), value));
            }
        }
        public MaterialInfo Material
        {
            get
            {
                return Properties.GetValue<MaterialInfo>("Material");
            }
            set
            {
                SetProps((GetPropId(nameof(Material)), value));

            }
        }
        public Polyline2d Outline
        {
            get
            {
                return Properties.GetValue<Polyline2d>("Outline");
            }
            set
            {
                SetProps((GetPropId(nameof(Outline)), value));
            }
        }


        public QdHarden(QdHardenDef lawnDef) : base(lawnDef)
        {
            Type = LayoutElementType.Harden;
            Outline = new Polyline2d();
        }

  
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(GetShapes()[0].Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        }

     
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdHarden(Definition as QdHardenDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var lawn = (QdHarden)src;
            Outline = lawn.Outline.Clone() as Polyline2d;
            Material = lawn.Material.Clone();
        }


        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            foreach (var curve in GetShapes()[0].Curve2ds)
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
            foreach (var curve in GetShapes()[0].Curve2ds)
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
