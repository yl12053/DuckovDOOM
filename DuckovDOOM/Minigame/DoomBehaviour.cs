using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Duckov;
using Duckov.MiniGames;
using FMOD.Studio;
using FMODUnity;
using HarmonyLib;
using ManagedDoom;
using ManagedDoom.Duckov;
using UnityEngine;

namespace DuckovDOOM.Minigame;

public class DoomBehaviour: MiniGameBehaviour
{
    private DuckovDoom? Doom;
    public string? wadName;
    
    private static volatile float musicBusVolumeMultiplier = 1f;

    private static readonly AccessTools.FieldRef<AudioManager, AudioManager.Bus> musicBusRef
        = AccessTools.FieldRefAccess<AudioManager, AudioManager.Bus>("musicBus");

    private static Bus bus = RuntimeManager.GetBus("bus:/Master/Music");

    public static float MusicBusVolumeMultiplier
    {
        get => musicBusVolumeMultiplier;
        set
        {
            musicBusVolumeMultiplier = value;
            bus.setVolume(GameManager.Instance.audioManager.musicBus.Volume);
        }
    }

    private void StartDoom()
    {
        Doom = new(new CommandLineArgs(new[]
        {
            "-iwad",
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), wadName)
        }), ModBehaviour.Instance?.cfg, Game, wadName);
        Doom.DoStart(DuckovVideo.query() ? 1f : 0f);
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

    private CancellationTokenSource? _fadeCts;
    private CancellationTokenSource? _fadeCtsSfx;
    private static CancellationTokenSource? _fadeCtsMus;
    
    public async UniTask FadeMus(float fullduration, bool isFadeout, CancellationToken ct = default)
    {
        _fadeCtsMus?.Cancel();
        _fadeCtsMus = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_fadeCtsMus.Token, ct);
        var step = 1f / fullduration;
        if (isFadeout) step = -step;
        try
        {
            bool shouldStop = false;
            while (!shouldStop)
            {
                var mul = MusicBusVolumeMultiplier;
                mul += step * Time.deltaTime * 1000;
                if (mul >= 1f)
                {
                    mul = 1f;
                    shouldStop = true;
                }
                else if (mul <= 0f)
                {
                    mul = 0f;
                    shouldStop = true;
                }
                
                MusicBusVolumeMultiplier = mul;
                if (shouldStop) return;
                await UniTask.Yield(PlayerLoopTiming.Update, linkedCts.Token);
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            linkedCts.Dispose();
        }
    }
    
    public async UniTask FadeTo(float fullduration, bool isFadeout, CancellationToken ct = default)
    {
        _fadeCts?.Cancel();
        _fadeCts = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_fadeCts.Token, ct);
        var step = 1f / fullduration;
        if (isFadeout) step = -step;
        try
        {
            if (Doom == null || Doom.Disposed) return;

            var mus = Doom.music;
            bool shouldStop = false;
            while (!shouldStop && mus != null)
            {
                var mul = mus.volumeMultiplier;
                mul += step * Time.deltaTime * 1000;
                if (mul >= 1f)
                {
                    mul = 1f;
                    shouldStop = true;
                }
                else if (mul <= 0f)
                {
                    mul = 0f;
                    shouldStop = true;
                }
                
                mus.volumeMultiplier = mul;
                Debug.Log("New vol: " + mul);
                if (shouldStop) return;
                await UniTask.Yield(PlayerLoopTiming.Update, linkedCts.Token);
                mus = Doom.music;
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            linkedCts.Dispose();
        }
    }
    
    public async UniTask FadeToSfx(float fullduration, bool isFadeout, CancellationToken ct = default)
    {
        _fadeCtsSfx?.Cancel();
        _fadeCtsSfx = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_fadeCtsSfx.Token, ct);
        var step = 1f / fullduration;
        if (isFadeout) step = -step;
        try
        {
            if (Doom == null || Doom.Disposed) return;

            var mus = Doom.sound;
            bool shouldStop = false;
            while (!shouldStop && mus != null)
            {
                var mul = mus.VolumeMultiply;
                mul += step * Time.deltaTime * 1000;
                if (mul >= 1f)
                {
                    mul = 1f;
                    shouldStop = true;
                }
                else if (mul <= 0f)
                {
                    mul = 0f;
                    shouldStop = true;
                }
                
                mus.VolumeMultiply = mul;
                Debug.Log("New vol: " + mul);
                if (shouldStop) return;
                await UniTask.Yield(PlayerLoopTiming.Update, linkedCts.Token);
                mus = Doom.sound;
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            linkedCts.Dispose();
        }
    }
    
    private void GamingConsoleInteractChanged(bool attached)
    {
        if (attached)
        {
            if (Doom == null || Doom.Disposed) StartDoom();
        }
        FadeTo(1000f, !attached, this.GetCancellationTokenOnDestroy()).Forget();
        FadeToSfx(1000f, !attached, this.GetCancellationTokenOnDestroy()).Forget();
        FadeMus(1000f, attached).Forget();
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

    void OnApplicationQuit()
    {
        if (Doom == null) return;
        if (Doom.Disposed) return;
        Doom.Dispose();
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