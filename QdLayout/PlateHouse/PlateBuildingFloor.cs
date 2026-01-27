using LightCAD.MathLib;
using LightCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{

    public class PlateBuildingFloor : IConvertDictionary
    {


        /// <summary>
        /// 楼层号
        /// </summary>
        public int FloorNum;
        /// <summary>
        /// 理论房间总数
        /// </summary>
        public double RoomCount;
        /// <summary>
        /// 是否在二维图纸上渲染
        /// </summary>
        public bool DrawLdwg;
        /// <summary>
        /// 组成房间的版集合 门窗挂接在板上
        /// </summary>
        public List<PlateK> PlateKs;

        public double FloorHeight;
        /// <summary>
        /// 每层楼的房间列表
        /// </summary>
        public List<PlateRoom> Rooms;

        /// <summary>
        /// 每层楼独立走廊  ，可以为空，（房间已经包含走廊部分 ）
        /// </summary>
     //   public List<PlatePassageway> Passageways;
        public List<PlatePassagewayK> PassagewayKs;
        public PlateBuildingFloor()
        { }
        public PlateBuildingFloor(Vector2 location, Vector2 BuildingDirect, List<PlateRoom> rooms, int floorNum, double floorHeight, double RoomCount, double RoomSizeWidth, double RoomSizeLength, double PassagewaySizeWidth)
        {
            this.Rooms = new List<PlateRoom>();
            Vector2 loca = location.Clone();
            this.RoomCount = RoomCount;
            this.FloorHeight = floorHeight;
            this.FloorNum = floorNum;
            foreach (var item in rooms)
            {
                item.Localtion = loca.Clone();
                loca = new Vector2(loca.X + (BuildingDirect.X * RoomSizeWidth * item.CellNumber), loca.Y + (BuildingDirect.Y * RoomSizeWidth * item.CellNumber));
                item.RoomWidth = RoomSizeWidth;
                item.RoomLength = RoomSizeLength;
                item.Build = true;
                item.passagewayCellWidth = PassagewaySizeWidth;
                item.PlatePassagewayInit();
                item.Direct = BuildingDirect;
                this.Rooms.Add(item);
            }

        }
        public void ResetPlateBuildingFloor(Vector2 location, List<PlateRoom> rooms, Vector2 BuildingDirect, double RoomSizeWidth, double RoomSizeLength, double PassagewaySizeWidth)
        {
            Vector2 loca = location.Clone();
            this.Rooms.Clear();

            foreach (var item in rooms)
            {
                item.Localtion = loca.Clone();
                loca = new Vector2(loca.X + (BuildingDirect.X * RoomSizeWidth * item.CellNumber), loca.Y + (BuildingDirect.Y * RoomSizeWidth * item.CellNumber));
                item.RoomWidth = RoomSizeWidth;
                item.RoomLength = RoomSizeLength;
                item.Build = true;
                item.passagewayCellWidth = PassagewaySizeWidth;
                item.PlatePassagewayInit();
                item.Direct = BuildingDirect;
                this.Rooms.Add(item);
            }
        }
        public Polygon2d GetEmptyRoomLine()
        {
            double resultCount = 0;
            foreach (var item in this.Rooms)
            {
                resultCount += item.CellNumber;
            }
            if (this.RoomCount == resultCount)
            {
                return null;
            }
            else
            {
                if (resultCount != 0)
                {

                    var lastroom = this.Rooms.LastOrDefault();
                    List<Vector2> points = new List<Vector2>();
                    Polygon2d polygon2D = new Polygon2d();
                    Vector2 dir2 = new Vector2(lastroom.Direct.Y, -lastroom.Direct.X);

                    var Localtion = lastroom.Localtion + lastroom.Direct * lastroom.RoomWidth * lastroom.CellNumber;
                    points.Add(Localtion);
                    points.Add(Localtion + lastroom.Direct * lastroom.RoomWidth * (this.RoomCount - resultCount));

                    points.Add(Localtion + lastroom.Direct * lastroom.RoomWidth * (this.RoomCount - resultCount) + dir2 * (lastroom.RoomLength));
                    points.Add(Localtion + dir2 * (lastroom.RoomLength));
                    polygon2D.Points = points.ToArray();

                    return polygon2D;
                }
                else
                {
                    return null;
                }
            }
        }
        public void PlateKsReset()
        {

        }
        public void PassagewayKsReset()
        {

        }
        /// <summary>
        /// 获取当前楼层的走廊线段
        /// </summary>
        /// <param name="startStair"></param>
        /// <param name="endStair"></param>
        /// <returns></returns>
        public List<PlatePassagewayK> GetPassagewayLines(bool startStair, bool endStair)
        {
            List<PlatePassagewayK> line2Ds = new List<PlatePassagewayK>();
            bool start = true;
            bool end = true;
            if (this.Rooms.Count == 0)
            {
                return line2Ds;
            }
            var sroom = this.Rooms.FirstOrDefault();
            var eroom = this.Rooms.LastOrDefault();
            Vector2 dir2 = new Vector2(sroom.Direct.Y, -sroom.Direct.X);

            if (!startStair)
            {
         
                Line2d line2D = new Line2d();
                line2D.Start = sroom.Localtion + dir2 * (sroom.RoomLength);
                line2D.End = sroom.Localtion + dir2 * (sroom.RoomLength + sroom.passagewayCellWidth);
                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                platePassagewayK.Line = line2D;
                platePassagewayK.PassagewayKType = PlatePassagewayKType.Wall;
                line2Ds.Add(platePassagewayK);
            }
            Line2d line2D1 = new Line2d();
            line2D1.Start = sroom.Localtion + dir2 * (sroom.RoomLength + sroom.passagewayCellWidth);
            line2D1.End = eroom.Localtion + eroom.Direct * eroom.RoomWidth * eroom.CellNumber + dir2 * (eroom.RoomLength + eroom.passagewayCellWidth);
            PlatePassagewayK platePassagewayK1 = new PlatePassagewayK();
            platePassagewayK1.Line = line2D1;
            platePassagewayK1.PassagewayKType = PlatePassagewayKType.Fence;
            line2Ds.Add(platePassagewayK1);
            if (!endStair)
            {
                Line2d line2D = new Line2d();
                line2D.Start = eroom.Localtion + eroom.Direct * eroom.RoomWidth * eroom.CellNumber + dir2 * (eroom.RoomLength);
                line2D.End = eroom.Localtion + eroom.Direct * eroom.RoomWidth * eroom.CellNumber + dir2 * (eroom.RoomLength + eroom.passagewayCellWidth);
                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                platePassagewayK.Line = line2D;
                platePassagewayK.PassagewayKType = PlatePassagewayKType.Wall;
                line2Ds.Add(platePassagewayK);
            }
            return line2Ds;
        }
        //public PlateHouse(LcComponentDefinition comDef) : base(comDef)
        //{
        //    this.GridCells= new List<GridCell>();
        //    this.PassagewayCells= new List<PassagewayCell>();
        //    this.PlateCells= new List<PlateCell>();
        //    this.StairsCells = new List<StairsCell>();
        //}



        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.FloorNum), this.FloorNum);
            dict.Add(nameof(this.RoomCount), this.RoomCount);
            dict.Add(nameof(this.PlateKs), this.PlateKs);
            dict.Add(nameof(this.FloorHeight), this.FloorHeight);
            dict.Add(nameof(this.Rooms), this.Rooms);
            dict.Add(nameof(this.PassagewayKs), this.PassagewayKs);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.FloorNum = props.GetInt(nameof(this.FloorNum));
            this.RoomCount = props.GetInt(nameof(this.RoomCount));
            //       this.PlateKs = props.GetList<PlateK>(nameof(this.PlateKs));
            this.PlateKs = new List<PlateK>();
            if (props.ContainsKey(nameof(this.PlateKs)))
            {
                var thisPlateKs = props.GetValue<List<PropertyObject>>(nameof(this.PlateKs));
                if (thisPlateKs != null)
                {
                    foreach (var obj in thisPlateKs)
                    {
                        var PlateKs = new PlateK();

                        PlateKs.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.PlateKs.Add(PlateKs);
                    }
                }
            }

            this.FloorHeight = props.GetDouble(nameof(this.FloorHeight));
            //  this.Rooms = props.GetList<PlateRoom>(nameof(this.Rooms));
            this.Rooms = new List<PlateRoom>();
            if (props.ContainsKey(nameof(this.Rooms)))
            {
                var thisRooms = props.GetValue<List<PropertyObject>>(nameof(this.Rooms));
                if (thisRooms != null)
                    foreach (var obj in thisRooms)
                    {
                        var Room = new PlateRoom();

                        Room.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.Rooms.Add(Room);
                    }
            }

            this.PassagewayKs = new List<PlatePassagewayK>();
            if (props.ContainsKey(nameof(this.PassagewayKs)))
            {
                var thisPassagewayKs = props.GetValue<List<PropertyObject>>(nameof(this.PassagewayKs));
                if (thisPassagewayKs != null)
                    foreach (var obj in thisPassagewayKs)
                    {
                        var PassagewayK = new PlatePassagewayK();

                        PassagewayK.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.PassagewayKs.Add(PassagewayK);
                    }
            }
            //  this.PassagewayKs = props.GetList<PlatePassagewayK>(nameof(this.PassagewayKs));
        }
    }
}
