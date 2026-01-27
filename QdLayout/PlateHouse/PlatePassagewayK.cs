using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    public enum PlatePassagewayKType
    {
        //围栏
        Fence,
        //幕墙
        Wall,
        //只有底板
        None,
    }
    public class PlatePassagewayK : IConvertDictionary
    {

        public Line2d Line;
        public double Weight;
        public double Height;
        public PlatePassagewayKType PassagewayKType;
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.Line), this.Line);
            dict.Add(nameof(this.Weight), this.Weight);
            dict.Add(nameof(this.Height), this.Height);
            dict.Add(nameof(this.PassagewayKType), this.PassagewayKType);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.Line = props.GetCurve2d(nameof(this.Line)) as Line2d;
            this.Weight = props.GetDouble(nameof(this.Weight));
            this.Height = props.GetDouble(nameof(this.Height));
            this.PassagewayKType = (PlatePassagewayKType)props.GetInt(nameof(this.PassagewayKType));
        }

    }
}
