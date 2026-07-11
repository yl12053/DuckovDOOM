using DuckovDOOM.Event;
using FeatherMod.Saves;
using FeatherMod.Utils;

namespace DuckovDOOM.Minigame;

public class DoomUtils
{
    public static DoomEventHandler.LevelIndicator? PersonalBestOf(string wadName)
    {
        if (ModBehaviour.Instance == null) return null;
        return SaveUtils.Load<DoomEventHandler.LevelIndicator>(new Identifier(ModBehaviour.Instance.GetModid(),
                $"max_level_{wadName}"));
    }
}