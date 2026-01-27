using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    [ComDefCategory($"场布施工设计.建构筑物")]
    [ComClass("板房")]
    public class PlateBuildGroupDef : LcComponentDefinition
    {
        public PlateBuildGroupDef() : base()
        {
            //this.ShapeProviders = new Dictionary<string, ShapeProvider>()
            //{
            //    { "板房", new ShapeProvider { Uuid = "34D5B806-B9C4-4D95-910E-DBF2DDD60895", Name = "", UseType = "", CreateShape = GetShape_PlateBuliding } }

            //};

        }
        //private static Curve2dGroupCollection GetShape_PlateBuliding(LcParameterSet properties, ShapeCreator creator)
        //{
        //    creator.DrawLine(new Vector2(0, 0), new Vector2(0, 0));
        //    return new Curve2dGroupCollection() { new Curve2dGroup() { Curve2ds = creator.Curves } };
        //}
    }
}
