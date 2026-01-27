using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdLawn : DirectComponent
    {
        public double Bottom
        {
            get
            {
                return Properties.GetValue<double>("Bottom");
            }
            set
            {
                SetProps((GetPropId(nameof(Bottom)), value));
            }
        }
 
        public MaterialInfo Material
        {
            get
            {
                return Properties.GetValue<MaterialInfo>("Material");
            }
            set
            {
                SetProps((GetPropId(nameof(Material)), value));

            }
        }
        public Polyline2d Outline
        {
            get
            {
                return this.BaseCurve as Polyline2d;
            }
            set
            {
                this.BaseCurve = value;
            }
        }


        public QdLawn(QdLawnDef lawnDef) : base(lawnDef)
        {
            Type = LayoutElementType.Lawn;
            Outline = new Polyline2d();
        }

  
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(GetShapes()[0].Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        } 
        
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdLawn(Definition as QdLawnDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var lawn = (QdLawn)src; 
        } 
    }
}
