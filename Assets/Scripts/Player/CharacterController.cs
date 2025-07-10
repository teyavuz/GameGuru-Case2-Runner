using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Animator animator;
    private Coroutine moveCoroutine;

    public event Action OnReachedTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine(targetPosition));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        animator.speed = 1f;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        animator.speed = 0f;

        OnReachedTarget?.Invoke();
        moveCoroutine = null;
    }

    public void PlayVictoryAnimation()
    {
        animator.speed = 1f;
        animator.Play("Win");
    }
}
