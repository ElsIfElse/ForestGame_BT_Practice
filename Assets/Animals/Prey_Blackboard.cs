using UnityEngine;
using UnityEngine.Events;

public class Prey_Blackboard : AnimalBlackboard_Base
{
    public bool isSafe = false;
    float scaredDistance = 20f;
    GameObject[] predators;


    GameObject closestPredator;
    GameObject closestHome;

    bool needToGoSafePlace = false;
    UnityEvent fleeing = new();
    public bool isEnemyNearby;
    public bool isFleeDone = false;

    public override void Start()
    {
        SetAnimalType("Prey");
        SetAnimalBreed("Sheep");

        base.Start();

        fleeing.AddListener(GetToSafePlace);
        GetPredators();

        closestPredator = predators[0];
        closestHome = home;
    }
    void Update()
    {
        isEnemyNearby = IsEnemyNearby();

        if (!isEnemyNearby)
        {
            isFleeDone = false;
        }
    }
    // Is Enemy Nearby
    public bool IsEnemyNearby()
    {
        GetPredators();

        for (int i = 0; i < predators.Length; i++)
        {
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
    }

    // Get To Safe Place
    void GetToSafePlace()
    {
        animator.PlayRun();
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
            ResetWanderingBools();
            needToGoSafePlace = true;
            fleeing.Invoke();
        }
        else if (needToGoSafePlace && !IsCloserThan(gameObject.transform.position, closestPredator.transform.position, scaredDistance))
        {
            isFleeDone = true;
            needToGoSafePlace = false;
            isEnemyNearby = false;
        }
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
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject == closestHome || other.gameObject == home)
        {
            TurnOffVisual();
            isSafe = true;
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject == closestHome || other.gameObject == home)
        {
            TurnOnVisual();
            isSafe = false;
        }
    }
}
