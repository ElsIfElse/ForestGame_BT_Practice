using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Wolf_State : MonoBehaviour
{
    GameObject wolfBody;
    NavMeshAgent wolfAgent;
    GameObject[] homes;
    GameObject[] huntingAreas;
    GameObject[] wanderingAreas;
    public GameObject home;

    [Space(10)]
    [Header("Wolf Behaviour Settings")]
    public float sleepTime = 20f;
    public float wakeUpTime = 8f;
    public float chaseDistance = 15f;
    public float attackDistance = 2f;
    public float runningSpeed;
    public float walkingSpeed;
    //
    World_Status worldStatus;
    Animation_Manager_Wolf wolfAnimationManager;
    Manager_Collector managerCollector;
    // -- //
    public string wolfName;
    string[] names = {
        "Max", "Bella", "Charlie", "Luna", "Rocky", "Lucy", "Cooper", "Molly", "Buddy", "Daisy",
        "Oliver", "Lola", "Tucker", "Sadie", "Duke", "Maggie", "Bear", "Sophie", "Zeus", "Chloe",
        "Riley", "Stella", "Jack", "Zoey", "Marley", "Roxy", "Lucky", "Penny", "Finn", "Ruby"
    };

    ////// Conditions For Tree
    [Space(10)]
    [Header("Is Day Conditions")]
    public bool isDay;
    public bool isHome = false;

    [Space(10)]
    [Header("Not Hungry Conditions")]
    public bool notHungry = true;
    public bool hasFood = false;
    public bool canAttack = false;
    public bool canSeePrey = false;
 
    ////// Helper variables
    [Space(10)]
    [Header("Helper Variables")]
    public bool isEating = false;
    public bool isAttacking = false;
    public bool isOnWayToHuntingArea = false;
    Vector3 currentPreyLocation;
    public GameObject currentPrey;
    public bool isWandering = false;
    public bool wanderingHelper = false; 
    Vector3 nextWanderingLocation;
    GameObject[] preys;
    float lastTimeAte;
    GameObject huntingArea;
    void Start()
    {
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        wolfAgent = gameObject.GetComponent<NavMeshAgent>();
        wolfBody = transform.Find("WolfBody").gameObject;
        wolfAnimationManager = GetComponent<Animation_Manager_Wolf>();
        huntingAreas = GameObject.FindGameObjectsWithTag("WolfHuntingArea");
        wanderingAreas = GameObject.FindGameObjectsWithTag("WolfWanderingArea");
        homes = GameObject.FindGameObjectsWithTag("WolfHome");
        wolfName = GetRandomName();

        AssignHome();
        GetRandomSpeeds();

        worldStatus.hourPassedEvent.AddListener(GettingHungryLogic);
        worldStatus.dayPassedEvent.AddListener(GettingHungryLogic);

        huntingArea = GetRandomHuntingArea();
    }

    void Update()
    {
        GetClosestPreyLocation();
        
        isDay = worldStatus.isDay;
        canSeePrey = CanSeePrey();

        if(!notHungry){
            GetClosestPreyLocation();
        }
        if(isHome && !isDay){
            TurnOffAgent();
        }
        else{
            TurnOnAgent();
        }
    }
    
    ////// Is Day Actions
    public void StayHomeAndSleep(){
        ResetWanderParams();
        ResetHuntingParams();
        StopAgent();
        // wolfAnimationManager.PlayIdle();
    }
    public void GoHome(){
        ResetHuntingParams();
        ResetWanderParams();
        Walk();
        wolfAgent.SetDestination(home.transform.position); 
    }

    ////// Not Hungry Actions
    public void Eat_Action(){
        if(!isEating){
            lastTimeAte = worldStatus.currentTimeInHours;
            ResetWanderParams();
            ResetHuntingParams();
            StartCoroutine(Eat_Action_Coroutine());
        }
    }
    IEnumerator Eat_Action_Coroutine(){
        isEating = true; 
        StopAgent();
        wolfAnimationManager.PlayEat();
        yield return new WaitForSeconds(4.4f);
        notHungry = true;
        hasFood = false;
        isEating = false;
    }
    public void GettingHungryLogic(){
        if(notHungry && !hasFood && !isEating && !isAttacking && !isOnWayToHuntingArea && Mathf.Abs(worldStatus.currentTimeInHours - lastTimeAte) >= 8f){
            ResetHuntingParams();
            ResetWanderParams();
            int randomNumber = Random.Range(0,100);
            if(randomNumber < 20){
                notHungry = false;
            }
        }
    }
    // -- //
    public void Attack_Action(){
        if(!isAttacking){
            ResetWanderParams();
            ResetHuntingParams();
            isAttacking = true;
            StartCoroutine(Attack_Action_Coroutine());
        }
    }
    IEnumerator Attack_Action_Coroutine(){
        worldStatus.RemoveSheep(currentPrey.GetComponent<Id_Script>().id);
        
        StopAgent();
        wolfAnimationManager.PlayAttack();
        yield return new WaitForSeconds(1.7f);

        isAttacking = false;
        hasFood = true;
        canAttack = false;
    }
    // -- //
    public void Chase_Action(){
        ResetHuntingParams();
        ResetWanderParams();
        wolfAgent.SetDestination(currentPreyLocation);
        Run();
    }
    void GetClosestPreyLocation(){
        preys = GameObject.FindGameObjectsWithTag("Prey");

        if(preys.Length == 0 && preys == null){
            ResetHuntingParams();
            ResetWanderParams();

            Debug.Log("No preys found");
            return;
        }
        
        GameObject closest = null;  
        Vector3 closestLocation = preys[0].transform.position;
        float closestDistance = Vector3.Distance(gameObject.transform.position,closestLocation);

        for(int i = 0; i < preys.Length; i++){
            GameObject currentCheckedPrey = preys[i];
            Vector3 currentCheckedPreyLocation = currentCheckedPrey.transform.position;
            float currentCheckedPreyDistance = Vector3.Distance(gameObject.transform.position,currentCheckedPreyLocation);
            
            if(currentCheckedPrey.GetComponent<Sheep_Status>().isSafe == false && currentCheckedPreyDistance < closestDistance){
                closestLocation = currentCheckedPreyLocation;
                closestDistance = currentCheckedPreyDistance;
                closest = preys[i];
            }
        }

        currentPrey = closest;
        currentPreyLocation = closestLocation;
    }
    // -- //
    public void GoToHuntingLocation_Action(){
        if(!isOnWayToHuntingArea){
            ResetWanderParams();
            StartCoroutine(GoToHuntingLocation_Action_Coroutine());
        }
    }
    GameObject GetRandomHuntingArea(){
        GameObject randomArea = huntingAreas[Random.Range(0,huntingAreas.Length)];
        return randomArea;
    }
    IEnumerator GoToHuntingLocation_Action_Coroutine(){
        isOnWayToHuntingArea = true;

        Walk();

        float huntingAreaX = huntingArea.transform.position.x;
        float huntingAreaZ = huntingArea.transform.position.z;

        float huntingAreaScaleX = huntingArea.transform.localScale.x;
        float huntingAreaScaleZ = huntingArea.transform.localScale.z;

        float randomX = UnityEngine.Random.Range(huntingAreaX - huntingAreaScaleX/2, huntingAreaX + huntingAreaScaleX/2);
        float randomZ = UnityEngine.Random.Range(huntingAreaZ - huntingAreaScaleZ/2, huntingAreaZ + huntingAreaScaleZ/2);
        
        Vector3 randomLocation = new Vector3(randomX,0,randomZ);

        wolfAgent.SetDestination(randomLocation);
    
        while(Vector3.Distance(gameObject.transform.position,randomLocation) > 1f){
            yield return null;
        }

        Debug.Log("Reached Hunting Position");
        wolfAnimationManager.PlayIdle();
        yield return new WaitForSeconds(2f);
        ResetHuntingParams();
    }
    void ResetHuntingParams(){
        StopAllCoroutines();
        isOnWayToHuntingArea = false;
    }
    
    ////// Wandering
    public void WanderingAction(){
        if(!isWandering){
            ResetHuntingParams();
            Walk();
            isWandering = true;

            nextWanderingLocation = GetWanderingLocation();
            wolfAgent.SetDestination(nextWanderingLocation);
        }
        else if(!wanderingHelper && isWandering && Vector3.Distance(gameObject.transform.position,nextWanderingLocation) < 0.2f){
            StopAllCoroutines();
            wanderingHelper = true;
            StartCoroutine(IdleWhileWander());
        }
    }
    IEnumerator IdleWhileWander(){
        wolfAnimationManager.PlayIdle();
        yield return new WaitForSeconds(10f);
        isWandering = false;
        wanderingHelper = false;
    }
    public void ResetWanderParams(){
        StopAllCoroutines();
        isWandering = false;
        wanderingHelper = false;
    }
    Vector3 GetWanderingLocation(){
        GameObject randomArea = wanderingAreas[Random.Range(0,wanderingAreas.Length)];

        float randomX = randomArea.transform.position.x;
        float randomZ = randomArea.transform.position.z;

        float randomScaleX = randomArea.transform.localScale.x;
        float randomScaleZ = randomArea.transform.localScale.z;

        float randomXPosition = UnityEngine.Random.Range(randomX - randomScaleX/2, randomX + randomScaleX/2);
        float randomZPosition = UnityEngine.Random.Range(randomZ - randomScaleZ/2, randomZ + randomScaleZ/2);
        
        Vector3 randomLocation = new Vector3(randomXPosition,0,randomZPosition);
        return randomLocation;
    }
    
    ////// Utilities
    void StopAgent(){
        wolfAgent.ResetPath();
    }
    void Walk(){
        wolfAnimationManager.PlayWalk();
        wolfAgent.speed = walkingSpeed;
    }
    void Run(){
        wolfAnimationManager.PlayRun();
        wolfAgent.speed = runningSpeed;
    }
    void AssignHome(){ 
        homes = GameObject.FindGameObjectsWithTag("WolfHome");
        home = homes[UnityEngine.Random.Range(0,homes.Length)];
    }
    public void TurnOnAgent(){
        wolfAgent.enabled = true;
    }
    public void TurnOffAgent(){
        wolfAgent.enabled = false;
    }
    string GetRandomName(){
        string randomName = names[Random.Range(0,names.Length)];
        return randomName;
    }
    public void GetRandomSpeeds(){
        walkingSpeed = UnityEngine.Random.Range(3,5);
        runningSpeed = UnityEngine.Random.Range(7,10);
    }
    
    ////// Condition returns
    bool IsSleepTime(){
        if(worldStatus.currentTimeInHours >= sleepTime && worldStatus.currentTimeInHours < wakeUpTime){
            return true;
        }
        else{
            return false;
        }
    }
    bool CanSeePrey(){
        if(currentPrey == null){
            return false;
        }
        else{
            return Vector3.Distance(gameObject.transform.position,currentPreyLocation) < chaseDistance && currentPrey.GetComponent<Sheep_Status>().isSafe == false;
        }  
    }
    
    ////// Collider Triggers
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == home && !isDay){
            wolfBody.SetActive(false);
            isHome = true;
        }
        if(other.gameObject == currentPrey){
            canAttack = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == home){
            wolfBody.SetActive(true);
            isHome = false;
        }
        if(other.gameObject == currentPrey){
            canAttack = false;
        }
    }
}
