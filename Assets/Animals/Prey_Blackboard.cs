using UnityEngine;
using UnityEngine.Events;

public class Prey_Blackboard : AnimalBlackboard_Base
{
    public bool isSafe = false;
    float scaredDistance = 10f;
    GameObject[] predators;

    GameObject closestPredator;
    GameObject closestHome;

    bool needToGoSafePlace = false;
    UnityEvent fleeing = new();
    public bool isEnemyNearby;
    public bool isFleeDone = false;
    public bool isHiding = false;

    public override void Start()
    {
        GetPredators();
        base.Start();
        InitializePrey();
    }
    protected void InitializePrey()
    {
        SetAnimalType("Prey");
        fleeing.AddListener(GetToSafePlace_Helper);
        GetPredators();

        closestPredator = predators[0];
        closestHome = home;
    }
    public override void Update()
    {
        base.Update();
        isEnemyNearby = IsEnemyNearby();

        if (!isEnemyNearby)
        {
            isFleeDone = false;
        }

        if (isEnemyNearby && isSafe)
        {
            Hiding();
        }
    }
    // Is Enemy Nearby
    public bool IsEnemyNearby()
    {
        GetPredators();

        if (predators.Length == 0 || predators == null)
        {
            Debug.Log("No predators found in IsEnemyNearby() by " + gameObject.name);
            return false;
        }

        closestPredator = predators[0];

        for (int i = 0; i < predators.Length; i++)
        {
            if (predators[i] == null)
            {
                Debug.Log("No predators found in IsEnemyNearby() in the loopby " + gameObject.name);
            }
            if (IsCloserThan(gameObject.transform.position, predators[i].transform.position, Vector3.Distance(gameObject.transform.position, closestPredator.transform.position)))
            {
                closestPredator = predators[i];
            }
        }

        if (IsCloserThan(gameObject.transform.position, closestPredator.transform.position, scaredDistance))
        {
            return true;
        }

        return false;

    }
    void GetPredators()
    {
        predators = GameObject.FindGameObjectsWithTag("Predator");
        // Debug.Log(predators.Length);
    }
    protected override void CheckIsSleeping()
    {
        if (isSleeping)
        {
            animalAgent.radius = 0f;
            TurnOffVisual();
            TurnOffNavmeshAgentComponent();
        }
        else
        {
            if (!isHiding)
            {
                animalAgent.radius = 1f;
                TurnOnNavmeshAgentComponent(); 
                TurnOnVisual();
            }
        }
    }
    // Get To Safe Place
    void GetToSafePlace_Helper()
    {
        RunSpeed();
        GetHomes(animalBreed);
        GetClosestHome();

        animalAgent.SetDestination(closestHome.transform.position);
        Debug.Log("Get To Safe Place");
    }
    public void GetToSafePlace_Action()
    {
        if (!needToGoSafePlace)
        {
            needToGoSafePlace = true;
            ResetWanderingBools();
            fleeing.Invoke();
            isFleeDone = false;
        }

        else if (needToGoSafePlace && !IsCloserThan(gameObject.transform.position, closestPredator.transform.position, scaredDistance))
        {
            isFleeDone = true;
            needToGoSafePlace = false;
            // isEnemyNearby = false;
        }
    }
    public void Hiding()
    {
        if (isHiding == false)
        {
            isHiding = true;
            Hiding_Helper();
        }
        else if (!IsEnemyNearby() && isHiding == true)
        {
            isHiding = false;
        }
    }
    public void Hiding_Helper()
    {
        Debug.Log("Hiding helper is fired");
        gameObject.transform.position = home.transform.position;
        animalAgent. radius = 0f;
        animalAgent.ResetPath();
        ResetWanderingBools();
        // StopAllCoroutines();
        TurnOffVisual();
        TurnOffNavmeshAgentComponent();
    }

    void GetClosestHome()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (IsCloserThan(gameObject.transform.position, homes[i].transform.position, Vector3.Distance(gameObject.transform.position, closestHome.transform.position)))
            {
                closestHome = homes[i];
            }
        }
    }

    // Is Safe
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == home && !isDay)
        {
            isSafe = true;
            isHome = true;
            isSleeping = true;

            gameObject.transform.position = home.transform.position;
        }

        if (other.gameObject == home)
        {
            isHome = true;
        }

        if (other.gameObject == closestHome || other.gameObject == home)
        {
            isSafe = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == closestHome || other.gameObject == home)
        {
            isSafe = false;
            isHome = false;
            isHiding = false;
        }        
    }
}
