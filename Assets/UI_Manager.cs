
using TMPro;
using UnityEngine;


public class UI_Manager : MonoBehaviour
{
    World_Status worldStatus;
    Spawn_Manager spawnManager;
    Manager_Collector managerCollector;
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
    //
    [SerializeField] TextMeshProUGUI cameraType;
    [SerializeField] GameObject streamView;
    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        worldStatus = managerCollector.worldStatus;
        spawnManager = managerCollector.spawnManager;

        SetDay();
        SetTime();
        SetTimeSpeed();

        worldStatus.hourPassedEvent.AddListener(SetTime);
        worldStatus.dayPassedEvent.AddListener(SetDay);
        worldStatus.timeSpeedChanged.AddListener(SetTimeSpeed);

        worldStatus.wolfAdded.AddListener(SetWolfNumber);
        worldStatus.sheepAdded.AddListener(SetSheepNumber);
        worldStatus.rabbitAdded.AddListener(SetRabbitNumber);
        worldStatus.goatAdded.AddListener(SetGoatNumber);

        worldStatus.wolfRemoved.AddListener(SetWolfNumber);
        worldStatus.sheepRemoved.AddListener(SetSheepNumber);
        worldStatus.rabbitRemoved.AddListener(SetRabbitNumber);
        worldStatus.goatRemoved.AddListener(SetGoatNumber);

        SetWolfNumber();
        SetSheepNumber();
        SetRabbitNumber();
        SetGoatNumber();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){
            ToggleStreamView();
        }
        if(Input.GetKeyDown(KeyCode.I)){
            ToggleAnimalCard();
        }
    }
    public void SetTime(){
        timeText.text = "Time: " + worldStatus.currentTimeInHours + ":00";
    }
    public void SetDay(){
        dayText.text = "Day " + worldStatus.dayCounter;
    }
    public void SetTimeSpeed(){
        if(worldStatus.timeSpeed == 0){
            timeSpeed.text = "||";
        }
        if(worldStatus.timeSpeed == 1){
            timeSpeed.text = ">   1x";
        }
        if(worldStatus.timeSpeed == 2){
            timeSpeed.text = ">>  2x";
        }
        if(worldStatus.timeSpeed == 3){
            timeSpeed.text = ">>> 3x";
        }
    }
    void SetWolfNumber(){
        wolfCounterText.text ="Wolves: " + worldStatus.wolfDict.Count.ToString();
    }
    void SetSheepNumber(){
        sheepCounterText.text ="Sheeps: " + worldStatus.sheepDict.Count.ToString();
    }
    void SetRabbitNumber(){
        rabbitCounterText.text ="Rabbits: " + worldStatus.rabbitDict.Count.ToString();
    }
    void SetGoatNumber(){
        goatCounterText.text ="Goats: " + worldStatus.goatDict.Count.ToString();
    }
    //
    public void SetAnimalName(string currentName){
        animalName_Text.text = currentName;
    }
    public void SetAnimalAge(int currentAge){
        animalAge_Text.text = currentAge.ToString();
    }
    public void SetAnimalImage(string animalType){
        switch(animalType){
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
        }
    }
    public void TurnOnAnimalCard(){
        animalCard.SetActive(true);
    }
    public void TurnOffAnimalCard(){
        animalCard.SetActive(false);
    }
    void ToggleStreamView(){
        streamView.SetActive(!streamView.activeSelf);
    }
    void ToggleAnimalCard(){
        animalCard.SetActive(!animalCard.activeSelf);
    }
    public void HandHeldCamera(){
        cameraType.text = "Hand_Cam";
    }
    public void AnimalCamera(){
        cameraType.text = "Animal_Cam";
    }
}
