using LightCAD.MathLib;
using LightCAD.Runtime;
using OpenTK.Graphics.OpenGL;
using QdLayout.PlateHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ThreeJs4Net;
using TS;
using static QdLayout.PlateGroupSet;

namespace QdLayout
{

    partial class PlateGroupSet : Form
    {
        public List<PlateRoom> plateRooms = new List<PlateRoom>();
        public List<PlateBuildGroup> BuildGroups = new List<PlateBuildGroup>();
        public bool minGrid = false;
        public PlateBuilding PlateHouse = null;
        public Matrix3 matrix3 = null;
        public List<GridCell> checkcelles = new List<GridCell>();
        public DocumentRuntime docrt;
        public PlateBuildGroup localplateBuildGroup = null;
        public double rommwith = 3000;
        public double roomlength = 6000;
        public double passwaywidth = 1000;
        public double stairwidth = 1200;
        public double statirlength = 4200;
        public int screenX = 50;
        public int screenY = 350;
        public double scale = 60;
        public System.Windows.Forms.Label label111;
        public System.Windows.Forms.Label labe2222;
        public System.Windows.Forms.Label labe3333;
        public System.Windows.Forms.Label labe4444;
        public System.Windows.Forms.Label labletext;
        public class BuildGroupIndexs
        {
            public int index;
            public PlateBuildGroup group;
            public int floors;
            public bool Draw;
            public int ScreenY;
        }
        public List<BuildGroupIndexs> BuildGroupsindexs = new List<BuildGroupIndexs>();
        public PlateGroupSet(DocumentRuntime docr, List<PlateBuildGroup> plateBuildGroups)
        {
            InitializeComponent();
            graphics = this.DrawPlan.CreateGraphics();

            matrix3 = Matrix3.GetMirror(new Vector2(0, 0), new Vector2(800, 0));
            this.docrt = docr;
            this.BuildGroups = plateBuildGroups;
            //if (plateBuildGroup.Elements.Count() > 0)
            //{
            //    PlateBuildingGroupDraw(this.localplateBuildGroup, 0);
            //}
            plateRoomsReset();
            foreach (var item in plateBuildGroups)
            {
                int i = 0;
                localplateBuildGroup = item;//以后要注释 临时
                if (item.Elements.Count > 0)
                {
                    foreach (var floor in (item.Elements.FirstOrDefault() as PlateBuilding).Floors)
                    {

                        cbxBuildGroups.Items.Add(item.BuildGroupName  + "(" + floor.FloorNum.ToString() + "层)");

                        BuildGroupIndexs buildGroupIndex = new BuildGroupIndexs();
                        buildGroupIndex.group = item;
                        buildGroupIndex.floors = floor.FloorNum;
                        buildGroupIndex.index = i;
                        BuildGroupsindexs.Add(buildGroupIndex);
                        i++;
                    }
                }
            }
            
            if (PlateGroupSet.floorindex == 1)
            {
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                comboBox3.SelectedIndex = 1;
            }
            label111 = new System.Windows.Forms.Label();
            label111.Size = new Size(100, 2);
            label111.Location = new Point(12, 51);
            label111.BackColor = System.Drawing.Color.Red;
            label111.Visible = false;
            label111.TabIndex = 2;
            this.DrawPlan.Controls.Add(label111);
            labe2222 = new System.Windows.Forms.Label();
            labe2222.Size = new Size(100, 2);
            labe2222.Location = new Point(12, 51);
            labe2222.BackColor = System.Drawing.Color.Red;
            labe2222.Visible = false;
            labe2222.TabIndex = 2;
            this.DrawPlan.Controls.Add(labe2222);
            labe3333 = new System.Windows.Forms.Label();
            labe3333.Size = new Size(100, 2);
            labe3333.Location = new Point(12, 51);
            labe3333.BackColor = System.Drawing.Color.Red;
            labe3333.Visible = false;
            labe3333.TabIndex = 2;
            this.DrawPlan.Controls.Add(labe3333);
            labe4444 = new System.Windows.Forms.Label();
            labe4444.Size = new Size(100, 2);
            labe4444.Location = new Point(12, 51);
            labe4444.BackColor = System.Drawing.Color.Red;
            labe4444.Visible = false;
            labe4444.TabIndex = 2;
            this.DrawPlan.Controls.Add(labe4444);
            labletext = new System.Windows.Forms.Label();
            labletext.AutoSize = true;
            labletext.Location = new Point(12, 51);
            labletext.ForeColor = System.Drawing.Color.Red;
            labletext.BackColor = System.Drawing.Color.Transparent;
            labletext.Visible = false;
            labletext.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labletext.TabIndex = 2;
            this.DrawPlan.Controls.Add(labletext);
            SetupTimer();
            for  (int ix= 0 ; ix < cbxBuildGroups.Items.Count; ix++)
            {
                cbxBuildGroups.SetItemCheckState(ix, CheckState.Checked);
            }
        }
        public Graphics graphics;

