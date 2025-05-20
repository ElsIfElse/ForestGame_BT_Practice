using System.Collections;
using UnityEditor.Analytics;
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
    public bool isFriendly = false;
    [SerializeField] float chanceToGetFriendly;

    // Tree conditions
    public bool isDay;
    public bool isHome;
    public bool isDying = false;

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
    protected GameObject friendIndicator;
    Audio_Manager audioManager;

    // Sleep
    public bool isSleeping = false;

    // Befriending
    ParticleSystem friendParticle;

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
        audioManager = managerCollector.audioManager;

        friendParticle = gameObject.transform.Find("friendParticle").GetComponent<ParticleSystem>();
        friendIndicator = gameObject.transform.Find("friendIndicator").gameObject;
        friendIndicator.SetActive(false);
    }

    void InitializeAnimal()
    {
        SubscribeTo_Events();
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
    public virtual void Update()
    {
        isDay = worldStatus.isDay;

        CheckIsSleeping();

        if (worldStatus.currentTimeInHours == 7)
        {
            WakeUp();
        }
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
                if (sheepHomes.Length == 0)
                {
                    Debug.LogError("No sheep homes found RETRYING");
                    AssignHome();
                }
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
                if (rabbitHomes.Length == 0)
                {
                    Debug.LogError("No rabbit homes found RETRYING");
                    AssignHome();
                }
                GameObject rabbitHome = rabbitHomes[Random.Range(0, rabbitHomes.Length)];
                home = rabbitHome;

                break;

            case "Goat":
                GameObject[] goatHomes = GameObject.FindGameObjectsWithTag("GoatHome");
                if (goatHomes.Length == 0)
                {
                    Debug.LogError("No goat homes found RETRYING");
                    AssignHome();
                }
                GameObject goatHome = goatHomes[Random.Range(0, goatHomes.Length)];
                home = goatHome;

                break;

            case "Bear":
                GameObject[] bearHomes = GameObject.FindGameObjectsWithTag("BearHome");
                if (bearHomes.Length == 0)
                {
                    Debug.LogError("No bear homes found RETRYING");
                    AssignHome();
                }

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
                if (sheepWanderingAreas.Length == 0)
                {
                    Debug.LogError("No sheep WanderingArea AKA MEADOW found");
                }

                wanderingAreas = sheepWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Wolf":
                GameObject[] wolfWanderingAreas = GameObject.FindGameObjectsWithTag("WolfWanderingArea");
                if (wolfWanderingAreas.Length == 0)
                {
                    Debug.LogError("No wolf WanderingArea found");
                }

                wanderingAreas = wolfWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Rabbit":
                GameObject[] rabbitWanderingAreas = GameObject.FindGameObjectsWithTag("Meadow");
                if (rabbitWanderingAreas.Length == 0)
                {
                    Debug.LogError("No rabbit WanderingArea AKA MEADOW found");
                }

                wanderingAreas = rabbitWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;

            case "Goat":
                GameObject[] goatWanderingAreas = GameObject.FindGameObjectsWithTag("GoatWanderingArea");
                if (goatWanderingAreas.Length == 0)
                {
                    Debug.LogError("No goat WanderingArea found");
                }

                wanderingAreas = goatWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];

                break;

            case "Bear":
                GameObject[] bearWanderingAreas = GameObject.FindGameObjectsWithTag("BearWanderingArea");
                if (bearWanderingAreas.Length == 0)
                {
                    Debug.LogError("No bear WanderingArea found");
                }

                wanderingAreas = bearWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
                break;
        }
    }
    void AssignAnimalBreed()
    {
        animalBreed = gameObject.name;
        Debug.Log(animalBreed);
    }
    public void SetId(int id)
    {
        animalId = id;
    }
    void GetAnimalVisual()
    {
        animalVisual = gameObject.transform.Find("animalVisual").gameObject;
        if (animalVisual == null) Debug.Log("No visual found on " + gameObject.name);
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
        if (animalAgent == null) Debug.Log("No agent found on " + gameObject.name);
    }
    void GetAnimator()
    {
        animator = gameObject.GetComponent<Animal_AnimatorBaseClass>();
        if (animator == null) Debug.Log("No animator found on " + gameObject.name);
    }

    // Wandering Functions
    void SubscribeTo_Events()
    {
        gotWanderingLocation.AddListener(GoToWanderingLocation);
        // worldStatus.dayPassedEvent.AddListener(WakeUp);
    }
    void GoToWanderingLocation()
    {
        WalkSpeed();
        animalAgent.SetDestination(currentWanderingLocation);
    }
    public void Wandering()
    {

        if (!isIdle && animalAgent.velocity.sqrMagnitude < 1f && hasWanderingLocation)
        {
            currentStuckTime -= Time.deltaTime;

            if (currentStuckTime <= 0f)
            {
                // Debug.Log("Animal Stuck Time is up. Looking for new location for " + animalBreed);
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
            StopAllCoroutines();

            hasWanderingLocation = true;
            isWandering = true;
            currentWanderingLocation = GetRandomLocationAtArea(wanderingArea);

            WalkSpeed();

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
        yield return new WaitForSeconds(3.4f);
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
    public void StayAndIdle()
    {
        StopAllCoroutines();
        animator.PlayIdle();
        ResetWanderingBools();
        animalAgent.ResetPath();
    }
    public void StayHomeAndSleep_Action()
    {
        if (!isSleeping && isHome && !isDay)
        {
            isSleeping = true;
            animalAgent.radius = 0f;
            animalAgent.ResetPath();
            ResetWanderingBools();
            StopAllCoroutines();
            TurnOffVisual();
            TurnOffNavmeshAgentComponent();
        }
    }
    void WakeUp()
    {
        // Debug.Log("Waking up " + gameObject.name);
        if (isSleeping == true && isDay)
        {
            isSleeping = false;
            animalAgent.radius = 1f;
            TurnOnNavmeshAgentComponent();
            ResetWanderingBools();
            TurnOnVisual();
        }

    }
    public void GoHome()
    {
        StopAllCoroutines();
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
    protected void TurnOnNavmeshAgentComponent()
    {
        animalAgent.enabled = true;
    }
    protected void TurnOffNavmeshAgentComponent()
    {
        animalAgent.enabled = false;
    }
    protected virtual void CheckIsSleeping()
    {
        if (isSleeping)
        {
            animalAgent.radius = 0f;
            TurnOffVisual();
            TurnOffNavmeshAgentComponent();
        }
        else
        {
            animalAgent.radius = 1f;
            TurnOnNavmeshAgentComponent();
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

    // Player Interactions
    public void ChanceToGetFriendlyAfterFeeding()
    {
        float randomNum = Random.Range(0, 100);

        if (randomNum < chanceToGetFriendly)
        {
            isFriendly = true;
            audioManager.PlayAnimalBecameFriendly();
            PlayFriendlyParticle();
            TurnOnFriendlyAnimalVisual();

        }

        return;

    }
    public void PlayFriendlyParticle()
    {
        friendParticle.Play();
    }
    public void TurnOnFriendlyAnimalVisual()
    {
        friendIndicator.SetActive(true);
    }
}
