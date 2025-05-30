using System;
using System.Collections;
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
    Feedback_Notifications notifications;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        uiManager = managerCollector.uiManager;
        audioManager = managerCollector.audioManager;
        playerCameraRotation = GameObject.FindWithTag("FpsCameraHandler").GetComponent<CinemachinePanTilt>();
        playerMovement = GetComponent<Player_Movement>();
        craftingTableManager = managerCollector.craftingTableManager;
        notifications = managerCollector.notifications;

    }
    void Update()
    {
        InteractionRaycasting();

        CloseChest_Action();
        CloseCraftingTable_Action();
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

        // Debug.DrawRay(interactionRay.origin, interactionRay.direction * interactionRayLength, Color.red);


        if (Physics.Raycast(interactionRay.origin, interactionRay.direction, out interactionHitInfo, interactionRayLength, interactionlayerMask))
        {
            GameObject currentObject = interactionHitInfo.transform.gameObject;

            // FOOD
            if (currentObject.layer == 7)
            {
                uiManager.TurnOnIndicator_PickFood();
                PickFood_Action();
            }

            // ANIMAL
            else if (currentObject.layer == 8 && currentObject.GetComponent<AnimalBlackboard_Base>().isHome == false)
            {
                if (currentObject.GetComponent<AnimalBlackboard_Base>().isFriendly == false)
                {
                    uiManager.TurnOnIndicator_FeedAnimal();
                    FeedAnimal_Action();
                }
                else if (currentObject.GetComponent<AnimalBlackboard_Base>().hasCamera == false)
                {
                    uiManager.TurnOnIndicator_SetupCamera();
                    SetCameraOnAnimal_Action(currentObject);
                }
            }

            // INTERACTABLE
            else if (currentObject.layer == 11)
            {
                if (currentObject.transform.name == "Chest")
                {
                    uiManager.TurnOnIndicator_OpenChest();

                    OpenChest_Action();
                }
                else if (currentObject.transform.name == "CraftingTable")
                {
                    uiManager.TurnOnIndicator_CraftingTable();
                    OpenCraftingTable_Action();
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
        if (Input.GetKeyDown(KeyCode.E))
        {
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

                        notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Sheep food");
                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log($"Sheep was fed and used 1 sheep food");
                    }

                    notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, "-1 Sheep Food");

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

                        notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Wolf food");
                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Wolf was fed and used 1 wolf food");
                    }

                    notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, "-1 Wolf Food");

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

                        notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Goat food");
                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Goat was fed and used 1 Goat food");
                    }

                    notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, "-1 Goat Food");

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

                        notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Rabbit food");
                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Rabbit was fed and used 1 Rabbit food");
                    }

                    notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, "-1 Rabbit Food");

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

                        notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Bear food");
                        audioManager.PlayCantDoIt();
                        return;
                    }

                    slotToCheck.DecreaseSlotValueBy(1);
                    slotToCheck.SetSlotUi();

                    if (isDebugOn)
                    {
                        Debug.Log("Bear was fed and used 1 Bear food");
                    }

                    notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, "-1 Bear Food");

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
    public void CraftCamera_Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            craftingTableManager.CraftItem("Camera");
        }
    }
    void OpenCraftingTable_Action()
    {
        if (Input.GetKeyDown(KeyCode.E) && !uiManager.craftingTable_UI.activeSelf)
        {
            originalPanAxis = playerCameraRotation.PanAxis.Value;
            originalTilAxis = playerCameraRotation.TiltAxis.Value;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMovement.enabled = false;
            playerCameraRotation.enabled = false;

            uiManager.TurnOnCraftingTable();
        }
    }
    void CloseCraftingTable_Action()
    {
        if (uiManager.craftingTable_UI.activeSelf && Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerCameraRotation.PanAxis.Value = originalPanAxis;
            playerCameraRotation.TiltAxis.Value = originalTilAxis;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.enabled = true;
            playerCameraRotation.enabled = true;
            uiManager.TurnOffCraftingTable();
        }
    }

    void SetCameraOnAnimal_Action(GameObject animalObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (animalObject.GetComponent<AnimalBlackboard_Base>().isFriendly == false)
            {
                notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "Animal is not friendly");
            }
            else if (backpack.CheckIfBackpackHasItem("Camera") == false)
            {
                notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "You have no Camera");
            }
            
            if (animalObject.GetComponent<AnimalBlackboard_Base>().isFriendly == true && backpack.CheckIfBackpackHasItem("Camera") == true)
            {
                animalObject.GetComponent<AnimalBlackboard_Base>().PlaceCameraOnAnimal();
                backpack.RemoveItem("Camera");
                StartCoroutine(SetupCameraSoundHelper_Coroutine());
            }
            else
            {
                audioManager.PlayCantDoIt();
            }
        }
    }
    
    IEnumerator SetupCameraSoundHelper_Coroutine()
    {   
        audioManager.PlaySetupCamera_01();
        yield return new WaitForSeconds(1.2f);
        audioManager.PlaySetupCamera_02();
    }
 
    // UTILITIES


    // REFERENCES

    // DEBUGGIN

}