        private System.Windows.Forms.Timer timer;

        private void SetupTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10; // 每 100 毫秒刷新一次
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            GraphicsDrawLines(this.DrawLines);
            foreach (var item in textinfos)
            {
                DrawText(item.local, item.text);
            }
        }
        public void InitDraw(PlateBuilding plateHouse)
        {

        }
        public Vector2 staticmove = new Vector2(0, 0);
        public void PlateBuildingGroupDraw()
        {
            graphics.Clear(System.Drawing.Color.Black);
            this.DrawLines = new List<DrawLine>();
            this.textinfos = new List<textinfo>();
            int DrawScreenY = (int)this.screenY;
            foreach (var item_ in BuildGroupsindexs)
            {
                if (!item_.Draw) continue;
                item_.ScreenY = DrawScreenY;
                List<Line2d> ls = new List<Line2d>();
                //   var drawset = item.GetDrawSet();

                // int indexx = cbxBuildGroups.Items.IndexOf(Checkeitem);
                foreach (var item in item_.group.Elements)
                {
                    PlateBuilding plateBuilding = item as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[item_.floors - 1];

                    DrawPolygon(plateBuildingFloor.GetEmptyRoomLine(), DrawScreenY);
                    if (ls.Count > 0)
                    {
                        List<Line2d> lsss = new List<Line2d>();
                        foreach (var item_p in plateBuildingFloor.GetPassagewayLines(plateBuilding.StartStair, plateBuilding.EndStair))
                        {
                            lsss.Add(item_p.Line);
                        }
                        ls = GetLines(ls, lsss);
                    }
                    else
                    {
                        List<Line2d> lsss = new List<Line2d>();
                        foreach (var item_p in plateBuildingFloor.GetPassagewayLines(plateBuilding.StartStair, plateBuilding.EndStair))
                        {
                            lsss.Add(item_p.Line);
                        }

                        ls = lsss;
                    }
                    foreach (var item_S in plateBuilding.Stairs)
                    {
                        DrawPolygon(item_S.StairsCellOutline, DrawScreenY);
                        //DrawText(item_.GetCenter(), "楼梯", movedir);
                    }
                    foreach (var item_R in plateBuildingFloor.Rooms)
                    {
                        DrawPolygon(item_R.GetRoomPolygon2d(), DrawScreenY);
                        textinfo textinfo = new textinfo();
                        PointF location = GetPointF(item_R.GetCenter(), DrawScreenY);
                        textinfo.local = location;
                        textinfo.text = item_R.Name;
                        textinfos.Add(textinfo);

                        if (!string.IsNullOrEmpty(item_R.RoomConfig))
                        {
                            var roomConfig = PlateRoomConfigManager.GetRoomConfig(item_.group.GroupType, item_R.Name, item_R.RoomConfig);
                            var polygon2ds = item_R.GetRoomConfigPolygon2d(roomConfig);
                            foreach (var polygon in polygon2ds)
                            {
                                DrawPolygon(polygon, DrawScreenY);
                            }
                        }
                    }
                }
                DrawPolygon(ls, DrawScreenY);
                GraphicsDrawLines(this.DrawLines);
                foreach (var item in textinfos)
                {
                    DrawText(item.local, item.text);
                }

                DrawScreenY = DrawScreenY + this.screenY + 50;
            }

        }
        public class textinfo
        {
            public string text;
            public PointF local;
        }
        public class DrawLine
        {
            public PointF S;
            public PointF E;
        }
        public List<DrawLine> DrawLines = new List<DrawLine>();
        public List<textinfo> textinfos = new List<textinfo>();
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
        public void DrawText(PointF center, string text)
        {
            Vector2 dir = new Vector2(0, 0);
            var fontSize = 8;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            System.Drawing.Font font = new System.Drawing.Font("宋体", 8);

            Brush brush = Brushes.White;

            graphics.DrawString(text, font, brush, center, format);
        }

