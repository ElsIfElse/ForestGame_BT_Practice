using UnityEngine;

public class BackPack_Slot : Slot_Base
{
    public void BackpackToChest()
    {
        // If chest is full
        if (chest.StorageValueSum() >= chest.StorageCapacity())
        {
            Debug.Log("Chest is full");
            return;
        }

        // If the item type is present already
        for (int i = 0; i < chest.StorageSlots().Length; i++)
        {
            Chest_Slot checkedSlot = chest.StorageSlots()[i] as Chest_Slot;

            if (checkedSlot.GetSlotName() == GetSlotName())
            {
                checkedSlot.SetSlotName(GetSlotName());
                checkedSlot.SetSlotImage(GetSlotImage());
                checkedSlot.IncreaseSlotValueBy(1);

                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }
                return;
            }
        }

        // If there is an empty slot
        for (int i = 0; i < chest.StorageSlots().Length; i++)
        {
            Chest_Slot checkedSlot = chest.StorageSlots()[i] as Chest_Slot;

            if (checkedSlot.GetSlotValue() == 0 || checkedSlot.GetSlotName() == "Empty")
            {
                checkedSlot.SetSlot(GetSlotName(), 1, GetSlotImage());

                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }
                return;
            }
        }

        Debug.Log("No empty slot in chest");
        return;
    }
}
