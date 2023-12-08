using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MixMod
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public Type ResourceType { get; }

        public LocalizedDescriptionAttribute(Type resourceType, string name) : base(name)
        {
            ResourceType = resourceType;
        }

        public override string Description
        {
            get
            {
                var resourceManager = ResourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as ResourceManager;
                var culture = ResourceType.GetProperty("Culture", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as CultureInfo;
                if (resourceManager is null)
                {
                    return null;
                }

                return resourceManager.GetString(base.Description, culture);
            }
        }
    }

}