        public void DrawPolygon(Polygon2d polygon2d, int moveScreen)
        {
            if (polygon2d == null) return;
            Vector2 vector2d = null;
            foreach (var item in polygon2d.Points)
            {
                if (vector2d != null)
                {
                    DrawLine drawLine = new DrawLine();
                    drawLine.S = GetPointF(vector2d, moveScreen);
                    drawLine.E = GetPointF(item, moveScreen);
                    this.DrawLines.Add(drawLine);
                }
                vector2d = item;
            }
            DrawLine drawLine1 = new DrawLine();
            drawLine1.S = GetPointF(vector2d, moveScreen);
            drawLine1.E = GetPointF(polygon2d.Points.FirstOrDefault(), moveScreen);
            this.DrawLines.Add(drawLine1);
        }

        public void DrawPolygon(List<Line2d> lines, int moveScreen)
        {

            Pen pen = new Pen(System.Drawing.Color.White, 1);

            foreach (var item in lines)
            {
                DrawLine drawLine = new DrawLine();
                drawLine.S = GetPointF(item.Start, moveScreen);
                drawLine.E = GetPointF(item.End, moveScreen);
                this.DrawLines.Add(drawLine);
            }

        }
        public void GraphicsDrawLines(List<DrawLine> lines)
        {
            Pen pen = new Pen(System.Drawing.Color.White, 1);
            foreach (var item in lines)
            {
                graphics.DrawLine(pen, item.S, item.E);
            }

        }
        //public void DrawSelectPolygon(Polygon2d polygon2d, Vector2 move, System.Drawing.Color color)
        //{
        //    if (polygon2d == null)
        //        return;
        //    Pen pen = new Pen(color, 1);
        //    Vector2 vector2d = null;
        //    foreach (var item in polygon2d.Points)
        //    {
        //        if (vector2d != null)
        //        {
        //            graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(item.X + move.X, item.Y + move.Y)));
        //        }
        //        vector2d = item;
        //    }
        //    graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(polygon2d.Points.FirstOrDefault().X + move.X, polygon2d.Points.FirstOrDefault().Y + move.Y)));
        //}

        public PointF GetPointF(Vector2 vector2d, int movescreenY)
        {
            Vector2 vector2 = matrix3.MultiplyPoint(new Vector2(vector2d.X / scale, vector2d.Y / scale));
            PointF pointF = new PointF();
            pointF.X = (float)vector2.X + (float)screenX;
            pointF.Y = (float)vector2.Y + (float)movescreenY;
            return pointF;
        }



        private void DrawPlan_Paint(object sender, PaintEventArgs e)
        {
            //Pen pen = new Pen(System.Drawing.Color.White, 1);
            //Point p1 = new Point(0, 0);
            //Point p2 = new Point(1000, 1000);


            //graphics.DrawLine(pen, p1, p2);
            //graphics.DrawLine(pen, p2, p3);
            //graphics.DrawLine(pen, p3, p4);
            // InitDraw(PlateHouse);

        }

        private void BtGetMinGrid_Click(object sender, System.EventArgs e)
        {
            if (minGrid)
            {
                minGrid = false;
            }
            else
            {
                minGrid = true;
            }
            DrawPlan.Invalidate();
        }

        private void BtCombineGrid_Click(object sender, System.EventArgs e)
        {

        }
        public class selectRoom
        {
            public PlateRoom room;
            public PlateBuildGroup group;
            public int floorIndex;

        }
        public selectRoom plateRoom1 = null;
        public selectRoom plateRoom2 = null;
        private PlateRoom GetPlateRoomByPoint(Vector2 point, PlateBuildGroup plateHouse, int index)
        {
            List<Line2d> ls = new List<Line2d>();

            Vector2 p = point;
            foreach (var item in plateHouse.Elements)
            {
                PlateBuilding plateBuilding = item as PlateBuilding;
                PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[index];

                foreach (var item_ in plateBuildingFloor.Rooms)
                {

                    var Polygon = item_.GetRoomPolygon2d();
                    if (PointInRoom(Polygon, p))
                    {
                        if (plateRoom1 == null)
                        {
                            // if (plateRoom2!=null&&plateRoom2.room == item_)
                            {
                                selectRoom selectRoom = new selectRoom();
                                selectRoom.group = plateHouse;
                                selectRoom.room = item_;
                                selectRoom.floorIndex = index;
                                plateRoom1 = selectRoom;
                            }
                            //else
                            //{
                            //    plateRoom2 = null;
                            //}
                        }
                        else if (plateRoom2 == null)
                        {
                            if (plateRoom1.room != item_)
                            {
                                selectRoom selectRoom = new selectRoom();
                                selectRoom.group = plateHouse;
                                selectRoom.room = item_;
                                selectRoom.floorIndex = index;

                                plateRoom2 = selectRoom;
                            }
                            else
                            {
                                plateRoom1 = null;
                            }
                        }
                        else
                        {
                            if (plateRoom1.room == item_)
                            {

                                plateRoom1 = null;
                            }
                            else
                                if (plateRoom2.room == item_)
                            {

                                plateRoom2 = null;
                            }
                        }

                        return item_;
                    }


                }
            }
            return null;
        }

