using LightCAD.Core.Component;
using LightCAD.MathLib;
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
    internal static class QdFoundationPitProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("69FBC6C4-F356-23B2-2704-56C0809CBF3B", "基坑",  基坑 )
            });
            ConvertToProvider("3E2422F7-F11D-27E4-B026-F4EE87ED08A6", nameof(GetSolid_基坑), GetSolid_基坑, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
      

            return new MaterialInfo[] { MaterialManager.GetMaterial(MaterialManager.Metal1Uuid) ,MaterialManager.GetMaterial(MaterialManager.ConcreteUuid) };
        }
        internal static Curve2dGroupCollection 基坑(LcParameterSet pset, ShapeCreator creator)
        {
            var com = creator.ComIns as DirectComponent;
            var outline = com.BaseCurve as Polyline2d;
            var bottom = pset.GetValue<double>("Bottom");
            var pattern = pset.GetValue<int>("Pattern");
            var factor = pset.GetValue<double>("Factor");
            var elevation = pset.GetValue<double>("Elevation");
            var width = (elevation - bottom) * factor;
            if (ShapeUtils.isClockWise(outline.Curve2ds.SelectMany(n => n.GetPoints(2)).ToListEx()))
            {
                outline.Reverse();
            }
            var curves = new List<Curve2d>();
            curves = outline.Curve2ds.Clone();
            var topCurves = new List<Curve2d>();
            var bottomCurves = new List<Curve2d>();
            if (pattern == 0)
            {
                bottomCurves = GetShape(curves);
                topCurves = ShapeExtend(bottomCurves, width);
            }
            else
            {
                topCurves = GetShape(curves);
                bottomCurves = ShapeExtend(topCurves, -width);
            }
            var space = width / 4;
            var connectCurves=new List<Curve2d>();
            for (var i=0;i<topCurves.Count;i++)
            {
                var tl = topCurves[i] as Line2d;
                var bl = bottomCurves[i] as Line2d;
                if (space!=0)
                {
                    var count = Convert.ToInt32(Math.Floor(tl.Length / space));
                    for (var k = 2; k < count-2; k++)
                    {
                        var sp = tl.Start.Clone().AddScaledVector(tl.Dir, space * k);
                        var ep = sp.Clone().AddScaledVector(tl.Dir.Clone().RotateAround(new Vector2(), Math.PI / 2), space * (k % 2 + 1));
                        connectCurves.Add(new Line2d(sp, ep));
                    }
                }
                connectCurves.Add(new Line2d(tl.Start.Clone(),bl.Start.Clone()));
            }
            var baseCurveGrp = new Curve2dGroup { Curve2ds = new ListEx<Curve2d>()};
            baseCurveGrp.Curve2ds.AddRange(topCurves);
            baseCurveGrp.Curve2ds.AddRange(bottomCurves);
            baseCurveGrp.Curve2ds.AddRange(connectCurves);
            //baseCurveGrp.Color = Color.Yellow;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_基坑(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var outline= pset.GetValue<Polyline2d>("Outline");
            var bottom = pset.GetValue<double>("Bottom");
            var pattern = pset.GetValue<int>("Pattern");
            var factor = pset.GetValue<double>("Factor");
            var elevation = pset.GetValue<double>("Elevation");
            var width = (elevation - bottom) * factor;
            if (ShapeUtils.isClockWise(outline.Curve2ds.SelectMany(n => n.GetPoints(2)).ToListEx()))
            {
                outline.Reverse();
            }
            var curves = new List<Curve2d>();
            curves = GetShape(outline.Curve2ds.Clone());
            var topCurves = new List<Curve3d>();
            var bottomCurves = new List<Curve3d>();
            if (pattern == 0)
            {
                bottomCurves = curves.Select(n=>n.ToCurve3d().Translate(0,0, bottom)).ToList();
                topCurves = ShapeExtend(curves, width).Select(n => n.ToCurve3d().Translate(0, 0, elevation)).ToList();
            }
            else
            {
                topCurves = GetShape(curves).Select(n => n.ToCurve3d().Translate(0, 0, elevation)).ToList();
                bottomCurves = ShapeExtend(curves, -width).Select(n => n.ToCurve3d().Translate(0, 0, bottom)).ToList();
            }
            var pitSurfaces = new List<Surface3d>();
            var pitB = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), bottomCurves.Clone());
            var pitT = new PlanarSurface3d(new Plane(new Vector3(0, 0, -1)), bottomCurves.Clone());
            pitSurfaces.Add(pitB);
            pitSurfaces.Add(pitT);
            var posArr = new ListEx<double>();
            var idxArr = new ListEx<int>();
            int idxOffset = 0;
            for (int i = 0; i < pitSurfaces.Count; i++)
            {
                var face = pitSurfaces[i];
                var tuple = face.Trianglate();
                posArr.AddRange(tuple.Position);
                idxArr.AddRange(tuple.Indices.Select(idx => idx + idxOffset));
                idxOffset += tuple.Position.Length / 3;
            }
            var slopeSurfaces = new List<Surface3d>();
            for (var i = 0; i < topCurves.Count; i++) 
            {
                var tl = topCurves[i].Clone() as Line3d;
                var bl = bottomCurves[i].Clone() as Line3d;
                var tbs = new Line3d(tl.Start.Clone(), bl.Start.Clone());
                var tbe = new Line3d(tl.End.Clone(), bl.End.Clone());
                bl.Reverse();
                tbs.Reverse();
                var loop = new List<Curve3d>() { tl,tbe,bl,tbs};
                var normal = new Vector3().CrossVectors(tl.Dir,tbs.Dir);
                var slopeB = new PlanarSurface3d(new Plane(normal), loop.Clone());
                var slopeT = new PlanarSurface3d(new Plane(normal.Clone().Negate()), loop.Clone());
                slopeSurfaces.Add(slopeB);
                slopeSurfaces.Add(slopeT);
            }
            var slopePosArr = new ListEx<double>();
            var slopeIdxArr = new ListEx<int>();
            int slopeIdxOffset = 0;
            for (int i = 0; i < slopeSurfaces.Count; i++)
            {
                var face = slopeSurfaces[i];
                var tuple = face.Trianglate();
                slopePosArr.AddRange(tuple.Position);
                slopeIdxArr.AddRange(tuple.Indices.Select(idx => idx + slopeIdxOffset));
                slopeIdxOffset += tuple.Position.Length / 3;
            }
            return new Solid3dCollection() { new Solid3d() {
                    Name="Pit",
                    Geometry = new LightCAD.MathLib.GeometryData()
                    {
                        Verteics = posArr.ToArray(),
                        Indics = idxArr.ToArray(),
                        Groups = new GeometryGroup[1]
                        {
                            new GeometryGroup{ Name = "Geometry", Start = 0, Count = idxArr.Length, MaterialIndex = 0 },
                        }
                    } 
             },
            new Solid3d() {
                    Name="Slope",
                    Geometry = new LightCAD.MathLib.GeometryData()
                    {
                        Verteics = slopePosArr.ToArray(),
                        Indics = slopeIdxArr.ToArray(),
                        Groups = new GeometryGroup[1]
                        {
                            new GeometryGroup{ Name = "Geometry", Start = 0, Count = slopeIdxArr.Length, MaterialIndex = 1 },
                        }
                    }
             }};
        }
        public static List<Curve2d> GetShape(List<Curve2d> curves)
        {
            var newCurves = new List<Curve2d>();
            for (var i = 0; i < curves.Count; i++)
            {
                var curve = curves[i].Clone();
                if (curve is Line2d line)
                {
                    newCurves.Add(line);
                }
                else if (curve is Arc2d arc)
                {                   
                    var count = Convert.ToInt32(Math.Abs((arc.EndAngle - arc.StartAngle) / Math.PI * 16));
                    count = Math.Max(5, count);
                    var ps = arc.GetPoints(count);
                    for (var k = 0; k < count; k++)
                    {
                        newCurves.Add(new Line2d(ps[k].Clone(), ps[k + 1].Clone()));
                    }
                }
            }
            return newCurves;
        }
        public static List<Curve2d> ShapeExtend(List<Curve2d> curves,double width)
        {
            var newCurves = new List<Curve2d>();
            for (var i = 0; i < curves.Count; i++)
            {
                var curve = curves[i].Clone();
                if (curve is Line2d line)
                {
                    line.Translate(line.Dir.Clone().RotateAround(new Vector2(), - Math.PI / 2).MultiplyScalar(width));
                    newCurves.Add(line);
                }
                else if (curve is Arc2d arc)
                {
                    if (arc.IsClockwise)
                        arc.Radius -= width;
                    else
                        arc.Radius += width;
                    //newCurves.Add(arc);
                    var count = Convert.ToInt32(Math.Abs((arc.EndAngle - arc.StartAngle) / Math.PI * 16));
                    count = Math.Max(5, count);
                    var ps = arc.GetPoints(count);
                    for (var k = 0; k < count; k++)
                    {
                        newCurves.Add(new Line2d(ps[k].Clone(), ps[k + 1].Clone()));
                    }
                }
            }

            for (var i = 0; i < newCurves.Count; i++)
            {
                var lastCurve = newCurves[i == 0 ? newCurves.Count - 1 : i - 1];
                var nextCurve = newCurves[i == newCurves.Count - 1 ? 0 : i + 1];
                if (newCurves[i] is Line2d line)
                {
                    if (lastCurve is Line2d lastLine)
                    {
                        var cps = Intersect2d.XLineWithXLine(line.Start, line.Dir.Clone().Negate(), lastLine.Start, lastLine.Dir);
                        if (cps != null)
                        {
                            line.Start = cps;
                        }
                    }
                    //else if (lastCurve is Arc2d lastArc)
                    //{
                    //    var cps = Intersect2d.CircleWithXLine( new Circle2d() { Center= lastArc.Center,Radius=lastArc.Radius,IsClosed=lastArc.IsClosed},new XLine2d() { Origin= line.Start,Direction=line.Dir.Clone().Negate()});
                    //    if (cps.Count>0)
                    //    {
                    //        var minLen = cps.Select(n => n.DistanceToSquared(line.Start)).Min();
                    //        line.Start = cps.First(n => n.DistanceToSquared(line.Start) == minLen);
                    //    }
                    //}
                    if (nextCurve is Line2d nextLine)
                    {
                        var cpe = Intersect2d.XLineWithXLine(line.Start, line.Dir, nextLine.Start, nextLine.Dir);
                        if (cpe != null)
                        {
                            line.End = cpe;
                        }
                    }
                    //else if (nextCurve is Arc2d nextArc)
                    //{
                    //    var cps = Intersect2d.CircleWithXLine(new Circle2d() { Center = nextArc.Center, Radius = nextArc.Radius, IsClosed = nextArc.IsClosed }, new XLine2d() { Origin = line.Start, Direction = line.Dir });
                    //    if (cps.Count > 0)
                    //    {
                    //        var minLen = cps.Select(n => n.DistanceToSquared(line.Start)).Min();
                    //        line.End = cps.First(n => n.DistanceToSquared(line.Start) == minLen);
                    //    }
                    //}
                }
                //else if (newCurves[i] is Arc2d arc)
                //{
                //    var ps = arc.GetPoints(1);
                //    if (lastCurve is Line2d lastLine)
                //    {
                //        var cps = Intersect2d.CircleWithXLine(new Circle2d() { Center = arc.Center, Radius = arc.Radius, IsClosed = arc.IsClosed }, new XLine2d() { Origin = lastLine.Start, Direction = lastLine.Dir });
                //        if (cps.Count > 0)
                //        {
                //            var minLen = cps.Select(n => n.DistanceToSquared(lastLine.End)).Min();
                //            if (arc.IsClockwise)
                //            {
                //                arc.EndAngle = cps.First(n => n.DistanceToSquared(lastLine.End) == minLen).Sub(arc.Center).Angle();
                //            }
                //            else
                //            {
                //                arc.StartAngle = cps.First(n => n.DistanceToSquared(lastLine.End) == minLen).Sub(arc.Center).Angle();
                //            }
                //        }
                //    }
                //    else if (lastCurve is Arc2d lastArc)
                //    {
                //        var cps = Intersect2d.CircleWithCircle(new Circle2d() { Center = arc.Center, Radius = arc.Radius, IsClosed = arc.IsClosed }, new Circle2d() { Center = lastArc.Center, Radius = lastArc.Radius, IsClosed = lastArc.IsClosed });
                //        if (cps.Count > 0)
                //        {
                //            var minLen = cps.Select(n => n.DistanceToSquared(ps[0])).Min();
                //            if (arc.IsClockwise)
                //            {
                //                arc.EndAngle = cps.First(n => n.DistanceToSquared(ps[0]) == minLen).Sub(arc.Center).Angle();
                //            }
                //            else
                //            {
                //                arc.StartAngle = cps.First(n => n.DistanceToSquared(ps[0]) == minLen).Sub(arc.Center).Angle();
                //            }
                //        }
                //    }
                //    if (nextCurve is Line2d nextLine)
                //    {
                //        var cps = Intersect2d.CircleWithXLine(new Circle2d() { Center = arc.Center, Radius = arc.Radius, IsClosed = arc.IsClosed }, new XLine2d() { Origin = nextLine.Start, Direction = nextLine.Dir.Clone().Negate() });
                //        if (cps.Count > 0)
                //        {
                //            var minLen = cps.Select(n => n.DistanceToSquared(nextLine.Start)).Min();
                //            if (arc.IsClockwise)
                //            {
                //                arc.EndAngle = cps.First(n => n.DistanceToSquared(nextLine.Start) == minLen).Sub(arc.Center).Angle();
                //            }
                //            else
                //            {
                //                arc.StartAngle = cps.First(n => n.DistanceToSquared(nextLine.Start) == minLen).Sub(arc.Center).Angle();
                //            }
                //        }
                //    }
                //    else if (nextCurve is Arc2d nextArc)
                //    {
                //        var cps = Intersect2d.CircleWithCircle(new Circle2d() { Center = arc.Center, Radius = arc.Radius, IsClosed = arc.IsClosed }, new Circle2d() { Center = nextArc.Center, Radius = nextArc.Radius, IsClosed = nextArc.IsClosed });
                //        if (cps.Count > 0)
                //        {
                //            var minLen = cps.Select(n => n.DistanceToSquared(ps[1])).Min();
                //            if (arc.IsClockwise)
                //            {
                //                arc.EndAngle = cps.First(n => n.DistanceToSquared(ps[1]) == minLen).Sub(arc.Center).Angle();
                //            }
                //            else
                //            {
                //                arc.StartAngle = cps.First(n => n.DistanceToSquared(ps[1]) == minLen).Sub(arc.Center).Angle();
                //            }
                //        }
                //    }
                //}

            }
            return newCurves;
        }
 
    }
}
