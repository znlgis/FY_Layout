 
namespace QdLayout
{
    public class FoundationPit3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var foundationPit = cptIns as QdFoundationPit;
            var outline = foundationPit.Outline;
            var bottom = foundationPit.Bottom;
            var pattern = foundationPit.Pattern;
            var factor = foundationPit.Factor;
            var elevation = foundationPit.Elevation;
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
                bottomCurves = curves.Select(n => n.ToCurve3d().Translate(0, 0, bottom)).ToList();
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
                var loop = new List<Curve3d>() { tl, tbe, bl, tbs };
                var normal = new Vector3().CrossVectors(tl.Dir, tbs.Dir);
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
            var geoSlope = new BufferGeometry();
            geoSlope.setAttribute("position", new BufferAttribute(slopePosArr.ToArray(), 3, false));
            geoSlope.setIndex(new BufferAttribute(slopeIdxArr.ToArray(), 1));
            geoSlope.computeVertexNormals();
            geoSlope.SetUV();
            var geoPit = new BufferGeometry();
            geoPit.setAttribute("position", new BufferAttribute(posArr.ToArray(), 3, false));
            geoPit.setIndex(new BufferAttribute(idxArr.ToArray(), 1));
            geoPit.computeVertexNormals();
            geoPit.SetUV();
            var matS = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GetMaterial(MaterialManager.ConcreteUuid));
            var meshS = new Mesh(geoSlope, matS);
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GetMaterial(MaterialManager.EarthworkUuid));
            var mesh  = new Mesh(geoPit, mat);
            return new List<Object3D> { mesh ,meshS};
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
        public static List<Curve2d> ShapeExtend(List<Curve2d> curves, double width)
        {
            var newCurves = new List<Curve2d>();
            for (var i = 0; i < curves.Count; i++)
            {
                var curve = curves[i].Clone();
                if (curve is Line2d line)
                {
                    line.Translate(line.Dir.Clone().RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(width));
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
                    if (nextCurve is Line2d nextLine)
                    {
                        var cpe = Intersect2d.XLineWithXLine(line.Start, line.Dir, nextLine.Start, nextLine.Dir);
                        if (cpe != null)
                        {
                            line.End = cpe;
                        }
                    }
           
                } 
            }
            return newCurves;
        }

    }
}
