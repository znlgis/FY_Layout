using Newtonsoft.Json.Linq;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace QdLayout
{
    public class PlateBuildingAction : ComponentInstance2dAction
    {
        public PlateBuildingAction() { }


        public PlateBuildingAction(IDocumentEditor docEditor) : base(docEditor)
        {
            commandCtrl.WriteInfo("命令：PlateHouse");
        }
        public PlateBuilding InitHouse()
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
            //   var PlateHouseDef = docRt.GetUseComDef($"{NamespaceKey}.建构筑物", "板房", null) as PlateHouseDef;
            //PlateBuilding plateHouse = new PlateBuilding();
            //plateHouse.Initilize(docRt.Document);


            Polygon2d PassagewayOutline = new Polygon2d();
            Vector2[] vector2s = new Vector2[]{
                new Vector2(0, 0), new Vector2(9000, 0),new Vector2(9000, 1000),new Vector2(0, 1000)
            };
            PassagewayOutline.Points = vector2s;
            PlatePassageway passagewayCell = new PlatePassageway(PassagewayOutline);

            // plateHouse.Passageways = new List<PlatePassageway> { passagewayCell };


            Polygon2d StairsCellOutline = new Polygon2d();
            Vector2[] Stairsvector2s = new Vector2[]{
                new Vector2(9000, 0), new Vector2(10000, 0),new Vector2(10000, 4000),new Vector2(9000, 4000)
            };
            StairsCellOutline.Points = Stairsvector2s;
            //     PlateStair stairs = new PlateStair(StairsCellOutline);
            //   plateHouse.Stairs = new List<PlateStair>() { stairs };



            Polygon2d PlateCellOutline = new Polygon2d();
            Vector2[] plateCellvector2s = new Vector2[]{
                new Vector2(0, 1000), new Vector2(3000, 1000),new Vector2(3000, 4000),new Vector2(0, 4000)
            };
            PlateCellOutline.Points = plateCellvector2s;
            //   PlateRoom plateCell = new PlateRoom(PlateCellOutline);
            //    plateHouse.Rooms.Add(plateCell);

            var start = 3000;
            for (int i = 0; i < 4; i++)
            {

                Polygon2d PlateCellOutline22 = new Polygon2d();
                Vector2[] plateCellvector22s = new Vector2[]{
                new Vector2(start, 1000), new Vector2(start+1500, 1000),new Vector2(start+1500, 4000),new Vector2(start, 4000)
                };
                PlateCellOutline22.Points = plateCellvector22s;
                //    PlateRoom plateCel2 = new PlateRoom(PlateCellOutline22);
                ///  plateHouse.Rooms.Add(plateCel2);
                start += 1500;
            }
            // return plateHouse;
            return null;
        }
        //static PlateHouseAction()
        //{
        //    CreateMethods = new LcCreateMethod[1];
        //    CreateMethods[0] = new LcCreateMethod()
        //    {
        //        Name = "CreateLawn",
        //        Description = "创建围栏",
        //        Steps = new LcCreateStep[]
        //        {
        //            new LcCreateStep { Name = "Step0", Options = "指定围栏第一个点:" },
        //            new LcCreateStep { Name = "Step1", Options = "下一点[圆弧(A)/闭合(C)/放弃(U)]:" },
        //            new LcCreateStep { Name = "Step2", Options = "下一点[圆弧(A)/闭合(C)/放弃(U)]:" },
        //        }
        //    };
        //}
        public class buildList()
        {
            public string name;
            public Vector2 localtion;
        }
        public async void execes(string[] args = null)
        {
            string json = "   [ { \"BlockType\":\"STL\", \"Id\":1, \"HorizontalObjId\":0, \"VerticlObjId\":-1, \"HorizontalAlignment\":\"Left\", \"VerticlAlignment\":\"Bottom\", \"Left\":1000, \"Right\":-1, \"Top\":-1, \"Bottom\":-1, \"Width\":-1, \"Height\":-1, \"Rotate\":0, \"LeftArrayCount\":-1, \"LeftArraySpace\":-1, \"TopArrayCount\":-1, \"TopArraySpace\":-1 }, { \"BlockType\":\"XFTD\", \"Id\":2, \"HorizontalObjId\":1, \"VerticlObjId\":\"-1\", \"HorizontalAlignment\":\"Left\", \"VerticlAlignment\":\"None\", \"Left\":300, \"Right\":-1, \"Top\":-1, \"Bottom\":-1, \"Width\":4000, \"Height\":-1, \"Rotate\":0, \"LeftArrayCount\":-1, \"LeftArraySpace\":-1, \"TopArrayCount\":-1, \"TopArraySpace\":-1 }, { \"BlockType\":\"BGL\", \"Id\":3, \"HorizontalObjId\":2, \"VerticlObjId\":-1, \"HorizontalAlignment\":\"Left\", \"VerticlAlignment\":\"Bottom\", \"Left\":300, \"Right\":-1, \"Top\":-1, \"Bottom\":8000, \"Width\":-1, \"Height\":-1, \"Rotate\":0, \"LeftArrayCount\":-1, \"LeftArraySpace\":-1, \"TopArrayCount\":-1, \"TopArraySpace\":-1 }, { \"BlockType\":\"DM\", \"Id\":4, \"HorizontalObjId\":3, \"VerticlObjId\":2, \"HorizontalAlignment\":\"CenterAligned\", \"VerticlAlignment\":\"Top\", \"Left\":-1, \"Right\":-1, \"Top\":8000, \"Bottom\":-1, \"Width\":-1, \"Height\":-1, \"Rotate\":0, \"LeftArrayCount\":-1, \"LeftArraySpace\":-1, \"TopArrayCount\":-1, \"TopArraySpace\":-1 },  { \"BlockType\":\"SSL\", \"Id\":5, \"HorizontalObjId\":2, \"VerticlObjId\":4, \"HorizontalAlignment\":\"LeftAligned\", \"VerticlAlignment\":\"Bottom\", \"Left\":-1, \"Right\":0, \"Top\":-1, \"Bottom\":3500, \"Width\":-1, \"Height\":-1, \"Rotate\":0, \"LeftArrayCount\":-1, \"LeftArraySpace\":-1, \"TopArrayCount\":-1, \"TopArraySpace\":-1 } ]";
            JArray js = JArray.Parse(json);


            var elements = this.docRt.Action.SelectedElements.FindAll((ele) => ele.Type == LayoutElementType.PropertyLine);

            Double MaxX=0;
            Double MinX=0;
            Double MaxY=0;
            Double MinY=0;
            if (elements.Count > 1)
            {
                this.docRt.Action.ClearSelects();
            }
          
            else if (elements.Count == 1)
            {
                QdPropertyLine qdPropertyLine=  elements.FirstOrDefault() as QdPropertyLine;
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
                    if(MinX> line2D1.End.X)
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

                Double mx = MinX + 300;
                Double my = MinY + 3500;
                List<string> ls = new List<string> { "STL", "BGL", "SSL" };
                List<buildList> builds = new List<buildList>();
                foreach (var item in ls)
                {
                    buildList buildList = new buildList();
                    buildList.name = item;
                    buildList.localtion = new Vector2(mx, my);
                    builds.Add(buildList);
                    my += 3500;
                }

            }
        
        }
       
        public async void ExecCreate(string[] args = null)
        {

            var elements = this.docRt.Action.SelectedElements.FindAll((ele) => ele.Type == LayoutElementType.PlateBuildGroup);


            if (elements.Count > 1)
            {
                this.docRt.Action.ClearSelects();
            }
            else if (elements.Count == 0)
            {

            }
            else
            {
                PlateBuildGroup plateBuildGroup = elements[0] as PlateBuildGroup;
                plateBuildGroup.OnPropertyChangedBefore("", null, null);
                List<PlateBuildGroup> plateBuildGroups = new List<PlateBuildGroup>();
                plateBuildGroups.Add(plateBuildGroup);
                PlateGroupSet plateSet = new PlateGroupSet(this.docRt, plateBuildGroups);
                plateSet.ShowDialog();
                plateBuildGroup.DirtyType = DirtyType.Change;
            }
            // PlateBuilding plateHouse = InitHouse();

        }
    }
}
