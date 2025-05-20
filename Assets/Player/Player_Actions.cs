using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    Player_Movement playerMovement;
    CinemachinePanTilt playerCameraRotation;
    Audio_Manager audioManager;

    float originalPanAxis;
    float originalTilAxis;

    //

    [SerializeField] Backpack backpack;
    CraftingTable_Manager craftingTableManager;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        uiManager = managerCollector.uiManager;
        audioManager = managerCollector.audioManager;
        playerCameraRotation = GameObject.FindWithTag("FpsCameraHandler").GetComponent<CinemachinePanTilt>();
        playerMovement = GetComponent<Player_Movement>();
        craftingTableManager = managerCollector.craftingTableManager;
        
    }
    void Update()
    {
        InteractionRaycasting();

        CloseChest_Action();
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


        if (Physics.Raycast(interactionRay.origin, interactionRay.direction, out interactionHitInfo, interactionRayLength, interactionlayerMask))
        {

            if (interactionHitInfo.transform.gameObject.layer == 7)
            {
                uiManager.TurnOnIndicator_PickFood();
                PickFood_Action();
            }
            else if (interactionHitInfo.transform.gameObject.layer == 8)
            {
                if (interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().isFriendly == false)
                {
                    uiManager.TurnOnIndicator_FeedAnimal();
                    FeedAnimal_Action();
                }
            }
            else if (interactionHitInfo.transform.gameObject.layer == 11)   
            {
                GameObject hitObject = interactionHitInfo.transform.gameObject;

                if (hitObject.transform.name == "Chest")
                {
                    uiManager.TurnOnIndicator_OpenChest();

                    OpenChest_Action();
                }
                else if (hitObject.transform.name == "CraftingTable")
                {
                    uiManager.TurnOnIndicator_CraftingTable();
                    CraftCamera_Action();
                }
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            backpack.ItemPickup(interactionHitInfo.transform.gameObject.GetComponent<Pickable_Object>());
        }

        return;
    }
    void FeedAnimal_Action()
    {
        if (Input.GetKeyDown(KeyCode.E)){
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

                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log($"Sheep was fed and used 1 sheep food");
                    }

                    interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().ChanceToGetFriendlyAfterFeeding();
                    audioManager.PlayFeedingAnimal();
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

                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Wolf was fed and used 1 wolf food");
                    }

                    interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().ChanceToGetFriendlyAfterFeeding();
                    audioManager.PlayFeedingAnimal();
                    break;

                case "Goat":

                    if (isDebugOn)
                    {
                        Debug.Log("Attempting to feed Goat");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("GoatFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if (isDebugOn)
                        {
                            Debug.Log("Backpack does not contain Goat food");
                            
                        }

                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Goat was fed and used 1 Goat food");
                    }

                    interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().ChanceToGetFriendlyAfterFeeding();
                    audioManager.PlayFeedingAnimal();
                    break;

                case "Rabbit":

                    if (isDebugOn)
                    {
                        Debug.Log("Attempting to feed Rabbit");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("RabbitFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if (isDebugOn)
                        {
                            Debug.Log("Backpack does not contain Rabbit food");
                            
                        }

                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Rabbit was fed and used 1 Rabbit food");
                    }

                    interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().ChanceToGetFriendlyAfterFeeding();
                    audioManager.PlayFeedingAnimal();
                    break;

                case "Bear":

                    if (isDebugOn)
                    {
                        Debug.Log("Attempting to feed Bear");
                    }

                    slotToCheck = backpack.CheckIfBackpackHasItem("BearFood");

                    if (slotToCheck == null || slotToCheck.GetSlotValue() == 0)
                    {
                        if (isDebugOn)
                        {
                            Debug.Log("Backpack does not contain Bear food");
                            
                        }

                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Bear was fed and used 1 Bear food");
                    }

                    interactionHitInfo.transform.gameObject.GetComponent<AnimalBlackboard_Base>().ChanceToGetFriendlyAfterFeeding();
                    audioManager.PlayFeedingAnimal();
                    break;
            }      
        }
    }
    void OpenChest_Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            originalPanAxis = playerCameraRotation.PanAxis.Value;
            originalTilAxis = playerCameraRotation.TiltAxis.Value;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerMovement.enabled = false;
            playerCameraRotation.enabled = false;

            Inventory_Manager.Instance.OpenChest();
            Inventory_Manager.Instance.OpenBackpack();

            playerMovement.StopPlayer();
        }
    }
    void CloseChest_Action()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerCameraRotation.PanAxis.Value = originalPanAxis;
            playerCameraRotation.TiltAxis.Value = originalTilAxis;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.enabled = true;
            playerCameraRotation.enabled = true;

            Inventory_Manager.Instance.CloseChest();
            Inventory_Manager.Instance.CloseBackpack();
        }
    }
    void CraftCamera_Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            craftingTableManager.CraftItem("Camera");
        }
    }
    void SetCameraOnAnimal_Action()
    {
        
    }
 
    // UTILITIES


    // REFERENCES

    // DEBUGGIN

}
