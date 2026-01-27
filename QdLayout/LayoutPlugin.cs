using LightCAD.MathLib.Csg;
using QdLayout.Properties;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static OpenTK.Graphics.OpenGL.GL;

namespace QdLayout
{
    public class LayoutPlugin : ILcPlugin
    {
        public static TabItem LayoutItem = new TabItem
        {
            Name = "LayoutMajor",
            Text = "场布",
            ShortcutKey = "ALT-L",
            ButtonGroups = new List<TabButtonGroup>
            {
                new TabButtonGroup 
                {
                    Buttons = new List<TabButton>
                    {
                        new TabButton
                        {
                            Name = "CreateProject",
                            Text = "创建项目",
                            Icon = Properties.Resources.创建项目,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name="Lawn",
                            Text="草坪",
                            Icon= Properties.Resources.草地,
                            IsCommand=true,
                            DropDowns = new List<TabButton>
                            {
                               new TabButton
                               {
                                   Name="Lawn",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.草地,
                                   IsCommand=true,
                               },
                               new TabButton
                               {
                                   Name="LawnRec",
                                   Text="矩形绘制",
                                   Icon= Properties.Resources.草地,
                                   IsCommand=true,
                               },
                               new TabButton
                               {
                                   Name = "LawnChange",
                                   Text = "转换多段线",
                                   Icon = Properties.Resources.草地,
                                   IsCommand = true,
                               },
                            }
                        },
                        new TabButton
                        {
                            Name = "Fence",
                            Text = "围栏",
                            Icon = Properties.Resources.围墙,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name = "PlanBuild",
                            Text = "拟建建筑",
                            Icon = Properties.Resources.拟建建筑,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name = "FoundationPit",
                            Text = "基坑",
                            Icon = Properties.Resources.FoundationPit,
                            IsCommand = true,
                            DropDowns = new List<TabButton>
                            {
                                new TabButton
                                {
                                   Name="FoundationPit",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.FoundationPit,
                                   IsCommand=true,
                                },
                                new TabButton
                                {
                                  Name="FoundationPitRec",
                                  Text="矩形绘制",
                                  Icon= Properties.Resources.FoundationPit,
                                  IsCommand=true,
                                },
                                new TabButton
                                {
                                   Name="FoundationPitChange",
                                   Text="转换多段线",
                                   Icon= Properties.Resources.FoundationPit,
                                   IsCommand=true,
                                },
                            }
                        },
                        new TabButton
                        {
                            Name = "Ground",
                            Text = "硬化地面",
                            Icon = Properties.Resources.Road,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name = "Earthwork",
                            Text = "土方回填",
                            Icon = Properties.Resources.土方回填,
                            IsCommand = true,
                            DropDowns = new List<TabButton>
                            {
                                new TabButton{
                                   Name="Earthwork",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.土方回填,
                                   IsCommand=true,
                                },
                                new TabButton{
                                  Name="EarthworkRec",
                                  Text="矩形绘制",
                                  Icon= Properties.Resources.土方回填,
                                  IsCommand=true,
                                },
                                new TabButton{
                                   Name="EarthworkChange",
                                   Text="转换多段线",
                                   Icon= Properties.Resources.土方回填,
                                   IsCommand=true,
                                },
                            }
                        },
                        new TabButton
                        {
                            Name = "Berm",
                            Text = "出土道路",
                            Icon = Properties.Resources.道路,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name = "Barrier",
                            Text = "防护栏杆",
                            Icon = Properties.Resources.防护栏杆,
                            IsCommand = true,
                        },
                        new TabButton
                        {
                            Name = "Road",
                            Text = "城市道路",
                            Icon = Properties.Resources.Harden,
                            IsCommand = true,
                            Width = 78,
                            DropDowns = new List<TabButton>
                            {
                                new TabButton{
                                   Name="Ground",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.Harden,
                                   IsCommand=true,
                                },
                                new TabButton{
                                  Name="GroundRec",
                                  Text="矩形绘制",
                                  Icon= Properties.Resources.Harden,
                                  IsCommand=true,
                                },
                                new TabButton{
                                   Name="GroundChange",
                                   Text="转换多段线",
                                   Icon= Properties.Resources.Harden,
                                   IsCommand=true,
                                },
                            }
                        },
                        new TabButton
                        {
                            Name = "Site",
                            Text = "场地",
                            Icon = Properties.Resources.Site1,
                            IsCommand = true,
                            DropDowns = new List<TabButton>
                            {
                                new TabButton{
                                   Name="Site",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.Site1,
                                   IsCommand=true,
                                },
                                new TabButton{
                                  Name="SiteRec",
                                  Text="矩形绘制",
                                  Icon= Properties.Resources.Site1,
                                  IsCommand=true,
                                },
                                new TabButton{
                                   Name="SiteChange",
                                   Text="转换多段线",
                                   Icon= Properties.Resources.Site1,
                                   IsCommand=true,
                                },
                            }
                        },
                        new TabButton
                        {
                            Name = "PropertyLine",
                            Text = "用地红线",
                            Icon = Properties.Resources.用地红线,
                            IsCommand = true,
                            Width = 78,
                            DropDowns = new List<TabButton>
                            {
                                new TabButton{
                                   Name="PropertyLine",
                                   Text="任意绘制",
                                   Icon= Properties.Resources.用地红线,
                                   IsCommand=true,
                                },
                                new TabButton{
                                  Name="PropertyLineRec",
                                  Text="矩形绘制",
                                  Icon= Properties.Resources.用地红线,
                                  IsCommand=true,
                                },
                                new TabButton{
                                   Name="PropertyLineChange",
                                   Text="转换多段线",
                                   Icon= Properties.Resources.用地红线,
                                   IsCommand=true,
                                },
                            }
                        },
                        new TabButton
                        {
                            Name = "SelRedlinesForArrange",
                            Text = "选择模板生成方案",
                            Icon = Properties.Resources.排布方案,
                            IsCommand = true,
                            Width = 115,
                        },
                        new TabButton
                        {
                            Name = "PlateBuilding",
                            Text = "编辑板房布置",
                            Icon = Properties.Resources.板房设置,
                            IsCommand = true,
                            Width = 95,
                        },
                        new TabButton
                        {
                            Name = "PersonnelRoomArrange",
                            Text = "人员及房间布置",
                            Icon = Properties.Resources.人员设置,
                            IsCommand = true,
                            Width = 95,
                        },
                    }
                }
            }   
        };
        //public static TabItem LayoutItem = new TabItem
        //{
        //    Name = "LayoutMajor",
        //    Text = "场布",
        //    ShortcutKey = "ALT-L",
        //    ButtonGroups = new List<TabButtonGroup>
        //    {


        


