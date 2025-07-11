using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    [Header("Platform Settings")]
    [Min(1)] public int maxPlatformCount;
    public Transform LastCubeTransform;
    public CharacterController Player;
    public Transform FinishingPlatform;

    [Header("UI")]
    [SerializeField] private GameObject pressToStartUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera normalCam;
    [SerializeField] private CinemachineFreeLook winCam;

    private Coroutine cameraRotationCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pressToStartUI.SetActive(true);
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        GameState = GameState.Ready;
    }

    private void Update()
    {
        if (GameState == GameState.Ready && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        GameState = GameState.Playing;
        pressToStartUI.SetActive(false);
        Debug.Log("Game Started!");

        var firstPlatform = PlatformSpawner.Instance.LastPlatformTransform;
        PlatformMover mover = firstPlatform.GetComponent<PlatformMover>();
        if (mover != null) mover.enabled = true;
    }

    public void GameOver()
    {
        GameState = GameState.GameOver;
        AudioManager.Instance.ResetPerfectPitch();
        Debug.Log("Game Over");
        StartCoroutine(WaitASecondAndLoadScene(5f, gameOverUI));
    }

    public void TriggerWin()
    {
        GameState = GameState.Win;
        Player.PlayVictoryAnimation();

        if (normalCam != null) normalCam.enabled = false;
        if (winCam != null) winCam.enabled = true;

        cameraRotationCoroutine = StartCoroutine(RotateCameraAroundPlayer());
        winUI.SetActive(true);
        AudioManager.Instance.PlayVictoryMusic();
        AudioManager.Instance.ResetPerfectPitch();
        Debug.Log("You Win!");


        StartCoroutine(WaitASecondAndLoadScene(7f, winUI));
    }

    private IEnumerator RotateCameraAroundPlayer()
    {
        while (GameState == GameState.Win)
        {
            if (winCam != null)
            {
                winCam.m_XAxis.Value += Time.deltaTime * 10f;
            }
            yield return null;
        }
    }

    private IEnumerator WaitASecondAndLoadScene(float seconds, GameObject UI)
    {
        AudioManager.Instance.PlayGameOverMusic();
        yield return new WaitForSeconds(3f);
        UI.SetActive(true);
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum GameState
{
    Ready,
    Playing,
    GameOver,
    Win
}
