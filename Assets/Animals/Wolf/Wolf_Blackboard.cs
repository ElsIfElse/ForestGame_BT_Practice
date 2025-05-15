using System.Collections;
using UnityEngine;

public class Wolf_Blackboard : Animal_BaseClass
{

    public bool hasFood = false;
    public bool isOnWayToHuntingArea = false;
    public bool canSeePrey;
    public bool canAttack = false;    
    bool isAttacking = false;

    bool isSleepingCoroutineOn = false;

    public GameObject currentPrey = null;
    public GameObject[] preys;
    GameObject[] huntingAreas;
    GameObject huntingArea;

    float lastTimeAte;
    float chaseDistance = 15f;
    Vector3 currentPreyLocation;
    Vector3 currentHuntingAreaLocation;
    string preyType;


    public override void Start()
    {
        notHungry = true;
        isOnWayToHuntingArea = false;

        SetAnimalType("Wolf");
        base.Start(); 

        huntingArea = GetRandomHuntingArea();

        managerCollector.worldStatus.hourPassedEvent.AddListener(GettingHungryLogic);
        managerCollector.worldStatus.dayPassedEvent.AddListener(GettingHungryLogic);        
    }
    void Update()
    {
        canSeePrey = CanSeePrey();
        isDay = worldStatus.isDay;

        if(!notHungry){
            GetClosestPreyLocation();
        }
    }

    public void GoToHuntingLocation_Action(){
        if(isWolfDebugOn){Debug.Log("Go To Hunting Action Is On");}

        if(!isOnWayToHuntingArea){
            isOnWayToHuntingArea = true;
            ResetWanderParams();
            StartCoroutine(GoToHuntingLocation_Action_Coroutine());
        }

        if(isOnWayToHuntingArea && Vector3.Distance(gameObject.transform.position,currentHuntingAreaLocation) < 0.2f){
            StopAllCoroutines(); 
            isOnWayToHuntingArea = false;
        }
    }

    IEnumerator GoToHuntingLocation_Action_Coroutine(){
        if(isWolfDebugOn){Debug.Log("Go To Hunting Location Coroutine Started");}

        Vector3 randomLocation = GetRandomLocationAtArea(huntingArea);
        currentHuntingAreaLocation = randomLocation;
        animalAgent.SetDestination(randomLocation);

        Walk();
        yield return null;
    }

    public void Chase_Action(){
        ResetWanderParams();
        Run();
        animalAgent.SetDestination(currentPreyLocation);
    }

    public void Attack_Action(){
        if(!isAttacking){
            ResetAllBools();
            StartCoroutine(Attack_Action_Coroutine());
        }
    }

    IEnumerator Attack_Action_Coroutine(){
        isAttacking = true;

        switch(preyType){
            case "Sheep":
                managerCollector.worldStatus.RemoveSheep(currentPrey.GetComponent<Animal_BaseClass>().animalId);
                break;

            case "Goat":
                managerCollector.worldStatus.RemoveGoat(currentPrey.GetComponent<Animal_BaseClass>().animalId);
                break;
                
            case "Rabbit":
                managerCollector.worldStatus.RemoveRabbit(currentPrey.GetComponent<Animal_BaseClass>().animalId);
                break;
        }

        StopAgent();
        animalAnimator.PlayAttack(); 
        yield return new WaitForSeconds(1.7f);

        isAttacking = false;
        hasFood = true;
        canAttack = false;
    }

    public override void Eat_Action()
    {
        if(!isEating){
            ResetAllBools();
            lastTimeAte = worldStatus.currentTimeInHours;
            StartCoroutine(Eat_Action_Coroutine());
        }
    }

    public override IEnumerator Eat_Action_Coroutine()
    {
        isEating = true; 
        StopAgent();
        animalAnimator.PlayEat();
        yield return new WaitForSeconds(4.4f);
        notHungry = true;
        hasFood = false;
        isEating = false;
    }

    public override void Go_Home_Action()
    {
        ResetAllBools();
        GoHome();
    }
    public override void Sleep_Action()
    {
        if(!isSleepingCoroutineOn){
            ResetAllBools();
            StartCoroutine(Sleeping_Action_Coroutine());
        }
        else{
            if(isDay){
                ResetAllBools();
                isSleepingCoroutineOn = false;
            }
        }
    }
    IEnumerator Sleeping_Action_Coroutine(){
        isSleepingCoroutineOn = true;
        StopAgent(); 
        yield return null;
    }
    public override void Wandering_Action()
    {
        if(!isWandering){
            if(isWolfDebugOn){Debug.Log("Entered !isWandering");}
            isWandering = true;
            ResetHuntingParams();
            StartCoroutine(Wandering_Action_Coroutine());
        }

        else{
            if(!isWanderIdling && animalAgent.remainingDistance < 0.2f){
                if(isWolfDebugOn){Debug.Log("Arrived To Wandering Location");}
                isWanderIdling = true;
                ResetHuntingParams();
                StartCoroutine(WanderIdle_Coroutine());
            }
        }
    }
    public override IEnumerator Wandering_Action_Coroutine()
    {
        if(isWolfDebugOn){Debug.Log("Wander Action Coroutine Started");}

        Walk();
        wanderingLocation = GetRandomLocationAtArea(wanderingArea);
        animalAgent.SetDestination(wanderingLocation);
        yield return null;
    }
    public IEnumerator WanderIdle_Coroutine()
    {
        if(isWolfDebugOn){Debug.Log("Wander Idle Coroutine Started");}
        StopAgent();
        animalAnimator.PlayIdle();
        yield return new WaitForSeconds(10f);

        isWanderIdling = false;
        isWandering = false;
    }
    public void GoHome(){
        ResetHuntingParams();
        ResetWanderParams();
        animalAgent.SetDestination(home.transform.position);
        Walk();
    }

