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
    public string GetPickableName() {
        return pickableName;
    }

    [SerializeField] int pickableValue = 1;
    public int GetPickableValue() {
        return pickableValue;
    }

    [SerializeField] Sprite pickableImage;
    public Sprite GetPickableImage() {
        return pickableImage;
    }
}
