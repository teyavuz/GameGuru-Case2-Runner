using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour, ICollectible
{
    [Header("Effect Settings")]
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float destroyDelay = 0.5f;

    private void Update()
    {
        transform.Rotate(Vector3.up, 30 * Time.deltaTime);
    }
    public virtual void Collect()
    {
        Instantiate(collectEffect, transform.position, Quaternion.identity);

        AudioManager.Instance.CollectibleCollected(collectSound);

        OnCollected();

        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    protected abstract void OnCollected();
}
