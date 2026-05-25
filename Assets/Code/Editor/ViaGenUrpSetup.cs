#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ViaGen.Editor
{
    public static class ViaGenUrpSetup
    {
        [MenuItem("ViaGen/Setup/Configure URP Pipeline")]
        public static void ConfigureUrp()
        {
            var guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset");
            if (guids.Length == 0)
            {
                Debug.LogWarning("[ViaGen] Crie um URP Asset (Assets > Create > Rendering > URP Asset) e execute novamente.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var urp = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
            if (urp == null) return;

            GraphicsSettings.defaultRenderPipeline = urp;
            QualitySettings.renderPipeline = urp;
            Debug.Log($"[ViaGen] URP ativo: {path}");
        }
    }
}
#endif
