using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Base : MonoBehaviour
{
    // References
    [SerializeField] protected Backpack backpack;
    [SerializeField] protected Chest chest;

    // Set Get
    [Space]
    [Header("Slot Info")]
    [SerializeField] string slotName = "Empty";
    public string GetSlotName()
    {
        return slotName;
    }
    public void SetSlotName(string newName)
    {
        slotName = newName;
    }

    [SerializeField] int slotValue = 0;
    public int GetSlotValue()
    {
        return slotValue;
    }
    public void SetValue(int value)
    {
        slotValue = value;
    }
    public void DecreaseSlotValueBy(int value)
    {
        slotValue -= value;

        if (slotValue <= 0)
        {
            EmptySlot();
        }
    }

    [SerializeField] Sprite slotImage = null;
    public Sprite GetSlotImage()
    {
        return slotImage;
    }
    public void SetSlotImage(Sprite newImage)
    {
        slotImage = newImage;
    }

    // Utilities
    public void SetSlot(string newName, int newValue, Sprite newImage)
    {
        SetSlotImage(newImage);
        SetSlotName(newName);
        SetValue(newValue);

        SetSlotUi();

        // slotButton.image.sprite = newImage;
        // // slotNameText.text = newName;
        // slotValueText.text = GetSlotValue().ToString();  
    }
    public void SetSlotUi()
    {

        if (slotButton == null) slotButton = GetComponent<Button>();
        if (slotValueText == null) slotValueText = GetComponentInChildren<TextMeshProUGUI>();

        if (slotButton != null)
        {
            slotButton.image.sprite = slotImage;
        }

        if (slotValueText != null)
        {
            slotValueText.text = GetSlotValue().ToString();
        }

        // slotNameText.text = slotName;
    }
    public void EmptySlot()
    {
        slotName = "Empty";
        slotValue = 0;
        slotImage = null;

        SetSlotUi();
    }

    // UI
    [SerializeField] Button slotButton;
    // [SerializeField] TextMeshProUGUI slotNameText;
    [SerializeField] TextMeshProUGUI slotValueText;

    void Start()
    {
        slotButton = GetComponent<Button>();
        slotValueText = GetComponentInChildren<TextMeshProUGUI>();
        Inventory_Manager.Instance.openingInventoryEvent.AddListener(SetSlotUi);


        // slotNameText = GetComponentInChildren<TextMeshProUGUI>();

    }

    public void GetReferences()
    {
        if(slotButton == null) slotButton = GetComponent<Button>();
        if(slotValueText == null) slotValueText = GetComponentInChildren<TextMeshProUGUI>();
    }

}
