using System.Collections.Generic;
using UnityEngine;

namespace ViaGen.Planet
{
    public class PlanetManager : MonoBehaviour
    {
        public static PlanetManager Instance { get; private set; }
        private readonly HashSet<PlanetEmotion> _visited = new();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        public void RegisterGeneratedPlanet(PlanetEmotion emotion) => _visited.Add(emotion);
        public bool HasVisited(PlanetEmotion emotion) => _visited.Contains(emotion);
    }
}
