using UnityEngine;

public class Manager_Collector : MonoBehaviour
{

    [HideInInspector] public UI_Manager uiManager;
    [HideInInspector] public World_Status worldStatus;
    [HideInInspector] public Spawn_Manager spawnManager;
    [HideInInspector] public Audio_Manager audioManager;
    [HideInInspector] public Camera_Handler cameraHandler;
    [HideInInspector] public Fps_Camera_Handler fpsCameraHandler;
    [HideInInspector] public GameObject inventoryManager;
    [HideInInspector] public Backpack backpack;
    [HideInInspector] public Chest chest;
    [HideInInspector] public CraftingTable_Manager craftingTableManager;
    [HideInInspector] public Animal_Collection animalCollection;
    [HideInInspector] public Feedback_Notifications notifications;





    void Awake()
    {
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<Spawn_Manager>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UI_Manager>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<Audio_Manager>();
        cameraHandler = GameObject.FindWithTag("CameraHandler").GetComponent<Camera_Handler>();
        fpsCameraHandler = GameObject.FindWithTag("FpsCameraHandler").GetComponent<Fps_Camera_Handler>();

        craftingTableManager = GameObject.FindWithTag("CraftingTableManager").GetComponent<CraftingTable_Manager>();

        inventoryManager = GameObject.FindWithTag("InventoryManager");
        backpack = inventoryManager.GetComponent<Backpack>();
        chest = inventoryManager.GetComponent<Chest>();

        animalCollection = GameObject.FindWithTag("WorldStatus").GetComponent<Animal_Collection>();
        notifications = GameObject.FindWithTag("WorldStatus").GetComponent<Feedback_Notifications>();
    }
    void Start()
    {
        if (uiManager == null)
        { 
            uiManager = GameObject.FindWithTag("UIManager").GetComponent<UI_Manager>();
        }
    }
}
