using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdEarthwork : DirectComponent
    {
        public double ElevationBottom
        {
            get
            {
                return Properties.GetValue<double>("ElevationBottom");
            }
            set
            {
                SetProps((GetPropId(nameof(ElevationBottom)), value));
            }
        }
        public double ElevationTop
        {
            get
            {
                return Properties.GetValue<double>("ElevationTop");
            }
            set
            {
                SetProps((GetPropId(nameof(ElevationTop)), value));
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


        public QdEarthwork(QdEarthworkDef lawnDef) : base(lawnDef)
        {
            Type = LayoutElementType.Earthwork;
            Outline = new Polyline2d();
        }

  
        public override Box2 GetBoundingBox()
        {
            var box = new Box2().ExpandByPoints(Outline.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
            return box;
        }


        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdEarthwork(Definition as QdEarthworkDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var lawn = (QdEarthwork)src; 
        }

     }
}
