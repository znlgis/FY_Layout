using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class LcCurveChangeLoop
    {
        public static List<LcPolyLine> CheckLoops(List<LcCurve2d> eles)
        {
            var polys = new List<LcPolyLine>();
            for (var i = 0; i < eles.Count; i++)
            {
                var poly = new Polyline2d();
                poly.Curve2ds = new List<Curve2d>();
                var line = eles[i];
                ChangeLineToPolyLine(poly, line);
                if (eles[i] is LcCircle)
                {
                    poly.IsClosed = true;
                    polys.Add(new LcPolyLine() { Curve2ds=poly.Curve2ds.ToLcListCurve(),Curve=poly});
                    continue;
                }
                if (poly.Curve2ds.First().GetPoints(1).First().Similarity(poly.Curve2ds.Last().GetPoints(1).Last(), 0))
                {
                    //if (CheckIntersectUnStartOrEnd(poly))
                    //{
                    //    msg = "线段闭环内有相交线";
                    //    return false;
                    //}
                    poly.IsClosed = true;
                    polys.Add(new LcPolyLine() { Curve2ds = poly.Curve2ds.ToLcListCurve(), Curve = poly });
                    continue;
                }
                var flag = false;
                for (var k = eles.Count - 1; k > i; k--)
                {
                    for (var j = eles.Count - 1; j > i; j--)
                    {
                        var ele = eles[j];
                        if (ChangeLineToPolyLine(poly, ele))
                        {
                            eles.Remove(ele);
                            break;
                        }
                    }
                    if (poly.Curve2ds.Count > 1 && poly.Curve2ds.First().GetPoints(1).First().Similarity(poly.Curve2ds.Last().GetPoints(1).Last(), 0))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    poly.IsClosed = true;
                    polys.Add(new LcPolyLine() { Curve2ds = poly.Curve2ds.ToLcListCurve(), Curve = poly });
                }
            }
            foreach (var poly in polys)
            {
                poly.Initilize(eles.First().Document);
            }
            return polys;
        }
        private static bool ChangeLineToPolyLine(Polyline2d lcPoly, LcCurve2d element)
        {
            if (element.Type == BuiltinElementType.Line)
            {
                var line = element as LcLine;
                if (lcPoly.Curve2ds.Count == 0)
                {
                    lcPoly.Curve2ds.Add(new Line2d(line.Start.Clone(), line.End.Clone()));
                    return true;
                }
                else
                {
                    var lastP = lcPoly.Curve2ds.Last().GetPoints(1).Last();
                    if (lastP.Similarity(line.Start, 0))
                    {
                        lcPoly.Curve2ds.Add(line.Curve);
                        return true;
                    }
                    else if (lastP.Similarity(line.End, 0))
                    {
                        line.Curve.Reverse();
                        lcPoly.Curve2ds.Add(line.Curve);
                        return true;
                    }
                }
            }
            else if (element.Type == BuiltinElementType.PloyLine)
            {
                var poly = element as LcPolyLine;
                if (lcPoly.Curve2ds.Count == 0)
                {
                    lcPoly.Curve2ds.AddRange(poly.Curve2ds);
                    return true;
                }
                else
                {
                    if (lcPoly.IsClosed)
                        return false;
                    var ps = poly.PolyLine.GetPoints(1);
                    var startP = ps.First();
                    var endP = ps.Last();
                    var lastP = lcPoly.Curve2ds.Last().GetPoints(1).Last();
                    if (lastP.Similarity(startP, 0))
                    {
                        lcPoly.Curve2ds.AddRange(poly.PolyLine.Curve2ds);
                        return true;
                    }
                    else if (lastP.Similarity(endP, 0))
                    {
                        poly.PolyLine.Reverse();
                        lcPoly.Curve2ds.AddRange(poly.PolyLine.Curve2ds);
                        return true;
                    }
                }
            }
            else if (element.Type == BuiltinElementType.Arc)
            {
                var arc = element as LcArc;
                if (lcPoly.Curve2ds.Count == 0)
                {
                    lcPoly.Curve2ds.Add(arc.Curve);
                    return true;
                }
                else
                {
                    var ps = arc.Curve.GetPoints(1);
                    var startP = ps.First();
                    var endP = ps.Last();
                    var lastP = lcPoly.Curve2ds.Last().GetPoints(1).Last();
                    if (lastP.Similarity(startP, 0))
                    {
                        lcPoly.Curve2ds.Add(arc.Curve);
                        return true;
                    }
                    else if (lastP.Similarity(endP, 0))
                    {
                        arc.Curve.Reverse();
                        lcPoly.Curve2ds.Add(arc.Curve);
                        return true;
                    }
                }
            }
            else if (element.Type == BuiltinElementType.Circle)
            {
                var circle = element as LcCircle;
                if (lcPoly.Curve2ds.Count == 0)
                {
                    var arc1 = new Arc2d();
                    arc1.StartAngle = 0;
                    arc1.EndAngle = Math.PI;
                    arc1.Center = circle.Center.Clone();
                    arc1.Radius = circle.Radius;
                    var arcp1 = arc1.GetPoints(2);
                    arc1.Startp = arcp1[0];
                    arc1.Endp = arcp1[2];
                    arc1.Midp = arcp1[1];
                    var arc2 = new Arc2d();
                    arc2.StartAngle = Math.PI;
                    arc2.EndAngle = Math.PI * 2;
                    arc2.Center = circle.Center.Clone();
                    arc2.Radius = circle.Radius;
                    var arcp2 = arc2.GetPoints(2);
                    arc2.Startp = arcp2[0];
                    arc2.Endp = arcp2[2];
                    arc2.Midp = arcp2[1];
                    lcPoly.Curve2ds.Add(arc1);
                    lcPoly.Curve2ds.Add(arc2);
                    return true;
                }
            }
            return false;
        }
    }
}
