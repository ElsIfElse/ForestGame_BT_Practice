using UnityEngine;



[CreateAssetMenu(fileName = "Food", menuName = "Pickable Objects", order = 0)]
public class Pickable_Data : ScriptableObject {
    public string pickableName;
    public int pickableValue;
    public Sprite pickableImage;
}
