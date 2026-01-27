using LightCAD.MathLib;
using LightCAD.Runtime;
using netDxf.Entities;
using Newtonsoft.Json.Linq;
using OpenTK.Graphics.OpenGL;
using QdLayout.Fence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using System.Security.Cryptography;

using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net;
using static netDxf.Entities.HatchBoundaryPath;
using static QdLayout.ArrangementWindow;
 
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public class PlateBuildGroupAction : ElementAction
    {
        public PlateBuildGroupAction() { }
        public PlateBuildGroupAction(IDocumentEditor docEditor) : base(docEditor)
        {

            commandCtrl.WriteInfo("命令：PlateHouse");
        }
        public override void Draw(LcCanvas2d canvas, LcElement element, Matrix3 matrix)
        {
            var pen = this.GetDrawPen(element);
            if (element.IsSelected)
                pen = Constants.SelectedPen;
            var group = element as PlateBuildGroup;
            //Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), group.Location);
            //matrix3.Multiply(Matrix3.Rotate(group.BuildRotate, new Vector2(0, 0)));

            //Matrix3 matrix2 = Matrix3.Rotate(group.BuildRotate, new Vector2(0, 0));
            //Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), group.Location);
            //Matrix3 matrix3 = matrix2.Premultiply(matrix4);

            //  matrix3.Multiply(Matrix3.Rotate(group.BuildRotate,group.Location));
            // Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), group.Location);
            if (group.Elements.Count == 0)
            {

                canvas.DrawCurve(pen, group.PlateBuildGroupPolygon, matrix);
            }
            else
            {
                Matrix3 matrix3 = Matrix3.Rotate(group.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), group.Location);
                matrix3.Premultiply(matrix4);
                List<Line2d> ls = new List<Line2d>();
                foreach (var ele in group.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[PlateGroupSet.floorindex];
                    foreach (var item in plateBuildingFloor.Rooms)
                    {
                        DrawPolygon(item.GetRoomPolygon2d(), canvas, pen, matrix3);
                        DrawText(item.Name, canvas, item.GetCenter(), matrix3, new Vector2(item.Direct.Y, -item.Direct.X), pen, group.BuildRotate);
                        if (!string.IsNullOrEmpty(item.RoomConfig))
                        {
                            var roomConfig = PlateRoomConfigManager.GetRoomConfig(group.GroupType, item.Name, item.RoomConfig);
                            DrawRoomConfig(canvas, item, roomConfig, matrix3, pen);
                        }
                    }
                    //var eleAction = (ele.RtAction as ElementAction);
                    //eleAction.Draw(canvas, ele, matrix3);
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
                        DrawPolygon(item_.StairsCellOutline, canvas, pen, matrix3);
                        DrawText("楼梯", canvas, item_.GetCenter(), matrix3, new Vector2(plateBuilding.BuildingDirect.Y, -plateBuilding.BuildingDirect.X), pen, group.BuildRotate);
                    }

                }
                foreach (var item in ls)
                {
                    canvas.DrawLine(pen, item.Start, item.End, matrix3);
                }

            }

        }
        public void DrawPolygon(Polygon2d polygon2d, LcCanvas2d canvas, LcPaint pen, Matrix3 matrix)
        {
            Vector2 vector2d = null;
            foreach (var item in polygon2d.Points)
            {
                if (vector2d != null)
                {
                    canvas.DrawLine(pen, vector2d, item, matrix);
                }
                vector2d = item;
            }
            canvas.DrawLine(pen, polygon2d.Points[0], polygon2d.Points[polygon2d.Points.Length - 1], matrix);
        }
        public void DrawText(string context, LcCanvas2d canvas, Vector2 center, Matrix3 matrix, Vector2 dir, LcPaint pen,double rotate)
        {
            var txtStyleName = "Standard"; // this.docRt.Document.ActiveTextStyle.StyleName;
            var txtStyle = this.docRt.Document.TextStyles.GetTextStyleByName(txtStyleName);
            var fontSize = 600;
            var ro = dir.Angle() * (180 / Math.PI);
            var dircit = dir.Clone();
            if (ro >= 180 & ro < 360)
            {
                ro -= 180;
                dircit = new Vector2(-dircit.X, -dircit.Y);
            }
            //ro -= rotate;  
            var height = new Vector2(dircit.Y, -dircit.X);

            var location = center.Clone() - dircit * (context.Length / 2) * fontSize + height * (fontSize / 2);
            var paint = new LcTextPaint()
            {
                Color = pen.Color,
                Size = fontSize,
                WordSpace = fontSize / 10,
                Position = location,
                Rotation = ro,
                Oblique = 1,
                WidthFactor = 1,
                Backward = txtStyle.Backward,
                UpsideDown = txtStyle.UpsideDown
            };
            if (txtStyle.IsFontMiss && txtStyle.IsUseReplaceFont)
            {
                paint.FontName = txtStyle.ReplaceFontFileName;
            }
            else
            {
                paint.FontName = txtStyle.FontFileName;
            }
            if (txtStyle.IsBigFontMiss && txtStyle.IsUseReplaceBigFont)
            {
                paint.FontName2 = txtStyle.ReplaceBigFontFileName;
            }
            else
            {
                paint.FontName2 = txtStyle.BigFontFileName;
            }
            canvas.DrawText2(paint, context, matrix );
        }

        public void DrawRoomConfig(LcCanvas2d canvas, PlateRoom room, RoomConfig roomConfig, Matrix3 matrix, LcPaint pen)
        {
            var configFilePath = roomConfig.ConfigFilePath;
            var doc = LoadManager.Load(configFilePath);

            var roomDir = room.Direct.Clone().RotateAround(new Vector2(), -Math.PI / 2);
            var roomMatrix = matrix.Clone()
                            .Multiply(new Matrix3().MakeTranslation(room.Localtion.X, room.Localtion.Y))
                            .Multiply(new Matrix3().MakeRotationAround(new Vector2(), roomDir.Angle()));

            void drawBlockRef(LcBlockRef blockRef, Matrix3 matrix)
            {
                var block = blockRef.Container as LcBlock;
                var offset = block.BasePoint.Clone().Negate();
                var transMatrix = new Matrix3().MakeTranslation(offset.X, offset.Y);

                var newMatrix = matrix.Clone().Multiply(transMatrix).Multiply(blockRef.Matrix);

                foreach (var ele in block.Elements)
                {
                    if (ele is LcCurve2d curve)
                    {
                        canvas.DrawCurve(pen, curve.Curve, newMatrix);
                    }
                    else if (ele is LcBlockRef subBlockRef)
                    {
                        drawBlockRef(subBlockRef, newMatrix);
                    }
                }
            }
            foreach (var ele in doc.ModelSpace.Elements)
            {
                if (ele is LcCurve2d curve)
                {
                    canvas.DrawCurve(pen, curve.Curve, roomMatrix);
                }
                else if (ele is LcBlockRef blockRef)
                {
                    drawBlockRef(blockRef, roomMatrix);
                }
            }

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
        public override void Draw(LcCanvas2d canvas, LcElement element, Vector2 offset)
        {
            var grp = element as PlateBuildGroup;
            // Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), grp.Location);
            // matrix3.Multiply(Matrix3.Rotate(grp.BuildRotate, grp.Location));
    
      


            var pen = this.GetDrawPen(element);
            if (element.IsSelected)
                pen = Constants.SelectedPen;
            if (grp.Elements.Count == 0)
            {

                canvas.DrawCurve(pen, grp.PlateBuildGroupPolygon, offset);

            }
            else
            {
                Matrix3 matrix3 = Matrix3.Rotate(grp.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), grp.Location);
                matrix3.Premultiply(matrix4);
                List<Line2d> ls = new List<Line2d>();
                foreach (var ele in grp.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[PlateGroupSet.floorindex];
                    foreach (var item in plateBuildingFloor.Rooms)
                    {
                        DrawPolygon(item.GetRoomPolygon2d(), canvas, pen, matrix3);

                        DrawText(item.Name, canvas, item.GetCenter(), matrix3, new Vector2(item.Direct.Y, -item.Direct.X), pen, grp.BuildRotate);

                        if (!string.IsNullOrEmpty(item.RoomConfig))
                        {
                            var roomConfig = PlateRoomConfigManager.GetRoomConfig(grp.GroupType, item.Name, item.RoomConfig);
                            DrawRoomConfig(canvas, item, roomConfig, matrix3, pen);
                        }

                    }
                    //var eleAction = (ele.RtAction as ElementAction);
                    //eleAction.Draw(canvas, ele, matrix3);
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
                        DrawPolygon(item_.StairsCellOutline, canvas, pen, matrix3);
                        DrawText("楼梯", canvas, item_.GetCenter(), matrix3, new Vector2(plateBuilding.BuildingDirect.Y, -plateBuilding.BuildingDirect.X), pen, grp.BuildRotate);
                        //   DrawText("楼梯", canvas, item_.GetCenter(), matrix3);


                    }

                }
                foreach (var item in ls)
                {
                    canvas.DrawLine(pen, item.Start, item.End, matrix3);
                }
            }


        }
        public LcComponentInstance GetPlateBuildGate(Vector2 pos)
        {
            var gateDefId = "1d53efc8-7e07-4fa6-9efe-38d838adcff8";
            var gateDef = ComponentDefinitionManager.Get(gateDefId);
            if (gateDef == null)
                return null;

            var comIns = gateDef.CreateInstance() as LcComponentInstance;
            comIns.Transform3d.Position = pos.ToVector3();

            return comIns;
        }
        public void GetLists(int index, Dictionary<string, PlateBuildGroup> builds)
        {

            JArray js = JArray.Parse(PlateBuildGroup.PBGZ1[index]);


            var elements = this.docRt.Action.SelectedElements.FindAll((ele) => ele.Type == LayoutElementType.PropertyLine);

            Double MaxX = 0;
            Double MinX = 0;
            Double MaxY = 0;
            Double MinY = 0;
            if (elements.Count > 1)
            {
                this.docRt.Action.ClearSelects();
            }

            else if (elements.Count == 1)
            {
                QdPropertyLine qdPropertyLine = elements.FirstOrDefault() as QdPropertyLine;
                Line2d line2D = qdPropertyLine.Curve2ds.FirstOrDefault() as Line2d;
                MaxX = line2D.Start.X;
                MinX = line2D.End.X;
                MaxY = line2D.Start.Y;
                MinY = line2D.End.Y;
                foreach (var item in qdPropertyLine.Curve2ds)
                {
                    Line2d line2D1 = item as Line2d;
                    if (MaxX < line2D1.Start.X)
                    {
                        MaxX = line2D1.Start.X;
                    }
                    if (MaxX < line2D1.End.X)
                    {
                        MaxX = line2D1.End.X;
                    }
                    if (MinX > line2D1.Start.X)
                    {
                        MinX = line2D1.Start.X;
                    }
                    if (MinX > line2D1.End.X)
                    {
                        MinX = line2D1.End.X;
                    }
                    if (MaxY < line2D1.Start.Y)
                    {
                        MaxY = line2D1.Start.Y;
                    }
                    if (MaxY < line2D1.End.Y)
                    {
                        MaxY = line2D1.End.Y;
                    }
                    if (MinY > line2D1.Start.Y)
                    {
                        MinY = line2D1.Start.Y;
                    }
                    if (MinY > line2D1.End.Y)
                    {
                        MinY = line2D1.End.Y;
                    }
                }
                List<BuidInfo> buildInfos = new List<BuidInfo>();
                BuidInfo b0 = new BuidInfo();
                b0.Id = "0";
                b0.MaxX = MaxX;
                b0.MinX = MinX;
                b0.MaxY = MaxY;
                b0.MinY = MinY;
                buildInfos.Add(b0);
                int i = 1;
                foreach (var item in js)
                {

                    string name = ((Newtonsoft.Json.Linq.JValue)item["BlockType"]).Value.ToString();
                    string HorizontalObjId = ((Newtonsoft.Json.Linq.JValue)item["HorizontalObjId"]).Value.ToString();
                    string VerticlObjId = ((Newtonsoft.Json.Linq.JValue)item["VerticlObjId"]).Value.ToString();
                    string HorizontalAlignment = ((Newtonsoft.Json.Linq.JValue)item["HorizontalAlignment"]).Value.ToString();
                    string VerticlAlignment = ((Newtonsoft.Json.Linq.JValue)item["VerticlAlignment"]).Value.ToString();
                    string Left = ((Newtonsoft.Json.Linq.JValue)item["Left"]).Value.ToString();
                    string Right = ((Newtonsoft.Json.Linq.JValue)item["Right"]).Value.ToString();
                    string Top = ((Newtonsoft.Json.Linq.JValue)item["Top"]).Value.ToString();
                    string Bottom = ((Newtonsoft.Json.Linq.JValue)item["Bottom"]).Value.ToString();
                    string Width = ((Newtonsoft.Json.Linq.JValue)item["Width"]).Value.ToString();
                    string Height = ((Newtonsoft.Json.Linq.JValue)item["Height"]).Value.ToString();
                    BuidInfo b1 = new BuidInfo();
                    b1.Id = i.ToString();
                    i++;

                    var build = builds.Where(x => x.Key == name)?.FirstOrDefault();
                    if (build.Value.Value != null)
                    {
                        b1.group = build.Value.Value;
                        b1.group.BuildRotate = Convert.ToDouble(((Newtonsoft.Json.Linq.JValue)item["Rotate"]).Value.ToString());
                        if (HorizontalObjId != "-1")
                        {
                            //HorizontalAlignment: None / LeftAligned / CenterAligned / RightAligned / Fill / Left / Right
                            //VerticlAlignment: None / LeftAligned / CenterAligned / RightAligned / Fill / Left / Right
                            var Hb = buildInfos.Where(x => x.Id == HorizontalObjId).FirstOrDefault();

                            if (HorizontalAlignment != "None")
                            {
                                switch (HorizontalAlignment)
                                {
                                    case "LeftAligned": b1.MinX = Hb.MinX; b1.MaxX = b1.MinX + Math.Abs(b1.group.BoundingBox.Width); break;
                                    case "CenterAligned": b1.MinX = b1.group.BoundingBox.Min.X + ((Hb.MinX + Hb.MaxX) - b1.group.BoundingBox.Center.X); b1.MaxX = b1.MinX + Math.Abs(b1.group.BoundingBox.Width); break;
                                    case "RightAligned": b1.MaxX = Hb.MaxX; b1.MinX = b1.MaxX - Math.Abs(b1.group.BoundingBox.Width); break;
                                    case "Fill": break;
                                    case "Left": b1.MinX = (HorizontalObjId == "0" ? Hb.MinX : Hb.MaxX) + Convert.ToInt32(Left); b1.MaxX = b1.MinX + Math.Abs(b1.group.BoundingBox.Width); break;
                                    case "Right": b1.MaxX = Hb.MaxX - Convert.ToInt32(Right); b1.MinX = b1.MaxX - Math.Abs(b1.group.BoundingBox.Width); break;
                                }

                            }


                        }
                        else
                        {
                            var vb = buildInfos.Where(x => x.Id == "0").FirstOrDefault();
                            b1.MaxX = vb.MinX + Math.Abs(b1.group.BoundingBox.Width);
                            b1.MinX = vb.MinX;
                        }
                        if (VerticlObjId != "-1")
                        {
                            var vb = buildInfos.Where(x => x.Id == VerticlObjId).FirstOrDefault();

                            if (VerticlAlignment != "None")
                            {
                                if (b1.group.GroupType != PlateBuildGroupType.U)
                                {
                                    switch (VerticlAlignment)
                                    {

                                        case "Top": b1.MaxY = (VerticlObjId == "0" ? vb.MaxY : vb.MinY) - Convert.ToInt32(Top); b1.MinY = b1.MaxY - Math.Abs(b1.group.BoundingBox.Height); break;
                                        case "Bottom": b1.MinY = (VerticlObjId == "0" ? vb.MinY : vb.MaxY) + Convert.ToInt32(Bottom); b1.MaxY = b1.MinY + Math.Abs(b1.group.BoundingBox.Height); break;

                                    }

                                }
                                else
                                {
                                    if (VerticlAlignment == "Top")
                                    {
                                        b1.MaxY = (VerticlObjId == "0" ? vb.MaxY : vb.MinY) - Convert.ToInt32(Top);
                                        b1.MinY = b1.MaxY - Math.Abs(b1.group.BoundingBox.Height);
                                    }
                                    else if (VerticlAlignment == "Bottom")
                                    {
 
                                      
                                        if ((Math.Abs(b1.group.BoundingBox.Height)- b1.group.RoomSizeLength- b1.group.PassagewaySizeWidth) > Convert.ToInt32(Bottom))
                                        {
                                            b1.MinY = (VerticlObjId == "0" ? vb.MinY : vb.MaxY) + 1300;
                                            b1.MaxY = b1.MinY + Math.Abs(b1.group.BoundingBox.Height);
                              
                                        }
                                        else
                                        {
                                            int bh = Convert.ToInt32(Bottom)-(int)(Math.Abs(b1.group.BoundingBox.Height) - b1.group.RoomSizeLength - b1.group.PassagewaySizeWidth)  ;
                                            b1.MinY = (VerticlObjId == "0" ? vb.MinY : vb.MaxY) + bh;
                                            b1.MaxY = b1.MinY + Math.Abs(b1.group.BoundingBox.Height);
 
                                        }
                                    }
                                }

                            }



                        }
                        else
                        {
                            var vb = buildInfos.Where(x => x.Id == "0").FirstOrDefault();
                            b1.MaxY = vb.MinY + Math.Abs(b1.group.BoundingBox.Height);
                            b1.MinY = vb.MinY;

                        }
                        Vector2 min= b1.group.GetBoundingBox().Min;
                        b1.group.Location = new Vector2(b1.MinX-min.X, b1.MinY - min.Y);
                        b1.group.ResetBoundingBox();
           
                        b1.group.DirtyType = DirtyType.Change;
                    }
                    else
                    {
                        if (HorizontalObjId != "-1")
                        {
                            //HorizontalAlignment: None / LeftAligned / CenterAligned / RightAligned / Fill / Left / Right
                            //VerticlAlignment: None / LeftAligned / CenterAligned / RightAligned / Fill / Left / Right
                            var Hb = buildInfos.Where(x => x.Id == HorizontalObjId).FirstOrDefault();
                            if (HorizontalAlignment != "None")
                            {
                                switch (HorizontalAlignment)
                                {
                                    case "LeftAligned": b1.MinX = Hb.MinX; b1.MaxX = b1.MinX + Convert.ToInt32(Width); break;
                                    case "CenterAligned": b1.MinX = ((Hb.MinX + Hb.MaxX) / 2 - Convert.ToInt32(Width) / 2); b1.CenterX = (Hb.MinX + Hb.MaxX) / 2; b1.MaxX = b1.MinX + Convert.ToInt32(Width); break;
                                    case "RightAligned": b1.MaxX = Hb.MaxX; b1.MinX = b1.MaxX - Convert.ToInt32(Width); break;
                                    case "Fill": break;
                                    case "Left": b1.MinX = (HorizontalObjId == "0" ? Hb.MinX : Hb.MaxX) + Convert.ToInt32(Left); b1.MaxX = b1.MinX + Convert.ToInt32(Width); break;
                                    case "Right": b1.MaxX = Hb.MaxX - Convert.ToInt32(Right); b1.MinX = b1.MaxX - Convert.ToInt32(Width); break;
                                }

                            }

                        }
                        else
                        {
                            var vb = buildInfos.Where(x => x.Id == "0").FirstOrDefault();
                            b1.MaxX = vb.MaxX;
                            b1.MinX = vb.MinX;
                        }
                        if (VerticlObjId != "-1")
                        {
                            var vb = buildInfos.Where(x => x.Id == VerticlObjId).FirstOrDefault();
                            if (VerticlAlignment != "None")
                            {
                                switch (VerticlAlignment)
                                {
                                    case "Top": b1.MaxY = (VerticlObjId == "0" ? vb.MaxY : vb.MinY) - Convert.ToInt32(Top); b1.MinY = b1.MaxY - Convert.ToInt32(Height); break;
                                    case "Bottom": b1.MinY = (VerticlObjId == "0" ? vb.MaxY : vb.MinY) + Convert.ToInt32(Bottom); b1.MaxY = b1.MinY + Convert.ToInt32(Height); break;
                                }

                            }
                        }
                        else
                        {
                            var vb = buildInfos.Where(x => x.Id == "0").FirstOrDefault();
                            b1.MaxY = vb.MaxY;
                            b1.MinY = vb.MinY;
                        }
                        if (name == "XFTD")
                        {
                            var RoadAction = new RoadAction(docEditor);
                            RoadAction.CreateRoad(new Line2d(new Vector2((b1.MinX + b1.MaxX) / 2, b1.MinY), new Vector2((b1.MinX + b1.MaxX) / 2, b1.MaxY)), Convert.ToInt32(Width));
                        }
                        if (name == "DM")
                        {
                            Vector2 po = new Vector2(b1.CenterX, b1.MinY);
                            var gate = GetPlateBuildGate(po);
                            if (gate != null)
                            {
                                this.vportRt.ActiveElementSet.InsertElement(gate);

                                Polyline2d polyline2D = new Polyline2d();
                                polyline2D.Curve2ds = new List<Curve2d>();
                                Line2d line1 = new Line2d(new Vector2(b1.CenterX - 5000, MinY), new Vector2(MinX, MinY));
                                Line2d line2 = new Line2d(new Vector2(MinX, MinY), new Vector2(MinX, MaxY));
                                Line2d line3 = new Line2d(new Vector2(MinX, MaxY), new Vector2(MaxX, MaxY));
                                Line2d line4 = new Line2d(new Vector2(MaxX, MaxY), new Vector2(MaxX, MinY));
                                Line2d line5 = new Line2d(new Vector2(MaxX, MinY), new Vector2(b1.CenterX + 5000, MinY));
                                polyline2D.Curve2ds.Add(line1);
                                polyline2D.Curve2ds.Add(line2);
                                polyline2D.Curve2ds.Add(line3);
                                polyline2D.Curve2ds.Add(line4);
                                polyline2D.Curve2ds.Add(line5);
                                SetFence(polyline2D);
                            }
                        }
                    }
                    buildInfos.Add(b1);
                }

            }

        }
        public void SetFence(Polyline2d polyline2D)
        {
            var FenceDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "围墙", null) as QdFenceDef;
            QdFence qdFence = new QdFence(FenceDef);
            qdFence.Initilize(docRt.Document);
            qdFence.BaseCurve = polyline2D;
            qdFence.FenceWidth = Convert.ToDouble(FenceSet.FenceWidth);
            qdFence.FenceHeight = Convert.ToDouble(FenceSet.FenceHeight);
            qdFence.FenceColumnHeight = Convert.ToDouble(FenceSet.FenceColumnHeight);
            qdFence.FenceColumnInterval = Convert.ToDouble(FenceSet.FenceColumnInterval);
            qdFence.FenceColor = FenceSet.FenceColor;
            qdFence.FenceColumnColor = FenceSet.FenceColumnColor;
            qdFence.Bottom = 0;
            this.vportRt.ActiveElementSet.InsertElement(qdFence);
        }
        public class BuidInfo
        {
            public string Id;
            public Double MaxX;
            public Double MinX;
            public Double MaxY;
            public Double MinY;
            public Double CenterX;
            public PlateBuildGroup group;
        }
        public double passwaywidth = 1000;
        public double stairwidth = 1200;
        public double statirlength = 4200;
        public async void SetGroup(string[] args = null)
        {
            var elements = this.docRt.Action.SelectedElements.FindAll((ele) => ele.Type == LayoutElementType.PropertyLine);

            var index = ArrangementRuntime.ArrangeIndex;
            var buildiType = ArrangementRuntime.BuildiType;


            Dictionary<string, PlateBuildGroup> lists = new Dictionary<string, PlateBuildGroup>();
            foreach (var item in ArrangementRuntime.PlateGroups)
            {
                var PlateBuildingGroupDef = this.docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildGroupDef;
                PlateBuildGroup plateBuildGroup = new PlateBuildGroup(PlateBuildingGroupDef,"",44,PlateBuildGroupType.一,BuildingType.Work);

                plateBuildGroup.ResetGroup(item);
                //   Vector2 vector2 = buils.Where(x => x.name == item.name)?.FirstOrDefault().localtion;
                //    if (vector2 == null) continue;
                plateBuildGroup.Location = new Vector2(0, 0);
                plateBuildGroup.Initilize(this.docRt.Document);

                plateBuildGroup.PlateBuildGroupInit2(this.docRt, Convert.ToDouble(plateBuildGroup.RoomSizeWidth), Convert.ToDouble(plateBuildGroup.RoomSizeLength)
            , Convert.ToDouble(this.passwaywidth), Convert.ToDouble(this.stairwidth), Convert.ToDouble(this.statirlength)
            , item.FloorCount, item.room);

                if (item.name == "办公楼")
                {
                    lists.Add("BGL", plateBuildGroup);
                }

                if (item.name == "宿舍楼")
                {
                    lists.Add("SSL", plateBuildGroup);
                }

                if (item.name == "食堂楼")
                {
                    lists.Add("STL", plateBuildGroup);
                }
                if (item.name == "卫浴楼")
                {
                    lists.Add("WYL", plateBuildGroup);
                }
            }

            GetLists(index, lists);
            foreach (var item in lists)
            {
                this.vportRt.ActiveElementSet.InsertElement(item.Value);
            }


        }

       
        public static LcCreateMethod[] CreateMethods;
        private PointInputer inputer { get; set; }
        static PlateBuildGroupAction()
        {
            CreateMethods = new LcCreateMethod[1];
            CreateMethods[0] = new LcCreateMethod()
            {
                Name = "CreateBuildGroup",
                Description = "创建U型楼",
                Steps = new LcCreateStep[]
                {
                    new LcCreateStep { Name = "Step0", Options = "指定U型楼位置:" },
                }
            };
        }
        private Polygon2d polygon2D = new Polygon2d();

        public async void ExecCreate(string[] args = null)
        {

            List<Vector2> points = new List<Vector2>();

            double width = 3000;
            double length = 6000;
            double passwidth = 1000;


            points.Add(new Vector2(0, 0));
            points.Add(new Vector2(length + passwidth, 0));
            points.Add(new Vector2(length + passwidth, 6 * width - length - passwidth));
            points.Add(new Vector2(length + passwidth + 10 * width, 6 * width - length - passwidth));
            points.Add(new Vector2(length + passwidth + 10 * width, 0));
            points.Add(new Vector2((length + passwidth) * 2 + 10 * width, 0));
            points.Add(new Vector2((length + passwidth) * 2 + 10 * width, 6 * width));
            points.Add(new Vector2(0, 6 * width));
            polygon2D.Points = points.ToArray();
            await this.StartCreating();
            this.inputer = new PointInputer(this.docEditor);
            var curMethod = CreateMethods[0];

            Step0:
            var step0 = curMethod.Steps[0];
            var result0 = await inputer.Execute(step0.Options);

            if (inputer.isCancelled)
            {
                this.Cancel();
                goto End;
            }

            // zcb: 增加Res0为空的判定
            if (result0 == null)
            {
                this.Cancel();
                goto End;
            }

            if (result0.ValueX != null)
            {
                var doc = this.docRt.Document;
                var PlateBuildingGroupDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateBuildGroupDef;
                PlateBuildGroup plateBuildGroup = new PlateBuildGroup(PlateBuildingGroupDef, "办公楼", 44, PlateBuildGroupType.U, BuildingType.Work);
                Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), (Vector2)result0.ValueX);
                plateBuildGroup.PlateBuildGroupPolygon = polygon2D.Multiply(matrix3);
                plateBuildGroup.Location = (Vector2)result0.ValueX;
                plateBuildGroup.Initilize(doc);

                //var group = doc.CreateObject<PlateBuildGroup>();

                //group.Name = "办公楼";
                //group.RoomCount = 30;
                //group.GroupType = PlateBuildGroupType.U;
                //group.GroupBuildingType = BuildingType.Work;
                //group.PlateBuildGroupPolygon = polygon2D.Multiply(matrix3);
                this.vportRt.ActiveElementSet.InsertElement(plateBuildGroup);

                this.docRt.Action.ClearSelects();
            }
            else
                goto Step0;
            End:
            this.inputer = null;
            this.EndCreating();
        }
        public override void DrawTemp(LcCanvas2d canvas)
        {
            var mp = this.inputer?.InputP;
            var wcs_mp = mp;
            LcPaint auxPen = GetAuxDrawPen();
            Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), (Vector2)mp);
            canvas.DrawCurve(auxPen, polygon2D, matrix3);
            //      plateBuildGroup.PlateBuildGroupPolygon = polygon2D.Multiply(matrix3);
        }

        private PlateBuildGroup _PlateBuildGroup;
        private string _gripName;
        private Vector2 _position;
        public override void DrawDragGrip(LcCanvas2d canvas)
        {
            if (_gripName == null || _PlateBuildGroup == null)
            {
                return;
            }
            LcPaint auxPen = Constants.AuxOrangePaint;





            if (_PlateBuildGroup.Elements.Count == 0)
            {
                Matrix3 matrix3 = Matrix3.GetMove(_PlateBuildGroup.Location, (Vector2)_position);
                canvas.DrawCurve(auxPen, _PlateBuildGroup.PlateBuildGroupPolygon, matrix3);


            }
            else
            {
                //Matrix3 matrix3 = Matrix3.GetMove(new Vector2(0, 0), (Vector2)_position);
                //matrix3.Multiply(Matrix3.Rotate(_PlateBuildGroup.BuildRotate, _position));
                //Matrix3 matrix2 = Matrix3.Rotate(_PlateBuildGroup.BuildRotate, new Vector2(0, 0));
                //Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), _position);
                //Matrix3 matrix3 = matrix2.Premultiply(matrix4);
                Matrix3 matrix3 = Matrix3.Rotate(_PlateBuildGroup.BuildRotate, new Vector2(0, 0));
                Matrix3 matrix4 = Matrix3.GetMove(new Vector2(0, 0), _position);
                matrix3.Premultiply(matrix4);
                List<Line2d> ls = new List<Line2d>();
                foreach (var ele in _PlateBuildGroup.Elements)
                {
                    PlateBuilding plateBuilding = ele as PlateBuilding;
                    PlateBuildingFloor plateBuildingFloor = plateBuilding.Floors[PlateGroupSet.floorindex];
                    foreach (var item in plateBuildingFloor.Rooms)
                    {
                        DrawPolygon(item.GetRoomPolygon2d(), canvas, auxPen, matrix3);
                        DrawText(item.Name, canvas, item.GetCenter(), matrix3, new Vector2(item.Direct.Y, -item.Direct.X), auxPen, _PlateBuildGroup.BuildRotate);
                    }
                    //var eleAction = (ele.RtAction as ElementAction);
                    //eleAction.Draw(canvas, ele, matrix3);
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
                        DrawPolygon(item_.StairsCellOutline, canvas, auxPen, matrix3);
                        DrawText("楼梯", canvas, item_.GetCenter(), matrix3, new Vector2(plateBuilding.BuildingDirect.Y, -plateBuilding.BuildingDirect.X), auxPen, _PlateBuildGroup.BuildRotate);
                        //   DrawText("楼梯", canvas, item_.GetCenter(), matrix3);


                    }

                }
                foreach (var item in ls)
                {
                    canvas.DrawLine(auxPen, item.Start, item.End, matrix3);
                }
            }


        }
        public override ControlGrip[] GetControlGrips(LcElement element)
        {
            var plateBuildGroup = element as PlateBuildGroup;
            var grips = new List<ControlGrip>();
            var grip = new ControlGrip
            {
                Element = plateBuildGroup,
                Name = $"location",
                Position = plateBuildGroup.Location,
            };
            grips.Add(grip);
            return grips.ToArray();
        }

        public override void SetDragGrip(LcElement element, ControlGrip grip, Vector2 position, bool isEnd)
        {
            var plateBuildGroup = element as PlateBuildGroup;
            var Fence = element as QdFence;
            _PlateBuildGroup = plateBuildGroup;
            if (!isEnd)
            {
                _gripName = grip.Name;
                _position = position;
            }
            else
            {
                var oldPosition = plateBuildGroup.Location.Clone();
                Matrix3 matrix3 = Matrix3.GetMove(plateBuildGroup.Location, (Vector2)_position);
                plateBuildGroup.Location = position.Clone();
                if (plateBuildGroup.PlateBuildGroupPolygon != null)
                    plateBuildGroup.PlateBuildGroupPolygon = plateBuildGroup.PlateBuildGroupPolygon.Multiply(matrix3);
                plateBuildGroup.DirtyType = DirtyType.Change;
                plateBuildGroup.Refresh();
            }
        }
        /// <summary>
        /// 属性栏
        /// </summary>
        /// <returns></returns>
        public override List<PropertyObserver> GetPropertyObservers()
        {
            return new List<PropertyObserver>()
            {
                new PropertyObserver()
                {
                    Name = "ArrangeArea",
                    DisplayName = "排布区域",
                    Getter = (ele) => (ele as PlateBuildGroup).ArrangeArea.ToString(),
                },
                new PropertyObserver()
                {
                    Name = "DetailRoomType",
                    DisplayName = "房型",
                    Getter = (ele) => (ele as PlateBuildGroup).DetailRoomType.ToString(),
                    //Setter = (ele, value) =>
                    //{
                    //    var circle = (ele as PlateBuildGroup);
                    //}
                },
                new PropertyObserver()
                {
                    Name = "RoomWidth",
                    DisplayName = "宽度",
                    Getter = (ele) => Math.Round((ele as PlateBuildGroup).RoomSizeLength, 0),
                    //Setter = (ele, value) =>
                    //{
                    //    var circle = (ele as LcCircle);
                    //    var radius = Convert.ToDouble(value);
                    //    circle.SetProps((LcCircle.PN_Radius, radius));
                    //}
                },
                new PropertyObserver()
                {
                    Name = "RoomLength",
                    DisplayName = "长度",
                    Getter = (ele) => Math.Round((ele as PlateBuildGroup).RoomSizeWidth, 0),
                    //Setter = (ele, value) =>
                    //{
                    //    var circle = (ele as LcCircle);
                    //    var radius = Convert.ToDouble(value) / 2;
                    //    circle.SetProps((LcCircle.PN_Radius, radius));
                    //}
                },
                new PropertyObserver()
                {
                    Name = "GroupType",
                    DisplayName = "排布形状",
                    Getter = (ele) => (ele as PlateBuildGroup).GroupType.ToString() + "("+(ele as PlateBuildGroup).DeatilPlateBuildType.ToString()+  ")" ,
                    Setter = (ele, value) =>
                    {
                        //var circle = (ele as LcCircle);
                        //var radius = Convert.ToDouble(value) / 2 / Math.PI;
                        //circle.SetProps((LcCircle.PN_Radius, radius));
                    }
                },
            };
        }
    }
}
