using System;
using System.IO;
using System.Reflection;
using Duckov.MiniGames;
using ManagedDoom;
using ManagedDoom.Duckov;
using UnityEngine;

namespace DuckovDOOM.Minigame;

public class DoomBehaviour: MiniGameBehaviour
{
    private DuckovDoom? Doom;
    public string wadName;
    
    protected override void Start()
    {
        base.Start();
        Doom = new(new CommandLineArgs(new[]
        {
            "-iwad",
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), wadName)
        }), ModBehaviour.Instance.cfg, Game);
        Doom.DoStart();
        var disp = Game.gameObject.transform.Find("DoomDisplay");
        if (disp == null)
        {
            Debug.LogError("Cant find");
            return;
        }
        
        var renderer = disp.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No Renderer");
            return;
        }
        
        renderer.material.mainTexture = Doom.video.Texture; 
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Doom.Dispose();
        Doom = null;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Doom.DoUpdate(deltaTime);
    }

    protected void OnGUI()
    {
        if (Doom == null) return;
        Event e = Event.current;
        if (e != null && e.isKey)
        {
            if (e.type == UnityEngine.EventType.KeyDown)
            {
                Doom?.KeyDown(e.keyCode);
            }

            if (e.type == UnityEngine.EventType.KeyUp)
            {
                Doom?.KeyUp(e.keyCode);
            }
        }
    }
}