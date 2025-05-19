using UnityEngine;

public class Chest_Slot : Slot_Base
{
    public void ChestToBackpack()
    {
        // If Backpack Full
        if (backpack.StorageValueSum() >= backpack.StorageCapacity())
        {
            Debug.Log("Backpack is full");
            return;
        }

        // If the item type is present already
        for (int i = 0; i < backpack.StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = backpack.StorageSlots()[i] as BackPack_Slot;

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
        for (int i = 0; i < backpack.StorageSlots().Length; i++)
        {
            BackPack_Slot checkedSlot = backpack.StorageSlots()[i] as BackPack_Slot;

            if (checkedSlot.GetSlotValue() == 0)
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

        Debug.Log("No empty slot in backpack");
        return;
    }
}