    public void ResetHuntingParams(){
        StopAllCoroutines();
        isOnWayToHuntingArea = false;
    }
    public void ResetWanderParams()
    {
        StopAllCoroutines();
        isWanderIdling = false;
        isWandering = false;
    }
    void ResetAllBools(){
        ResetHuntingParams();
        ResetWanderParams();
    }

    public void GettingHungryLogic(){
        if(notHungry && !hasFood && !isEating && !isAttacking && !isOnWayToHuntingArea && Mathf.Abs(managerCollector.worldStatus.currentTimeInHours - lastTimeAte) >= 8f){
            int randomNumber = Random.Range(0,100);

            if(randomNumber < 10){
                notHungry = false;
            }
        }
    }
    void GetClosestPreyLocation()
{
    preys = GameObject.FindGameObjectsWithTag("Prey");

    // Proper null/empty check
    if (preys == null || preys.Length == 0)
    {
        if (isWolfDebugOn) Debug.Log("No preys found");
        currentPrey = null;
        return;
    }

    GameObject closest = null;
    float closestDistance = Mathf.Infinity;
    string closestPreyType = "";
    Vector3 closestLocation = Vector3.zero;

    foreach (GameObject currentPrey in preys)
    {
        // Skip destroyed or invalid prey
        if (currentPrey == null) continue;

        // Get position safely
        Vector3 currentPosition;
        try
        {
            currentPosition = currentPrey.transform.position;
        }
        catch (MissingReferenceException)
        {
            continue;
        }

        float currentDistance = Vector3.Distance(transform.position, currentPosition);

        // Skip if further than current closest
        if (currentDistance >= closestDistance) continue;

        // Check prey type and safety status
        bool isValidPrey = false;
        string currentPreyType = currentPrey.name;

        switch (currentPreyType)
        {
            case "Goat":
                var goat = currentPrey.GetComponent<Goat_Blackboard>();
                if (goat != null && !goat.isSafe) isValidPrey = true;
                break;

            case "Sheep":
                var sheep = currentPrey.GetComponent<Sheep_Blackboard>();
                if (sheep != null && !sheep.isSafe) isValidPrey = true;
                break;

            case "Rabbit":
                var rabbit = currentPrey.GetComponent<Rabbit_Blackboard>();
                if (rabbit != null && !rabbit.isSafe) isValidPrey = true;
                break;
        }

        if (isValidPrey)
        {
            closest = currentPrey;
            closestDistance = currentDistance;
            closestPreyType = currentPreyType;
            closestLocation = currentPosition;
        }
    }

    if (closest == null)
    {
        if (isWolfDebugOn) Debug.Log("No valid prey found");
        currentPrey = null;
        return;
    }

    currentPrey = closest;
    currentPreyLocation = closestLocation;
    preyType = closestPreyType;
}
    bool CanSeePrey(){
        if(currentPrey == null){
            if(isWolfDebugOn){Debug.Log("No Prey Found");}
            return false;
        }

        if(isWolfDebugOn){Debug.Log(preyType);}
        switch(preyType){
            case "Goat":
                if(Vector3.Distance(gameObject.transform.position,currentPreyLocation) < chaseDistance && currentPrey.GetComponent<Goat_Blackboard>().isSafe == false){
                    return true;
                }
                else{
                    if(isWolfDebugOn){Debug.Log("Prey is out of sight");}
                    return false;
                }  

            case "Sheep":
                if(Vector3.Distance(gameObject.transform.position,currentPreyLocation) < chaseDistance && currentPrey.GetComponent<Sheep_Blackboard>().isSafe == false){
                    return true;
                }
                else{
                    if(isWolfDebugOn){Debug.Log("Prey is out of sight");}
                    return false;
                }  

            case "Rabbit":
                if(Vector3.Distance(gameObject.transform.position,currentPreyLocation) < chaseDistance && currentPrey.GetComponent<Rabbit_Blackboard>().isSafe == false){
                    return true;
                }
                else{
                    if(isWolfDebugOn){Debug.Log("Prey is out of sight");}
                    return false;
                }   

            default: return false;
        }
        // if(Vector3.Distance(gameObject.transform.position,currentPreyLocation) < chaseDistance && currentPrey.GetComponent<Sheep_Status>().isSafe == false){
        //     return true;
        // }
        // else{
        //     if(isWolfDebugOn){Debug.Log("Prey is out of sight");}
        //     return false;
        // }  
    }

    GameObject GetRandomHuntingArea(){
        switch(animalType){
            case "Bear":
                huntingAreas = GameObject.FindGameObjectsWithTag("BearHuntingArea");
                break;

            case "Wolf":
                huntingAreas = GameObject.FindGameObjectsWithTag("WolfHuntingArea");
                break;
        }

        GameObject randomArea = huntingAreas[Random.Range(0,huntingAreas.Length)];
        return randomArea;
    }
    ////// Collider Triggers
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == home && !isDay){
            animalVisual.SetActive(false);
            isHome = true;
        }
        if(other.gameObject == currentPrey){
            canAttack = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == home){
            animalVisual.SetActive(true);
            isHome = false;
        }
        if(other.gameObject == currentPrey){
            canAttack = false;
        }
    }
} 
