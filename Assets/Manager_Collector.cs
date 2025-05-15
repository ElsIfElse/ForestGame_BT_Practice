using UnityEngine;

public class Manager_Collector : MonoBehaviour
{
    
    [HideInInspector] public UI_Manager uiManager;
    [HideInInspector] public World_Status worldStatus;
    [HideInInspector] public Spawn_Manager spawnManager;
    [HideInInspector] public Audio_Manager audioManager;
    [HideInInspector] public Camera_Handler cameraHandler;


    void Awake()
    {
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<Spawn_Manager>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UI_Manager>(); 
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<Audio_Manager>();
        cameraHandler = GameObject.FindWithTag("CameraHandler").GetComponent<Camera_Handler>();
    }
}
