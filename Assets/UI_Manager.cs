
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    World_Status worldStatus;
    Spawn_Manager spawnManager;
    Manager_Collector managerCollector;
    Audio_Manager audioManager;
    Animal_Collection animalCollection;
    //
    [Header("Time Texts")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeSpeed;
    //
    [Space]
    [Header("Counter Texts")]
    public TextMeshProUGUI wolfCounterText;
    public TextMeshProUGUI sheepCounterText;
    public TextMeshProUGUI rabbitCounterText;
    public TextMeshProUGUI goatCounterText;
    //
    [Space]
    [Header("Animal Card UI Elements")]
    public GameObject animalCard;
    public TextMeshProUGUI animalName_Text;
    public TextMeshProUGUI animalAge_Text;
    public UnityEngine.UI.Image animalImage_Image;
    //
    [Space]
    [Header("Animal Sprites")]
    public Sprite wolfImage;
    public Sprite sheepImage;
    public Sprite rabbitImage;
    public Sprite goatImage;
    public Sprite bearImage;
    //
    [Space]
    [Header("Stream UI Elements")]
    [SerializeField] TextMeshProUGUI viewerCounter;
    [SerializeField] TextMeshProUGUI cameraType;
    [SerializeField] GameObject streamView;

    [Space]
    [Header("Interaction Indicators")]
    [SerializeField] Sprite indicator_PickFood_Sprite;
    [SerializeField] Sprite indicator_FeedAnimal_Sprite;
    [SerializeField] Sprite indicator_SetCameraOnAnimal_Sprite;
    [SerializeField] Sprite indicator_OpenChest_Sprite;
    [SerializeField] Sprite indicator_CraftingTable_Sprite;
    [SerializeField] Sprite indicator_SetupCamera_Sprite;
    [SerializeField] UnityEngine.UI.Image currentIndicator;

    [SerializeField] GameObject indicatorObject;
    [SerializeField] Image indicatorBackgroundImage;
    [SerializeField] Image indicatorFrameImage;

    [Space]
    [Header("Crafting")]
    [SerializeField] public GameObject craftingTable_UI;
    float indicatorFadeTime = 0.5f;
    //
    public bool isPhoneOut = false;
    float viewershipRandomizerCounter;
    float viewershipRandomizerFrequency = 1f;
    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        worldStatus = managerCollector.worldStatus;
        spawnManager = managerCollector.spawnManager;
        audioManager = managerCollector.audioManager;
        animalCollection = managerCollector.animalCollection;

        indicatorBackgroundImage = indicatorObject.transform.Find("BG").GetComponent<Image>();
        indicatorFrameImage = indicatorObject.transform.Find("Frame").GetComponent<Image>();

        SetDay();
        SetTime();
        SetTimeSpeed();

        worldStatus.hourPassedEvent.AddListener(SetTime);
        worldStatus.dayPassedEvent.AddListener(SetDay);
        worldStatus.timeSpeedChanged.AddListener(SetTimeSpeed);

        animalCollection.wolfAdded.AddListener(SetWolfNumber);
        animalCollection.sheepAdded.AddListener(SetSheepNumber);
        animalCollection.rabbitAdded.AddListener(SetRabbitNumber);
        animalCollection.goatAdded.AddListener(SetGoatNumber);

        animalCollection.wolfRemoved.AddListener(SetWolfNumber);
        animalCollection.sheepRemoved.AddListener(SetSheepNumber);
        animalCollection.rabbitRemoved.AddListener(SetRabbitNumber);
        animalCollection.goatRemoved.AddListener(SetGoatNumber);

        SetWolfNumber();
        SetSheepNumber();
        SetRabbitNumber();
        SetGoatNumber();

        viewershipRandomizerCounter = viewershipRandomizerFrequency;
        RandomizerViewershipBetween(10, 40);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleStreamView();
            audioManager.PlayTabletToggle();
            isPhoneOut = !isPhoneOut;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleAnimalCard();
        }

        RandomizerViewership();
    }
    public void SetTime()
    {
        timeText.text = "Time: " + worldStatus.currentTimeInHours + ":00";
    }
    public void SetDay()
    {
        dayText.text = "Day " + worldStatus.dayCounter;
    }
    public void SetTimeSpeed()
    {
        if (worldStatus.timeSpeed == 0)
        {
            timeSpeed.text = "||";
        }
        if (worldStatus.timeSpeed == 1)
        {
            timeSpeed.text = ">   1x";
        }
        if (worldStatus.timeSpeed == 2)
        {
            timeSpeed.text = ">>  2x";
        }
        if (worldStatus.timeSpeed == 3)
        {
            timeSpeed.text = ">>> 3x";
        }
    }
    void SetWolfNumber()
    {
        wolfCounterText.text = "Wolves: " + animalCollection.wolfDict.Count.ToString();
    }
    void SetSheepNumber()
    {
        sheepCounterText.text = "Sheeps: " + animalCollection.sheepDict.Count.ToString();
    }
    void SetRabbitNumber()
    {
        rabbitCounterText.text = "Rabbits: " + animalCollection.rabbitDict.Count.ToString();
    }
    void SetGoatNumber()
    {
        goatCounterText.text = "Goats: " + animalCollection.goatDict.Count.ToString();
    }

    // Card Settings
    public void SetAnimalName(string currentName)
    {
        animalName_Text.text = currentName;
    }
    public void SetAnimalAge(int currentAge)
    {
        animalAge_Text.text = currentAge.ToString();
    }
    public void SetAnimalImage(string animalType)
    {
        switch (animalType)
        {
            case "Sheep":
                animalImage_Image.sprite = sheepImage;
                break;
            case "Wolf":
                animalImage_Image.sprite = wolfImage;
                break;
            case "Rabbit":
                animalImage_Image.sprite = rabbitImage;
                break;
            case "Goat":
                animalImage_Image.sprite = goatImage;
                break;
            case "Bear":
                animalImage_Image.sprite = bearImage;
                break;
        }
    }
    public void TurnOnAnimalCard()
    {
        animalCard.SetActive(true);
    }
    public void TurnOffAnimalCard()
    {
        animalCard.SetActive(false);
    }
    void ToggleStreamView()
    {
        streamView.SetActive(!streamView.activeSelf);
    }
    void ToggleAnimalCard()
    {
        animalCard.SetActive(!animalCard.activeSelf);
    }
    public void HandHeldCamera()
    {
        cameraType.text = "Hand_Cam";
    }
    public void AnimalCamera()
    {
        cameraType.text = "Animal_Cam";
    }

    // Viewership
    public void SetViewerCounter(int currentViewers)
    {
        viewerCounter.text = currentViewers.ToString();
    }
    void RandomizerViewershipBetween(int min, int max)
    {
        int randomViewerNum = Random.Range(min, max);
        SetViewerCounter(randomViewerNum);
    }
    void RandomizerViewership()
    {
        viewershipRandomizerCounter -= Time.deltaTime;

        if (viewershipRandomizerCounter <= 0)
        {
            float chanceToChangeViewerNumber = Random.Range(0, 100);

            if (chanceToChangeViewerNumber < 10)
            {
                RandomizerViewershipBetween(10, 40);
            }

            viewershipRandomizerCounter = viewershipRandomizerFrequency;

        }


    }

    // Interaction Indicators
    public void TurnOnIndicator_PickFood()
    {
        indicatorObject.SetActive(true);
        currentIndicator.sprite = indicator_PickFood_Sprite;
        FadeIndicatorIn();
    }
    public void TurnOnIndicator_FeedAnimal()
    {
        indicatorObject.SetActive(true);
        currentIndicator.sprite = indicator_FeedAnimal_Sprite;
        FadeIndicatorIn();
    }
    public void TurnOnIndicator_OpenChest()
    {
        indicatorObject.SetActive(true);
        currentIndicator.sprite = indicator_OpenChest_Sprite;
        FadeIndicatorIn();
    }
    public void TurnOnIndicator_SetupCamera()
    {
        indicatorObject.SetActive(true);
        currentIndicator.sprite = indicator_SetupCamera_Sprite;
        FadeIndicatorIn();
    }
    public void TurnOnIndicator_CraftingTable()
    {
        indicatorObject.SetActive(true);
        currentIndicator.sprite = indicator_CraftingTable_Sprite;
        FadeIndicatorIn();
    }
    // public void TurnOnIndicator_SetCameraOnAnmial()
    // {
    //     indicatorObject.SetActive(true);
    //     currentIndicator.sprite = indicator_SetCameraOnAnimal_Sprite;
    // }
    public void TurnOffIndicator()
    {
        FadeIndicatorOut();
        indicatorObject.SetActive(false);
    }
    void FadeIndicatorIn()
    {
        indicatorBackgroundImage.DOFade(1, indicatorFadeTime);
        indicatorFrameImage.DOFade(1, indicatorFadeTime);
        currentIndicator.DOFade(1, indicatorFadeTime);
    }
    void FadeIndicatorOut()
    {
        indicatorBackgroundImage.DOFade(0, indicatorFadeTime);
        indicatorFrameImage.DOFade(0, indicatorFadeTime);
        currentIndicator.DOFade(0, indicatorFadeTime);
    }
    
    // Crafting
    public void TurnOnCraftingTable()
    {
        craftingTable_UI.SetActive(true);
    }
    public void TurnOffCraftingTable()
    {
        craftingTable_UI.SetActive(false);
    }
    

    
}
