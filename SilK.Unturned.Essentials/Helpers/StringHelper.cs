using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilK.Unturned.Essentials.Helpers
{
    public static class StringHelper
    {
        public static T? FindBestMatch<T>(this IEnumerable<T> enumerable, string searchTerm, Func<T, string> searchTermFunc)
        {
            searchTerm = searchTerm.ToLower();

            return enumerable.Where(element =>
                    searchTermFunc(element).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .MinBy(element =>
                    global::OpenMod.Core.Helpers.StringHelper.LevenshteinDistance(searchTerm,
                        searchTermFunc(element).ToLower()))
                .FirstOrDefault();
        }
    }
}
