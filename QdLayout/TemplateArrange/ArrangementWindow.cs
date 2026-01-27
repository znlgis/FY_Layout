using Subro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LightCAD.Drawing.ElementAction;

namespace QdLayout
{

    public partial class ArrangementWindow : Form, IArrangementWindow
    {
        private List<PlateArrangeGroup> plateGroups = new List<PlateArrangeGroup>();
        public bool IsActive { get; set; }
        private ArrangementArea arrangeArea;
        public Type WinType => this.GetType();
        public ArrangementWindow()
        {
            InitializeComponent();
        }
        public ArrangementWindow(ArrangementArea buildingType) : this()
        {
            InitUI(buildingType);
        }

        void InitUI(ArrangementArea buildingType)
        {
            this.tabControl1.SelectedIndex = 1;
            this.dgArrange.AutoGenerateColumns = false;
            this.dgArrange.AllowUserToAddRows = false;//关闭自动产生行
            this.dgArrange.RowHeadersVisible = false;//第一列空白列关闭 
            this.arrangeArea = buildingType;
            plateGroups = new List<PlateArrangeGroup>();
            if (buildingType == ArrangementArea.WorkArea)
            {
                p3.Image = Properties.Resources.办3;
                p2.Image = Properties.Resources.办2;
                p1.Image = Properties.Resources.办1;
            }
            else if (buildingType == ArrangementArea.WorkerArea)
            {
                p3.Image = Properties.Resources.工3;
                p2.Image = Properties.Resources.工2;
                p1.Image = Properties.Resources.工1;
            }
        }

        #region 旧代码
        public ArrangementWindow(List<PlateBuildGroup> plateBuildGroups) : this()
        {
            //this.plateBuildGroups = plateBuildGroups;
            //InitUI(plateBuildGroups);
            //plateGroups = new List<PlateArrangeGroup>();
        }
        void InitUI(List<PlateBuildGroup> plateBuildGroups)
        {
            for (int i = 0; i < plateBuildGroups.Count; i++)
            {
                var group = plateBuildGroups[i];
                this.dgArrange.Rows[i].Tag = group;
                for (int j = 0; j < this.dgArrange.Columns.Count; j++)
                {
                    switch (j)
                    {
                        case 0://楼栋
                            this.dgArrange.Rows[i].Cells[j].Value = group.GroupBuildingType;
                            break;
                        case 1://房型  需根据 PlateRoom确定
                            this.dgArrange.Rows[i].Cells[j].Value = RoomType.Normal;
                            break;
                        case 2://房间总数
                            this.dgArrange.Rows[i].Cells[j].Value = group.RoomCount;
                            break;
                        case 3://排布方式
                            this.dgArrange.Rows[i].Cells[j].Value = group.GroupType;
                            break;
                    }
                }
            }
        }
        #endregion
        public event Action<PlateArrangeGroup> Edit;
        public event Action<List<PlateArrangeGroup>> Save;
        public event Action<List<PlateArrangeGroup>, int> InsertCAD;
        public event Action Init;

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            plateGroups = GetGroups();
            Save?.Invoke(plateGroups);
        }

        List<PlateArrangeGroup> GetGroups()//更新tag
        {
            List<PlateArrangeGroup> groups = new List<PlateArrangeGroup>();
            for (int i = 0; i < this.dgArrange.Rows.Count; i++)
            {
                if (ResetPlateArrangeGroup(this.dgArrange.Rows[i]))
                {
                    var tag = this.dgArrange.Rows[i].Tag as PlateArrangeGroup;
                    groups.Add(tag);
                }
            }
            return groups;
        }
        bool ResetPlateArrangeGroup(DataGridViewRow row)
        {
            var plateArrangeGroup = row.Tag as PlateArrangeGroup;
            if (plateArrangeGroup == null)
            {
                return false;
            }
            //排布方式
            if (row.Cells["ArrangementMode"].Value.ToString() == "一")
            {
                plateArrangeGroup.GroupType = PlateBuildGroupType.一;
            }
            else if (row.Cells["ArrangementMode"].Value.ToString() == "L")
            {
                plateArrangeGroup.GroupType = PlateBuildGroupType.L;
            }
            else if (row.Cells["ArrangementMode"].Value.ToString() == "U")
            {
                plateArrangeGroup.GroupType = PlateBuildGroupType.U;
            }
            //房间类型
            var text = row.Cells["Room"].Value.ToString();
            if (text.Contains("K式"))
            {
                plateArrangeGroup.DetailRoomType = DetailRoomType.KTypeRoom;
            }
            else if (text.Contains("箱式"))
            {
                plateArrangeGroup.DetailRoomType = DetailRoomType.BoxTypeRoom;
            }
            //房间规格
            var text1 = row.Cells["Size"].Value.ToString().Split("*");
            if (text1.Length == 2)
            {
                plateArrangeGroup.RoomSizeWidth = Convert.ToDouble(text1[0]);
                plateArrangeGroup.RoomSizeLength = Convert.ToDouble(text1[1]);
            }
            //房间数量
            var text2 = row.Cells["Data"].Value.ToString();
            plateArrangeGroup.num = Convert.ToDouble(text2);
            //楼栋
            var text3 = row.Cells["Building"].Value.ToString();
            plateArrangeGroup.name = text3;
            return true;
        }

