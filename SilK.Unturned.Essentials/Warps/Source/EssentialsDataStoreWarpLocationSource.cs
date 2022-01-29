using SilK.Unturned.Essentials.API.Directories.Sources;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Directories.Sources.DataStores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Warps.Source
{
    public class EssentialsDataStoreWarpLocationSource : DataStoreSource<WarpLocation, WarpLocationData, string>,
        IWarpLocationSource
    {
        public EssentialsDataStoreWarpLocationSource(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IReadOnlyCollection<IWarpLocation> WarpLocations => Contents;

        protected override string DataStoreKey => "warps";

        protected override bool ElementMatchesData(WarpLocation element, WarpLocationData elementData)
        {
            return element.WarpLocationData.Equals(elementData);
        }

        protected override bool IdentifiersMatch(string id1, string id2)
        {
            return id1.Equals(id2, StringComparison.OrdinalIgnoreCase);
        }

        protected override string GetId(WarpLocationData elementData)
        {
            return elementData.Name;
        }

        async Task<IReadOnlyCollection<IWarpLocation>> IElementSource<IWarpLocation>.GetContentsAsync() =>
            await GetContentsAsync();
    }
}
