using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
    [Min(1)] public int maxPlatformCount;
    public Transform LastCubeTransform;

    public CharacterController Player;

    void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        GameState = GameState.GameOver;
        Debug.Log("Game Over");
    }
}

public enum GameState
{
    Ready,
    Playing,
    GameOver,
    Win
}
