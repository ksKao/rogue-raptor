using UnityEngine;
using UnityEngine.VFX;

public class Player : Singleton<Player>
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float runSpeedMultiplier = 2;
    [SerializeField] private float attackSpeed = 1; // How many times this character can attack in one second

    [Header("VFX")]
    [SerializeField] private VisualEffect swordSlash;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private PlayerAnimation anim; // custom class for handling animation
    private float turnSmoothVelocity; // used for turning character during movement
    private bool isRunning = false;
    private bool lockAction = false; // if true, character cannot perform any action, used for preventing user spam clicking to perform multiple consecutive action without cooldown


    public float AttackSpeed => attackSpeed;

    protected override void Awake()
    {
        base.Awake();

        playerInput = new PlayerInput();
        anim = new PlayerAnimation(GetComponentInChildren<Animator>());

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
                anim.ChangeAnimationState(PlayerAnimation.RUN);
            }
            else
            {
                characterController.Move(speed * Time.deltaTime * direction);
                anim.ChangeAnimationState(PlayerAnimation.WALK);
            }
        }
        else
        {
            anim.ChangeAnimationState(PlayerAnimation.IDLE);
        }
    }

    private void Attack()
    {
        if (lockAction) return;

        lockAction = true;

        // calculate animation speed, use 1 / x because thats how many attack the player can perform in a second
        float normalizedTime = 1 / attackSpeed;

        // play animation
        anim.ChangeAnimationState(PlayerAnimation.ATTACK, normalizedTime);

        // play vfx
        swordSlash.Play();
        swordSlash.playRate = normalizedTime < 1 ? 1 + normalizedTime : normalizedTime == 1 ? 1 : 1 - normalizedTime;

        // unlock action after animation finished
        Invoke(nameof(UnlockAction), normalizedTime);
    }

    private void UnlockAction()
    {
        lockAction = false;
    }
}
