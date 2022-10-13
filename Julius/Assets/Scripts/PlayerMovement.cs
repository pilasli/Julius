using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private GameManager _gameManager;
    private UIManager _uiManager;
    [SerializeField] CapsuleCollider2D capsul2D;
    [SerializeField] CircleCollider2D circle2D;
    [SerializeField] private int lives = 3;
    [SerializeField] private int numOfLives = 3;
    private Vector3 localScale;
    private bool isGrounded;
    private bool canTakeDamage = true;
    private float idleTime;
    private float orginalGravity;
    private float directionX;

    private bool canJump = false;
    private bool canAirJump = false;
    private float jumpPower = 14f;
    private float lastJumpTime = -100f;
    private float coyoteTime = 0.1f;
    private float coyoteCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private bool isWalking;
    private bool isRunning;
    private float walkSpeed = 300f;
    private float runSpeed = 400f;
    private bool canMove = true;
    private bool isDead = false;
    private bool isHurt = false;
    private bool isFacingRight = true;

    private bool isDashing;
    private bool canDash = true;
    private float dashTime = 0.5f;
    private float dashTimeLeft;
    private float dashSpeed = 10f;
    private float dashCooldown = 0.2f;

    private bool isSliding;
    private bool canSlide = true;
    private float slideSpeed = 10f;
    private float slideTime = 0.5f;
    private float slideCooldown = 0.2f;

    [SerializeField] private float aheadAmountX = 2f;
    [SerializeField] private float aheadAmountY = 6f;
    private Vector3 camTargetPositionOrj;
    [SerializeField] private Transform camTarget;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem turnDust;
    [SerializeField] private ParticleSystem jumpDust;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The GameManager on PlayerMovement is <null>");
        }
        _uiManager = GameObject.Find("Canvas").GetComponentInParent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager on PlayerMovement is <null>");
        }

        // To set necessary objects
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        localScale = transform.localScale;
        camTargetPositionOrj = new Vector3(camTarget.localPosition.x, camTarget.localPosition.y, camTarget.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (isSliding)
        {
            return;
        }

        // Set a bool veriable to understand we are on ground or not
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (isGrounded)
        {
            // To solve extra jump problem (because of Ground check system) a timer is added  
            animator.SetBool("IdleTime", false);
            if (Time.time >= (lastJumpTime + 0.2f))
            {
                lastJumpTime = Time.time;
                canJump = true;
            }
            canDash = true;

            if (Time.time >= idleTime + 10f)
            {
                idleTime = Time.time;
                animator.SetBool("IdleTime", true);
            }
        }

        if (!isDead && canMove)
        {
            directionX = Input.GetAxisRaw("Horizontal") * walkSpeed;

            //If press sprint button we will run instead of walk (faster movement)
            if (Input.GetButton("Sprint") && canMove)
            {
                directionX = Input.GetAxisRaw("Horizontal") * runSpeed;
                rb.velocity = new Vector2(directionX * Time.deltaTime, rb.velocity.y);
            }
        }

        // Manage coyote time, for a while we can jump even if we have left platform
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Manager jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump, on ground
        if (jumpBufferCounter >= 0 && !isDead && canJump && coyoteCounter > 0f)
        {
            canJump = false;
            CreateDust("jumpDust");
            AudioManager.instance.Play("Man_Jump");
            AudioManager.instance.Play("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpBufferCounter = 0;
        }
        // AirJump, after Dash on air we can jump one more time
        if (jumpBufferCounter >= 0 && !isDead && canAirJump & !isGrounded)
        {
            canAirJump = false;
            AudioManager.instance.Play("Man_Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpBufferCounter = 0;
        }
        // Jump control to get lower jumps if we stop pressing jump button       
        if (Input.GetButtonUp("Jump") && !isDead && !isGrounded)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Move camera point to change target that camera follows
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(aheadAmountX, aheadAmountY, camTarget.localPosition.z);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && isGrounded && Input.GetAxisRaw("Horizontal") == 0)
        {
            camTarget.localPosition = new Vector3(aheadAmountX, aheadAmountY - 8f, camTarget.localPosition.z);
        }
        else if (Input.GetKey(KeyCode.F) && isGrounded && Input.GetAxisRaw("Horizontal") == 0)
        {
            camTarget.localPosition = new Vector3(aheadAmountX + 8f, aheadAmountY, camTarget.localPosition.z);
        }
        else
        {
            camTarget.localPosition = camTargetPositionOrj;
        }

        //Dash, we can dash on air
        if (Input.GetButtonDown("Dash_Slide") && canDash && !isGrounded)
        {
            StartCoroutine(Dash());
            canAirJump = true;
        }

        //Slide, we can slide on ground
        if (Input.GetButtonDown("Dash_Slide") && canSlide && isGrounded)
        {
            StartCoroutine(Slide());
        }
        SetAnimationState();    //Check for animation states
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (isSliding)
        {
            return;
        }

        if (!isHurt)
        {
            rb.velocity = new Vector2(directionX * Time.deltaTime, rb.velocity.y);
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    // Setting animation states
    void SetAnimationState()
    {
        if (directionX == 0 || !isGrounded || isDashing || isSliding)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }

        if (rb.velocity.y == 0 || isGrounded || isDashing || isSliding)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }

        if (rb.velocity.y > 0)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", true);
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
            if (rb.velocity.y < -10f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -10f);
            }
        }

        if (Mathf.Abs(directionX) == walkSpeed && isGrounded && !isDashing && !isSliding)
        {
            animator.SetBool("IsWalking", true);
        }

        if (Mathf.Abs(directionX) == runSpeed && isGrounded && !isDashing && !isSliding)
        {
            animator.SetBool("IsRunning", true);

        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    //Checks for looking direction
    void CheckWhereToFace()
    {
        if (directionX > 0)
        {
            isFacingRight = true;
        }
        else if (directionX < 0)
        {
            isFacingRight = false;
        }
        if (((isFacingRight) && (localScale.x < 0)) || ((!isFacingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
            //Changing turnDust direction to back of player
            turnDust.transform.localScale = new Vector3(turnDust.transform.localScale.x * -1f, turnDust.transform.localScale.y, turnDust.transform.localScale.z);
            if (isGrounded)
            {
                CreateDust("turnDust");
            }
        }
        transform.localScale = localScale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool("IsDashing", true);
        float orginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        AudioManager.instance.Play("Dash");
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = orginalGravity;
        isDashing = false;
        animator.SetBool("IsDashing", false);
        yield return new WaitForSeconds(dashCooldown);
    }

    private IEnumerator Slide()
    {
        canSlide = false;
        isSliding = true;
        animator.SetBool("IsSliding", true);
        CreateDust("turnDust");
        capsul2D.enabled = false;
        circle2D.enabled = true;
        AudioManager.instance.Play("Slide");
        rb.velocity = new Vector2(transform.localScale.x * slideSpeed, 0f);
        yield return new WaitForSeconds(slideTime);
        circle2D.enabled = false;
        capsul2D.enabled = true;
        isSliding = false;
        animator.SetBool("IsSliding", false);
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;
        canTakeDamage = false;
        AudioManager.instance.Play("Hurt");
        animator.SetTrigger("IsHurt");
        canJump = false;
        canMove = false;
        canSlide = false;
        canDash = false;
        yield return new WaitForSeconds(0.7f);
        canJump = true;
        canMove = true;
        canSlide = true;
        canDash = true;
        isHurt = false;
        canTakeDamage = true;
    }

    private void CreateDust(string dust)
    {
        switch (dust)
        {
            case "turnDust":
                turnDust.Play();
                break;
            case "jumpDust":
                jumpDust.Play();
                break;
            default:
                Debug.Log("No dust to play");
                break;
        }
    }

    // Check for triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Damage();
        }
        if (other.gameObject.CompareTag("LiveUpgrade"))
        {
            Destroy(other.gameObject);
            UpgradeLive();
        }
        if (other.tag == "DeadZone")
        {
            GameOverProcess();
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Reached finish flag");
            directionX = 0;
            canMove = false;
            WinGameProcess();
        }
    }

    // Check for collisions
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Saw"))
        {
            Damage();
        }
        if (other.gameObject.CompareTag("Axe"))
        {
            Damage();
        }
    }

    // Damage calculations for player
    public void Damage()
    {
        if (canTakeDamage)
        {
            if (lives > 0)
            {
                lives--;
                _uiManager.UpdateLives(lives, numOfLives);

                StartCoroutine("HurtRoutine");
                rb.AddForce(-transform.forward * 500);
                if (lives < 1)
                {
                    isDead = true;
                    animator.SetTrigger("IsDead");
                    directionX = 0;
                    canMove = false;
                    GameOverProcess();
                }
            }
        }
    }

    // Upgrade player live value when take liveUpgrade
    public void UpgradeLive()
    {
        if (numOfLives < _uiManager.livesImage.Length)
        {
            AudioManager.instance.Play("Live_Upgrade");
            numOfLives++;
            lives = numOfLives;
            _uiManager.UpdateNumOfLives(numOfLives);
        }
    }

    // Game over process
    private void GameOverProcess()
    {
        _uiManager.GameOverSequence();
        _gameManager.GameOver();
    }

    // Winning game process
    private void WinGameProcess()
    {
        _gameManager.WinGame();
    }
}
