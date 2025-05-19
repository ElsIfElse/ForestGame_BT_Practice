using UnityEngine;

public class Backpack : Storage
{
    void Start()
    {
        storageCapacity = 10;
        storageSlotNumber = 4;
        // storageSlots = new BackPack_Slot[storageSlotNumber];
    }
    public void ItemPickup(Pickable_Object pickable)
    {
        if (StorageValueSum() >= StorageCapacity())
        {
            Debug.Log("Backpack is full"); 
            return;
        }

        // If the item type is present already
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = StorageSlots()[i] as BackPack_Slot;
            if(checkedSlot == null) Debug.Log("Slot is null");
            if(pickable == null) Debug.Log("Pickable is null");

            if (checkedSlot.GetSlotName() == pickable.GetPickableName())
            {
                checkedSlot.SetSlot(pickable.GetPickableName(), pickable.GetPickableValue(), pickable.GetPickableImage());
                return;
            }
        }

        // If there is an empty slot
        for (int i = 0; i < StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = StorageSlots()[i] as BackPack_Slot;

            if (checkedSlot.GetSlotValue() == 0)
            {
                if(checkedSlot == null) Debug.Log("Slot is null");
                if(pickable == null) Debug.Log("Pickable is null");

                Debug.Log(pickable.GetPickableName() + ", " + pickable.GetPickableValue() + ", " + pickable.GetPickableImage());

                checkedSlot.SetSlot(pickable.GetPickableName(),pickable.GetPickableValue(),pickable.GetPickableImage());
                return;
            }
        }

        Debug.Log("No empty slot in backpack");
        return;
    }
}
