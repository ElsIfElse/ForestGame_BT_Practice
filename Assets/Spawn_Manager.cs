using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UTILITIES;
public class Spawn_Manager : MonoBehaviour
{
    [Header("References")]
    public Camera_Handler cameraHandler;

    [Header("Sheep")]
    public GameObject sheepCollectorObject;
    public int sheepSpawnAmount;
    public GameObject spawnArea_Sheep;

    [Header("Wolf")]
    public GameObject wolfCollectorObject;
    public int wolfSpawnAmount;
    public GameObject spawnArea_Wolf;

    [Header("Rabbit")]
    public GameObject rabbitCollectorObject;
    public int rabbitSpawnAmount;
    public GameObject spawnArea_Rabbit;

    [Header("Goat")]
    public GameObject goatCollectorObject;
    public int goatSpawnAmount;
    public GameObject spawnArea_Goat;

    [Header("Bear")]
    public GameObject bearCollectorObject;
    public int bearSpawnAmount;
    public GameObject spawnArea_Bear;

    int sheepAvoidancePriority = 1;
    int wolfAvoidancePriority = 1;
    int rabbitAvoidancePriority = 1;
    int goatAvoidancePriority = 1;
    int bearAvoidancePriority = 1;
    int idCounter = 0;

    [HideInInspector] public UnityEvent sheepSpawned;
    [HideInInspector] public UnityEvent wolfSpawned;
    Manager_Collector managerCollector;
    Animal_Collection animalCollection;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        animalCollection = managerCollector.animalCollection;
        cameraHandler = managerCollector.cameraHandler;
        Addressables.InitializeAsync();

