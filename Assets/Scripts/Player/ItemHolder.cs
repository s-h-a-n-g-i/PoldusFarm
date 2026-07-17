using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public GameObject[] holdingObjects = new GameObject[3];

    public int holdingSlot = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (holdingObjects[holdingSlot] != null)
        {
            SetItemToHand();    
        }
    }

    private void SetItemToHand() 
    {
        if (holdingObjects[holdingSlot].GetComponent<Rigidbody>()) 
        {
            //holdingObjects[holdingSlot].GetComponent<Rigidbody>();
        }
        holdingObjects[holdingSlot].transform.SetParent(transform, false);


    }

    public void AddItemToEQ(GameObject item) 
    {
        DropItemFromEQ(holdingObjects[holdingSlot]);
        holdingObjects[holdingSlot] = item;
    }

    public void DropItemFromEQ(GameObject item) 
    {
        if (holdingObjects[holdingSlot] == null) return;
        
        holdingObjects[holdingSlot].transform.SetParent(null);
    }

}
