using UnityEngine;

public class AnimationListner : MonoBehaviour
{
    private Movement movement;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        animator.SetFloat("X", Mathf.Abs(movement.MoveDir));
        animator.SetFloat("Y", Mathf.Abs(movement.MoveJump));
        animator.SetBool("Ground", movement.IsGrounded);
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
