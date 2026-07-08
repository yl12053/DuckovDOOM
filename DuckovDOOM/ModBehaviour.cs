using System.Reflection;
using DuckovDOOM.Minigame;
using FeatherMod;
using FeatherMod.Utils;

namespace DuckovDOOM;

public class ModBehaviour : Duckov.Modding.ModBehaviour, IHasModid
{
    public string GetModid()
    {
        return "DuckovDOOM";
    }

    protected override void OnAfterSetup()
    {
        base.OnAfterSetup();
        ModPathResolver.Register(GetModid(), Assembly.GetExecutingAssembly().Location);
        I18n.InitI18n(GetModid());
        Items.Init();
        Doom.Init();
    }
}