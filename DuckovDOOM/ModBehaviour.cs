using System;
using System.Reflection;
using Duckov;
using Duckov.MiniGames;
using DuckovDOOM.Mixin;
using FeatherMod;
using FeatherMod.Events;
using FeatherMod.Events.GameEvents;
using FeatherMod.Register;
using FeatherMod.Utils;
using HarmonyLib;
using ManagedDoom;
using ManagedDoom.Duckov;
using ManagedDoom.Event;
using ModSetting;
using ModSetting.Api;
using UnityEngine;
using Doom = DuckovDOOM.Minigame.Doom;

namespace DuckovDOOM;

public class ModBehaviour : Duckov.Modding.ModBehaviour, IHasModid
{
    private bool hasGrab = false;

    public static ModBehaviour? Instance { get; private set; }
    public Config? cfg { get; private set; }

    private Harmony? harmony;
    
    public string GetModid()
    {
        return "DuckovDOOM";
    }

    protected override void OnAfterSetup()
    {
        base.OnAfterSetup();
        harmony = new Harmony(GetModid());
        Type busType = typeof(AudioManager).GetNestedType("Bus", BindingFlags.Public | BindingFlags.Instance);
        var getter = AccessTools.PropertyGetter(busType, "Volume");
        var postfix = typeof(BusMixin).GetMethod("PostfixVolume", BindingFlags.Public | BindingFlags.Static);
        harmony.Patch(getter, postfix: new HarmonyMethod(postfix));
        ModPathResolver.Register(GetModid(), Assembly.GetExecutingAssembly().Location);
        I18n.InitI18n(GetModid());
        Items.Init();
        Doom.Init();
        GamingConsole.OnGamingConsoleInteractChanged += Change;
        DuckovVideo.query = () => hasGrab;
        var settingsBuilder = SettingsBuilder.Create(info);
        cfg = new Config(settingsBuilder);
        EventBusManager.Instance.Sync.Register<LanguageChangedEvent>(evt => cfg.refreshUI(), -1);
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
        
        EventBusManager.Instance.Sync.Register<DoomFinishedLevel>(level => Debug.Log($"Finished level: {level.wadName}:{level.episode},{level.map}"), 0, RegistryManager.CurrentModid);
        EventBusManager.Instance.Sync.Register<DoomLoadLevel>(level => Debug.Log($"Loaded level: {level.wadName}:{level.episode},{level.map}"), 0, RegistryManager.CurrentModid);
        EventBusManager.Instance.Sync.Register<DoomPickupWeapon>(level => Debug.Log($"Grabbed weapon: {level.weapon}?{level.isDroppedFromEnemy}"), 0, RegistryManager.CurrentModid);
    }

    protected void OnDisable()
    {
        harmony?.UnpatchAll();
        GamingConsole.OnGamingConsoleInteractChanged -= Change;
        DuckovVideo.query = () => false;
        Setting.Clear();
        Instance = null;
    }

    private void Change(bool val)
    {
        hasGrab = val;
        Debug.Log("Changed stat");
    }
}