using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SilK.Unturned.Essentials.Localization
{
    public class SubStringLocalizer : IStringLocalizer
    {
        public readonly IStringLocalizer BaseStringLocalizer;

        public readonly string NamePrefix;

        private readonly string[] _subSections;

        public SubStringLocalizer(IStringLocalizer stringLocalizer, params string[] subSections)
        {
            BaseStringLocalizer = stringLocalizer;
            NamePrefix = string.Join(":", subSections) + ":";
            _subSections = subSections.ToArray();
        }

        public LocalizedString this[string name] => BaseStringLocalizer[NamePrefix + name];

        public LocalizedString this[string name, params object[] arguments] => BaseStringLocalizer[NamePrefix + name, arguments];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return BaseStringLocalizer.GetAllStrings(includeParentCultures);
        }

        [Obsolete]
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new SubStringLocalizer(BaseStringLocalizer.WithCulture(culture), _subSections);
        }
    }
}
