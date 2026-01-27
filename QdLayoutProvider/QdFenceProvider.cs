using LightCAD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayoutProvider
{
    internal static class QdFenceProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
                {
                    ("5A9EC5E6-09BF-4DB3-9AD2-DD0DA74F099A", "围墙",  围墙 )
                });

        }
        internal static Curve2dGroupCollection 围墙(LcParameterSet properties, ShapeCreator creator)
        {
            var curve2ds = properties.GetValue<Curve2d[]>("Lines").ToList();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curve2ds.Clone().ToListEx() };
            //baseCurveGrp.Color = Color.Blue;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
    }
}
