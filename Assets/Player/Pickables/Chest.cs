using UnityEngine;

public class Chest : Storage
{
    void Start()
    {
        storageCapacity = 20;
        storageSlotNumber = 5;
        storageSlots = new Chest_Slot[storageSlotNumber];
    }
}
