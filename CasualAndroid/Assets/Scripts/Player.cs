using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private Animator anim;

    private enum MovementState { idle, jumping, falling }

    private float jumpPower = 5f;
    [SerializeField] private float maxJumpMagnitude = 10f;
    [SerializeField] private LayerMask jumpableGround;

    private Vector2 dragStartPos;


    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        coll = GetComponent<Collider2D>();
    }



    private void Update()
    {
        if (IsGrounded())
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStartPos = GetInputPosition();
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 DragEndPos = GetInputPosition();
                Vector2 velocity = (DragEndPos - dragStartPos) * jumpPower;

                //limiting the jump velocity magnitude
                float jumpMagnitude = velocity.magnitude;
                if (jumpMagnitude > maxJumpMagnitude)
                {
                    velocity = velocity.normalized * maxJumpMagnitude;
                }

                Vector2[] trajectory = Plot(rb, (Vector2)transform.position, velocity, 200);
                lineRenderer.positionCount = trajectory.Length;

                Vector3[] positions = new Vector3[trajectory.Length];
                for (int i = 0; i < trajectory.Length; i++)
                {
                    positions[i] = trajectory[i];
                }
                lineRenderer.SetPositions(positions);

            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 DragEndPos = GetInputPosition();
                Vector2 velocity = (DragEndPos - dragStartPos) * jumpPower;

                float jumpMagnitude = velocity.magnitude;
                if (jumpMagnitude > maxJumpMagnitude)
                {
                    velocity = velocity.normalized * maxJumpMagnitude;
                }

                rb.velocity = velocity;
                lineRenderer.positionCount = 0;
            }
        }

        UpdateAnimationState();
    }

    private Vector2 GetInputPosition()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return Camera.main.ScreenToWorldPoint(touch.position);
            }
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return Vector2.zero;
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;

            if (moveStep.magnitude > maxJumpMagnitude)
            {
                break;
            }

            results[i] = pos;
        }

        return results;
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;
        if(IsGrounded())
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, .1f, jumpableGround);
    }



}
