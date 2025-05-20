using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Predator_Blackboard : AnimalBlackboard_Base
{
    // Looking For Prey
    GameObject huntingArea;
    GameObject[] huntingAreas;
    GameObject[] preys;
    GameObject currentPrey;

    Vector3 currentLookingForPrayLocation;
    bool hasLookingForPreyLocation = false;
    float chaseDistance = 20f;
    UnityEvent gotLookingForPreyLocation = new UnityEvent();

    // Tree Conditions
    public bool isHungry = false;
    public bool hasFood = false;
    public bool canSeePrey = false;
    public bool canAttack = false;

    // Getting Hungry Logic Helpers
    float lastTimeAte = 4;
    float timeWithoutHungerAfterEating = 8f;

    // Attacking Helpers
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isEating = false;

    public override void Start()
    {
        base.Start();

        worldStatus.dayPassedEvent.AddListener(() =>
        {
            TurnOnVisual();
            TurnOnNavmeshAgentComponent();
        });
        PredatorInitialization();
    }
    public override void Update()
    {
        base.Update();
    }
    void PredatorInitialization()
    {
        SetAnimalType("Predator");
        GetHuntingAreas();
        AssignHuntingArea();
        GetPreys();

        currentPrey = preys[0];

        worldStatus.hourPassedEvent.AddListener(GettingHungryLogic);

        SubscribeToGoToLookingForPreyLocationEvent();
    }
    void GetHuntingAreas()
    {
        huntingAreas = GameObject.FindGameObjectsWithTag("WolfHuntingArea");
    }
    void AssignHuntingArea()
    {
        huntingArea = huntingAreas[UnityEngine.Random.Range(0, huntingAreas.Length)];
    }

    // Tree Actions
    public void Eat()
    {
        animalAgent.ResetPath();

        if (!isEating)
        {
            animator.PlayEat();
            isEating = true;
            StartCoroutine(EatHelper_Coroutine());
        }
    }
    IEnumerator EatHelper_Coroutine()
    {
        yield return new WaitForSeconds(4.4f);
        isHungry = false;
        hasFood = false;
        isEating = false;
    }
    public void Attack()
    {
        if (!isAttacking)
        {
            StopAllCoroutines();
            StartCoroutine(AttackHelper_Coroutine());
        }
    }
    IEnumerator AttackHelper_Coroutine()
    {
        animalAgent.velocity = Vector3.zero;

        currentPrey.GetComponent<Prey_Blackboard>().Dying();
        currentPrey = null;
        isAttacking = true;
        animalAgent.ResetPath();
        animator.PlayAttack();

        yield return new WaitForSeconds(1.7f);

        isAttacking = false;
        hasFood = true;
        canAttack = false;
    }
    public void Chase()
    {
        GetClosestPrey();
        RunSpeed();
        animalAgent.SetDestination(currentPrey.transform.position);
    }

    // Looking For Prey | Hunting
    public void LookForPrey()
    {

        GetClosestPrey();

        if (!hasLookingForPreyLocation)
        {
            hasLookingForPreyLocation = true;
            currentLookingForPrayLocation = GetRandomLocationAtArea(huntingArea);
            gotLookingForPreyLocation.Invoke();
        }
        else
        {
            if (IsCloserThan(gameObject.transform.position, currentLookingForPrayLocation, chaseDistance) && currentPrey.GetComponent<Prey_Blackboard>().isSafe == false)
            {
                animalAgent.ResetPath();
                hasLookingForPreyLocation = false;
                canSeePrey = true;
            }
            else
            {
                canSeePrey = false;
            }
        }
    }
    protected void GetPreys()
    {
        preys = GameObject.FindGameObjectsWithTag("Prey");
    }
    protected void GetClosestPrey()
    {
        GetPreys();

        if (preys == null)
        {
            return;
        }
        if (currentPrey == null) currentPrey = preys[0];

        for (int i = 0; i < preys.Length; i++)
        {
            if (IsCloserThan(gameObject.transform.position, preys[i].transform.position, Vector3.Distance(gameObject.transform.position, currentPrey.transform.position)) && preys[i].GetComponent<Prey_Blackboard>().isSafe == false)
            {
                currentPrey = preys[i];
            }
        }
    }
    protected void GoToLookingForPreyLocation()
    {
        ResetWanderingBools();
        WalkSpeed();
        animalAgent.SetDestination(currentLookingForPrayLocation);
    }
    private void SubscribeToGoToLookingForPreyLocationEvent()
    {
        gotLookingForPreyLocation.AddListener(GoToLookingForPreyLocation);
    }
    public void GettingHungryLogic()
    {
        if (!isHungry && !hasFood && Mathf.Abs(worldStatus.currentTimeInHours - lastTimeAte) >= timeWithoutHungerAfterEating)
        {
            int randomNumber = UnityEngine.Random.Range(0, 100);

            if (randomNumber < 10)
            {
                isHungry = true;
            }
        }
    }

    // Can Attack   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == home && !isDay)
        {
            isHome = true;
        }

        if (other.gameObject == currentPrey && currentPrey.GetComponent<Prey_Blackboard>().isSafe == false)
        {
            canAttack = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == home)
        {
            isHome = false;
        }
        if(other.gameObject == currentPrey){
            canAttack = false;
        }
    }

}