        private void btnInsertCad_Click(object sender, System.EventArgs e)
        {
            //更新各楼栋的数量 先按照比例计算
            plateGroups = GetGroups();
            //RefreshPlateBuildRooms();
            var index = CalIndex();
            InsertCAD?.Invoke(this.plateGroups, index);
            this.Close();
        }
   
       

        int CalIndex()
        {
            int index = 0;
            if (cb1.Checked)
            {
                index = 0;
            }
            else if (cb2.Checked)
            {
                index = 1;
            }
            else if (cb3.Checked)
            {
                index = 2;
            }
            return index;
        }
        private void dgArrange_DoubleClick(object sender, System.EventArgs e)
        {
            //var dg = sender as DataGridView;
            //var item = dg.SelectedCells;
            //if (item != null && item.Count == 1)
            //{
            //    var columnIndex = item[0].ColumnIndex;
            //    var rowIndex = item[0].RowIndex;
            //    var headerText = dg.Columns[columnIndex].HeaderText;
            //    var row = dg.Rows[rowIndex];
            //    if (headerText == "编辑")
            //    {
            //        ////判断是否是编辑列
            //        Edit?.Invoke(row.Tag as PlateArrangeGroup);
            //    }
            //}
        }

        private void ArrangementWindow_Load(object sender, System.EventArgs e)
        {
            Init?.Invoke();
        }

        public void ResetDGV(DataTable dataTable)
        {
            foreach (var item in dataTable.Rows)
            {
                PlateArrangeGroup builds_ = new PlateArrangeGroup();
                builds_.FloorCount = 2;
                builds_.ArrangeArea = this.arrangeArea;

                var count = Convert.ToInt32(((System.Data.DataRow)item)["roomCount"].ToString());
                var sizeCount = Convert.ToInt32(((System.Data.DataRow)item)["roomSize"].ToString());
                int houseCount = 0;
                var coun = ((System.Data.DataRow)item)["roomCount2"]?.ToString();
                if (!String.IsNullOrEmpty(coun))
                {
                    houseCount = Convert.ToInt32(coun.ToString());
                }
                
                string name = ((System.Data.DataRow)item).ItemArray[4].ToString();
                var buildName = ((System.Data.DataRow)item).ItemArray[3].ToString();
                if (buildName.Contains("食堂楼"))
                {
                    builds_.FloorCount = 1;
                }
                if (buildName == "卫浴室")
                {
                    AddDormitoryBuilding(count);
                }
                else
                {
                    if (plateGroups.Where(x => x.name == buildName).Count() > 0)
                    {
                        builds_ = plateGroups.Where(x => x.name == buildName).FirstOrDefault();
                    }
                    else
                    {
                        builds_.room = new List<PlateRoom>();
                        builds_.name = buildName;
                        InitPlateArrangeGroup(builds_);
                        plateGroups.Add(builds_);
                    }
                    ResetGroup(count, sizeCount, builds_, name, BuildingType.Work);
                }
                if (houseCount>0 )
                {
                   builds_ = new PlateArrangeGroup();
                    AddDormitoryBuilding(houseCount);
                }


                void AddDormitoryBuilding(int count)//宿舍楼
                {
                    builds_.FloorCount = 1;
                    if (plateGroups.Where(x => x.name == "宿舍楼").Count() > 0)
                    {
                        builds_ = plateGroups.Where(x => x.name == "宿舍楼").FirstOrDefault();
                    }
                    else
                    {
                        builds_.room = new List<PlateRoom>();
                        builds_.name = "宿舍楼";
                        InitPlateArrangeGroup(builds_);
                        plateGroups.Add(builds_);
                    }
                    ResetGroup(count, sizeCount, builds_, name, BuildingType.House);
                }
            }
   
            if (arrangeArea == ArrangementArea.WorkerArea)
            {
                List<PlateArrangeGroup> groupList = new List<PlateArrangeGroup>();
                for (int i = 0; i < plateGroups.Count; i++)
                {
                    var group = plateGroups[i];
                    if (group.room.Count > 10)
                    {
                        int groupSize = 10;
                        for (int m = 0; m < group.room.Count; m += groupSize)
                        {
                            PlateArrangeGroup plate = group.Clone();
                            plate.room.Clear();
                            plate.room.AddRange(group.room.Skip(m).Take(groupSize).ToLcList());
                            plate.num = 0;
                            foreach (PlateRoom plateRoom in plate.room)
                            {
                                plate.num += plateRoom.CellNumber;
                            }
                            groupList.Add(plate);
                        }
                    }
                }
                //int groupSize = 50; //每组数据100条
                //for (int i = 0; i < list.Count; i += groupSize)
                //{
                //    //去除数据 其中Skip 表示跳过多少条数据  Take表示获取多少条数据
                //    groupList.Add(list.Skip(i).Take(groupSize).ToList());
                //}
                plateGroups = groupList;
            }
            ResetDgv();

            void ResetGroup(int count, int sizeCount, PlateArrangeGroup builds_, string name, BuildingType buildingType)
            {
                builds_.num += count * sizeCount;
                for (int i = 0; i < count; i++)
                {
                    PlateRoom plateRoom = new PlateRoom();
                    plateRoom.Name = name;
                    plateRoom.CellNumber = sizeCount;
                    plateRoom.RoomType = RoomType.Normal;
                    plateRoom.RoomBuildingType = buildingType;
                    builds_.room.Add(plateRoom);
                }
            }
        }
        void ResetDgv()
        {
            int j = 0;
            foreach (var iterm in plateGroups)
            {
                this.dgArrange.Rows.Add();
                this.dgArrange.Rows[j].Cells["Building"].Value = iterm.name;
                this.dgArrange.Rows[j].Cells["Data"].Value = iterm.num;
                var text = $"{iterm.RoomSizeWidth}*{iterm.RoomSizeLength}";
                if (!(this.dgArrange.Rows[j].Cells["Size"] as DataGridViewComboBoxCell).Items.Contains(text))
                {
                    (this.dgArrange.Rows[j].Cells["Size"] as DataGridViewComboBoxCell).Items.Add(text);
                }
                this.dgArrange.Rows[j].Cells["Size"].Value = text;
                var text1 = "K式房";
                switch (iterm.DetailRoomType)
                {
                    case DetailRoomType.BoxTypeRoom:
                        text1 = "箱式房";
                        break;
                }

                this.dgArrange.Rows[j].Cells["Room"].Value = text1;
                this.dgArrange.Rows[j].Cells["ArrangementMode"].Value = iterm.GroupType.ToString();
                this.dgArrange.Rows[j].Tag = iterm;
                j++;
            }
        }
        public void ResetDGV(PlateArrangeGroup arrangeGroup)
        {
            //for (int i = 0;i<this.dgArrange.Rows.Count;i++)
            //{
            //    var tag = this.dgArrange.Rows[i].Tag as PlateArrangeGroup;
            //    if (tag.name == arrangeGroup.name)
            //    {
            //        this.dgArrange.Rows[i].Cells["ArrangementMode"].Value = arrangeGroup.GroupType.ToString();
            //        this.dgArrange.Rows[i].Cells["Room"].Value = arrangeGroup.DetailRoomType;
            //        this.dgArrange.Rows[i].Cells["Size"].Value = $"{arrangeGroup.RoomSizeLength}*{arrangeGroup.RoomSizeWidth}";
            //        this.dgArrange.Rows[i].Cells["Building"].Value = arrangeGroup.name;
            //        break;
            //    }
            //}
        }

