using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QdLayout
{
    public class QdFoundationPit : DirectComponent, IEmbed  
    {
        /// <summary>
        /// 基底样式
        /// </summary>
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
        /// <summary>
        /// 垂直基坑壁支护样式
        /// </summary>
        public MaterialInfo VerticalMaterial
        {
            get
            {
                return Properties.GetValue<MaterialInfo>("VerticalMaterial");
            }
            set
            {
                SetProps((GetPropId(nameof(VerticalMaterial)), value));
            }
        }
        /// <summary>
        /// 放坡基坑壁支护样式
        /// </summary>
        public MaterialInfo InclineMaterial
        {
            get
            {
                return Properties.GetValue<MaterialInfo>("InclineMaterial");
            }
            set
            {
                SetProps((GetPropId(nameof(InclineMaterial)), value));
            }
        }
        /// <summary>
        /// 放坡系数
        /// </summary>
        public double Factor
        {
            get
            {
                return Properties.GetValue<double>("Factor");
            }
            set
            {
                SetProps((GetPropId(nameof(Factor)), value));
            }
        }
        /// <summary>
        /// 土方顶绝对标高（m）
        /// </summary>
        public double Elevation
        {
            get
            {
                return Properties.GetValue<double>("Elevation");
            }
            set
            {
                SetProps((GetPropId(nameof(Elevation)), value));
            }
        }
        /// <summary>
        /// 土方底绝对标高（m）
        /// </summary>
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
        /// <summary>
        /// 放坡方式，0：向外放坡，1：向内放坡
        /// </summary>
        public int Pattern
        {
            get
            {
                return Properties.GetValue<int>("Pattern");
            }
            set
            {
                SetProps((GetPropId(nameof(Pattern)), value));
            }
        }
        public Polyline2d Outline
        {
            get
            {
                return  this.BaseCurve as Polyline2d;
            }
            set
            {
                this.BaseCurve = value;
            }
        }

        public LcList<EmbedAssociation> EmbedAssociations { get; } = new LcList<EmbedAssociation>();

        public bool CanChangeHost => false;

        public Profile2[] GetEmbedHoles()
        {
            var profiles = new Profile2[] { new Profile2() { OutLoop = new Polyline2d() { Curve2ds = this.GetShapes()[0].Curve2ds.Clone().Take(this.Outline.Curve2ds.Count).ToList() } } };
            return profiles;
        }
        public QdFoundationPit(QdFoundationPitDef lawnDef) : base(lawnDef)
        {
            Type = LayoutElementType.FoundationPit;
            Outline = new Polyline2d();
        } 
        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(this.GetShapes()[0].Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        }

 
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdFoundationPit(Definition as QdFoundationPitDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            var lawn = (QdFoundationPit)src;
            Outline = lawn.Outline.Clone() as Polyline2d; 
        }
 
 
    }
}
