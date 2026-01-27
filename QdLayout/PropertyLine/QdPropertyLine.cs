using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdPropertyLine : LcPolyLine
    {     
        public QdPropertyLine( )  
        {
            Type = LayoutElementType.PropertyLine; 
        }
        public const int PN_OpenLine = 100111;

        private QdOpenLine openLine;
        public QdOpenLine OpenLine  
        {
            get
            {
                return this.openLine;
            }
            set
            {

                SetProps((PN_OpenLine, value));
            }
        }
        protected override object GetProp(int propId)
        {
            if (PN_OpenLine == propId)
            {
                return this.openLine;
            }
            return base.GetProp(propId);
        }

        protected override bool SetProp(int propId, object propVal)
        {
            if (!base.SetProp(propId, propVal)&&propId==PN_OpenLine)
            {
                var oldVal = openLine;
                this.OnPropertyChangedBefore(nameof(OpenLine), oldVal, propVal);
                this.openLine = (QdOpenLine)propVal;
                this.OnPropertyChangedAfter(nameof(OpenLine), oldVal, propVal);
            }
            return true;

        }
        public override void OnRemoveAfter(ILcCollection parent)
        {
            if (openLine != null)
            {
                OpenLine.PropertyLine = null;
            }
            base.OnRemoveAfter(parent);
        }
        public override Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict = null)
        {
            dict = base.ToDictionary(dict);

            dict.Add(nameof(this.OpenLine), this.OpenLine);
            return dict;
        }
        public override void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            base.FromDictionary(props);

            this.OpenLine = props.GetLcObject<QdOpenLine>(nameof(OpenLine));

        }
    }
}
