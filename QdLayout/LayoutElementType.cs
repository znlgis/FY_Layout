
using LightCAD.Core;

namespace QdLayout
{
    public static class LayoutElementType
    {
        
        public static ElementType Lawn = new ElementType
        {
            Guid = Guid.ParseExact("{63A566EA-3702-A98C-7A6B-8DBEA6B3F41A}", "B").ToLcGuid(),
            Name = "Lawn",
            DispalyName = "草坪",
            ClassType = typeof(QdLawn)
        };
        public static ElementType PlateBuilding = new ElementType
        {
            Guid = Guid.ParseExact("{63A523EA-3702-A98C-7A6B-8DBEA5C3F42B}", "B").ToLcGuid(),
            Name = "PlateBuilding",
            DispalyName = "板房",
            ClassType = typeof(PlateBuilding)
        };
        public static ElementType PlateBuildGroup = new ElementType
        {
            Guid = Guid.ParseExact("{C3A523EA-3702-A98C-7A6B-8DBEA2D2F4D4}", "B").ToLcGuid(),
            Name = "PlateBuildGroup",
            DispalyName = "板房楼栋",
            ClassType = typeof(PlateBuildGroup)
        };
        public static ElementType FoundationPit = new ElementType
        {
            Guid = Guid.ParseExact("{C082E6EA-E099-94AB-50FA-50DF72D286D4}", "B").ToLcGuid(),
            Name = "FoundationPit",
            DispalyName = "基坑",
            ClassType = typeof(QdFoundationPit)
        };

        public static ElementType Fence = new ElementType
        {
            Guid = Guid.ParseExact("{5A9EC5E6-09BF-4DB3-9AD2-DD0DA74F099A}", "B").ToLcGuid(),
            Name = "Fence",
            DispalyName = "围墙",
            ClassType = typeof(QdFence)
        };

        public static ElementType PlanBuild = new ElementType
        {
            Guid = Guid.ParseExact("{5A5EC5E6-06BF-4DB3-4AD2-DD0DA52F078A}", "B").ToLcGuid(),
            Name = "PlanBuild",
            DispalyName = "拟建建筑",
            ClassType = typeof(QdPlanBuild)
        };

        public static ElementType Road = new ElementType
        {
            Guid = Guid.ParseExact("{85AB36C8-FBB1-424B-C7C4-1F92576EC5BD}", "B").ToLcGuid(),
            Name = "Road",
            DispalyName = "硬化地面",
            ClassType = typeof(QdRoad)
        };
        public static ElementType Earthwork = new ElementType
        {
            Guid = Guid.ParseExact("{B20D7BB7-6209-B3DB-2761-E1197E59723E}", "B").ToLcGuid(),
            Name = "Earthwork",
            DispalyName = "土方回填",
            ClassType = typeof(QdEarthwork)
        };
        public static ElementType Berm = new ElementType
        {
            Guid = Guid.ParseExact("{982B310D-627F-9168-6A63-1257A26BDDF8}", "B").ToLcGuid(),
            Name = "Berm",
            DispalyName = "出土道路",
            ClassType = typeof(QdBerm)
        };
        public static ElementType Harden = new ElementType
        {
            Guid = Guid.ParseExact("{897700F4-F50B-62EE-3900-59D967BEBC85}", "B").ToLcGuid(),
            Name = "Harden",
            DispalyName = "路面硬化",
            ClassType = typeof(QdHarden)
        };
        public static ElementType Barrier = new ElementType
        {
            Guid = Guid.ParseExact("{6A8E5173-8D53-4542-8272-5B1A6680AFD2}", "B").ToLcGuid(),
            Name = "Barrier",
            DispalyName = "防护栏杆",
            ClassType = typeof(QdBarrier)
        };

        public static ElementType Site = new ElementType
        {
            Guid = Guid.ParseExact("{15AC9FC1-9BC3-4D11-DFDB-3426FAE73336}", "B").ToLcGuid(),
            Name = "Site",
            DispalyName = "场地",
            ClassType = typeof(QdSite)
        };
        public static ElementType Ground = new ElementType
        {
            Guid = Guid.ParseExact("{85AB36C8-FBB1-424B-C7C4-1F92576EC5BD}", "B").ToLcGuid(),
            Name = "Ground",
            DispalyName = "硬化地面",
            ClassType = typeof(QdGround)
        };
        public static ElementType PropertyLine = new ElementType
        {
            Guid = Guid.ParseExact("{6FF93D96-A3D3-DAC7-D720-8497DA8E3A9A}", "B").ToLcGuid(),
            Name = "PropertyLine",
            DispalyName = "用地红线",
            ClassType = typeof(QdPropertyLine)
        };
        public static ElementType OpenLine = new ElementType
        {
            Guid = Guid.ParseExact("{6FE950F0-9E13-FA8A-E216-AAE0D40BECBC}", "B").ToLcGuid(),
            Name = "OpenLine",
            DispalyName = "开门边线",
            ClassType = typeof(QdOpenLine)
        };

        public static ElementType LayoutEquipment = new ElementType
        {
            Guid = Guid.ParseExact("{552E316C-ADB2-43A7-AC77-9E0069ACEDC8}", "B").ToLcGuid(),
            Name = "LayoutEquipment",
            DispalyName = "场布设备",
            ClassType = typeof(QdLayoutEquipment)
        };

        public static ElementType[] All = new ElementType[]
        {
            Lawn,FoundationPit,Road,Earthwork,Berm,Harden,Site,PropertyLine,Fence,PlateBuilding,PlateBuildGroup,OpenLine
        };
    }
}
