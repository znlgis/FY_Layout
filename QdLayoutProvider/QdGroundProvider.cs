using LightCAD.Core.Component;
using LightCAD.MathLib.Csg;
using LightCAD.RenderUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static ThreeJs4Net.ImageBitmapLoader;

namespace QdArchProvider
{
    internal static class QdGroundProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("61D0A892-F105-4821-A810-DCABF54A1517", "硬化地面",  硬化地面 )
            });
            ConvertToProvider("5CFC3B1F-80D7-A51C-5520-C4DD88FD7D4C", nameof(GetSolid_硬化地面), GetSolid_硬化地面, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { pset.GetValue<MaterialInfo>("Material") };
        }
        internal static Curve2dGroupCollection 硬化地面(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var com = creator.ComIns as DirectComponent;
            var outline = com.BaseCurve as Polyline2d;
            curves = outline.Curve2ds.Clone();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            //baseCurveGrp.Color = Color.Green;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_硬化地面(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var outline= pset.GetValue<Polyline2d>("Outline");
            var bottom = pset.GetValue<double>("Bottom");
            var thickness = pset.GetValue<double>("Thickness");
            var platgeo = CreateGround(outline, thickness);
            platgeo.translate(0,0, bottom );
             return new Solid3dCollection() { new Solid3d() {
                    Name="Ground",
                    Geometry = new LightCAD.MathLib.GeometryData()
                    {
                        Verteics = platgeo.attributes.position.array,
                        Indics = platgeo.index.intArray,
                        Groups = new GeometryGroup[1]
                        {
                            new GeometryGroup{ Name = "Geometry", Start = 0, Count = platgeo.index.intArray.Length, MaterialIndex = 0 },
                        }
                    } 
             }};
        }
        private static ThreeJs4Net.BufferGeometry CreateGround(Polyline2d polyline,double thickness)
        {
            var ps = polyline.GetPoints().ToListEx();
            if (ShapeUtils.isClockWise(ps))
            {
                ps.Reverse();
            }
            var shape = new ThreeJs4Net.Shape(ps);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, -1));
            return GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, thickness).GetBufferGeometry();
        }

    }
}