        public bool PointInRoom(Polygon2d polygon, Vector2 point)
        {
            double minx = polygon.Points[0].X;

            double maxx = polygon.Points[0].X;

            double maxY = polygon.Points[0].Y;
            double minY = polygon.Points[0].Y;
            foreach (var item in polygon.Points)
            {
                if (item.X < minx)
                {
                    minx = item.X;
                }
                if (item.Y < minY)
                {
                    minY = item.Y;
                }
                if (item.X > maxx)
                {
                    maxx = item.X;
                }
                if (item.Y > maxY)
                {
                    maxY = item.Y;
                }
            }
            if (point.X >= minx && point.Y >= minY &&
                point.X <= maxx && point.Y <= maxY)
                return true;
            return false;
        }
        private void DrawPlan_Click(object sender, System.EventArgs e)
        {
            //Vector2 mouseLocation = new Vector2(0, 0);
            //Point p = ((System.Windows.Forms.MouseEventArgs)e).Location;
            //p = new Point(p.X - 50, p.Y - 400);
            //Vector2 vector2d = new Vector2(p.X, p.Y);
            //vector2d = matrix3.Invert().MultiplyPoint(vector2d);
            //mouseLocation = new Vector2(vector2d.X * scale, vector2d.Y * scale);

            //string lname = "";
            //GetPlateRoomByPoint(mouseLocation, localplateBuildGroup, comboBox1.SelectedIndex);


        }

        private void button8_Click(object sender, System.EventArgs e)
        {

        }


