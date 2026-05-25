using System.Collections.Generic;
using UnityEngine;

namespace ViaGen.Ship
{
    public class ShipManager : MonoBehaviour
    {
        public static ShipManager Instance { get; private set; }
        private readonly HashSet<string> _tech = new() { "scanner_basic" };

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        public bool HasTech(string techId) => _tech.Contains(techId);
        public void UnlockTech(string techId) => _tech.Add(techId);
    }
}
