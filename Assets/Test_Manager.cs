using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.AI;

public class Test_Manager : MonoBehaviour
{
    [Header("Sheep")]
    public GameObject sheepPrefab;
    public GameObject sheepCollectorObject;
    [Header("Wolf")]
    public GameObject wolfPrefab;
    public GameObject wolfCollectorObject;
    
    [Header("References")]
    public Camera_Handler cameraHandler;
    
    int sheepAvoidancePriority = 1;
    int wolfAvoidancePriority = 1;

    void Update()
    {
        // SpawnSheep();
    }

    void SpawnSheep(){
        if(Input.GetKeyDown(KeyCode.S)){
            float randomX = Random.Range(-10,10);
            float randomZ = Random.Range(-10,10);
            Vector3 spawnLoc = new Vector3(randomX,0,randomZ);

            GameObject sheep = Instantiate(sheepPrefab,spawnLoc,Quaternion.identity);

            sheep.GetComponent<NavMeshAgent>().avoidancePriority = sheepAvoidancePriority;
            sheep.transform.parent = sheepCollectorObject.transform;

            cameraHandler.CollectAnimals();
            sheepAvoidancePriority++;
        }
        if(Input.GetKeyDown(KeyCode.W)){
            float randomX = Random.Range(-30,0);
            float randomZ = Random.Range(10,20);
            Vector3 spawnLoc = new Vector3(randomX,0,randomZ);

            GameObject wolf  = Instantiate(wolfPrefab,spawnLoc,Quaternion.identity);

            wolf.transform.parent = wolfCollectorObject.transform;
            wolf.GetComponent<NavMeshAgent>().avoidancePriority = wolfAvoidancePriority;
            cameraHandler.CollectAnimals();

            wolfAvoidancePriority++;
        }
    }

}
