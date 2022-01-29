using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.API.Directories.Sources
{
    public interface IElementSource<TElement>
    {
        Task<IReadOnlyCollection<TElement>> GetContentsAsync();
    }
}
