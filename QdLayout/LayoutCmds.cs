using LightCAD.Runtime.Interface;
using ThreeJs4Net;
using System.Windows.Forms;
using System.Collections.Generic;

namespace QdLayout
{
    [CommandClass]
    public class LayoutCmds
    {
        [CommandMethod(Name = "Fence", ShortCuts = "W")]
        public CommandResult DrawWall222(IDocumentEditor docEditor, string[] args)
        {
            var fenceAction = new FenceAction(docEditor);
              fenceAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "PlateBuilding", ShortCuts = "W")]
        public CommandResult DrawWall2322(IDocumentEditor docEditor, string[] args)
        {
            var PlateHouseAction = new PlateBuildingAction(docEditor);
            PlateHouseAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "jisuang", ShortCuts = "W")]
        public CommandResult Draw1112(IDocumentEditor docEditor, string[] args)
        {
          //  var PlateHouseAction = new PlateBuildingAction(docEditor);
         //   PlateHouseAction.execes(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "SetBuildGroup", ShortCuts = "W")]
        public CommandResult SetGroup(IDocumentEditor docEditor, string[] args)
        {
            var PlateBuildGroupAction = new PlateBuildGroupAction(docEditor);
            PlateBuildGroupAction.SetGroup(args);
            return CommandResult.Succ();
        }
        
        [CommandMethod(Name = "PlateUBuild", ShortCuts = "W")]
        public CommandResult PlateUBuild(IDocumentEditor docEditor, string[] args)
        {
            var PlateBuildGroupAction = new PlateBuildGroupAction(docEditor);
            PlateBuildGroupAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Lawn", ShortCuts = "LW")]
        public CommandResult DrawLawn(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new LawnAction(docEditor);
            lawnAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "LawnRec", ShortCuts = "LWRC")]
        public CommandResult DrawLawnRec(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new LawnAction(docEditor);
            lawnAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "LawnChange", ShortCuts = "LWCH")]
        public CommandResult DrawLawnChange(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new LawnAction(docEditor);
            lawnAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Site", ShortCuts = "Site")]
        public CommandResult DrawSite(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new SiteAction(docEditor);
            lawnAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "SiteRec", ShortCuts = "STRC")]
        public CommandResult DrawSiteRec(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new SiteAction(docEditor);
            lawnAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "SiteChange", ShortCuts = "STCH")]
        public CommandResult DrawSiteChange(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new SiteAction(docEditor);
            lawnAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "PropertyLine", ShortCuts = "PropertyLine")]
        public CommandResult DrawPropertyLine(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new PropertyLineAction(docEditor);
            lawnAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "PropertyLineRec", ShortCuts = "PLRC")]
        public CommandResult DrawPropertyLineRec(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new PropertyLineAction(docEditor);
            lawnAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "PropertyLineChange", ShortCuts = "PLCH")]
        public CommandResult DrawPropertyLineChange(IDocumentEditor docEditor, string[] args)
        {
            var lawnAction = new PropertyLineAction(docEditor);
            lawnAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "PlanBuild", ShortCuts = "PB")]
        public CommandResult DrawPlanBuild(IDocumentEditor docEditor, string[] args)
        {
            PlanBuildAction planBuildAction = new PlanBuildAction(docEditor);
            planBuildAction.ExecCreatePlanBuild(args);
            return CommandResult.Succ();
        }

        [CommandMethod(Name = "PickLinePlanBuild", ShortCuts = "PLPB")]
        public CommandResult DrawPickLinePlanBuild(IDocumentEditor docEditor, string[] args)
        {
            PlanBuildAction planBuildAction = new PlanBuildAction(docEditor);
            planBuildAction.ExecPickLineCreatePlanBuild(args);
            return CommandResult.Succ();
        }
        

        [CommandMethod(Name = "FoundationPit", ShortCuts = "FDP")]
        public CommandResult DrawFoundationPit(IDocumentEditor docEditor, string[] args)
        {
            var fdpAction = new FoundationPitAction(docEditor);
            fdpAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "FoundationPitRec", ShortCuts = "FDPRE")]
        public CommandResult DrawFoundationPitRec(IDocumentEditor docEditor, string[] args)
        {
            var fdpAction = new FoundationPitAction(docEditor);
            fdpAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "FoundationPitChange", ShortCuts = "FDPCH")]
        public CommandResult DrawFoundationPitChange(IDocumentEditor docEditor, string[] args)
        {
            var fdpAction = new FoundationPitAction(docEditor);
            fdpAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Road", ShortCuts = "ROD")]
        public CommandResult DrawRoad(IDocumentEditor docEditor, string[] args)
        {
            var roadAction = new RoadAction(docEditor);
            roadAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Ground", ShortCuts = "GOD")]
        public CommandResult DrawGround(IDocumentEditor docEditor, string[] args)
        {
            var oldbAction = new GroundAction(docEditor);
            oldbAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "GroundRec", ShortCuts = "GDRC")]
        public CommandResult DrawGroundRec(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new GroundAction(docEditor);
            hardenAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "GroundChange", ShortCuts = "GODCH")]
        public CommandResult DrawGroundChange(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new GroundAction(docEditor);
            hardenAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Earthwork", ShortCuts = "EWK")]
        public CommandResult DrawEarthwork(IDocumentEditor docEditor, string[] args)
        {
            var oldbAction = new EarthworkAction(docEditor);
            oldbAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "EarthworkRec", ShortCuts = "EWRC")]
        public CommandResult DrawEarthworkRec(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new EarthworkAction(docEditor);
            hardenAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "EarthworkChange", ShortCuts = "EWCH")]
        public CommandResult DrawEarthworkChange(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new EarthworkAction(docEditor);
            hardenAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Berm", ShortCuts = "BRM")]
        public CommandResult DrawBerm(IDocumentEditor docEditor, string[] args)
        {
            var bermAction = new BermAction(docEditor);
            bermAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Barrier", ShortCuts = "BRR")]
        public CommandResult DrawBarrier(IDocumentEditor docEditor, string[] args)
        {
            var barrierAction = new BarrierAction(docEditor);
            barrierAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "Harden", ShortCuts = "HDR")]
        public CommandResult DrawHarden(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new HardenAction(docEditor);
            hardenAction.ExecCreatePoly(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "HardenRec", ShortCuts = "HDRC")]
        public CommandResult DrawHardenRec(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new HardenAction(docEditor);
            hardenAction.ExecCreateRec(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "HardenChange", ShortCuts = "HDCH")]
        public CommandResult DrawHardenChange(IDocumentEditor docEditor, string[] args)
        {
            var hardenAction = new HardenAction(docEditor);
            hardenAction.ExecCreate(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "OpenOuterLine", ShortCuts = "OPL")]
        public CommandResult OpenOuterLine(IDocumentEditor docEditor, string[] args)
        {
            var tcaRt = new OpenLineAction(docEditor);
            tcaRt.ExecCreateOpenLine(args);
            return CommandResult.Succ();
        }
        [CommandMethod(Name = "SelRedLinesForArrange", ShortCuts = "SRFA")]
        public CommandResult SelRedLinesForArrange(IDocumentEditor docEditor, string[] args)
        {
            TemComAttriAction tcaRt = new TemComAttriAction(docEditor);
            tcaRt.ExecCreate(args);
            return CommandResult.Succ();
        }

        [CommandMethod(Name = "DrawOrAdjust", ShortCuts = "DRAD")]
        public CommandResult DrawOrAdjust(IDocumentEditor docEditor, string[] args)
        {
            DrawOrAdjustAction drad = new DrawOrAdjustAction(docEditor);
            drad.ExecCreate(args);
            return CommandResult.Succ();
        }
    }
}
