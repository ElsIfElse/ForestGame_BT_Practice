using UnityEngine;
using UnityEngine.Events;

public class Inventory_Manager : MonoBehaviour
{   
    public static Inventory_Manager Instance { get; private set; }

    bool isBackpackOpen = false;
    bool isChestOpen = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Only allow one backpack
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        backpack_UI.SetActive(false);
        chest_Ui.SetActive(false);
    }

    [SerializeField] GameObject backpack_UI;
    [SerializeField] GameObject chest_Ui;
    public UnityEvent openingInventoryEvent = new UnityEvent();

    public void OpenChest()
    {
        chest_Ui.SetActive(true);
        isChestOpen = true;
    }
    public void CloseChest()
    {
        chest_Ui.SetActive(false);
        isChestOpen = false;
    }
    public void OpenBackpack()
    {
        backpack_UI.SetActive(true);
        isBackpackOpen = true;
    }
    public void CloseBackpack()
    {
        backpack_UI.SetActive(false);
        isBackpackOpen = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isChestOpen)
        {
            backpack_UI.SetActive(!backpack_UI.activeSelf);
            // openingInventoryEvent.Invoke();
        }
    }
}
