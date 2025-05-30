using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Audio_Manager : MonoBehaviour
{
    Queue<AudioSource> audioSources = new Queue<AudioSource>();
    int initialPoolSize = 5;
    Vector3 defaultSpawnLocation = Vector3.zero;

    [Header("Audio Clips")]

    public AudioClip nightAmbiance;
    public AudioClip rainSound;
    public AudioClip forestAmbianceSound;

    [Space]
    [Header("Stream Sounds")]
    public AudioClip chatWishNotificationSound;
    public AudioClip wishFulfilledSound;
    public AudioClip wishFailedSound;
    public AudioClip tabletOut;
    public AudioClip cameraSwitchSound;
    //
    [Space]
    [Header("Thunder Clips")]

    public AudioClip[] thunderClips;

    [Space]
    [Header("Player Sounds")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip jumpSound_ground;
    [SerializeField] private AudioClip jumpSound_exhale;
    [SerializeField] private AudioClip animalBecameFriendly;
    [SerializeField] private AudioClip feedingAnimal;
    [SerializeField] private AudioClip cantDoIt;
    [SerializeField] private AudioClip setupCamera_01;
    [SerializeField] private AudioClip setupCamera_02;

    [Space]
    [Header("Score Clips")]
    [SerializeField] private AudioClip score_01;
    AudioSource scoreSource;

    [Space]
    [Header("UI Sounds")]
    [SerializeField] private AudioClip uiClickSound;
    AudioSource walkSource;
    AudioSource runSource;

    AudioSource rainSource;
    AudioSource forestAmbianceSource;
    //
    Manager_Collector managerCollector;
    World_Status worldStatus;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        worldStatus = managerCollector.worldStatus;
            
        for(int i = 0; i < initialPoolSize; i++){
            CreateAudioSource();
        }

        InitializeRainAudioSource();
        InitializeForestAmbianceSource();
        InitializeScoreSource();

        InitializeWalkSoundSource();
        InitializeRunSoundSource();

        worldStatus.thunderEvent.AddListener(PlayThunder);
    }

    // SCORE
    void InitializeScoreSource()
    {   
        scoreSource = new GameObject().AddComponent<AudioSource>();
        scoreSource.clip = score_01;
        scoreSource.volume = 1f;
        scoreSource.Play(); 
    }

    // UI
    public void PlayUIClickSound()
    {
        PlayAudio(uiClickSound, 0.7f, true, false);
    }

    // Walk
    void InitializeWalkSoundSource()
    {
        GameObject walkObject = new GameObject();
        walkObject.name = "Walk AudioSource";
        walkSource = walkObject.AddComponent<AudioSource>();
        walkSource.clip = walkSound;
        walkSource.loop = true;

        walkSource.time = 1.9f;

        walkSource.Play();
    }
    public void PlayWalkSound(){
        walkSource.volume = 0.25f;
    }
    public void StopWalkSound(){
        walkSource.volume = 0f;
    }

    // Run
    void InitializeRunSoundSource()
    {
        GameObject walkObject = new GameObject();
        walkObject.name = "Run AudioSource";
        runSource = walkObject.AddComponent<AudioSource>();
        runSource.clip = runSound;
        runSource.loop = true;

        runSource.time = 1.9f;

        runSource.Play();
    }
    public void PlayRunSound(){
        runSource.volume = 0.35f;
    }
    public void StopRunSound(){
        runSource.volume = 0f;
    }

    // Forest
    void InitializeForestAmbianceSource(){
        GameObject forestSoundObject = new GameObject();
        forestSoundObject.name = "Forest_Ambiance AudioSource";
        forestAmbianceSource = forestSoundObject.AddComponent<AudioSource>();

        forestAmbianceSource.clip = forestAmbianceSound;
        forestAmbianceSource.volume = 0.5f;
        forestAmbianceSource.loop = true;

        worldStatus.rainStarted.AddListener(StopForestAmbiance);
        worldStatus.rainStopped.AddListener(PlayForestAmbiance);

        PlayForestAmbiance();
    }
    void PlayForestAmbiance(){
        forestAmbianceSource.Play();
        forestAmbianceSource.DOFade(0.5f,2f);
    }
    void StopForestAmbiance(){
        forestAmbianceSource.DOFade(0.0f,2f);
        StartCoroutine(Timer(2f,forestAmbianceSource.Stop));
        
    }
    
    // Rain
    void InitializeRainAudioSource(){

        GameObject rainObject = new GameObject();
        rainObject.name = "Rain AudioSource";
        rainSource = rainObject.AddComponent<AudioSource>();

        rainSource.clip = rainSound;
        rainSource.volume = 0.5f;
        rainSource.loop = true;

        worldStatus.rainStarted.AddListener(PlayRain);
        worldStatus.rainStopped.AddListener(StopRain);
    }
    public void PlayRain(){
        rainSource.Play();
        rainSource.DOFade(0.5f,2);
    }
    public void StopRain(){
        rainSource.DOFade(0.0f,2);
        StartCoroutine(Timer(2f,rainSource.Stop));
    }

    // Pool Sounds
    public void PlayThunder(){
        PlayAudio(thunderClips[UnityEngine.Random.Range(0,thunderClips.Length)],0.5f,true,false,1.5f);
    }
    public void PlayBirds(){
        PlayAudio(forestAmbianceSound,0.5f,true,true);
    }
    public void PlayNight(){
        PlayAudio(nightAmbiance,0.5f,true,true);
    }
    public void PlayChatNotification(){
        PlayAudio(chatWishNotificationSound,0.5f);
    }
    public void PlayWishFulfilled(){
        PlayAudio(wishFulfilledSound,0.5f);
    }
    public void PlayWishFailed()
    {
        PlayAudio(wishFailedSound, 0.5f);
    }
    public void PlayTabletToggle()
    {
        PlayAudio(tabletOut, 0.7f,true);
    }
    public void PlayJumpSound()
    {
        PlayAudio(jumpSound_ground, 0.2f);
        PlayAudio(jumpSound_exhale, 0.5f);
    }
    public void PlayCameraSwitchSound()
    {
        PlayAudio(cameraSwitchSound, 0.5f,true);
    }

    // Interaction Sounds
    public void PlayPickSound()
    {
        PlayAudio(pickSound, 0.3f,true,false,0.23f);
    }  
    public void PlayAnimalBecameFriendly(){
        PlayAudio(animalBecameFriendly,0.5f);
    }
    public void PlayFeedingAnimal()
    {
        PlayAudio(feedingAnimal, 0.5f);
    }
    public void PlayCantDoIt()
    {
        PlayAudio(cantDoIt, 0.5f);
    }
    public void PlaySetupCamera_01()
    {
        PlayAudio(setupCamera_01, 0.5f);
    }
    public void PlaySetupCamera_02()
    {
        PlayAudio(setupCamera_02, 0.5f);
    }
    // Utility
    void PlayAudio(AudioClip clipToPlay,float volume = 0.5f,bool isRandomPitch = false,bool isLooping = false,float startFrom = 0,float delay = 0){
        
        if(audioSources.Count == 0){
            for(int i = 0; i < initialPoolSize*2; i++){
                CreateAudioSource();
            }

            initialPoolSize *= 2;
            Debug.Log("Audio pool was 0. Created audiosource with Buffer");
        }
        
        AudioSource source = audioSources.Dequeue();

        if(isRandomPitch){
            source.pitch = UnityEngine.Random.Range(0.85f,1.2f);
        }
        else{
            source.pitch = 1f;
        }

        source.clip = clipToPlay;
        source.volume = volume;
        source.time = startFrom;
        source.PlayDelayed(delay);

        StartCoroutine(EnqueueWhenFinished_Coroutine(source));

    }
    void CreateAudioSource(Vector3? spawnLocation = null){
        Vector3 objectSpawnLocation = spawnLocation ?? defaultSpawnLocation;

        GameObject audioSourceObject = new GameObject();

        audioSourceObject.AddComponent<AudioSource>();
        audioSourceObject.name = "Pooled AudioSource";
        audioSourceObject.transform.parent = transform;
        audioSourceObject.transform.localPosition = objectSpawnLocation;
        audioSourceObject.transform.localRotation = Quaternion.identity;
        audioSourceObject.transform.localScale = Vector3.one;

        audioSources.Enqueue(audioSourceObject.GetComponent<AudioSource>());


    }
    IEnumerator EnqueueWhenFinished_Coroutine(AudioSource source){
        while(source.isPlaying == true){
            yield return null;
        }

        audioSources.Enqueue(source);
    }
    IEnumerator Timer(float timeToWait,Action action){
        yield return new WaitForSeconds(timeToWait);
        action();
    }


}