        //    }
        //};

        public void InitUI()
        {

        }

        public void Loaded()
        {
            LcDocument.RegistElementTypes(LayoutElementType.All);
            LcRuntime.RegistAssemblies.Add("QdLayout");
            LcDocument.ElementActions.Add(LayoutElementType.PlateBuilding, new PlateBuildingAction());
            LcDocument.ElementActions.Add(LayoutElementType.PlateBuildGroup, new PlateBuildGroupAction());
            LcDocument.Element3dActions.Add(LayoutElementType.PlateBuildGroup, new PlateBuild3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Lawn, new LawnAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Lawn, new Lawn3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Fence, new FenceAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Fence, new Fence3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.PlanBuild, new PlanBuildAction());
            LcDocument.Element3dActions.Add(LayoutElementType.PlanBuild, new PlanBuild3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.FoundationPit, new FoundationPitAction());
            LcDocument.Element3dActions.Add(LayoutElementType.FoundationPit, new FoundationPit3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Ground, new GroundAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Ground, new Ground3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Road, new RoadAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Road, new Road3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Earthwork, new EarthworkAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Earthwork, new Earthwork3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Berm, new BermAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Berm, new Berm3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Barrier, new BarrierAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Barrier, new Barrier3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Harden, new HardenAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Harden, new Harden3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.Site, new SiteAction());
            LcDocument.Element3dActions.Add(LayoutElementType.Site, new Site3dAction());
            LcDocument.ElementActions.Add(LayoutElementType.PropertyLine, new PropertyLineAction());
            LcDocument.ElementActions.Add(LayoutElementType.OpenLine, new OpenLineAction());
            LcDocument.ElementActions.Add(LayoutElementType.LayoutEquipment, new LayoutEquipmentAction());
            LcDocument.Element3dActions.Add(LayoutElementType.LayoutEquipment, new LayoutEquipment3dAction());


            InitUI();
            PlateRoomConfigManager.Init();
        }

