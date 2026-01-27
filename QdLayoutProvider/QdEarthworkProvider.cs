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
    internal static class QdEarthworkProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("96DEEECF-AA62-E66B-AD3F-2B6A0C95209B", "土方回填",  土方回填 )
            });
            ConvertToProvider("975D7536-2598-B62B-A2C9-A603D5590907", nameof(GetSolid_土方回填), GetSolid_土方回填, GetSolidMats);
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { MaterialManager.GetMaterial(MaterialManager.Metal1Uuid) };

        }
        internal static Curve2dGroupCollection 土方回填(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var com = creator.ComIns as DirectComponent;
            var outline = com.BaseCurve as Polyline2d;
            curves = outline.Curve2ds.Clone();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            //baseCurveGrp.Color = Color.Green;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
        private static Solid3dCollection GetSolid_土方回填(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator)
        {
            var outline= pset.GetValue<Polyline2d>("Outline");
            var bottom = pset.GetValue<double>("ElevationBottom");
            var top = pset.GetValue<double>("ElevationTop");
            var platgeo = CreateEarthwork(outline, top-bottom);
            platgeo.translate(0,0, top);
             return new Solid3dCollection() { new Solid3d() {
                    Name="Earthwork",
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
        private static ThreeJs4Net.BufferGeometry CreateEarthwork(Polyline2d polyline,double height )
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
