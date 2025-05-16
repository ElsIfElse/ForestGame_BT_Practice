
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stream_Chat : MonoBehaviour
{
    [HideInInspector]
    public string[] twitchUsernames = new string[]
    {
        "PixelFox", "GameWolf", "ChillCat", "TurboZap", "NovaByte",
        "ShadowX", "FrostyTV", "BlazeOrb", "QuickZap", "LunarAce",
        "Ghostly", "NitroFox", "ZenPlay", "ByteKing", "SkyDash",
        "EchoZap", "FireNix", "Starry", "IceWolf", "GlowZap",
        "Dashy", "PixelZap", "NovaX", "ChillVibes", "FrostZap",
        "TurboAce", "LunarZap", "GhostZap", "NitroDash", "ZenFox",
        "ByteZap", "SkyFox", "EchoByte", "FireDash", "StarZap",
        "IceZap", "GlowFox", "DashZap", "PixelAce", "NovaDash",
        "ChillByte", "FrostAce", "TurboByte", "LunarAce", "GhostByte",
        "NitroAce", "ZenZap", "ByteDash", "SkyZap", "EchoFox"
    };

    // WISHES
    [Header("Wishes")]
    string[] chatWishes = new string[] { "Bear", "Sheep", "Wolf", "Goat", "Rabbit", "HandCam" };
    public string currentWish = null;
    public string[] generalChatMessages;

    public string[] wishBearMessages;
    public string[] wishGoatMessages;
    public string[] wishWolfMessages;
    public string[] wishRabbitMessages;
    public string[] wishSheepMessages;
    public string[] wishHandCamMessages;
    public string[] wishFailedMessages;
    //
    [SerializeField] List<TextMeshProUGUI> chatMessageSlots = new List<TextMeshProUGUI>(15);
    List<String> messageList = new List<String>(15);
    //
    public float messageFrequencyCheck = 2f;

    public float percentageChanceToGetMessage = 10f;
    public float basePercentageToGetMessage = 10f;
    public float chatWishFrequencyCheck = 10f;
    public float percentageChanceToGetWish = 10f;
    public float wishMessageChanceMultiplier = 5f;
    //
    float messageTimer;
    float chatWishTimer;
    float wishFulfillmentTimer;
    float timeToFullfillWishTimer;
    [Space]

    [Header("Wish Settings")]
    [SerializeField] float requiredTimeToFullfillWish = 3f;
    [SerializeField] float timeToFullfillWish = 30f;

    public bool isWishActive = false;
    bool reminderSent = false;
    Manager_Collector managerCollector;
    Audio_Manager audioManager;
    Camera_Handler cameraHandler;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        audioManager = managerCollector.audioManager;
        cameraHandler = managerCollector.cameraHandler;

        percentageChanceToGetMessage = basePercentageToGetMessage;

        for (int i = 0; i < 15; i++)
        {
            messageList.Add("");
        }

        messageTimer = messageFrequencyCheck;

        generalChatMessages = new string[]{
        "That wolf just side-eyed the camera 😂",
        "Bear just vibin",
        "GOAT got hops fr",
        "Is the sheep okay??",
        "Bro that rabbit zoomed",
        "Theyre forming a squad 🐺🐑🐰",
        "Why is the bear so dramatic lol",
        "Sheep meeting in session 🐑🗣️",
        "That goat is on a mission",
        "Wolf tryna act casual 😅",
        "CAMERA JUMPSCARE 😭",
        "Bear.exe has stopped working",
        "Rabbit using stealth mode",
        "Someones getting hunted 👀",
        "This is better than Netflix",
        "Not the sheep doing parkour",
        "Bear has entered god mode",
        "Goat glitching into the matrix",
        "Who gave the rabbit coffee",
        "Wolf doing his villain arc again",
        "Can we get a bear emote??",
        "THEYRE FIGHTINGGG 🔥",
        "Thats not a goat, thats a mountain ninja",
        "Bear belly flop 10/10",
        "Zoom in on the drama pls",
        "Rabbit went full Naruto",
        "Sheep said: nope",
        "Goat jumped like 3 meters wth",
        "Wolf got the zoomies",
        "Bears nose in HD 💀",
        "This rabbits got better cardio than me",
        "Bunny hop detected 🐇💨",
        "I swear the sheep looked at me",
        "Goat ASMR incoming",
        "Wolf plotting something 100%",
        "Sheep just rage quit",
        "Bear hit the splits 😳",
        "This forest has lore",
        "Theyre doing Animal Survivor rn",
        "That goat is a parkour legend",
        "Rabbit jumped into another timeline",
        "Bear just slipped lol",
        "Who's the main character here?",
        "Camera needs a jumpcut editor 😂",
        "Wolf just t-posed for dominance",
        "Goat almost took the camera out 😭",
        "Wait… is that a staring contest?",
        "Animal drama incoming",
        "Bunny said ✨BYE✨",
        "Why do the sheep always look guilty?",
        "Bear sneezed and scared me"
    };
        wishBearMessages = new string[]{
            "Where's the bear??",
            "Show the bear again!",
            "I need more bear content please 🙏",
            "Bear cam when?",
            "Bring back the bear!!",
            "That bear better come back",
            "Can we get bear action?",
            "WHERE IS BEAR 😭",
            "I miss the bear already",
            "Bear was just getting interesting",
            "Please switch to bear 🐻",
            "Bear was the MVP",
            "I feel incomplete without the bear",
            "Give me BEAR",
            "Bear is my spirit animal",
            "Back to the bear plz",
            "B E A R C A M",
            "I just joined, where bear at?",
            "If bear returns, I’ll sub",
            "Bear made my day",
            "Bear energy is unmatched",
            "Can we vibe with the bear again?",
            "BEAR VIBES ONLY",
            "The bear deserves more screen time",
            "Bear stole the show, ngl",
            "We were robbed of bear moments",
            "Bear fans rise up 🐾",
            "Bear had the best drama",
            "Still thinking about that bear 🐻",
            "Where’d my fluffy king go?"
        };
        wishGoatMessages = new string[]{
            "Goat cam please!!",
            "Switch to the goat 🐐",
            "I NEED to see more goat parkour",
            "Where did the goat go??",
            "Goat = content gold",
            "Bring back GOATY",
            "Put the goat on stream again!",
            "Goat was climbing like a pro",
            "Goat jumps are life",
            "More goat less drama",
            "Can’t stop thinking about the goat",
            "Goat was in beast mode",
            "Goat content top tier",
            "More goat chaos pls",
            "Goat cam is underrated",
            "Return of the goat when?",
            "That goat better come back",
            "Put the goat back on! 🐐",
            "Why did we leave the goat!?",
            "Goat fans assemble",
            "He’s the real G.O.A.T",
            "Let me see that fluffy jumper again",
            "Goat supremacy forever",
            "Where is the cliff goat??",
            "I want to watch the goat do flips",
            "Goat stare hits different",
            "Back to the mountain goat please",
            "The goat was cooking fr",
            "I miss the parkour goat",
            "Switch back to goat now"
        };
        wishWolfMessages = new string[]{
            "Wolf cam when?",
            "Where’s the sneaky boy 🐺",
            "Wolf POV pls",
            "Give me wolf or give me sleep",
            "Switch back to the wolf!",
            "I need more wolf drama",
            "Wolf was up to something",
            "Wolf is my favorite 😭",
            "Back to villain cam (aka wolf)",
            "Bring back our emo king 🐺",
            "Wolf was just getting started",
            "Show me what wolf doing now",
            "Wolf moves in silence… bring him back",
            "I only came for the wolf",
            "Don’t leave us hanging on the wolf arc",
            "Wolf is content material",
            "Let’s follow wolf again pls",
            "Wolf cam = instant hype",
            "Wolf is cooking something 👀",
            "He’s planning something, I know it",
            "Return to wolf cam NOW",
            "Wolf fans rise 🐺",
            "Justice for wolf",
            "Bring back our fuzzy overlord",
            "Wolf deserves a spin-off show",
            "Why’d y’all switch off the wolf 😭",
            "Wolf tension was peak cinema",
            "More of the sassy wolf pls",
            "I bet wolf did something while we looked away",
            "WOLF CAM NOW"
        };
        wishRabbitMessages = new string[]{
            "Bring back the bunny!",
            "That rabbit was on a mission",
            "Rabbit cam again please 🐇",
            "I need to see the zoomies again",
            "Where’s the speedy lad?",
            "Rabbit was GOATED",
            "Switch back to the bun bun!",
            "I want more ninja rabbit",
            "Rabbit had that energy",
            "More hops please",
            "That bunny had no chill",
            "Bring back the fluffy chaos",
            "We need more rabbit energy",
            "Give us the stealth bunny again",
            "Rabbit made my day",
            "Where’d the speedy bean go?",
            "Rabbit fans stay winning",
            "Can’t stop thinking about that jump",
            "Put the rabbit back! 🐰",
            "I NEED the bunny arc to continue",
            "Rabbit drama was peaking",
            "Back to bun cam pls",
            "The rabbit was content gold",
            "Rabbit stole the show",
            "Zoomy bun incoming?",
            "Justice for rabbit cam",
            "Where bunny? Show bunny!",
            "Fluff cam when?",
            "Rabbit was the highlight tbh",
            "No rabbit, no peace"
        };
        wishSheepMessages = new string[]{
            "Where’s the sheep group chat?",
            "Put the sheep back on!",
            "I love sheep drama 🐑",
            "More confused sheep plz",
            "Sheep are up to something again",
            "Switch to sheep chaos now",
            "That sheep was sus 👀",
            "Can we go back to sheep cam?",
            "I need to know what the flock is up to",
            "Sheep squad rise up",
            "Bring back the woolies",
            "Sheep content is always underrated",
            "I feel spiritually connected to the sheep",
            "Sheep vibes = healing",
            "The sheep lore needs more screen time",
            "That sheep was doing side quests",
            "Sheep just vanished?? Bring back!",
            "More floofy chaos pls",
            "The sheep were scheming I swear",
            "Don’t cut away from the flock!",
            "I was invested in the sheep plotline",
            "We need sheep closure",
            "Back to floof cam",
            "Put the sheep squad back on stream",
            "I just tuned in, where sheep at?",
            "That sheep blinked weird, I need answers",
            "Bring back the silent drama kings",
            "Wool gang represent 🐑",
            "That one sheep had protagonist energy",
            "Sheep cam = serotonin"
        };
        wishHandCamMessages = new string[]{
            "Hand cam plz 🙏",
            "Let me see those magic fingers",
            "Hand cam when??",
            "I miss the hand cam 😭",
            "Show hands or it didn’t happen",
            "Where’s the hand POV?",
            "I need the hand action again",
            "Hand cam was peak content",
            "That petting technique was OP",
            "Let me see the setup again",
            "Camera on hands plsss",
            "Hand cam return soon?",
            "We need more hand footage",
            "I came for the hand cam",
            "Not gonna lie I miss the hand cam",
            "Hands were the real MVP",
            "Back to hand vibes?",
            "Petting cam when 🥺",
            "Bring back the human element",
            "I miss the chaos hands brought",
            "Camera on hands NOW",
            "Hands were cooking last time",
            "I want to see the controller again",
            "Let us see da hands 🫱",
            "Hand cam is oddly soothing",
            "Hands had better choreography than Broadway",
            "Need those IRL vibes again",
            "Petting montage when?",
            "I just want to see hand again 😭",
            "That hand cam was hypnotic"
        };
    }




    string RandomizeColor()
    {
        string[] colors = new string[] { "<color=red>", "<color=blue>", "<color=green>", "<color=yellow>", "<color=purple>", "<color=black>", "<color=orange>" };
        return colors[UnityEngine.Random.Range(0, colors.Length)];
    }

    void CreateRandomMessageAndAddToList()
    {
        switch (currentWish)
        {
            case "Rabbit":
                string randomRabbitWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishRabbitMessages[UnityEngine.Random.Range(0, wishRabbitMessages.Length)];
                messageList.Add(randomRabbitWishMessage);
                break;

            case "Sheep":
                string randomSheepWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishSheepMessages[UnityEngine.Random.Range(0, wishSheepMessages.Length)];
                messageList.Add(randomSheepWishMessage);
                break;

            case "Wolf":
                string randomWolfWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishWolfMessages[UnityEngine.Random.Range(0, wishWolfMessages.Length)];
                messageList.Add(randomWolfWishMessage);
                break;

            case "Goat":
                string randomGoatWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishGoatMessages[UnityEngine.Random.Range(0, wishGoatMessages.Length)];
                messageList.Add(randomGoatWishMessage);
                break;

            case "HandCam":
                string randomHandCamWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishHandCamMessages[UnityEngine.Random.Range(0, wishHandCamMessages.Length)];
                messageList.Add(randomHandCamWishMessage);
                break;

            case "Bear":
                string randomBearWishMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + wishBearMessages[UnityEngine.Random.Range(0, wishBearMessages.Length)];
                messageList.Add(randomBearWishMessage);
                break;

            default:
                string randomGeneralMessage = RandomizeColor() + twitchUsernames[UnityEngine.Random.Range(0, twitchUsernames.Length)] + "</color>" + ": " + generalChatMessages[UnityEngine.Random.Range(0, generalChatMessages.Length)];
                messageList.Add(randomGeneralMessage);
                break;
        }
    }

    void ResetMessageLocations()
    {
        messageList.RemoveAt(0);

        for (int i = 1; i < chatMessageSlots.Count; i++)
        {
            chatMessageSlots[i].text = messageList[i - 1];
        }
    }

    void AddMessage()
    {
        ResetMessageLocations();
        CreateRandomMessageAndAddToList();
        chatMessageSlots[0].text = messageList[0];
    }


    void Update()
    {
        GetMessagesRandomly();
        GetWish();

        FulfillingWishTimer();
        WishFailureTimer(); 
        Debug.Log(cameraHandler.CurrentAnimalType());
    }

    void GetMessagesRandomly()
    {
        messageTimer -= Time.deltaTime;

        if (messageTimer <= 0)
        {
            float randomNum = UnityEngine.Random.Range(0f, 100f);

            if (randomNum < percentageChanceToGetMessage)
            {
                AddMessage();
            }
            messageTimer = messageFrequencyCheck;
        }
    }

    void GetWish()
    {
        chatWishTimer -= Time.deltaTime;

        if (chatWishTimer <= 0)
        {
            float randomNum = UnityEngine.Random.Range(0f, 100f);

            if (randomNum < percentageChanceToGetWish && !isWishActive)
            {
                string wish = chatWishes[UnityEngine.Random.Range(0, chatWishes.Length)];

                if (wish == currentWish)
                {
                    Debug.Log("Wish Already Active");
                }
                else
                {
                    isWishActive = true;
                    audioManager.PlayChatNotification();
                    Debug.Log("New Wish: " + wish);
                    currentWish = wish;
                    percentageChanceToGetMessage = basePercentageToGetMessage * wishMessageChanceMultiplier;
                }
            }
            chatWishTimer = chatWishFrequencyCheck;
        }
    }

    void FulfillingWishTimer()
    {
        if (isWishActive && (cameraHandler.CurrentAnimalType() == currentWish || cameraHandler.CurrentlyViewedAnimalType() == currentWish))
        {
            wishFulfillmentTimer -= Time.deltaTime;

            if (wishFulfillmentTimer <= 0)
            {
                Debug.Log("Wish Fulfilled");
                audioManager.PlayWishFulfilled();
                isWishActive = false;
                currentWish = null;
                percentageChanceToGetMessage = basePercentageToGetMessage;
            }
        }
        else
        {
            wishFulfillmentTimer = requiredTimeToFullfillWish;
        }
    }
    void WishFailureTimer()
    {
        if (isWishActive)
        {
            timeToFullfillWishTimer -= Time.deltaTime;

            if (timeToFullfillWishTimer < timeToFullfillWish / 2 && !reminderSent)
            {
                reminderSent = true;
                audioManager.PlayChatNotification();
            }
            else if (timeToFullfillWishTimer <= 0)
            {
                Debug.Log("Wish Failed");
                audioManager.PlayWishFailed();
                isWishActive = false;
                currentWish = null;
                reminderSent = false;
                percentageChanceToGetMessage = basePercentageToGetMessage;
            }
        }

        else
        {
            timeToFullfillWishTimer = timeToFullfillWish;
        }
    }

}
