 
namespace QdLayout
{
    public class Berm3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance comIns)
        {
            Solid3dCollection solids = comIns.GetSolids();
            if (solids == null)
            {
                return new List<Object3D>();
            }

            List<Object3D> list = new List<Object3D>();
            foreach (Solid3d item4 in solids)
            {
                GeometryData geometry = item4.Geometry;
                BufferGeometry bufferGeometry = new BufferGeometry();
                bufferGeometry.setAttribute("position", new BufferAttribute(geometry.Verteics, 3));
                bufferGeometry.setIndex(new BufferAttribute(geometry.Indics, 1));
                bufferGeometry.groups.AddRange(geometry.Groups);
                bufferGeometry.computeVertexNormals();
                bufferGeometry.SetUV();
                Mesh mesh = new Mesh(bufferGeometry, comIns.GetSolidMaterials(item4)?.Select((MaterialInfo m) =>LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(m)).ToArray());
                list.Add(mesh);
                mesh.name = item4.Name;
                if (item4.Edge != null)
                {
                    GeometryData edge = item4.Edge;
                    BufferGeometry bufferGeometry2 = new BufferGeometry();
                    bufferGeometry2.setAttribute("position", new BufferAttribute(edge.Verteics, 3));
                    bufferGeometry2.setIndex(new BufferAttribute(edge.Indics, 1));
                    LineSegments item = new LineSegments(bufferGeometry2, new LineBasicMaterial
                    {
                        color = new Color(0)
                    });
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
