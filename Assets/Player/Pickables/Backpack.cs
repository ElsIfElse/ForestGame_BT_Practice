using Unity.VisualScripting;
using UnityEngine;

public class Backpack : Storage
{
    protected override void Start()
    {
        base.Start();
        storageCapacity = 10;
        storageSlotNumber = 4;
        // storageSlots = new BackPack_Slot[storageSlotNumber];
    }
    public void ItemPickup(Pickable_Object pickable)
    {
        if (StorageValueSum() >= StorageCapacity())
        {
            Debug.Log("Backpack is full");
            audioManager.PlayCantDoIt();
            notifications.CreateMessageObject(Feedback_Notifications.messageTypes.Interaction_Fail, "Backpack is full");
            return;
        }

        // If the item type is present already
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = StorageSlots()[i] as BackPack_Slot;
            if (checkedSlot == null) Debug.Log("Slot is null");
            if (pickable == null) Debug.Log("Pickable is null");

            if (checkedSlot.GetSlotName() == pickable.GetPickableName())
            {
                checkedSlot.SetSlot(pickable.GetPickableName(), checkedSlot.GetSlotValue() + pickable.GetPickableValue(), pickable.GetPickableImage());
                audioManager.PlayPickSound();
                return;
            }
        }

        // If there is an empty slot
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = StorageSlots()[i] as BackPack_Slot;

            if (checkedSlot.GetSlotValue() == 0)
            {
                if (checkedSlot == null) Debug.Log("Slot is null");
                if (pickable == null) Debug.Log("Pickable is null");

                Debug.Log(pickable.GetPickableName() + ", " + pickable.GetPickableValue() + ", " + pickable.GetPickableImage());

                checkedSlot.SetSlot(pickable.GetPickableName(), checkedSlot.GetSlotValue() + pickable.GetPickableValue(), pickable.GetPickableImage());
                audioManager.PlayPickSound();
                return;
            }
        }

        Debug.Log("No empty slot in backpack");
        audioManager.PlayCantDoIt();
        return;
    }

    public bool IsBackpackFull()
    {
        return StorageValueSum() >= StorageCapacity();
    }
    public BackPack_Slot CheckIfBackpackHasItem(string itemName)
    {
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            if (StorageSlots()[i].GetSlotName() == itemName)
            {
                return StorageSlots()[i] as BackPack_Slot;
            }
        }
        return null;
    }
    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            BackPack_Slot currentSlot = storageSlots[i] as BackPack_Slot;
            if (currentSlot.GetSlotName() == itemName)
            {
                currentSlot.DecreaseSlotValueBy(1);

                if (currentSlot.GetSlotValue() == 0)
                {
                    currentSlot.EmptySlot();
                }

                currentSlot.SetSlotUi();
                Debug.Log(itemName.ToUpper() + " was removed from backpack");
                return;
            }
        }
        
        Debug.Log("Item was not found in backpack");
    }
}
