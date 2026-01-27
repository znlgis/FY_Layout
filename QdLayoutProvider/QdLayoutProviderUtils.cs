using LightCAD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayoutProvider
{
    internal static class QdLayoutProviderUtils
    {
        internal static string UseType = "设计";
        internal static Func<string, ImageResource> GetThumbnail = null;
        internal static Func<string, ImageResource> GetThumbnail3 = null;
        internal static void ConvertToProviders(List<(string uuid, string name, CreateShape creator)> createshapes)
        {
            var shapeProviders = QdLayoutProvider.QdLayoutDllProviderImporter.ShapeProviders;
            foreach (var kv in createshapes)
            {
                if (!shapeProviders.Contains(kv.uuid))
                {
                    var provider = new ShapeProvider { Uuid = kv.uuid, Name = kv.name, UseType = UseType, CreateShape = kv.creator };
                    provider.Thumbnail = GetThumbnail?.Invoke(kv.name);
                    shapeProviders.Add(provider);
                }
            }
        }
        internal static void ConvertToProvider(string uuid, string name, CreateSolid createSolid, GetSolidMaterial getSolidMat)
        {
            var solidProviders = QdLayoutProvider.QdLayoutDllProviderImporter.SolidProviders;
            if (solidProviders.Contains(uuid))
                return;
            var provider = new SolidProvider { Uuid = uuid, Name = name, CreateSolid = createSolid, GetSolidMaterial = getSolidMat };
            provider.Thumbnail= GetThumbnail3?.Invoke(uuid);
            solidProviders.Add(provider);
        }
    }
}
