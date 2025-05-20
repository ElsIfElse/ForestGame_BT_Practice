using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.Rendering;
using System.Collections;
using System;

public class World_Status : MonoBehaviour
{
    public bool isDay = true;
    bool isRaining = false;
    //
    public float dayCounter = 0;
    public float currentTimeInHours = 7;
    public float hourLengthInSeconds = 2;
    float sunsetTime = 20;
    float sunriseTime = 7;
    //
    [HideInInspector]
    public float hourTimerCounter;
    public float thunderTimerCounter;
    public int timeSpeed;
    //
    [Space]
    public Material daySkybox;
    public Material nightSkybox;
    public Light sun;
    Color defaultAmbientColor;
    Color nightAmbientColor;

    [Space]
    [HideInInspector] public UnityEvent hourPassedEvent;
    
    [HideInInspector] public UnityEvent dayPassedEvent;
    
    [HideInInspector] public UnityEvent timeSpeedChanged;

    //
    
    [HideInInspector] public UnityEvent wolfAdded;
    [HideInInspector] public UnityEvent sheepAdded;
    [HideInInspector] public UnityEvent rabbitAdded;
    [HideInInspector] public UnityEvent goatAdded;
    [HideInInspector] public UnityEvent bearAdded;
    
    [HideInInspector] public UnityEvent wolfRemoved;
    [HideInInspector] public UnityEvent sheepRemoved;
    [HideInInspector] public UnityEvent rabbitRemoved;
    [HideInInspector] public UnityEvent goatRemoved;
    [HideInInspector] public UnityEvent bearRemoved;
    //
    [HideInInspector]
    public UnityEvent rainStarted;
    [HideInInspector]
    public UnityEvent rainStopped;
    [HideInInspector]
    public UnityEvent thunderEvent;
    //
    [HideInInspector]
    public List<GameObject> sheeps = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> wolves = new List<GameObject>();
    //
    [HideInInspector]
    public Dictionary<int,GameObject> sheepDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> wolfDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> rabbitDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> goatDict = new Dictionary<int,GameObject>();
    public Dictionary<int,GameObject> bearDict = new Dictionary<int,GameObject>();

    int objectId = 0;
    //
    [Space]
    [Header("Weather")]
    public int chanceForRain;
    public int chanceForRainToStop;
    public int chanceForThunder;
    public float thunderCheckInterval;
    ParticleSystem rainParticle;
    [SerializeField] ParticleSystem lightningParticle;
    [SerializeField] GameObject lightningSpawnArea;
    //
    Color ogSunColor;



