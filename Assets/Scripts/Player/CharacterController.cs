using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            GameManager.Instance.GameState = GameState.Playing; // Oyuncu yerine ulaştı
        }
    }

    public void MoveTo(Transform platform)
    {
        Vector3 newPos = new Vector3(platform.position.x, transform.position.y, platform.position.z);
        targetPosition = newPos;
        isMoving = true;
    }
}
