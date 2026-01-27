using LightCAD.Core;
using LightCAD.Runtime;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;

namespace QdLayout
{
    public enum BuildingType
    {
        Work,//办公
        House,//住宅
        DiningHall,//餐厅
        Other//其他
    }
    public enum DetailRoomType
    {
        /// <summary>
        /// K式房
        /// </summary>
        KTypeRoom,
        /// <summary>
        /// 箱式房
        /// </summary>
        BoxTypeRoom,
    }
    public class PlateBuilding : IConvertDictionary
    {
        public DetailRoomType  DetailRoomType;
        public BuildingType BuildingType;
        public bool IsStartClose;
        public bool IsEndClose;
        public Vector2 BuildingLocation;
        //建筑物方向
        public Vector2 BuildingDirect;
        /// <summary>
        /// 楼层信息
        /// </summary>
        public int BuildingFloor;
        /// <summary>
        /// 建筑物轮廓
        /// </summary>
        public Polygon2d PlateBuildingOutline;
        /// <summary>
        /// 建筑物宽度
        /// </summary>
        public double PlateBuildingWidth;
        /// <summary>
        /// 建筑物长度
        /// </summary>
        public double PlateBuildingLength;
        /// <summary>
        /// 单位房间的宽度
        /// </summary>
        public double RoomSizeWidth;
        /// <summary>
        /// 单位房间的长度
        /// </summary>
        public bool StartStair;
        public bool EndStair;
        public double RoomSizeLength;



