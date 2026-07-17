using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector2 clampAngle;
    [SerializeField] private float sensivity = 100;
    private InputAction mouseAction;
    [SerializeField] private GameObject cam;

    private Vector2 rotation;
    void Start()
    {
        mouseAction = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        Vector2 mouseInput = mouseAction.ReadValue<Vector2>();

        mouseInput *= sensivity;

        rotation += mouseInput * Time.fixedDeltaTime;
        rotation.y = Mathf.Clamp(rotation.y, clampAngle.x, clampAngle.y);

        cam.transform.localEulerAngles = new Vector3(-rotation.y, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.x, 0);
    }




}
