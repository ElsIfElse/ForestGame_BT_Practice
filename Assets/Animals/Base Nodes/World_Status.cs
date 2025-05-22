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
    

    //
    [HideInInspector]
    public UnityEvent rainStarted;
    [HideInInspector]
    public UnityEvent rainStopped;
    [HideInInspector]
    public UnityEvent thunderEvent;
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
        // HandleTimeSpeed();
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
