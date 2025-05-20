using Unity.VisualScripting;
using UnityEngine;

public class Chest : Storage
{
    protected override void Start()
    {
        base.Start();
        storageCapacity = 20;
        storageSlotNumber = 5;
    }
        
    // storageSlots = new Chest_Slot[storageSlotNumber];
    public BackPack_Slot CheckIfChestHasItem(string itemName)
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
    
}
