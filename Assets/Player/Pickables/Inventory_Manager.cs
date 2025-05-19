using UnityEngine;
using UnityEngine.Events;

public class Inventory_Manager : MonoBehaviour
{   
 public static Inventory_Manager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Only allow one backpack
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: remove if not needed across scenes
    }

    private void Start()
    {
        backpack_UI.SetActive(false);
        
    }

    [SerializeField] GameObject backpack_UI;
    public UnityEvent openingInventoryEvent = new UnityEvent();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            backpack_UI.SetActive(!backpack_UI.activeSelf);
            openingInventoryEvent.Invoke();
        }
    }
}
