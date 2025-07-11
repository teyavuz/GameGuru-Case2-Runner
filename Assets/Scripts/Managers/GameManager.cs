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
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera normalCam;
    [SerializeField] private CinemachineFreeLook winCam;

    private Coroutine cameraRotationCoroutine;

    private void Awake()
    {
        Instance = this;
        GameState = GameState.Ready;
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        GameState = GameState.Playing;
    }

    public void GameOver()
    {
        GameState = GameState.GameOver;
        AudioManager.Instance.ResetPerfectPitch();
        Debug.Log("Game Over");
        StartCoroutine(WaitASecondAndLoadScene(5f));
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

    private IEnumerator WaitASecondAndLoadScene(float seconds)
    {
        AudioManager.Instance.PlayGameOverMusic();
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);
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
