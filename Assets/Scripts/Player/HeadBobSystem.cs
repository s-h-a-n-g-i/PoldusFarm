using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBobSystem : MonoBehaviour
{
    [Range(0.001f, 0.1f)]
    public float Amount;

    [Range(1f, 30f)]
    public float Frequency = 10f;

    [Range(10f, 100f)]
    public float Smooth = 10f;

    Vector3 startPos;

    private InputAction movement;

    void Start()
    {
        movement = InputSystem.actions.FindAction("Move");
        startPos = transform.localPosition;
    }

    void Update()
    {
        CheckForHeadBobTrigger();
        stopHeadBob();
    }

    private void CheckForHeadBobTrigger()
    {
        Vector2 move = movement.ReadValue<Vector2>();
        float inputCheck = new Vector3(move.x, 0, move.y).magnitude;
        if (inputCheck > 0)
            StartHeadBob();
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Sin(Time.time * Frequency / 2) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }


    private void stopHeadBob()
    {
        if (transform.localPosition == startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }

}
