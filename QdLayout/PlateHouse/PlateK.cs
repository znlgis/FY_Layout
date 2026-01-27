using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.InfoTip;

namespace QdLayout
{
    public class PlateK : IConvertDictionary
    {
   
        public Line2d Line;
        public double Weight;
        public double Height;
        public Vector2 Door;
        public double DoorOffset;
        public Vector2 Window;
        public double WindowOffset;
        public PlateK()
        { 
        }
        public Dictionary<string, PropertyObject> ToDictionary(Dictionary<string, PropertyObject> dict)
        {
            if (dict == null)
                dict = new Dictionary<string, PropertyObject>();
            dict.Add(nameof(this.Line), this.Line);
            dict.Add(nameof(this.Weight), this.Weight);
            dict.Add(nameof(this.Height), this.Height);
            dict.Add(nameof(this.Door), this.Door);
            dict.Add(nameof(this.DoorOffset), this.DoorOffset);
            dict.Add(nameof(this.Window), this.Window);
            dict.Add(nameof(this.WindowOffset), this.WindowOffset);
            return dict;
        }

        public void FromDictionary(Dictionary<string, PropertyObject> props)
        {
            this.Line = props.GetCurve2d(nameof(this.Line)) as Line2d;
            this.Weight = props.GetDouble(nameof(this.Weight));
            this.Height = props.GetDouble(nameof(this.Height));
            this.Door = props.GetVector2(nameof(Door));
            this.DoorOffset = props.GetDouble(nameof(this.DoorOffset));
            this.Window = props.GetVector2(nameof(Window));
            this.WindowOffset = props.GetDouble(nameof(this.WindowOffset));
        }
        

    }
}
