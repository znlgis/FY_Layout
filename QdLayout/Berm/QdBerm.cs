using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdBerm : DirectComponent
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
        public double Factor
        {
            get
            {
                return Properties.GetValue<double>("Factor");
            }
            set
            {
                SetProps((GetPropId(nameof(Factor)), value));
            }
        }
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
        public double ElevationStart
        {
            get
            {
                return Properties.GetValue<double>("ElevationStart");
            }
            set
            {
                SetProps((GetPropId(nameof(ElevationStart)), value));
            }
        }
        public double ElevationEnd
        {
            get
            {
                return Properties.GetValue<double>("ElevationEnd");
            }
            set
            {
                SetProps((GetPropId(nameof(ElevationEnd)), value));
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
        public Line2d Baseline
        {
            get
            {
                return BaseCurve as Line2d;
            }
            set
            {
                BaseCurve = value;
            }
        }
      
        public QdBerm(QdBermDef bermDef) : base(bermDef)
        {
            Type = LayoutElementType.Berm;
            Baseline = new Line2d();
        }
        public override Curve2dGroupCollection GetShapes()
        {
            var curves = new List<Curve2d>();
            var width = this.Width;
            var baseline  = this.Baseline;
            var normal = baseline.Dir.Clone().RotateAround(new Vector2(), Math.PI / 2);
            var leftL = baseline.Clone().Translate(normal.Clone().MultiplyScalar(width / 2)) as Line2d;
            var rightL = baseline.Clone().Translate(normal.Clone().MultiplyScalar(-width / 2)) as Line2d;
            curves.Add(baseline.Clone());
            curves.Add(leftL);
            curves.Add(rightL);
            curves.Add(new Line2d(leftL.Start.Clone(), rightL.Start.Clone()));
            curves.Add(new Line2d(leftL.End.Clone(), rightL.End.Clone()));
            var bottom = this.Bottom;
            var factor = this.Factor;
            var eleStart = this.ElevationStart;
            var eleEnd = this.ElevationEnd;
            var btmL = new Line3d(leftL.Start.ToVector3(bottom), leftL.End.ToVector3(bottom));
            var btmR = new Line3d(rightL.Start.ToVector3(bottom), rightL.End.ToVector3(bottom));
            var startB = factor * (eleStart - bottom);
            var endB = factor * (eleEnd - bottom);
            btmL.Start.AddScaledVector(normal.ToVector3(), startB);
            btmL.End.AddScaledVector(normal.ToVector3(), endB);
            btmR.Start.AddScaledVector(normal.ToVector3(), -startB);
            btmR.End.AddScaledVector(normal.ToVector3(), -endB);
            curves.Add(new Line2d(btmL.Start.ToVector2(), btmL.End.ToVector2()));
            curves.Add(new Line2d(btmR.Start.ToVector2(), btmR.End.ToVector2()));
            curves.Add(new Line2d(btmL.Start.ToVector2(), btmR.Start.ToVector2()));
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        public override MaterialInfo[] GetSolidMaterials(Solid3d solid)
        {
            return new MaterialInfo[] { Material };
        }
        public override Solid3dCollection GetSolids()
        {
            var line = this.Baseline;
            var bottom = this.Bottom;
            var factor = this.Factor;
            var width = this.Width;
            var eleStart = this.ElevationStart;
            var eleEnd = this.ElevationEnd;
            var normal = line.Dir.Clone().RotateAround(new Vector2(), Math.PI / 2);
            var leftL = line.Clone().Translate(normal.Clone().MultiplyScalar(width / 2)) as Line2d;
            var rightL = line.Clone().Translate(normal.Clone().MultiplyScalar(-width / 2)) as Line2d;
            var topL = new Line3d(leftL.Start.ToVector3(eleStart), leftL.End.ToVector3(eleEnd));
            var topR = new Line3d(rightL.Start.ToVector3(eleStart), rightL.End.ToVector3(eleEnd));
            var topLrE = new Line3d(topL.End.Clone(), topR.End.Clone());
            var topRlS = new Line3d(topR.Start.Clone(), topL.Start.Clone());
            var topLoop = new List<Curve3d>() { topL.Clone(), topLrE.Clone(), topR.Clone().Reverse(), topRlS.Clone() };
            var tNor = new Vector3().CrossVectors(topL.Dir, topLrE.Dir).Negate();
            var surfaces = new List<Surface3d>();
            var topFace = new PlanarSurface3d(new Plane(tNor), topLoop);
            surfaces.Add(topFace);
            var btmL = new Line3d(leftL.Start.ToVector3(bottom), leftL.End.ToVector3(bottom));
            var btmR = new Line3d(rightL.Start.ToVector3(bottom), rightL.End.ToVector3(bottom));
            var startB = factor * (eleStart - bottom);
            var endB = factor * (eleEnd - bottom);
            btmL.Start.AddScaledVector(normal.ToVector3(), startB);
            btmL.End.AddScaledVector(normal.ToVector3(), endB);
            btmR.Start.AddScaledVector(normal.ToVector3(), -startB);
            btmR.End.AddScaledVector(normal.ToVector3(), -endB);
            var btmLrE = new Line3d(btmL.End.Clone(), btmR.End.Clone());
            var btmRlS = new Line3d(btmR.Start.Clone(), btmL.Start.Clone());
            var btmLoop = new List<Curve3d>() { btmL.Clone(), btmLrE.Clone(), btmR.Clone().Reverse(), btmRlS.Clone() };
            var btmFace = new PlanarSurface3d(new Plane(new Vector3(0, 0, -1)), btmLoop);
            surfaces.Add(btmFace);
            var btsl = new Line3d(btmL.Start.Clone(), topL.Start.Clone());
            var tbel = new Line3d(topL.End.Clone(), btmL.End.Clone());
            var leftLoop = new List<Curve3d>() { topL.Clone(), tbel.Clone(), btmL.Clone().Reverse(), btsl.Clone() };
            var leftNor = new Vector3().CrossVectors(btmL.Dir, btsl.Dir).Negate();
            var leftFace = new PlanarSurface3d(new Plane(leftNor), leftLoop);
            surfaces.Add(leftFace);
            var btsr = new Line3d(btmR.Start.Clone(), topR.Start.Clone());
            var tber = new Line3d(topR.End.Clone(), btmR.End.Clone());
            var rightLoop = new List<Curve3d>() { topR.Clone(), tber.Clone(), btmR.Clone().Reverse(), btsr.Clone() };
            var rightNor = new Vector3().CrossVectors(btmR.Dir, btsr.Dir);
            var rightFace = new PlanarSurface3d(new Plane(rightNor), rightLoop);
            surfaces.Add(rightFace);
            if (eleStart - bottom > 0)
            {
                var backLoop = new List<Curve3d>() { topRlS.Clone(), btsl.Clone().Reverse(), btmRlS.Clone().Reverse(), btsr.Clone() };
                var backNor = new Vector3().CrossVectors(topRlS.Dir, btsl.Dir).Negate();
                var backFace = new PlanarSurface3d(new Plane(backNor), backLoop);
                surfaces.Add(backFace);
            }
            if (eleEnd - bottom > 0)
            {
                var frontLoop = new List<Curve3d>() { topLrE.Clone(), tber.Clone(), btmLrE.Clone().Reverse(), tbel.Clone().Reverse() };
                var frontNor = new Vector3().CrossVectors(topLrE.Dir, tber.Dir);
                var frontFace = new PlanarSurface3d(new Plane(frontNor), frontLoop);
                surfaces.Add(frontFace);
            }
            var posArr = new ListEx<double>();
            var idxArr = new ListEx<int>();
            int idxOffset = 0;
            for (int i = 0; i < surfaces.Count; i++)
            {
                var face = surfaces[i];
                var tuple = face.Trianglate();
                posArr.AddRange(tuple.Position);
                idxArr.AddRange(tuple.Indices.Select(idx => idx + idxOffset));
                idxOffset += tuple.Position.Length / 3;
            }
            return new Solid3dCollection() { new Solid3d() {
                    Name="Berm",
                    Geometry = new LightCAD.MathLib.GeometryData()
                    {
                        Verteics = posArr.ToArray(),
                        Indics = idxArr.ToArray(),
                        Groups = new GeometryGroup[1]
                        {
                            new GeometryGroup{ Name = "Geometry", Start = 0, Count = idxArr.Length, MaterialIndex = 0 },
                        }
                    }
             } };
        }
        public override Box2 GetBoundingBox()
        {
            var ps = this.GetShapes().SelectMany(n=>n.Curve2ds.SelectMany(n=>n.GetPoints()));
            return new Box2().ExpandByPoints(ps.ToArray());
        }

        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdBerm(Definition as QdBermDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var berm = (QdBerm)src;
            Baseline = berm.Baseline.Clone() as Line2d;
            Material = berm.Material.Clone();
        }


        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            } 
            foreach (var curve in this.GetShapes().FirstOrDefault()?.Curve2ds)
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
            foreach (var curve in this.GetShapes().FirstOrDefault()?.Curve2ds)
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
