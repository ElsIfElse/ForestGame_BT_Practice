using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class Spawn_Manager : MonoBehaviour
{
    [Header("References")]
    public Camera_Handler cameraHandler;

    [Header("Sheep")]
    public GameObject sheepCollectorObject;
    public int sheepSpawnAmount;

    [Header("Wolf")]
    public GameObject wolfCollectorObject;
    public int wolfSpawnAmount;

    [Header("Rabbit")]
    public GameObject rabbitCollectorObject;
    public int rabbitSpawnAmount;

    [Header("Goat")]
    public GameObject goatCollectorObject;
    public int goatSpawnAmount;

    [Header("Bear")]
    public GameObject bearCollectorObject;
    public int bearSpawnAmount;

    int sheepAvoidancePriority = 1;
    int wolfAvoidancePriority = 1;
    int rabbitAvoidancePriority = 1;
    int goatAvoidancePriority = 1;
    int bearAvoidancePriority = 1;
    int idCounter = 0;

    [HideInInspector] public UnityEvent sheepSpawned;
    [HideInInspector] public UnityEvent wolfSpawned;
    Manager_Collector managerCollector;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
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

        float randomX = Random.Range(-30f,10f);
        float randomZ = Random.Range(-130f,-90f);

        Vector3 samplePos = new Vector3(randomX, 0, randomZ);
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(samplePos, out navHit, 20f, NavMesh.AllAreas))
        {
            samplePos = navHit.position;

            RaycastHit groundHit;
            if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f))
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

                cameraHandler.CollectAnimals();
                sheepAvoidancePriority++;
                managerCollector.worldStatus.AddSheep(sheep);
                idCounter++;
            };
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position for Sheep. RETRYING");
            SpawnSheep();
        }
    }
    void SpawnWolf()
    
    {
        float randomX = Random.Range(-30f,10f);
        float randomZ = Random.Range(-130f,-90f);

    Vector3 samplePos = new Vector3(randomX, 0, randomZ);
    NavMeshHit navHit;

    if (NavMesh.SamplePosition(samplePos, out navHit, 20f, NavMesh.AllAreas))
    {
        samplePos = navHit.position;

        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f))
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

            cameraHandler.CollectAnimals();
            wolfAvoidancePriority++;
            managerCollector.worldStatus.AddWolf(wolf);
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

    float randomX = Random.Range(-15f,-125f);
    float randomZ = Random.Range(-4f,-140f); // Start should be lower than end
    float randomY = 0f;

    Vector3 samplePos = new Vector3(randomX, 0, randomZ);
    NavMeshHit navHit;

    // Try to find nearest NavMesh position within a radius
    if (NavMesh.SamplePosition(samplePos, out navHit, 5f, NavMesh.AllAreas))
    {
        // Update position with closest NavMesh position
        samplePos = navHit.position;

        // Raycast from above to get correct terrain Y height
        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f))
        {
            randomY = groundHit.point.y;
            samplePos.y = randomY;
        }

        // Spawn the rabbit at the correct location
        Addressables.InstantiateAsync("Rabbit", samplePos, Quaternion.identity).Completed += (handle) =>
        {
            GameObject rabbit = handle.Result;
            rabbit.name = "Rabbit";
            rabbit.GetComponent<Animal_BaseClass>().animalId = idCounter;
            rabbit.transform.parent = rabbitCollectorObject.transform;
            rabbit.GetComponent<NavMeshAgent>().avoidancePriority = rabbitAvoidancePriority;
            cameraHandler.CollectAnimals();
            rabbitAvoidancePriority++;

            managerCollector.worldStatus.AddRabbit(rabbit);
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

    float randomX = Random.Range(-15f,-125f);
    float randomZ = Random.Range(-4f,-140f);

    Vector3 samplePos = new Vector3(randomX, 0, randomZ);
    NavMeshHit navHit;

    if (NavMesh.SamplePosition(samplePos, out navHit, 10f, NavMesh.AllAreas))
    {
        samplePos = navHit.position;

        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f))
        {
            samplePos.y = groundHit.point.y;
        }

        Addressables.InstantiateAsync("Goat", samplePos, Quaternion.identity).Completed += (handle) =>
        {
            GameObject goat = handle.Result;
            goat.name = "Goat";
            goat.GetComponent<Animal_BaseClass>().animalId = idCounter;
            goat.GetComponent<NavMeshAgent>().avoidancePriority = goatAvoidancePriority;
            goat.transform.parent = goatCollectorObject.transform;

            cameraHandler.CollectAnimals();
            goatAvoidancePriority++;
            managerCollector.worldStatus.AddGoat(goat);
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
    float randomX = Random.Range(-15f,-125f);
    float randomZ = Random.Range(-4f,-140f);

    Vector3 samplePos = new Vector3(randomX, 0, randomZ);
    NavMeshHit navHit;

    if (NavMesh.SamplePosition(samplePos, out navHit, 10f, NavMesh.AllAreas))
    {
        samplePos = navHit.position;

        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(samplePos.x, 100f, samplePos.z), Vector3.down, out groundHit, 200f))
        {
            samplePos.y = groundHit.point.y;
        }

        Addressables.InstantiateAsync("Bear", samplePos, Quaternion.identity).Completed += (handle) =>
        {
            GameObject bear = handle.Result;
            bear.name = "Bear"; 
            bear.GetComponent<Animal_BaseClass>().animalId = idCounter;
            bear.GetComponent<NavMeshAgent>().avoidancePriority = bearAvoidancePriority;
            bear.transform.parent = bearCollectorObject.transform;

            cameraHandler.CollectAnimals();
            bearAvoidancePriority++;
            managerCollector.worldStatus.AddBear(bear);
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
