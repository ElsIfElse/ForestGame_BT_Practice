using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player_Actions : MonoBehaviour
{
    [Header("Raycast Settings")]
    Ray interactionRay;
    [SerializeField] GameObject playerCamera;
    [SerializeField] float interactionRayLength = 10f;
    [SerializeField] LayerMask interactionlayerMask;


    [SerializeField] Vector3 raycastOffset;

    RaycastHit interactionHitInfo;
    Manager_Collector managerCollector;
    UI_Manager uiManager;
    bool isDebugOn = false;

    //

    [SerializeField] Backpack backpack;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        uiManager = managerCollector.uiManager;
    }
    void Update()
    {
        InteractionRaycasting();

        PickFood_Action();
        FeedAnimal_Action();
    }

    // INITIALIZATION
    void SetRay()
    {
        interactionRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    }

    // RAYCASTING
    void InteractionRaycasting()
    {
        SetRay();

        Debug.DrawRay(interactionRay.origin, interactionRay.direction * interactionRayLength, Color.red);


        if (Physics.Raycast(interactionRay.origin, interactionRay.direction, out interactionHitInfo, interactionRayLength,interactionlayerMask))
        {
            // Debug.Log("Hit Object's Layer Is: "+interactionHitInfo.transform.gameObject.layer);
            if (interactionHitInfo.transform.gameObject.layer == 7)
            {
                uiManager.TurnOnIndicator_PickFood();
            }
            else if (interactionHitInfo.transform.gameObject.layer == 8)
            {
                uiManager.TurnOnIndicator_FeedAnimal();
            }
        }
        else
        {
            uiManager.TurnOffIndicator();
        }
        
    }

    // INTERACTIONS
    void PickFood_Action()
    {
        if (interactionHitInfo.transform.gameObject.layer == 7 && Input.GetKeyDown(KeyCode.E))
        {
            backpack.ItemPickup(interactionHitInfo.transform.gameObject.GetComponent<Pickable_Object>());
        }

        return;
        
    }
    void FeedAnimal_Action()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactionHitInfo.transform.gameObject.layer == 8){
            switch (interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().animalBreed)
            {
                case "Sheep":

                    if (isDebugOn)
                    {
                        Debug.Log("Attempting to feed sheep");
                    }

                    BackPack_Slot slotToCheck = backpack.CheckIfBackpackHasItem("SheepFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if (isDebugOn)
                        {
                            Debug.Log("Backpack does not contain sheep food");
                        }

                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);

                    if (isDebugOn)
                    {
                        Debug.Log($"Sheep was fed and used 1 sheep food");
                    }
                    break;
                case "Wolf":

                    if (isDebugOn)
                    {
                        Debug.Log("Attempting to feed wolf");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("WolfFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if (isDebugOn)
                        {
                            Debug.Log("Backpack does not contain wolf food");
                        }

                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);

                    if (isDebugOn)
                    {
                        Debug.Log("Wolf was fed and used 1 wolf food");
                    }

                break;
                case "Goat":

                    if(isDebugOn){
                        Debug.Log("Attempting to feed Goat");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("GoatFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if(isDebugOn){
                            Debug.Log("Backpack does not contain Goat food");
                        }

                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    
                    if (isDebugOn)
                    {
                        Debug.Log("Goat was fed and used 1 Goat food");
                    }

                break;
                case "Rabbit":

                    if(isDebugOn){
                        Debug.Log("Attempting to feed Rabbit");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("RabbitFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if(isDebugOn){
                            Debug.Log("Backpack does not contain Rabbit food");
                        }

                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    
                    if (isDebugOn)
                    {
                        Debug.Log("Rabbit was fed and used 1 Rabbit food");
                    }

                break;
                case "Bear":

                    if(isDebugOn){
                        Debug.Log("Attempting to feed Bear");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("BearFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if(isDebugOn){
                            Debug.Log("Backpack does not contain Bear food");
                        }

                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    
                    if (isDebugOn)
                    {
                        Debug.Log("Bear was fed and used 1 Bear food");
                    }

                break;
            }      
        }
    }
    void SetCameraOnAnimal_Action()
    {
        
    }

    // UTILITIES


    // REFERENCES

    // DEBUGGING

}
