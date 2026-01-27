using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdSite : DirectComponent, IEmbed
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
                //Properties.SetValue("Bottom", value);
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
                return this.BaseCurve as Polyline2d;
            }
            set
            {
                BaseCurve = value;
            }
        }
        public const int PN_EmbedAssociation = 100111;
        private LcList<EmbedAssociation> embedAssociations;
        public LcList<EmbedAssociation> EmbedAssociations { get => embedAssociations; set=> SetProps((PN_EmbedAssociation,value)); }

        public bool CanChangeHost => false;

        protected override object GetProp(int propId)
        {
            if (PN_EmbedAssociation == propId)
            {
                return this.embedAssociations;
            }
            return base.GetProp(propId);
        }

        protected override bool SetProp(int propId, object propVal)
        {
            if (!base.SetProp(propId, propVal))
            {
                if (propId == PN_EmbedAssociation)
                {
                    var oldVal = embedAssociations;
                    this.OnPropertyChangedBefore(nameof(EmbedAssociations), oldVal, propVal);
                    this.embedAssociations = (LcList<EmbedAssociation>)propVal;
                    this.embedAssociations?.Set(this.GetDocument, nameof(EmbedAssociations), this);
                    this.ResetCache();
                    this.OnPropertyChangedAfter(nameof(EmbedAssociations), oldVal, propVal);
                }
            }
            return true;

        }
        public override void OnRemoveAfter(ILcCollection parent)
        {
            for (var i= EmbedAssociations.Count-1;i>=0;i--)
            {
                EmbedAssociations[i].UnAssociat();
            }
            base.OnRemoveAfter(parent);
        }
        public override void OnInsertAfter(ILcCollection parent)
        {
            CheckInSite(this,this.Parent as IElementSet);
            base.OnInsertAfter(parent);
            (this.Parent as LcObject).ObjectChangedAfter += QdSite_ObjectChangedAfter;
            (this.Parent as LcObject).PropertyChangedAfter += QdSite_PropertyChangedAfter;
        }

        private void QdSite_PropertyChangedAfter(object? sender, PropertyChangedEventArgs e)
        {
            if (e.Object is IComponentInstance component&&component.Definition.Features=="场地布置") 
            {
                CheckInSite(this, [e.Object as LcElement]);
            }
        }

        public override void OnRemoveBefore(ILcCollection parent)
        {
            (this.Parent as LcObject).ObjectChangedAfter -= QdSite_ObjectChangedAfter;
            (this.Parent as LcObject).PropertyChangedAfter -= QdSite_PropertyChangedAfter;
            base.OnRemoveBefore(parent);
        }

        private void QdSite_ObjectChangedAfter(object? sender, ObjectChangedEventArgs e)
        {
            if (e.Type == ObjectChangeType.Insert)
            {
                var ele = e.Target as LcElement;
                CheckInSite(this, [ele]);
            }else if (e.Type==ObjectChangeType.Remove)
            {
                var ele = e.Target as LcElement;
                CheckInSite(this, [ele],false);
            }
        }
        private void CheckInSite(QdSite site, IElementSet elementSet, bool isAdd = true)
        {
            CheckInSite(site,elementSet.Elements, isAdd);
        }
        private void CheckInSite(QdSite site, ICollection<LcElement> elements,bool isAdd=true)
        {
            if (isAdd)
            {
                foreach (var ele in elements)
                {
                    if (ele is IComponentInstance component && component.Definition.Features == "场地布置")
                        if (site.BoundingBox.ContainsBox(ele.BoundingBox) || site.BoundingBox.IntersectsBox(ele.BoundingBox))
                        {
                            AddLayout(site, ele);
                        }
                }
            }
            else
            {
                foreach (var ele in elements)
                {
                    if (site.EmbedAssociations.Any(n => n.Embed == ele))
                    { 
                        var oldAsc = site.EmbedAssociations.First(n => n.Embed == ele);
                        oldAsc.UnAssociat(); 
                    }
                }
            }
        }
        public QdSite(QdSiteDef siteDef) : base(siteDef)
        {
            Type = LayoutElementType.Site;
            Outline = new Polyline2d();
            EmbedAssociations = new LcList<EmbedAssociation>();
        }
        public void CheckHasFoundationPit()
        {
            this.OnPropertyChangedBefore("association", null, null);
            var fdps = (this.Parent as IElementSet).Elements.Where(n => n is IComponentInstance component && component.Definition.Features=="场地布置");
            foreach (LcElement fdp in fdps)
            {
                if (this.BoundingBox.ContainsBox(fdp.BoundingBox) || this.BoundingBox.IntersectsBox(fdp.BoundingBox))
                {
                    AddLayout(this,fdp);
                }
            }
            this.OnPropertyChangedAfter("association",null,null);
        }
        public void AddLayout(QdSite site, LcElement fdp)
        {
            if (site.EmbedAssociations.Any(n => n.Embed == fdp))
            {
                var oldAsc = site.EmbedAssociations.First(n => n.Embed == fdp);
                oldAsc.UnAssociat();
            }
            EmbedAssociation association = new EmbedAssociation();
            association.EmbedTag = (fdp as LcElement).Id; //将原始的LcElement.Id存下来，方便Remove时来匹配
            association.EmbedName = nameof(fdp);
            association.Embed = fdp as IEmbed;
            association.Host = site;
            association.HostTag = site.Id;
            association.HostName = nameof(QdSite);
            association.Initilize(site.Document);
            association.Associat();
        }
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(Outline.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        }

        //public override void Move(Vector2 startPoint, Vector2 endPoint)
        //{
        //    Translate(endPoint - startPoint);
        //}

        //public override void Translate(Vector2 vec)
        //{
        //    Outline = Outline.Clone().Translate(vec) as Polyline2d;
        //}

        //public override void Translate(double dx, double dy)
        //{
        //    Translate(new Vector2(dx, dy));
        //}
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdSite(Definition as QdSiteDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var site = (QdSite)src;
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
