using Duckov;
using DuckovDOOM.Minigame;
using HarmonyLib;
using UnityEngine;

namespace DuckovDOOM.Mixin;

public class BusMixin
{
    public static void PostfixVolume(ref float __result, AudioManager.Bus __instance)
    {
        if (__instance.Name.Equals("Master/Music"))
        {
            __result *= DoomBehaviour.MusicBusVolumeMultiplier;
        }
    }
}