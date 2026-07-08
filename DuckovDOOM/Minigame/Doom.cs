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
        camera.orthographic = true;
        camera.orthographicSize = 160f;
        camera.cullingMask = -1;

        ui.clearFlags = CameraClearFlags.Nothing;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.layer = 30;
        cube.name = "DEBUG_CUBE";
        cube.transform.SetParent(minigameBase.transform, false);
        cube.transform.localPosition = new Vector3(0, 0, 5);
        cube.transform.localScale = new Vector3(40, 40, 40);
        Object.Destroy(cube.GetComponent<Collider>());

        var renderer = cube.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("UI/Default"));
        renderer.material.color = Color.red;
        
        MinigameUtil.RegisterMinigame(new Identifier("DuckovDOOM", "doom"), minigameBase);
        if (MinigameUtil.Instance.MinigameRegistry.Get(new Identifier("DuckovDOOM", "doom")) == null)
        {
            Debug.Log("Unexpected null");
        }
    }
}