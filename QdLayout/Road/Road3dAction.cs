namespace QdLayout
{
    public class Road3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var results = new List<Object3D>();
            var road = cptIns as QdRoad;
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            var centerMat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(new MaterialInfo() { Color = Color.Yellow });
            var outerMat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(new MaterialInfo() { Color = Color.White });
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(road.Material);
            foreach (var mdiLine in road.MdiLines)
            {
                var shape = road.CreateShape(mdiLine, road.Width);
                var shapePs = shape.SelectMany(n => n.GetPoints()).ToListEx();
                //if (ShapeUtils.isClockWise(shapePs))
                //{
                //    shapePs.Reverse();
                //}
                var geo = GeoModelUtil.GetStretchGeometryData(new Shape(shapePs), coodMat, 0, -road.Thickness).GetBufferGeometry();
                geo.computeVertexNormals();
                geo.SetUV();
                var centerShape = road.CreateShape(mdiLine, 150);
                var geoCenter = GeoModelUtil.GetStretchGeometryData(new Shape(centerShape.SelectMany(n => n.GetPoints()).ToListEx()), coodMat, 0, 1).GetBufferGeometry();
                geoCenter.translate(0, 0, 1.1);
                geoCenter.computeVertexNormals();
                Curve2d leftCurve, rightCurve;
                if (mdiLine is Line2d line)
                {
                    leftCurve = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), -Math.PI / 2).MultiplyScalar(road.Width / 2 - 500));
                    rightCurve = line.Clone().Translate(line.Dir.RotateAround(new Vector2(), Math.PI / 2).MultiplyScalar(road.Width / 2 - 500));
                }
                else
                {
                    var arc = mdiLine as Arc2d;
                    var leftArc = arc.Clone() as Arc2d;
                    leftArc.Radius -= road.Width / 2 - 500;
                    var rightArc = arc.Clone() as Arc2d;
                    rightArc.Radius += road.Width / 2 - 500;
                    leftCurve = leftArc;
                    rightCurve = rightArc;
                }
                var leftShape = road.CreateShape(leftCurve, 150);
                var rightShape = road.CreateShape(rightCurve, 150);
                var geoLeft = GeoModelUtil.GetStretchGeometryData(new Shape(leftShape.SelectMany(n => n.GetPoints()).ToListEx()), coodMat, 0, 1).GetBufferGeometry();
                geoLeft.translate(0, 0, 1.1);
                geoLeft.computeVertexNormals();
                var geoRight = GeoModelUtil.GetStretchGeometryData(new Shape(rightShape.SelectMany(n => n.GetPoints()).ToListEx()), coodMat, 0, 1).GetBufferGeometry();
                geoRight.translate(0, 0, 1.1);
                geoRight.computeVertexNormals();
                var mesh3 = new Mesh(geoLeft, outerMat);
                results.Add(mesh3);
                var mesh4 = new Mesh(geoRight, outerMat);
                results.Add(mesh4);
                var mesh = new Mesh(geo, mat);
                results.Add(mesh);
                var mesh2 = new Mesh(geoCenter, centerMat);
                results.Add(mesh2);
            }
            foreach (var kvp in road.EmbedAssociations)
            {
                if (kvp.Host==road)
                {
                    var loop = (kvp.HostTag as List<Curve2d>);
                    if (loop.Count > 0)
                    {
                        var shapes = loop.SelectMany(n => n.GetPoints()).ToListEx();
                        if (ShapeUtils.isClockWise(shapes))
                        {
                            shapes.Reverse();
                        }
                        var geo = GeoModelUtil.GetStretchGeometryData(new Shape(shapes), coodMat, 0, -road.Thickness).GetBufferGeometry();
                        geo.computeVertexNormals();
                        geo.SetUV();
                        var mesh = new Mesh(geo, mat);
                        results.Add(mesh);
                    }
                }
            }
            results.ForEach(n=>n.TranslateZ(road.Bottom));
            return results;
        }
        
       
     }
}
