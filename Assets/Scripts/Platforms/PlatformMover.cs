using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float perfectThreshold = 0.03f;
    [SerializeField] private float minX = -1.5f;
    [SerializeField] private float maxX = 1.5f;

    private int direction = 1; // 1 sağa, -1 sola
    private bool isPlaced;

    private void OnEnable()
    {
        isPlaced = false;
    }

    private void Update()
    {
        if (speed > 0f && !isPlaced)
            Move();

        if (Input.GetMouseButtonDown(0) && !isPlaced)
            Stop();
    }

    /// <summary>
    /// Platformu belirlenen sınırlar içerisinde sağa sola hareket ettiren kod.
    /// </summary>
    private void Move()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= maxX)
        {
            direction = -1;
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= minX)
        {
            direction = 1;
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Platformu durdurur, hizalama kontrolünü yapıp ya perfect snap yada lose tetikler.
    /// Lose yoksa playerı yeni platforma koşturur.
    /// </summary>
    private void Stop()
    {
        if (isPlaced) return;

        Transform previous = GameManager.Instance.LastCubeTransform;
        float hangOver = transform.position.x - previous.position.x;
        float absHangOver = Mathf.Abs(hangOver);
        float maxSize = previous.localScale.x;

        // Lose sistemi. parçaya fizik ekleyip gameover'ı çalıştırır.
        if (absHangOver >= maxSize)
        {
            gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject, 2f);
            GameManager.Instance.GameOver();
            return;
        }

        isPlaced = true;
        speed = 0f;

        
        //Debug.Log($"Platform Count: {PlatformSpawner.Instance.platformCount}, Perfect Threshold: {absHangOver < perfectThreshold}");
        
        // İlk platformun countunu verdim burda
        bool isFirstMovablePlatform = (PlatformSpawner.Instance.platformCount == 1);
        
        if (absHangOver < perfectThreshold && !isFirstMovablePlatform)
        {
            transform.position = new Vector3(
                previous.position.x,
                transform.position.y,
                transform.position.z
            );

            AudioManager.Instance.PlayPerfectNote();
        }
        else if (!isFirstMovablePlatform) // Eğer ilk platform değilse taşan parçayı kesiyo.
        {
            AudioManager.Instance.ResetPerfectPitch();
            CutPlatform(hangOver, previous);
        }
        else // İlk platform ise hiçbir şey yapmıyor.
        {
            AudioManager.Instance.ResetPerfectPitch();
        }

        GameManager.Instance.LastCubeTransform = transform;

        var player = GameManager.Instance.Player;
        Vector3 platformCenter = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);

        player.OnReachedTarget += HandlePlatformArrival;
        player.MoveTo(platformCenter);
    }

    /// <summary>
    /// Oyuncu platformun merkezine ulaştığında çalışır, yeni platform oluşturur veya bitiş platformuna yönlendirir
    /// </summary>
    private void HandlePlatformArrival()
    {
        GameManager.Instance.Player.OnReachedTarget -= HandlePlatformArrival;

        if (PlatformSpawner.Instance.platformCount == GameManager.Instance.maxPlatformCount)
        {
            MoveToFinishPlatform();
        }
        else
        {
            PlatformSpawner.Instance.SpawnNextPlatform();
        }
    }

    /// <summary>
    /// Platformdan taşan kısmı keser ve düşen blok oluşturur
    /// </summary>
    private void CutPlatform(float hangOver, Transform previous)
    {
        float direction = hangOver > 0 ? 1f : -1f;
        float absHangOver = Mathf.Abs(hangOver);
        float newSize = previous.localScale.x - absHangOver;
        float newX = previous.position.x + (transform.position.x - previous.position.x) / 2f;

        transform.localScale = new Vector3(newSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Taşan kısmı hesaplayıp oluşturma işlemi
        float fallSize = absHangOver;
        float fallX = transform.position.x + (newSize / 2f + fallSize / 2f) * direction;

        GameObject fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingBlock.transform.localScale = new Vector3(fallSize, transform.localScale.y, transform.localScale.z);
        fallingBlock.transform.position = new Vector3(fallX, transform.position.y, transform.position.z);
        fallingBlock.AddComponent<Rigidbody>();
        Destroy(fallingBlock, 2f);
    }

    /// <summary>
    /// Oyuncuyu bitiş platformuna koşturur
    /// </summary>
    private void MoveToFinishPlatform()
    {
        var player = GameManager.Instance.Player;
        var finishPos = GameManager.Instance.FinishingPlatform.position;
        var target = new Vector3(finishPos.x, player.transform.position.y, finishPos.z);

        player.OnReachedTarget += HandleVictory;
        player.MoveTo(target);
    }

    /// <summary>
    /// Oyuncu bitiş platformuna ulaştığında dansı başlatır.
    /// </summary>
    private void HandleVictory()
    {
        GameManager.Instance.Player.OnReachedTarget -= HandleVictory;

        GameManager.Instance.TriggerWin();
    }
}
