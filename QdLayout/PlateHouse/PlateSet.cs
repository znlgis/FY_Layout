using LightCAD.MathLib;
using QdLayout.PlateHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QdLayout
{
    partial class PlateSet : Form
    {
        public List<PlateRoom> plateRooms = new List<PlateRoom>();
        public List<PlateBuildGroup> BuildGroups = new List<PlateBuildGroup>();
        public bool minGrid = false;
        public PlateBuilding PlateHouse = null;
        public Matrix3 matrix3 = null;
        public List<GridCell> checkcelles = new List<GridCell>();
        public DocumentRuntime docrt;
        public PlateSet(DocumentRuntime docrt)
        {
            InitializeComponent();
            graphics = this.DrawPlan.CreateGraphics();
            //this.PlateHouse = plateHouse;
            matrix3 = Matrix3.GetMirror(new Vector2(0, 0), new Vector2(800, 0));
            this.docrt = docrt;
            //matrix3.Multiply(Matrix3.GetMove(new Vector2(0, 0), new Vector2(10, -440)));
            //  matrix3.Multiply(Matrix3.GetScale(0.0006));

        }
        public Graphics graphics;

        private System.Windows.Forms.Timer timer;

        private void SetupTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 每 100 毫秒刷新一次
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            DrawPlan.Invalidate();
        }
        public void InitDraw(PlateBuilding plateHouse)
        {
            //foreach (var item in plateHouse.Passageways)
            //{
            //    DrawPolygon(item.PassagewayOutline);
            //    Box2 box = new Box2(item.PassagewayOutline.Points[0], item.PassagewayOutline.Points[2]);
            //    DrawText(box.Center, "走廊");
            //    if (minGrid)
            //    {
            //        foreach (var item_ in item.GridCells)
            //        {
            //            DrawPolygon(item_.polygon2D);
            //        }
            //    }
            //}

            //foreach (var item in plateHouse.Stairs)
            //{
            //    DrawPolygon(item.StairsCellOutline);
            //    Box2 box = new Box2(item.StairsCellOutline.Points[0], item.StairsCellOutline.Points[2]);
            //    DrawText(box.Center, "楼梯");
            //    if (minGrid)
            //    {
            //        foreach (var item_ in item.GridCells)
            //        {
            //            DrawPolygon(item_.polygon2D);
            //        }
            //    }
            //}

            //foreach (var item in plateHouse.Rooms)
            //{
            //    DrawPolygon(item.Outline);
            //    Box2 box = new Box2(item.Outline.Points[0], item.Outline.Points[2]);
            //    DrawText(box.Center, "房间");
            //    if (minGrid)
            //    {
            //        foreach (var item_ in item.GridCellIds)
            //        {
            //            DrawPolygon(item_.polygon2D);
            //        }
            //    }
            //}
        }
        public void PlateBuildingGroupDraw(PlateBuildGroup plateHouse, int index)
        {
            graphics.Clear(System.Drawing.Color.Black);
            List<Line2d> ls = new List<Line2d>();
            Vector2 movedir = new Vector2(0, 0);
            if (plateHouse.GroupType == PlateBuildGroupType.一)
            {
                movedir = new Vector2(0, (plateHouse.RoomSizeLength + plateHouse.PassagewaySizeWidth));
            }
            foreach (var item in plateHouse.Elements)
            {
                PlateBuilding plateBuilding = item as PlateBuilding;
                PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[index];

                DrawPolygon(plateBuildingFloor.GetEmptyRoomLine(), movedir);
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
                foreach (var item_ in plateBuilding.Stairs)
                {
                    DrawPolygon(item_.StairsCellOutline, movedir);
                    //DrawText(item_.GetCenter(), "楼梯", movedir);
                }
                foreach (var item_ in plateBuildingFloor.Rooms)
                {
                    matrix3 = Matrix3.GetMirror(new Vector2(0, 0), new Vector2(800, 0));

                    DrawPolygon(item_.GetRoomPolygon2d(), movedir);

                    // DrawPolygon(item_.GetpassagewayLine(), movedir);
                    DrawText(item_.GetCenter(), item_.Name, movedir);

                }
            }
            DrawPolygon(ls, movedir);
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
        public void DrawText(Vector2 center, string text, Vector2 move)
        {
            Vector2 c = new Vector2(center.X - 300, center.Y + 60);
            System.Drawing.Font font = new System.Drawing.Font("宋体", 12);
            Brush brush = Brushes.White;
            PointF location = GetPointF(new Vector2(c.X + move.X, c.Y + move.Y));
            graphics.DrawString(text, font, brush, location);
        }
        public void DrawPolygon(Polygon2d polygon2d, Vector2 move)
        {
            if (polygon2d == null)
                return;
            Pen pen = new Pen(System.Drawing.Color.White, 1);
            Vector2 vector2d = null;
            foreach (var item in polygon2d.Points)
            {
                if (vector2d != null)
                {
                    graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(item.X + move.X, item.Y + move.Y)));
                }
                vector2d = item;
            }
            graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(polygon2d.Points.FirstOrDefault().X + move.X, polygon2d.Points.FirstOrDefault().Y + move.Y)));
        }
        public void DrawPolygon(List<Line2d> lines, Vector2 move)
        {
            Pen pen = new Pen(System.Drawing.Color.White, 1);

            foreach (var item in lines)
            {
                Vector2 vector2d = item.Start;
                Vector2 vectorEnd = item.End;
                graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(vectorEnd.X + move.X, vectorEnd.Y + move.Y)));


            }

        }
        public void DrawSelectPolygon(Polygon2d polygon2d, Vector2 move, System.Drawing.Color color)
        {
            if (polygon2d == null)
                return;
            Pen pen = new Pen(color, 1);
            Vector2 vector2d = null;
            foreach (var item in polygon2d.Points)
            {
                if (vector2d != null)
                {
                    graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(item.X + move.X, item.Y + move.Y)));
                }
                vector2d = item;
            }
            graphics.DrawLine(pen, GetPointF(new Vector2(vector2d.X + move.X, vector2d.Y + move.Y)), GetPointF(new Vector2(polygon2d.Points.FirstOrDefault().X + move.X, polygon2d.Points.FirstOrDefault().Y + move.Y)));
        }

        public PointF GetPointF(Vector2 vector2d)
        {
            Vector2 vector2 = matrix3.MultiplyPoint(new Vector2(vector2d.X / 100, vector2d.Y / 100));
            PointF pointF = new PointF();
            pointF.X = (float)vector2.X + 50;
            pointF.Y = (float)vector2.Y + 400;
            return pointF;
        }

        private void btSetMinGrid_Click(object sender, System.EventArgs e)
        {

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
        public PlateRoom plateRoom1 = null;
        public PlateRoom plateRoom2 = null;
        private PlateRoom GetPlateRoomByPoint(Vector2 point, PlateBuildGroup plateHouse, int index)
        {
            List<Line2d> ls = new List<Line2d>();
            Vector2 movedir = new Vector2(0, 0);
            if (plateHouse.GroupType == PlateBuildGroupType.一)
            {
                movedir = new Vector2(0, (plateHouse.RoomSizeLength + plateHouse.PassagewaySizeWidth));
            }
            Vector2 p = point - movedir;
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
                            if (plateRoom2 != item_)
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.Red);
                                plateRoom1 = item_;
                            }
                            else
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.White);
                                plateRoom2 = null;
                            }
                        }
                        else if (plateRoom2 == null)
                        {
                            if (plateRoom1 != item_)
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.Green);
                                plateRoom2 = item_;
                            }
                            else
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.White);
                                plateRoom1 = null;
                            }
                        }
                        else
                        {
                            if (plateRoom1 == item_)
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.White);
                                plateRoom1 = null;
                            }
                            else
                                if (plateRoom2 == item_)
                            {
                                DrawSelectPolygon(Polygon, movedir, System.Drawing.Color.White);
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
            Vector2 mouseLocation = new Vector2(0, 0);
            Point p = ((System.Windows.Forms.MouseEventArgs)e).Location;
            p = new Point(p.X - 50, p.Y - 400);
            Vector2 vector2d = new Vector2(p.X, p.Y);
            vector2d = matrix3.Invert().MultiplyPoint(vector2d);
            mouseLocation = new Vector2(vector2d.X * 100, vector2d.Y * 100);

            string lname = "";
            if (listBox1.SelectedItem != null)
                lname = listBox1.SelectedItem.ToString();
            else
                lname = listBox1.Items[0].ToString();
            PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();


            GetPlateRoomByPoint(mouseLocation, plateBuildGroup, comboBox1.SelectedIndex);
            //if (plateHouse.GroupType == PlateBuildingGroupType.一)
            //{
            //    movedir = new Vector2(0, (plateHouse.RoomSizeLength + plateHouse.PassagewaySizeWidth));
            //}
            //Vector2 vector2 = matrix3.MultiplyPoint(new Vector2(vector2d.X / 100, vector2d.Y / 100));
            //PointF pointF = new PointF();
            //pointF.X = (float)vector2.X + 50;
            //pointF.Y = (float)vector2.Y + 400;

        }

        private void button8_Click(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            InputerBox inputerBox = new InputerBox();
            inputerBox.ShowDialog();
            var PlateBuildingGroupDef = docrt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildGroupDef;
            PlateBuildGroup plateBuildGroup = new PlateBuildGroup(PlateBuildingGroupDef, InputerBox.inputerRestul, Convert.ToInt32(this.textBox4.Text), PlateBuildGroupType.一, BuildingType.Work);
            BuildGroups.Add(plateBuildGroup);
            listBox1.Items.Add(InputerBox.inputerRestul);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            plateRooms.Clear();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                for (int i = 0; i < Convert.ToInt32(item.Cells[1].Value); i++)
                {
                    PlateRoom plateRoom = new PlateRoom(item.Cells[0].Value.ToString(), BuildingType.Work, Convert.ToInt32(item.Cells[2].Value));
                    plateRooms.Add(plateRoom);
                }
            }
        }

        private void label9_Click(object sender, System.EventArgs e)
        {

        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            string lname = "";
            if (listBox1.SelectedItem != null)
                lname = listBox1.SelectedItem.ToString();
            else
                lname = listBox1.Items[0].ToString();
            PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();
            if (plateBuildGroup == null) return;
            plateBuildGroup.Elements.Clear();
            switch (buildstyle.SelectedIndex)
            {
                case 0:
                    plateBuildGroup.GroupType = PlateBuildGroupType.一;
                    break;
                case 1:
                    plateBuildGroup.GroupType = PlateBuildGroupType.L;
                    break;
                default:
                    plateBuildGroup.GroupType = PlateBuildGroupType.U;
                    break;
            }
            double kcount = plateBuildGroup.RoomCount;
            List<PlateRoom> kplateRooms = new List<PlateRoom>();
            foreach (var item in plateRooms)
            {
                kplateRooms.Add((PlateRoom)item);
                //if (kcount >= item.CellNumber)
                //{
                //    kplateRooms.Add((PlateRoom)item);
                //    kcount -= item.CellNumber;
                //}
            }
            plateBuildGroup.PlateBuildGroupInit2(this.docrt, Convert.ToDouble(this.rommwith.Text), Convert.ToDouble(this.roomlength.Text)
                , Convert.ToDouble(this.passwaywidth.Text), Convert.ToDouble(this.stairwidth.Text), Convert.ToDouble(this.statirlength.Text)
                , this.comboBox1.Items.Count, kplateRooms);
            PlateBuildingGroupDraw(plateBuildGroup, comboBox1.SelectedIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string lname = "";
            if (listBox1.SelectedItem != null)
                lname = listBox1.SelectedItem.ToString();
            else
                lname = listBox1.Items[0].ToString();
            PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();
            PlateBuildingGroupDraw(plateBuildGroup, comboBox1.SelectedIndex);
        }

        private void button12_Click(object sender, System.EventArgs e)
        {
            comboBox1.Items.Add(comboBox1.Items.Count + 1);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var item = listBox1.SelectedItem.ToString();
                PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == item).FirstOrDefault();
                if (plateBuildGroup != null)
                {
                    BuildGroups.Remove(plateBuildGroup);
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }
                else
                    listBox1.Items.Remove(listBox1.SelectedItem);
            }

        }
        public void ResetPlateBuildingGroup(PlateBuildGroup plateHouse, int index, int type)
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
                kplateRooms.Remove(plateRoom1);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2), plateRoom1);
            }
            if (type == 2)
            {

                kplateRooms.Remove(plateRoom1);
                kplateRooms.Insert(kplateRooms.IndexOf(plateRoom2) + 1, plateRoom1);
            }
            if (type == 3)
            {

                int p1 = kplateRooms.IndexOf(plateRoom1);
                int p2 = kplateRooms.IndexOf(plateRoom2);
                kplateRooms.Remove(plateRoom1);
                kplateRooms.Insert(p1, plateRoom2);
                kplateRooms.RemoveAt(p2);
                kplateRooms.Insert(p2, plateRoom1);

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

        private void 交换房间ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (plateRoom1 != null && plateRoom2 != null)
            {
                if (plateRoom1.CellNumber != plateRoom2.CellNumber)
                {
                    string lname = "";
                    if (listBox1.SelectedItem != null)
                        lname = listBox1.SelectedItem.ToString();
                    else
                        lname = listBox1.Items[0].ToString();
                    PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();

                    ResetPlateBuildingGroup(plateBuildGroup, comboBox1.SelectedIndex, 3);
                    PlateBuildingGroupDraw(plateBuildGroup, comboBox1.SelectedIndex);
                }
            }
        }

        private void 插入房间前ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (plateRoom1 != null && plateRoom2 != null)
            {

                string lname = "";
                if (listBox1.SelectedItem != null)
                    lname = listBox1.SelectedItem.ToString();
                else
                    lname = listBox1.Items[0].ToString();
                PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();

                ResetPlateBuildingGroup(plateBuildGroup, comboBox1.SelectedIndex, 1);
                PlateBuildingGroupDraw(plateBuildGroup, comboBox1.SelectedIndex);

            }
        }

        private void 插入房间后ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (plateRoom1 != null && plateRoom2 != null)
            {

                string lname = "";
                if (listBox1.SelectedItem != null)
                    lname = listBox1.SelectedItem.ToString();
                else
                    lname = listBox1.Items[0].ToString();
                PlateBuildGroup plateBuildGroup = BuildGroups.Where(x => x.BuildGroupName == lname).FirstOrDefault();

                ResetPlateBuildingGroup(plateBuildGroup, comboBox1.SelectedIndex, 2);
                PlateBuildingGroupDraw(plateBuildGroup, comboBox1.SelectedIndex);

            }
        }
    }
}
