using LightCAD.Core;
using LightCAD.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class QdPlanBuild : DirectComponent, IComponentEdit
    {
        /// <summary>
        /// 基础多段线 
        /// </summary>
        public Polyline2d BasePolyline
        {
            get
            {
                return Properties.GetValue<Polyline2d>("BasePolyline");
            }
            set
            {
                SetProps((GetPropId(nameof(BasePolyline)), value));
            }
        }

        /// <summary>
        /// 构件样式
        /// </summary>
        //public string ComponentStyle
        //{
        //    get { return Convert.ToString(this.Properties["ComponentStyle"]); }
        //    set { this.Properties["ComponentStyle"] = value; }
        //}

        /// <summary>
        /// 一层低标高
        /// </summary>
        public string OneBottomLevel
        {
            get 
            { 
                return Convert.ToString(this.Properties["OneBottomLevel"]); 
            }
            set 
            { 
                SetProps((GetPropId(nameof(OneBottomLevel)), value)); 
            }
        }

        /// <summary>
        /// 地上层数
        /// </summary>
        public double GroundUpStorey
        {
            get 
            {
                return Convert.ToDouble(this.Properties["GroundUpStorey"]); 
            }

            set 
            {
                SetProps((GetPropId(nameof(GroundUpStorey)), value));
            }
        }

        /// <summary>
        /// 地下层数
        /// </summary>
        public double GroundDownStorey
        {

            get 
            { 
                return Convert.ToDouble(this.Properties["GroundDownStorey"]); 
            }
            set
            {
                SetProps((GetPropId(nameof(GroundDownStorey)), value));
            }
        }

        /// <summary>
        /// 层高
        /// </summary>
        public double StoreyHeight
        {
            get 
            { 
                return Convert.ToDouble(this.Properties["StoreyHeight"]); 
            }
            set 
            {
                SetProps((GetPropId(nameof(StoreyHeight)), value));
            }
        }
        /// <summary>
        /// 层高
        /// </summary>
        public double SignedArea
        {
            get
            {
                return Convert.ToDouble(this.Properties["SignedArea"]);
            }
            set
            {
                SetProps((GetPropId(nameof(SignedArea)), value));
            }
        }

        /// <summary>
        /// 建筑类型
        /// </summary>
        //public string BuildType
        //{
        //    get { return Convert.ToString(this.Properties["BuildType"]); }
        //    set { this.Properties["BuildType"] = value; }
        //}

        /// <summary>
        /// 外墙材质
        /// </summary>
        //public MaterialInfo OuterWallMaterial
        //{
        //    get { return this.Properties.GetValue<MaterialInfo>("OuterWallMaterial"); }
        //    set { this.Properties.SetValue("OuterWallMaterial", value); }
        //}

        /// <summary>
        /// 平屋顶材质
        /// </summary>
        //public MaterialInfo FlatRoofMaterial
        //{
        //    get { return this.Properties.GetValue<MaterialInfo>("FlatRoofMaterial"); }
        //    set { this.Properties.SetValue("FlatRoofMaterial", value); }
        //}

        /// <summary>
        /// 坡屋顶材质
        /// </summary>
        //public MaterialInfo SlopRoofMaterial
        //{
        //    get { return this.Properties.GetValue<MaterialInfo>("SlopRoofMaterial"); }
        //    set { this.Properties.SetValue("SlopRoofMaterial", value); }
        //}

        /// <summary>
        /// 女儿墙材质
        /// </summary>
        //public MaterialInfo ParapetMaterial
        //{
        //    get { return this.Properties.GetValue<MaterialInfo>("ParapetMaterial"); }
        //    set { this.Properties.SetValue("ParapetMaterial", value); }
        //}

        /// <summary>
        /// 屋顶类型
        /// </summary>
        //public string RoofType
        //{
        //    get { return Convert.ToString(this.Properties["RoofType"]); }
        //    set { this.Properties["RoofType"] = value; }
        //}

        /// <summary>
        /// 有无女儿墙
        /// </summary>
        //public bool IsHaveParapet
        //{
        //    get { return Convert.ToBoolean(this.Properties["IsHaveParapet"]); }
        //    set { this.Properties["IsHaveParapet"] = value; }
        //}

        /// <summary>
        /// 女儿墙高度
        /// </summary>
        //public double ParapetHeight
        //{
        //    get { return Convert.ToDouble(this.Properties["ParapetHeight"]); }
        //    set { this.Properties["ParapetHeight"] = value; }
        //}

        /// <summary>
        /// 女儿墙厚度
        /// </summary>
        //public double ParapetThickness
        //{
        //    get { return Convert.ToDouble(this.Properties["ParapetThickness"]); }
        //    set { this.Properties["ParapetThickness"] = value; }
        //}

        /// <summary>
        /// 屋面坡度
        /// </summary>
        //public double RoofSlope
        //{
        //    get { return Convert.ToDouble(this.Properties["RoofSlope"]); }
        //    set { this.Properties["RoofSlope"] = value; }
        //}

        /// <summary>
        /// 挑檐出挑长度
        /// </summary>
        //public double OutStandLength
        //{
        //    get { return Convert.ToDouble(this.Properties["OutstandLength"]); }
        //    set { this.Properties["OutstandLength"] = value; }
        //}

        /// <summary>
        /// 拟建建筑名称
        /// </summary>
        //public LcText PlanBuildName
        //{
        //    get { return (LcText)this.Properties["PlanBuildName"]; }
        //    set { this.Properties["PlanBuildName"] = value; }
        //}

        public QdPlanBuild(QdPlanBuildDef qdPlanBuild) : base(qdPlanBuild)
        {
            this.Type = LayoutElementType.PlanBuild;
        }

        public override Curve2dGroupCollection GetShapes()
        {
            if (shapes == null)
            {
                ListEx<Curve2d> curve2Ds = new ListEx<Curve2d>();
                if (this.BasePolyline != null) 
                {
                    foreach (var item in this.BasePolyline.Curve2ds)
                    {
                        curve2Ds.Add(item);
                    }
                }
 
                shapes = new Curve2dGroupCollection() { new Curve2dGroup() { Curve2ds = curve2Ds } };
            }
            return shapes;
        }

        public Profile2[] GetEmbedHoles()
        {
            throw new NotImplementedException();
        }

        public LcElement Stretch(Box2 box, Vector2 vector)
        {
            throw new NotImplementedException();
        }

        public override Box2 GetBoundingBox()
        {
            return new Box2().ExpandByPoints(this.BasePolyline.Curve2ds.SelectMany(n => n.GetPoints()).ToArray());
        }

        public override void Move(Vector2 startPoint, Vector2 endPoint)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.BaseCurve = curve2D.Translate(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();

        }

        public override void Translate(Vector2 vec)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Translate(vec);
            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            ResetBoundingBox();
        }
        public override void Translate(double dx, double dy)
        {
            Translate(new Vector2(dx, dy));
        }

        public override void Mirror(Vector2 axisStart, Vector2 axisEnd)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            var axisDir = new Vector2().SubVectors(axisEnd, axisStart).Normalize();
            this.BaseCurve = curve2D.Mirror(axisStart, axisDir);
            this.UpdatePolylineReverseSide();

            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();
        }

        public override void Rotate(Vector2 basePoint, double rotateAngle)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Rotate(basePoint, rotateAngle);

            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();
        }

        public override void Scale(Vector2 basePoint, double scaleFactor)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Scale(basePoint, scaleFactor);

            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();
        }
        public override void Scale(double scaleFactor, Matrix3 matrix3)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Scale(scaleFactor, matrix3);
            this.BasePolyline = this.BaseCurve as Polyline2d;


            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();
        }
        public override void Scale(Vector2 basePoint, Vector2 scaleVector)
        {
            Polyline2d curve2D = this.BasePolyline as Polyline2d;

            this.OnPropertyChangedBefore(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);
            this.BaseCurve = curve2D.Scale(basePoint, scaleVector, this.BoundingBox);
            this.BasePolyline = this.BaseCurve as Polyline2d;

            this.ResetCache();
            this.OnPropertyChangedAfter(nameof(this.BaseCurve), this.BaseCurve, this.BaseCurve);

            this.DirtyType = DirtyType.Change;
            ResetBoundingBox();
        }
        public override void OnBatchInsertAfter()
        {
            ResetBoundingBox();
        }
        public override LcElement Clone()
        {
            var clone = new QdPlanBuild(Definition as QdPlanBuildDef);
            clone.Copy(this);
            //clone.Initilize(Document);
            return clone;
        }

        public override void Copy(LcElement src)
        {
            base.Copy(src);
            QdPlanBuild qdPlanBuild = (QdPlanBuild)src;
            this.BasePolyline = qdPlanBuild.BasePolyline.Clone() as Polyline2d;
        }

        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            foreach (var curve in this.BasePolyline.Curve2ds)
            {
                if (curve is Line2d line)
                {
                    if (Intersect2d.IsPolygonWithLine(testPoly.Points, line.Start, line.End))
                    {
                        return true;
                    }
                }
                else if (curve is Arc2d arc)
                {
                    if (Intersect2d.IsPolygonWithArc(testPoly.Points, arc))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool IncludedByBox(Polygon2d testPoly, List<RefChildElement> includedChildren = null)
        {
            var thisBox = BoundingBox;
            if (!testPoly.BoundingBox.ContainsBox(thisBox))
                return false;
            if (testPoly.BoundingBox.ContainsBox(thisBox))
                return true;
            foreach (var curve in this.BasePolyline.Curve2ds)
            {
                if (curve is Line2d line)
                {
                    if (Intersect2d.IsPolygonWithLine(testPoly.Points, line.Start, line.End))
                    {
                        return true;
                    }
                }
                else if (curve is Arc2d arc)
                {
                    if (Intersect2d.IsPolygonWithArc(testPoly.Points, arc))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public double UpdatePolylineReverseSide() 
        {
            double signedArea = CalculateSignedArea(this.BasePolyline.Curve2ds);
            if (signedArea > 0)
            {
                List<Curve2d> newCurves = new List<Curve2d>();
                for (int i = 0; i < this.BasePolyline.Curve2ds.Count; i++)
                {
                    if (this.BasePolyline.Curve2ds[i] is Line2d)
                    {
                        Line2d line2d = this.BasePolyline.Curve2ds[i] as Line2d;
                        newCurves.Add(new Line2d(line2d.End, line2d.Start));
                    }
                    else if (this.BasePolyline.Curve2ds[i] is Arc2d)
                    {

                    }
                }
                this.BasePolyline.Curve2ds = newCurves;
            }
            else if (signedArea < 0) { }
            else {  };

            return signedArea;
        }

        /// <summary>
        /// 计算多段线组成的面积
        /// </summary>
        /// <param name="curve2ds"></param>
        /// <returns></returns>
        public double CalculateSignedArea(List<Curve2d> curve2ds)
        {
            double area = 0.0;

            for (int i = 0; i < curve2ds.Count; i++)
            {
                Line2d line = curve2ds[i] as Line2d;
                if (line != null)
                {
                    Vector2 start = line.Start;
                    Vector2 end = line.End;
                    area += (end.X - start.X) * (end.Y + start.Y);
                }
            }

            return area * 0.5;
        }
    }
}
