using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // References
	Rigidbody2D rb;
    PlayerInput input;

    // Movement
	[SerializeField]
    float moveSpeed;
    float moveX;

    // Jumping
    [SerializeField]
    float jumpSpeed;
    [SerializeField]
    float jumpBuffer;
    float jumpBufferCounter;
    [SerializeField]
    float hangTime;
    float hangTimeCounter;

    // Ground Checks
    bool grounded;
    [SerializeField]
    Transform feet;
    [SerializeField]
    float groundCheckRadius;
    [SerializeField]
    LayerMask walkMask;

    // Jump Charging
    [SerializeField]
    float jumpChargeTime;
    float jumpChargeCounter = 0;
    [SerializeField]
    float jumpChargeModifier;
    Vector2 startScale;
    [SerializeField]
    Vector2 scaleAmount;

    // Stomping
    [SerializeField]
    float stompSpeed;
    [SerializeField]
    float stompDelayTime;
    [SerializeField]
    GameObject stompParticles;
    [SerializeField]
    float stompCheckDistance; // This is a check to make sure the player isn't right above the ground when they stomp.
    [SerializeField]
    int stompDamage;
    [SerializeField]
    float stompRadius;

    // GIMP Charges
    [SerializeField]
    int currentGimp;
    [SerializeField]
    int maxGimp;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
	}

	private void Start() {
        startScale = transform.localScale;
	}

	// Update is called once per frame
	private void Update()
    {
        moveX = 0;
        if (jumpChargeCounter <= jumpChargeTime * 0.25f) moveX = input.actions["Move"].ReadValue<float>();

		if (moveX > 0.5) moveX = 1;
        else if (moveX < -0.5) moveX = -1;

		if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;
		if (hangTimeCounter > 0) hangTimeCounter -= Time.deltaTime;

		if (input.actions["Jump"].WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBuffer;
        }

        if (input.actions["Charge"].IsPressed() && grounded)
        {
            if (jumpChargeCounter < jumpChargeTime) jumpChargeCounter += Time.deltaTime;
		} else if (jumpChargeCounter > 0)
        {
            jumpChargeCounter -= Time.deltaTime;
        }

        transform.localScale = new Vector3(startScale.x + (jumpChargeCounter / jumpChargeTime * scaleAmount.x), startScale.y - (jumpChargeCounter / jumpChargeTime * scaleAmount.y), transform.localScale.z);
	
        if (input.actions["Stomp"].WasPressedThisFrame() && !grounded)
        {
            if (!Physics2D.Raycast(feet.position, Vector2.down, stompCheckDistance, walkMask))
                StartCoroutine(Stomp());
        }
    }

	private void FixedUpdate() {
		rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        grounded = Physics2D.OverlapCircle(feet.position, groundCheckRadius, walkMask);

        if (grounded)
        {
            hangTimeCounter = hangTime;
        }

        if (hangTimeCounter > 0 && jumpBufferCounter > 0)
        {
            float yVelocity = jumpSpeed;

            if (jumpChargeCounter > jumpChargeTime * 0.25f)
            {
                yVelocity += jumpChargeCounter / jumpChargeTime * jumpChargeModifier;
                jumpChargeCounter = 0;
            }
            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
            jumpBufferCounter = 0;
		}   
	}

    public IEnumerator Stomp() {
        float stompTimeCounter = stompDelayTime;
        float gravity = rb.gravityScale;

        while (stompTimeCounter > 0)
        {
            rb.gravityScale = 0;
            stompTimeCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            yield return null;
        }

        rb.gravityScale = gravity;

        while (!grounded)
        {
			rb.velocity = new Vector2(0, -stompSpeed);
			yield return null;
        }
        Instantiate(stompParticles, feet.position, Quaternion.identity);
        Collider2D[] collisions = Physics2D.OverlapCircleAll(feet.position, stompRadius);

        foreach (Collider2D collision in collisions)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(stompDamage);
            }
        }
    }
}
