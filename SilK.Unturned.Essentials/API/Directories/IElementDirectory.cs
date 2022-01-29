using SilK.Unturned.Essentials.API.Directories.Sources;
using System.Collections.Generic;

namespace SilK.Unturned.Essentials.API.Directories
{
    public interface IElementDirectory<TElement, TElementSource> where TElementSource : IElementSource<TElement>
    {
        IReadOnlyCollection<TElementSource> Sources { get; }

        void AddSource(TElementSource source);

        void RemoveSource(TElementSource source);
    }
}
