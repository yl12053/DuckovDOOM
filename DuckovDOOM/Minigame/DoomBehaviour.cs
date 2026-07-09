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

    private void StartDoom()
    {
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

        var music = Game.gameObject.transform.Find("DoomMusic");
        if (music == null)
        {
            Debug.LogError("Cant find music component");
            return;
        }
        
        var musicComp = music.GetComponent<AudioBehaviour>();
        musicComp.func = (floats) =>
        {
            if (Doom.Disposed || Doom.music.IsDisposed) return;
            Doom.music.stream.OnGetData(floats, 2, 0);
        };
        musicComp.Play();
    }
    
    private void GamingConsoleInteractChanged(bool attached)
    {
        if (attached)
        {
            if (Doom == null || Doom.Disposed) StartDoom();
        }
    }
    
    protected override void Start()
    {
        base.Start();
        StartDoom();
        GamingConsole.OnGamingConsoleInteractChanged += GamingConsoleInteractChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GamingConsole.OnGamingConsoleInteractChanged -= GamingConsoleInteractChanged;
        if (Doom == null) return;
        if (Doom.Disposed) return;
        Doom.Dispose();
        Doom = null;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (Doom == null) return;
        if (Doom.Disposed) return;
        Doom.DoUpdate(deltaTime);
    }

    protected void OnGUI()
    {
        if (Doom == null) return;
        if (Doom.Disposed) return;
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