using LightCAD.DBUtility;
using LightCAD.DBUtility.Model;
using QdLayout;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LightCAD.DBUtility.PlateArrange;
using static ThreeJs4Net.PropertyBinding;

namespace QdLayout
{
    
    public interface IArrangementWindow : IWindow
    {
        public event Action<List<PlateArrangeGroup>> Save;
        public event Action<List<PlateArrangeGroup>,int> InsertCAD;
        public event Action<PlateArrangeGroup> Edit;
        public event Action Init;
        public void ResetDGVByPA(DataTable dataTable);
        public void ResetDGV(DataTable dataTable);
    }
    public class ArrangementRuntime()
    {
        public IArrangementWindow Control { get; set; }
        public static int ArrangeIndex;
        public static ArrangementArea BuildiType;//区域
        public static List<PlateArrangeGroup> PlateGroups = new List<PlateArrangeGroup>();
        private DataTable dt;
        public void ShowWindow(ArrangementArea buildingType,DataTable dataTable)
        {
            dt = dataTable;
            BuildiType = buildingType;
            Control = new ArrangementWindow( buildingType);
            Control.Save += Control_Save;
            Control.InsertCAD += Control_InsertCAD;
            //Control.Edit += Control_Edit;
            Control.Init += Control_Init;
           AppRuntime.UISystem.ShowWindow(Control, AppRuntime.App.ActiveWindow);
        }
        private void Control_Init()//根据数据库 生成 List<PlateBuildGroup>
        {
            //var dt1 = SQLiteUtility.GetPlateArrange();
            //if (dt1.Rows.Count == 0)
                Control.ResetDGV(this.dt);
            //else
            //    Control.ResetDGVByPA(dt1);
        }

        private void Control_Edit(PlateArrangeGroup plateBuildGroup)
        {
            //进入编辑界面
            EditArrangeRuntime editArrangeRuntime = new EditArrangeRuntime();
            editArrangeRuntime.ShowWindow(win =>
            {
                ////更新数据库
                //var plateGroup = win.group;
                //Control.ResetDGV(plateGroup);
            });
        }

        private void Control_InsertCAD(List<PlateArrangeGroup> plateArranges, int index)
        {
            PlateGroups = RefreshPlateBuildRooms(plateArranges);
            ArrangeIndex = index;
            CommandCenter.ActiveInstance.Execute("SetBuildGroup");
            
        }
        public static List<PlateArrangeGroup> RefreshPlateBuildRooms(List<PlateArrangeGroup> plateArranges)
        {
            foreach (var item in plateArranges)
            {
                switch (item.GroupType)
                {
                    case PlateBuildGroupType.一:
                        item.PlateBuildRoomNum = new List<double>
                        {
                            item.num
                        };
                        break;
                    case PlateBuildGroupType.L:
                        var a = 2;
                        var b = 3;
                        item.PlateBuildRoomNum = new List<double>
                        {
                            (int)item.num * a/(a+b),
                            item.num-((int)item.num * a/(a+b)) ,
                        };
                        break;
                    case PlateBuildGroupType.U:
                        var a1 = 2;
                        var b1 = 3;
                        item.PlateBuildRoomNum = new List<double>
                        {
                            (int)item.num * a1/(a1*2+b1),
                            item.num-(((int)item.num * a1/(a1*2+b1))*2) ,
                            (int)item.num * a1/(a1*2+b1),
                        };
                        break;
                }
            }
            return plateArranges;
        }
        private void Control_Save(List<PlateArrangeGroup> arrangeGroups)
        {
            //保存到数据库
            var dt = SQLiteUtility.GetPlateArrange();
            for (int i = 0; i < arrangeGroups.Count; i++)
            {
                var group = arrangeGroups[i];
                //如果数据库有对应的id 则更新
                //否则新建
                if (String.IsNullOrEmpty(group.id))
                {
                    group.id = Guid.NewGuid().ToString();
                    SQLiteUtility.CreateProjectModel(ConverseTo(group));
                }
                else
                {
                    SQLiteUtility.UpdateProjectModel(ConverseTo(group));
                }
            }
        }


        PlateArrangeModel ConverseTo(PlateArrangeGroup group)
        {
            return new PlateArrangeModel()
            {
                id = group.id,
                Name = group.name,
                GroupType = group.GroupType.ToString(),
                DeatilGroupType = group.DeatilGroupType.ToString(),
                DetailRoomType = group.DetailRoomType.ToString(),
                RoomSizeLength = group.RoomSizeLength.ToString(),
                RoomSizeWidth = group.RoomSizeWidth.ToString(),
                TotalStandardRoomCount = group.num.ToString(),
                FloorNum = group.FloorCount.ToString(),
            };
        }
    }
}
