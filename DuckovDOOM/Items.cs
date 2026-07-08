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
        ItemData cartridge = new ItemData();
        cartridge.itemId = 379720;
        cartridge.localizationKey = "DuckovDOOM.item.name.doomcartridge";
        cartridge.localizationDesc = "DuckovDOOM.item.desc.doomcartridge";
        cartridge.weight = 0.1f;
        cartridge.value = 155;
        cartridge.quality = 5;
        cartridge.displayQuality = DisplayQuality.Orange;
        cartridge.spritePath = "doom.png";
        cartridge.tags = new List<string>();
        cartridge.tags.Add("Cartridge");
        
        ItemUtils.CreateCustomCartridge(new Identifier("DuckovDOOM", "doom"), new Identifier("DuckovDOOM", "doom"), cartridge);
    }
}