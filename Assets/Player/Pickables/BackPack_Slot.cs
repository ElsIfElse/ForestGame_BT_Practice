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
            Chest_Slot checkedChestSlot = chest.StorageSlots()[i] as Chest_Slot;

            if (checkedChestSlot.GetSlotName() == GetSlotName())
            {
                Debug.Log($"Item was present in chest. Stacking {GetSlotName()}");

                checkedChestSlot.SetSlot(GetSlotName(), checkedChestSlot.GetSlotValue() + 1, GetSlotImage());

                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }

                checkedChestSlot.SetSlotUi();
                SetSlotUi();
                return;
            }

            Debug.Log("Item type is not present in chest");
        }

        // If there is an empty slot
        for (int i = 0; i < chest.StorageSlots().Length; i++)
        {
            Chest_Slot checkedChestSlot = chest.StorageSlots()[i] as Chest_Slot;

            if (checkedChestSlot.GetSlotValue() == 0 || checkedChestSlot.GetSlotName() == "Empty")
            {
                Debug.Log($"Empty slot found. Transfering {GetSlotName()}");

                checkedChestSlot.SetSlot(GetSlotName(), checkedChestSlot.GetSlotValue()+1, GetSlotImage());
                DecreaseSlotValueBy(1);

                if (GetSlotValue() == 0)
                {
                    EmptySlot();
                }

                checkedChestSlot.SetSlotUi();
                SetSlotUi();
                return;
            }
        }

        Debug.Log("No empty slot in chest");
        return;
    }
}
