using UnityEngine;

namespace ViaGen.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;

        private void Start()
        {
            if (playerPrefab == null)
            {
                SpawnProceduralPlayer();
                return;
            }

            var point = spawnPoint != null ? spawnPoint.position : transform.position;
            var rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;
            Instantiate(playerPrefab, point, rot);
        }

        private void SpawnProceduralPlayer()
        {
            var point = spawnPoint != null ? spawnPoint.position : transform.position;
            var player = new GameObject("Player");
            player.transform.position = point;

            var cc = player.AddComponent<CharacterController>();
            cc.height = 1.8f;
            cc.radius = 0.35f;
            cc.center = new Vector3(0f, 0.9f, 0f);

            player.AddComponent<SurvivalStats>();
            player.AddComponent<PlayerVisualProcedural>();
            player.AddComponent<PlayerController>();

            var visual = new GameObject("Visual");
            visual.transform.SetParent(player.transform);
            visual.transform.localPosition = Vector3.zero;
            TryAttachAstronautMesh(visual.transform);
        }

        private static void TryAttachAstronautMesh(Transform visual)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Characters/Player");
            if (prefab != null)
            {
                var instance = Instantiate(prefab, visual);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
