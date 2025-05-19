using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Camera_Handler : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera handHeldCamera;
    [Space]
    [Header("Camera Follow")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 10, -10);
    [SerializeField] private Vector3 originalPosition = new Vector3(0, 26, -120);
    [SerializeField] private float followSmoothness = 5f;

    [Space]
    [Space]
    [Header("Camera Movement")]
    [SerializeField] private float handheldCamSmoothness = 5f;
    [SerializeField] private float animalCamSmoothness = 5f;
    [SerializeField] private float movementSpeed = 10f;

    [Space]
    [Header("Camera Zoom")]
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float cameraBaseZoom = 120;
    [SerializeField] private float cameraMaxZoom = 40;
    [SerializeField] private float cameraMinZoom = 80;

    [Space]
    [Header("Flashlight")]
    [SerializeField] private Light flashlight;
    [SerializeField] private float flashlightIntensity = 10f;
    [SerializeField] private int[] flashLightStrengthLevel = new int[4] { 0, 10, 20, 30 };
    [SerializeField] private int[] flashLightRanges = new int[4] { 0, 10, 20, 30 };
    private int flashLightStrengthLevelCounter = 0;
    [SerializeField] private bool isFlashlightOn = false;


    private List<GameObject> animals = new();
    private List<GameObject> wolfs = new();
    private List<GameObject> preys = new();

    private int animalIndex = 0;
    private GameObject followedAnimal;
    private bool followAnimal = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    UnityEvent followAnimalMode = new();
    UnityEvent baseMode = new();
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile baseProfile;
    [SerializeField] private VolumeProfile cameraProfile;
    //
    bool isSheepDebugOn = true;
    bool isCamHandHeld = true;
    bool isPhoneOut = false;
    //
    Manager_Collector managerCollector;
    UI_Manager uiManager;
    Audio_Manager audioManager;
    [SerializeField] GameObject handcamPositionObject;
    [SerializeField] GameObject crosshair;
    Ray handheldCameraRay;

    //
    [Space]
    [Header("Handheld Raycasting")]
    [SerializeField] LayerMask handheldRayLayerMask;
    void Start()
    {
        CollectAnimals();
        targetPosition = originalPosition;
        targetRotation = transform.rotation;

        followAnimalMode.AddListener(CameraVolume);
        baseMode.AddListener(BaseVolume);

        DebugReferences();

        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        uiManager = managerCollector.uiManager;
        audioManager = managerCollector.audioManager;

        CameraVolume();
        followedAnimal = handcamPositionObject;

        
    }

    void Update()
    {
        Debug.DrawRay(handheldCameraRay.origin, handheldCameraRay.direction * 20f, Color.red);

        HandleFollowAnimal();
        SmoothCameraMovement();
        HandleHandHeldCameraZoom();

        HandleFlashlight();
        Crosshair();
        // DEPRECATED FUNCTIONS

        // HandleCameraMovement();
        // HandleCameraZoom();
        // ClickingOnAnimal();
        // GODMODE();

        Debug.Log("Currently handheld animal ray hit: " + CurrentlyViewedAnimalType());
    }

    public void CollectAnimals()
    {
        animals.Clear();

        preys = GameObject.FindGameObjectsWithTag("Prey").ToList();
        wolfs = GameObject.FindGameObjectsWithTag("Predator").ToList();

        animals.AddRange(wolfs);
        animals.AddRange(preys);

        // Debug.Log("Animals Found: " + animals.Count);
    }

    private void HandleFollowAnimal()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            isCamHandHeld = false;
            if (animals.Count == 0)
            {
                Debug.Log("Animal List is emmpty. Attempting to collect animals.");
                CollectAnimals();
            }
            ;

            animalIndex++;
            if (animalIndex >= animals.Count) animalIndex = 0;
            followedAnimal = animals[animalIndex];

            uiManager.AnimalCamera();
            SetAnimalCard(followedAnimal);
            audioManager.PlayCameraSwitchSound();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            isCamHandHeld = false;
            if (animals.Count == 0)
            {
                Debug.Log("Animal List is emmpty. Attempting to collect animals.");
                CollectAnimals();
            };

            animalIndex--;
            if (animalIndex < 0) animalIndex = animals.Count - 1;
            followedAnimal = animals[animalIndex];
            uiManager.AnimalCamera();
            SetAnimalCard(followedAnimal);
            audioManager.PlayCameraSwitchSound();
        }

        else if (Input.GetKeyDown(KeyCode.V))
        {
            followedAnimal = handcamPositionObject;
            uiManager.HandHeldCamera();
            audioManager.PlayCameraSwitchSound();
        }

        Transform cameraHolderTransform = followedAnimal.transform.FindDeepChild("CameraHolder");

        if (cameraHolderTransform == null)
        {
            cameraHolderTransform = handcamPositionObject.transform;
            uiManager.TurnOffAnimalCard();
            isCamHandHeld = true;
        }

        Vector3 cameraHolderLocation = cameraHolderTransform.position;
        targetPosition = cameraHolderLocation;
        targetRotation = Quaternion.LookRotation(cameraHolderTransform.forward);
    }

    private void SmoothCameraMovement()
    {
        if (isCamHandHeld)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, handheldCamSmoothness * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, handheldCamSmoothness * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, animalCamSmoothness * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, animalCamSmoothness * Time.deltaTime);
        }
    }

    void CameraVolume()
    {
        globalVolume.profile = cameraProfile;

        if (isSheepDebugOn)
        {
            Debug.Log("Camera Volume Event Is Fired");
        }
    }
    void BaseVolume()
    {
        globalVolume.profile = baseProfile;

        if (isSheepDebugOn)
        {
            Debug.Log("Base Volume Event Is Fired");
        }
    }

    void SetAnimalCard(GameObject animal)
    {
        if (animal != null)
        {
            AnimalBlackboard_Base data = animal.GetComponent<AnimalBlackboard_Base>();
            Debug.Log(data.animalName + ", " + data.animalBreed + " is being followed.");
            uiManager.SetAnimalImage(data.animalBreed);
            uiManager.SetAnimalName(data.animalName);
        }
    }
    public string CurrentAnimalType()
    {
        if (followedAnimal.GetComponent<AnimalBlackboard_Base>() != null)
        {
            return followedAnimal.GetComponent<AnimalBlackboard_Base>().animalBreed;
        }
        else
        {
            return "HandCam";
        }
    }

    public void HandleHandHeldCameraZoom()
    {
        if (isCamHandHeld)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            handHeldCamera.fieldOfView -= scroll * zoomSpeed;

            if (handHeldCamera.fieldOfView <= cameraMaxZoom)
            {
                handHeldCamera.fieldOfView = cameraMaxZoom;
            }
            else if (handHeldCamera.fieldOfView >= cameraMinZoom)
            {
                handHeldCamera.fieldOfView = cameraMinZoom;
            }
            else if (Input.GetMouseButtonDown(2))
            {
                handHeldCamera.fieldOfView = cameraBaseZoom;
            }
        }
    }

    // Flashlight
    void HandleFlashlight()
    {
        if (uiManager.isPhoneOut && isCamHandHeld)
        {
            flashlight.enabled = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                flashLightStrengthLevelCounter++;

                if (flashLightStrengthLevelCounter >= flashLightStrengthLevel.Count())
                {
                    flashLightStrengthLevelCounter = 0;
                }

                flashlight.intensity = flashLightStrengthLevel[flashLightStrengthLevelCounter];
                flashlight.range = flashLightRanges[flashLightStrengthLevelCounter];
            }
        }
        else
        {
            flashlight.enabled = false;
            flashLightStrengthLevelCounter = 0;

            flashlight.intensity = flashLightStrengthLevel[flashLightStrengthLevelCounter];
            flashlight.range = flashLightRanges[flashLightStrengthLevelCounter];
        }
    }

    void Crosshair()
    {
        if (uiManager.isPhoneOut)
        {
            crosshair.SetActive(false);
        }
        else
        {
            crosshair.SetActive(true);
        }
    }

    // Handheld Camera Raycasting
    public string CurrentlyViewedAnimalType()
    {
        handheldCameraRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(handheldCameraRay, out hit,20f, handheldRayLayerMask))
        {
            return hit.transform.gameObject.GetComponent<AnimalBlackboard_Base>().animalBreed;
        }
        else
        {
            return null;
        }
    }

    // Debugging
    void DebugReferences()
    {
        if (isSheepDebugOn)
        {
            if (globalVolume == null)
            {
                Debug.Log("Did not find Volume. Searching by name in Hierarchy");
                globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();

                if (globalVolume == null) Debug.LogError("Could not find Global Volume");
            }

            if (baseProfile == null) Debug.Log("Did not find Base Profile.");
            if (cameraProfile == null) Debug.Log("Did not find Camera Profile.");
        }
    }

    // Deprecated
    private void HandleCameraMovement()
    {
        if (!followAnimal)
        {
            float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
            float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow

            // Move in global X and Z directions (not camera-relative)
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            targetPosition += moveDirection * movementSpeed * Time.deltaTime;
        }
    }
    private void HandleCameraZoom()
    {
        if (!followAnimal)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            targetPosition += transform.forward * scroll * zoomSpeed;
        }
    }
    void ClickingOnAnimal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Prey" || hit.transform.tag == "Predator")
                {
                    Debug.Log("Clicked on Animal. Animal ID is: " + hit.transform.gameObject.GetComponent<AnimalBlackboard_Base>().animalId);
                    uiManager.TurnOnAnimalCard();
                    followAnimal = true;
                    followedAnimal = hit.transform.gameObject;
                    SetAnimalCard(followedAnimal);
                    followAnimalMode.Invoke();
                }
            }
        }
    }

    

}