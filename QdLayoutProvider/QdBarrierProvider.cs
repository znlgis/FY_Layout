using LightCAD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayoutProvider
{
    internal static class QdBarrierProvider
    {
        internal static void RegistProviders()
        {
            ConvertToProviders(new List<(string uuid, string name, CreateShape creator)>
                {
                    ("6A8E5173-8D53-4542-8272-5B1A6680AFD2", "防护栏杆",  防护栏杆 )
                });

        }
        internal static Curve2dGroupCollection 防护栏杆(LcParameterSet properties, ShapeCreator creator)
        {
            var curve2ds = properties.GetValue<Curve2d[]>("Lines").ToList();
            var baseCurveGrp = new Curve2dGroup { Curve2ds = curve2ds.Clone().ToListEx() };
            //baseCurveGrp.Color = Color.Blue;
            return new Curve2dGroupCollection { baseCurveGrp };
        }
    }
}
