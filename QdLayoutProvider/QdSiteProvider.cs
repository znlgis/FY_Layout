using LightCAD.Core.Component;
using LightCAD.MathLib.Csg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static ThreeJs4Net.ImageBitmapLoader;

namespace QdArchProvider
{
    internal static class QdSiteProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
            {
                    ("61C3A847-E445-83E6-DEB7-724A1248B04D", "场地",  场地 )
            });
        }
        private static MaterialInfo[] GetSolidMats(LcComponentDefinition definition, LcParameterSet pset, SolidCreator creator, Solid3d solid)
        {
            return new MaterialInfo[] { pset.GetValue<MaterialInfo>("Material") };
        }
        internal static Curve2dGroupCollection 场地(LcParameterSet pset, ShapeCreator creator)
        {
            var curves = new List<Curve2d>();
            var outline = pset.GetValue<Polyline2d>("Outline");
            curves = outline.Curve2ds.Clone();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curves.ToListEx() };
            return new Curve2dGroupCollection { baseCurveGrp };
        }
    }
}
