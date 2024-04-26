using UnityEngine;
using UnityEngine.VFX;

public class Player : Entity
{
    [SerializeField] private float runSpeedMultiplier = 2;

    [Header("VFX")]
    [SerializeField] private VisualEffect swordSlash;

    // components
    private PlayerInput playerInput;
    private CharacterController characterController;

    // animations
    private static readonly AnimationHash IDLE_ANIMATION = new("Idle");
    public static readonly AnimationHash RUN_ANIMATION = new("Run");

    // states
    private bool isRunning = false;
    private float turnSmoothVelocity; // used for turning character during movement

    protected override void Awake()
    {
        base.Awake();

        playerInput = new PlayerInput();

        characterController = GetComponent<CharacterController>();

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
        if (!lockAction)
            Move(playerInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Move(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // https://www.youtube.com/watch?v=4HpC--2iowE
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (isRunning)
            {
                characterController.Move(runSpeedMultiplier * speed * Time.deltaTime * direction);
                animationController.ChangeAnimationState(RUN_ANIMATION);
            }
            else
            {
                characterController.Move(speed * Time.deltaTime * direction);
                animationController.ChangeAnimationState(AnimationController.WALK_ANIMATION);
            }
        }
        else
        {
            // put transition false here because of weird transition from attack to idle
            animationController.ChangeAnimationState(IDLE_ANIMATION, 0, false);
        }
    }

    protected override void Attack()
    {
        base.Attack();

        // check hit
        // https://docs.unity3d.com/ScriptReference/Physics.CapsuleCast.html
        Vector3 p1 = transform.position + characterController.center + 0.5f * -characterController.height * Vector3.up;
        Vector3 p2 = p1 + Vector3.up * characterController.height;
        if (Physics.CapsuleCast(p1, p2, characterController.radius, transform.forward, out RaycastHit hit, attackRange))
        {
            if (hit.transform.TryGetComponent(out Monster monster))
            {
                monster.OnHit();
            }
        }

        float secondPerAttack = 1 / attackSpeed;

        // play vfx
        swordSlash.Play();
        swordSlash.playRate = secondPerAttack < 1 ? 1 + secondPerAttack : secondPerAttack == 1 ? 1 : 1 - secondPerAttack;
    }
}
