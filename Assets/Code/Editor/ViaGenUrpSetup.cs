#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ViaGen.Editor
{
    public static class ViaGenUrpSetup
    {
        public static void ConfigureUrp()
        {
            var guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset");
            if (guids.Length == 0)
            {
                guids = AssetDatabase.FindAssets("t:RenderPipelineAsset");
            }

            if (guids.Length == 0)
            {
                Debug.LogWarning("[ViaGen] Crie um URP Asset (Assets > Create > Rendering > URP Asset) e execute novamente.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var urp = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(path);
            if (urp == null) return;

            GraphicsSettings.defaultRenderPipeline = urp;
            QualitySettings.renderPipeline = urp;
            Debug.Log($"[ViaGen] URP ativo: {path}");
        }
    }
}
#endif
