using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{

    InputAction movementAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction crouchAction;
    Vector2 movement;


    CharacterController characterController;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundMask;
    private float gravity;

    [SerializeField] private float normalFov;
    [SerializeField] private float fovScale;
    [SerializeField] private float timeFov;
    private float currnetFov;
    [SerializeField] private Camera cam;

    private float speed = 0;
    [SerializeField] private float crouchSpeed = 3;
    [SerializeField] private float normalSpeed = 6;
    [SerializeField] private float sprintSpeed = 9;
    [HideInInspector] public bool hidden = false;

    [SerializeField] private HeadBobSystem hedbob;


    private bool canSprint = true;
    private float stamina = 100;
    [SerializeField] private float staminaDrain = 0.5f;
    private float resetStamina = 5;
    private bool isResetingStamina;

    [HideInInspector] public bool playerSprinting { get; private set; }
    [HideInInspector] public bool playerCrouching { get; private set; }
    [HideInInspector] public bool playerWalking { get; private set; }
    [HideInInspector] public bool playerMoving { get; private set; }

    public bool Loud = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currnetFov = normalFov;
        speed = normalSpeed;
        characterController = GetComponent<CharacterController>();
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
    }
    private void Update()
    {
        cam.fieldOfView = currnetFov;

        StaminaReset();

        Crouch();
        if (!hidden) Sprint();
        Move();
    }
    private void SetupBools()
    {
        playerSprinting = speed == sprintSpeed;
        playerCrouching = speed == crouchSpeed;
        playerWalking = speed == normalSpeed;
    }

    /// <summary>
    /// /RESET STAMINE ZROB BO N MA
    /// </summary>

    private void StaminaReset()
    {

        //canSprint = (stamina > 0);
        stamina = stamina < 0 ? stamina : 0;

        if (speed != sprintSpeed)
        {
            if (resetStamina < Time.timeSinceLevelLoad && !isResetingStamina)
            {
                resetStamina += 5;
            }
        }

        if (isResetingStamina)
        {

        }

    }

    private void Crouch()
    {
        if (crouchAction.IsPressed())
        {
            hedbob.Frequency = 5f;
            hedbob.Amount = 0.02f;
            hidden = true;
            transform.localScale = new Vector3(1, 0.5f, 1);
            speed = crouchSpeed;
        }
        else if (!crouchAction.IsPressed() && speed == crouchSpeed)
        {
            hedbob.Amount = 0.08f;
            hedbob.Frequency = 10f;
            speed = normalSpeed;
            hidden = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Sprint()
    {
        if (canSprint)
            if (sprintAction.IsPressed() && speed != sprintSpeed)
            {
                Loud = true;
                stamina -= staminaDrain;
                hedbob.Frequency = 15f;
                speed = sprintSpeed;
                StopAllCoroutines();
                StartCoroutine(changeFovUp());
            }
            else if (!sprintAction.IsPressed() && speed == sprintSpeed)
            {
                Loud = false;
                hedbob.Frequency = 10f;
                speed = normalSpeed;
                StopAllCoroutines();
                StartCoroutine(changeFovDown());
            }
    }



    private IEnumerator changeFovUp()
    {
        float leftTime = 0;


        while (leftTime < timeFov)
        {
            currnetFov = Mathf.Lerp(normalFov, normalFov + fovScale, (leftTime / timeFov));
            leftTime += Time.deltaTime;
            yield return null;
        }
        currnetFov = normalFov + fovScale;
    }

    private IEnumerator changeFovDown()
    {
        float leftTime = 0;


        while (leftTime < timeFov)
        {
            currnetFov = Mathf.Lerp(normalFov + fovScale, normalFov, (leftTime / timeFov));
            leftTime += Time.deltaTime;
            yield return null;
        }
        currnetFov = normalFov;
    }


    private void Move()
    {
        movement = movementAction.ReadValue<Vector2>();
        Vector3 movement3 = new Vector3(movement.y, 0, movement.x);

        Vector3 move = (transform.forward * movement.y + transform.right * movement.x) * speed * Time.deltaTime;

        playerMoving = move != Vector3.zero;

        move.y = Gravity();
        characterController.Move(move);
    }

    private bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.transform.position, 0.4f, groundMask);
    }


    private float Gravity()
    {
        if (!Grounded())
        {
            gravity -= 2;
            return gravity * Time.deltaTime;
        }

        gravity = -2;
        return 0f;
    }


}
