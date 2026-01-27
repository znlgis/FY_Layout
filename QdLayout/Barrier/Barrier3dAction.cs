using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    internal class Barrier3dAction : ComponentInstance3dAction
    {
        public override List<Object3D> Render(IComponentInstance cptIns)
        {
            var result = new List<Object3D>() { };
            QdBarrier qdBarrier = cptIns as QdBarrier;
            var BarrierDef = qdBarrier.Definition as QdBarrierDef;
            var BarrierLine = qdBarrier.BasePolyline;
            double width = qdBarrier.BarrierWidth;
            double height = qdBarrier.BarrierHeight;

            foreach (var ele in BarrierLine.Curve2ds)
            {
                Vector2 start = new Vector2();
                Vector2 end = new Vector2();
                if (ele.Type == Curve2dType.Line2d)
                {
                    start = (ele as Line2d).Start;
                    end = (ele as Line2d).End;
                    result.AddRange(MakeBarrierLine(start,end,width,height));
                }
            }

            return result;
        }

        private Extrude3d MakeCube(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, double height)
        {
            var result = new List<Object3D>() { };
            var outloop = new List<Curve3d>();

            outloop.Add(new Line3d(p1.ToVector3(), p2.ToVector3()));
            outloop.Add(new Line3d(p2.ToVector3(), p3.ToVector3()));
            outloop.Add(new Line3d(p3.ToVector3(), p4.ToVector3()));
            outloop.Add(new Line3d(p4.ToVector3(), p1.ToVector3()));
            var ctf = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), outloop);
            return new Extrude3d(ctf, new Vector3(0, 0, 1), height);
        }

        private Mesh MakeSolid3d(Extrude3d extrude)
        {
            extrude.CreateMesh();
            var buffergeo = new BufferGeometry();
            buffergeo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics, 3, false));
            buffergeo.setIndex(new BufferAttribute(extrude.Geometry.Indics, 1));
            buffergeo.computeVertexNormals();
            buffergeo.SetUV();
            return new Mesh(buffergeo, LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.Metal1Uuid));
        }

        private List<Object3D> MakeBarrierSingle(Vector2 sp, Vector2 ep, double Width, double Height)
        {
            List<Object3D> result = new List<Object3D>();
            var Angle = Vector2.GetAngle(sp, ep);
            Vector2 direction = (ep - sp).Normalize();
            Matrix3 matrix3 = Matrix3.RotateInRadian(Angle, sp);

            double halfCol = 20;// half
            double fullCol = halfCol * 2;
            double thickness = 10;// half
            double halfleg = 500;
            double fullleg = halfleg* 2;
            double legthick = 10;

            // wall
            {
                Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y + thickness));
                Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y + thickness));
                Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y - thickness));
                Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y - thickness));
                var wall = MakeCube(p1, p2, p3, p4, Height * 0.75);
                var solid = MakeSolid3d(wall);
                solid.position.Z += Height * 0.25;
                result.Add(solid);
            }

            // col start
            {
                Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y + halfCol));
                Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + fullCol, sp.Y + halfCol));
                Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + fullCol, sp.Y - halfCol));
                Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y - halfCol));
                var col1 = MakeCube(p1, p2, p3, p4, Height);
                var colSolid1 = MakeSolid3d(col1);
                result.Add(colSolid1);
            }

            // col end
            { 
                Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y + halfCol));
                Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + Width - fullCol, sp.Y + halfCol));
                Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + Width - fullCol, sp.Y - halfCol));
                Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y - halfCol));
                var col2 = MakeCube(p1, p2, p3, p4, Height);
                var colSolid2 = MakeSolid3d(col2);
                result.Add(colSolid2);
            }

            // leg start
            {
                Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y + halfleg));
                Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + fullCol, sp.Y + halfleg));
                Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + fullCol, sp.Y - halfleg));
                Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X, sp.Y - halfleg));
                var leg1 = MakeCube(p1, p2, p3, p4, legthick);
                var legSolid1 = MakeSolid3d(leg1);
                result.Add(legSolid1);
            }

            // leg end
            {
                Vector2 p1 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y + halfleg));
                Vector2 p2 = matrix3.MultiplyPoint(new Vector2(sp.X + Width - fullCol, sp.Y + halfleg));
                Vector2 p3 = matrix3.MultiplyPoint(new Vector2(sp.X + Width - fullCol, sp.Y - halfleg));
                Vector2 p4 = matrix3.MultiplyPoint(new Vector2(sp.X + Width, sp.Y - halfleg));
                var leg2 = MakeCube(p1, p2, p3, p4, legthick);
                var legSolid2 = MakeSolid3d(leg2);
                result.Add(legSolid2);
            }

            return result;
        }

        private List<Object3D> MakeBarrierLine(Vector2 sp, Vector2 ep, double Width, double Height, double interval = 50)
        {
            List<Object3D> result = new List<Object3D>();

            double dist = Vector2.Distance(sp, ep);
            var Angle = Vector2.GetAngle(sp, ep);
            Matrix3 matrix3 = Matrix3.RotateInRadian(Angle, sp);
            Vector2 direction = (ep - sp).Normalize();
            Vector2 startp = new Vector2(sp.X, sp.Y); 
            Vector2 endp = new Vector2(sp.X, sp.Y);
            int num = (int)Math.Ceiling(dist / (Width + interval));
            for(int i = 0; i < num; i++)
            {
                endp = startp + direction * Width;
                result.AddRange(MakeBarrierSingle(startp, endp, Width, Height));
                startp = endp + direction * interval;
            }

            return result;
        }
    }
}
