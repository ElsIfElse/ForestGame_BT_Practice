using UnityEngine;

public class CraftingTable_Manager : MonoBehaviour
{
    Manager_Collector managerCollector;
    Backpack backpack;
    [SerializeField] Sprite camera_Sprite;
    Audio_Manager audioManager;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        backpack = managerCollector.backpack;
        audioManager = managerCollector.audioManager;
    }
    public void CraftItem(string itemName)
    {
        if (backpack.IsBackpackFull())
        {
            Debug.Log("Backpack is full");
            audioManager.PlayCantDoIt();
            return;
        }

        Pickable_Object newItem = Pickable_Object_Creation(itemName);
        backpack.ItemPickup(newItem);

    }

    Pickable_Object Pickable_Object_Creation(string itemName)
    {
        Pickable_Object craftedItem;

        switch (itemName)
        {
            case "Camera":
                craftedItem = new Pickable_Object();
                craftedItem.SetPickableName("Camera");
                craftedItem.SetPickableValue(1);
                craftedItem.SetPickableImage(camera_Sprite);

                return craftedItem;
            
            case "SheepFood":
                return null;
        }

        return null;
    }
    

}
