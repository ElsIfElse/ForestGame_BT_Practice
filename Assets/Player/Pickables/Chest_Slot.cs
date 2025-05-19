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
            BackPack_Slot checkedBackpackSlot = backpack.StorageSlots()[i] as BackPack_Slot;

            if (checkedBackpackSlot.GetSlotName() == GetSlotName())
            {

                checkedBackpackSlot.SetSlot(GetSlotName(), checkedBackpackSlot.GetSlotValue() + 1, GetSlotImage());

                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }

                SetSlotUi();
                return;
            }
        }

        // If there is an empty slot
        for (int i = 0; i < backpack.StorageSlots().Length; i++)
        {
            BackPack_Slot checkedBackpackSlot = backpack.StorageSlots()[i] as BackPack_Slot;

            if (checkedBackpackSlot.GetSlotValue() == 0)
            {
                checkedBackpackSlot.SetSlot(GetSlotName(), checkedBackpackSlot.GetSlotValue() + 1, GetSlotImage());

                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }

                SetSlotUi();
                return;
            }
        }

        Debug.Log("No empty slot in backpack");
        return;
    }
}
