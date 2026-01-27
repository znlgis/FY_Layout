using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThreeJs4Net;

namespace QdLayout
{
    public class QdRoad : DirectComponent, IEmbed
    {
        public const int PN_MdiLines = 100111;

        public double Width
        {
            get
            {
                return Properties.GetValue<double>("Width");
            }
            set
            {
                SetProps((GetPropId(nameof(Width)), value));
            }
        }
        public double Radius
        {
            get
            {
                return Properties.GetValue<double>("Radius");
            }
            set
            {
                SetProps((GetPropId(nameof(Radius)), value));
            }
        }

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
        /// <summary>
        /// 拐角半径
        /// </summary>
        public double CornerRadius { get
            {
                return  this.Radius <= 0 ? 5000 : this.Radius;
            }
        }
        //交叉口轮廓
        //public Dictionary<QdRoad, List<Curve2d>> Intersections { get; set; } = new Dictionary<QdRoad, List<Curve2d>>();
        private LcList<Curve2d> mdiLines = new LcList<Curve2d>();
        //去掉路口后中心线
        public LcList<Curve2d> MdiLines {
            get 
            {
                return this.mdiLines;
            }
            set 
            {

                SetProps((PN_MdiLines, value));
            }
        }
        public const int PN_EmbedAssociation = 100111;
        private LcList<EmbedAssociation> embedAssociations = new LcList<EmbedAssociation>();
        public LcList<EmbedAssociation> EmbedAssociations { get => embedAssociations; set => SetProps((PN_EmbedAssociation, value)); }

        public bool CanChangeHost => false;

        protected override object GetProp(int propId)
        {
            if (PN_MdiLines==propId)
            {
                return this.mdiLines;
            }else
            if (PN_EmbedAssociation == propId)
            {
                return this.embedAssociations;
            }
            return base.GetProp(propId);
        }

        protected override bool SetProp(int propId, object propVal)
        {
            if(!base.SetProp(propId,propVal)&&propId==PN_MdiLines)
            {
                var oldVal = mdiLines;
                this.OnPropertyChangedBefore(nameof(MdiLines), oldVal, propVal);
                this.mdiLines = (LcList<Curve2d>)propVal; 
                this.mdiLines?.Set(this.GetDocument, nameof(MdiLines), this);
                this.ResetCache();
                this.OnPropertyChangedAfter(nameof(MdiLines), oldVal, propVal);
            }
            else if(propId == PN_EmbedAssociation)
                {
                var oldVal = embedAssociations;
                this.OnPropertyChangedBefore(nameof(EmbedAssociations), oldVal, propVal);
                this.embedAssociations = (LcList<EmbedAssociation>)propVal;
                this.embedAssociations?.Set(this.GetDocument, nameof(EmbedAssociations), this);
                this.ResetCache();
                this.OnPropertyChangedAfter(nameof(EmbedAssociations), oldVal, propVal);
            }
            return true;

        }

