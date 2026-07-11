using System.Collections.Generic;
using Duckov.Utilities;
using FeatherMod;
using FeatherMod.Utils;
using ItemStatsSystem;
using UnityEngine;

namespace DuckovDOOM;

public class Items
{
    public static void Init()
    {
        if (ModBehaviour.Instance == null) return;
        ItemData cartridge = new ItemData();
        cartridge.itemId = 379720;
        cartridge.localizationKey = "DuckovDOOM.item.name.doomcartridge";
        cartridge.localizationDesc = "DuckovDOOM.item.name.doomcartridge_Desc";
        cartridge.weight = 0.1f;
        cartridge.value = 155;
        cartridge.quality = 5;
        cartridge.displayQuality = DisplayQuality.Orange;
        cartridge.spritePath = "doom.png";
        cartridge.tags = new List<string>();
        cartridge.tags.Add("Cartridge");
        
        ItemUtils.CreateCustomCartridge(new Identifier(ModBehaviour.Instance.GetModid(), "doom"), new Identifier(ModBehaviour.Instance.GetModid(), "doom"), cartridge);
        
        ItemData cartridge2 = new ItemData();
        cartridge2.itemId = 379721;
        cartridge2.localizationKey = "DuckovDOOM.item.name.doomcartridge2";
        cartridge2.localizationDesc = "DuckovDOOM.item.name.doomcartridge2_Desc";
        cartridge2.weight = 0.1f;
        cartridge2.value = 155;
        cartridge2.quality = 5;
        cartridge2.displayQuality = DisplayQuality.Orange;
        cartridge2.spritePath = "doom2.png";
        cartridge2.tags = new List<string>();
        cartridge2.tags.Add("Cartridge");
        
        ItemUtils.CreateCustomCartridge(new Identifier(ModBehaviour.Instance.GetModid(), "doom2"), new Identifier(ModBehaviour.Instance.GetModid(), "doom2"), cartridge2);
    }
}