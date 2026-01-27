using LightCAD.Core;
using QdArchProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdLayoutProvider
{
    public class QdLayoutDllProviderImporter : IDllProviderImporter
    {
        internal static ShapeProviderCollection ShapeProviders = new ShapeProviderCollection();
        internal static SolidProviderCollection SolidProviders = new SolidProviderCollection();
        public (ShapeProviderCollection shapeProviders, SolidProviderCollection solidProviders) GetImportProviders()
        {
            QdFenceProvider.RegistProviders();
            QdLawnProvider.RegistProviders();
            QdFoundationPitProvider.RegistProviders();
            QdGroundProvider.RegistProviders();
            QdEarthworkProvider.RegistProviders();
            QdBermProvider.RegistProviders();
            QdHardenProvider.RegistProviders();
            QdSiteProvider.RegistProviders();
            QdIntersectionProvider.RegistProviders();
            return (ShapeProviders, SolidProviders);
        }
    }
}
