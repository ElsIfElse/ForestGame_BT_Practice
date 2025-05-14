using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Sheep_Status : MonoBehaviour
{
    public string sheepName;
    public float sheepWalkSpeed;
    public float sheepRunningSpeed;
    //
    public bool isSafe;
    public bool isHome;
    public bool isEnemyNear;
    public bool isFleeing;
    public bool isHungry;
    public bool atFood;
    public bool isAtWanderingLocation = false;
    public bool wanderingHelper = false;

    bool isWandering = false;
    //
    GameObject[] homes;
    public GameObject home;
    GameObject[] meadows;
    GameObject[] safeAreas; 
    GameObject[] predators;
    //
    NavMeshAgent sheepAgent;
    Animation_Manager animationManager;
    Vector3 nextWanderingLocation;
    GameObject sheepBody;
    float originalRadius = 0.5f;
    World_Status worldStatus;
    void Start()
    {
        sheepAgent = GetComponent<NavMeshAgent>();
        animationManager = GetComponent<Animation_Manager>();

        sheepBody = transform.Find("SheepBody").gameObject;
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();

        AssignHome();
        RandomizeSheepSpeed();

    }
    void Update()
    {
        CheckConditions();
    }
    
    // Conditions
    void CheckConditions(){
        isEnemyNear = IsEnemyNearby();
        // isHome = Vector3.Distance(gameObject.transform.position,home.transform.position) < 0.5f;
        isAtWanderingLocation = Vector3.Distance(gameObject.transform.position,nextWanderingLocation) < 0.5f;

        if(((isSafe || isHome) && isEnemyNear) || (isHome && !worldStatus.isDay)){
            sheepAgent.enabled = false;
            TurnOffAgent();
            sheepBody.SetActive(false);
        }
        else{
            sheepAgent.radius = originalRadius;
            sheepAgent.enabled = true;
            sheepBody.SetActive(true); 
        }
    }
    bool IsEnemyNearby(){
        GetAllPredators();
        if(predators.Length == 0 && predators == null){
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
        return closestDistance < 10;
    }
    
    // Utility Actions
    public void IdleStayStill(){
        ResetWanderParams(); 
        sheepAgent.ResetPath(); 
        animationManager.PlayIdle();
    }
    public void EatStandStill(){
        sheepAgent.ResetPath();
        animationManager.PlayEat(); 
    }
    public void SetSpeed(float speedType){
        sheepAgent.speed = speedType;
    }
    public void AssignHome(){
        homes = GameObject.FindGameObjectsWithTag("SheepHome");
        home = homes[UnityEngine.Random.Range(0,homes.Length)];
    }
    public void RandomizeSheepSpeed(){
        sheepWalkSpeed = UnityEngine.Random.Range(3,4);
        sheepRunningSpeed = UnityEngine.Random.Range(6,8);
    }
    public void TurnOnAgent(){
        sheepAgent.enabled = true;
    }
    public void TurnOffAgent(){
        sheepAgent.enabled = false;
    }
    // Utility Returns
    float DistanceFromHome(){
        return Vector3.Distance(gameObject.transform.position,home.transform.position);
    }
    GameObject PickRandomMeadow(){
        meadows = GameObject.FindGameObjectsWithTag("Meadow");
        int randomIndex = UnityEngine.Random.Range(0,meadows.Length);
        return meadows[randomIndex];
    }
    Vector3 GetRandomMeadowLocationForWandering(){
        GameObject randomMeadow = PickRandomMeadow();
        
        float meadowPosX = randomMeadow.transform.position.x;
        float meadowPosZ = randomMeadow.transform.position.z;

        float meadowScaleX = randomMeadow.transform.localScale.x;
        float meadowScaleZ = randomMeadow.transform.localScale.z;

        float randomX = UnityEngine.Random.Range(meadowPosX - meadowScaleX/2, meadowPosX + meadowScaleX/2);
        float randomZ = UnityEngine.Random.Range(meadowPosZ - meadowScaleZ/2, meadowPosZ + meadowScaleZ/2);
        
        Vector3 randomLocation = new Vector3(randomX,0,randomZ);
        return randomLocation;

    }
    void GetAllPredators(){
        predators = null;
        predators = GameObject.FindGameObjectsWithTag("Predator");
        if(predators.Length == 0){
            predators = null;
        }
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

        return closestSafeArea.transform.position;
    }
    
    // Is Safe Actions     
    public void GoToClosestSafePlace(){
        TurnOnAgent();
        SetSpeed(sheepRunningSpeed);
        ResetWanderParams();
        animationManager.PlayRun();
        sheepAgent.SetDestination(GetClosestSafePlaceLocation());
    }

    // Wandering
    public void WanderingAction(){
        if(!isWandering){
            TurnOnAgent();
            SetSpeed(sheepWalkSpeed);
            isWandering = true;

            nextWanderingLocation = GetRandomMeadowLocationForWandering();
            sheepAgent.SetDestination(nextWanderingLocation);
            animationManager.PlayWalk();
        }
        else if(!wanderingHelper && isWandering && Vector3.Distance(gameObject.transform.position,nextWanderingLocation) < 0.5f){
            wanderingHelper = true;
            StartCoroutine(WaitForEating());
        }
    }
    IEnumerator WaitForEating(){
        animationManager.PlayEat();
        yield return new WaitForSeconds(3f);
        isWandering = false;
        wanderingHelper = false;
    }
    public void ResetWanderParams(){
        StopAllCoroutines();
        isWandering = false;
        wanderingHelper = false;
    }
    
    // Is Day Actions
    public void GoHome(){
        TurnOnAgent();
        SetSpeed(sheepWalkSpeed);
        ResetWanderParams();
        animationManager.PlayWalk();
        sheepAgent.SetDestination(home.transform.position);
    }

    // Triggers
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SafeArea") || other.gameObject == home){
            isSafe = true;
        } 

        if(other.gameObject == home){
            isHome = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("SafeArea") || other.gameObject == home){
            isSafe = false;
        }
        if(other.gameObject == home){
            isHome = false;
        }
    }
  
}
