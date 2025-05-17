using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class AnimalBlackboard_Base : MonoBehaviour
{

    // Animal Description
    public string animalName;
    public string animalBreed;
    public string animalType;
    public int animalId;

    // Tree conditions
    public bool isDay;
    public bool isHome;

    // Homes and Areas
    [SerializeField] protected GameObject home;
    protected GameObject[] homes;

    [SerializeField] protected GameObject wanderingArea;
    protected GameObject[] wanderingAreas;

    // Animal Speeds
    protected float speed_Walking;
    protected float speed_Running;

    // Wandering Helpers
    [SerializeField] protected bool isWandering = false;
    [SerializeField] protected bool isIdle = false;
    protected bool hasWanderingLocation = false;
    [SerializeField] Vector3 currentWanderingLocation;
    UnityEvent gotWanderingLocation = new UnityEvent();
    float currentStuckTime = 3f;
    float maximumStuckTime = 3f;

    // References
    protected Animal_AnimatorBaseClass animator;
    protected Manager_Collector managerCollector;
    protected World_Status worldStatus;
    protected NavMeshAgent animalAgent;
    protected GameObject animalVisual;

    // String Arrays For Names
    public string[] names = {
        "Finn", "Sparky", "Oliver", "Bear", "Luna", "Maggie", "Max", "Charlie", "Daisy", "Buddy", "Rocky", "Lucy", "Cooper", "Ginger", "Simba", "Coco", "Duke", "Lola", "Hunter", "Sandy", "Molly", "Sam", "Toby", "Ruby", "Baxter", "Abby", "Rusty", "Gracie", "Bella", "Maverick", "Penny", "Jasper", "Cody", "Lily", "Odie", "Zoey", "Nala", "Sasha", "Maddie", "Murphy", "Bailey", "Tucker", "Emma", "Fiona", "Dakota", "Chloe", "Chase", "Lila", "Jackson", "Morgan", "Hank", "Sadie", "Gizmo", "Ava", "Jax", "Willow", "Riley", "Sophie", "Bentley", "Lacey", "Mason", "Lexi", "Katie", "Diesel", "Julia", "Hannah", "Titan", "Paisley", "Easton", "Lainey", "Bryce", "Ashley", "Colby", "Avery", "Kolby", "Skye", "Savannah", "Jace", "Cheyenne", "Jenna", "Kai", "Kayla", "Landry", "Leslie", "Mackenzie", "Mallory", "Micah", "Natalie", "Nicole", "Noah", "Paige", "Parker", "Quinn", "Rachel", "Reese", "Rylee", "Savannah", "Sierra", "Taylor", "Tatum", "Teagan", "Tessa", "Trey", "Trinity", "Tyson", "Vivian", "Waylon", "Wesley", "Wyatt", "Xander", "Yvonne", "Zachary"
    };

    public virtual void Start()
    {
        GetScriptReferences();
        InitializeAnimal();
    }

    // Initializer Functions
    void GetScriptReferences()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        worldStatus = managerCollector.worldStatus;
    }

    void InitializeAnimal()
    {
        SubscribeTo_WanderingEvent();
        GetAgent();
        GetAnimator();
        GetAnimalVisual();

        AssignAnimalBreed();
        AssignHome();
        AssignWanderingArea();
        AssignSpeeds();
        AssignRandomName();

        WalkSpeed();

    }
    void Update()
    {
        isDay = worldStatus.isDay;
    }

    void AssignSpeeds()
    {
        switch (animalBreed)
        {
            case "Sheep":
                speed_Walking = Random.Range(2, 5);
                speed_Running = Random.Range(6, 8);
                break;

            case "Wolf":
                speed_Walking = Random.Range(4, 6);
                speed_Running = Random.Range(7, 12);
                break;

            case "Rabbit":
                speed_Walking = Random.Range(2, 5);
                speed_Running = Random.Range(6, 9);
                break;

            case "Goat":
                speed_Walking = Random.Range(5, 7);
                speed_Running = Random.Range(8, 11);
                break;

            case "Bear":
                speed_Walking = Random.Range(4, 6);
                speed_Running = Random.Range(8, 12);
                break;
        }
    }
    void AssignHome()
    {
        switch (animalBreed)
        {
            case "Sheep":
                GameObject[] sheepHomes = GameObject.FindGameObjectsWithTag("SheepHome");
                GameObject sheepHome = sheepHomes[Random.Range(0, sheepHomes.Length)];
                home = sheepHome;
                break;

            case "Wolf":
                GameObject[] wolfHomes = GameObject.FindGameObjectsWithTag("WolfHome");
                if (wolfHomes.Length == 0)
                {
                    Debug.LogError("No wolf homes found RETRYING");
                    AssignHome();
                }
                GameObject wolfHome = wolfHomes[Random.Range(0, wolfHomes.Length)];
                home = wolfHome;
                break;

            case "Rabbit":
                GameObject[] rabbitHomes = GameObject.FindGameObjectsWithTag("RabbitHome");
                GameObject rabbitHome = rabbitHomes[Random.Range(0, rabbitHomes.Length)];
                home = rabbitHome;
                break;

            case "Goat":
                GameObject[] goatHomes = GameObject.FindGameObjectsWithTag("GoatHome");
                GameObject goatHome = goatHomes[Random.Range(0, goatHomes.Length)];
                home = goatHome;
                break;

            case "Bear":
                GameObject[] bearHomes = GameObject.FindGameObjectsWithTag("BearHome");
                GameObject bearHome = bearHomes[Random.Range(0, bearHomes.Length)];
                home = bearHome;
                break;
        }

        if (home == null)
        {
            Debug.Log("No home found for " + animalBreed);
        }
    }
    void AssignRandomName()
    {
        animalName = names[Random.Range(0, names.Length)];
    }
    void AssignWanderingArea()
    {
        switch (animalBreed)
        {
            case "Sheep":
                GameObject[] sheepWanderingAreas = GameObject.FindGameObjectsWithTag("Meadow");
                wanderingAreas = sheepWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Wolf":
                GameObject[] wolfWanderingAreas = GameObject.FindGameObjectsWithTag("WolfWanderingArea");
                wanderingAreas = wolfWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Rabbit":
                GameObject[] rabbitWanderingAreas = GameObject.FindGameObjectsWithTag("Meadow");
                wanderingAreas = rabbitWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Goat":
                GameObject[] goatWanderingAreas = GameObject.FindGameObjectsWithTag("GoatWanderingArea");
                wanderingAreas = goatWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Bear":
                GameObject[] bearWanderingAreas = GameObject.FindGameObjectsWithTag("BearWanderingArea");
                wanderingAreas = bearWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;
        }
    }
    void AssignAnimalBreed()
    {
        animalBreed = gameObject.name;
    }
    public void SetId(int id)
    {
        animalId = id;
    }
    void GetAnimalVisual()
    {
        animalVisual = gameObject.transform.Find("animalVisual").gameObject;
    }
    protected void SetAnimalType(string type)
    {
        animalType = type;
    }
    protected void SetAnimalBreed(string breed)
    {
        animalBreed = breed;
    }
    void GetAgent()
    {
        animalAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    void GetAnimator()
    {
        animator = gameObject.GetComponent<Animal_AnimatorBaseClass>();
    }

    // Wandering Functions
    void SubscribeTo_WanderingEvent()
    {
        gotWanderingLocation.AddListener(GoToWanderingLocation);
    }
    void GoToWanderingLocation()
    {
        WalkSpeed();
        animalAgent.SetDestination(currentWanderingLocation);
        animator.PlayWalk();
    }
    public void Wandering()
    {
        if (!isIdle && animalAgent.velocity.sqrMagnitude < 1f && hasWanderingLocation)
        {
            currentStuckTime -= Time.deltaTime;

            if (currentStuckTime <= 0f)
            {
                Debug.Log("Animal Stuck Time is up. Looking for new location for "+animalBreed);
                hasWanderingLocation = false;
                currentStuckTime = maximumStuckTime;
            }
        }
        else
        {
            currentStuckTime = maximumStuckTime;
        }

        if (!hasWanderingLocation)
        {
            hasWanderingLocation = true;
            isWandering = true;
            currentWanderingLocation = GetRandomLocationAtArea(wanderingArea);

            while (IsCloserThan(gameObject.transform.position, currentWanderingLocation, 5f))
            {
                currentWanderingLocation = GetRandomLocationAtArea(wanderingArea);
            }

            gotWanderingLocation.Invoke();
        }
        else
        {
            if (IsCloserThan(gameObject.transform.position, currentWanderingLocation, 2f) && isWandering && !isIdle)
            {
                isIdle = true;
                isWandering = false;
                StartCoroutine(WanderingHelper_Idle_Coroutine());
            }
        } 
    }
    IEnumerator WanderingHelper_Idle_Coroutine()
    {
        animator.PlayIdle();
        animalAgent.ResetPath();
        yield return new WaitForSeconds(3f);
        isIdle = false;
        hasWanderingLocation = false;
    }
    protected void ResetWanderingBools()
    {
        isIdle = false;
        isWandering = false;
        hasWanderingLocation = false;
    }

    // Tree Actions
    public void Stay()
    {
        StopAllCoroutines();
        animator.PlayIdle();
        // ResetWanderingBools();
        animalAgent.ResetPath();
    }
    public void GoHome()
    {
        // StopAllCoroutines();
        WalkSpeed();
        animalAgent.SetDestination(home.transform.position);
    }

    // Utilities
    protected void GetHomes(string animalBreed)
    {
        switch (animalBreed)
        {
            case "Wolf":
                homes = GameObject.FindGameObjectsWithTag("WolfHome");
                break;
            case "Sheep":
                homes = GameObject.FindGameObjectsWithTag("SheepHome");
                break;
            case "Rabbit":
                homes = GameObject.FindGameObjectsWithTag("RabbitHome");
                break;
            case "Goat":
                homes = GameObject.FindGameObjectsWithTag("GoatHome");
                break;
            case "Bear":
                homes = GameObject.FindGameObjectsWithTag("BearHome");
                break;
        }
    }
    protected Vector3 GetRandomLocationAtArea(GameObject area)
    {
        float areaCenterX = area.transform.position.x;
        float areaCenterZ = area.transform.position.z;

        float areaHalfWidth = area.transform.localScale.x / 2f;
        float areaHalfDepth = area.transform.localScale.z / 2f;

        // Generate random positions within the area
        float randomX = UnityEngine.Random.Range(areaCenterX - areaHalfWidth, areaCenterX + areaHalfWidth);
        float randomZ = UnityEngine.Random.Range(areaCenterZ - areaHalfDepth, areaCenterZ + areaHalfDepth);
        float randomY = 0;

        // Use the RANDOM positions for NavMesh sampling
        NavMeshHit hit;

        if (NavMesh.SamplePosition(new Vector3(randomX, 0, randomZ), out hit, 30f, NavMesh.AllAreas))
        {
            randomX = hit.position.x;
            randomZ = hit.position.z;
        }
        else
        {
            Debug.Log("NavMesh.SamplePosition failed. Going to alternative location.");
            return transform.position + new Vector3(5, 0, 5);
        }


        // Use the RANDOM positions for raycasting
        RaycastHit groundHit;

        if (Physics.Raycast(new Vector3(randomX, 50f, randomZ), Vector3.down, out groundHit, Mathf.Infinity))
        {
            randomY = groundHit.point.y;
        }

        return new Vector3(randomX, randomY, randomZ);
    }
    protected bool IsCloserThan(Vector3 location, Vector3 target, float distance)
    {
        return Vector3.Distance(location, target) < distance;
    }
    protected void TurnOnVisual()
    {
        animalVisual.SetActive(true);
    }
    protected void TurnOffVisual()
    {
        animalVisual.SetActive(false);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == home)
        {
            if (!isDay)
            {
                TurnOffVisual();
                isHome = true;
            }
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject == home)
        {
            isHome = false;
            TurnOnVisual();
        }
    }
    protected void WalkSpeed()
    {
        animator.PlayWalk();
        animalAgent.speed = speed_Walking;
    }
    protected void RunSpeed()
    {
        animator.PlayRun();
        animalAgent.speed = speed_Running;
    }
}
