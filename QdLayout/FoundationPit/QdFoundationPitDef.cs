using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayout
{
    [ComDefCategory($"场布施工设计.土方基坑")]
    [ComClass("基坑")]
    public class QdFoundationPitDef : LcComponentDefinition
    {
        public QdFoundationPitDef() : base()
        {
            this.Features = "场地布置";
        }
    }
}
