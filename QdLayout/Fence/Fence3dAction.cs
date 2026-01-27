
using netDxf.Entities;
using QdLayout.Fence;
using Svg;
using System;
using System.Drawing.Drawing2D;
using System.Security;
using ThreeJs4Net;


namespace QdLayout
{
    public class Fence3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var result = new List<Object3D>() { };
            QdFence qdFence = cptIns as QdFence;
            var FenceDef = qdFence.Definition as QdFenceDef;
            var FenceLine = qdFence.BasePolyline;
            List<Line2d> lastColLines = new List<Line2d>();

        

            //const texture = new THREE.TextureLoader().load("textures/water.jpg");
            //texture.wrapS = THREE.RepeatWrapping;
            //texture.wrapT = THREE.RepeatWrapping;
            //texture.repeat.set(4, 4);
            //Texture textureInfo = new Texture();
            ////    textureInfo.center = new Vector2(0.5f, 0.5f);
            //textureInfo.wrapS = 1;
            //textureInfo.wrapS = 1;
            //textureInfo.repeat = new Vector2(1, 1);
            ;
            //ThreeJs4Net.Image imge = new ThreeJs4Net.Image();
            //imge.src = @".\Resources\Texture\FenceTexture.jpg";
            //textureInfo.mipmaps = new ListEx<ThreeJs4Net.Image>();
            //textureInfo.mipmaps.Add(imge);
   

     


