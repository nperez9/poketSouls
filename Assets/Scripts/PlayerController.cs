using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private LayerMask grassLayer;

    private bool isMoving = false;
    private Vector2 input;
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector2 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWakeable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        
        CheckForBattles();
    }

    private bool IsWakeable(Vector3 targetPos)
    {
        // Checks overposition on the solidOBjects tile
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckForBattles()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(Random.Range(0, 10) <= 1)
            {
                Debug.Log("Battle Begins!");
            }
        }
    }
}
