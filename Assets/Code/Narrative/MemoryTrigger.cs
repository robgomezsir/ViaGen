using UnityEngine;

namespace ViaGen.Narrative
{
    public class MemoryTrigger : MonoBehaviour
    {
        [SerializeField] private string memoryId;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            MemoryManager.Instance?.PlayMemory(memoryId);
        }
    }
}
