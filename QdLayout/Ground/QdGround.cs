using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdGround : DirectComponent
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
        public double Thickness
        {
            get
            {
                return Properties.GetValue<double>("Thickness");
            }
            set
            {
                SetProps((GetPropId(nameof(Thickness)), value));
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
        public QdGround(QdGroundDef lawnDef) : base(lawnDef)
        {
            Type = LayoutElementType.Ground;
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
            var clone = new QdGround(Definition as QdGroundDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var lawn = (QdGround)src; 
        } 
    }
}
