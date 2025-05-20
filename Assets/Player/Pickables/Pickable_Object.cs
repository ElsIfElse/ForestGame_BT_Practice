using UnityEngine;

public class Pickable_Object : MonoBehaviour
{
    [SerializeField] Pickable_Data pickableData;

    void Start()
    {
        pickableName = pickableData.pickableName;
        pickableValue = pickableData.pickableValue;
        pickableImage = pickableData.pickableImage;
    }

    [SerializeField] string pickableName;
    public string GetPickableName()
    {
        return pickableName;
    }
    public void SetPickableName(string newName)
    {
        pickableName = newName;
    }

    [SerializeField] int pickableValue = 1;
    public int GetPickableValue()
    {
        return pickableValue;
    }
    public void SetPickableValue(int newValue)
    {
        pickableValue = newValue;
    }

    [SerializeField] Sprite pickableImage;
    public Sprite GetPickableImage()
    {
        return pickableImage;
    }
    public void SetPickableImage(Sprite newImage)
    {
        pickableImage = newImage;
    }
}
