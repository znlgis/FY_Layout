using LightCAD.MathLib;
using OpenTK.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Security.Policy;
using ThreeJs4Net;
using static netDxf.Entities.HatchBoundaryPath;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public class PlateBuild3dAction : ComponentInstance3dAction
    {
        public override Object3D[] RenderNode(ModelNode node)
        {
            var results = new List<Object3D>();
            var plateBuildGroup = node.Component as PlateBuildGroup;
            //幕墙厚度
            var wallthickness = 50;
            //地板厚度
            var slabthickness = 50;
            //楼层高度
            //var floorHeight = 3000;
            //走廊宽度
            var passWidth = 1000;
            //窗户高度
            var winBottom = 1000;
            //屋顶坡度
            var roofAngle = Math.PI / 12;
            var doorSize = new Vector2(900, 2000);
            var winSize = new Vector2(plateBuildGroup.RoomSizeWidth/2-200, 1000);
            //var location = (plateBuild.Parent is PlateBuildGroup group?group.Location:new Vector2());
            var location = plateBuildGroup.Location;
            //var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.CurtainUuid);
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(new MaterialInfo() { Color=new  LightCAD.MathLib.Color(0xFFFFFF), Opcity=1 });
            var matBoard = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.BoardUuid);
            var matRoad = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.RoadUuid);
            foreach (var plateBuild in plateBuildGroup.Elements)
            {
                //var offset = new Vector2().AddVectors(location, plateBuild.BuildingLocation);
                var offset = plateBuild.BuildingLocation;
                var rotationAngle = plateBuild.BuildingDirect.Angle() - new Vector2(0, 1).Angle() ;
                foreach (var floor in plateBuild.Floors)
                {
                    {
                        //var loops = new List<Line2d>();
                        //loops.Add(new Line2d(new Vector2(-wallthickness / 2, -wallthickness / 2), new Vector2(plateBuild.RoomSizeLength,- wallthickness / 2)));
                        //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(new Vector2(0, wallthickness))));
                        //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(new Vector2(wallthickness/2 - plateBuild.RoomSizeLength, 0))));
                        var fwallOffset = new Vector2(plateBuild.RoomSizeLength, 0);
                        var bwallOffset = new Vector2(wallthickness / 2, 0);
                        var pwallOffset = new Vector2(0, 0);
                        var partmatrix = new Matrix4();
                        partmatrix.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
                        //隔板墙
                        var partShapePs = new ListEx<Vector2>();
                        partShapePs.Add(new Vector2(-wallthickness / 2, 0));
                        partShapePs.Add(new Vector2(plateBuild.RoomSizeLength, 0));
                        partShapePs.Add(new Vector2(plateBuild.RoomSizeLength, floor.FloorHeight));
                        //if (floor.FloorNum>1)
                        //{
                        //    var ch = (plateBuild.RoomSizeLength / 2 + wallthickness / 4)* Math.Tan(roofAngle);
                        //    partShapePs.Add(new Vector2(plateBuild.RoomSizeLength / 2 - wallthickness / 4, floor.FloorHeight+ ch));
                        //}
                        partShapePs.Add(new Vector2(-wallthickness / 2, floor.FloorHeight));
                        var partShape = new Shape(partShapePs);
                        var pwallGeo = GeoModelUtil.GetStretchGeometryData(partShape, partmatrix, 0, wallthickness).GetBufferGeometry();
                        pwallGeo.computeVertexNormals();
                        pwallGeo.SetUV();
                        pwallGeo.translate(0, -wallthickness / 2, 0);
                        pwallGeo.rotateZ(rotationAngle);
                        pwallGeo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));
                        var pwallMesh = new Mesh(pwallGeo, mat);
                        results.Add(pwallMesh);
                        for (var i = 0; i < floor.Rooms.Count; i++)
                        {
                            var room = floor.Rooms[i];
                            var roomWidth = plateBuild.RoomSizeWidth * room.CellNumber;
                            var shape = new ListEx<Vector2>();
                            shape.Add(new Vector2(wallthickness / 2, 0));
                            shape.Add(new Vector2(roomWidth - wallthickness / 2, 0));
                            shape.Add(new Vector2(roomWidth - wallthickness / 2, floor.FloorHeight));
                            shape.Add(new Vector2(wallthickness / 2, floor.FloorHeight));
                            var coodMat = new Matrix4();
                            coodMat.MakeBasis(new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0));
                            //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(new Vector2(0, (roomWidth) - wallthickness ))));
                            //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(new Vector2(plateBuild.RoomSizeLength - wallthickness/2, 0))));
                            //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(0, wallthickness)));
                            //loops.Add(new Line2d(loops.Last().End.Clone(), loops.Last().End.Clone().Add(wallthickness/2 - plateBuild.RoomSizeLength - (i == floor.Rooms.Count - 1 ? wallthickness : 0), 0)));
                            {
                                //正面墙
                                var frontShapePs = shape.Select(n => n.Clone()).ToListEx();
                                var holes = new ListEx<ListEx<Vector2>>();
                                var doorHole = new ListEx<Vector2>();
                                var doorOffsetX = roomWidth - doorSize.X - 100;
                                doorHole.Add(new Vector2(doorOffsetX, slabthickness));
                                doorHole.Add(new Vector2(doorOffsetX + doorSize.X, slabthickness));
                                doorHole.Add(new Vector2(doorOffsetX + doorSize.X, doorSize.Y + slabthickness));
                                doorHole.Add(new Vector2(doorOffsetX, doorSize.Y + slabthickness));
                                var winHole = new ListEx<Vector2>();
                                var winOffsetX = (roomWidth - plateBuild.RoomSizeWidth) + (plateBuild.RoomSizeWidth / 2 - winSize.X) / 2;
                                winHole.Add(new Vector2(winOffsetX, winBottom));
                                winHole.Add(new Vector2(winOffsetX + winSize.X, winBottom));
                                winHole.Add(new Vector2(winOffsetX + winSize.X, winBottom + winSize.Y));
                                winHole.Add(new Vector2(winOffsetX, winBottom + winSize.Y));
                                var fshape = new Shape(frontShapePs);
                                fshape.holes.Add(new ThreeJs4Net.Path(doorHole));
                                fshape.holes.Add(new ThreeJs4Net.Path(winHole));

                                var winOffset = fwallOffset.Clone().Add(new Vector2(-wallthickness / 2, winOffsetX + winSize.X / 2)).RotateAround(new Vector2(), rotationAngle);
                                var wins = CreateWin(plateBuild, winBottom, winSize.Y, winSize.X, 50, rotationAngle, new Vector3(offset.X + winOffset.X, offset.Y + winOffset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0)));
                                results.AddRange(wins);

                                var doorOffset = fwallOffset.Clone().Add(new Vector2(0, doorOffsetX)).RotateAround(new Vector2(), rotationAngle);
                                var doors = CreateDoor(plateBuild, doorSize.Y, doorSize.X, 50, rotationAngle, new Vector3(offset.X + doorOffset.X, offset.Y + doorOffset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0) + slabthickness));
                                results.AddRange(doors);

                                if (room.CellNumber > 1)
                                {
                                    var doorHoleL = new ListEx<Vector2>();
                                    var doorLOffsetX = 100;
                                    doorHoleL.Add(new Vector2(doorLOffsetX, slabthickness));
                                    doorHoleL.Add(new Vector2(doorLOffsetX + doorSize.X, slabthickness));
                                    doorHoleL.Add(new Vector2(doorLOffsetX + doorSize.X, doorSize.Y + slabthickness));
                                    doorHoleL.Add(new Vector2(doorLOffsetX, doorSize.Y + slabthickness));
                                    var winHoleL = new ListEx<Vector2>();
                                    var winLOffsetX = plateBuild.RoomSizeWidth / 2 + (plateBuild.RoomSizeWidth / 2 - winSize.X) / 2;
                                    winHoleL.Add(new Vector2(winLOffsetX, winBottom));
                                    winHoleL.Add(new Vector2(winLOffsetX + winSize.X, winBottom));
                                    winHoleL.Add(new Vector2(winLOffsetX + winSize.X, winBottom + winSize.Y));
                                    winHoleL.Add(new Vector2(winLOffsetX, winBottom + winSize.Y));
                                    fshape.holes.Add(new ThreeJs4Net.Path(doorHoleL));
                                    fshape.holes.Add(new ThreeJs4Net.Path(winHoleL));

                                    var winOffsetL = fwallOffset.Clone().Add(new Vector2(-wallthickness / 2, winLOffsetX + winSize.X / 2)).RotateAround(new Vector2(), rotationAngle);
                                    var winsL = CreateWin(plateBuild, winBottom, winSize.Y, winSize.X, 50, rotationAngle, new Vector3(offset.X + winOffsetL.X, offset.Y + winOffsetL.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0)));
                                    results.AddRange(winsL);

                                    var doorOffsetL = fwallOffset.Clone().Add(new Vector2(0, doorLOffsetX)).RotateAround(new Vector2(), rotationAngle);
                                    var doorsL = CreateDoor(plateBuild, doorSize.Y, doorSize.X, 50, rotationAngle, new Vector3(offset.X + doorOffsetL.X, offset.Y + doorOffsetL.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0) + slabthickness));
                                    results.AddRange(doorsL);
                                }

                                var fwallGeo = GeoModelUtil.GetStretchGeometryData(fshape, coodMat, 0, -wallthickness).GetBufferGeometry();
                                fwallGeo.computeVertexNormals();
                                fwallGeo.SetUV();
                                fwallGeo.rotateZ(rotationAngle);
                                var wfs = fwallOffset.Clone().RotateAround(new Vector2(), rotationAngle);
                                fwallGeo.translate(offset.X + wfs.X, offset.Y + wfs.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));

                                fwallOffset.Y += roomWidth;
                                var wallMesh = new Mesh(fwallGeo, mat);
                                results.Add(wallMesh);
                            }
                            {
                                //背面墙
                                var backShapePs = shape.Select(n => n.Clone()).ToListEx();
                                var backShape = new Shape(backShapePs);
                                var bwfs = bwallOffset.Clone().RotateAround(new Vector2(), rotationAngle);
                                for (var k = 0; k < room.CellNumber; k++)
                                {
                                    var bwinHole = new ListEx<Vector2>();
                                    bwinHole.Add(new Vector2((plateBuild.RoomSizeWidth / 2 - wallthickness / 2 - winSize.X) / 2 + plateBuild.RoomSizeWidth * k, winBottom));
                                    bwinHole.Add(new Vector2((plateBuild.RoomSizeWidth / 2 - wallthickness / 2 + winSize.X) / 2 + plateBuild.RoomSizeWidth * k, winBottom));
                                    bwinHole.Add(new Vector2((plateBuild.RoomSizeWidth / 2 - wallthickness / 2 + winSize.X) / 2 + plateBuild.RoomSizeWidth * k, winBottom + winSize.Y));
                                    bwinHole.Add(new Vector2((plateBuild.RoomSizeWidth / 2 - wallthickness / 2 - winSize.X) / 2 + plateBuild.RoomSizeWidth * k, winBottom + winSize.Y));
                                    backShape.holes.Add(new ThreeJs4Net.Path(bwinHole));

                                    var winOffset = bwallOffset.Clone().Add(new Vector2(-wallthickness / 2, (plateBuild.RoomSizeWidth - wallthickness) / 4 + plateBuild.RoomSizeWidth * k)).RotateAround(new Vector2(), rotationAngle);
                                    var wins = CreateWin(plateBuild, winBottom, winSize.Y, winSize.X, 50, rotationAngle, new Vector3(offset.X + winOffset.X, offset.Y + winOffset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0)));
                                    results.AddRange(wins);
                                }
                                var bwallGeo = GeoModelUtil.GetStretchGeometryData(backShape, coodMat, 0, -wallthickness).GetBufferGeometry();
                                bwallGeo.computeVertexNormals();
                                bwallGeo.SetUV();
                                bwallGeo.rotateZ(rotationAngle);
                                bwallGeo.translate(offset.X + bwfs.X, offset.Y + bwfs.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));


                                bwallOffset.Y += roomWidth;
                                var wallMesh = new Mesh(bwallGeo, mat);
                                results.Add(wallMesh);
                            }
                            {
                                var roomPWall = pwallGeo.clone();
                                pwallOffset.Y += roomWidth;
                                var pwfs = pwallOffset.Clone().RotateAround(new Vector2(), rotationAngle);
                                roomPWall.translate(pwfs.X, pwfs.Y, 0);
                                var rpwallMesh = new Mesh(roomPWall, mat);
                                results.Add(rpwallMesh);
                            }

                        }
                        //loops.Add(new Line2d(loops.Last().End.Clone(), loops.First().Start.Clone()));
                        //var profile = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loops.Select(n => n.ToCurve3d().Translate(0, 0, 0)).ToList());
                        //var extrude = new Extrude3d(profile, new Vector3(0, 0, 1), floor.FloorHeight);
                        //extrude.CreateMesh();
                        //var geo = new BufferGeometry();
                        //geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        //geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        //geo.computeVertexNormals();
                        //geo.SetUV();
                        //geo.rotateZ(rotationAngle);
                        //geo.translate(offset.X, offset.Y,floor.FloorHeight* (floor.FloorNum - 1));
                        //var mesh = new Mesh(geo, mat);
                        //results.Add(mesh);
                    }
                    {
                        var btmLoop = new List<Line2d>();
                        //var length =floor.FloorNum==1? plateBuild.RoomSizeLength: plateBuild.RoomSizeLength+ passWidth;
                        btmLoop.Add(new Line2d(new Vector2(), new Vector2(plateBuild.RoomSizeLength, 0)));
                        btmLoop.Add(new Line2d(btmLoop.Last().End.Clone(), btmLoop.Last().End.Clone().Add(new Vector2(0, plateBuild.RoomSizeWidth * floor.RoomCount))));
                        btmLoop.Add(new Line2d(btmLoop.Last().End.Clone(), btmLoop.Last().End.Clone().Add(new Vector2(-plateBuild.RoomSizeLength, 0))));
                        btmLoop.Add(new Line2d(btmLoop.Last().End.Clone(), btmLoop.First().Start.Clone()));
                        var profileBtm = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), btmLoop.Select(n => n.ToCurve3d().Translate(0, 0, 0)).ToList());
                        var extrudeBtm = new Extrude3d(profileBtm, new Vector3(0, 0, 1), slabthickness);
                        extrudeBtm.CreateMesh();
                        var geoBtm = new BufferGeometry();
                        geoBtm.setAttribute("position", new BufferAttribute(extrudeBtm.Geometry.Verteics.ToArray(), 3, false));
                        geoBtm.setIndex(new BufferAttribute(extrudeBtm.Geometry.Indics.ToArray(), 1));
                        geoBtm.computeVertexNormals();
                        geoBtm.SetUV();
                        geoBtm.rotateZ(rotationAngle);
                        geoBtm.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));
                        var meshBtm = new Mesh(geoBtm, mat);
                        results.Add(meshBtm);
                    }
                    {
                        var passLoop = new List<Line2d>();
                        passLoop.Add(new Line2d(new Vector2(plateBuild.RoomSizeLength, 0), new Vector2(plateBuild.RoomSizeLength + passWidth, 0)));
                        if (floor.FloorNum > 1)
                        {
                            if (plateBuild.StartStair)
                            {
                                var meshs = CreatePlateStair(plateBuildGroup, plateBuild, floor.FloorHeight, plateBuild.Stairs.First().StairSizeWidth, plateBuild.Stairs.First().StairSizeLength, passWidth, true);
                                results.AddRange(meshs);
                                passLoop.First().Translate(0, -plateBuild.Stairs.First().StairSizeWidth);
                            }
                            if (plateBuild.EndStair)
                            {
                                var meshs = CreatePlateStair(plateBuildGroup, plateBuild, floor.FloorHeight, plateBuild.Stairs.First().StairSizeWidth, plateBuild.Stairs.First().StairSizeLength, passWidth, false);
                                results.AddRange(meshs);
                            }
                        }
                        passLoop.Add(new Line2d(passLoop.Last().End.Clone(), new Vector2(passLoop.Last().End.X, plateBuild.RoomSizeWidth * floor.RoomCount + (plateBuild.EndStair && floor.FloorNum > 1 ? plateBuild.Stairs.Last().StairSizeWidth : 0))));
                        passLoop.Add(new Line2d(passLoop.Last().End.Clone(), passLoop.Last().End.Clone().Add(new Vector2(-passWidth, 0))));
                        passLoop.Add(new Line2d(passLoop.Last().End.Clone(), passLoop.First().Start.Clone()));
                        var profilePass = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), passLoop.Select(n => n.ToCurve3d().Translate(0, 0, 0)).ToList());
                        var extrudePass = new Extrude3d(profilePass, new Vector3(0, 0, 1), slabthickness);
                        extrudePass.CreateMesh();
                        var geoPass = new BufferGeometry();
                        geoPass.setAttribute("position", new BufferAttribute(extrudePass.Geometry.Verteics.ToArray(), 3, false));
                        geoPass.setIndex(new BufferAttribute(extrudePass.Geometry.Indics.ToArray(), 1));
                        geoPass.computeVertexNormals();
                        geoPass.SetUV();
                        geoPass.rotateZ(rotationAngle);
                        geoPass.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));
                        var meshPass = new Mesh(geoPass, floor.FloorNum > 1 ? matBoard : matRoad);
                        results.Add(meshPass);
                    }
                    if (floor.FloorNum > 1)
                    {
                        results.AddRange(CreateRailing(plateBuild, floor/*, location*/));
                    }
                    results.AddRange(CreateBand(plateBuild, floor, offset, rotationAngle, wallthickness, passWidth, roofAngle, winBottom, winSize));
                }
                if (plateBuild.IsStartClose)
                {
                    var closeShapePs = new ListEx<Vector2>();
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength, 0));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, 0));
                    var allHeight = plateBuild.Floors.Sum(n => n.FloorHeight);
                    //closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, allHeight - passWidth*Math.Tan(roofAngle)));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, allHeight));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength, allHeight));
                    var closeShape = new Shape(closeShapePs);
                    var matrix = new Matrix4();
                    matrix.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
                    var cwallGeo = GeoModelUtil.GetStretchGeometryData(closeShape, matrix, 0, wallthickness).GetBufferGeometry();
                    cwallGeo.computeVertexNormals();
                    cwallGeo.SetUV();
                    cwallGeo.translate(0, -wallthickness / 2, 0);
                    cwallGeo.rotateZ(rotationAngle);
                    cwallGeo.translate(offset.X, offset.Y, 0);
                    var cmesh = new Mesh(cwallGeo, mat);
                    results.Add(cmesh);
                }
                if (plateBuild.IsEndClose)
                {
                    var closeShapePs = new ListEx<Vector2>();
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength, 0));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, 0));
                    var allHeight = plateBuild.Floors.Sum(n => n.FloorHeight);
                    //closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, allHeight - passWidth * Math.Tan(roofAngle)));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength + passWidth, allHeight));
                    closeShapePs.Add(new Vector2(plateBuild.RoomSizeLength, allHeight));
                    var closeShape = new Shape(closeShapePs);
                    var matrix = new Matrix4();
                    matrix.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
                    var cwallGeo = GeoModelUtil.GetStretchGeometryData(closeShape, matrix, 0, wallthickness).GetBufferGeometry();
                    cwallGeo.computeVertexNormals();
                    cwallGeo.SetUV();
                    cwallGeo.translate(0, -wallthickness / 2 + plateBuild.Floors.First().RoomCount * plateBuild.RoomSizeWidth, 0);
                    cwallGeo.rotateZ(rotationAngle);
                    cwallGeo.translate(offset.X, offset.Y, 0);
                    var cmesh = new Mesh(cwallGeo, mat);
                    results.Add(cmesh);

                }
                //var roof =  CreateRoof(plateBuild,wallthickness,passWidth,rotationAngle, offset, roofAngle);
                //results.AddRange(roof);
                var roof2 = CreateRoof2(plateBuildGroup, plateBuild, wallthickness, passWidth, rotationAngle, offset, roofAngle);
                results.AddRange(roof2);

            }
            foreach (var item in results)
            {
                //var matrix = new Matrix4().MakeTranslation(-location.X,-location.Y,0);
                var matrix = new Matrix4().MakeRotationZ(plateBuildGroup.BuildRotate / 180 * Math.PI);//.Multiply(matrix);
                matrix = new Matrix4().MakeTranslation(location.X, location.Y, 0).Multiply(matrix);
                item.applyMatrix4(matrix);
            }
            return results.ToArray();
        }
        //创建集装箱封边条
        public List<Object3D> CreateBand(PlateBuilding plateBuild, PlateBuildingFloor floor, Vector2 location,double angle,double wallThickness,double passwidth,double roofAngle,double winB,Vector2 winSize)
        {
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.CurtainUuid);
            var bmat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.BoardUuid);
            var objs = new List<Object3D>();
            var loop = new List<Curve3d>();
            loop.Add(new Line3d(new Vector3(), new Vector3(20, 0, 0)));
            loop.Add(new Line3d(new Vector3(20,0,0), new Vector3(20, 50, 0)));
            loop.Add(new Line3d(new Vector3(20, 50, 0), new Vector3(0, 50, 0)));
            loop.Add(new Line3d(new Vector3(0, 50, 0), new Vector3()));
            var extrude = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loop),new Vector3(0,0,1),floor.FloorHeight);
            extrude.CreateMesh();
            var geo = new BufferGeometry();
            geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
            geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
            geo.computeVertexNormals();
            geo.SetUV();
            geo.rotateZ(angle);
            geo.translate(0,0, (floor.FloorNum - 1) * floor.FloorHeight);

            #region 屋顶走廊支架
            var extSup = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loop), new Vector3(0, 0, 1), passwidth);
            extSup.CreateMesh();
            var supGeo = new BufferGeometry();
            supGeo.setAttribute("position", new BufferAttribute(extSup.Geometry.Verteics.ToArray(), 3, false));
            supGeo.setIndex(new BufferAttribute(extSup.Geometry.Indics.ToArray(), 1));
            supGeo.computeVertexNormals();
            supGeo.SetUV();
            //supGeo.rotateY(floor.FloorNum==plateBuild.Floors.Count? Math.PI / 2  + roofAngle:Math.PI/2);
            supGeo.rotateY( Math.PI / 2);
            supGeo.rotateZ(angle);
            supGeo.translate(0, 0, plateBuild.Floors.Sum(n=>n.FloorNum<=floor.FloorNum?n.FloorHeight:0));

            var extSup2 = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loop), new Vector3(0, 0, 1), passwidth/Math.Cos(Math.PI/8));
            extSup2.CreateMesh();
            var supGeo2 = new BufferGeometry();
            supGeo2.setAttribute("position", new BufferAttribute(extSup2.Geometry.Verteics.ToArray(), 3, false));
            supGeo2.setIndex(new BufferAttribute(extSup2.Geometry.Indics.ToArray(), 1));
            supGeo2.computeVertexNormals();
            supGeo2.SetUV();
            supGeo2.rotateY(Math.PI*3 / 8);
            supGeo2.rotateZ(angle);
            supGeo2.translate(0, 0, plateBuild.Floors.Sum(n=>n.FloorNum<=floor.FloorNum?n.FloorHeight:0) - passwidth * Math.Tan(Math.PI / 8));
            //supGeo2.translate(0, 0, plateBuild.Floors.Sum(n=>n.FloorNum<floor.FloorNum?n.FloorHeight:0) - passwidth*Math.Tan(Math.PI/8)-(floor.FloorNum == plateBuild.Floors.Count ? passwidth * Math.Tan(roofAngle) : 0));
            #endregion
            #region 窗户支架
            var extT = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loop), new Vector3(0, 0, 1), plateBuild.RoomSizeWidth/2);
            extT.CreateMesh();
            var tGeo = new BufferGeometry();
            tGeo.setAttribute("position", new BufferAttribute(extT.Geometry.Verteics.ToArray(), 3, false));
            tGeo.setIndex(new BufferAttribute(extT.Geometry.Indics.ToArray(), 1));
            tGeo.computeVertexNormals();
            tGeo.SetUV();
            tGeo.rotateX(-Math.PI / 2);
            tGeo.rotateZ(angle);
            tGeo.translate(0, 0, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0));
            #endregion
            #region  防风绳
            var windH = 2000;
            var windB = 500;
            var extWind = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), new List<Curve3d>() { new Arc3d() { Radius = 5, StartAngle = 0, EndAngle = Math.PI,Center=new Vector3() }, new Arc3d() { Radius = 5, StartAngle = Math.PI, EndAngle = Math.PI*2, Center = new Vector3() } }), new Vector3(0, 0, 1),Math.Sqrt(Math.Pow(plateBuild.RoomSizeLength / 3,2)+Math.Pow(windH, 2)) );
            extWind.CreateMesh();

            var windGeo = new BufferGeometry();
            windGeo.setAttribute("position", new BufferAttribute(extWind.Geometry.Verteics.ToArray(), 3, false));
            windGeo.setIndex(new BufferAttribute(extWind.Geometry.Indics.ToArray(), 1));
            windGeo.computeVertexNormals();
            windGeo.SetUV();
            var tanAngle = Math.Atan((plateBuild.RoomSizeLength / 3)/ windH);
            windGeo.rotateY(tanAngle);
            windGeo.rotateZ(angle);
            windGeo.translate(0, 0, plateBuild.Floors.Sum(n=>n.FloorNum<floor.FloorNum?n.FloorHeight:0)+ windB);

            var winGeo2 = new BufferGeometry();
            winGeo2.setAttribute("position", new BufferAttribute(extWind.Geometry.Verteics.ToArray(), 3, false));
            winGeo2.setIndex(new BufferAttribute(extWind.Geometry.Indics.ToArray(), 1));
            winGeo2.computeVertexNormals();
            winGeo2.SetUV();
            winGeo2.rotateY(-tanAngle);
            winGeo2.rotateZ(angle);
            winGeo2.translate(0, 0, plateBuild.Floors.Sum(n=>n.FloorNum<floor.FloorNum?n.FloorHeight:0) + windB);

            var extWindV = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), new List<Curve3d>() { new Arc3d() { Radius = 5, StartAngle = 0, EndAngle = Math.PI, Center = new Vector3() }, new Arc3d() { Radius = 5, StartAngle = Math.PI, EndAngle = Math.PI * 2, Center = new Vector3() } }), new Vector3(0, 0, 1), Math.Sqrt(Math.Pow(plateBuild.RoomSizeWidth / 2, 2) + Math.Pow(windH, 2)));
            extWindV.CreateMesh();

            var windGeoV = new BufferGeometry();
            windGeoV.setAttribute("position", new BufferAttribute(extWindV.Geometry.Verteics.ToArray(), 3, false));
            windGeoV.setIndex(new BufferAttribute(extWindV.Geometry.Indics.ToArray(), 1));
            windGeoV.computeVertexNormals();
            windGeoV.SetUV();
            var tanVAngle = Math.Atan((plateBuild.RoomSizeWidth / 2) /windH);
            windGeoV.rotateX(tanVAngle);
            windGeoV.rotateZ(angle);
            windGeoV.translate(0, 0, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0) + windB);

            var windGeoV2 = new BufferGeometry();
            windGeoV2.setAttribute("position", new BufferAttribute(extWindV.Geometry.Verteics.ToArray(), 3, false));
            windGeoV2.setIndex(new BufferAttribute(extWindV.Geometry.Indics.ToArray(), 1));
            windGeoV2.computeVertexNormals();
            windGeoV2.SetUV();
            windGeoV2.rotateX(-tanVAngle);
            windGeoV2.rotateZ(angle);
            windGeoV2.translate(0, 0, plateBuild.Floors.Sum(n => n.FloorNum < floor.FloorNum ? n.FloorHeight : 0) + windB);
            #endregion
            for (var i=0;i<=floor.RoomCount*2;i++)
            {
                if (i < floor.RoomCount * 2)
                {
                    var wind = windGeoV.clone();
                    var offsetw = new Vector2(-wallThickness / 2 - 10, plateBuild.RoomSizeWidth / 2 * (i + 1));
                    offsetw = offsetw.RotateAround(new Vector2(), angle);
                    offsetw.Add(location);
                    wind.translate(offsetw.X, offsetw.Y, 0);
                    var meshw = new Mesh(wind, bmat);
                    objs.Add(meshw);

                    var wind2 = windGeoV2.clone();
                    var offsetw2 = new Vector2(-wallThickness / 2 - 10, plateBuild.RoomSizeWidth / 2 * i);
                    offsetw2 = offsetw2.RotateAround(new Vector2(), angle);
                    offsetw2.Add(location);
                    wind2.translate(offsetw2.X, offsetw2.Y, 0);
                    var meshw2 = new Mesh(wind2, bmat);
                    objs.Add(meshw2);
                    if (i % 2 == 0)
                    {
                        var window1 = tGeo.clone();
                        window1.translate(offsetw2.X, offsetw2.Y, winB - 50);
                        var meshWd = new Mesh(window1, mat);
                        objs.Add(meshWd);

                        var window2 = tGeo.clone();
                        window2.translate(offsetw2.X, offsetw2.Y, winB +winSize.Y+100);
                        var meshWd2 = new Mesh(window2, mat);
                        objs.Add(meshWd2);
                    }


                    //if (i%2==0)
                    //{
                    //    var wind3 = windGeoV.clone();
                    //    var offsetw3 = new Vector2(plateBuild.RoomSizeLength, plateBuild.RoomSizeWidth / 2 * (i + 1));
                    //    offsetw3 = offsetw3.RotateAround(new Vector2(), angle);
                    //    offsetw3.Add(location);
                    //    wind3.translate(offsetw3.X, offsetw3.Y, 0);
                    //    var meshw3= new Mesh(wind3, mat);
                    //    objs.Add(meshw3);

                    //    var wind4 = windGeoV2.clone();
                    //    var offsetw4 = new Vector2(plateBuild.RoomSizeLength, plateBuild.RoomSizeWidth / 2 * i);
                    //    offsetw4 = offsetw4.RotateAround(new Vector2(), angle);
                    //    offsetw4.Add(location);
                    //    wind4.translate(offsetw4.X, offsetw4.Y, 0);
                    //    var meshw4 = new Mesh(wind4, mat);
                    //    objs.Add(meshw4);

                    //}
                }
                {
                    var geo1 = geo.clone();
                    var offset = new Vector2(-wallThickness / 2 - 20, -wallThickness / 2 + i * plateBuild.RoomSizeWidth / 2);
                    offset = offset.RotateAround(new Vector2(), angle);
                    offset.Add(location);
                    geo1.translate(offset.X, offset.Y, 0);
                    var mesh = new Mesh(geo1, mat);
                    objs.Add(mesh);

                    var offset2 = new Vector2(wallThickness / 2 + plateBuild.RoomSizeLength + 20, 0);
                    offset2 = offset2.RotateAround(new Vector2(), angle);
                    var geo2 = geo1.clone();
                    geo2.translate(offset2.X, offset2.Y, 0);
                    var mesh2 = new Mesh(geo2, mat);
                    objs.Add(mesh2);

                    if ((plateBuild.IsStartClose && i == 0) || (plateBuild.IsEndClose && i == floor.RoomCount * 2))
                        continue;

                    var geo3 = supGeo.clone();
                    var offset3 = new Vector2().AddVectors(offset, offset2);
                    //var offset3 = new Vector2(wallThickness / 2 + plateBuild.RoomSizeLength + 20, -wallThickness / 2 + i * plateBuild.RoomSizeWidth);
                    //offset3 = offset3.RotateAround(new Vector2(), angle);
                    //offset3.Add(location);
                    geo3.translate(offset3.X, offset3.Y, 0);
                    var mesh3 = new Mesh(geo3, mat);
                    objs.Add(mesh3);

                    var geo4 = supGeo2.clone();
                    geo4.translate(offset3.X, offset3.Y, 0);
                    var mesh4 = new Mesh(geo4, mat);
                    objs.Add(mesh4);
                }
            }
            for (var i = 0; i < floor.Rooms.Count; i++)
            {
                var room = floor.Rooms[i];
                var beforeOffsetL = floor.Rooms.Take(i).Sum(n=>n.CellNumber)*plateBuild.RoomSizeWidth;
                var offsetX = plateBuild.RoomSizeLength+10;
                if (room.CellNumber == 1)
                {
                    var wind3 = windGeoV.clone();
                    var offsetw3 = new Vector2(offsetX, plateBuild.RoomSizeWidth / 2 + beforeOffsetL);
                    offsetw3 = offsetw3.RotateAround(new Vector2(), angle);
                    offsetw3.Add(location);
                    wind3.translate(offsetw3.X, offsetw3.Y, 0);
                    var meshw3 = new Mesh(wind3, bmat);
                    objs.Add(meshw3);

                    var wind4 = windGeoV2.clone();
                    var offsetw4 = new Vector2(offsetX, beforeOffsetL);
                    offsetw4 = offsetw4.RotateAround(new Vector2(), angle);
                    offsetw4.Add(location);
                    wind4.translate(offsetw4.X, offsetw4.Y, 0);
                    var meshw4 = new Mesh(wind4, bmat);
                    objs.Add(meshw4);

                    var offsetWd = new Vector2(offsetX - wallThickness/2, beforeOffsetL);
                    offsetWd = offsetWd.RotateAround(new Vector2(), angle);
                    offsetWd.Add(location);
                    var window1 = tGeo.clone();
                    window1.translate(offsetWd.X, offsetWd.Y, winB - 50);
                    var meshWd = new Mesh(window1, mat);
                    objs.Add(meshWd);

                    var window2 = tGeo.clone();
                    window2.translate(offsetWd.X, offsetWd.Y, winB + winSize.Y + 100);
                    var meshWd2 = new Mesh(window2, mat);
                    objs.Add(meshWd2);
                }
                else
                {
                    var wind1 = windGeoV.clone();
                    var offsetw1 = new Vector2(offsetX, plateBuild.RoomSizeWidth + beforeOffsetL + (room.CellNumber - 1.5) * plateBuild.RoomSizeWidth);
                    offsetw1 = offsetw1.RotateAround(new Vector2(), angle);
                    offsetw1.Add(location);
                    wind1.translate(offsetw1.X, offsetw1.Y, 0);
                    var meshw1 = new Mesh(wind1, bmat);
                    objs.Add(meshw1);

                    var wind2 = windGeoV2.clone();
                    var offsetw2 = new Vector2(offsetX, plateBuild.RoomSizeWidth / 2 + beforeOffsetL + (room.CellNumber - 1.5) * plateBuild.RoomSizeWidth);
                    offsetw2 = offsetw2.RotateAround(new Vector2(), angle);
                    offsetw2.Add(location);
                    wind2.translate(offsetw2.X, offsetw2.Y, 0);
                    var meshw2 = new Mesh(wind2, bmat);
                    objs.Add(meshw2);

                    var wind3 = windGeoV.clone();
                    var offsetw3 = new Vector2(offsetX, plateBuild.RoomSizeWidth + beforeOffsetL);
                    offsetw3 = offsetw3.RotateAround(new Vector2(), angle);
                    offsetw3.Add(location);
                    wind3.translate(offsetw3.X, offsetw3.Y, 0);
                    var meshw3 = new Mesh(wind3, bmat);
                    objs.Add(meshw3);

                    var wind4 = windGeoV2.clone();
                    var offsetw4 = new Vector2(offsetX, plateBuild.RoomSizeWidth / 2 + beforeOffsetL);
                    offsetw4 = offsetw4.RotateAround(new Vector2(), angle);
                    offsetw4.Add(location);
                    wind4.translate(offsetw4.X, offsetw4.Y, 0);
                    var meshw4 = new Mesh(wind4, bmat);
                    objs.Add(meshw4);

                    for(var k=2;k< (room.CellNumber-1)*2;k++)
                    {
                        var wind5 = windGeoV.clone();
                        var offsetw5 = new Vector2(offsetX, plateBuild.RoomSizeWidth/2*(k+1) + beforeOffsetL);
                        offsetw5 = offsetw5.RotateAround(new Vector2(), angle);
                        offsetw5.Add(location);
                        wind5.translate(offsetw5.X, offsetw5.Y, 0);
                        var meshw5 = new Mesh(wind5, bmat);
                        objs.Add(meshw5);

                        var wind6 = windGeoV2.clone();
                        var offsetw6 = new Vector2(offsetX, plateBuild.RoomSizeWidth / 2 *k+ beforeOffsetL);
                        offsetw6 = offsetw6.RotateAround(new Vector2(), angle);
                        offsetw6.Add(location);
                        wind6.translate(offsetw6.X, offsetw6.Y, 0);
                        var meshw6 = new Mesh(wind6, bmat);
                        objs.Add(meshw6);
                    }

                    var offsetWd = new Vector2(offsetX - wallThickness / 2, plateBuild.RoomSizeWidth / 2 + beforeOffsetL + (room.CellNumber - 1.5) * plateBuild.RoomSizeWidth);
                    offsetWd = offsetWd.RotateAround(new Vector2(), angle);
                    offsetWd.Add(location);
                    var window1 = tGeo.clone();
                    window1.translate(offsetWd.X, offsetWd.Y, winB - 50);
                    var meshWd = new Mesh(window1, mat);
                    objs.Add(meshWd);

                    var window2 = tGeo.clone();
                    window2.translate(offsetWd.X, offsetWd.Y, winB + winSize.Y + 100);
                    var meshWd2 = new Mesh(window2, mat);
                    objs.Add(meshWd2);

                    var offsetWd2 = new Vector2(offsetX - wallThickness / 2, plateBuild.RoomSizeWidth / 2 + beforeOffsetL);
                    offsetWd2 = offsetWd2.RotateAround(new Vector2(), angle);
                    offsetWd2.Add(location);
                    var window3 = tGeo.clone();
                    window3.translate(offsetWd2.X, offsetWd2.Y, winB - 50);
                    var meshWd3 = new Mesh(window3, mat);
                    objs.Add(meshWd3);

                    var window4 = tGeo.clone();
                    window4.translate(offsetWd2.X, offsetWd2.Y, winB + winSize.Y + 100);
                    var meshWd4 = new Mesh(window4, mat);
                    objs.Add(meshWd4);

                }
            }
            for (var i=0;i<3;i++)
            {
                var wind = windGeo.clone();
                var offsetw = new Vector2( plateBuild.RoomSizeLength / 3 * i, -wallThickness / 2 - 10);
                offsetw = offsetw.RotateAround(new Vector2(), angle);
                offsetw.Add(location);
                wind.translate(offsetw.X, offsetw.Y, 0);
                var meshw = new Mesh(wind, bmat);
                objs.Add(meshw);

                var wind2 = winGeo2.clone();
                var offsetw2 = new Vector2( plateBuild.RoomSizeLength / 3 * (i+1), -wallThickness / 2 - 10);
                offsetw2 = offsetw2.RotateAround(new Vector2(), angle);
                offsetw2.Add(location);
                wind2.translate(offsetw2.X, offsetw2.Y, 0);
                var meshw2 = new Mesh(wind2, bmat);
                objs.Add(meshw2);

                var wind3 = windGeo.clone();
                var offsetw3 = new Vector2(plateBuild.RoomSizeLength / 3 * i, plateBuild.RoomSizeWidth * floor.RoomCount + wallThickness / 2 + 10);
                offsetw3 = offsetw3.RotateAround(new Vector2(), angle);
                offsetw3.Add(location);
                wind3.translate(offsetw3.X, offsetw3.Y, 0);
                var meshw3 = new Mesh(wind3, bmat);
                objs.Add(meshw3);

                var wind4 = winGeo2.clone();
                var offsetw4 = new Vector2(plateBuild.RoomSizeLength / 3 * (i + 1), plateBuild.RoomSizeWidth * floor.RoomCount + wallThickness / 2 + 10);
                offsetw4 = offsetw4.RotateAround(new Vector2(), angle);
                offsetw4.Add(location);
                wind4.translate(offsetw4.X, offsetw4.Y, 0);
                var meshw4 = new Mesh(wind4, bmat);
                objs.Add(meshw4);
            }
            #region 板房左右两侧支架
            var geoL = geo.clone();
            geoL.rotateZ(-Math.PI / 2);
            var magin = -wallThickness / 2 - 20;
            var offsetL = new Vector2(magin, - wallThickness / 2);
            offsetL = offsetL.RotateAround(new Vector2(), angle);
            offsetL.Add(location);
            geoL.translate(offsetL.X, offsetL.Y, 0);
            var meshl1 = new Mesh(geoL, mat);
            objs.Add(meshl1);

            var geoL2 = geoL.clone();
            var offsetL2 = new Vector2(plateBuild.RoomSizeLength / 3 - magin - 25, 0);
            offsetL2 = offsetL2.RotateAround(new Vector2(), angle);
            geoL2.translate(offsetL2.X, offsetL2.Y, 0);
            var meshl2 = new Mesh(geoL2, mat);
            objs.Add(meshl2);

            var geoL3 = geoL.clone();
            var offsetL3 = new Vector2(plateBuild.RoomSizeLength * 2 / 3- magin-25, 0);
            offsetL3 = offsetL3.RotateAround(new Vector2(), angle);
            geoL3.translate(offsetL3.X, offsetL3.Y, 0);
            var meshl3 = new Mesh(geoL3, mat);
            objs.Add(meshl3);

            var geoL4 = geoL.clone();
            var offsetL4 = new Vector2( plateBuild.RoomSizeLength +20, 0);
            offsetL4 = offsetL4.RotateAround(new Vector2(), angle);
            geoL4.translate(offsetL4.X, offsetL4.Y, 0);
            var meshl4 = new Mesh(geoL4, mat);
            objs.Add(meshl4);

            var geoR = geoL.clone();
            var offsetR = new Vector2(0, plateBuild.RoomSizeWidth * floor.RoomCount + wallThickness + 20);
            offsetR = offsetR.RotateAround(new Vector2(), angle);
            geoR.translate(offsetR.X, offsetR.Y, 0);
            var meshR = new Mesh(geoR, mat);
            objs.Add(meshR);

            var geoR2 = geoL2.clone();
            geoR2.translate(offsetR.X, offsetR.Y, 0);
            var meshR2 = new Mesh(geoR2, mat);
            objs.Add(meshR2);

            var geoR3 = geoL3.clone();
            geoR3.translate(offsetR.X, offsetR.Y, 0);
            var meshR3 = new Mesh(geoR3, mat);
            objs.Add(meshR3);

            var geoR4 = geoL4.clone();
            geoR4.translate(offsetR.X, offsetR.Y, 0);
            var meshR4 = new Mesh(geoR4, mat);
            objs.Add(meshR4);
            #endregion
            //楼层间隔支架
            {
                var loop2 = new List<Curve3d>();
                loop2.Add(new Line3d(new Vector3(-wallThickness / 2, -wallThickness / 2), new Vector3(plateBuild.RoomSizeLength, -wallThickness / 2)));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(0, plateBuild.RoomSizeWidth * floor.RoomCount+ wallThickness, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(20, 0, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(0, -plateBuild.RoomSizeWidth * floor.RoomCount - wallThickness-20, 0))));
                //loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(0, -20, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(-plateBuild.RoomSizeLength - wallThickness / 2 - 40, 0, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(0, plateBuild.RoomSizeWidth * floor.RoomCount + wallThickness + 40, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(plateBuild.RoomSizeLength + wallThickness / 2 + 20, 0, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(0, -20, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.Last().End.Clone().Add(new Vector3(-plateBuild.RoomSizeLength - wallThickness / 2, 0, 0))));
                loop2.Add(new Line3d(loop2.Last().End.Clone(), loop2.First().Start.Clone()));
                loop2.Reverse();
                loop2.ForEach(n => n.Reverse());
                var extrude2 = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), loop2), new Vector3(0, 0, 1), 50);
                extrude2.CreateMesh();
                var geo2 = new BufferGeometry();
                geo2.setAttribute("position", new BufferAttribute(extrude2.Geometry.Verteics.ToArray(), 3, false));
                geo2.setIndex(new BufferAttribute(extrude2.Geometry.Indics.ToArray(), 1));
                geo2.computeVertexNormals();
                geo2.SetUV();
                geo2.rotateZ(angle);
                geo2.translate(location.X, location.Y, plateBuild.Floors.Sum(n=>n.FloorNum<=floor.FloorNum?n.FloorHeight:0) - 50);
                var mesh2 = new Mesh(geo2, mat);
                objs.Add(mesh2);
            }
            return objs;
        }
        //创建门
        public List<Object3D> CreateDoor(PlateBuilding plateBuild, double height, double width, double thickness, double angle, Vector3 offset)
        {
            var objs = new List<Object3D>();
            //var shape = ShapeUtil.Rect(0, 0, width, height);
            //var start = new Vector3(0, 0, 0);
            //var end = new Vector3(width, 0, 0);
            //var lineVec = new Vector3().SubVectors(end, start);
            //var len = lineVec.Length();
            //var xAxis = lineVec.Clone().Normalize();
            //var yAxis = GeoUtil.ZAxis;
            //var zAxis = new Vector3().CrossVectors(xAxis, yAxis).Normalize();
            //var coodMat = new Matrix4();
            //coodMat.MakeBasis(xAxis, yAxis, zAxis);
            //coodMat.SetPosition(start);
            //var doorGeoData = GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, -thickness);
            var profile = new List<Curve3d>();
            profile.Add(new Line3d(new Vector3(),new Vector3(width,0,0)));
            profile.Add(new Line3d(new Vector3(width, 0,0), new Vector3(width, thickness, 0)));
            profile.Add(new Line3d(new Vector3(width, thickness,0), new Vector3(0, thickness, 0)));
            profile.Add(new Line3d(new Vector3(0, thickness, 0), new Vector3()));
            var extrude = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)),profile),new Vector3(0,0,1),height);
            extrude.CreateMesh();
            var doorGeo = new BufferGeometry();
            doorGeo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics , 3, false));
            doorGeo.setIndex(new BufferAttribute(extrude.Geometry.Indics, 1));
            doorGeo.setAttribute("uv", new BufferAttribute(extrude.Geometry.Uvs, 2, false));
            doorGeo.setAttribute("normal", new BufferAttribute(extrude.Geometry.Normals, 3, false));
            //doorGeo.SetUV();
            doorGeo.rotateZ(Math.PI / 2);
            doorGeo.rotateZ(angle);
            doorGeo.translate(offset.X, offset.Y, offset.Z);
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.DoorUuid);
            var mesh = new Mesh(doorGeo, mat);
            objs.Add(mesh);
            return objs;
        }
        //创建窗户
        public List<Object3D> CreateWin(PlateBuilding plateBuild,double bottom,double height,double width,double thickness,double angle,Vector3 offset)
        {
            var glassThickness = 10;
            var frameWidth = 50;
            var start = new Vector3(-width / 2, 0, bottom);
            var end = new Vector3(width / 2, 0, bottom);
            var lineVec = new Vector3().SubVectors(end, start);
            var len = lineVec.Length();

            var xAxis = lineVec.Clone().Normalize();
            var yAxis = GeoUtil.ZAxis;
            var zAxis = new Vector3().CrossVectors(xAxis, yAxis).Normalize();
            var coodMat = new Matrix4();
            coodMat.MakeBasis(xAxis, yAxis, zAxis);
            coodMat.SetPosition(start);

            var leftGlassShape = ShapeUtil.Rect(frameWidth, frameWidth, len / 2 - frameWidth, height - frameWidth);
            var rightGlassShape = ShapeUtil.Rect(frameWidth + len / 2, frameWidth, len - frameWidth, height - frameWidth);
            var leftFrameShape = ShapeUtil.Rect(0, 0, len / 2, height);
            var rightFrameShape = ShapeUtil.Rect(len / 2, 0, len, height);
            leftFrameShape.holes.Add(leftGlassShape);
            rightFrameShape.holes.Add(rightGlassShape);

            var topGlassGeoData = GeoModelUtil.GetStretchGeometryData(leftGlassShape, coodMat, -thickness / 2 + glassThickness / 2, -thickness / 2 - glassThickness / 2);
            var bottomGlassGeoData = GeoModelUtil.GetStretchGeometryData(rightGlassShape, coodMat, thickness / 2 + glassThickness / 2, thickness - glassThickness / 2);
            var topFrameGeoData = GeoModelUtil.GetStretchGeometryData(leftFrameShape, coodMat, 0, -thickness/2);
            var bottomFrameGeoData = GeoModelUtil.GetStretchGeometryData(rightFrameShape, coodMat, thickness/2, 0);

            var objs=new List<Object3D>();
            var glassGeo = new BufferGeometry();
            glassGeo.setAttribute("position", new BufferAttribute(topGlassGeoData.Position.ToArray(), 3, false));
            glassGeo.setIndex(new BufferAttribute(topGlassGeoData.Index.ToArray(), 1));
            glassGeo.computeVertexNormals();
            glassGeo.SetUV();
            glassGeo.rotateZ(Math.PI/2);
            glassGeo.rotateZ(angle);
            glassGeo.translate(offset.X,offset.Y,offset.Z);
            var glassMat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.GlassUuid);
            var meshglass = new Mesh(glassGeo, glassMat);
            objs.Add(meshglass);

            var glassGeo2 = new BufferGeometry();
            glassGeo2.setAttribute("position", new BufferAttribute(bottomGlassGeoData.Position.ToArray(), 3, false));
            glassGeo2.setIndex(new BufferAttribute(bottomGlassGeoData.Index.ToArray(), 1));
            glassGeo2.computeVertexNormals();
            glassGeo2.SetUV();
            glassGeo2.rotateZ(Math.PI / 2);
            glassGeo2.rotateZ(angle);
            glassGeo2.translate(offset.X, offset.Y, offset.Z); 
            var meshglass2 = new Mesh(glassGeo2, glassMat);
            objs.Add(meshglass2);

            var frameGeo = new BufferGeometry();
            frameGeo.setAttribute("position", new BufferAttribute(topFrameGeoData.Position.ToArray(), 3, false));
            frameGeo.setIndex(new BufferAttribute(topFrameGeoData.Index.ToArray(), 1));
            frameGeo.computeVertexNormals();
            frameGeo.SetUV();
            frameGeo.rotateZ(Math.PI / 2);
            frameGeo.rotateZ(angle);
            frameGeo.translate(offset.X, offset.Y, offset.Z);
            var frameMat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.Metal1Uuid);
            var meshframe = new Mesh(frameGeo, frameMat);
            objs.Add(meshframe);

            var frameGeo2 = new BufferGeometry();
            frameGeo2.setAttribute("position", new BufferAttribute(bottomFrameGeoData.Position.ToArray(), 3, false));
            frameGeo2.setIndex(new BufferAttribute(bottomFrameGeoData.Index.ToArray(), 1));
            frameGeo2.computeVertexNormals();
            frameGeo2.SetUV();
            frameGeo2.rotateZ(Math.PI / 2);
            frameGeo2.rotateZ(angle);
            frameGeo2.translate(offset.X, offset.Y, offset.Z);
            var meshframe2 = new Mesh(frameGeo2, frameMat);
            objs.Add(meshframe2);
            return objs;
        }
        //创建屋顶
        public List<Object3D> CreateRoof(PlateBuilding plateBuild,double wallthickness,double passWidth,double angle,Vector2 offset,double roofAngle)
        {
            var objs = new List<Object3D>();
            var roofShapePs = new ListEx<Vector2>();
            var widthHalf = plateBuild.RoomSizeLength / 2 + wallthickness / 4 + passWidth;
            var ch = (widthHalf- passWidth) * Math.Tan(roofAngle);
            var bch = widthHalf * Math.Tan(roofAngle);
            var startX = -passWidth - wallthickness / 2;
            roofShapePs.Add(new Vector2(startX, plateBuild.Floors.Sum(n => n.FloorHeight) + ch-bch));
            roofShapePs.Add(new Vector2(startX + widthHalf, plateBuild.Floors.Sum(n => n.FloorHeight) + ch));
            roofShapePs.Add(new Vector2(startX + widthHalf * 2, plateBuild.Floors.Sum(n => n.FloorHeight) + ch - bch));
            roofShapePs.Add(new Vector2(startX + widthHalf * 2, plateBuild.Floors.Sum(n => n.FloorHeight) + ch - bch + wallthickness));
            roofShapePs.Add(new Vector2(startX + widthHalf, plateBuild.Floors.Sum(n => n.FloorHeight) + ch + wallthickness));
            roofShapePs.Add(new Vector2(startX, plateBuild.Floors.Sum(n => n.FloorHeight) + ch - bch + wallthickness));
            if (ShapeUtils.isClockWise(roofShapePs))
            {
                roofShapePs.Reverse();
            }
            var shape = new ThreeJs4Net.Shape(roofShapePs);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            var roofGeo = GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, plateBuild.RoomSizeWidth*plateBuild.Floors.Last().RoomCount+wallthickness).GetBufferGeometry();
            roofGeo.computeVertexNormals();
            roofGeo.SetUV();
            roofGeo.translate(0,-wallthickness/2,0);
            roofGeo.rotateZ(angle);
            roofGeo.translate(offset.X, offset.Y, 0);
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.RoofUuid);
            var meshroof = new Mesh(roofGeo, mat);
            objs.Add(meshroof);
            return objs;
        }
        public List<Object3D> CreateRoof2(PlateBuildGroup group, PlateBuilding plateBuild, double wallthickness, double passWidth, double angle, Vector2 offset, double roofAngle)
        {
            var connectSType = "N";//N：起始点无连接。X:侧边有楼栋连接。D:顶部有楼栋连接
            var connectEType = "N";
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.RoofUuid);
            var baseLine = new Line2d(plateBuild.BuildingLocation.Clone(),plateBuild.BuildingLocation.Clone().AddScaledVector(plateBuild.BuildingDirect, plateBuild.Floors.First().RoomCount* plateBuild.RoomSizeWidth));
            foreach (PlateBuilding ele in group.Elements)
            {
                if (ele==plateBuild)
                {
                    continue;
                }
                var oline= new Line2d(ele.BuildingLocation.Clone(), ele.BuildingLocation.Clone().AddScaledVector(ele.BuildingDirect, ele.Floors.First().RoomCount * ele.RoomSizeWidth));
                var cp = Intersect2d.XLineWithXLine(baseLine.Start,baseLine.Dir ,oline.Start,oline.Dir);
                if (cp!=null)
                {
                    if (cp.Similarity(baseLine.Start))
                    {
                        connectSType = "X";
                    }else if (cp.Similarity(baseLine.End))
                    {
                        connectEType = "X";
                    }else if (cp.DistanceToSquared(baseLine.Start)< cp.DistanceToSquared(baseLine.End))
                    {
                        connectSType = "D";
                    }
                    else
                    {
                        connectEType = "D";
                    }

                }
            }
            var roofThickness = 50;
            //屋檐宽度
            var eaveWidth = 100;
            var objs = new List<Object3D>();
            var widthHalf = plateBuild.RoomSizeLength / 2 + wallthickness / 4 + passWidth/2;
            var tanRfa = Math.Tan(roofAngle);
            var ch = tanRfa * widthHalf;
            var cy = ch / tanRfa;
            var startP = new Vector3(-wallthickness / 2 + widthHalf, eaveWidth, ch);
            var endP = new Vector3(-wallthickness / 2 + widthHalf, plateBuild.RoomSizeWidth * plateBuild.Floors.First().RoomCount - eaveWidth, ch);
            var stP = new Vector3(-wallthickness / 2 + widthHalf, cy - wallthickness / 2, ch);
            var slbp = new Vector3(-wallthickness / 2 - eaveWidth, -eaveWidth - wallthickness / 2, -eaveWidth * tanRfa);
            var srbp = new Vector3(widthHalf * 2 - wallthickness / 2 + eaveWidth, -eaveWidth - wallthickness / 2, -eaveWidth * tanRfa);
            var etP = new Vector3(-wallthickness / 2 + widthHalf, plateBuild.RoomSizeWidth * plateBuild.Floors.First().RoomCount + wallthickness / 2 - cy, ch);
            var elbp = new Vector3(-wallthickness / 2 - eaveWidth, plateBuild.RoomSizeWidth * plateBuild.Floors.First().RoomCount + eaveWidth + wallthickness / 2, -eaveWidth * tanRfa);
            var erbp = new Vector3(widthHalf * 2 - wallthickness / 2 + eaveWidth, plateBuild.RoomSizeWidth * plateBuild.Floors.First().RoomCount + eaveWidth + wallthickness / 2, -eaveWidth * tanRfa);
            if (connectSType=="N")
            {
                {
                    var loop = new List<Line3d>() { };
                    loop.Add(new Line3d(srbp.Clone(), stP.Clone()));
                    loop.Add(new Line3d(stP.Clone(), slbp.Clone()));
                    loop.Add(new Line3d(slbp.Clone(), srbp.Clone()));
                    var normal = new Vector3().CrossVectors(loop[0].Dir, loop[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loop.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var mesh = new Mesh(geo, mat);
                    objs.Add(mesh);
                }
                if (connectEType=="X")
                {
                    {
                        var loopE = new List<Line3d>() { };
                        loopE.Add(new Line3d(elbp.Clone(), etP.Clone()));
                        loopE.Add(new Line3d(etP.Clone(), new Vector3(erbp.X , etP.Y, etP.Z)));
                        loopE.Add(new Line3d(loopE.Last().End.Clone(), erbp.Clone()));
                        loopE.Add(new Line3d(loopE.Last().End.Clone(), elbp.Clone()));
                        var normalE = new Vector3().CrossVectors(loopE[0].Dir, loopE[1].Dir);
                        var extrudeE = new Extrude3d(new PlanarSurface3d(new Plane(normalE), loopE.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrudeE.CreateMesh();
                        var geoE = new BufferGeometry();
                        geoE.setAttribute("position", new BufferAttribute(extrudeE.Geometry.Verteics.ToArray(), 3, false));
                        geoE.setIndex(new BufferAttribute(extrudeE.Geometry.Indics.ToArray(), 1));
                        geoE.computeVertexNormals();
                        geoE.SetUV();
                        geoE.rotateZ(angle);
                        geoE.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshE = new Mesh(geoE, mat);
                        objs.Add(meshE);
                    }
                    var connectp = new Vector3(srbp.X, etP.Y - widthHalf - eaveWidth, srbp.Z);
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(etP.Clone(), stP.Clone()));
                        loopR.Add(new Line3d(stP.Clone(), srbp.Clone()));
                        loopR.Add(new Line3d(srbp.Clone(), connectp.Clone()));
                        loopR.Add(new Line3d(connectp.Clone(), etP.Clone()));
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);


                    }
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(etP.Clone(), connectp.Clone()));
                        loopR.Add(new Line3d(connectp.Clone(), new Vector3(connectp.X,etP.Y, etP.Z)));
                        loopR.Add(new Line3d(loopR.Last().End.Clone(), etP.Clone()));
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);
                    }
                }
                else if (connectEType == "D")
                {
                    {
                        var loopL = new List<Line3d>() { };
                        loopL.Add(new Line3d(slbp.Clone(), stP.Clone()));
                        loopL.Add(new Line3d(stP.Clone(), endP.Clone()));
                        loopL.Add(new Line3d(endP.Clone(), new Vector3(elbp.X,endP.Y,elbp.Z)));
                        loopL.Add(new Line3d(loopL.Last().End.Clone(), loopL.First().Start.Clone()));
                        var normal = new Vector3().CrossVectors(loopL[0].Dir, loopL[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopL.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);
                    }
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(new Vector3(erbp.X, endP.Y, erbp.Z), endP.Clone()));
                        loopR.Add(new Line3d(endP.Clone(), stP.Clone()));
                        loopR.Add(new Line3d(stP.Clone(), srbp.Clone()));
                        loopR.Add(new Line3d(loopR.Last().End.Clone(), loopR.First().Start.Clone()));
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);
                    } 
                }
            }
             

            if (connectEType=="N")
            {
                {
                    var loop = new List<Line3d>() { };
                    loop.Add(new Line3d(elbp.Clone(), etP.Clone()));
                    loop.Add(new Line3d(etP.Clone(), erbp.Clone()));
                    loop.Add(new Line3d(erbp.Clone(), elbp.Clone()));
                    var normal = new Vector3().CrossVectors(loop[0].Dir, loop[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loop.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshFront = new Mesh(geo, mat);
                    objs.Add(meshFront);
                }
                if (connectSType == "X")
                {
                    {
                        var loopS = new List<Line3d>() { };
                        loopS.Add(new Line3d(slbp.Clone(), new Vector3(srbp.X , srbp.Y, srbp.Z)));
                        loopS.Add(new Line3d(loopS.Last().End.Clone(), new Vector3(srbp.X  , stP.Y, stP.Z)));
                        loopS.Add(new Line3d(loopS.Last().End.Clone(), stP.Clone()));
                        loopS.Add(new Line3d(stP.Clone(), slbp.Clone()));
                        var normalS = new Vector3().CrossVectors(loopS[0].Dir, loopS[1].Dir);
                        var extrudeS = new Extrude3d(new PlanarSurface3d(new Plane(normalS), loopS.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrudeS.CreateMesh();
                        var geoS = new BufferGeometry();
                        geoS.setAttribute("position", new BufferAttribute(extrudeS.Geometry.Verteics.ToArray(), 3, false));
                        geoS.setIndex(new BufferAttribute(extrudeS.Geometry.Indics.ToArray(), 1));
                        geoS.computeVertexNormals();
                        geoS.SetUV();
                        geoS.rotateZ(angle);
                        geoS.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshS = new Mesh(geoS, mat);
                        objs.Add(meshS);
                    }
                    var connectp = new Vector3(srbp.X, stP.Y + widthHalf + eaveWidth, slbp.Z);
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(stP.Clone(), connectp.Clone()));
                        loopR.Add(new Line3d(connectp.Clone(), erbp.Clone()));
                        loopR.Add(new Line3d(erbp.Clone(), etP.Clone()));
                        loopR.Add(new Line3d(etP.Clone(), stP.Clone()));
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshR = new Mesh(geo, mat);
                        objs.Add(meshR);
                    }
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(stP.Clone(),new Vector3(connectp.X,stP.Y, stP.Z)));
                        loopR.Add(new Line3d(loopR.Last().End.Clone(), connectp.Clone()));
                        loopR.Add(new Line3d(connectp.Clone(), stP.Clone())); 
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshR = new Mesh(geo, mat);
                        objs.Add(meshR);
                    }
                }
                else if (connectSType == "D")
                {
                    {
                        var loopL = new List<Line3d>() { };
                        loopL.Add(new Line3d(new Vector3(slbp.X, startP.Y, slbp.Z), startP.Clone()));
                        loopL.Add(new Line3d(startP.Clone(), etP.Clone()));
                        loopL.Add(new Line3d(etP.Clone(), elbp.Clone()));
                        loopL.Add(new Line3d(loopL.Last().End.Clone(), loopL.First().Start.Clone()));
                        var normal = new Vector3().CrossVectors(loopL[0].Dir, loopL[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopL.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);
                    }
                    {
                        var loopR = new List<Line3d>() { };
                        loopR.Add(new Line3d(erbp.Clone(), etP.Clone()));
                        loopR.Add(new Line3d(etP.Clone(), startP.Clone()));
                        loopR.Add(new Line3d(startP.Clone(), new Vector3(srbp.X, startP.Y, srbp.Z)));
                        loopR.Add(new Line3d(loopR.Last().End.Clone(), loopR.First().Start.Clone()));
                        var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                        var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                        extrude.CreateMesh();
                        var geo = new BufferGeometry();
                        geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                        geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                        geo.computeVertexNormals();
                        geo.SetUV();
                        geo.rotateZ(angle);
                        geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                        var meshL = new Mesh(geo, mat);
                        objs.Add(meshL);
                    }
                }
            }
            if (connectSType != "D" && connectEType != "D")
            {
                var loopL = new List<Line3d>() { };
                loopL.Add(new Line3d(slbp.Clone(), stP.Clone()));
                loopL.Add(new Line3d(stP.Clone(), etP.Clone()));
                loopL.Add(new Line3d(etP.Clone(),  elbp));
                loopL.Add(new Line3d(elbp.Clone(), slbp.Clone()));
                var normalL = new Vector3().CrossVectors(loopL[0].Dir, loopL[1].Dir);
                var extrudeL = new Extrude3d(new PlanarSurface3d(new Plane(normalL), loopL.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                extrudeL.CreateMesh();
                var geoL = new BufferGeometry();
                geoL.setAttribute("position", new BufferAttribute(extrudeL.Geometry.Verteics.ToArray(), 3, false));
                geoL.setIndex(new BufferAttribute(extrudeL.Geometry.Indics.ToArray(), 1));
                geoL.computeVertexNormals();
                geoL.SetUV();
                geoL.rotateZ(angle);
                geoL.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                var meshL = new Mesh(geoL, mat);
                objs.Add(meshL);
            }
            if (connectEType == "N" && connectSType == "N")
            {
                var loopR = new List<Line3d>() { };
                loopR.Add(new Line3d(erbp.Clone(), etP.Clone()));
                loopR.Add(new Line3d(etP.Clone(), stP.Clone()));
                loopR.Add(new Line3d(stP.Clone(), srbp.Clone()));
                loopR.Add(new Line3d(loopR.Last().End.Clone(), loopR.First().Start.Clone()));
                var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                extrude.CreateMesh();
                var geo = new BufferGeometry();
                geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                geo.computeVertexNormals();
                geo.SetUV();
                geo.rotateZ(angle);
                geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                var meshL = new Mesh(geo, mat);
                objs.Add(meshL);
            }
            if (connectEType == "D" && connectSType == "D")
            {
                {
                    var loopL = new List<Line3d>() { };
                    loopL.Add(new Line3d(new Vector3(slbp.X,startP.Y,slbp.Z), startP.Clone()));
                    loopL.Add(new Line3d(startP.Clone(), endP.Clone()));
                    loopL.Add(new Line3d(endP.Clone(), new Vector3(elbp.X, endP.Y, elbp.Z)));
                    loopL.Add(new Line3d(loopL.Last().End.Clone(), loopL.First().Start.Clone()));
                    var normal = new Vector3().CrossVectors(loopL[0].Dir, loopL[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopL.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshL = new Mesh(geo, mat);
                    objs.Add(meshL);
                }
                {
                    var loopR = new List<Line3d>() { };
                    loopR.Add(new Line3d(new Vector3(erbp.X, endP.Y, erbp.Z), endP.Clone()));
                    loopR.Add(new Line3d(endP.Clone(), startP.Clone()));
                    loopR.Add(new Line3d(startP.Clone(), new Vector3(srbp.X, startP.Y, srbp.Z)));
                    loopR.Add(new Line3d(loopR.Last().End.Clone(), loopR.First().Start.Clone()));
                    var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshL = new Mesh(geo, mat);
                    objs.Add(meshL);
                }
            }
            if (connectEType == "X" && connectSType == "X")
            {
                {
                    var loopS = new List<Line3d>() { };
                    loopS.Add(new Line3d(slbp.Clone(), srbp.Clone()));
                    loopS.Add(new Line3d(srbp.Clone(), new Vector3(srbp.X, stP.Y, stP.Z)));
                    loopS.Add(new Line3d(loopS.Last().End.Clone(), stP.Clone()));
                    loopS.Add(new Line3d(stP.Clone(), slbp.Clone()));
                    var normalS = new Vector3().CrossVectors(loopS[0].Dir, loopS[1].Dir);
                    var extrudeS = new Extrude3d(new PlanarSurface3d(new Plane(normalS), loopS.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrudeS.CreateMesh();
                    var geoS = new BufferGeometry();
                    geoS.setAttribute("position", new BufferAttribute(extrudeS.Geometry.Verteics.ToArray(), 3, false));
                    geoS.setIndex(new BufferAttribute(extrudeS.Geometry.Indics.ToArray(), 1));
                    geoS.computeVertexNormals();
                    geoS.SetUV();
                    geoS.rotateZ(angle);
                    geoS.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshS = new Mesh(geoS, mat);
                    objs.Add(meshS);
                }
                {
                    var loopE = new List<Line3d>() { };
                    loopE.Add(new Line3d(elbp.Clone(), etP.Clone()));
                    loopE.Add(new Line3d(etP.Clone(), new Vector3(erbp.X, etP.Y, etP.Z)));
                    loopE.Add(new Line3d(loopE.Last().End.Clone(), erbp.Clone()));
                    loopE.Add(new Line3d(erbp.Clone(), elbp.Clone()));
                    var normalE = new Vector3().CrossVectors(loopE[0].Dir, loopE[1].Dir);
                    var extrudeE = new Extrude3d(new PlanarSurface3d(new Plane(normalE), loopE.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrudeE.CreateMesh();
                    var geoE = new BufferGeometry();
                    geoE.setAttribute("position", new BufferAttribute(extrudeE.Geometry.Verteics.ToArray(), 3, false));
                    geoE.setIndex(new BufferAttribute(extrudeE.Geometry.Indics.ToArray(), 1));
                    geoE.computeVertexNormals();
                    geoE.SetUV();
                    geoE.rotateZ(angle);
                    geoE.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshE = new Mesh(geoE, mat);
                    objs.Add(meshE);
                }
                var connectpS = new Vector3(srbp.X, stP.Y + widthHalf + eaveWidth, srbp.Z);
                var connectpE = new Vector3(srbp.X, etP.Y - widthHalf - eaveWidth, srbp.Z);
                {
                    var loopR = new List<Line3d>() { };
                    loopR.Add(new Line3d(stP.Clone(), connectpS.Clone()));
                    loopR.Add(new Line3d(connectpS.Clone(), connectpE.Clone()));
                    loopR.Add(new Line3d(connectpE.Clone(), etP.Clone()));
                    loopR.Add(new Line3d(etP.Clone(), stP.Clone()));
                    var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshR = new Mesh(geo, mat);
                    objs.Add(meshR);
                }
                {
                    var loopR = new List<Line3d>() { };
                    loopR.Add(new Line3d(etP.Clone(), connectpE.Clone()));
                    loopR.Add(new Line3d(connectpE.Clone(), new Vector3(connectpE.X, etP.Y, etP.Z)));
                    loopR.Add(new Line3d(loopR.Last().End.Clone(), etP.Clone()));
                    var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshL = new Mesh(geo, mat);
                    objs.Add(meshL);
                }
                {
                    var loopR = new List<Line3d>() { };
                    loopR.Add(new Line3d(stP.Clone(), new Vector3(connectpS.X, stP.Y, stP.Z)));
                    loopR.Add(new Line3d(loopR.Last().End.Clone(), connectpS.Clone()));
                    loopR.Add(new Line3d(connectpS.Clone(), stP.Clone()));
                    var normal = new Vector3().CrossVectors(loopR[0].Dir, loopR[1].Dir);
                    var extrude = new Extrude3d(new PlanarSurface3d(new Plane(normal), loopR.ToList<Curve3d>()), new Vector3(0, 0, 1), roofThickness);
                    extrude.CreateMesh();
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.computeVertexNormals();
                    geo.SetUV();
                    geo.rotateZ(angle);
                    geo.translate(offset.X, offset.Y, plateBuild.Floors.Sum(n => n.FloorHeight));
                    var meshR = new Mesh(geo, mat);
                    objs.Add(meshR);
                }

            }
            return objs;
        }

        //创建楼梯
        public List<Object3D> CreatePlateStair(PlateBuildGroup group, PlateBuilding plateBuild ,double height,double width,double length,double passWidth,bool isStart)
        {
            //var location = group.Location;
            var location = new Vector2();
            var rotationAngle = plateBuild.BuildingDirect.Angle() - new Vector2(0, 1).Angle();
            var offset = new Vector2().AddVectors(location, plateBuild.BuildingLocation).AddScaledVector(new Vector2(1,0).RotateAround(new Vector2(),rotationAngle),plateBuild.RoomSizeLength-length+passWidth);
            //var offset = plateBuild.BuildingLocation.Clone().AddScaledVector(new Vector2(1, 0).RotateAround(new Vector2(), rotationAngle), plateBuild.RoomSizeLength - length + passWidth);
            if (!isStart)
            {
                offset = offset.AddScaledVector(new Vector2(0,1).RotateAround(new Vector2(), rotationAngle),plateBuild.RoomSizeWidth*plateBuild.Floors.First().RoomCount+ width);
            }
            var stepNum = 16;
            var stepThickness = 50;
            var ladderLength = length - passWidth;
            var stepWidth = ladderLength / (stepNum-1);
            var stepHeight = height / stepNum;
            var stepgeo = CreateLadderStep(stepWidth, width, stepThickness); 
            var objs = new List<Object3D>();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.CurtainUuid);
            var matStair = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.BoardUuid);
            for (var i = 0; i < stepNum-1; i++)
            {
                var geo = stepgeo.clone().translateQuick(i * stepWidth, 0, (i + 1) * stepHeight);
                geo.rotateZ(rotationAngle);
                geo.translate(offset.X, offset.Y, 0);
                var meshStair = new Mesh(geo, matStair);
                objs.Add(meshStair);
            }
            //var plat = CreateLadderStep(passWidth, width, stepThickness);
            //plat.translateQuick(ladderLength, 0, height);
            //plat.rotateZ(rotationAngle);
            //plat.translate(offset.X, offset.Y, 0);
            //var meshPlat = new Mesh(plat, matStair);
            //objs.Add(meshPlat);

            var shapePs = new ListEx<Vector2>();
            var line = new Line2d(new Vector2(-stepWidth,0), new Vector2(ladderLength, height));
            line.Translate(0, stepThickness);
            var tcp = Intersect2d.XLineWithXLine(line.Start, line.Dir, new Vector2(), new Vector2(1, 0));
            line.Start = tcp.Clone();
            {
                var line3d = new Line3d(new Vector3(line.Start.X, 0, line.Start.Y), new Vector3(line.End.X, 0, line.End.Y));
                line3d.RotateRoundAxis(new Vector3(),new Vector3(0,0,1), rotationAngle);
                var offsetL = new Vector2(0, -1).RotateAround(new Vector2(), rotationAngle).MultiplyScalar(isStart ? width : 0);
                line3d.Translate(offset.X + offsetL.X, offset.Y+ offsetL.Y, 0);
                for (var i = 1; i <= 4; i++)
                {
                    var tube = new Tube3d();
                    tube.Radius = i == 4 ? 30 : 15;
                    tube.Path = new List<Curve3d>() { line3d };
                    tube.CreateMesh();
                    var tubegeo = new BufferGeometry();
                    tubegeo.setAttribute("position", new BufferAttribute(tube.Geometry.Verteics.ToArray(), 3, false));
                    tubegeo.setIndex(new BufferAttribute(tube.Geometry.Indics.ToArray(), 1));
                    tubegeo.computeVertexNormals();
                    tubegeo.SetUV();
                    tubegeo.translate(0,0, i * 250);
                    var mesh = new Mesh(tubegeo, mat);
                    objs.Add(mesh);

                    var normal = line3d.Dir.ToVector2().Normalize();
                    var sp = line3d.End.Clone();
                    sp.Z += i * 250 - 50;
                    var ep = sp.Clone().Add((normal * passWidth).ToVector3());
                    var tube1 = new Tube3d();
                    tube1.Radius = i == 4 ? 30 : 15;
                    tube1.Path = new List<Curve3d>() { new Line3d() { Start = sp, End = ep } };
                    tube1.CreateMesh();
                    var tubegeo1 = new BufferGeometry();
                    tubegeo1.setAttribute("position", new BufferAttribute(tube1.Geometry.Verteics.ToArray(), 3, false));
                    tubegeo1.setIndex(new BufferAttribute(tube1.Geometry.Indics.ToArray(), 1));
                    tubegeo1.computeVertexNormals();
                    tubegeo1.SetUV();
                    var meshtube = new Mesh(tubegeo1, mat);
                    objs.Add(meshtube);

                    var nextP = ep.Clone().Add(normal.Clone().RotateAround(new Vector2(), isStart ? Math.PI / 2 : -Math.PI / 2).ToVector3() * width);
                    var tube2 = new Tube3d();
                    tube2.Radius = i == 4 ? 30 : 15;
                    tube2.Path = new List<Curve3d>() { new Line3d() { Start = ep.Clone(), End = nextP.Clone() } };
                    tube2.CreateMesh();
                    var tube2geo = new BufferGeometry();
                    tube2geo.setAttribute("position", new BufferAttribute(tube2.Geometry.Verteics.ToArray(), 3, false));
                    tube2geo.setIndex(new BufferAttribute(tube2.Geometry.Indics.ToArray(), 1));
                    tube2geo.computeVertexNormals();
                    tube2geo.SetUV();
                    var meshTube2 = new Mesh(tube2geo, mat);
                    objs.Add(meshTube2);
                }
                var count = Math.Ceiling(line.Length / 1000);
                for (var i=0;i<count;i++)
                {
                    var profile = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), new List<Curve3d>() { new Arc3d() { Center = new Vector3(), Radius = i==0?30:15, StartAngle = 0, EndAngle = Math.PI }, new Arc3d() { Center = new Vector3(), Radius = i == 0 ? 30 : 15, StartAngle = Math.PI, EndAngle = Math.PI * 2 } });
                    var extrude = new Extrude3d(profile, new Vector3(0, 0, 1), i == 0?1020:1000);
                    extrude.CreateMesh();
                    var position = line3d.Start.Clone().AddScaledVector(line3d.Dir, line3d.Distance()/count*i);
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.setAttribute("uv", new BufferAttribute(extrude.Geometry.Uvs, 2, false));
                    geo.setAttribute("normal", new BufferAttribute(extrude.Geometry.Normals, 3, false));
                    geo.translate(position.X, position.Y, position.Z);
                    var mesh = new Mesh(geo, mat);
                    objs.Add(mesh);
                }
                var support = new Extrude3d(new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), new List<Curve3d>() { new Arc3d() { Center = new Vector3(), Radius = 50, StartAngle = 0, EndAngle = Math.PI }, new Arc3d() { Center = new Vector3(), Radius = 50 , StartAngle = Math.PI, EndAngle = Math.PI * 2 } }), new Vector3(0, 0, 1),   1080+plateBuild.Floors.First().FloorHeight);
                support.CreateMesh();
                var geosupport = new BufferGeometry();
                geosupport.setAttribute("position", new BufferAttribute(support.Geometry.Verteics.ToArray(), 3, false));
                geosupport.setIndex(new BufferAttribute(support.Geometry.Indics.ToArray(), 1));
                geosupport.computeVertexNormals();
                geosupport.SetUV();
                geosupport.translate(line3d.End.X, line3d.End.Y, 0);
                var meshgeosupport = new Mesh(geosupport, mat);
                objs.Add(meshgeosupport);
                var geosupport2 = geosupport.clone();
                geosupport2.computeVertexNormals();
                geosupport2.SetUV();
                geosupport2.translate(new Vector3(1,0, 0).RotateRoundAxis(new Vector3(), new Vector3(0,0,1), rotationAngle).MulScalar(passWidth));
                var meshgeosupport2 = new Mesh(geosupport2, mat);
                objs.Add(meshgeosupport2);
                //var geosupport3 = geosupport2.clone();
                //geosupport3.computeVertexNormals();
                //geosupport3.SetUV();
                //geosupport3.translate((isStart?new Vector3(0, 1, 0): new Vector3(0, -1, 0)).RotateRoundAxis(new Vector3(), new Vector3(0, 0, 1), rotationAngle).MulScalar(width-25));
                //var meshgeosupport3 = new Mesh(geosupport3, mat);
                //objs.Add(meshgeosupport3);
            }
            var btmL = line.Clone().Translate(0,-stepHeight- stepThickness) as Line2d;
            var cp = Intersect2d.XLineWithXLine(btmL.Start,btmL.Dir, new Vector2(),new Vector2(1,0));
            btmL.Start = cp;
            btmL.Reverse();
            shapePs.Add(line.Start);
            shapePs.Add(line.End);
            shapePs.Add(btmL.Start);
            shapePs.Add(btmL.End);
            if (ShapeUtils.isClockWise(shapePs))
            {
                shapePs.Reverse();
            }
            var shape = new ThreeJs4Net.Shape(shapePs);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0,0,1), new Vector3(0, -1, 0));
            var ladderGeo = GeoModelUtil.GetStretchGeometryData(shape, coodMat, 0, -stepThickness).GetBufferGeometry();
            ladderGeo.computeVertexNormals();
            ladderGeo.SetUV();
            ladderGeo.rotateZ(rotationAngle);
            ladderGeo.translate(offset.X, offset.Y, 0);

            if (isStart)
            {
                var inneroffsetL = new Vector2(0, -1).RotateAround(new Vector2(), rotationAngle).MultiplyScalar(stepThickness*1.5);
                var innerLadderGeo = ladderGeo.clone();
                innerLadderGeo.computeVertexNormals();
                innerLadderGeo.SetUV();
                innerLadderGeo.translate(inneroffsetL.X,inneroffsetL.Y,0);
                var meshinl = new Mesh(innerLadderGeo, matStair);
                objs.Add(meshinl);

                var offsetL =  new Vector2(0,-1).RotateAround(new Vector2(), rotationAngle).MultiplyScalar(width);
                ladderGeo.translate(offsetL.X,offsetL.Y,0);
            }
            else
            {
                var inneroffsetL = new Vector2(0, -1).RotateAround(new Vector2(), rotationAngle).MultiplyScalar(width- stepThickness / 2);
                var innerLadderGeo = ladderGeo.clone();
                innerLadderGeo.computeVertexNormals();
                innerLadderGeo.SetUV();
                innerLadderGeo.translate(inneroffsetL.X, inneroffsetL.Y, 0);
                var meshinl = new Mesh(innerLadderGeo, matStair);
                objs.Add(meshinl);

                var offsetL = new Vector2(0, -1).RotateAround(new Vector2(), rotationAngle).MultiplyScalar(stepThickness);
                ladderGeo.translate(offsetL.X, offsetL.Y, 0);
            }
            var meshladder = new Mesh(ladderGeo, matStair);
            objs.Add(meshladder); 
            return objs;
        }
        //创建梯段
        private ThreeJs4Net.BufferGeometry CreateLadderStep(double stepWidth, double ladderLen, double stepThickness)
        {
            var poly = new ListEx<Vector2>();
            poly.Push(new Vector2(0, 0));
            poly.Push(new Vector2(ladderLen, 0));
            poly.Push(new Vector2(ladderLen, stepWidth));
            poly.Push(new Vector2(0, stepWidth));
            var shape = new ThreeJs4Net.Shape(poly);
            var coodMat = new Matrix4();
            coodMat.MakeBasis(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            var geo = GeoModelUtil.GetStretchGeometryData(shape, coodMat, stepThickness, 0).GetBufferGeometry();
            geo.name = "Step";
            geo.rotateZ(-Math.PI/2);
            geo.computeVertexNormals();
            geo.SetUV();
            return geo;
        }
        //创建栏杆扶手
        public List<Object3D> CreateRailing(PlateBuilding plateBuild,PlateBuildingFloor floor/*,Vector2 location*/ )
        {
            Vector2 location = new Vector2();
            var mat = LightCAD.RenderUtils.RenderMaterialManager.GetRenderMaterial(MaterialManager.CurtainUuid);
            var objs = new List<Object3D>();
            var profile = new PlanarSurface3d(new Plane(new Vector3(0, 0, 1)), new List<Curve3d>() { new Arc3d() { Center = new Vector3(), Radius = 15, StartAngle = 0, EndAngle = Math.PI }, new Arc3d() { Center = new Vector3(), Radius = 15, StartAngle = Math.PI, EndAngle = Math.PI*2 } } );
            var extrude = new Extrude3d(profile, new Vector3(0, 0, 1), 1000);
            extrude.CreateMesh();
            foreach (var pass in floor.PassagewayKs)
            {
                if (pass.PassagewayKType != PlatePassagewayKType.Fence)
                    continue;
                var line = pass.Line;
                for (var count=0;count <= Math.Ceiling(line.Length / 1000); count++)
                {
                    var position = line.Start.Clone().AddScaledVector(line.Dir,1000*count);
                    var geo = new BufferGeometry();
                    geo.setAttribute("position", new BufferAttribute(extrude.Geometry.Verteics.ToArray(), 3, false));
                    geo.setIndex(new BufferAttribute(extrude.Geometry.Indics.ToArray(), 1));
                    geo.setAttribute("uv", new BufferAttribute(extrude.Geometry.Uvs, 2, false));
                    geo.setAttribute("normal", new BufferAttribute(extrude.Geometry.Normals, 3, false));
                    geo.translate(location.X+position.X, location.Y+ position.Y, plateBuild.Floors.Sum(n=>n.FloorNum<floor.FloorNum?n.FloorHeight:0));
                    var mesh = new Mesh(geo, mat);
                    objs.Add(mesh);
                }
                for (var i=1;i<=4; i++)
                {
                    var tube = new Tube3d();
                    tube.Radius = i == 4?30:15;
                    tube.Path = new List<Curve3d>() { line.ToCurve3d() };
                    tube.CreateMesh();
                    var tubegeo = new BufferGeometry();
                    tubegeo.setAttribute("position", new BufferAttribute(tube.Geometry.Verteics.ToArray(), 3, false));
                    tubegeo.setIndex(new BufferAttribute(tube.Geometry.Indics.ToArray(), 1));
                    tubegeo.computeVertexNormals();
                    tubegeo.SetUV();
                    tubegeo.translate(location.X, location.Y, plateBuild.Floors.Sum(n=>n.FloorNum<floor.FloorNum?n.FloorHeight:0)+i*250);
                    var mesh = new Mesh(tubegeo, mat);
                    objs.Add(mesh);
                }
               
            }
            return objs;
        }
    }
}