    void Awake()
    {
        rainParticle = Camera.main.transform.Find("Rain").gameObject.GetComponent<ParticleSystem>();
        rainParticle.Stop();

        lightningParticle.Stop();
        defaultAmbientColor = RenderSettings.ambientLight;
        nightAmbientColor = Color.black;
    }
    void Start()
    {
        RenderSettings.skybox = daySkybox;
        hourTimerCounter = hourLengthInSeconds;

        ogSunColor = sun.color;

        hourPassedEvent.AddListener(RandomChanceForRain);

        dayPassedEvent.AddListener(SetFog);
        rainStopped.AddListener(SetFog);
        rainStarted.AddListener(SetFog);

   
    }
    void Update()
    {
        if (isDay == true)
        {
            RenderSettings.skybox = daySkybox;
            sun.DOIntensity(1, 4).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.00f, 4).SetEase(Ease.Linear);
            RenderSettings.ambientLight = defaultAmbientColor;
            DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, 1f, 10).SetEase(Ease.Linear);
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
            sun.DOIntensity(0.0f, 4).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.015f, 4).SetEase(Ease.Linear);
            RenderSettings.ambientLight = nightAmbientColor;
            DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, 0.15f, 10).SetEase(Ease.Linear);
        }

        HourPass();
        SunsetSunrise();
        HandleTimeSpeed();
        ThunderTimer();

        //
        GOD_MODE();

    }
    // Time Management
    void SunsetSunrise(){
        if(currentTimeInHours >= sunriseTime && currentTimeInHours < sunsetTime){
            isDay = true;
        }
        else{
            isDay = false;
        }

    }
    void HourPass(){
        hourTimerCounter -= Time.deltaTime;

        if(hourTimerCounter <= 0){
            currentTimeInHours++;
            hourTimerCounter = hourLengthInSeconds;

            hourPassedEvent.Invoke();
            if(currentTimeInHours == 24){
                dayCounter++; 
                dayPassedEvent.Invoke();
                currentTimeInHours = 0;
            }
        }
    }
    void HandleTimeSpeed(){
        if(Input.GetKeyDown(KeyCode.P)){
            timeSpeed = 0;
            timeSpeedChanged.Invoke();
            Time.timeScale = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            timeSpeed = 1;
            timeSpeedChanged.Invoke();
            Time.timeScale = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            timeSpeed = 2;
            timeSpeedChanged.Invoke();
            Time.timeScale = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            timeSpeed = 3;
            timeSpeedChanged.Invoke();
            Time.timeScale = 3;
        }
    }
    
    // Dictionary Handling
    public void AddWolf(GameObject wolf){
        wolfDict.Add(objectId,wolf);
        objectId++;

        wolfAdded.Invoke();
    }
    public void AddSheep(GameObject sheep){
        sheepDict.Add(objectId,sheep);
        objectId++;

        sheepAdded.Invoke();
    }
    public void AddRabbit(GameObject rabbit){
        rabbitDict.Add(objectId,rabbit);
        objectId++;

        rabbitAdded.Invoke();
    }
    public void AddGoat(GameObject goat){
        goatDict.Add(objectId,goat);
        objectId++;

        goatAdded.Invoke();
    }
    public void AddBear(GameObject bear)
    {
        bearDict.Add(objectId,bear);
        objectId++;

        bearAdded.Invoke();   
    }
    public void RemoveWolf(int id){
        Addressables.ReleaseInstance(wolfDict[id]);
        wolfDict.Remove(id);
        wolfRemoved.Invoke();
    }
    public void RemoveSheep(int id){
        Addressables.ReleaseInstance(sheepDict[id]);
        sheepDict.Remove(id);
        sheepRemoved.Invoke();
    }
    public void RemoveRabbit(int id){
        Addressables.ReleaseInstance(rabbitDict[id]);
        rabbitDict.Remove(id);
        rabbitRemoved.Invoke();
    }
    public void RemoveGoat(int id){
        Addressables.ReleaseInstance(goatDict[id]);
        goatDict.Remove(id);
        goatRemoved.Invoke();
    }
    public void RemoveBear(int id){
        Addressables.ReleaseInstance(bearDict[id]);
        bearDict.Remove(id);
        bearRemoved.Invoke();
    }
    public void RemoveAnimal(GameObject targetAnimal){
        string animalBreed = targetAnimal.GetComponent<AnimalBlackboard_Base>().animalBreed;
        switch (animalBreed)
        {
            case "Wolf":
            StartCoroutine(Remover_Coroutine(()=>RemoveWolf(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId)));
                // RemoveWolf(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Sheep":
                StartCoroutine(Remover_Coroutine(()=>RemoveSheep(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),2.7f));

                // RemoveSheep(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Rabbit":
                StartCoroutine(Remover_Coroutine(()=>RemoveRabbit(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),1.1f));

                // RemoveRabbit(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Goat":
                StartCoroutine(Remover_Coroutine(()=>RemoveGoat(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),2.7f));

                // RemoveGoat(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;
                
            case "Bear":
                StartCoroutine(Remover_Coroutine(()=>RemoveBear(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId)));

                // RemoveBear(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;
        }
    }

    IEnumerator Remover_Coroutine(Action action,float timeToWait = 0f){
        yield return new WaitForSeconds(timeToWait);
        action();
    }

    // Weather Settings
    void RandomChanceForRain(){
        if(!isRaining){
            float randomChance = UnityEngine.Random.Range(0f,100f);

            if(randomChance < chanceForRain){
                rainStarted.Invoke();

                rainParticle.GetComponent<ParticleSystem>().Play();
                isRaining = true;
            }
        }

        else{
            float randomChance = UnityEngine.Random.Range(0f,100f);

            if(randomChance < chanceForRainToStop){
                rainStopped.Invoke();

                rainParticle.GetComponent<ParticleSystem>().Stop();
                isRaining = false;
            }
        }
    }

    void RandomChanceForThunder(){
        float randomChance = UnityEngine.Random.Range(0f,100f);
        if(randomChance < chanceForThunder){
            Thunder();
        }
    }

    void Thunder(){
        float areaCenterX = lightningSpawnArea.transform.position.x;
        float areaCenterZ = lightningSpawnArea.transform.position.z;

        float areaHalfWidth = lightningSpawnArea.transform.localScale.x / 2f;
        float areaHalfDepth = lightningSpawnArea.transform.localScale.z / 2f;

        // Generate random positions within the area
        float randomX = UnityEngine.Random.Range(areaCenterX - areaHalfWidth, areaCenterX + areaHalfWidth);
        float randomZ = UnityEngine.Random.Range(areaCenterZ - areaHalfDepth, areaCenterZ + areaHalfDepth);
        float randomY = 0;

        // Use the RANDOM positions for NavMesh sampling

        // Use the RANDOM positions for raycasting
        RaycastHit groundHit;
        if (Physics.Raycast(new Vector3(randomX, 50f, randomZ), Vector3.down, out groundHit, Mathf.Infinity)) {
            randomY = groundHit.point.y;
        }

        lightningParticle.transform.position = new Vector3(randomX, randomY, randomZ);
        lightningParticle.GetComponent<ParticleSystem>().Play();
        thunderEvent.Invoke();
    }
    void GOD_MODE(){
        if(Input.GetKeyDown(KeyCode.T)){
           Thunder();
        }
    }
    void ThunderTimer(){
        if(isRaining){
            thunderTimerCounter -= Time.deltaTime;

            if(thunderTimerCounter <= 0){
                thunderTimerCounter = thunderCheckInterval;
                RandomChanceForThunder();
            }
        }
    }
    void SetFog(){
        if(isRaining){
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.01f, 3).SetEase(Ease.Linear);
            sun.DOIntensity(0.3f,3).SetEase(Ease.Linear);
        }
        else{
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.00f, 3).SetEase(Ease.Linear);
            sun.DOIntensity(1,3).SetEase(Ease.Linear);
        }
    }
} 