        private void plateRoomsReset()
        {
            plateRooms.Clear();
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("会议室", BuildingType.Work, 2);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 3; i++)
            {
                PlateRoom plateRoom = new PlateRoom("监理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }

            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("总监理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 4; i++)
            {
                PlateRoom plateRoom = new PlateRoom("业主", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("业主负责", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("业主经理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }

            ////1


            for (int i = 0; i < 2; i++)
            {
                PlateRoom plateRoom = new PlateRoom("监理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("审计", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("设计", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("业主", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("物资", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("机电", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("工程部", BuildingType.Work, 2);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("质检", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("安装经理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 3; i++)
            {
                PlateRoom plateRoom = new PlateRoom("安装", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("业主", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("党员活动室", BuildingType.Work, 2);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("书记", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("项目经理", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("指挥长", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }

            ///2

            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("大会议室", BuildingType.Work, 4);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("接待室", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("安全", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }



            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("资料室", BuildingType.Work, 1);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("商务部", BuildingType.Work, 2);
                plateRooms.Add(plateRoom);
            }
            for (int i = 0; i < 1; i++)
            {
                PlateRoom plateRoom = new PlateRoom("设计技术部", BuildingType.Work, 3);
                plateRooms.Add(plateRoom);
            }
        }

        private void label9_Click(object sender, System.EventArgs e)
        {

        }
        public void PlateBuildingGroupInit()
        {
            string lname = "";
            PlateBuildGroup plateBuildGroup = this.localplateBuildGroup;
            if (plateBuildGroup == null) return;
            plateBuildGroup.Elements.Clear();

            double kcount = plateBuildGroup.RoomCount;
            List<PlateRoom> kplateRooms = new List<PlateRoom>();
            foreach (var item in plateRooms)
            {
                kplateRooms.Add((PlateRoom)item);

            }
            plateBuildGroup.PlateBuildGroupInit2(this.docrt, Convert.ToDouble(this.rommwith), Convert.ToDouble(this.roomlength)
                , Convert.ToDouble(this.passwaywidth), Convert.ToDouble(this.stairwidth), Convert.ToDouble(this.statirlength)
                , 2, kplateRooms);
            PlateBuildingGroupDraw();
        }
        private void button4_Click(object sender, System.EventArgs e)
        {
            PlateBuildingGroupInit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string lname = "";
            PlateBuildingGroupDraw();
        }

        private void button12_Click(object sender, System.EventArgs e)
        {
            // comboBox1.Items.Add(comboBox1.Items.Count + 1);
        }


        public void ResetPlateBuildingGroupByFloor(PlateBuildGroup plateHouse, int index, int type)
        {
            List<PlateRoom> kplateRooms = new List<PlateRoom>();
            List<double> Floorscell = new List<double>();
            foreach (var item in plateHouse.Elements)
            {
                PlateBuilding plateBuilding = item as PlateBuilding;
                PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[index];

                Floorscell.Add(plateBuildingFloor.RoomCount);
                foreach (var item_ in plateBuildingFloor.Rooms)
                {
                    kplateRooms.Add((PlateRoom)item_);
                }
            }

            if (type == 1)
            {
                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2.room), plateRoom1.room);
            }
            if (type == 2)
            {

                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2.room) + 1, plateRoom1.room);
            }
            if (type == 3)
            {

                int p1 = kplateRooms.IndexOf(plateRoom1.room);
                int p2 = kplateRooms.IndexOf(plateRoom2.room);
                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(p1, plateRoom2.room);
                kplateRooms.RemoveAt(p2);
                kplateRooms.Insert(p2, plateRoom1.room);

            }
            bool pipei = true;
            List<List<PlateRoom>> kRooms = new List<List<PlateRoom>>();
            foreach (var item1 in Floorscell)
            {
                double number = item1;
                List<PlateRoom> plateRooms = new List<PlateRoom>();

                foreach (var item in kplateRooms)
                {
                    if (number == 0)
                        break;
                    if (number >= item.CellNumber)
                    {
                        number -= item.CellNumber;
                        plateRooms.Add(item);
                    }
                    else
                    {
                        if (number != 0)
                        {
                            pipei = false;
                            break;
                        }
                    }
                }
                foreach (var item in plateRooms)
                {
                    kplateRooms.Remove(item);
                }

                if (!pipei)
                {
                    break;
                }
                kRooms.Add(plateRooms);
            }
            if (pipei)
            {
                foreach (var item in plateHouse.Elements)
                {
                    PlateBuilding plateBuilding = item as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[index];
                    plateBuildingFloor.ResetPlateBuildingFloor(plateBuilding.BuildingLocation, kRooms[plateHouse.Elements.IndexOf(item)], plateBuilding.BuildingDirect,
                    plateBuilding.RoomSizeWidth, plateBuilding.RoomSizeLength, plateHouse.PassagewaySizeWidth);
                    plateBuildingFloor.Rooms = kRooms[plateHouse.Elements.IndexOf(item)];
                }
            }
        }
        public void ResetPlateBuildingGroupByGroup(PlateBuildGroup plateHouse, int type)
        {
            List<PlateRoom> kplateRooms = new List<PlateRoom>();
            List<double> Floorscell = new List<double>();
            for (int i = 0; i < (plateHouse.Elements.FirstOrDefault() as PlateBuilding).Floors.Count; i++)
            {
                foreach (var item in plateHouse.Elements)
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

            if (type == 1)
            {
                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2.room), plateRoom1.room);
            }
            if (type == 2)
            {

                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2.room) + 1, plateRoom1.room);
            }
            if (type == 3)
            {

                int p1 = kplateRooms.IndexOf(plateRoom1.room);
                int p2 = kplateRooms.IndexOf(plateRoom2.room);
                kplateRooms.Remove(plateRoom1.room);
                kplateRooms.Insert(p1, plateRoom2.room);
                kplateRooms.RemoveAt(p2);
                kplateRooms.Insert(p2, plateRoom1.room);

            }
            bool pipei = true;
            string errorRoom = "";
            List<List<PlateRoom>> kRooms = new List<List<PlateRoom>>();
            foreach (var item1 in Floorscell)
            {
                double number = item1;
                List<PlateRoom> plateRooms = new List<PlateRoom>();

                foreach (var item in kplateRooms)
                {
                    if (number == 0)
                        break;
                    if (number >= item.CellNumber)
                    {
                        number -= item.CellNumber;
                        plateRooms.Add(item);
                    }
                    else
                    {
                        if (number != 0)
                        {
                            errorRoom = item.Name;
                            pipei = false;
                            break;
                        }
                    }
                }
                foreach (var item in plateRooms)
                {
                    kplateRooms.Remove(item);
                }

                if (!pipei)
                {
                    break;
                }
                kRooms.Add(plateRooms);
            }
            if (pipei)
            {
                int Index = 0;
                for (int i = 0; i < (plateHouse.Elements.FirstOrDefault() as PlateBuilding).Floors.Count; i++)
                {
                    foreach (var item in plateHouse.Elements)
                    {
                        PlateBuilding plateBuilding = item as PlateBuilding;
                        PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[i];
                        plateBuildingFloor.ResetPlateBuildingFloor(plateBuilding.BuildingLocation, kRooms[Index], plateBuilding.BuildingDirect,
                        plateBuilding.RoomSizeWidth, plateBuilding.RoomSizeLength, plateHouse.PassagewaySizeWidth);
                        //    plateBuildingFloor.Rooms = kRooms[Index];
                        Index++;
                    }
                }
            }
            else
            {
                MessageBox.Show(errorRoom + "无法依次往后顺延！");
            }
        }
        private void 交换房间ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //if (plateRoom1 != null && plateRoom2 != null)
            //{
            //    if (plateRoom1.CellNumber == plateRoom2.CellNumber)
            //    {

            //        ResetPlateBuildingGroup(this.localplateBuildGroup, 0, 3);
            //        PlateBuildingGroupDraw();
            //        plateRoom1 = null;
            //        plateRoom2 = null;
            //    }
            //}
        }

        private void 插入房间前ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //if (plateRoom1 != null && plateRoom2 != null)
            //{


            //    ResetPlateBuildingGroup(this.localplateBuildGroup, 0, 1);
            //    PlateBuildingGroupDraw();
            //    plateRoom1 = null;
            //    plateRoom2 = null;
            //}
        }

        private void 插入房间后ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //if (plateRoom1 != null && plateRoom2 != null)
            //{


            //    ResetPlateBuildingGroup(this.localplateBuildGroup, 0, 2);
            //    PlateBuildingGroupDraw();
            //    plateRoom1 = null;
            //    plateRoom2 = null;

            //}
        }
        public static int floorindex = 0;
        private void button2_Click_1(object sender, System.EventArgs e)
        {
            floorindex = comboBox3.SelectedIndex;
            this.localplateBuildGroup.Refresh();
            this.Close();
        }

        private Point? downPos;
        private void DrawPlan_MouseDown(object sender, MouseEventArgs e)
        {
            Vector2 mouseLocation = new Vector2(0, 0);
            Point p = ((System.Windows.Forms.MouseEventArgs)e).Location;
            downPos = p;
            plateRoom1 = null;
            plateRoom2 = null;
            foreach (var item in BuildGroupsindexs)
            {
                if (item.Draw)
                {
                    if (p.Y <= item.ScreenY && p.Y > item.ScreenY - screenY)
                    {
                        p = new Point(p.X - (int)screenX, p.Y - (int)item.ScreenY);
                        Vector2 vector2d = new Vector2(p.X, p.Y);
                        vector2d = matrix3.Clone().Invert().MultiplyPoint(vector2d);
                        mouseLocation = new Vector2(vector2d.X * scale, vector2d.Y * scale);

                        string lname = "";
                        GetPlateRoomByPoint(mouseLocation, item.group, item.floors - 1);

                        if (plateRoom1 != null)
                        {
                            var center = plateRoom1.room.GetCenter().Clone();
                            Vector2 vector2 = matrix3.MultiplyPoint(new Vector2(center.X / scale, center.Y / scale));
                            var roomCen = new Point((int)vector2.X + (int)screenX, (int)vector2.Y + item.ScreenY);
                            drawMoveHouse(roomCen);
                            label111.Visible = true;
                            labe2222.Visible = true;
                            labe3333.Visible = true;
                            labe4444.Visible = true;
                            labletext.Visible = true;
                        }
                        else
                        {
                            label111.Visible = false;
                            labe2222.Visible = false;
                            labe3333.Visible = false;
                            labe4444.Visible = false;
                            labletext.Visible = false;
                        }
                    }
                }
            }

        }

        private void DrawPlan_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPos == null)
                return;

            Point p = ((System.Windows.Forms.MouseEventArgs)e).Location;
            p = new Point(p.X, p.Y);

            if (MathEx.FloatEQ(downPos.Value.X, p.X, 1) && MathEx.FloatEQ(downPos.Value.Y, p.Y))
                return;

            label111.Visible = true;
            labe2222.Visible = true;
            labe3333.Visible = true;
            labe4444.Visible = true;
            labletext.Visible = true;

            drawMoveHouse(p);

            //  Vector2 vector2d = new Vector2(p.X, p.Y);
            //vector2d = matrix3.Invert().MultiplyPoint(vector2d);
            // mouseLocation = new Vector2(vector2d.X * scale, vector2d.Y * scale);
            // PlateBuildingGroupDraw(this.localplateBuildGroup, comboBox1.SelectedIndex);
            //   graphics.DrawLine(pen, p, new PointF(p.X + 10, p.Y + 10));
        }
        private void DrawPlan_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = ((System.Windows.Forms.MouseEventArgs)e).Location;
            if (MathEx.FloatEQ(downPos.Value.X, p.X, 1) && MathEx.FloatEQ(downPos.Value.Y, p.Y))
            {
                if (this.plateRoom1 != null && this.plateRoom2 == null)
                {
                    changeRoomConfig(plateRoom1);
                }
                downPos = null;
                return;
            }

