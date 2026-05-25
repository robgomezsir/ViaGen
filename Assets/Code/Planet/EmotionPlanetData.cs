using System;
using UnityEngine;

namespace ViaGen.Planet
{
    [Serializable]
    public class PlanetPOI
    {
        public string id;
        public Vector3 localPosition;
        public GameObject prefab;
        public bool spawnOnStart = true;
    }

    [CreateAssetMenu(fileName = "PlanetData", menuName = "ViaGen/Emotion Planet Data")]
    public class EmotionPlanetData : ScriptableObject
    {
        public PlanetEmotion emotion;
        public string displayName;
        public int terrainSeed = 42;
        public float terrainHeightScale = 120f;
        public Color fogColor = new(0.15f, 0.1f, 0.25f, 1f);
        public float fogDensity = 0.008f;
        public Color skyTint = new(0.4f, 0.35f, 0.7f, 1f);
        public PlanetPOI[] fixedPOIs;
    }
}
