
using LightCAD.RenderUtils;

namespace QdLayout
{
    public class Site3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var results = new List<Object3D>();
            var site = cptIns as QdSite;
            //var shape = new  Shape(site.Outline.Curve2ds.SelectMany(n=>n.GetPoints()).ToListEx());
            //foreach (var emb in site.EmbedAssociations)
            //{
            //    var holes = emb.Embed.GetEmbedHoles();
            //    if (holes?.Length > 0)
            //    {
            //        foreach (var hole in holes)
            //        {
            //            var holeShape = ConvertToShape(hole);
            //            shape.holes.Push(holeShape);
            //        }
            //        continue;
            //    }
            //}
            //var coodMat = new Matrix4();
            //coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            //var geo = GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, -1).GetBufferGeometry();
            //geo.translate(0, 0, -1.1);
            //geo.computeVertexNormals();
            //geo.SetUV(); 
            //(cptIns as LcElement).Parent.ObjectChangedAfter += Parent_ObjectChangedAfter;
            //(cptIns as LcElement).Parent.PropertyChangedAfter += Parent_PropertyChangedAfter;  
            var top = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), site.Outline.Curve2ds.Select(n => n.ToCurve3d().Translate(0, 0, - 1.1)).ToList());
            var btm = new PlanarSurface3d(new Plane(new Vector3(0, 0, -1)), site.Outline.Curve2ds.Select(n => n.ToCurve3d().Translate(0, 0, -site.Thickness)).ToList());

            var extrud = new Extrude3d(btm, new Vector3(0, 0, 1), site.Thickness); 
            extrud.CreateTopoModel();
            var surfaces = extrud.Surfaces.ToList();
            surfaces.RemoveRange(0, 2);
            foreach (var emb in site.EmbedAssociations)
            {
                var holes = emb.Embed.GetEmbedHoles();
                if (holes?.Length > 0)
                {
                    foreach (var hole in holes)
                    {
                        top.Profiles[0].InnerLoops.Add(hole.OutLoop.Curve2ds.Select(n => n.ToCurve3d().Translate(0, 0, -1.1)).ToList());
                    }
                    continue;
                }
            }
            
            surfaces.Add(top);
            surfaces.Add(btm);
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
            var geo = new BufferGeometry();
            geo.setAttribute("position", new BufferAttribute(posArr.ToArray(), 3, false));
            geo.setIndex(new BufferAttribute(idxArr.ToArray(), 1));
            geo.computeVertexNormals();
            geo.SetUV();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(site.Material);
            var mesh = new Mesh(geo, mat);
            results.Add(mesh);
            results.ForEach(n => n.TranslateZ(site.Bottom));
            return results;
        }

        private void Parent_PropertyChangedAfter(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Parent_ObjectChangedAfter(object? sender, ObjectChangedEventArgs e)
        {
            var parent = (e.Collection as IElementSet);
            var comins = (e.Target as IComponentInstance);
            if (comins.Properties.Definition.Contains("场地布置"))
            {
                //find Site
                // site change
                //如果没有场地元素  拆离事件
                //parent  as lceddddd -=
            }
        }

        public Shape ConvertToShape(Profile2 profile)
        {
            var shape = new Shape();

            var points = new ListEx<Vector2>();
            foreach (var curve in profile.OutLoop.Curve2ds)
            {
                var ps = curve.GetPoints(1).SkipLast(1).ToListEx();
                points.Push(ps.ToArray());
            }
            shape.setFromPoints(points);

            return shape;
        }
     }
}
