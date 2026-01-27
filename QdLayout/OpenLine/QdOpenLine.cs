using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdOpenLine : LcLine
    {     
        public QdOpenLine( )  
        {
            Type = LayoutElementType.OpenLine; 
        }
        public const int PN_PropertyLine = 100111;

        private QdPropertyLine propertyLine;
        public QdPropertyLine PropertyLine
        {
            get
            {
                return this.propertyLine;
            }
            set
            {

                SetProps((PN_PropertyLine, value));
            }
        }
        protected override object GetProp(int propId)
        {
            if (PN_PropertyLine == propId)
            {
                return this.propertyLine;
            }
            return base.GetProp(propId);
        }

        protected override bool SetProp(int propId, object propVal)
        {
            if (!base.SetProp(propId, propVal) && propId == PN_PropertyLine)
            {
                var oldVal = propertyLine;
                this.OnPropertyChangedBefore(nameof(PropertyLine), oldVal, propVal);
                this.propertyLine = (QdPropertyLine)propVal;
                this.OnPropertyChangedAfter(nameof(PropertyLine), oldVal, propVal);
            }
            return true;

        }
        public override void OnRemoveAfter(ILcCollection parent)
        {
            if (propertyLine!=null)
            {
                PropertyLine.OpenLine = null;
            }
            base.OnRemoveAfter(parent);
        }
        public override Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict = null)
        {
            dict = base.ToDictionary(dict);

            dict.Add(nameof(this.PropertyLine), this.PropertyLine);
            return dict;
        }
        public override void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            base.FromDictionary(props);

            this.PropertyLine = props.GetLcObject<QdPropertyLine>(nameof(PropertyLine));

        }
    }
}
