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
        MinigameUtil.RegisterMinigame(new Identifier("DuckovDOOM", "doom"), makeDoom("freedoom1.wad", new Identifier("DuckovDOOM", "doom")));
        MinigameUtil.RegisterMinigame(new Identifier("DuckovDOOM", "doom2"), makeDoom("freedoom2.wad", new Identifier("DuckovDOOM", "doom2")));
    }

    public static GameObject makeDoom(string wad, Identifier identifier)
    {
        var minigameBase = MinigameUtil.NewMinigameBase(identifier, out var camera, out var ui);
        minigameBase.AddComponent<AudioListener>();
        minigameBase.GetComponent<MiniGame>().tickTiming = MiniGame.TickTiming.Update;
        camera.orthographic = true;
        camera.orthographicSize = 160f;
        camera.cullingMask = -1;

        ui.clearFlags = CameraClearFlags.Nothing;

        var doombehaviour = minigameBase.gameObject.AddComponent<DoomBehaviour>();
        doombehaviour.wadName = wad;
        
        var screen = GameObject.CreatePrimitive(PrimitiveType.Quad);
        screen.layer = 30;
        screen.name = "DoomDisplay";
        
        screen.transform.SetParent(minigameBase.transform, false);
        screen.transform.localPosition = new Vector3(0, 0, 5);
        // screen.transform.localScale = new Vector3(320, 480, 1);
        screen.transform.localScale = new Vector3(310, 420, 1);
        screen.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.forward);
        var renderer = screen.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("UI/Default"));

        GameObject back = GameObject.CreatePrimitive(PrimitiveType.Cube);
        back.layer = 30;
        back.name = "BACKGROUND";
        back.transform.SetParent(minigameBase.transform, false);
        back.transform.localPosition = new Vector3(0, 0, 10);
        back.transform.localScale = new Vector3(480, 320, 1);
        Object.Destroy(back.GetComponent<Collider>());

        var rendererBg = back.GetComponent<MeshRenderer>();
        rendererBg.material = new Material(Shader.Find("UI/Default"));
        rendererBg.material.color = Color.black;

        return minigameBase;
    }
}