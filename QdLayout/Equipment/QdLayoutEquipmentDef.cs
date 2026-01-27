namespace QdLayout
{
    [ComDefCategory("建筑施工图设计.机械设备")]
    [ComClass("场布设备")]
    public partial class QdLayoutEquipmentDef : LcComponentDefinition
    {
        public QdLayoutEquipmentDef() : base()
        {
           
        }
        public override IComponentInstance CreateInstance()
        {
            return new QdLayoutEquipmentInstance(this);
        }
    }
}