            Vector2 lastEndP = new Vector2();
            foreach (var ele in FenceLine.Curve2ds)
            {
                Vector2 start = new Vector2();
                Vector2 end = new Vector2();
                if (ele.Type == Curve2dType.Line2d)
                {
                    start = (ele as Line2d).Start;
                    end = (ele as Line2d).End;


                    var Angle = Vector2.GetAngle(start, end);
                    Matrix3 matrix3 = Matrix3.RotateInRadian(Angle, start);

                    Vector2 p1 = matrix3.MultiplyPoint(new Vector2(start.X, start.Y - qdFence.FenceWidth / 2));
                    Vector2 p4 = matrix3.MultiplyPoint(new Vector2(start.X, start.Y + qdFence.FenceWidth / 2));

                    Vector2 p2 = p1 + (end - start);
                    Vector2 p3 = p4 + (end - start);
                    result.AddRange(Drawex(p1, p2, p3, p4, qdFence.FenceHeight, qdFence.FenceColor));

                }
            }
            bool firstLine = true;
            foreach (var ele in FenceLine.Curve2ds)
            {
                Vector2 start = new Vector2();
                Vector2 end = new Vector2();
                if (ele.Type == Curve2dType.Line2d)
                {
                    start = (ele as Line2d).Start;
                    end = (ele as Line2d).End;

                    if (firstLine)
                    {
                        Vector2 direction = (end - start).Normalize();
                        lastEndP = start + direction * qdFence.FenceWidth;

                    }
                    else
                    {
                        lastEndP = null;
                        foreach (var item in lastColLines)
                        {
                            lastEndP = Intersect2d.LineWithLine((ele as Line2d), item);
                            if (lastEndP != null)
                            {
                                break;

                            }
                        }
                        if (lastEndP == null)
                        {
                            Vector2 direction = (end - start).Normalize();
                            lastEndP = start + direction * qdFence.FenceWidth;
                        }
                    }

                    result.AddRange(DrawFence(start, end, qdFence.FenceWidth, firstLine, qdFence.FenceHeight, qdFence.FenceColumnHeight +3, qdFence.FenceColor, qdFence.FenceColumnColor, lastEndP, qdFence.FenceColumnInterval, out lastColLines));
                    firstLine = false;
                }
            }
            return result;
        }
        public Object3D DrawFenceTexture(Vector3 sp, Vector3 ep,double height )
        {
            var outloop = new List<Curve3d>();
            MaterialInfo materialInfo = new MaterialInfo();
            materialInfo.RepeatTexture = @".\Resources\Texture\FenceTexture.jpg";
            //    materialInfo.Texture
              Material material = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(materialInfo);

            outloop.Add(new Line3d(sp, ep));
            outloop.Add(new Line3d(ep, new Vector3(ep.X,ep.Y+1, 0)));
            outloop.Add(new Line3d(new Vector3(ep.X, ep.Y + 1, 0), new Vector3(sp.X, sp.Y + 1, 0)));
            outloop.Add(new Line3d(new Vector3(sp.X, sp.Y + 1, 0), sp));
            var ctf = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), outloop);
            var extrude = new Extrude3d(ctf, new Vector3(0, 0, 1), height);
            extrude.CreateMesh();
            var buffergeo = new BufferGeometry();
            buffergeo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics, 3, false));
            buffergeo.setIndex(new BufferAttribute(extrude.Geometry.Indics, 1));
            buffergeo.computeVertexNormals();
            buffergeo.SetUV();
           var wallConMesh = new ThreeJs4Net.Mesh(buffergeo, material);
            return wallConMesh;
            //  result.Add(wallConMesh);
        }
        public Object3D[] Drawex(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, double height, string color)
        {
            var result = new List<Object3D>() { };
            var outloop = new List<Curve3d>();

            outloop.Add(new Line3d(p1.ToVector3(), p2.ToVector3()));
            outloop.Add(new Line3d(p2.ToVector3(), p3.ToVector3()));
            outloop.Add(new Line3d(p3.ToVector3(), p4.ToVector3()));
            outloop.Add(new Line3d(p4.ToVector3(), p1.ToVector3()));
            var ctf = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), outloop);
            var extrude = new Extrude3d(ctf, new Vector3(0, 0, 1), height);
            //extrude.Edge
             extrude.CreateMesh();
            var buffergeo = new BufferGeometry();
            buffergeo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics, 3, false));
            buffergeo.setIndex(new BufferAttribute(extrude.Geometry.Indics, 1));
            buffergeo.computeVertexNormals();
            buffergeo.SetUV();
          //  var material = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.ConcreteUuid).clone();
            MaterialInfo materialInfo = new MaterialInfo();
            materialInfo.Color.Set(Convert.ToInt32(color, 16));
             //  material.color =FenceSet.GetColorByUint(Convert.ToUInt32(color));
             //    materialInfo.Texture
             Material material = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(materialInfo);
            // Material material = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(materialInfo); 
            //FenceSet.GetColorByUint(Convert.ToUInt32(color));
            var wallConMesh = new ThreeJs4Net.Mesh(buffergeo, material);


           // var mesh = new Mesh(geo, materials);
            var lineGeoData = extrude.Edge;
            var lineGeo = new BufferGeometry();
            lineGeo.setAttribute("position", new BufferAttribute(lineGeoData.Verteics, 3, false));
            lineGeo.setIndex(new BufferAttribute(lineGeoData.Indics, 1));
 
            var line = new LineSegments(lineGeo, new LineBasicMaterial { color = new Color(0x00000000) });

            return new Object3D[] { wallConMesh, line };
         //   return wallConMesh;
        }

        public List<Object3D> DrawFence(Vector2 sp, Vector2 ep, double FenceWidth, bool startDraw, double FenceHeight, double FenceColumnHeight, string FenceColor, string FenceColumnColor, Vector2 lastEndp, double Interval, out List<Line2d> line2Ds)
        {
            line2Ds = new List<Line2d>();
            var result = new List<Object3D>();
            var Angle = Vector2.GetAngle(sp, ep);
            Matrix3 matrix3 = Matrix3.RotateInRadian(Angle, sp);

            //第一个柱子
            Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X - Convert.ToDouble(FenceWidth), sp.Y - Convert.ToDouble(FenceWidth)));
            Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + Convert.ToDouble(FenceWidth), sp.Y - Convert.ToDouble(FenceWidth)));
            Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + Convert.ToDouble(FenceWidth), sp.Y + Convert.ToDouble(FenceWidth)));
            Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X - Convert.ToDouble(FenceWidth), sp.Y + Convert.ToDouble(FenceWidth)));

            Vector2 Texture1 = matrix3.MultiplyPoint(new Vector2(sp.X - Convert.ToDouble(FenceWidth)/2, sp.Y - Convert.ToDouble(FenceWidth)/2));
            Vector2 Texture2 = matrix3.MultiplyPoint(new Vector2(sp.X + Convert.ToDouble(FenceWidth)/2, sp.Y - Convert.ToDouble(FenceWidth)/2));

            Vector2 direction = (ep - sp).Normalize();
            Vector2 NextstartP = lastEndp.Clone();
            if (startDraw)
            {
                result.AddRange(Drawex(p1, p2, p3, p4, FenceColumnHeight, FenceColumnColor));
            }
            //------[]------[*3
            while (((ep - NextstartP).Length() >= (2 * Interval + Convert.ToDouble(FenceWidth) * 3)))
            {
                Vector2 NextColCenter = NextstartP + direction * Interval;
                Vector2 NextendP = NextColCenter + direction * Convert.ToDouble(FenceWidth);

                Vector2 NextColEnd = NextendP + direction * Convert.ToDouble(FenceWidth) * 2;

                p1 = p1 + (NextColEnd - NextstartP);
                p2 = p2 + (NextColEnd - NextstartP);
                p3 = p3 + (NextColEnd - NextstartP);
                p4 = p4 + (NextColEnd - NextstartP);


                result.AddRange(Drawex(p1, p2, p3, p4, FenceColumnHeight, FenceColumnColor));
             //   result.Add(DrawFenceTexture(new Vector3(Texture1.X+ NextstartP.X, Texture1.Y + NextstartP.Y), new Vector3(Texture2.X + NextColEnd.X, Texture2.Y + NextColEnd.Y),1000));
                NextstartP = NextColEnd.Clone();
            }
            if ((ep - NextstartP).Length() > Interval + Convert.ToDouble(FenceWidth))
            {
                var centerLenth = ((ep - NextstartP).Length() - Convert.ToDouble(FenceWidth)) / 2;
                Vector2 NextColCenter = NextstartP + direction * centerLenth;
                Vector2 NextendP = NextColCenter + direction * Convert.ToDouble(FenceWidth);


                Vector2 NextColEnd = NextendP + direction * Convert.ToDouble(FenceWidth) * 2;

                p1 = p1 + (NextColEnd - NextstartP);
                p2 = p2 + (NextColEnd - NextstartP);
                p3 = p3 + (NextColEnd - NextstartP);
                p4 = p4 + (NextColEnd - NextstartP);
                result.AddRange(Drawex(p1, p2, p3, p4, FenceColumnHeight, FenceColumnColor));
                NextstartP = NextColEnd.Clone();
            }

            Vector2 endP = ep - direction * Convert.ToDouble(FenceWidth);

            p1 = p1 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p2 = p2 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p3 = p3 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            p4 = p4 + (ep + direction * Convert.ToDouble(FenceWidth) - NextstartP);
            result.AddRange(Drawex(p1, p2, p3, p4, FenceColumnHeight, FenceColumnColor));

            line2Ds.Add(new Line2d(p1, p2));
            line2Ds.Add(new Line2d(p2, p3));
            line2Ds.Add(new Line2d(p3, p4));
            line2Ds.Add(new Line2d(p4, p1));
            return result;

        }
    }
}
