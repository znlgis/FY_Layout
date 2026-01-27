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
    internal static class QdHardenProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("B7B7891D-7D99-456F-C1A1-C162DEFD17ED", "路面硬化",  路面硬化 )
            });
            ConvertToProvider("92F5F90F-E5A8-BAE0-C366-1F4D69DD5FBF", nameof(GetSolid_路面硬化), GetSolid_路面硬化, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { pset.GetValue<MaterialInfo>("Material") };
        }
        internal static Curve2dGroupCollection 路面硬化(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var outline = pset.GetValue<Polyline2d>("Outline");
            curves = outline.Curve2ds.Clone();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            //baseCurveGrp.Color = Color.Green;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_路面硬化(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var outline= pset.GetValue<Polyline2d>("Outline");
            var bottom = pset.GetValue<double>("Bottom");
            var thickness = pset.GetValue<double>("Thickness");
            var platgeo = CreateOldBuilding(outline, thickness);
            platgeo.translate(0,0, bottom );
             return new Solid3dCollection() { new Solid3d() {
                    Name="Harden",
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
        private static ThreeJs4Net.BufferGeometry CreateOldBuilding(Polyline2d polyline,double thickness)
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
