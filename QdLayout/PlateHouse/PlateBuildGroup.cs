using LightCAD.MathLib;
using OpenTK.Graphics.GL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace QdLayout
{


    public enum PlateBuildGroupType
    {
        一,
        L,
        U
    }
    public enum DeatilPlateBuildGroupType
    {
        A,
        B,
        C,
        D,
    }



    public class PlateBuildGroup : DirectComponent
    {
        public static List<string> PBGZ1 = new List<string>() {

        "[{\"BlockType\":\"STL\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":1000,\"Right\":-1,\"Top\":-1,\"Bottom\":300,\"Width\":-1,\"Height\":-1,\"Rotate\":90,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"XFTD\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"None\",\"Left\":1300,\"Right\":-1,\"Top\":-1,\"Bottom\":300,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"BGL\",\"Id\":3,\"HorizontalObjId\":2,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":1300,\"Right\":-1,\"Top\":-1,\"Bottom\":12000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":4,\"HorizontalObjId\":3,\"VerticlObjId\":3,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Top\",\"Left\":-1,\"Right\":-1,\"Top\":8000,\"Bottom\":-1,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}, {\"BlockType\":\"SSL\",\"Id\":5,\"HorizontalObjId\":3,\"VerticlObjId\":3,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":0,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}]",
        "[{\"BlockType\":\"XFTD\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"None\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":-1,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"BGL\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":1300,\"Right\":-1,\"Top\":-1,\"Bottom\":12000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":3,\"HorizontalObjId\":2,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Top\",\"Left\":-1,\"Right\":-1,\"Top\":8000,\"Bottom\":-1,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"STL\",\"Id\":4,\"HorizontalObjId\":2,\"VerticlObjId\":2,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":0,\"Right\":-1,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":5,\"HorizontalObjId\":2,\"VerticlObjId\":4,\"HorizontalAlignment\":\"RightAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":0,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}]",
        "[{\"BlockType\":\"BGL\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":1000,\"Right\":-1,\"Top\":-1,\"Bottom\":8000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":1,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Top\",\"Left\":-1,\"Right\":-1,\"Top\":8000,\"Bottom\":-1,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":3,\"HorizontalObjId\":1,\"VerticlObjId\":1,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":0,\"Right\":-1,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}, {\"BlockType\":\"STL\",\"Id\":4,\"HorizontalObjId\":1,\"VerticlObjId\":3,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":0,\"Right\":-1,\"Top\":-1,\"Bottom\":8000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}]",

        "[{\"BlockType\":\"SSL\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":1000,\"Right\":-1,\"Top\":-1,\"Bottom\":1000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"WYL\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":1,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Top\",\"Left\":-1,\"Right\":-1,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"XFTD\",\"Id\":3,\"HorizontalObjId\":1,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"None\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":-1,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":4,\"HorizontalObjId\":3,\"VerticlObjId\":0,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":-1,\"Top\":-1,\"Bottom\":0,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1}, {\"BlockType\":\"SSL\",\"Id\":5,\"HorizontalObjId\":3,\"VerticlObjId\":3,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":8000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":0,\"LeftArraySpace\":-3500,\"TopArrayCount\":0,\"TopArraySpace\":3500}]",
        "[{\"BlockType\":\"XFTD\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"None\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":-1,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":-1,\"Top\":-1,\"Bottom\":0,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"STL\",\"Id\":3,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Right\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":300,\"Top\":-1,\"Bottom\":1000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":4,\"HorizontalObjId\":1,\"VerticlObjId\":3,\"HorizontalAlignment\":\"Right\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":300,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":5,\"HorizontalObjId\":3,\"VerticlObjId\":3,\"HorizontalAlignment\":\"Right\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":3500,\"Top\":-1,\"Bottom\":0,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":0,\"LeftArraySpace\":3500,\"TopArrayCount\":2,\"TopArraySpace\":3500},{\"BlockType\":\"SSL\",\"Id\":6,\"HorizontalObjId\":1,\"VerticlObjId\":3,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":0,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"WYL\",\"Id\":7,\"HorizontalObjId\":1,\"VerticlObjId\":6,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":300,\"Right\":-1,\"Top\":0,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":8,\"HorizontalObjId\":6,\"VerticlObjId\":6,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":3500,\"Right\":-1,\"Top\":-1,\"Bottom\":0,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":0,\"LeftArraySpace\":-3500,\"TopArrayCount\":2,\"TopArraySpace\":3500}]",
        "[{\"BlockType\":\"XFTD\",\"Id\":1,\"HorizontalObjId\":0,\"VerticlObjId\":-1,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"None\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":-1,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"DM\",\"Id\":2,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"CenterAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":-1,\"Right\":-1,\"Top\":-1,\"Bottom\":0,\"Width\":4000,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"STL\",\"Id\":3,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"Left\",\"VerticlAlignment\":\"Bottom\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":1000,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":4,\"HorizontalObjId\":1,\"VerticlObjId\":3,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Bottom\",\"Left\":300,\"Right\":-1,\"Top\":-1,\"Bottom\":3500,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":0,\"TopArraySpace\":3500},{\"BlockType\":\"WYL\",\"Id\":5,\"HorizontalObjId\":1,\"VerticlObjId\":0,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Top\",\"Left\":300,\"Right\":-1,\"Top\":3500,\"Bottom\":-1,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":-1,\"TopArraySpace\":-1},{\"BlockType\":\"SSL\",\"Id\":6,\"HorizontalObjId\":1,\"VerticlObjId\":4,\"HorizontalAlignment\":\"LeftAligned\",\"VerticlAlignment\":\"Top\",\"Left\":300,\"Right\":-1,\"Top\":3500,\"Bottom\":-1,\"Width\":-1,\"Height\":-1,\"Rotate\":0,\"LeftArrayCount\":-1,\"LeftArraySpace\":-1,\"TopArrayCount\":0,\"TopArraySpace\":-3500}]"};
        public Polygon2d PlateBuildGroupPolygon;
        public Vector2 Location;
        public double RoomCount;
        public double BuildRotate;
        public string BuildGroupName;
        //建筑物方式
        public PlateBuildGroupType GroupType;
        public DeatilPlateBuildGroupType DeatilPlateBuildType;// A B C D
        public ArrangementArea ArrangeArea;//所属区域
        public DetailRoomType DetailRoomType;//K式房 箱式房
        public BuildingType GroupBuildingType;
        public List<Double> buildsRoomCount; //各栋的房间数量
        public List<PlateBuilding> Elements=new List<PlateBuilding>(); // 
        public Dictionary<Vector2, string> Texts; // 
        /// <summary>
        /// 单位房间的宽度
        /// </summary>
        public double RoomSizeWidth;
        /// <summary>
        /// 单位房间的长度
        /// </summary>
        public double RoomSizeLength;
        /// <summary>
        /// 走廊宽度
        /// </summary>
        public double PassagewaySizeWidth;
        /// <summary>
        /// 楼梯宽度
        /// </summary>
        public double StairSizeWidth;
        public double StairSizeLength;

        public PlateBuildGroup(LcComponentDefinition comDef, string name, int roomcount, PlateBuildGroupType type, BuildingType groupBuildingType) : base(comDef)
        {
            this.Type = LayoutElementType.PlateBuildGroup;
            BuildGroupName = name;
            RoomCount = roomcount;
            GroupType = type;
            GroupBuildingType = groupBuildingType;
        }
        public override Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict = null)
        {
            dict = base.ToDictionary(dict);

            dict.Add(nameof(this.PlateBuildGroupPolygon), this.PlateBuildGroupPolygon);
            dict.Add(nameof(this.Location), this.Location);
            dict.Add(nameof(this.RoomCount), this.RoomCount);
            dict.Add(nameof(this.BuildGroupName), this.BuildGroupName);
            dict.Add(nameof(this.GroupType), this.GroupType);
            dict.Add(nameof(this.GroupBuildingType), this.GroupBuildingType);
            dict.Add(nameof(this.RoomSizeWidth), this.RoomSizeWidth);
            dict.Add(nameof(this.RoomSizeLength), this.RoomSizeLength);
            dict.Add(nameof(this.PassagewaySizeWidth), this.PassagewaySizeWidth);
            dict.Add(nameof(this.StairSizeWidth), this.StairSizeWidth);
            dict.Add(nameof(this.StairSizeLength), this.StairSizeLength);
            return dict;
        }
        public override void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            base.FromDictionary(props);
            this.PlateBuildGroupPolygon = props.GetCurve2d(nameof(this.PlateBuildGroupPolygon)) as Polygon2d;
            this.Location = props.GetVector2(nameof(this.Location));
            this.RoomCount = props.GetDouble(nameof(this.RoomCount));
            this.BuildGroupName = props.GetString(nameof(this.BuildGroupName));
            this.GroupType = (PlateBuildGroupType)props.GetInt(nameof(this.GroupType));
            this.GroupBuildingType = (BuildingType)props.GetInt(nameof(this.GroupBuildingType));
            this.RoomSizeWidth = props.GetDouble(nameof(this.RoomSizeWidth));
            this.RoomSizeLength = props.GetDouble(nameof(this.RoomSizeLength));
            this.PassagewaySizeWidth = props.GetDouble(nameof(this.PassagewaySizeWidth));
            this.StairSizeWidth = props.GetDouble(nameof(this.StairSizeWidth));
            this.StairSizeLength = props.GetDouble(nameof(this.StairSizeLength));

        }

        //添加
        public PlateBuildGroup ResetGroup(PlateArrangeGroup arrangeGroup)
        {
            //PlateBuildGroup plateBuildGroup = new PlateBuildGroup(arrangeGroup.name, (int)arrangeGroup.num, arrangeGroup.GroupType, BuildingType.Work);//BuildingType 需给出
            this.BuildGroupName = arrangeGroup.name;
            this.GroupBuildingType = BuildingType.Work;//这一行不太对
            this.ArrangeArea = arrangeGroup.ArrangeArea;
            this.Reset(arrangeGroup);
            return this;
        }
        //编辑
        PlateBuildGroup ResetEditGroup(PlateArrangeGroup arrangeGroup)
        {
            //PlateBuildGroup plateBuildGroup = new PlateBuildGroup(arrangeGroup.name, (int)arrangeGroup.num, arrangeGroup.GroupType, BuildingType.Work);//BuildingType 需给出
            this.Reset(arrangeGroup);
            return this;
        }
        public void Refresh()
        {

            this.OnPropertyChangedBefore("Parent", null, null);
            this.ResetCache();
            this.OnPropertyChangedAfter("Parent", null, null);

        }
        void Reset(PlateArrangeGroup arrangeGroup)
        {
            this.RoomCount = arrangeGroup.num;
            this.GroupType = arrangeGroup.GroupType;
            this.buildsRoomCount = arrangeGroup.PlateBuildRoomNum;
            this.RoomSizeWidth = arrangeGroup.RoomSizeWidth;
            this.RoomSizeLength = arrangeGroup.RoomSizeLength;
            this.DetailRoomType = arrangeGroup.DetailRoomType;
            this.DeatilPlateBuildType = arrangeGroup.DeatilGroupType;
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
        public List<Line2d> GetLines(List<Line2d> lines, List<Line2d> line2s)
        {
            Line2d line1 = null;
            Line2d line2 = null;
            List<Line2d> ls = new List<Line2d>();
            foreach (var item in lines)
            {
                foreach (var item_ in line2s)
                {

                    if (Line2d.IsPointOn(item.Start, item.End, item_.Start) && Line2d.IsPointOn(item.Start, item.End, item_.End))
                    {
                        line1 = item;
                        line2 = item_;
                    }
                    if (Line2d.IsPointOn(item_.Start, item_.End, item.Start) && Line2d.IsPointOn(item_.Start, item_.End, item.End))
                    {
                        line1 = item_;
                        line2 = item;
                    }
                }
            }
            foreach (var item in lines)
            {
                if (item != line1 && item != line2)
                    ls.Add(item);
            }
            foreach (var item in line2s)
            {
                if (item != line1 && item != line2)
                    ls.Add(item);
            }
            if (line1 != null && line2 != null)
            {
                if (line1.Start.DistanceTo(line2.Start) < line1.Start.DistanceTo(line2.End))
                {
                    ls.Add(new Line2d(line1.Start, line2.Start));
                    ls.Add(new Line2d(line1.End, line2.End));
                }
                else
                {
                    ls.Add(new Line2d(line1.Start, line2.End));
                    ls.Add(new Line2d(line1.End, line2.Start));
                }
            }
            return ls;
        }
        public virtual Curve2dGroupCollection GetShapes()
        {
            if (shapes == null)
            {
                shapes = new Curve2dGroupCollection() { new Curve2dGroup() { Curve2ds = new ListEx<Curve2d>() } };
                Texts = new Dictionary<Vector2, string>();
                //shapes.FirstOrDefault().Curve2ds.Add(PlateBuildGroupPolygon);
                Matrix3 matrix3 = Matrix3.Rotate(this.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), this.Location);
                matrix3.Premultiply(matrix4);
                List<Line2d> ls = new List<Line2d>();
                foreach (var ele in this.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[PlateGroupSet.floorindex];
                    foreach (var item in plateBuildingFloor.Rooms)
                    {
                        shapes.FirstOrDefault().Curve2ds.Add(item.GetRoomPolygon2d());
                        Texts.Add(matrix3.MultiplyPoint(item.GetCenter()), item.Name);
                    }


                    if (ls.Count > 0)
                    {
                        List<Line2d> lsss = new List<Line2d>();
                        foreach (var item in plateBuildingFloor.GetPassagewayLines(plateBuilding.StartStair, plateBuilding.EndStair))
                        {
                            lsss.Add(item.Line);
                        }
                        ls = GetLines(ls, lsss);
                    }
                    else
                    {
                        List<Line2d> lsss = new List<Line2d>();
                        foreach (var item in plateBuildingFloor.GetPassagewayLines(plateBuilding.StartStair, plateBuilding.EndStair))
                        {
                            lsss.Add(item.Line);
                        }

                        ls = lsss;
                    }


                    foreach (var item_ in plateBuilding.Stairs)
                    {
                        shapes.FirstOrDefault().Curve2ds.Add(item_.StairsCellOutline);
                        Texts.Add(matrix3.MultiplyPoint(item_.GetCenter()), "楼梯");

                    }
                }
                foreach (var item in ls)
                {
                    shapes.FirstOrDefault().Curve2ds.Add(item);
                    //   canvas.DrawLine(pen, item.Start, item.End, matrix3);
                }
            }
            return shapes;
        }

        public void InitPassageway()
        {
            List<Line2d> ls = new List<Line2d>();
            if (this.GroupType == PlateBuildGroupType.一)
            {
                if (this.Elements.Count > 0)
                {
                    PlateBuilding plateBuilding = this.Elements[0] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[0];
                    SetPaaageway(plateBuildingFloor.GetPassagewayLines(plateBuilding.StartStair, plateBuilding.EndStair), plateBuilding);
                }
            }
            if (this.GroupType == PlateBuildGroupType.L)
            {
                if (this.Elements.Count > 1)
                {
                    PlateBuilding plateBuilding1 = this.Elements[0] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor1 = plateBuilding1.Floors[0];
                    PlateBuilding plateBuilding2 = this.Elements[1] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor2 = plateBuilding2.Floors[0];

                    List<PlatePassagewayK> line2s = CombinePassageway(plateBuildingFloor1.GetPassagewayLines(plateBuilding1.StartStair, plateBuilding1.EndStair),
                           plateBuildingFloor2.GetPassagewayLines(plateBuilding2.StartStair, plateBuilding2.EndStair));
                    SetPaaageway(CombinePassageway(line2s, plateBuildingFloor2.Rooms.FirstOrDefault().GetRoomline()), plateBuilding1);

                    List<PlatePassagewayK> plateBuilding2pass = CombinePassageway(plateBuildingFloor2.GetPassagewayLines(plateBuilding2.StartStair, plateBuilding2.EndStair),
                          plateBuildingFloor1.GetPassagewayLines(plateBuilding1.StartStair, plateBuilding1.EndStair));
                    SetPaaageway(plateBuilding2pass, plateBuilding2);

                }
            }
            if (this.GroupType == PlateBuildGroupType.U)
            {
                if (this.Elements.Count > 2)
                {
                    PlateBuilding plateBuilding1 = this.Elements[0] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor1 = plateBuilding1.Floors[0];
                    PlateBuilding plateBuilding2 = this.Elements[1] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor2 = plateBuilding2.Floors[0];
                    PlateBuilding plateBuilding3 = this.Elements[2] as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor3 = plateBuilding3.Floors[0];

                    List<PlatePassagewayK> plateBuilding1Passageway = CombinePassageway(plateBuildingFloor1.GetPassagewayLines(plateBuilding1.StartStair, plateBuilding1.EndStair),
                                             plateBuildingFloor2.GetPassagewayLines(plateBuilding2.StartStair, plateBuilding2.EndStair));
                    SetPaaageway(CombinePassageway(plateBuilding1Passageway, plateBuildingFloor2.Rooms.FirstOrDefault().GetRoomline()), plateBuilding1);


                    List<PlatePassagewayK> plateBuilding3Passageway = CombinePassageway(plateBuildingFloor3.GetPassagewayLines(plateBuilding3.StartStair, plateBuilding3.EndStair),
                                            plateBuildingFloor2.GetPassagewayLines(plateBuilding2.StartStair, plateBuilding2.EndStair));
                    SetPaaageway(CombinePassageway(plateBuilding3Passageway, plateBuildingFloor2.Rooms.LastOrDefault().GetRoomline()), plateBuilding3);

                    List<PlatePassagewayK> plateBuilding2Passageway = CombinePassageway(plateBuildingFloor2.GetPassagewayLines(plateBuilding2.StartStair, plateBuilding2.EndStair),
                                              plateBuildingFloor1.GetPassagewayLines(plateBuilding1.StartStair, plateBuilding1.EndStair));
                    SetPaaageway(CombinePassageway(plateBuilding2Passageway, plateBuildingFloor3.GetPassagewayLines(plateBuilding3.StartStair, plateBuilding3.EndStair)), plateBuilding2);
                }
            }

        }
        public void SetPaaageway(List<PlatePassagewayK> lines, PlateBuilding plateBuilding)
        {
            for (int i = 1; i < plateBuilding.Floors.Count(); i++)
            {
                plateBuilding.Floors[i].PassagewayKs = new List<PlatePassagewayK>();
                foreach (var item in lines)
                {
                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                    platePassagewayK.PassagewayKType = item.PassagewayKType;
                    platePassagewayK.Height = 1500;
                    platePassagewayK.Weight = 10;
                    platePassagewayK.Line = item.Line.Clone() as Line2d;
                    plateBuilding.Floors[i].PassagewayKs.Add(platePassagewayK);
                }
            }


        }
        public List<PlatePassagewayK> CombinePassageway(List<PlatePassagewayK> lines, List<PlatePassagewayK> pline2s)
        {
            Line2d line1 = null;
            Line2d line2 = null;
            List<PlatePassagewayK> ls = new List<PlatePassagewayK>();

            List<PlatePassagewayK> newList = new List<PlatePassagewayK>();
            List<Line2d> removeList = new List<Line2d>();
            foreach (var item_line in lines)
            {
                var item = item_line.Line;
                foreach (var item_line_ in pline2s)
                {
                    var item_ = item_line_.Line;
                    if (Line2d.IsPointOn(item.Start, item.End, item_.Start) && Line2d.IsPointOn(item.Start, item.End, item_.End))
                    {
                        ///重复线段
                        line1 = item;
                        line2 = item_;
                        if (item.Start == item_.Start || item.Start == item_.End || item.End == item_.Start || item.End == item_.End)
                        {
                            if ((item.Start == item_.Start && item.End == item_.End) || (item.Start == item_.End && item.End == item_.Start))
                            {

                            }
                            else
                            {
                                if (item.Start == item_.Start)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.End, item_.End);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                    newList.Add(platePassagewayK);
                                }
                                else if (item.Start == item_.End)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.End, item_.Start);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                                else if (item.End == item_.End)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.Start, item_.Start);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                                else if (item.End == item_.Start)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.Start, item_.End);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                            }
                        }
                        else
                        {
                            if (item.Start.DistanceTo(item_.Start) < item.Start.DistanceTo(item_.End))
                            {
                                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                platePassagewayK.Line = new Line2d(item.Start, item_.Start);
                                platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                PlatePassagewayK platePassagewayK1 = new PlatePassagewayK();
                                platePassagewayK1.Line = new Line2d(item.End, item_.End);
                                platePassagewayK1.PassagewayKType = item_line.PassagewayKType;
                                newList.Add(platePassagewayK);
                                newList.Add(platePassagewayK1);
                            }
                            else
                            {
                                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                platePassagewayK.Line = new Line2d(item.Start, item_.End);
                                platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                PlatePassagewayK platePassagewayK1 = new PlatePassagewayK();
                                platePassagewayK1.Line = new Line2d(item.End, item_.Start);
                                platePassagewayK1.PassagewayKType = item_line.PassagewayKType;
                                newList.Add(platePassagewayK);
                                newList.Add(platePassagewayK1);

                            }
                        }
                        removeList.Add(item);
                    }
                    if (Line2d.IsPointOn(item_.Start, item_.End, item.Start) && Line2d.IsPointOn(item_.Start, item_.End, item.End))
                    {
                        removeList.Add(item);
                    }
                }
            }
            foreach (var item in lines)
            {
                if (!removeList.Contains(item.Line))
                {
                    ls.Add(item);
                }
            }
            ls.AddRange(newList);
            return ls;
        }
        public List<PlatePassagewayK> CombinePassageway(List<PlatePassagewayK> lines, List<Line2d> pline2s)
        {
            Line2d line1 = null;
            Line2d line2 = null;
            List<PlatePassagewayK> ls = new List<PlatePassagewayK>();

            List<PlatePassagewayK> newList = new List<PlatePassagewayK>();
            List<Line2d> removeList = new List<Line2d>();
            foreach (var item_line in lines)
            {
                var item = item_line.Line;
                foreach (var item_ in pline2s)
                {

                    if (Line2d.IsPointOn(item.Start, item.End, item_.Start) && Line2d.IsPointOn(item.Start, item.End, item_.End))
                    {
                        ///重复线段
                        line1 = item;
                        line2 = item_;
                        if (item.Start == item_.Start || item.Start == item_.End || item.End == item_.Start || item.End == item_.End)
                        {
                            if ((item.Start == item_.Start && item.End == item_.End) || (item.Start == item_.End && item.End == item_.Start))
                            {

                            }
                            else
                            {
                                if (item.Start == item_.Start)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.End, item_.End);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                    newList.Add(platePassagewayK);
                                }
                                else if (item.Start == item_.End)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.End, item_.Start);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                                else if (item.End == item_.End)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.Start, item_.Start);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                                else if (item.End == item_.Start)
                                {
                                    PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                    platePassagewayK.Line = new Line2d(item.Start, item_.End);
                                    platePassagewayK.PassagewayKType = item_line.PassagewayKType;
                                    newList.Add(platePassagewayK);
                                }
                            }
                        }
                        else
                        {
                            if (item.Start.DistanceTo(item_.Start) < item.Start.DistanceTo(item_.End))
                            {
                                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                platePassagewayK.Line = new Line2d(item.Start, item_.Start);
                                platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                PlatePassagewayK platePassagewayK1 = new PlatePassagewayK();
                                platePassagewayK1.Line = new Line2d(item.End, item_.End);
                                platePassagewayK1.PassagewayKType = item_line.PassagewayKType;
                                newList.Add(platePassagewayK);
                                newList.Add(platePassagewayK1);
                            }
                            else
                            {
                                PlatePassagewayK platePassagewayK = new PlatePassagewayK();
                                platePassagewayK.Line = new Line2d(item.Start, item_.End);
                                platePassagewayK.PassagewayKType = item_line.PassagewayKType;

                                PlatePassagewayK platePassagewayK1 = new PlatePassagewayK();
                                platePassagewayK1.Line = new Line2d(item.End, item_.Start);
                                platePassagewayK1.PassagewayKType = item_line.PassagewayKType;
                                newList.Add(platePassagewayK);
                                newList.Add(platePassagewayK1);

                            }
                        }
                        removeList.Add(item);
                    }
                    if (Line2d.IsPointOn(item_.Start, item_.End, item.Start) && Line2d.IsPointOn(item_.Start, item_.End, item.End))
                    {
                        removeList.Add(item);
                    }
                }
            }
            foreach (var item in lines)
            {
                if (!removeList.Contains(item.Line))
                {
                    ls.Add(item);
                }
            }
            ls.AddRange(newList);
            return ls;
        }


        public override bool IncludedByBox(Polygon2d testPoly, List<RefChildElement> includedChildren = null)
        {
            var thisBox = BoundingBox;
            if (!testPoly.BoundingBox.ContainsBox(thisBox))
                return false;
            if (testPoly.BoundingBox.ContainsBox(thisBox))
                return true;
            if (this.Elements.Count > 0)
            {
                Matrix3 matrix3 = Matrix3.Rotate(this.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), this.Location);
                matrix3.Premultiply(matrix4);

                foreach (var ele in this.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;

                    foreach (PlateRoom item in plateBuilding.Floors[0].Rooms)
                    {
                        Vector2 firstp = null;
                        foreach (Vector2 point in item.GetRoomPolygon2d().Points)
                        {
                            if (firstp != null)
                            {

                                if (Intersect2d.IsPolygonWithLine(testPoly.Points, matrix3.MultiplyPoint(firstp), matrix3.MultiplyPoint(point)))
                                {
                                    return true;
                                }
                            }
                            else
                                firstp = point;
                        }
                        firstp = null;
                        foreach (Vector2 point in item.GetpassagewayLine().Points)
                        {
                            if (firstp != null)
                            {

                                if (Intersect2d.IsPolygonWithLine(testPoly.Points, matrix3.MultiplyPoint(firstp), matrix3.MultiplyPoint(point)))
                                {
                                    return true;
                                }
                            }
                            else
                                firstp = point;
                        }
                    }

                }
                return false;
            }
            else
            {
                Vector2 firstp = null;
                foreach (Vector2 point in this.PlateBuildGroupPolygon.Points)
                {
                    if (firstp != null)
                    {

                        if (Intersect2d.IsPolygonWithLine(testPoly.Points, firstp, point))
                        {
                            return true;
                        }
                    }
                    else
                        firstp = point;

                }
                return false;
            }
        }
        public override bool IntersectWithBox(Polygon2d testPoly, List<RefChildElement> intersectChildren = null)
        {
            var thisBox = BoundingBox;
            if (!thisBox.IntersectsBox(testPoly.BoundingBox) && !thisBox.ContainsBox(testPoly.BoundingBox))
            {
                //如果元素盒子，与多边形盒子不相交，那就可能不相交
                return false;
            }
            if (this.Elements.Count > 0)
            {

                Matrix3 matrix3 = Matrix3.Rotate(this.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), this.Location);
                matrix3.Premultiply(matrix4);
                foreach (var ele in this.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;

                    foreach (PlateRoom item in plateBuilding.Floors[0].Rooms)
                    {
                        Vector2 firstp = null;
                        foreach (Vector2 point in item.GetRoomPolygon2d().Points)
                        {
                            if (firstp != null)
                            {

                                if (Intersect2d.IsPolygonWithLine(testPoly.Points, matrix3.MultiplyPoint(firstp), matrix3.MultiplyPoint(point)))
                                {
                                    return true;
                                }
                            }
                            else
                                firstp = point;
                        }
                        firstp = null;
                        foreach (Vector2 point in item.GetpassagewayLine().Points)
                        {
                            if (firstp != null)
                            {

                                if (Intersect2d.IsPolygonWithLine(testPoly.Points, matrix3.MultiplyPoint(firstp), matrix3.MultiplyPoint(point)))
                                {
                                    return true;
                                }
                            }
                            else
                                firstp = point;
                        }
                    }

                }
                return false;
            }
            else
            {
                Vector2 firstp = null;
                foreach (Vector2 point in this.PlateBuildGroupPolygon.Points)
                {
                    if (firstp != null)
                    {

                        if (Intersect2d.IsPolygonWithLine(testPoly.Points, firstp, point))
                        {
                            return true;
                        }
                    }
                    else
                        firstp = point;

                }
                return false;
            }

        }

        public override Box2 GetBoundingBox()
        {

            if (this.Elements.Count == 0)
            {
                Box2 box2 = new Box2().ExpandByPoints(this.PlateBuildGroupPolygon.GetPoints());
                return box2;
            }
            else
            {
                Matrix3 matrix3 = Matrix3.Rotate(this.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), this.Location);
                matrix3.Premultiply(matrix4);
                Box2 box2 = null;
                foreach (PlateBuilding lc in this.Elements)
                {
                    if (box2 == null)
                    {
                        box2 = lc.GetBoundingBox();
                    }
                    else
                    {
                        box2.Union(lc.GetBoundingBox());
                    }
                }
                Vector2 p1 = matrix3.MultiplyPoint(box2.Min);
                Vector2 p2 = matrix3.MultiplyPoint(box2.Max);
                Vector2 p3 = new Vector2(p1.X, p2.Y);
                Vector2 p4 = new Vector2(p2.X, p1.Y);
                List<Vector2> ps = new List<Vector2>();
                ps.Add(p1);
                ps.Add(p2);
                ps.Add(p3);
                ps.Add(p4);

                return new Box2().ExpandByPoints(ps.ToArray());
            }
        }

        public void PlateBuildGroupInit(DocumentRuntime docRt, double roomSizeWidth, double roomSizeLength, double passagewaySizeWidth, double stairSizeWidth, double stairSizeLength, double floorNum, List<PlateRoom> rooms)
        {
            this.RoomSizeWidth = roomSizeWidth;
            this.RoomSizeLength = roomSizeLength;
            this.PassagewaySizeWidth = passagewaySizeWidth;
            this.StairSizeWidth = stairSizeWidth;
            this.StairSizeLength = stairSizeLength;
            var doc = docRt.Document;
            //    var PlateBuildingDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingGroupDef;
            if (this.GroupType == PlateBuildGroupType.一)
            {
                Vector2 vector2 = new Vector2(0, roomSizeLength + passagewaySizeWidth);

                PlateBuilding plateBuilding = new PlateBuilding(vector2, new Vector2(1, 0), this.GroupBuildingType, this.RoomCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, (int)floorNum, rooms, true, true, false, false);
                //   plateBuilding.Initilize(doc);
                this.Elements.Add(plateBuilding);
                //  this.InsertElement(plateBuilding);
            }
            else if (this.GroupType == PlateBuildGroupType.L)
            {
                int count = (int)this.RoomCount / 2 + (int)this.RoomCount % 2;
                int fistBuildCount = (int)(count / floorNum);
                int secondBuildCount = (int)((this.RoomCount - count) / floorNum);
                PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, true, true, false, false);
                //plateBuilding.Initilize(doc);
                //this.InsertElement(plateBuilding);
                this.Elements.Add(plateBuilding);

                count = (int)this.RoomCount / 2;

                double x = this.RoomSizeLength + this.PassagewaySizeWidth;
                double y = fistBuildCount * this.RoomSizeWidth;
                //     var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, false, true, false, false);
                //plateBuilding2.Initilize(doc);
                //this.InsertElement(plateBuilding2);
                this.Elements.Add(plateBuilding);
                for (int i = 1; i < floorNum; i++)
                {
                    plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, true, false, false);
                    plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, false, false);
                }
                plateBuilding.BuildingFloor = (int)floorNum;
                plateBuilding2.BuildingFloor = (int)floorNum;
            }
            else if (this.GroupType == PlateBuildGroupType.U)
            {
                int count = (int)this.RoomCount / 3 - (int)(1 * floorNum);

                int fistBuildCount = (int)(count / floorNum);
                int secondBuildCount = (int)((this.RoomCount - count * 2) / floorNum);
                int thirdCount = (int)(count / floorNum);



                //List<PlateRoom> krooms = AllotRooms(rooms, count);
                PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, true, false, false, true);

                this.Elements.Add(plateBuilding);

                double x = this.RoomSizeLength + this.PassagewaySizeWidth;
                double y = fistBuildCount * this.RoomSizeWidth;
                //   count = (int)this.RoomCount / 3 + (int)this.RoomCount % 3 + (int)(2 * floorNum);
                //  var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, false, false, false, false);
                this.Elements.Add(plateBuilding2);


                var x2 = x + secondBuildCount * this.RoomSizeWidth + this.RoomSizeLength + this.PassagewaySizeWidth;

                var y2 = y;
                //      var PlateBuildingDef3 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;
                PlateBuilding plateBuilding3 = new PlateBuilding(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, false, true, true, false);

                this.Elements.Add(plateBuilding3);
                for (int i = 1; i < floorNum; i++)
                {
                    plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, false, false, true);
                    plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, false, false, false);
                    plateBuilding3.PlateBuildingAddFloor(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, true, false);
                }
                plateBuilding.BuildingFloor = (int)floorNum;
                plateBuilding2.BuildingFloor = (int)floorNum;
                plateBuilding3.BuildingFloor = (int)floorNum;
            }
            InitPassageway();
        }


        public void PlateBuildGroupInit2(DocumentRuntime docRt, double roomSizeWidth, double roomSizeLength, double passagewaySizeWidth, double stairSizeWidth, double stairSizeLength, double floorNum, List<PlateRoom> rooms, List<PlateBuildingMath> plateBuildingMaths = null)
        {
            this.Elements = new List<PlateBuilding>();
            // this.DeatilPlateBuildType= DeatilPlateBuildGroupType.A;
            this.RoomSizeWidth = roomSizeWidth;
            this.RoomSizeLength = roomSizeLength;
            this.PassagewaySizeWidth = passagewaySizeWidth;
            this.StairSizeWidth = stairSizeWidth;
            this.StairSizeLength = stairSizeLength;
            var doc = docRt.Document;
            //    var PlateBuildingDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;
            bool iniStair = floorNum > 1 ? true : false;
            if (this.GroupType == PlateBuildGroupType.一)
            {
                Vector2 vector2 = new Vector2(0, roomSizeLength + passagewaySizeWidth);

                bool startStair = plateBuildingMaths == null ? iniStair : plateBuildingMaths.First().StartStair;
                bool endStair = plateBuildingMaths == null ? iniStair : plateBuildingMaths.First().EndStair;
                //if (floorNum>)
                //{

                //}
                PlateBuilding plateBuilding = new PlateBuilding(vector2, new Vector2(1, 0), this.GroupBuildingType, this.RoomCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, (int)floorNum, rooms, startStair, endStair, false, false);
                //plateBuilding.Initilize(doc);
                //this.InsertElement(plateBuilding);
                this.Elements.Add(plateBuilding);
            }
            else if (this.GroupType == PlateBuildGroupType.L)
            {
                bool startStair1 = plateBuildingMaths == null ? iniStair : plateBuildingMaths.First().StartStair;
                bool endStair1 = plateBuildingMaths == null ? false : plateBuildingMaths.First().EndStair;
                bool startStair2 = plateBuildingMaths == null ? false : plateBuildingMaths[1].StartStair;
                bool endStair2 = plateBuildingMaths == null ? iniStair : plateBuildingMaths[1].EndStair;

                bool isStartClose1 = false;
                bool isEndClose1 = true;
                bool isStartClose2 = false;
                bool isEndClose2 = false;
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.A)
                {
                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength,
                        this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair1, endStair1, isStartClose1, isEndClose1);
                    //plateBuilding.Initilize(doc);
                    //this.InsertElement(plateBuilding);
                    this.Elements.Add(plateBuilding);

                    //count = (int)this.RoomCount / 2;

                    double x = this.RoomSizeLength + this.PassagewaySizeWidth;
                    double y = fistBuildCount * this.RoomSizeWidth;
                    //   var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength,
                        1, rooms, startStair2, endStair2, isStartClose2, isEndClose2);
                    this.Elements.Add(plateBuilding2);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, true, false, false);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, false, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                }
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.B)
                {
                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    double x0 = 0;
                    double y0 = secondBuildCount * this.RoomSizeWidth;
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(x0, y0), new Vector2(1, 0), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth,
                        this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair1, endStair1, false, false);
                    this.Elements.Add(plateBuilding);

                    //count = (int)this.RoomCount / 2;

                    double x = fistBuildCount * this.RoomSizeWidth + this.PassagewaySizeWidth + this.RoomSizeLength;
                    double y = y0;
                    //    var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(0, -1), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength,
                        1, rooms, startStair2, endStair2, true, false);
                    this.Elements.Add(plateBuilding2);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, true, false, false);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, false, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                }
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.C)
                {
                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength,
                        1, rooms, startStair1, endStair1, false, false);
                    this.Elements.Add(plateBuilding);

                    //count = (int)this.RoomCount / 2;

                    double x = 0;
                    double y = fistBuildCount * this.RoomSizeWidth + this.PassagewaySizeWidth + this.RoomSizeLength;
                    //   var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength,
                        1, rooms, startStair2, endStair2, true, false);
                    this.Elements.Add(plateBuilding2);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, true, false, false);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, false, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                }
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.D)
                {
                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    double x0 = 0;
                    double y0 = secondBuildCount * this.RoomSizeWidth + this.RoomSizeLength + this.PassagewaySizeWidth;
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(x0, y0), new Vector2(1, 0), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength,
                        this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair1, endStair1, false, true);
                    this.Elements.Add(plateBuilding);


                    double x = fistBuildCount * this.RoomSizeWidth;
                    double y = y0 - this.PassagewaySizeWidth - this.RoomSizeLength;
                    //     var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(0, -1), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth,
                        this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair2, endStair2, false, false);
                    this.Elements.Add(plateBuilding2);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, true, false, false);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, false, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                }
            }
            else if (this.GroupType == PlateBuildGroupType.U)
            {
                bool startStair1 = plateBuildingMaths == null ? iniStair : plateBuildingMaths.First().StartStair;
                bool endStair1 = plateBuildingMaths == null ? false : plateBuildingMaths.First().EndStair;
                bool startStair2 = plateBuildingMaths == null ? false : plateBuildingMaths[1].StartStair;
                bool endStair2 = plateBuildingMaths == null ? false : plateBuildingMaths[1].EndStair;
                bool startStair3 = plateBuildingMaths == null ? false : plateBuildingMaths[2].StartStair;
                bool endStair3 = plateBuildingMaths == null ? iniStair : plateBuildingMaths[2].EndStair;



                bool isStartClose1 = false;
                bool isEndClose1 = true;
                bool isStartClose2 = false;
                bool isEndClose2 = false;
                bool isStartClose3 = true;
                bool isEndClose3 = false;
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.A)
                {
                    //  int count = (int)this.RoomCount / 3 - (int)(1 * floorNum);

                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    int thirdCount = (int)(buildsRoomCount[2] / floorNum);



                    //List<PlateRoom> krooms = AllotRooms(rooms, count);
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength,
                        this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair1, endStair1, isStartClose1, isEndClose1);
                    this.Elements.Add(plateBuilding);


                    double x = this.RoomSizeLength + this.PassagewaySizeWidth;
                    double y = fistBuildCount * this.RoomSizeWidth;
                    //   count = (int)this.RoomCount / 3 + (int)this.RoomCount % 3 + (int)(2 * floorNum);
                    // var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount,
                        this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair2, endStair2, isStartClose2, isEndClose2);
                    this.Elements.Add(plateBuilding2);


                    var x2 = x + secondBuildCount * this.RoomSizeWidth + this.RoomSizeLength + this.PassagewaySizeWidth;

                    var y2 = y;
                    //    var PlateBuildingDef3 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;
                    PlateBuilding plateBuilding3 = new PlateBuilding(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount,
                        this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair3, endStair3, isStartClose3, isEndClose3);
                    this.Elements.Add(plateBuilding3);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, false, false, true);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, false, false, false);
                        plateBuilding3.PlateBuildingAddFloor(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, true, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                    plateBuilding3.BuildingFloor = (int)floorNum;
                }
                if (this.DeatilPlateBuildType == DeatilPlateBuildGroupType.B)
                {
                    int fistBuildCount = (int)(buildsRoomCount[0] / floorNum);
                    int secondBuildCount = (int)(buildsRoomCount[1] / floorNum);
                    int thirdCount = (int)(buildsRoomCount[2] / floorNum);



                    //List<PlateRoom> krooms = AllotRooms(rooms, count);
                    PlateBuilding plateBuilding = new PlateBuilding(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount,
                        this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair1, endStair1, false, false);
                    this.Elements.Add(plateBuilding);


                    double x = 0;
                    double y = fistBuildCount * this.RoomSizeWidth + this.RoomSizeLength + this.PassagewaySizeWidth;

                    //  var PlateBuildingDef2 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;

                    PlateBuilding plateBuilding2 = new PlateBuilding(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount,
                        this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair2, endStair2, true, true);
                    this.Elements.Add(plateBuilding2);


                    var x2 = secondBuildCount * this.RoomSizeWidth;

                    var y2 = y - this.RoomSizeLength - this.PassagewaySizeWidth;
                    //   var PlateBuildingDef3 = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildingDef;
                    PlateBuilding plateBuilding3 = new PlateBuilding(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount,
                        this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, 1, rooms, startStair3, endStair3, false, false);
                    this.Elements.Add(plateBuilding3);
                    for (int i = 1; i < floorNum; i++)
                    {
                        plateBuilding.PlateBuildingAddFloor(new Vector2(0, 0), new Vector2(0, 1), this.GroupBuildingType, fistBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, true, false, false, true);
                        plateBuilding2.PlateBuildingAddFloor(new Vector2(x, y), new Vector2(1, 0), this.GroupBuildingType, secondBuildCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, false, false, false);
                        plateBuilding3.PlateBuildingAddFloor(new Vector2(x2, y2), new Vector2(0, -1), this.GroupBuildingType, thirdCount, this.RoomSizeWidth, this.RoomSizeLength, this.PassagewaySizeWidth, this.StairSizeWidth, this.StairSizeLength, i + 1, rooms, false, true, true, false);
                    }
                    plateBuilding.BuildingFloor = (int)floorNum;
                    plateBuilding2.BuildingFloor = (int)floorNum;
                    plateBuilding3.BuildingFloor = (int)floorNum;
                }
            }
            InitPassageway();
        }

        public void ResetPlateBuildings(DocumentRuntime docRt, PlateArrangeGroup plateBuildGroup, List<PlateBuildingMath> plateBuildingMaths)
        {
            List<PlateRoom> kplateRooms = new List<PlateRoom>();
            List<double> Floorscell = new List<double>();
            for (int i = 0; i < (this.Elements.FirstOrDefault() as PlateBuilding).Floors.Count; i++)
            {
                foreach (var item in this.Elements)
                {
                    PlateBuilding plateBuilding = item as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[i];

                    Floorscell.Add(plateBuildingFloor.RoomCount);
                    foreach (var item_ in plateBuildingFloor.Rooms)
                    {
                        kplateRooms.Add((PlateRoom)item_);
                    }
                }
            }
            for (int i = 0; i < this.Elements.Count; i++)
            {
                this.Elements.Remove(this.Elements[i]);
                i--;
            }
            this.ResetEditGroup(plateBuildGroup);
            this.PlateBuildGroupInit2(docRt, Convert.ToDouble(plateBuildGroup.RoomSizeWidth), Convert.ToDouble(plateBuildGroup.RoomSizeLength)
            , Convert.ToDouble(this.PassagewaySizeWidth), Convert.ToDouble(this.StairSizeWidth), Convert.ToDouble(this.StairSizeLength)
            , plateBuildGroup.FloorCount, kplateRooms, plateBuildingMaths);
        }

        public override LcElement Clone()
        {
            return null;
            //PlateBuildGroup plateBuildGroup =
            //    new PlateBuildGroup(this.Name, (int)this.RoomCount, this.GroupType, this.GroupBuildingType);


            //plateBuildGroup.PlateBuildGroupPolygon = this.PlateBuildGroupPolygon;
            //plateBuildGroup.Location = this.Location;
            //return plateBuildGroup;
        }

        public override void Copy(LcElement src)
        {

        }
        public override void Scale(Vector2 basePoint, double scaleFactor)
        {
            return;
        }

        public override void Scale(Vector2 basePoint, Vector2 scaleVector)
        {
            return;
        }
        public override void Mirror(Vector2 axisStart, Vector2 axisEnd)
        {

        }
        public override void Rotate(Vector2 basePoint, double rotateAngle)
        {

        }


    }
}