        public void Completed()
        {
            //var nameSpace = LcCategoryManager.NameSpaces;
            //var cates = LcCategoryManager.AllCategories;
            //var tabButtons = new List<TabButton>()
            //{
            //    new TabButton()
            //    {
            //        Name = "常用构件",
            //        Text = "常用构件",
            //        IsCommand = true,
            //        Width = 78,
            //        Icon = Resources.L常用构件,
            //    }
            //};
            //foreach (var c in cates)
            //{
            //    if (c.Name == "__默认__")
            //    {
            //        continue;
            //    }
            //    var tabButton = new TabButton()
            //    {
            //        Name = c.Name,
            //        Text = c.Name,
            //        Width = 78,
            //    };

            //    object obj = Resources.ResourceManager.GetObject("L"+c.Name);
            //    var icon = ((System.Drawing.Bitmap)(obj));
            //    tabButton.Icon = icon;
            //    var category = LcCategoryManager.AllCategories.FirstOrDefault(x => x.Name == c.Name);
            //    if (category == null)
            //        continue;

            //    if (category.Name == "安全防护") //830演示版本暂时关掉
            //        continue;

            //    var defs = ComponentDefinitionManager.Get(category);
            //    defs = defs?.Where(def => def.Solid3dProviders.Any(s => s?.SourceType == GeometrySouceType.System || s?.SourceType == GeometrySouceType.ExternalDll));
            //    if ((defs?.Count() ?? 0) == 0)
            //        continue;

            //    for (int k = 0; k < defs.Count(); k++)
            //    {
            //        var def = defs.ToList()[k];
            //        var but = GetTabButton(def);
            //        tabButton.DropDowns.Add(but);
            //    }

            //    tabButtons.Add(tabButton);
            //}
            //tabButtons.Add(new TabButton
            //{
            //    Name = "PropertyLine",
            //    Text = "用地红线",
            //    Icon = Properties.Resources.L用地红线,
            //    IsCommand = true,
            //    Width = 78,
            //    DropDowns = new List<TabButton>
            //                    {
            //                        new TabButton{
            //                           Name="PropertyLine",
            //                           Text="任意绘制",
            //                           Icon= Properties.Resources.L用地红线,
            //                           IsCommand=true,
            //                        },
            //                        new TabButton{
            //                          Name="PropertyLineRec",
            //                          Text="矩形绘制",
            //                          Icon= Properties.Resources.L用地红线,
            //                          IsCommand=true,
            //                        },
            //                        new TabButton{
            //                           Name="PropertyLineChange",
            //                           Text="转换多段线",
            //                           Icon= Properties.Resources.L用地红线,
            //                           IsCommand=true,
            //                        },
            //                    }
            //});
            //LayoutItem.ButtonGroups.Add(new TabButtonGroup
            //{
            //    Buttons = tabButtons
            //});
            AppRuntime.UISystem.AddInitTabItems([LayoutItem]);
        }

        public TabButton GetTabButton(LcComponentDefinition comDef)
        {
            var cmds = (comDef.Commands ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries);
            string cmdName = "";
            if (cmds.Length > 0)
            {
                cmdName = cmds[0];
            }
            else
            {
                cmdName = "LayoutItem";//放置场布元素
            }
            var but = new TabButton
            {
                Name = cmdName,
                Text = comDef.Name,
                IsCommand = true,
                Icon = comDef?.Thumbnail?.Image?.GetThumbnailImage(32, 32, null, 0),
            };
            if (cmds.Length > 1)
            {
                var ns = new NameValueString(cmds[1]);
                foreach (string key in ns)
                {
                    var value = ns[key];
                    var subBut = new TabButton
                    {
                        Name = key,
                        Text = value,
                        IsCommand = true,
                    };
                    but.DropDowns.Add(subBut);
                }
            }
            return but;
        }

        public void OnInitializeDocRt(DocumentRuntime docRt)
        {
        }
        public void OnDisposeDocRt(DocumentRuntime docRt)
        {
        }
    }

    public static class QdLayoutCategorySettings
    {
        public static string NamespaceKey = "场布施工设计";

        public static string UseType = "设计";
        public static void CnovertFromtype(ref FromType fromtype, out string convertLabel)
        {
            if (fromtype == FromType.Library)
            {
                fromtype = FromType.Document;
            }
            else
            {
                fromtype = FromType.Library;
            }
            convertLabel = FromTypeMessage(fromtype);
        }
        public static string FromTypeMessage(FromType fromtype)
        {
            string convertLabel = null;
            if (fromtype == FromType.Library)
            {
                convertLabel = "转到文档中";
            }
            else
            {
                convertLabel = "转到构件库中";
            }
            return convertLabel;
        }
    }
}
