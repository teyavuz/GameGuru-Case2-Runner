using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner Instance;

    [Header("Platform Settings")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Material[] platformMaterials;

    [Header("Finishing Platform")]
    [SerializeField] private GameObject finishingPlatformPrefab;

    [HideInInspector] public Transform LastPlatformTransform;

    [HideInInspector] public int platformCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        platformCount = 0;
        SpawnFirstPlatform();
        PlaceFinishingPlatform();
    }

    private void PlaceFinishingPlatform()
    {
        int platformCount = GameManager.Instance.maxPlatformCount;

        GameObject finishPlatform = Instantiate(finishingPlatformPrefab,
            new Vector3(0f, -0.2f, LastPlatformTransform.position.z + 3f * platformCount),
            Quaternion.identity);

        GameManager.Instance.FinishingPlatform = finishPlatform.transform;
    }

    private void SpawnFirstPlatform()
    {
        GameObject firstPlatform = Instantiate(platformPrefab, new Vector3(-4.8f, -0.2f, 2.4f), Quaternion.identity);
        firstPlatform.GetComponent<Renderer>().material = platformMaterials[Random.Range(0, platformMaterials.Length)];
        LastPlatformTransform = firstPlatform.transform;
        platformCount++;

        PlatformMover mover = firstPlatform.GetComponent<PlatformMover>();
        mover.enabled = false;
    }

    public void SpawnNextPlatform()
    {
        if (platformCount >= GameManager.Instance.maxPlatformCount)
        {
            return;
        }

        float lastPlatformZ = LastPlatformTransform.position.z;
        GameObject newPlatform = Instantiate(platformPrefab,
            new Vector3(LastPlatformTransform.position.x, LastPlatformTransform.position.y, lastPlatformZ + 3f),
            Quaternion.identity);

        newPlatform.GetComponent<Renderer>().material = platformMaterials[Random.Range(0, platformMaterials.Length)];

        if (platformCount > 1)
        {
            float previousScaleX = GameManager.Instance.LastCubeTransform.localScale.x;
            newPlatform.transform.localScale = new Vector3(previousScaleX, newPlatform.transform.localScale.y, newPlatform.transform.localScale.z);
        }

        LastPlatformTransform = newPlatform.transform;
        platformCount++;
    }
}
