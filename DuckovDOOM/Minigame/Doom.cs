using Duckov.MiniGames;
using Duckov.Utilities;
using FeatherMod.Minigame;
using FeatherMod.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuckovDOOM.Minigame;

public class Doom
{
    public static void Init()
    {
        var minigameBase = MinigameUtil.NewMinigameBase(new Identifier("DuckovDOOM", "doom"), out var camera, out var ui);
        minigameBase.GetComponent<MiniGame>().tickTiming = MiniGame.TickTiming.Update;
        camera.orthographic = true;
        camera.orthographicSize = 160f;
        camera.cullingMask = -1;

        ui.clearFlags = CameraClearFlags.Nothing;

        var doombehaviour = minigameBase.gameObject.AddComponent<DoomBehaviour>();
        doombehaviour.wadName = "doomu.wad";
        
        var screen = GameObject.CreatePrimitive(PrimitiveType.Quad);
        screen.layer = 30;
        screen.name = "DoomDisplay";
        
        screen.transform.SetParent(minigameBase.transform, false);
        screen.transform.localPosition = new Vector3(0, 0, 5);
        screen.transform.localScale = new Vector3(320, 480, 1);
        screen.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.forward);
        var renderer = screen.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("UI/Default"));
        
        MinigameUtil.RegisterMinigame(new Identifier("DuckovDOOM", "doom"), minigameBase);
        if (MinigameUtil.Instance.MinigameRegistry.Get(new Identifier("DuckovDOOM", "doom")) == null)
        {
            Debug.Log("Unexpected null");
        }
    }
}