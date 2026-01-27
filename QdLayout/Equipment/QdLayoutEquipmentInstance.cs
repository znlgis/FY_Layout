using LightCAD.Core;
using LightCAD.MathLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdLayoutEquipmentInstance : LcComponentInstance
    {
        public QdLayoutEquipmentInstance(LcComponentDefinition definition) : base(definition)
        {
            this.Type = LayoutElementType.LayoutEquipment;
        }
        public virtual (double, double, double, double) GetRadiusWidthHeightLength()
        {
            return (0, 0, 0, 0);
        }
    }
}
