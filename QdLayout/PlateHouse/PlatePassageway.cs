using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public class PlatePassageway : IConvertDictionary
    {

        public Polygon2d PassagewayOutline;
        public List<GridCell> GridCells;
        public PlatePassageway()
        {

        }
        public PlatePassageway(Polygon2d PassagewayOutline)
        {
            this.PassagewayOutline = PassagewayOutline;
            PassagewayCellInit();
        }
        public void PassagewayCellInit()
        {
            this.GridCells = GridCell.GetMinGridCell(this.PassagewayOutline, GridCellType.Passageway);
        }
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.PassagewayOutline), this.PassagewayOutline);
            dict.Add(nameof(this.GridCells), this.GridCells);

            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.PassagewayOutline = props.GetCurve2d(nameof(this.PassagewayOutline)) as Polygon2d;
            this.GridCells = new List<GridCell>();
            if (props.ContainsKey(nameof(this.GridCells)))
            {
                var thisGridCells = props.GetValue<List<PropertyObject>>(nameof(this.GridCells));
                if (thisGridCells != null)
                    foreach (var obj in thisGridCells)
                    {
                        var GridCell = new GridCell();

                        GridCell.FromDictionary(obj.Value as Dictionary<string, PropertyObject>);
                        this.GridCells.Add(GridCell);
                    }
            }
        }
    }
}
