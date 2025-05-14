using UnityEngine;

public class Manager_Collector : MonoBehaviour
{
    [HideInInspector]
    public UI_Manager uiManager;
    [HideInInspector]
    public World_Status worldStatus;
    [HideInInspector]
    public Spawn_Manager spawnManager;

    void Awake()
    {
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<Spawn_Manager>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UI_Manager>(); 
    }
}
