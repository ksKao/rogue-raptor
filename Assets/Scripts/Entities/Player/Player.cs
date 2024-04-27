using UnityEngine;

public class Player : Entity
{
    [SerializeField] private float runSpeedMultiplier = 2;

    // components
    private PlayerInput playerInput;
    private CharacterController characterController;

    // effects
    private KnightAttack knightAttack;

    // animations
    private readonly int IS_RUNNING_ANIM_PARAM_BOOL = Animator.StringToHash("IsRunning");

    // states
    private bool isRunning = false;
    private float turnSmoothVelocity; // used for turning character during movement

    protected override void Awake()
    {
        base.Awake();

        // override healthBar to use the one in UIManager instead
        healthBar = UIManager.Instance.PlayerHealthBar;
        healthBar.AlwaysLookAtCamera = false;

        playerInput = new PlayerInput();

        characterController = GetComponent<CharacterController>();
        knightAttack = GetComponentInChildren<KnightAttack>();

        playerInput.Player.Run.started += ctx => isRunning = true;
        playerInput.Player.Run.canceled += ctx => isRunning = false;
        playerInput.Player.Attack.performed += ctx => Attack();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void FixedUpdate()
    {
        if (IsLockAction) return;

        Move(playerInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Move(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        bool isMoving = direction.magnitude >= 0.1f;

        animator.SetBool(IS_MOVING_ANIM_PARAM_BOOL, isMoving);
        animator.SetBool(IS_RUNNING_ANIM_PARAM_BOOL, isRunning);

        if (isMoving)
        {
            // https://www.youtube.com/watch?v=4HpC--2iowE
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);


            if (isRunning)
            {
                characterController.Move(runSpeedMultiplier * speed * Time.deltaTime * direction);
            }
            else
            {
                characterController.Move(speed * Time.deltaTime * direction);
            }
        }
    }

    protected override void Attack()
    {
        if (IsLockAction) return;

        base.Attack();

        float playRate = SecondPerAttack < 1 ? 1 + SecondPerAttack : SecondPerAttack == 1 ? 1 : 1 - SecondPerAttack;
        knightAttack.Activate(this, playRate, SecondPerAttack);
    }

    public override void TakeDamage(int damage, Entity source = null, float knockbackDuration = 0.5f, float knockbackForce = 10)
    {
        if (!IsLockAction)
            transform.LookAt(source.transform);

        // do not knockback player
        base.TakeDamage(damage, source, knockbackDuration, 0);
    }
}