        void InitPlateArrangeGroup(PlateArrangeGroup builds_)
        {
            builds_.DetailRoomType = DetailRoomType.KTypeRoom;
            builds_.RoomSizeLength = 6000;
            builds_.RoomSizeWidth = 3000;
            if (builds_.name == "宿舍楼" || builds_.name == "食堂楼")
            {
                builds_.GroupType = PlateBuildGroupType.一;
            }
            else
            {
                builds_.GroupType = PlateBuildGroupType.L;
            }
        }

        public void ResetDGVByPA(DataTable dataTable)
        {
            plateGroups = new List<PlateArrangeGroup>();
            foreach (var item in dataTable.Rows)
            {
                PlateArrangeGroup builds_ = new PlateArrangeGroup();
                builds_.id = ((System.Data.DataRow)item)["id"].ToString();
                var count = Convert.ToInt32(((System.Data.DataRow)item)["buildingFloorNum"].ToString());
                builds_.num = count;
                builds_.RoomSizeWidth = Convert.ToInt32(((System.Data.DataRow)item)["roomWidth"].ToString());
                builds_.RoomSizeLength = Convert.ToInt32(((System.Data.DataRow)item)["roomLength"].ToString());
                builds_.name =((System.Data.DataRow)item)["buildingType"].ToString();
                builds_.FloorCount = Convert.ToInt32(((System.Data.DataRow)item)["floorNum"].ToString());
                var groupType = ((System.Data.DataRow)item)["detailGroupType"].ToString();
                var roomType = ((System.Data.DataRow)item)["detailRoomType"].ToString();
                var buildType = ((System.Data.DataRow)item)["plateBuildGroupType"].ToString();
                builds_.DeatilGroupType = (DeatilPlateBuildGroupType)Enum.Parse(typeof(DeatilPlateBuildGroupType), groupType);
                builds_.GroupType = (PlateBuildGroupType)Enum.Parse(typeof(PlateBuildGroupType), roomType);
                builds_.DetailRoomType = (DetailRoomType)Enum.Parse(typeof(DetailRoomType), buildType);
                plateGroups.Add(builds_);
            }
            ResetDgv();

        }
    }
}
