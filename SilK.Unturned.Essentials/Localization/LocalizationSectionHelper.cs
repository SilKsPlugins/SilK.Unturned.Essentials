using System;
using System.Collections.Generic;
using System.Reflection;

namespace SilK.Unturned.Essentials.Localization
{
    public static class LocalizationSectionHelper
    {
        public static string[] GetSubSectionsFromType(Type type)
        {
            var inheritedTypes = new Stack<Type>();

            var current = type;

            while (current != null)
            {
                inheritedTypes.Push(current);
                current = current.BaseType;
            }

            var subSections = new List<string>();

            while (inheritedTypes.Peek() != null)
            {
                current = inheritedTypes.Pop();

                var localizationSection = current.GetCustomAttribute<LocalizationSectionAttribute>();

                if (localizationSection == null)
                {
                    continue;
                }

                subSections.AddRange(localizationSection.SectionNames);
            }

            return subSections.ToArray();
        }
    }
}