            if (plateRoom1 != null)
            {
                Vector2 mouseLocation = new Vector2(0, 0);
                foreach (var item in BuildGroupsindexs)
                {
                    if (item.Draw)
                    {
                        if (p.Y <= item.ScreenY && p.Y > item.ScreenY - screenY)
                        {
                            p = new Point(p.X - (int)screenX, p.Y - (int)item.ScreenY);
                            Vector2 vector2d = new Vector2(p.X, p.Y);
                            vector2d = matrix3.Invert().MultiplyPoint(vector2d);
                            mouseLocation = new Vector2(vector2d.X * scale, vector2d.Y * scale);

                            string lname = "";
                            GetPlateRoomByPoint(mouseLocation, item.group, item.floors - 1);

                        }
                    }
                }


                if (plateRoom1 != null && plateRoom2 != null)
                {
                    if (plateRoom1.group == plateRoom2.group && plateRoom1.floorIndex == plateRoom2.floorIndex)
                    {
                        /// 同一个楼层房间拖拽
                        ResetPlateBuildingGroupByFloor(plateRoom2.group, plateRoom1.floorIndex, GetResetType(mouseLocation));
                    }
                    else if (plateRoom1.group == plateRoom2.group && plateRoom1.floorIndex != plateRoom2.floorIndex)
                    {
                        /// 同一个楼层房间拖拽
                        ResetPlateBuildingGroupByGroup(plateRoom2.group, GetResetType(mouseLocation));
                    }
                    PlateBuildingGroupDraw();
                }
            }
            label111.Visible = false;
            labe2222.Visible = false;
            labe3333.Visible = false;
            labe4444.Visible = false;
            labletext.Visible = false;
            this.plateRoom1 = null;
            this.plateRoom2 = null;
            graphics.Clear(System.Drawing.Color.Black);
            graphics.Clear(System.Drawing.Color.Black);
            graphics.Clear(System.Drawing.Color.Black);
            downPos = null;
        }
        public int GetResetType(Vector2 vector2)
        {
            Vector2 center = this.plateRoom2.room.GetCenter();
            if (plateRoom2.room.Direct.X > 0)
            {

                if (Math.Abs(vector2.X - center.X) < (plateRoom2.room.RoomWidth * plateRoom2.room.CellNumber) / 4)
                {
                    return 3;
                }
                else
                if (vector2.X < center.X)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
             if (plateRoom2.room.Direct.Y > 0)
            {
                if (Math.Abs(vector2.Y - center.Y) < (plateRoom2.room.RoomWidth * plateRoom2.room.CellNumber) / 4)
                {
                    return 3;
                }
                else
                if (vector2.Y < center.Y)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                if (Math.Abs(vector2.Y - center.Y) < (plateRoom2.room.RoomWidth * plateRoom2.room.CellNumber) / 4)
                {
                    return 3;
                }
                else
              if (vector2.Y > center.Y)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        public void drawMoveHouse(Point p)
        {
            Pen pen = new Pen(System.Drawing.Color.White, 1);


            if (this.plateRoom1 != null)
            {
                var width = (int)((plateRoom1.room.CellNumber * plateRoom1.room.RoomWidth) / scale);
                var length = (int)(plateRoom1.room.RoomLength / scale);
                if (plateRoom1.room.Direct.X > 0)
                {
                    Vector2 vector2d = null;
                    labletext.Text = plateRoom1.room.Name;
                    labletext.Location = new Point(p.X - (plateRoom1.room.Name.Length / 2) * 12 - 5, (int)(p.Y - 6));//-5为了和原位置的文字重合
                    //scale
                    label111.Size = new Size(width, 2);
                    label111.Location = new Point(p.X - width / 2, p.Y + length / 2);
                    labe2222.Size = new Size(2, length);
                    labe2222.Location = new Point(p.X + width / 2, p.Y - length / 2);
                    labe3333.Size = new Size(width, 2);
                    labe3333.Location = new Point(p.X - width / 2, p.Y - length / 2);
                    labe4444.Size = new Size(2, length);
                    labe4444.Location = new Point(p.X - width / 2, p.Y - length / 2);
                }
                else if (plateRoom1.room.Direct.Y > 0)
                {
                    Vector2 vector2d = null;
                    labletext.Text = plateRoom1.room.Name;
                    labletext.Location = new Point(p.X - (plateRoom1.room.Name.Length / 2) * 12 - 5, (int)(p.Y - 6));//-5为了和原位置的文字重合

                    label111.Size = new Size(length, 2);
                    label111.Location = new Point(p.X - length / 2, p.Y + width / 2);
                    labe2222.Size = new Size(2, width);
                    labe2222.Location = new Point(p.X + length / 2, p.Y - width / 2);
                    labe3333.Size = new Size(length, 2);
                    labe3333.Location = new Point(p.X - length / 2, p.Y - width / 2);
                    labe4444.Size = new Size(2, width);
                    labe4444.Location = new Point(p.X - length / 2, p.Y - width / 2);
                }
                else
                {
                    Vector2 vector2d = null;
                    labletext.Text = plateRoom1.room.Name;
                    labletext.Location = new Point(p.X - (plateRoom1.room.Name.Length / 2) * 12 - 5, (int)(p.Y - 6)); //-5为了和原位置的文字重合

                    label111.Size = new Size(length, 2);
                    label111.Location = new Point(p.X - length / 2, p.Y + width / 2);
                    labe2222.Size = new Size(2, width);
                    labe2222.Location = new Point(p.X + length / 2, p.Y - width / 2);
                    labe3333.Size = new Size(length, 2);
                    labe3333.Location = new Point(p.X - length / 2, p.Y - width / 2);
                    labe4444.Size = new Size(2, width);
                    labe4444.Location = new Point(p.X - length / 2, p.Y - width / 2);
                }

            }
        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void cbxBuildGroups_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void cbxBuildGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == System.Windows.Forms.CheckState.Unchecked)
            {
                BuildGroupsindexs[e.Index].Draw = false;
            }
            else
            {
                BuildGroupsindexs[e.Index].Draw = true;
            }
            PlateBuildingGroupDraw();
        }

        private async void btnImportRoomConfig_Click(object sender, System.EventArgs e)
        {
            if (this.plateRoom1 == null)
                return;

            var paths = await AppRuntime.UISystem.OpenFileDialog(true, "门窗配置文件(*.ldwg)|*.ldwg");
            if (paths == null || paths.Length == 0)
                return;

            var plateGrpType = plateRoom1.group.GroupType;
            var roomName = plateRoom1.room.Name;
            foreach (var path in paths)
            {
                var roomConfigName = System.IO.Path.GetFileNameWithoutExtension(path);
                var roomConfig = new RoomConfig()
                {
                    PlateBuildType = plateGrpType,
                    RoomName = roomName,
                    ConfigName = roomConfigName,
                    ConfigFilePath = path,
                };

                PlateRoomConfigManager.InsertRoomConfig(roomConfig);
            }
            PlateRoomConfigManager.Save();
            changeRoomConfig(plateRoom1);
        }

        private void changeRoomConfig(selectRoom selectRoom)
        {
            this.flpRoomConfigs.Controls.Clear();

            var plateGrpType = selectRoom.group.GroupType;
            var roomName = selectRoom.room.Name;

            var roomConfigs = PlateRoomConfigManager.GetRoomConfig(plateGrpType, roomName);
            if (roomConfigs == null || roomConfigs.Count == 0)
                return;

            void changeRoomConfig(RoomConfig roomConfig)
            {
                if (selectRoom.room.RoomConfig != roomConfig?.ConfigName)
                {
                    selectRoom.room.RoomConfig = roomConfig?.ConfigName;
                    PlateBuildingGroupDraw();
                }
            }

            var curRoomConfig = selectRoom.room.RoomConfig;
            foreach (var roomConfig in roomConfigs)
            {
                var configName = roomConfig.ConfigName;
                var roomConfigControl = new RoomConfigControl();
                roomConfigControl.RoomConfigName = configName;
                roomConfigControl.RoomConfig = roomConfig;
                roomConfigControl.SelectedAction = changeRoomConfig;

                if (!string.IsNullOrEmpty(curRoomConfig) && curRoomConfig == configName)
                {
                    roomConfigControl.Checked = true;
                }


                this.flpRoomConfigs.Controls.Add(roomConfigControl);
            }
        }
    }
}