        public QdRoad(QdRoadDef roadDef) : base(roadDef)
        {
            Type = LayoutElementType.Road;
        }
        public override LcElement Clone()
        {
            var clone = new QdSite(Definition as QdSiteDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }
        public override Box2 GetBoundingBox()
        {
            var shape = CreateShape(this.BaseCurve as Line2d,this.Width);
            return new Box2().ExpandByPoints(shape.SelectMany(n=>n.GetPoints()).ToArray());
        }
        public override Curve2dGroupCollection GetShapes()
        {
            if (shapes == null)
            {
                shapes = new Curve2dGroupCollection() { new Curve2dGroup() { Curve2ds = new ListEx<Curve2d>() } };
            }
            return shapes;
        }
        public void CreateShapeCurves()
        {
            this.GetShapes().Clear();
            if (this.MdiLines == null||this.MdiLines.Count==0)
            {
                this.mdiLines = new LcList<Curve2d>() { this.BaseCurve.Clone() };
            }
            
            Curve2dGroup crGrp = null;
            if (this.GetShapes().Count == 0)
            {
                this.GetShapes().Add(new Curve2dGroup { Curve2ds = new ListEx<Curve2d>() });
            }
            crGrp = this.GetShapes()[0];
            foreach (var curve in MdiLines)
            {
                crGrp.Curve2ds.AddRange(CreateShape(curve,this.Width));
            }
        }
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public Dictionary<string, List<QdRoad>> GetConnectRoads()
        {
            var roads = new Dictionary<string, List<QdRoad>>();
            roads.Add("S",new List<QdRoad>()); 
            roads.Add("E", new List<QdRoad>());
            roads.Add("C", new List<QdRoad>());

            if (this.Parent != null)
            {
                var elementSet = this.Parent as IElementSet;
                var eles = elementSet.Elements.Where(n => n.Id != this.Id && n.Type == LayoutElementType.Road);
                foreach (QdRoad road in eles)
                {
                    var ops = road.BaseCurve.GetPoints(1);
                    var ps = this.BaseCurve.GetPoints(1); 
                    if (ops[0].DistanceToSquared(ps[0]) <= 1)
                    {
                        //association.EmbedName = "ConnectRoad-SS";
                        //association.Associat();
                        roads["S"].Add(road);
                    }
                    else
                    if (ops[0].DistanceToSquared(ps[1]) <= 1)
                    {
                        //association.EmbedName = "ConnectRoad-ES";
                        //association.Associat();
                        roads["E"].Add(road);
                    }
                    else
                    if (ops[1].DistanceToSquared(ps[1]) <= 1)
                    {
                        //association.EmbedName = "ConnectRoad-EE";
                        //association.Associat();
                        roads["E"].Add(road);
                    }
                    else
                    if (ops[1].DistanceToSquared(ps[0]) <= 1)
                    {
                        //association.EmbedName = "ConnectRoad-SE";
                        //association.Associat();
                        roads["S"].Add(road);
                    }
                    else
                    {
                        var crossPs = Intersect2d.CurveWithCurve(road.BaseCurve, this.BaseCurve);
                        if (crossPs.Count > 0)
                        {

                            roads["C"].Add(road);
                            //association.EmbedName = "CrossRoad";
                            //association.Associat();
                        }
                        //foreach (var curve in road.MdiLines)
                        //{
                        //    var crossPs = Intersect2d.CurveWithCurve(curve, this.BaseCurve);
                        //    if (crossPs!=null&& crossPs.Count > 0)
                        //    {
                        //        roads["C"].Add(road);
                        //        break;
                        //        //association.EmbedName = "CrossRoad";
                        //        //association.Associat();
                        //    }
                        //}
                    }
                }
            }
            return roads;
        }
        private Line2d GetSpliceLine(Line2d baseLine,Line2d line,Vector2 crossP,double baseWidth,double width,double arcRadius)
        {
            var bvAngle = baseLine.Dir.RotateAround(new Vector2(), Math.PI / 2);
            var cvAngle = line.Dir.RotateAround(new Vector2(), Math.PI / 2);
            var br = baseLine.Clone().Translate(bvAngle * baseWidth / 2) as Line2d;
            var bl = baseLine.Clone().Translate(-bvAngle * baseWidth / 2) as Line2d;
            var cr = line.Clone().Translate(cvAngle * width / 2) as Line2d;
            var cl = line.Clone().Translate(-cvAngle * width / 2) as Line2d;
            //var flag = Intersect2d.IsLineWithLine(baseLine, cr) && Intersect2d.IsLineWithLine(baseLine, cl);
            //var minLLen = Math.Min(line.Start.DistanceTo(crossP),line.End.DistanceTo(crossP))/2;
            //var minBLen = Math.Min(baseLine.Start.DistanceTo(crossP), baseLine.End.DistanceTo(crossP))/2;
            //var minL = Math.Min(minBLen,minLLen);
            var linePrj = new Line2d();
            if (line.Angle > baseLine.Angle)
            {
                var angle = line.Angle - baseLine.Angle;
                if (angle <= Math.PI / 2)
                {
                    angle = angle / 2;
                    var crossBP = Intersect2d.XLineWithXLine(cl.Start, cl.Dir, br.Start, br.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    //len = Math.Min(minL, len);
                    var center = crossP.Clone().Add(line.Dir.RotateAround(new Vector2(), -angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else if (angle < Math.PI)
                {
                    angle = (Math.PI - angle) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(cr.Start, cr.Dir, br.Start, br.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    //len = Math.Min(minL, len);
                    var center = crossP.Clone().Add(line.Dir.RotateAround(new Vector2(), angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else if (angle < Math.PI * 1.5)
                {
                    angle = (angle - Math.PI) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(cl.Start, cl.Dir, bl.Start, bl.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    //len = Math.Min(minL, len);
                    var center = crossP.Clone().Add(line.Dir.RotateAround(new Vector2(), -angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else
                {
                    angle = (Math.PI * 2 - angle) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(cr.Start, cr.Dir, bl.Start, bl.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    //len = Math.Min(minL, len);
                    var center = crossP.Clone().Add(line.Dir.RotateAround(new Vector2(), angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
            }
            else
            {
                var angle = baseLine.Angle - line.Angle;
                if (angle <= Math.PI / 2)
                {
                    angle = angle / 2;
                    var crossBP = Intersect2d.XLineWithXLine(bl.Start, bl.Dir,cr.Start, cr.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    var center = crossP.Clone().Add(baseLine.Dir.RotateAround(new Vector2(), -angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else if (angle < Math.PI)
                {
                    angle = (Math.PI - angle) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(br.Start, br.Dir, cr.Start,cr.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    var center = crossP.Clone().Add(baseLine.Dir.RotateAround(new Vector2(), angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else if (angle < Math.PI * 1.5)
                {
                    angle = (angle - Math.PI) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(bl.Start, bl.Dir, cl.Start, cl.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    var center = crossP.Clone().Add(baseLine.Dir.RotateAround(new Vector2(), -angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
                else
                {
                    angle = (Math.PI * 2 - angle) / 2;
                    var crossBP = Intersect2d.XLineWithXLine(br.Start, br.Dir, cl.Start, cl.Dir);
                    var len = arcRadius / Math.Sin(angle);
                    var center = crossP.Clone().Add(baseLine.Dir.RotateAround(new Vector2(), angle) * len).Add(crossBP - crossP);
                    var mirCenter = center.Clone().Mirror(crossP, center.Clone().Sub(crossP).RotateAround(new Vector2(), Math.PI / 2));
                    linePrj = new Line2d(baseLine.PointProjection(center), baseLine.PointProjection(mirCenter));
                }
            }
            if (linePrj.Dir != baseLine.Dir)
                linePrj.Reverse();
            return linePrj;
        }
        public void DisposeConnectRoad(bool isCreate=false)
        {
            if (this.Parent!=null)
            {
                var elementSet = this.Parent as IElementSet;
                var roadDics = GetConnectRoads();
                var mdis = new LcList<Curve2d>();
                //foreach(var emb in this.EmbedAssociations)
                //{
                //    var qdIntersection =  emb.Host as QdIntersection;
                //    foreach (var association in qdIntersection.EmbedAssociations)
                //    {
                //        association.UnAssociat();
                //    }
                //    (qdIntersection.Parent as IElementSet).RemoveElement(qdIntersection);
                //}
                if (isCreate)
                {
                    for (var i = EmbedAssociations.Count - 1; i >= 0; i--)
                    {
                        EmbedAssociations[i].UnAssociat();
                    }
                }
                mdis.Add(this.BaseCurve.Clone());
                this.OnPropertyChangedBefore("", null, null);
                foreach (var crossRoad in roadDics["C"])
                {
                    if (crossRoad.BaseCurve is Line2d line && this.BaseCurve is Line2d baseLine)
                    {
                        //var bvAngle = baseLine.Dir.RotateAround(new Vector2(), Math.PI / 2);
                        //var cvAngle = line.Dir.RotateAround(new Vector2(), Math.PI / 2);
                        var crossP = Intersect2d.LineWithLine(line, baseLine);
                        if (crossP == null)
                            continue;
                        //var angle = Math.Abs(line.Angle - baseLine.Angle);
                        //angle =  angle < Math.PI / 2 ? angle : (angle > Math.PI * 1.5 ? Math.PI * 2 - angle : Math.Abs(Math.PI - angle));
                        //angle = angle / 2;
                        //var len = CornerRadius / Math.Tan(angle / 2);
                        //var rbl = baseLine.Clone().Translate(bvAngle * this.Width / 2) as Line2d;
                        //var rbcp = Intersect2d.XLineWithXLine(line.Start, line.Dir, rbl.Start, rbl.Dir);
                        //if (rbcp == null)
                        //    continue;
                        //var blen = rbcp.DistanceTo(crossP);

                        //var rcl = line.Clone().Translate(cvAngle * crossRoad.Width / 2) as Line2d;
                        //var rccp = Intersect2d.XLineWithXLine(baseLine.Start, baseLine.Dir, rcl.Start, rcl.Dir);
                        //if (rccp == null)
                        //    continue;
                        //var clen = rccp.DistanceTo(crossP);

                        //var centerPs = crossP.Clone().Add(baseLine.Dir.MultiplyScalar(len)).Add(bvAngle * CornerRadius);
                        //var prjPs = line.PointProjection(centerPs);
                        //var ldir = prjPs.Clone().Sub(crossP).Normalize();
                        //if (Math.Abs(prjPs.DistanceTo(centerPs) - CornerRadius) > 1)
                        //{
                        //    centerPs = crossP.Clone().Add(baseLine.Dir.MultiplyScalar(len)).Add(-bvAngle * CornerRadius);
                        //    prjPs = line.PointProjection(centerPs);
                        //    ldir = prjPs.Clone().Sub(crossP).Normalize();
                        //} 
                        //centerPs.Add(blen * ldir).Add(clen * baseLine.Dir);
                        //var cpDir = centerPs.Clone().Sub(crossP).Normalize().RotateAround(new Vector2(),Math.PI/2); 
                        //var ocPs = centerPs.Clone().Mirror(crossP,cpDir);
                        //var cprj= baseLine.PointProjection(centerPs);
                        //var oprj= baseLine.PointProjection(ocPs);
                        //var linePrj = new Line2d(cprj,oprj);
                        var arcRadius = Math.Max(this.Radius, crossRoad.Radius);
                        arcRadius = arcRadius == 0 ? this.CornerRadius : arcRadius;
                        var linePrj = GetSpliceLine(baseLine,line,crossP,this.Width,crossRoad.Width, arcRadius);
                        if (linePrj == null)
                            continue;
                        var count = mdis.Count;
                        for (var i= count-1;i>=0;i--)
                        {
                            var curMdi= mdis[i];
                            if (curMdi is Line2d nl)
                            {
                                var nls = LineSliceLine(nl, linePrj);
                                if (nls!=null)
                                {
                                    mdis.Remove(nl);
                                    mdis.InsertRange(i,nls);
                                }
                            }
                        }
                        if (isCreate)
                        {
                            if (crossRoad.EmbedAssociations.Any(n=>(n.Embed as QdRoad).Id==this.Id))
                            {
                                crossRoad.EmbedAssociations.First(n=>(n.Embed as QdRoad).Id == this.Id).UnAssociat();
                            }
                            var bcLine = baseLine.Clone() as Line2d;
                            var ccLine = line.Clone() as Line2d;
                            var flag = GenerateIntersection(this, crossRoad, bcLine, ccLine, linePrj.Length / 2, Math.Sqrt(Math.Pow(linePrj.Length / 2, 2) + Math.Pow(this.Width / 2 + arcRadius, 2) - Math.Pow(crossRoad.Width / 2 + arcRadius, 2)), crossP);
                            if (!flag)
                            {
                                GenerateIntersection(crossRoad,this,ccLine, bcLine, Math.Sqrt(Math.Pow(linePrj.Length / 2, 2) + Math.Pow(this.Width / 2 + arcRadius, 2) - Math.Pow(crossRoad.Width / 2 + arcRadius, 2)), linePrj.Length / 2, crossP,true);
                            }
                        }
                    }
                }
                if (roadDics["S"].Count==1)
                {
                    var road = roadDics["S"].First();
                    if (road.BaseCurve is Line2d line && this.BaseCurve is Line2d baseLine)
                    {
                        var angle = Math.Abs(line.Angle - baseLine.Angle);
                        angle = angle < Math.PI / 2 ? angle : (angle > Math.PI * 1.5 ? Math.PI * 2 - angle : Math.Abs(Math.PI - angle));
                        angle = angle / 2;
                        var arcRadius = Math.Max(this.Radius, road.Radius);
                        arcRadius = arcRadius == 0 ? this.CornerRadius : arcRadius;
                        var len = (arcRadius + this.Width / 2) / Math.Tan(angle / 2);
                        len = Math.Min(Math.Min(baseLine.Length / 2, line.Length / 2),len);
                        var ps = baseLine.Start.Clone().Add(baseLine.Dir.MultiplyScalar(len));
                        foreach (var n in mdis)
                        {
                            if (n is Line2d nl && ps.Clone().Sub(nl.Start).Normalize().Similarity(nl.Dir))
                            {
                                nl.Start = ps.Clone();
                            }
                        }
                        var ops = line.Start.Similarity(baseLine.Start) ? line.Start.Clone().Add(line.Dir.MultiplyScalar(len)) : line.End.Clone().Add(line.Dir.Negate().MultiplyScalar(len));
                        var included = Math.Abs(ops.Clone().Sub(baseLine.Start).Angle() - ps.Clone().Sub(baseLine.Start).Angle());
                        included = included > Math.PI ? Math.PI * 2 - included : included;
                        var dir = (new Vector2().AddVectors(ps, ops) / 2).Sub(baseLine.Start).Normalize();
                        var center = baseLine.Start.Clone().AddScaledVector(dir, len / Math.Cos(included / 2));
                        var arc = new Arc2d();
                        arc.Center = center.Clone();
                        arc.Radius = ps.DistanceTo(center);
                        arc.StartAngle = ps.Clone().Sub(center).Angle();
                        arc.EndAngle = ops.Clone().Sub(center).Angle();
                        arc.IsClockwise = ShapeUtils.isClockWise(new ListEx<Vector2>() { ps, ops, center });
                        var ep = arc.GetPoints(2)[1];
                        arc.EndAngle = ep.Sub(center).Angle();
                        mdis.Add(arc);
                    }
                }
                if (roadDics["E"].Count ==1)
                {
                    var road = roadDics["E"].First();
                    if (this.BaseCurve is Line2d baseLine && road.BaseCurve is Line2d line)
                    {
                        var angle = Math.Abs(line.Angle - baseLine.Angle);
                        angle = angle < Math.PI / 2 ? angle : (angle > Math.PI * 1.5 ? Math.PI * 2 - angle : Math.Abs(Math.PI - angle));
                        angle = angle / 2;
                        var arcRadius = Math.Max(this.Radius, road.Radius);
                        arcRadius = arcRadius == 0 ?   this.CornerRadius: arcRadius;
                        var len = (arcRadius + this.Width / 2) / Math.Tan(angle / 2);
                        len = Math.Min(Math.Min(baseLine.Length / 2, line.Length / 2), len);
                        var ps = baseLine.End.Clone().Add(baseLine.Dir.Negate().MultiplyScalar(len));
                        foreach (var n in mdis)
                        {
                            if (n is Line2d nl && nl.End.Clone().Sub(ps).Normalize().Similarity(nl.Dir))
                            {
                                nl.End = ps.Clone();
                            }
                        }
                        var ops = baseLine.End.Similarity(line.Start) ? line.Start.Clone().Add(line.Dir.MultiplyScalar(len)) : line.End.Clone().Add(line.Dir.Negate().MultiplyScalar(len));
                        var included = Math.Abs(ops.Clone().Sub(baseLine.End).Angle() - ps.Clone().Sub(baseLine.End).Angle());
                        included = included > Math.PI ? Math.PI * 2 - included : included;
                        var dir = (new Vector2().AddVectors(ps, ops) / 2).Sub(baseLine.End).Normalize();
                        var center = baseLine.End.Clone().AddScaledVector(dir, len / Math.Cos(included / 2));
                        var arc = new Arc2d();
                        arc.Center = center.Clone();
                        arc.Radius = ps.DistanceTo(center);
                        arc.StartAngle = ps.Clone().Sub(center).Angle();
                        arc.EndAngle = ops.Clone().Sub(center).Angle();
                        arc.IsClockwise = ShapeUtils.isClockWise(new ListEx<Vector2>() { ps, ops, center });
                        var ep = arc.GetPoints(2)[1];
                        arc.EndAngle = ep.Sub(center).Angle();
                        mdis.Add(arc);
                    }

                }
                this.MdiLines = mdis;
                this.CreateShapeCurves();
                this.OnPropertyChangedAfter("", null, null);
            }
        }
        public List<Line2d> LineSliceLine(Line2d curLine,Line2d sliceLine)
        {
            var csss = new Line2d(curLine.Start.Clone(), sliceLine.Start.Clone());
            var csee = new Line2d(sliceLine.End.Clone(), curLine.End.Clone());
            if (csss.Dir == curLine.Dir&&csee.Dir==curLine.Dir)
            {
                return new List<Line2d> { csss, csee };
            }
            else if (csss.Dir == curLine.Dir&& csss.LengthSq<curLine.LengthSq)
            {
                curLine.End = sliceLine.Start;
            }
            else if (csee.Dir == curLine.Dir && csee.LengthSq < curLine.LengthSq)
            {
                curLine.Start = sliceLine.End;
            }
            return null;
        }
        //private double MinRadius(double len1,double len2,double angle)
        //{
        //    var minL = Math.Min(len1, len2) / 2;
        //    return Math.Tan(angle / 2) * minL;
        //}
        private bool GenerateIntersection(QdRoad baseRoad, QdRoad crossRoad, Line2d baseLine, Line2d crossline, double len1, double len2, Vector2 cp,bool flag=false)
        {
            if(! (baseLine.Angle - crossline.Angle > Math.PI || (baseLine.Angle - crossline.Angle < 0 && baseLine.Angle - crossline.Angle > -Math.PI)))
                crossline.Reverse();

            var bvAngle = baseLine.Dir.RotateAround(new Vector2(), Math.PI / 2);
            var cvAngle = crossline.Dir.RotateAround(new Vector2(), Math.PI / 2);
            var br = baseLine.Clone().Translate(bvAngle * baseRoad.Width / 2) as Line2d;
            var bl = baseLine.Clone().Translate(-bvAngle * baseRoad.Width / 2) as Line2d;
            var cr = crossline.Clone().Translate(cvAngle * crossRoad.Width / 2) as Line2d;
            var cl = crossline.Clone().Translate(-cvAngle * crossRoad.Width / 2) as Line2d;
            if ((Intersect2d.IsLineWithLine(cl, bl) && Intersect2d.IsLineWithLine(cr, bl) && !Intersect2d.IsLineWithLine(cl, br) && !Intersect2d.IsLineWithLine(cr, br)) || (!Intersect2d.IsLineWithLine(cl, bl) && !Intersect2d.IsLineWithLine(cr, bl) && Intersect2d.IsLineWithLine(cl, br) && Intersect2d.IsLineWithLine(cr, br)))
            {
                return false;
            }
            var bcps = cp.Clone().Add(-baseLine.Dir * len1);
            var bcpe = cp.Clone().Add(baseLine.Dir * len1);
            var brs = br.PointProjection(bcps);
            var bre = br.PointProjection(bcpe);
            var bls = bl.PointProjection(bcps);
            var ble = bl.PointProjection(bcpe);
            var ccps = cp.Clone().Add(-crossline.Dir * len2);
            var ccpe = cp.Clone().Add(crossline.Dir * len2);
            var crs = cr.PointProjection(ccps);
            var cre = cr.PointProjection(ccpe);
            var cls = cl.PointProjection(ccps);
            var cle = cl.PointProjection(ccpe);
            var outLoop = new List<Curve2d>();
            outLoop.Add(new Line2d(cre.Clone(), cle.Clone()));
            if (Intersect2d.IsLineWithLine(baseLine,cl))
            {
                var clbreeCorner = Intersect2d.XLineWithXLine(bre, baseLine.Dir.Negate(), cle, crossline.Dir.Negate());
                if (clbreeCorner != null)
                {
                    var bdist = bre.DistanceTo(clbreeCorner);
                    var cdist = cle.DistanceTo(clbreeCorner);
                    var minDistance = Math.Min(bdist, cdist);
                    var angle = Math.Abs(new Vector2().SubVectors(cle, clbreeCorner).Angle() - new Vector2().SubVectors(bre, clbreeCorner).Angle());
                    angle = angle > Math.PI ? Math.PI * 2 - angle : angle;
                    var radius = minDistance * Math.Tan(angle / 2);
                    var center = bdist == minDistance ? bre.Clone().Add(bvAngle * radius) : cle.Clone().Add(-cvAngle * radius);
                    var arc = new Arc2d();
                    arc.Center = center;
                    arc.Radius = radius;
                    if (Math.Abs(bdist - cdist) < 1)
                    {
                        outLoop.Add(arc);
                        arc.StartAngle = new Vector2().SubVectors(cle, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(bre, center).Angle();
                    }
                    else if (bdist < cdist)
                    {
                        var arcsp = cle.Clone().Add((bdist - cdist) * crossline.Dir);
                        arc.StartAngle = new Vector2().SubVectors(arcsp, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(bre, center).Angle();
                        outLoop.Add(new Line2d(cle.Clone(), arcsp.Clone()));
                        outLoop.Add(arc);
                    }
                    else if (bdist > cdist)
                    {
                        var arcep = bre.Clone().Add((cdist - bdist) * baseLine.Dir);
                        arc.StartAngle = new Vector2().SubVectors(cle, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(arcep, center).Angle();
                        outLoop.Add(arc);
                        outLoop.Add(new Line2d(arcep.Clone(), bre.Clone()));
                    }
                }
                outLoop.Add(new Line2d(bre.Clone(), ble.Clone()));
                var blclesCorner = Intersect2d.XLineWithXLine(ble, baseLine.Dir.Negate(), cls, crossline.Dir);
                if (blclesCorner != null)
                {
                    var bdist = ble.DistanceTo(blclesCorner);
                    var cdist = cls.DistanceTo(blclesCorner);
                    var minDistance = Math.Min(bdist, cdist);
                    var angle = Math.Abs(new Vector2().SubVectors(ble, blclesCorner).Angle() - new Vector2().SubVectors(cls, blclesCorner).Angle());
                    angle = angle > Math.PI ? Math.PI * 2 - angle : angle;
                    var radius = minDistance * Math.Tan(angle / 2);
                    var center = bdist == minDistance ? ble.Clone().Add(-bvAngle * radius) : cls.Clone().Add(-cvAngle * radius);
                    var arc = new Arc2d();
                    arc.Center = center;
                    arc.Radius = radius;
                    if (Math.Abs(bdist - cdist) < 1)
                    {
                        outLoop.Add(arc);
                        arc.StartAngle = new Vector2().SubVectors(ble, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(cls, center).Angle();
                    }
                    else if (bdist < cdist)
                    {
                        var arcep = cls.Clone().Add((cdist - bdist) * crossline.Dir);
                        arc.StartAngle = new Vector2().SubVectors(ble, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(arcep, center).Angle();
                        outLoop.Add(arc);
                        outLoop.Add(new Line2d(arcep.Clone(), cls.Clone()));
                    }
                    else if (bdist > cdist)
                    {
                        var arcsp = ble.Clone().Add((cdist - bdist) * baseLine.Dir);
                        arc.StartAngle = new Vector2().SubVectors(arcsp, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(cls, center).Angle();
                        outLoop.Add(new Line2d(ble.Clone(), arcsp.Clone()));
                        outLoop.Add(arc);
                    }
                }
            }
            else
            {
                outLoop.Add(new Line2d(cle.Clone(), cls.Clone()));
            }

            outLoop.Add(new Line2d(cls.Clone(), crs.Clone()));
            if (Intersect2d.IsLineWithLine(baseLine, cr))
            {
                var blcrssCorner = Intersect2d.XLineWithXLine(bls, baseLine.Dir, crs, crossline.Dir);
                if (blcrssCorner != null)
                {
                    var bdist = bls.DistanceTo(blcrssCorner);
                    var cdist = crs.DistanceTo(blcrssCorner);
                    var minDistance = Math.Min(bdist, cdist);
                    var angle = Math.Abs(new Vector2().SubVectors(crs, blcrssCorner).Angle() - new Vector2().SubVectors(bls, blcrssCorner).Angle());
                    angle = angle > Math.PI ? Math.PI * 2 - angle : angle;
                    var radius = minDistance * Math.Tan(angle / 2);
                    var center = bdist == minDistance ? bls.Clone().Add(-bvAngle * radius) : crs.Clone().Add(cvAngle * radius);
                    var arc = new Arc2d();
                    arc.Center = center;
                    arc.Radius = radius;
                    if (Math.Abs(bdist - cdist) < 1)
                    {
                        outLoop.Add(arc);
                        arc.StartAngle = new Vector2().SubVectors(crs, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(bls, center).Angle();
                    }
                    else if (bdist < cdist)
                    {
                        var arcsp = crs.Clone().Add((cdist - bdist) * crossline.Dir);
                        outLoop.Add(new Line2d(crs.Clone(), arcsp.Clone()));
                        arc.StartAngle = new Vector2().SubVectors(arcsp, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(bls, center).Angle();
                        outLoop.Add(arc);
                    }
                    else if (bdist > cdist)
                    {
                        var arcep = bls.Clone().Add((bdist - cdist) * baseLine.Dir);
                        arc.StartAngle = new Vector2().SubVectors(crs, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(arcep, center).Angle();
                        outLoop.Add(arc);
                        outLoop.Add(new Line2d(arcep.Clone(), bls.Clone()));
                    }
                }
                outLoop.Add(new Line2d(bls.Clone(), brs.Clone()));
                var brcrseCorner = Intersect2d.XLineWithXLine(brs, baseLine.Dir, cre, crossline.Dir.Negate());
                if (brcrseCorner != null)
                {
                    var bdist = brs.DistanceTo(brcrseCorner);
                    var cdist = cre.DistanceTo(brcrseCorner);
                    var minDistance = Math.Min(bdist, cdist);
                    var angle = Math.Abs(new Vector2().SubVectors(cre, brcrseCorner).Angle() - new Vector2().SubVectors(brs, brcrseCorner).Angle());
                    angle = angle > Math.PI ? Math.PI * 2 - angle : angle;
                    var radius = minDistance * Math.Tan(angle / 2);
                    var center = bdist == minDistance ? brs.Clone().Add(bvAngle * radius) : cre.Clone().Add(cvAngle * radius);
                    var arc = new Arc2d();
                    arc.Center = center;
                    arc.Radius = radius;
                    if (Math.Abs(bdist - cdist) < 1)
                    {
                        outLoop.Add(arc);
                        arc.StartAngle = new Vector2().SubVectors(brs, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(cre, center).Angle();
                    }
                    else if (bdist < cdist)
                    {
                        var arcep = cre.Clone().Add((bdist - cdist) * crossline.Dir);
                        arc.StartAngle = new Vector2().SubVectors(brs, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(arcep, center).Angle();
                        outLoop.Add(arc);
                        outLoop.Add(new Line2d(arcep.Clone(), cre.Clone()));
                    }
                    else if (bdist > cdist)
                    {
                        var arcsp = brs.Clone().Add((bdist - cdist) * baseLine.Dir);
                        arc.StartAngle = new Vector2().SubVectors(arcsp, center).Angle();
                        arc.EndAngle = new Vector2().SubVectors(cre, center).Angle();
                        outLoop.Add(new Line2d(brs.Clone(), arcsp.Clone()));
                        outLoop.Add(arc);
                    }
                }
            }
            else
            {
                outLoop.Add(new Line2d(crs.Clone(), cre.Clone()));
            }
            var otherR = flag ? baseRoad : crossRoad;
            if (this.EmbedAssociations.Any(n =>( n.Embed as QdRoad).Id == otherR.Id))
            {
                var oldAsc = this.EmbedAssociations.First(n => (n.Embed as QdRoad).Id == otherR.Id);
                oldAsc.UnAssociat();
            }
            EmbedAssociation association = new EmbedAssociation();
            association.EmbedName = nameof(otherR);
            association.Embed = otherR as IEmbed;
            association.Host = this;
            association.HostTag =  outLoop;
            association.HostName = nameof(QdSite);
            association.Initilize(this.Document);
            association.Associat();
            return true;
        }

        public List<Curve2d> CreateShape(Curve2d curve, double width)
        {
            if (curve is Line2d line)
            {
                var leftLine = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(-width / 2)) as Line2d;
                var rightLine = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(width / 2)) as Line2d;
                return new List<Curve2d>() { leftLine, rightLine.Reverse() };
            }
            else
            {
                var arc = curve as Arc2d;
                var innerArc = arc.Clone() as Arc2d;
                innerArc.Radius -= width / 2;
                var outerArc = arc.Clone() as Arc2d;
                outerArc.Radius += width / 2;
                if (!arc.IsClockwise)
                {
                    innerArc.Reverse();
                }
                else
                {
                    outerArc.Reverse();
                }
                return new List<Curve2d>() { innerArc,outerArc };
            }
        }
        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            foreach (var curve in this.GetShapes()[0].Curve2ds)
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
            if (BaseCurve is Line2d baseline && Intersect2d.IsPolygonWithLine(testPoly.Points, baseline.Start, baseline.End))
            {
                return true;
            }
            if (BaseCurve is Arc2d basearc && Intersect2d.IsPolygonWithArc(testPoly.Points, basearc))
            {
                return true;
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
            foreach (var curve in this.GetShapes()[0].Curve2ds)
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
            if (BaseCurve is Line2d baseline && Intersect2d.IsPolygonWithLine(testPoly.Points, baseline.Start, baseline.End))
            {
                return true;
            }
            if (BaseCurve is Arc2d basearc && Intersect2d.IsPolygonWithArc(testPoly.Points, basearc))
            {
                return true;
            }
            return false;
        }
        public override void ResetCache()
        {
            base.ResetCache();
        }
 
        public Profile2[] GetEmbedHoles()
        {
            return new Profile2[] { };
        }

        public override Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict = null)
        {
            dict = base.ToDictionary(dict);

            dict.Add(nameof(this.MdiLines), this.MdiLines);
            return dict;
        }
        public override void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            base.FromDictionary(props);

            this.MdiLines = props.GetCurve2dArray<LcList<Curve2d>>(nameof(MdiLines));
            
        }
    }
}
