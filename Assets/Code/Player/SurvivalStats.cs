using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Player
{
    public class SurvivalStats : MonoBehaviour
    {
        [SerializeField] private float oxygen = 100f;
        [SerializeField] private float energy = 100f;
        [SerializeField] private float temperature = 20f;
        [SerializeField] private float fuel = 100f;

        public float Oxygen => oxygen;
        public float Energy => energy;
        public float Temperature => temperature;
        public float Fuel => fuel;

        public void DrainEnergy(float amount) => energy = Mathf.Max(0f, energy - amount);

        public bool TrySpendEnergy(float amount)
        {
            if (energy < amount) return false;
            energy -= amount;
            return true;
        }
    }
}
