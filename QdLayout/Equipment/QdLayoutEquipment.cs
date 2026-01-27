using LightCAD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdLayoutEquipment : QdLayoutEquipmentInstance
    {
        public QdLayoutEquipment(QdLayoutEquipmentDef definition) : base(definition)
        {
            this.Type = LayoutElementType.LayoutEquipment;
        }
        public LcElement Stretch(Box2 box, Vector2 vector)
        {
            return null;
        }
    }
}
