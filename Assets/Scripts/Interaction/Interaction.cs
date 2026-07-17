using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
interface I_Interactive
{
    public void Interacted();
}
public class Interaction : MonoBehaviour
{
    public GameObject crosshair;
    public Transform interactionTransform;
    public float interactionRange;
    private Movement playerMovement;
    private InputAction interactAction;
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
    }

    void Update()
    {
        InteractionUsed();
        //isInInteractionRange();
    }

    private void InteractionUsed()
    {
        if (interactAction.IsPressed())
        {
            Ray raycastCheckObject = new Ray(interactionTransform.position, interactionTransform.forward * interactionRange);
            if (Physics.Raycast(raycastCheckObject, out RaycastHit hit, interactionRange))
                if (hit.collider.gameObject.TryGetComponent(out I_Interactive interactiveObject)) interactiveObject.Interacted();
        }
    }


    private void isInInteractionRange()
    {
        Ray raycastCheckObject = new Ray(interactionTransform.position, interactionTransform.forward);
        if (Physics.Raycast(raycastCheckObject, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out I_Interactive interactiveObject))
            { crosshair.SetActive(true); }
            else
            { crosshair.SetActive(false); }
        }
        else
        { crosshair.SetActive(false); }
    }
}