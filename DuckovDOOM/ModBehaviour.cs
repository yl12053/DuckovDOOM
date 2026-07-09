using System;
using System.Reflection;
using Duckov.MiniGames;
using FeatherMod;
using FeatherMod.Utils;
using ManagedDoom;
using ManagedDoom.Duckov;
using ModSetting;
using ModSetting.Api;
using UnityEngine;
using Doom = DuckovDOOM.Minigame.Doom;

namespace DuckovDOOM;

public class ModBehaviour : Duckov.Modding.ModBehaviour, IHasModid
{
    private bool hasGrab = false;

    public static ModBehaviour Instance { get; private set; }
    public Config cfg { get; private set; }
    
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
        GamingConsole.OnGamingConsoleInteractChanged += Change;
        DuckovVideo.query = () => hasGrab;
        var settingsBuilder = SettingsBuilder.Create(info);
        cfg = new Config(settingsBuilder);
        Instance = this;

        ShopGoodsData cartidge = new ShopGoodsData()
        {
            merchantProfileID = "Merchant_Normal",
            itemIdentifier = new("DuckovDOOM", "doom"),
            maxStock = 1,
            forceUnlock = true,
            priceFactor = 1,
            possibility = 1
        };
        ShopUtils.AddGoods(cartidge);
    }

    protected void OnDisable()
    {
        GamingConsole.OnGamingConsoleInteractChanged -= Change;
        DuckovVideo.query = () => false;
        Setting.Clear();
    }

    private void Change(bool val)
    {
        hasGrab = val;
        Debug.Log("Changed stat");
    }
}