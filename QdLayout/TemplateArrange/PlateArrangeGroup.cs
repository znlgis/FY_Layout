using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utils;

namespace QdLayout
{
    public enum ArrangementArea
    {
        WorkArea,//办公区
        WorkerArea,//工人生活区
    }
    public class PlateArrangeGroup()
    {
        public string id;
        public string name;
        public PlateBuildGroupType GroupType;//1 U L
        public DeatilPlateBuildGroupType DeatilGroupType; // A型 B型 C D 
        public DetailRoomType DetailRoomType;//K式房 箱式房
        public int FloorCount;//楼层
        public ArrangementArea ArrangeArea;//所属区域
        public int TemplateNumber;//模板序号 
        /// <summary>
        /// 单位房间的宽度
        /// </summary>
        public double RoomSizeWidth;
        /// <summary>
        /// 单位房间的长度
        /// </summary>
        public double RoomSizeLength;
        public double num;//标准间总数
        public List<PlateRoom> room;
        public List<double> PlateBuildRoomNum;//各栋的房间数量


        public PlateArrangeGroup Copy(PlateArrangeGroup group)
        {
            id = group.id;
            name = group.name;
            GroupType = group.GroupType;
            DeatilGroupType = group.DeatilGroupType;
            DetailRoomType = group.DetailRoomType;
            FloorCount = group.FloorCount;
            RoomSizeWidth = group.RoomSizeWidth;
            RoomSizeLength = group.RoomSizeLength;
            num = group.num;
            room = group.room?.Clone();
            PlateBuildRoomNum = group.PlateBuildRoomNum?.Clone();
            return this;
        }
        public PlateArrangeGroup Clone()
        {
            var newObj = new PlateArrangeGroup().Copy(this);
            return newObj;
        }
    }

    public class PlateBuildingMath
    {
        public bool StartStair, EndStair;
        public int FloorCount;//楼层数量

        public PlateBuildingMath Copy(PlateBuildingMath group)
        {
            StartStair = group.StartStair;
            EndStair = group.EndStair;
            FloorCount = group.FloorCount;
            return this;
        }
        public PlateBuildingMath Clone()
        {
            var newObj = new PlateBuildingMath().Copy(this);
            return newObj;
        }
    }
}