        SpawnAtTheBeginningOfBuild(sheepSpawnAmount,wolfSpawnAmount,rabbitSpawnAmount,goatSpawnAmount,bearSpawnAmount);
    }
    void SpawnKeys(){
        if(Input.GetKeyDown(KeyCode.S)){
            SpawnSheep();
        }
        if(Input.GetKeyDown(KeyCode.W)){
            SpawnWolf();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            SpawnRabbit();
        }
        if(Input.GetKeyDown(KeyCode.G)){
            SpawnGoat();
        }
    }
    
    void SpawnSheep(){
 
        Vector3 samplePos = Utility_Collection.GetRandom_X_Z_LocationAtArea(spawnArea_Sheep);
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(samplePos, out navHit, 20f, NavMesh.AllAreas))
        {
            samplePos = navHit.position;

            RaycastHit groundHit;
            if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f,10))
            {
                samplePos.y = groundHit.point.y;
            }

            Addressables.InstantiateAsync("Sheep", samplePos, Quaternion.identity).Completed += (handle) =>
            {
                GameObject sheep = handle.Result;
                sheep.name = "Sheep";
                sheep.GetComponent<AnimalBlackboard_Base>().animalId = idCounter;
                sheep.GetComponent<NavMeshAgent>().avoidancePriority = sheepAvoidancePriority;
                sheep.transform.parent = sheepCollectorObject.transform;

                
                sheepAvoidancePriority++;
                animalCollection.AddSheep(sheep);
                animalCollection.AddAnimalTo_AllAnimals(sheep);
                idCounter++;
            };
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position for Sheep. RETRYING");
            SpawnSheep();
        }
    }
    void SpawnWolf(){

        Vector3 samplePos = Utility_Collection.GetRandom_X_Z_LocationAtArea(spawnArea_Wolf);
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(samplePos, out navHit, 20f, NavMesh.AllAreas))
        {
            samplePos = navHit.position;

            RaycastHit groundHit;
            if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f,10))
            {
                samplePos.y = groundHit.point.y;
            }

            Addressables.InstantiateAsync("Wolf", samplePos, Quaternion.identity).Completed += (handle) =>
            {
                GameObject wolf = handle.Result;
                wolf.name = "Wolf";
                wolf.GetComponent<AnimalBlackboard_Base>().animalId = idCounter;
                wolf.GetComponent<NavMeshAgent>().avoidancePriority = wolfAvoidancePriority;
                wolf.transform.parent = wolfCollectorObject.transform;

                
                wolfAvoidancePriority++;
                animalCollection.AddWolf(wolf);
                animalCollection.AddAnimalTo_AllAnimals(wolf);
                idCounter++;
            };
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position for Wolf. RETRYING");
            SpawnWolf();
        }
    }    
    void SpawnRabbit(){

    Vector3 samplePos = Utility_Collection.GetRandom_X_Z_LocationAtArea(spawnArea_Rabbit);    
    NavMeshHit navHit;

    // Try to find nearest NavMesh position within a radius
    if (NavMesh.SamplePosition(samplePos, out navHit, 5f, NavMesh.AllAreas))
    {
        // Update position with closest NavMesh position
        samplePos = navHit.position;

        // Raycast from above to get correct terrain Y height
        RaycastHit groundHit;
        
        if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f,10))
        {
            samplePos.y = groundHit.point.y;
        }

        // Spawn the rabbit at the correct location
        Addressables.InstantiateAsync("Rabbit", samplePos, Quaternion.identity).Completed += (handle) =>
        {
            GameObject rabbit = handle.Result;
            rabbit.name = "Rabbit";
            rabbit.GetComponent<AnimalBlackboard_Base>().animalId = idCounter;
            rabbit.transform.parent = rabbitCollectorObject.transform;
            rabbit.GetComponent<NavMeshAgent>().avoidancePriority = rabbitAvoidancePriority;
            
            rabbitAvoidancePriority++;

            animalCollection.AddRabbit(rabbit);
            animalCollection.AddAnimalTo_AllAnimals(rabbit);
            idCounter++;
        };
    }
    else
    {
        Debug.LogWarning("Failed to find NavMesh position for Rabbit. RETRYING");
        SpawnRabbit();
    }
}
    void SpawnGoat(){

        Vector3 samplePos = Utility_Collection.GetRandom_X_Z_LocationAtArea(spawnArea_Goat);  
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(samplePos, out navHit, 10f, NavMesh.AllAreas))
        {
            samplePos = navHit.position;

            RaycastHit groundHit;
            if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f,10))
            {
                samplePos.y = groundHit.point.y;
            }

            Addressables.InstantiateAsync("Goat", samplePos, Quaternion.identity).Completed += (handle) =>
            {
                GameObject goat = handle.Result;
                goat.name = "Goat";
                goat.GetComponent<AnimalBlackboard_Base>().animalId = idCounter;
                goat.GetComponent<NavMeshAgent>().avoidancePriority = goatAvoidancePriority;
                goat.transform.parent = goatCollectorObject.transform;

                
                goatAvoidancePriority++;
                animalCollection.AddGoat(goat);
                animalCollection.AddAnimalTo_AllAnimals(goat);
                idCounter++;
            };
    }
    else
    {
        Debug.LogWarning("Failed to find NavMesh position for Goat. RETRYING");
        SpawnGoat();
    }
}
    void SpawnBear()
    {
        Vector3 samplePos = Utility_Collection.GetRandom_X_Z_LocationAtArea(spawnArea_Bear); 
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(samplePos, out navHit, 10f, NavMesh.AllAreas))
        {
            samplePos = navHit.position;

            RaycastHit groundHit;
            if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f,10))
            {
                samplePos.y = groundHit.point.y;
            }

            Addressables.InstantiateAsync("Bear", samplePos, Quaternion.identity).Completed += (handle) =>
            {
                GameObject bear = handle.Result;
                bear.name = "Bear"; 
                bear.GetComponent<AnimalBlackboard_Base>().animalId = idCounter;
                bear.GetComponent<NavMeshAgent>().avoidancePriority = bearAvoidancePriority;
                bear.transform.parent = bearCollectorObject.transform;

                
                bearAvoidancePriority++;
                animalCollection.AddBear(bear);
                animalCollection.AddAnimalTo_AllAnimals(bear);
                idCounter++;
            };
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position for Goat. RETRYING");
            SpawnGoat();
        }
    }
    void SpawnAtTheBeginningOfBuild(int sheepSpawnAmount, int wolfSpawnAmount, int rabbitSpawnAmount, int goatSpawnAmount, int bearSpawnAmount)
    {
        for (int i = 0; i < sheepSpawnAmount; i++)
        {
            SpawnSheep();
        }

        for (int i = 0; i < wolfSpawnAmount; i++)
        {
            SpawnWolf();
        }

        for (int i = 0; i < rabbitSpawnAmount; i++)
        {
            SpawnRabbit();
        }

        for (int i = 0; i < goatSpawnAmount; i++)
        {
            SpawnGoat();
        }

        for (int i = 0; i < bearSpawnAmount; i++)
        {
            SpawnBear();
        }
    }
}
