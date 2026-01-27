using LightCAD.Core.Component;
using LightCAD.MathLib.Csg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static ThreeJs4Net.ImageBitmapLoader;

namespace QdArchProvider
{
    internal static class QdBermProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("A2802FC8-94B2-ABFD-8AA2-5CABD9CC8FAB", "出土道路",  出土道路 )
            });
            ConvertToProvider("C55AD616-A858-F513-48D9-A577B2696D94", nameof(GetSolid_出土道路), GetSolid_出土道路, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { pset.GetValue<MaterialInfo>("Material") };
        }
        internal static Curve2dGroupCollection 出土道路(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var width = pset.GetValue<double>("Width");
            var baseline = pset.GetValue<Line2d>("Baseline");
            var normal = baseline.Dir.Clone().RotateAround(new Vector2(),Math.PI/2);
            var leftL = baseline.Clone().Translate(normal.Clone().MultiplyScalar(width / 2)) as Line2d;
            var rightL = baseline.Clone().Translate(normal.Clone().MultiplyScalar(-width / 2)) as Line2d;
            curves.Add(baseline.Clone());
            curves.Add(leftL);
            curves.Add(rightL);
            curves.Add(new Line2d(leftL.Start.Clone(),rightL.Start.Clone()));
            curves.Add(new Line2d(leftL.End.Clone(), rightL.End.Clone()));
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            //baseCurveGrp.Color = Color.Green;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_出土道路(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var line= pset.GetValue<Line2d>("Baseline");
            var bottom = pset.GetValue<double>("Bottom");
            var factor = pset.GetValue<double>("Factor");
            var width = pset.GetValue<double>("Width");
            var eleStart = pset.GetValue<double>("ElevationStart");
            var eleEnd = pset.GetValue<double>("ElevationEnd");
            var normal = line.Dir.Clone().RotateAround(new Vector2(), Math.PI / 2);
            var leftL = line.Clone().Translate(normal.Clone().MultiplyScalar(width / 2)) as Line2d;
            var rightL = line.Clone().Translate(normal.Clone().MultiplyScalar(-width / 2)) as Line2d;
            var topL = new Line3d(leftL.Start.ToVector3(eleStart),leftL.End.ToVector3(eleEnd));
            var topR = new Line3d(rightL.Start.ToVector3(eleStart), rightL.End.ToVector3(eleEnd));
            var topLrE = new Line3d(topL.End.Clone(), topR.End.Clone());
            var topRlS = new Line3d(topR.Start.Clone(), topL.Start.Clone());
            var topLoop = new List<Curve3d>() { topL.Clone(), topLrE.Clone(), topR.Clone().Reverse(), topRlS.Clone() };
            var tNor = new Vector3().CrossVectors(topL.Dir, topLrE.Dir).Negate();
            var surfaces = new List<Surface3d>();
            var topFace = new PlanarSurface3d(new Plane(tNor),topLoop);
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
            var btmFace = new PlanarSurface3d(new Plane(new Vector3(0,0,-1)), btmLoop);
            surfaces.Add(btmFace);
            var btsl = new Line3d(btmL.Start.Clone(), topL.Start.Clone());
            var tbel = new Line3d(topL.End.Clone(), btmL.End.Clone());
            var leftLoop = new List<Curve3d>() { topL.Clone(), tbel.Clone(),btmL.Clone().Reverse(), btsl.Clone() };
            var leftNor = new Vector3().CrossVectors(btmL.Dir,btsl.Dir).Negate();
            var leftFace = new PlanarSurface3d(new Plane(leftNor), leftLoop);
            surfaces.Add(leftFace);
            var btsr = new Line3d(btmR.Start.Clone(), topR.Start.Clone());
            var tber = new Line3d(topR.End.Clone(), btmR.End.Clone());
            var rightLoop = new List<Curve3d>() { topR.Clone(), tber.Clone(), btmR.Clone().Reverse(), btsr.Clone() };
            var rightNor = new Vector3().CrossVectors(btmR.Dir, btsr.Dir);
            var rightFace = new PlanarSurface3d(new Plane(rightNor), rightLoop);
            surfaces.Add(rightFace);
            if (eleStart-bottom>0)
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
    }
}
