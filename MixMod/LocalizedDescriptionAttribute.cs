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
    public class LocalizedDescriptionAttribute(Type resourceType, string name) : DescriptionAttribute(name)
    {
        public Type ResourceType { get; } = resourceType;

        public override string Description
        {
            get
            {
                if (ResourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) is not ResourceManager resourceManager)
                {
                    return null;
                }
                
                var culture = ResourceType.GetProperty("Culture", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as CultureInfo;
                return resourceManager.GetString(base.Description, culture);
            }
        }
    }

}
