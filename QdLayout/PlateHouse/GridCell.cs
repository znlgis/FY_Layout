using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public enum GridCellType
    {
        Passageway,//走廊
        Plate,//板房
        Stairs//楼梯

    }

    public class GridCell : IConvertDictionary
    {
        ///板房config  板房标准尺寸 （标准房宽度 板厚 进深 几块板 每块板的宽度 之类 走道宽度）  外轮廓形状，楼梯，走廊，给房见数量和面积
        /// <summary>
        /// 每种类型格子的最小尺寸
        /// </summary>
        public static Dictionary<GridCellType, Vector2> GridCellTypeMinSize= new Dictionary<GridCellType, Vector2>()
         {
        { GridCellType.Passageway,new Vector2(1000,1000)},
        { GridCellType.Plate,new Vector2(1500,1500) },
        { GridCellType.Stairs,new Vector2(1000,1000) }
          };
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id;
        public Vector2 Center;
        public int width;
        public int height;
        public GridCellType gridCellType;
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.Id), this.Id);
            dict.Add(nameof(this.Center), this.Center);
            dict.Add(nameof(this.width), this.width);
            dict.Add(nameof(this.height), this.height);
            dict.Add(nameof(this.gridCellType), this.gridCellType);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.Id = props.GetInt(nameof(this.Id));
            this.Center = props.GetVector2(nameof(Center));
            this.width = props.GetInt(nameof(this.width));
            this.height = props.GetInt(nameof(this.height));
            this.gridCellType = (GridCellType)props.GetInt(nameof(this.gridCellType));
        }
        public Polygon2d polygon2D
        {
            get
            {
                Polygon2d polygon2D = new Polygon2d();
                Vector2[] points = new Vector2[]{
                new Vector2(Center.X-width/2, Center.Y-height/2),
                    new Vector2(Center.X+width/2, Center.Y-height/2),
                    new Vector2(Center.X+width/2, Center.Y+height/2),
                    new Vector2(Center.X-width/2, Center.Y+height/2),
                };
                polygon2D.Points = points;
                return polygon2D;
            }
        }
        public GridCell()
        {

        }
        public GridCell(Vector2 center, int width, int height)
        {
            this.Center = center;
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// 合并格子
        /// </summary>
        /// <param name="gridCells"></param>
        /// <returns></returns>
        public static List<GridCell> CombineGridCell(List<GridCell> gridCells)
        {
            return null;
        }
        /// <summary>
        /// 获取每种类型最小格子
        /// </summary>
        /// <param name="gridCells"></param>
        /// <returns></returns>
        public List<GridCell> GetMinGridCell()
        {
            return null;
        }
        public static List<GridCell> GetMinGridCell(Polygon2d polygon2D, GridCellType gridCellType)
        {
            List<GridCell> gridCells=new List<GridCell>();
            Vector2 XYscale= GridCellTypeMinSize.Where(X=>X.Key== gridCellType)?.FirstOrDefault().Value;
            if (polygon2D.Points.Count() != 4)
            {
                return null;
            }
            else
            {
               int xnum= (int)((polygon2D.Points[1].X- polygon2D.Points[0].X)/ XYscale.X);
               int Ynum = (int)((polygon2D.Points[3].Y - polygon2D.Points[0].Y) / XYscale.Y);
                for (int i = 0; i < xnum; i++)
                {
                    for (int j = 0; j < Ynum; j++)
                    {
                        Vector2 center = new Vector2(polygon2D.Points[0].X + i * XYscale.X+ XYscale.X/2, polygon2D.Points[0].Y + j * XYscale.Y + XYscale.Y / 2);
                        GridCell gridCell = new GridCell(center, (int)XYscale.X, (int)XYscale.Y);
                        gridCells.Add(gridCell);
                    }
                }
            }
            return gridCells;
        }
    }
}
