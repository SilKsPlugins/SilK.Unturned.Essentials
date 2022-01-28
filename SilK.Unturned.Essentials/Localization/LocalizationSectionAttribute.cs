using System;

namespace SilK.Unturned.Essentials.Localization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LocalizationSectionAttribute : Attribute
    {
        public string[] SectionNames { get; set; }

        public LocalizationSectionAttribute(params string[] sectionNames)
        {
            SectionNames = sectionNames;
        }
    }
}
