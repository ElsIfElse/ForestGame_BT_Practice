using UnityEngine;
using UnityEditor;

public class FixTerrainCircleBug : MonoBehaviour
{
    private const string ToggleKey = "TerrainRenderFixEnabled";

    [MenuItem("Tools/Toggle Terrain Render Fix")]
    public static void ToggleTerrainFix()
    {
        bool isEnabled = EditorPrefs.GetBool(ToggleKey, false);
        Terrain terrain = Terrain.activeTerrain;
        Camera cam = Camera.main;

        if (terrain == null)
        {
            Debug.LogWarning("❌ No active Terrain found.");
            return;
        }

        if (cam == null)
        {
            Debug.LogWarning("❌ No Main Camera found.");
            return;
        }

        if (!isEnabled)
        {
            // Boosted values to remove LOD fade bugs
            terrain.detailObjectDistance = 1000f;
            terrain.treeDistance = 1000f;
            terrain.treeBillboardDistance = 1000f;
            terrain.treeCrossFadeLength = 200f;
            terrain.treeMaximumFullLODCount = 10000;
            terrain.drawInstanced = false;
            terrain.basemapDistance = 1000f;

            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 1000f;

            EditorPrefs.SetBool(ToggleKey, true);
            Debug.Log("✅ Terrain Render Fix ENABLED.");
        }
        else
        {
            // Default Unity values
            terrain.detailObjectDistance = 80f;
            terrain.treeDistance = 500f;
            terrain.treeBillboardDistance = 50f;
            terrain.treeCrossFadeLength = 5f;
            terrain.treeMaximumFullLODCount = 50;
            terrain.drawInstanced = true;
            terrain.basemapDistance = 250f;

            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 1000f;

            EditorPrefs.SetBool(ToggleKey, false);
            Debug.Log("↩️ Terrain Render Fix DISABLED.");
        }
    }
}