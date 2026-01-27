using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightCAD.MathLib;
using LightCAD.Runtime;
using Utils;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public enum RoomType
    {
        /// <summary>
        /// 普通房间
        /// </summary>
        Normal,
        /// <summary>
        /// 楼梯
        /// </summary>
        Stair,
    }
    public class PlateRoom : IConvertDictionary
    {
        /// <summary>
        /// 房间位置左下角的点
        /// </summary>
        public Vector2 Localtion;
        /// <summary>
        /// 房间方向
        /// </summary>
        public Vector2 Direct;
        /// <summary>
        /// 占几个普通房间单元
        /// </summary>
        public double CellNumber;

        /// <summary>
        /// 普通房间 还是改造成楼梯的房间，也可把 房间用途作为类型存储
        /// </summary>
        public RoomType RoomType;
        public bool Build;
        public string Name;
        /// <summary>
        /// 房间的宽度
        /// </summary>
        public double RoomWidth;
        /// <summary>
        ///  房间的长度
        /// </summary>
        public double RoomLength;
        /// <summary>
        ///  房间的高度
        /// </summary>
        public double RoomHeight;
        /// <summary>
        ///  房间走廊
        /// </summary>
        public double passagewayCellWidth;
        /// <summary>
        /// 房间建筑类型  办公的还是住宿还是食堂用于总数统计
        /// </summary>
        public BuildingType RoomBuildingType;

        public string RoomConfig;

        public List<GridCell> GridCells;
        public List<PlatePassageway> passagewayCell;
        public PlateRoom()
        { }
        public PlateRoom(string name, BuildingType buildingType, int cellNumber)
        {

            this.Name = name;
            this.RoomBuildingType = buildingType;
            // this.RoomHeight = 0;
            this.RoomType = RoomType.Normal;
            this.CellNumber = cellNumber;
        }
        public void PlatePassagewayInit()
        {
            passagewayCell = new List<PlatePassageway>();
            //PlatePassageway platePassageway = new PlatePassageway(this);
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
        public void PlateCellInit()
        {
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
        public Polygon2d GetRoomPolygon2d()
        {
            List<Vector2> points = new List<Vector2>();
            Polygon2d polygon2D = new Polygon2d();
            Vector2 dir2 = new Vector2(this.Direct.Y, -this.Direct.X);
            points.Add(Localtion);
            points.Add(Localtion + this.Direct * this.RoomWidth * this.CellNumber);

            points.Add(Localtion + this.Direct * this.RoomWidth * this.CellNumber + dir2 * (this.RoomLength));
            points.Add(Localtion + dir2 * (this.RoomLength));
            polygon2D.Points = points.ToArray();

            return polygon2D;
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }

        public List<Polygon2d> GetRoomConfigPolygon2d(RoomConfig roomConfig)
        {
            var polygon2ds = new List<Polygon2d>();
            var configFilePath = roomConfig.ConfigFilePath;
            var doc = LoadManager.Load(configFilePath);

            var roomDir = this.Direct.Clone().RotateAround(new Vector2(), -Math.PI / 2);
            var roomMatrix = new Matrix3()
                            .MakeTranslation(this.Localtion.X, this.Localtion.Y)
                            .Multiply(new Matrix3().MakeRotationAround(new Vector2(), roomDir.Angle()));

            void drawBlockRef(LcBlockRef blockRef, Matrix3 matrix)
            {
                var block = blockRef.Container as LcBlock;
                var offset = block.BasePoint.Clone().Negate();
                var transMatrix = new Matrix3().MakeTranslation(offset.X, offset.Y);

                var newMatrix = matrix.Clone().Multiply(transMatrix).Multiply(blockRef.Matrix);

                foreach (var ele in block.Elements)
                {
                    if (ele is LcCurve2d curve)
                    {
                        drawCurve(curve.Curve, newMatrix);
                    }
                    else if (ele is LcBlockRef subBlockRef)
                    {
                        drawBlockRef(subBlockRef, newMatrix);
                    }
                }
            }

            void drawCurve(Curve2d curve, Matrix3 matrix)
            {
                if (curve is Arc2d arc)
                {
                    var ps = arc.GetPoints(30).Select(p => p.ApplyMatrix3(matrix));
                    var polygon2d = new Polygon2d()
                    {
                        Points = ps.ToArray()
                    };
                    polygon2ds.Add(polygon2d);
                }
                else if (curve is Line2d line)
                {
                    var ps = line.GetPoints(1).Select(p => p.ApplyMatrix3(matrix));
                    var polygon2d = new Polygon2d()
                    {
                        Points = ps.ToArray()
                    };
                    polygon2ds.Add(polygon2d);
                }
                else if (curve is Polyline2d polyline)
                {
                    foreach (var pcurve in polyline.Curve2ds)
                    {
                        drawCurve(pcurve, matrix);
                    }
                }
                else if (curve is Polygon2d polygon)
                {
                    var newPolygon = polygon.Clone() as Polygon2d;
                    newPolygon.Points.ForEach(p => p.ApplyMatrix3(matrix));
                    polygon2ds.Add(newPolygon);
                }
            }

            foreach (var ele in doc.ModelSpace.Elements)
            {
                if (ele is LcCurve2d curve)
                {
                    drawCurve(curve.Curve, roomMatrix);
                }
                else if (ele is LcBlockRef blockRef)
                {
                    drawBlockRef(blockRef, roomMatrix);
                }
            }

            return polygon2ds;
        }

        public List<Line2d> GetRoomline()
        {

            List<Line2d> lines = new List<Line2d>();
            Vector2 dir2 = new Vector2(this.Direct.Y, -this.Direct.X);
            Vector2 p1 = Localtion;
            Vector2 p2 = Localtion + this.Direct * this.RoomWidth * this.CellNumber;

            Vector2 p3 = Localtion + this.Direct * this.RoomWidth * this.CellNumber + dir2 * (this.RoomLength);
            Vector2 p4 = Localtion + dir2 * (this.RoomLength);
            lines.Add(new Line2d(p1, p2));
            lines.Add(new Line2d(p2, p3));
            lines.Add(new Line2d(p3, p4));
            lines.Add(new Line2d(p4, p1));
            return lines;

        }
        public Polygon2d GetpassagewayLine()
        {
            List<Vector2> points = new List<Vector2>();
            Polygon2d polygon2D = new Polygon2d();
            Vector2 dir2 = new Vector2(this.Direct.Y, -this.Direct.X);

            points.Add(Localtion + dir2 * (this.RoomLength));
            points.Add(Localtion + dir2 * (this.RoomLength) + this.Direct * this.RoomWidth * this.CellNumber);

            points.Add(Localtion + this.Direct * this.RoomWidth * this.CellNumber + dir2 * (this.RoomLength + this.passagewayCellWidth));
            points.Add(Localtion + dir2 * (this.RoomLength + this.passagewayCellWidth));
            polygon2D.Points = points.ToArray();
            return polygon2D;
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
        public Vector2 GetCenter()
        {


            Vector2 dir2 = new Vector2(this.Direct.Y, -this.Direct.X);

            Vector2 p1 = Localtion.Clone();
            Vector2 p2 = Localtion.Clone() + this.Direct * this.RoomWidth * this.CellNumber + dir2 * this.RoomLength;


            return (p1 + p2) / 2;
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
   
        public Vector2 GetTextlocation()
        {


            Vector2 dir2 = new Vector2(this.Direct.Y, -this.Direct.X);


            Vector2 p2 = Localtion.Clone() + this.Direct * this.RoomWidth / 3 + dir2 * this.RoomLength / 5;


            return p2;
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.Localtion), this.Localtion);
            dict.Add(nameof(this.Direct), this.Direct);
            dict.Add(nameof(this.CellNumber), this.CellNumber);
            dict.Add(nameof(this.RoomType), this.RoomType);
            dict.Add(nameof(this.Build), this.Build);
            dict.Add(nameof(this.Name), this.Name);
            dict.Add(nameof(this.RoomWidth), this.RoomWidth);
            dict.Add(nameof(this.RoomLength), this.RoomLength);
            dict.Add(nameof(this.RoomHeight), this.RoomHeight);
            dict.Add(nameof(this.passagewayCellWidth), this.passagewayCellWidth);
            dict.Add(nameof(this.RoomBuildingType), this.RoomBuildingType);
            dict.Add(nameof(this.GridCells), this.GridCells);
            dict.Add(nameof(this.passagewayCell), this.passagewayCell);
            dict.Add(nameof(this.RoomConfig), this.RoomConfig);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.Localtion = props.GetVector2(nameof(Localtion));
            this.Direct = props.GetVector2(nameof(Direct));
            this.CellNumber = props.GetDouble(nameof(CellNumber));
            this.RoomType = (RoomType)props.GetInt(nameof(RoomType));
            this.Build = props.GetBool(nameof(Build));
            this.Name = props.GetString(nameof(Name));
            this.RoomWidth = props.GetDouble(nameof(RoomWidth));
            this.RoomLength = props.GetDouble(nameof(RoomLength));
            this.RoomHeight = props.GetDouble(nameof(RoomHeight));
            this.passagewayCellWidth = props.GetDouble(nameof(passagewayCellWidth));
            this.RoomBuildingType = (BuildingType)props.GetInt(nameof(RoomBuildingType));
            // this.GridCells = props.GetList<GridCell>(nameof(GridCells));
            this.GridCells = new List<GridCell>();
            if (props.ContainsKey(nameof(this.GridCells)))
            {
                var thisGridCells = props.GetValue<List<PropertyObject>>(nameof(this.GridCells));
                if (thisGridCells != null)
                    foreach (var obj in thisGridCells)
                    {
                        var GridCell = new GridCell();

                        GridCell.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.GridCells.Add(GridCell);
                    }
            }
            this.passagewayCell = new List<PlatePassageway>();
            if (props.ContainsKey(nameof(this.passagewayCell)))
            {
                var thispassagewayCells = props.GetValue<List<PropertyObject>>(nameof(this.passagewayCell));
                if (thispassagewayCells != null)
                    foreach (var obj in thispassagewayCells)
                    {
                        var passageway = new PlatePassageway();

                        passageway.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.passagewayCell.Add(passageway);
                    }
            }
            //   this.passagewayCell = props.GetList<PlatePassageway>(nameof(passagewayCell));

            if (props.ContainsKey(nameof(this.RoomConfig)))
            {
                this.RoomConfig = props.GetString(nameof(this.RoomConfig));
            }

        }
        //public ListEx<EmbedAssociation> EmbedAssociations => throw new NotImplementedException();

        //public Profile2[] GetEmbedHoles()
        //{
        //    throw new NotImplementedException();
        //}

        //public LcElement Stretch(Box2 box, Vector2 vector)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
