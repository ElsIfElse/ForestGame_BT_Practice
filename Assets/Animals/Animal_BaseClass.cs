using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public abstract class Animal_BaseClass : MonoBehaviour
{
    // State Variables
    public bool isTired;
    public bool notHungry = true; 
    public bool isHome; 
    public bool isEating = false;
    public bool isDay;

    public bool isWandering;
    public bool isWanderIdling = false;
    // public bool wanderingHelper;
    public string[] names = {
        "Finn", "Sparky", "Oliver", "Bear", "Luna", "Maggie", "Max", "Charlie", "Daisy", "Buddy", "Rocky", "Lucy", "Cooper", "Ginger", "Simba", "Coco", "Duke", "Lola", "Hunter", "Sandy", "Molly", "Sam", "Toby", "Ruby", "Baxter", "Abby", "Rusty", "Gracie", "Bella", "Maverick", "Penny", "Jasper", "Cody", "Lily", "Odie", "Zoey", "Nala", "Sasha", "Maddie", "Murphy", "Bailey", "Tucker", "Emma", "Fiona", "Dakota", "Chloe", "Chase", "Lila", "Jackson", "Morgan", "Hank", "Sadie", "Gizmo", "Ava", "Jax", "Willow", "Riley", "Sophie", "Bentley", "Lacey", "Mason", "Lexi", "Katie", "Diesel", "Julia", "Hannah", "Titan", "Paisley", "Easton", "Lainey", "Bryce", "Ashley", "Colby", "Avery", "Kolby", "Skye", "Savannah", "Jace", "Cheyenne", "Jenna", "Kai", "Kayla", "Landry", "Leslie", "Mackenzie", "Mallory", "Micah", "Natalie", "Nicole", "Noah", "Paige", "Parker", "Quinn", "Rachel", "Reese", "Rylee", "Savannah", "Sierra", "Taylor", "Tatum", "Teagan", "Tessa", "Trey", "Trinity", "Tyson", "Vivian", "Waylon", "Wesley", "Wyatt", "Xander", "Yvonne", "Zachary"
    };
    public string animalName;
    public string animalType;
    
    public int animalId;

    public GameObject home;
    public GameObject wanderingArea;
    public GameObject animalVisual;
    GameObject[] homes;
    GameObject[] wanderingAreas;


    public Vector3 wanderingLocation;
    

    public float animalWalkspeed;
    public float animalRunspeed;
    public float enemySleepTime;
    public float enemyWakeTime;
    protected float preyAlertDistance;

    public NavMeshAgent animalAgent;

    public Animal_AnimatorBaseClass animalAnimator; 
    


    // References To Managers
    public Manager_Collector managerCollector;
    public World_Status worldStatus;

    //

    protected bool isWolfDebugOn = false;
    protected bool isSheepDebugOn = false;

    public virtual void Start() {
        animalVisual = transform.Find("animalVisual").gameObject;
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        worldStatus = managerCollector.worldStatus;

        animalAgent = GetComponent<NavMeshAgent>();
        animalAnimator = GetComponent<Animal_AnimatorBaseClass>();

        AssignHome();
        AssignBaseSpeeds();
        AssignRandomName(); 
        AssignWanderingArea();
        AssignAlertDistanceForPreys();
    }

    // Initial Assignments
    public void AssignAlertDistanceForPreys(){
        switch(animalType){
            case "Sheep":
                preyAlertDistance = Random.Range(4,8);
                break;
            case "Wolf":
                preyAlertDistance = Random.Range(5,10);
                break;
            case "Rabbit":
                preyAlertDistance = Random.Range(6,9);
                break;
            case "Goat":
                preyAlertDistance = Random.Range(7,11);
                break;
        }
    }
    public void AssignHome(){
        switch(animalType){
            case "Sheep":
                GameObject[] sheepHomes = GameObject.FindGameObjectsWithTag("SheepHome");
                GameObject sheepHome = sheepHomes[Random.Range(0,sheepHomes.Length)];
                home = sheepHome;
                break;

            case "Wolf":
                GameObject[] wolfHomes = GameObject.FindGameObjectsWithTag("WolfHome");
                GameObject wolfHome = wolfHomes[Random.Range(0,wolfHomes.Length)];
                home = wolfHome;
                break;

            case "Rabbit":
                GameObject[] rabbitHomes = GameObject.FindGameObjectsWithTag("RabbitHome");
                GameObject rabbitHome = rabbitHomes[Random.Range(0,rabbitHomes.Length)];
                home = rabbitHome;
                break;

            case "Goat":
                GameObject[] goatHomes = GameObject.FindGameObjectsWithTag("GoatHome");
                GameObject goatHome = goatHomes[Random.Range(0,goatHomes.Length)];
                home = goatHome;
                break;
        }
    }
    public void AssignBaseSpeeds(){
        switch(animalType){
            case "Sheep":
                animalWalkspeed = Random.Range(2,5);
                animalRunspeed = Random.Range(6,8);
                break;

            case "Wolf":
                animalWalkspeed = Random.Range(4,6);
                animalRunspeed = Random.Range(7,12);
                break;
            
            case "Rabbit":
                animalWalkspeed = Random.Range(2,5);
                animalRunspeed = Random.Range(6,9);
                break;
            
            case "Goat":
                animalWalkspeed = Random.Range(5,7);
                animalRunspeed = Random.Range(8,11);
                break;
        }
    }
    public void AssignRandomName(){
        animalName = names[Random.Range(0,names.Length)];
    }
    public void AssignWanderingArea(){
        switch(animalType){
            case "Sheep":
                GameObject[] sheepWanderingAreas = GameObject.FindGameObjectsWithTag("Meadow");
                wanderingAreas = sheepWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0,wanderingAreas.Length)];
                break;

            case "Wolf":
                GameObject[] wolfWanderingAreas = GameObject.FindGameObjectsWithTag("WolfWanderingArea");
                wanderingAreas = wolfWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0,wanderingAreas.Length)];
                break;

            case "Rabbit":
                GameObject[] rabbitWanderingAreas = GameObject.FindGameObjectsWithTag("Meadow");
                wanderingAreas = rabbitWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0,wanderingAreas.Length)];
                break;

            case "Goat":
                GameObject[] goatWanderingAreas = GameObject.FindGameObjectsWithTag("GoatWanderingArea");
                wanderingAreas = goatWanderingAreas;
                wanderingArea = wanderingAreas[Random.Range(0,wanderingAreas.Length)];
                break;
        }
    }
    public void SetAnimalType(string type){
        animalType = type;
    }
    public Vector3 GetWanderingLocation(){

        float randomX = wanderingArea.transform.position.x;
        float randomZ = wanderingArea.transform.position.z;

        float randomScaleX = wanderingArea.transform.localScale.x;
        float randomScaleZ = wanderingArea.transform.localScale.z;

        float randomXPosition = UnityEngine.Random.Range(randomX - randomScaleX/2, randomX + randomScaleX/2);
        float randomZPosition = UnityEngine.Random.Range(randomZ - randomScaleZ/2, randomZ + randomScaleZ/2);
        
        Vector3 randomLocation = new Vector3(randomXPosition,0,randomZPosition);
        return randomLocation;
    }
    
    // NavMesh utilities
    public void StopAgent(){
        animalAgent.ResetPath();
    }
    public void TurnOnAgent(){
        animalAgent.enabled = true;
    }
    public void TurnOffAgent(){
        animalAgent.enabled = false;
    }
    
    // Utilitites
    public void SetSpeed(float speed){
        animalAgent.speed = speed;
    }
    public void ResetActions(){
        StopAgent();
        StopAllCoroutines();
    }
    public bool IsItCloserThan(GameObject target,float distanceThreshold){
        return Vector3.Distance(gameObject.transform.position,target.transform.position) < distanceThreshold;
    }
    public void SetId(int id){
        animalId = id;
    }
    public void GoToDestinationWithResets(string speed,Vector3 location){
        ResetActions();
        TurnOnAgent();
        if(speed == "walk") Walk();
        else if(speed == "run") Run();

        
        animalAgent.SetDestination(location);
    }
    public Vector3 GetRandomLocationAtArea(GameObject area) {
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
        if (NavMesh.SamplePosition(new Vector3(randomX, 0, randomZ), out hit, 10f, NavMesh.AllAreas)) {
            randomX = hit.position.x;
            randomZ = hit.position.z;
        }

        // Use the RANDOM positions for raycasting
        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(randomX, 50f, randomZ), Vector3.down, out groundHit, Mathf.Infinity)) {
            randomY = groundHit.point.y;
        }

        return new Vector3(randomX, randomY, randomZ);
    }
    // Animation utilities
    public void Walk(){
        animalAgent.isStopped = false;
        SetSpeed(animalWalkspeed);
        animalAnimator.PlayWalk();
    }
    public void Run(){
        SetSpeed(animalRunspeed);
        animalAnimator.PlayRun();
    }
    public void Eat(){
        SetSpeed(0);
        animalAnimator.PlayEat();
    }

    // Abstract Functions
    public abstract void Eat_Action();
    public abstract IEnumerator Eat_Action_Coroutine();

    public abstract void Sleep_Action();
    public abstract void Wandering_Action();
    public abstract IEnumerator Wandering_Action_Coroutine();

    public abstract void Go_Home_Action();

}
 