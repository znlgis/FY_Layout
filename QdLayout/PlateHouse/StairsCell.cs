using LightCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class PlateStair : IConvertDictionary
    {
        public double StairSizeWidth;
        public double StairSizeLength;

        public Polygon2d StairsCellOutline;

        public List<GridCell> GridCells;
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.StairSizeWidth), this.StairSizeWidth);
            dict.Add(nameof(this.StairSizeLength), this.StairSizeLength);
            dict.Add(nameof(this.StairsCellOutline), this.StairsCellOutline);
            dict.Add(nameof(this.GridCells), this.GridCells);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.StairSizeWidth = props.GetDouble(nameof(this.StairSizeWidth));
            this.StairSizeLength = props.GetDouble(nameof(this.StairSizeLength));
            this.StairsCellOutline = props.GetCurve2d(nameof(this.StairsCellOutline)) as Polygon2d;
            this.GridCells = new List<GridCell>();
            if (props.ContainsKey(nameof(this.GridCells)))
            {
                var thisGridCells = props.GetValue<List<PropertyObject>>(nameof(this.GridCells));
                if (thisGridCells != null)
                {
                    foreach (var obj in thisGridCells)
                    {
                        var GridCell = new GridCell();
                        GridCell.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.GridCells.Add(GridCell);
                    }
                }
            }
        }
        public void StairsCellInit()
        {

            //this.GridCells = GridCell.GetMinGridCell(this.StairsCellOutline, GridCellType.Stairs);
        }
        public Vector2 GetCenter()
        {




            Vector2 p1 = StairsCellOutline.Points[0].Clone();
            Vector2 p2 = StairsCellOutline.Points[2].Clone();


            return (p1 + p2) / 2;
            //this.GridCellIds = GridCell.GetMinGridCell(this.Outline, GridCellType.Plate);
        }
        public PlateStair()
        {
        }
        public PlateStair(double StairSizeWidth, double StairSizeLength, Polygon2d StairsCellOutline)
        {
            this.StairSizeWidth = StairSizeWidth;
            this.StairSizeLength = StairSizeLength;
            this.StairsCellOutline = StairsCellOutline;
            // StairsCellInit();
        }

        //public ListEx<EmbedAssociation> EmbedAssociations => throw new NotImplementedException();

        //public Profile2[] GetEmbedHoles()
        //{
        //    throw new NotImplementedException();
        //}

        //public LcElement Stretch(Box2 box, Vector2 vector)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
