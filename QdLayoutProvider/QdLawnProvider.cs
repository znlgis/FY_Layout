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
    internal static class QdLawnProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("B11664F7-DAB9-07EB-205E-61265A5820F8", "草坪",  草坪 )
            });
            ConvertToProvider("CD8082BF-6218-37AC-11E0-72D68B763F63", nameof(GetSolid_草坪), GetSolid_草坪, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { pset.GetValue<MaterialInfo>("Material") };
        }
        internal static Curve2dGroupCollection 草坪(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var com = creator.ComIns as DirectComponent;
            var outline = com.BaseCurve as Polyline2d;
            curves = outline.Curve2ds.Clone();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            //baseCurveGrp.Color = Color.Green;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_草坪(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var outline= pset.GetValue<Polyline2d>("Outline");
            var bottom = pset.GetValue<double>("Bottom");  
            var platgeo = CreateLawn(outline );
            platgeo.translate(0,0, bottom);
             return new Solid3dCollection() { new Solid3d() {
                    Name="Lawn",
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
        private static ThreeJs4Net.BufferGeometry CreateLawn(Polyline2d polyline )
        {
            var shape = new ThreeJs4Net.Shape(polyline.GetPoints().ToListEx());
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            return GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0,-1).GetBufferGeometry();
        }

    }
}
