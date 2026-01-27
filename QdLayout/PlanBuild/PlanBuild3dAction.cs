using LightCAD.MathLib;
using QdLayout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;

namespace QdLayout
{
    public class PlanBuild3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance comIns)
        {
            List<Object3D> object3Ds = new List<Object3D>();

            QdPlanBuild qdPlanBuild = comIns as QdPlanBuild;
            Polyline2d basePolyline = qdPlanBuild.BasePolyline;

            List<Curve3d> curve3dList = new List<Curve3d>();
            for (int i = 0; i < basePolyline.Curve2ds.Count; i++)
            {
                if (basePolyline.Curve2ds[i] is Line2d)
                {
                    Line2d line2d = basePolyline.Curve2ds[i] as Line2d;
                    curve3dList.Add(new Line3d(line2d.Start.ToVector3(), line2d.End.ToVector3()));
                }
                else if (basePolyline.Curve2ds[i] is Arc2d)
                {
                    Arc2d arc2d = basePolyline.Curve2ds[i] as Arc2d;
                    curve3dList.Add(new Arc3d(arc2d.Center.ToVector3(), arc2d.Radius, arc2d.StartAngle, arc2d.EndAngle, arc2d.IsClockwise));
                }
            }

            //单层楼
            PlanarSurface3d planarSurface3d = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), curve3dList);
            Extrude3d extrude = new Extrude3d(planarSurface3d, new Vector3(0, 0, 1), qdPlanBuild.StoreyHeight);
            extrude.CreateMesh();
            
            BufferGeometry buffergeo = new BufferGeometry();
            buffergeo.attributes.uv=new BufferAttribute(extrude.Geometry.Uvs,2);
            buffergeo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics, 3, false));
            buffergeo.setIndex(new BufferAttribute(extrude.Geometry.Indics, 1));
            buffergeo.groups = extrude.Geometry.Groups.ToListEx();
            buffergeo.computeVertexNormals();

            Material[] materials = new Material[basePolyline.Curve2ds.Count + 2];
            materials[0] = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.ConcreteUuid);
            materials[1] = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.ConcreteUuid);

            for (int i = 2; i < basePolyline.Curve2ds.Count + 2; i++)
            {         
                MaterialInfo materialInfo = new MaterialInfo() { Color = new Color(0xffffff), Opcity = 1, Name = "框架结构" };
                materialInfo.Map = new TextureInfo()
                {
                    Map = @".\Resources\Texture\FrameStructure.jpg",
                    Repeat = ((int)((basePolyline.Curve2ds[i - 2] as Line2d).Length / 3000), 1),
                    WrapS = WrapType.RepeatWrapping,
                    WrapT = WrapType.ClampToEdgeWrapping,
                };

                materials[i] = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(materialInfo);
            }

                
           
            Mesh bottomStoreyMesh = new Mesh(buffergeo, materials);

            //循环添加多层楼
            for (int i = 0; i < qdPlanBuild.GroundUpStorey; i++)
            {
                object3Ds.Add(bottomStoreyMesh.Clone().TranslateZ(i * qdPlanBuild.StoreyHeight));
            }
            //屋顶
            PlanarSurface3d planarSurface3dRoof = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), curve3dList);
            Extrude3d extrudeRoof = new Extrude3d(planarSurface3dRoof, new Vector3(0, 0, 1), 3);
            extrudeRoof.CreateMesh();

            BufferGeometry buffergeoRoof = new BufferGeometry();
            buffergeoRoof.setAttribute("position", new BufferAttribute(extrudeRoof.Geometry.Verteics, 3, false));
            buffergeoRoof.setIndex(new BufferAttribute(extrudeRoof.Geometry.Indics, 1));
            buffergeoRoof.computeVertexNormals();
            buffergeoRoof.SetUV();

            Mesh bottomStoreyMeshRoof = new Mesh(buffergeoRoof,
                LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.ConcreteUuid));     
            object3Ds.Add(bottomStoreyMeshRoof);
            object3Ds.Add(bottomStoreyMeshRoof.Clone().TranslateZ(qdPlanBuild.GroundUpStorey * qdPlanBuild.StoreyHeight));


            return object3Ds;
        }
    }
}
