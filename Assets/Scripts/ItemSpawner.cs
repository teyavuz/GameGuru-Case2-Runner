using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private GameObject[] itemPrefabs;

    [Range(0, 100)]
    [SerializeField] private int spawnChance = 35;

    private void OnEnable()
    {
        if (PlatformSpawner.Instance.platformCount < 1) return;
        TrySpawnItemWithChance();
    }

    private void TrySpawnItemWithChance()
    {
        int dice = Random.Range(0, 100);

        if (dice < spawnChance)
        {
            SpawnRandomItem();
        }
    }

    private void SpawnRandomItem()
    {
        if (itemPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
    }
}
