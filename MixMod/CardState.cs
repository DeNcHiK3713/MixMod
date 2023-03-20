using System.ComponentModel;

namespace MixMod
{
    public enum CardState
    {
        [Description("Обычный режим, все карты и герои отображаются правильно")]
        Default,
        [Description("Все ваши герои и карты во время матча будут изменены")]
        OnlyMy,
        [Description("Все герои и карты во время матча будут изменены")]
        All,
        [Description("Все герои и карты во время матча будут обычными, без анимаций")]
        Disabled
    }
}
