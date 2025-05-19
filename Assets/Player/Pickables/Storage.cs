using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    protected int storageCapacity;
    public int StorageCapacity()
    {
        return storageCapacity;
    }

    protected int storageSlotNumber;
    public int StorageSlotNumber()
    {
        return storageSlotNumber;
    }

    [SerializeField] protected Slot_Base[] storageSlots;

    public int StorageValueSum()
    {
        int sum = 0;
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (storageSlots[i] != null)
            {
                sum += storageSlots[i].GetSlotValue();
            }
        }
        return sum;
    }
    public Slot_Base[] StorageSlots()
    {
        return storageSlots;
    }

}
