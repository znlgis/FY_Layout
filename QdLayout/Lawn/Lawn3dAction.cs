 
namespace QdLayout
{
    public class Lawn3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var earthwork = cptIns as QdLawn;

            var outline = earthwork.Outline;
            var bottom = earthwork.Bottom; 
            var platgeo = CreateEarthwork(outline);
            platgeo.translate(0, 0, bottom);
            platgeo.computeVertexNormals();
            platgeo.SetUV();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GetMaterial(MaterialManager.LawnUuid));
            var mesh = new Mesh(platgeo, mat);
            return new List<Object3D> { mesh };
        }
        private static ThreeJs4Net.BufferGeometry CreateEarthwork(Polyline2d polyline)
        {
            var ps = polyline.GetPoints().ToListEx();
            if (ShapeUtils.isClockWise(ps))
            {
                ps.Reverse();
            }
            var shape = new ThreeJs4Net.Shape(ps);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            return GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, -1).GetBufferGeometry();
        }
    }
}
