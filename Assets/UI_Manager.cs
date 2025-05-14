using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    World_Status worldStatus;
    Spawn_Manager spawnManager;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeSpeed;
    //
    public TextMeshProUGUI wolfCounterText;
    public TextMeshProUGUI sheepCounterText;
    public TextMeshProUGUI rabbitCounterText;
    public TextMeshProUGUI goatCounterText;
    //
    Manager_Collector managerCollector;

    void Awake()
    {

    }
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
}
