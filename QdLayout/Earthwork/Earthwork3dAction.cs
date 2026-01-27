 
namespace QdLayout
{
    public class Earthwork3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var earthwork = cptIns as QdEarthwork;

            var outline = earthwork.Outline;
            var bottom = earthwork.ElevationBottom;
            var top = earthwork.ElevationTop;
            var platgeo = CreateEarthwork(outline, top - bottom);
            platgeo.translate(0, 0, top);
            platgeo.computeVertexNormals();
            platgeo.SetUV();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GetMaterial(MaterialManager.EarthworkUuid));
            var mesh = new Mesh(platgeo, mat);
            return new List<Object3D> { mesh };
        }
        private static ThreeJs4Net.BufferGeometry CreateEarthwork(Polyline2d polyline, double height)
        {
            var ps = polyline.GetPoints().ToListEx();
            if (ShapeUtils.isClockWise(ps))
            {
                ps.Reverse();
            }
            var shape = new ThreeJs4Net.Shape(ps);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            return GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, -height).GetBufferGeometry();
        }
    }
}
