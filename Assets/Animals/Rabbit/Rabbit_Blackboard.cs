using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit_Blackboard : Animal_BaseClass
{
    public bool isEnemyNear;
    public bool isAtWanderingLocation;
    GameObject[] predators = null;
    GameObject[] meadows;
    GameObject[] safeAreas;
    float originalRadius = 0.5f;
    Vector3 nextWanderingLocation;


 
    public override void Start()
    {
        SetAnimalType("Rabbit");
        base.Start();
        
        animalAnimator = GetComponent<Animation_Manager_Rabbit>();
    }
    void Update()
    {
        isEnemyNear = IsEnemyNearby();
        isAtWanderingLocation = Vector3.Distance(gameObject.transform.position,nextWanderingLocation) < 0.5f;

        if(((isSafe || isHome) && isEnemyNear) || (isHome && !worldStatus.isDay)){
            animalAgent.enabled = false;
            TurnOffAgent();
            animalVisual.SetActive(false);
            ResetWanderParams();
        }
        else{
            animalAgent.radius = originalRadius;
            animalAgent.enabled = true;
            animalVisual.SetActive(true); 
        }
    }

    bool IsEnemyNearby(){
        predators = null;
        predators = GameObject.FindGameObjectsWithTag("Predator");
        
        if(predators.Length == 0 || predators == null){
            return true;
        }

        GameObject closestPredator = predators[0];
        float closestDistance = Vector3.Distance(closestPredator.transform.position,gameObject.transform.position);

        for(int i = 0; i < predators.Length; i++){
            float distance = Vector3.Distance(predators[i].transform.position,gameObject.transform.position);
            if(distance < closestDistance){
                closestPredator = predators[i];
                closestDistance = distance;
            }
        }
        return closestDistance < preyAlertDistance;
    }


    public override void Eat_Action()
    {
        animalAgent.ResetPath();
        animalAgent.isStopped = true;
        StopAgent();
        SetSpeed(0);
        TurnOnAgent();
        ResetWanderParams();
        isWandering = false;
        isWanderIdling = false;
        animalAnimator.PlayEat(); 
    }

    public override IEnumerator Eat_Action_Coroutine()
    {
        Debug.Log("Eating");
        yield return null;
    }

    public override void Go_Home_Action()
    {
        animalAgent.isStopped = false;
        Walk();
        TurnOnAgent();
        ResetWanderParams();
        Go_Home_Action();
    }

    public override void Sleep_Action()
    {
        TurnOnAgent();
        Debug.Log("Sleeping");
    }

    public void IdleStayStill(){
        ResetWanderParams();
        TurnOnAgent();
        animalAnimator.PlayIdle();
        SetSpeed(0);
    }

    public override void Wandering_Action()
    {
        TurnOnAgent();
        if(!isWandering){
            TurnOnAgent();
            animalAgent.isStopped = false;
            Walk();
            isWandering = true;

            nextWanderingLocation = GetRandomMeadowLocationForWandering();
            animalAgent.SetDestination(nextWanderingLocation);
        }
        else if(!isWanderIdling && isWandering && Vector3.Distance(gameObject.transform.position,nextWanderingLocation) < 0.5f){
            SetSpeed(0);
            animalAgent.isStopped = true;
            isWanderIdling = true;
            StartCoroutine(Wandering_Action_Coroutine());
        }
    }
    public void GoHome(){
        ResetWanderParams();
        animalAgent.SetDestination(home.transform.position);
        Walk();
    }
    Vector3 GetRandomMeadowLocationForWandering(){
        meadows = GameObject.FindGameObjectsWithTag("Meadow");
        int randomIndex = UnityEngine.Random.Range(0,meadows.Length);
        GameObject randomMeadow = meadows[randomIndex];
        
        float meadowPosX = randomMeadow.transform.position.x;
        float meadowPosZ = randomMeadow.transform.position.z;

        float meadowScaleX = randomMeadow.transform.localScale.x;
        float meadowScaleZ = randomMeadow.transform.localScale.z;

        float randomX = UnityEngine.Random.Range(meadowPosX - meadowScaleX/2, meadowPosX + meadowScaleX/2);
        float randomZ = UnityEngine.Random.Range(meadowPosZ - meadowScaleZ/2, meadowPosZ + meadowScaleZ/2);
        float randomY = 0;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(new Vector3(randomX,0,randomZ), out hit, 1f, NavMesh.AllAreas)){
            randomX = hit.position.x;
            randomZ = hit.position.z;
        }

        RaycastHit groundHit;
        if(Physics.Raycast(new Vector3(randomX,50f,randomZ), Vector3.down, out groundHit, 70f)){
            randomY = groundHit.point.y;
        }
        
        Vector3 randomLocation = new Vector3(randomX,randomY,randomZ);
        return randomLocation;

    }

    public override IEnumerator Wandering_Action_Coroutine()
    {
        animalAgent.isStopped = true;
        animalAgent.ResetPath();
        animalAnimator.PlayEat();
        yield return new WaitForSeconds(3f);
        isWandering = false;
        isWanderIdling = false;
    }

    void ResetWanderParams(){
        StopAllCoroutines();
        isWandering = false;
        isWanderIdling = false;
    }
    public void GoToClosestSafePlace(){
        animalAgent.isStopped = false;
        TurnOnAgent();
        Run();
        ResetWanderParams();
        animalAgent.SetDestination(GetClosestSafePlaceLocation());
    }
    Vector3 GetClosestSafePlaceLocation(){
        safeAreas = GameObject.FindGameObjectsWithTag("SafeArea");
        GameObject closestSafeArea = safeAreas[0];

        float closestDistance = Vector3.Distance(gameObject.transform.position,closestSafeArea.transform.position);

        for(int i = 0; i < safeAreas.Length;i++){
            float newDistance = Vector3.Distance(gameObject.transform.position,safeAreas[i].transform.position);

            if(newDistance < closestDistance){
                closestSafeArea = safeAreas[i];
                closestDistance = newDistance;
            }
        }

        if(DistanceFromHome() < closestDistance){
            closestSafeArea = home;
        }
        if(isSheepDebugOn){Debug.Log(closestSafeArea);}

        return closestSafeArea.transform.position;
    }
    float DistanceFromHome(){
        return Vector3.Distance(gameObject.transform.position,home.transform.position);
    }
    
    // Triggers
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SafeArea") || other.gameObject == home){
            animalVisual.SetActive(false);
            isSafe = true;
        } 

        if(other.gameObject == home){
            animalVisual.SetActive(false);
            isHome = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("SafeArea") || other.gameObject == home){
            animalVisual.SetActive(true);
            isSafe = false;
        }
        if(other.gameObject == home){
            animalVisual.SetActive(true);
            isHome = false;
        }
    }

}
