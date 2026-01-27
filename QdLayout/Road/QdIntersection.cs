using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdIntersection : LcComponentInstance,IEmbed    
    {
        public double Bottom
        {
            get
            {
                return Properties.GetValue<double>("Bottom");
            }
            set
            {
                Properties.SetValue("Bottom", value);
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
                Properties.SetValue("Material", value);
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
                Properties.SetValue("Outline", value);
            }
        }
    
        private Vector2 dragPoint;
        public Vector2 DragPoint
        {
            get
            {
                if (dragPoint == null)
                {
                    dragPoint = GetBoundingBox().Center;
                }
                return dragPoint;
            }
            set
            {
                dragPoint = value;
            }
        }

        public LcList<EmbedAssociation> EmbedAssociations { get; } = new LcList<EmbedAssociation>();

        public bool CanChangeHost => false;

        public override void OnRemoveAfter(ILcCollection parent)
        { 
            this.EmbedAssociations.ForEach(n => n.UnAssociat());
            base.OnRemoveAfter(parent);
        }
        public QdIntersection(QdIntersectionDef siteDef) : base(siteDef)
        {
           // Type = LayoutElementType.Intersection;
            Outline = new Polyline2d();
        }
        public void CheckHasFoundationPit()
        {
            this.OnPropertyChangedBefore("association", null, null);
            var fdps = (this.Parent as IElementSet).Elements.Where(n => n is QdFoundationPit);
            foreach (QdFoundationPit fdp in fdps)
            {
                if (this.BoundingBox.ContainsBox(fdp.BoundingBox) || this.BoundingBox.IntersectsBox(fdp.BoundingBox))
                {
                    AddFoundationPit(fdp);
                }
            }
            this.OnPropertyChangedAfter("association",null,null);
        }
        public void AddFoundationPit(QdFoundationPit fdp)
        {
            if (this.EmbedAssociations.Any(n => n.Embed == fdp))
            {
                var oldAsc = this.EmbedAssociations.First(n => n.Embed == fdp);
                oldAsc.UnAssociat();
            }
            EmbedAssociation association = new EmbedAssociation();
            association.EmbedTag = (fdp as LcElement).Id; //将原始的LcElement.Id存下来，方便Remove时来匹配
            association.EmbedName = "QdFoundationPit";
            association.Embed = fdp as IEmbed;
            association.Host = this;
            association.HostTag = this.Id;
            association.HostName = nameof(QdIntersection);
            association.Initilize(this.Document);
            association.Associat();
        }
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(Outline.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        }

        public override void Move(Vector2 startPoint, Vector2 endPoint)
        {
            OnPropertyChangedBefore(nameof(DragPoint), DragPoint, DragPoint);
            Translate(endPoint - startPoint);
            ResetCache();
            OnPropertyChangedAfter(nameof(DragPoint), DragPoint, DragPoint);
        }

        public override void Translate(Vector2 vec)
        {
            DragPoint += vec;
            Outline.Translate(vec);
        }

        public override void Translate(double dx, double dy)
        {
            Translate(new Vector2(dx, dy));
        }
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdIntersection(Definition as QdIntersectionDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var site = (QdIntersection)src;
            Outline = site.Outline.Clone() as Polyline2d;
            Material = site.Material.Clone();
        }


        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            foreach (var curve in Outline.Curve2ds)
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
            foreach (var curve in Outline.Curve2ds)
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

        public Profile2[] GetEmbedHoles()
        {
            var profiles = new Profile2[] { new Profile2() { OutLoop = new Polyline2d() { Curve2ds = this.GetShapes()[0].Curve2ds.Clone() } } };
            return profiles;
        }
    }
}