        /// <summary>
        /// 每层信息
        /// </summary>
        public List<PlateBuildingFloor> Floors;
        /// <summary>
        ///  楼的楼梯信息
        /// </summary>
        public List<PlateStair> Stairs;

     
        public   Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict  )
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.BuildingType), this.BuildingType);
            dict.Add(nameof(this.BuildingLocation), this.BuildingLocation);
            dict.Add(nameof(this.BuildingDirect), this.BuildingDirect);
            dict.Add(nameof(this.BuildingFloor), this.BuildingFloor);
            dict.Add(nameof(this.PlateBuildingOutline), this.PlateBuildingOutline);
            dict.Add(nameof(this.PlateBuildingWidth), this.PlateBuildingWidth);
            dict.Add(nameof(this.PlateBuildingLength), this.PlateBuildingLength);
            dict.Add(nameof(this.RoomSizeWidth), this.RoomSizeWidth);
            dict.Add(nameof(this.RoomSizeLength), this.RoomSizeLength);
            dict.Add(nameof(this.StartStair), this.StartStair);
            dict.Add(nameof(this.EndStair), this.EndStair);
            dict.Add(nameof(this.Floors), this.Floors);
            dict.Add(nameof(this.Stairs), this.Stairs);
            dict.Add(nameof(this.IsStartClose), this.IsStartClose);
            dict.Add(nameof(this.IsEndClose), this.IsEndClose);
            return dict;
        }
        public   void FromDictionary(Dictionary<string, PropertyObject> props)
        {
   
            this.PlateBuildingOutline = props.GetCurve2d(nameof(this.PlateBuildingOutline)) as Polygon2d;
            this.BuildingLocation = props.GetVector2(nameof(this.BuildingLocation));
            this.BuildingDirect = props.GetVector2(nameof(this.BuildingDirect));
            this.BuildingFloor = props.GetInt(nameof(this.BuildingFloor));


            this.BuildingType = (BuildingType)props.GetInt(nameof(this.BuildingType));
            this.RoomSizeWidth = props.GetDouble(nameof(this.RoomSizeWidth));
            this.RoomSizeLength = props.GetDouble(nameof(this.RoomSizeLength));
            this.PlateBuildingWidth = props.GetDouble(nameof(this.PlateBuildingWidth));
            this.PlateBuildingLength = props.GetDouble(nameof(this.PlateBuildingLength));
            this.StartStair = props.GetBool(nameof(this.StartStair));
            this.EndStair = props.GetBool(nameof(this.EndStair));
            this.IsStartClose = props.GetBool(nameof(this.IsStartClose));
            this.IsEndClose = props.GetBool(nameof(this.IsEndClose));


            this.Floors = new List<PlateBuildingFloor>();
            if (props.ContainsKey(nameof(this.Floors)))
            {
                var thisFloors = props.GetValue<List<PropertyObject>>(nameof(this.Floors));
                if (thisFloors != null)
                    foreach (var obj in thisFloors)
                    {
                        var Floor = new PlateBuildingFloor();

                        Floor.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.Floors.Add(Floor);
                    }
            }
            this.Stairs = new List<PlateStair>();
            if (props.ContainsKey(nameof(this.Stairs)))
            {
                var thisStairs = props.GetValue<List<PropertyObject>>(nameof(this.Stairs));
                if (thisStairs != null)
                    foreach (var obj in thisStairs)
                    {
                        var Stairs = new PlateStair();
                        Stairs.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.Stairs.Add(Stairs);
                    }
            }
            //  this.Floors = props.GetList<PlateBuildingFloor>(nameof(this.Floors));
            //  this.Stairs = props.GetList<PlateStair>(nameof(this.Stairs));


        }
        public  Box2 GetBoundingBox()
        {
            double count = 0;
            foreach (var item in this.Floors[0].Rooms)
            {
                count += item.CellNumber;
            }

            Vector2 dir2 = new Vector2(this.BuildingDirect.Y, -this.BuildingDirect.X);
            Vector2 p1 = this.BuildingLocation;
            Vector2 p3 = BuildingLocation + this.BuildingDirect * this.RoomSizeWidth * count + dir2 * (this.RoomSizeLength);
            Vector2 p2 = BuildingLocation + dir2 * (this.RoomSizeLength);
            Vector2 p4 = BuildingLocation + this.BuildingDirect * this.RoomSizeWidth * count  ;
            Vector2[] points = new Vector2[] { p1, p2, p3, p4 };
            return new Box2().ExpandByPoints(points);
        }
  
       
        public PlateBuilding( Vector2 location, Vector2 BuildingDirect, BuildingType buildingTyp, double RoomCount, double RoomSizeWidth, double RoomSizeLength, double PassagewaySizeWidth, double StairSizeWidth, double StairSizeLength, int floorNum, List<PlateRoom> Rooms, bool startStair, bool endStair, bool isStartClose, bool isEndClose)  
        {


            Floors = new List<PlateBuildingFloor>();
            this.BuildingLocation = location;
            this.BuildingDirect = BuildingDirect;
            this.BuildingFloor = floorNum;
            this.BuildingType = buildingTyp;
            this.RoomSizeWidth = RoomSizeWidth;
            this.RoomSizeLength = RoomSizeLength;
            this.StartStair = startStair;
            this.EndStair = endStair;
            this.IsStartClose=isStartClose;
            this.IsEndClose=isEndClose;
            for (int i = 0; i < floorNum; i++)
            {
                List<PlateRoom> floorrooms = new List<PlateRoom>();
                int floorRoomnum = (int)RoomCount / floorNum;
                if (i == 0)
                {
                    floorRoomnum = (int)RoomCount / floorNum + (int)RoomCount % floorNum;
                }
                floorrooms = AllotRooms(Rooms, floorRoomnum);
                PlateBuildingFloor plateBuildingFloor = new PlateBuildingFloor(location, BuildingDirect, floorrooms, i + 1, 3000, floorRoomnum, RoomSizeWidth, RoomSizeLength, PassagewaySizeWidth);
                Floors.Add(plateBuildingFloor);
            }
            InitStairs(location, startStair, endStair, PassagewaySizeWidth, StairSizeWidth, StairSizeLength, (int)RoomCount / floorNum + (int)RoomCount % floorNum);
        }
        public void PlateBuildingAddFloor( Vector2 location, Vector2 BuildingDirect, BuildingType buildingTyp, double RoomCount, double RoomSizeWidth, double RoomSizeLength, double PassagewaySizeWidth, double StairSizeWidth, double StairSizeLength, int floorNum, List<PlateRoom> Rooms, bool startStair, bool endStair, bool isStartClose, bool isEndClose)
        {    
                List<PlateRoom> floorrooms = new List<PlateRoom>();
                floorrooms = AllotRooms(Rooms, (int)RoomCount);
                PlateBuildingFloor plateBuildingFloor = new PlateBuildingFloor(location, BuildingDirect, floorrooms, floorNum, 3000, RoomCount, RoomSizeWidth, RoomSizeLength, PassagewaySizeWidth);
                this.Floors.Add(plateBuildingFloor);
      }
        public List<PlateRoom> AllotRooms(List<PlateRoom> rooms, int count)
        {
            List<PlateRoom> krooms = new List<PlateRoom>();

            int floorRoomnum = count;

            foreach (var item in rooms)
            {
                if (floorRoomnum >= item.CellNumber)
                {
                    floorRoomnum -= (int)item.CellNumber;
                    krooms.Add(item);
                }
            }
            foreach (var item in krooms)
            {
                rooms.Remove(item);
            }
            return krooms;
        }
        //public PlateHouse(LcComponentDefinition comDef) : base(comDef)
        //{
        //    this.GridCells= new List<GridCell>();
        //    this.PassagewayCells= new List<PassagewayCell>();
        //    this.PlateCells= new List<PlateCell>();
        //    this.StairsCells = new List<StairsCell>();
        //}
        public void InitStairs(Vector2 zero, bool startStair, bool endStair, double passagewayCellWidth, double StairSizeWidth, double StairSizeLength, int roomNum)
        {
            Stairs = new List<PlateStair>();

            Vector2 dir2 = new Vector2(BuildingDirect.Y, -BuildingDirect.X);



            if (startStair)
            {
                List<Vector2> points = new List<Vector2>();
                Vector2 p1 = zero - BuildingDirect * StairSizeWidth + dir2 * (RoomSizeLength + passagewayCellWidth - StairSizeLength);
                Vector2 p2 = zero - BuildingDirect * StairSizeWidth + dir2 * (RoomSizeLength + passagewayCellWidth);
                Vector2 p3 = zero + dir2 * (RoomSizeLength + passagewayCellWidth);
                Vector2 p4 = zero + dir2 * (RoomSizeLength + passagewayCellWidth - StairSizeLength);
                Polygon2d polygon2D = new Polygon2d();
                points.Add(p1);
                points.Add(p2);
                points.Add(p3);
                points.Add(p4);
                polygon2D.Points = points.ToArray();
                PlateStair plateStair = new PlateStair(StairSizeWidth, StairSizeLength, polygon2D);
                Stairs.Add(plateStair);
            }
            if (endStair)
            {
                List<Vector2> points = new List<Vector2>();
                Polygon2d polygon2D = new Polygon2d();
                Vector2 p1 = zero + BuildingDirect * RoomSizeWidth * roomNum + dir2 * (RoomSizeLength + passagewayCellWidth - StairSizeLength);
                Vector2 p2 = zero + BuildingDirect * RoomSizeWidth * roomNum + dir2 * (RoomSizeLength + passagewayCellWidth);
                Vector2 p3 = zero + BuildingDirect * StairSizeWidth + BuildingDirect * RoomSizeWidth * roomNum + dir2 * (RoomSizeLength + passagewayCellWidth);
                Vector2 p4 = zero + BuildingDirect * StairSizeWidth + BuildingDirect * RoomSizeWidth * roomNum + dir2 * (RoomSizeLength + passagewayCellWidth - StairSizeLength);
                points.Add(p1);
                points.Add(p2);
                points.Add(p3);
                points.Add(p4);
                polygon2D.Points = points.ToArray();
                PlateStair plateStair = new PlateStair(StairSizeWidth, StairSizeLength, polygon2D);
                Stairs.Add(plateStair);
            }

        }

        
    }
}
