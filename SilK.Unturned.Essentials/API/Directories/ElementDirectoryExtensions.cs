using SilK.Unturned.Essentials.API.Directories.Sources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.API.Directories
{
    public static class ElementDirectoryExtensions
    {
        public static async Task<IReadOnlyCollection<TElement>> GetContentsAsync<TElement, TElementSource>(
            this IElementDirectory<TElement, TElementSource> directory) where TElementSource : IElementSource<TElement>
        {
            var contents = new List<TElement>();

            foreach (var source in directory.Sources)
            {
                var sourceContents = await source.GetContentsAsync();

                contents.AddRange(sourceContents);
            }

            return contents;
        }
    }
}
