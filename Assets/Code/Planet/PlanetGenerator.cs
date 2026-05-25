using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Planet
{
    [RequireComponent(typeof(Terrain))]
    public class PlanetGenerator : MonoBehaviour
    {
        [SerializeField] private EmotionPlanetData planetData;
        private Terrain _terrain;

        private void Awake() => _terrain = GetComponent<Terrain>();

        private void Start()
        {
            if (planetData != null) Generate(planetData);
        }

        public void Generate(EmotionPlanetData data)
        {
            planetData = data;
            Random.InitState(data.terrainSeed + (int)data.emotion);
            GenerateTerrain(data);
            SpawnPOIs(data);
            ApplyAtmosphere(data);
            if (PlanetManager.Instance != null)
                PlanetManager.Instance.RegisterGeneratedPlanet(data.emotion);
        }

        private void GenerateTerrain(EmotionPlanetData data)
        {
            if (_terrain == null) return;
            var terrainData = _terrain.terrainData;
            var width = terrainData.heightmapResolution;
            var heights = new float[width, width];
            var scale = data.terrainHeightScale / 600f;
            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var nx = x / (float)width;
                    var ny = y / (float)width;
                    var h = Mathf.PerlinNoise(nx * 4f + data.terrainSeed, ny * 4f) * 0.5f;
                    h += Mathf.PerlinNoise(nx * 12f, ny * 12f) * 0.25f;
                    heights[y, x] = h * scale;
                }
            }
            terrainData.SetHeights(0, 0, heights);
        }

        private void SpawnPOIs(EmotionPlanetData data)
        {
            if (data.fixedPOIs == null) return;
            foreach (var poi in data.fixedPOIs)
            {
                if (!poi.spawnOnStart || poi.prefab == null) continue;
                var worldPos = transform.position + poi.localPosition;
                if (_terrain != null)
                {
                    var terrainH = _terrain.SampleHeight(worldPos);
                    worldPos.y = terrainH + transform.position.y + 1f;
                }
                Instantiate(poi.prefab, worldPos, Quaternion.identity, transform);
            }
        }

        private void ApplyAtmosphere(EmotionPlanetData data)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = data.fogColor;
            RenderSettings.fogDensity = data.fogDensity;
            RenderSettings.ambientLight = data.skyTint * 0.4f;
            var mainLight = FindFirstObjectByType<Light>();
            if (mainLight != null)
                mainLight.color = data.skyTint;
        }
    }
}
