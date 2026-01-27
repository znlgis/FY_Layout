 
namespace QdLayout
{
    public class Ground3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var earthwork = cptIns as QdGround; 
            var outline = earthwork.Outline;
            var thickness = earthwork.Thickness;
            var platgeo = CreateEarthwork(outline, thickness);
            platgeo.translate(0, 0, earthwork.Bottom);
            platgeo.computeVertexNormals();
            platgeo.SetUV();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GetMaterial(MaterialManager.RoadUuid));
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
