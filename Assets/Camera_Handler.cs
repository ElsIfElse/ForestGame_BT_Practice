using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Camera_Handler : MonoBehaviour
{
    [Header("Camera Follow")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 10, -10);
    [SerializeField] private Vector3 originalPosition = new Vector3(0, 26, -120);
    [SerializeField] private float followSmoothness = 5f;

    [Header("Free Movement")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float smoothness = 5f;

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

    void Start()
    {
        targetPosition = originalPosition;
        targetRotation = transform.rotation;
        CollectAnimals();

        followAnimalMode.AddListener(CameraVolume);
        baseMode.AddListener(BaseVolume);

        DebugReferences();

    }

    void Update()
    {
        GODMODE();
        HandleFollowAnimal();
        HandleCameraMovement();
        HandleCameraZoom();
        SmoothCameraMovement();
        ClickingOnAnimal();

        
    }

    public void CollectAnimals()
    {
        animals.Clear();
        preys = GameObject.FindGameObjectsWithTag("Prey").ToList();
        wolfs = GameObject.FindGameObjectsWithTag("Predator").ToList();
        animals.AddRange(wolfs);
        animals.AddRange(preys);
    }

    private void HandleFollowAnimal()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            followAnimal = true;
            animalIndex = (animalIndex + 1) % animals.Count;
            followedAnimal = animals[animalIndex];
            
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            followAnimal = true;
            animalIndex = (animalIndex - 1 + animals.Count) % animals.Count;
            followedAnimal = animals[animalIndex];
            
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            followAnimal = false;
            targetPosition = originalPosition;
            targetRotation = Quaternion.Euler(25f, 0, 0); // Reset to default angle
            
        }

        if (followAnimal && followedAnimal != null)
        {
            Transform cameraHolderTransform = followedAnimal.transform.FindDeepChild("CameraHolder");
            Vector3 cameraHolderLocation = cameraHolderTransform.position;
            targetPosition = cameraHolderLocation;
            targetRotation = Quaternion.LookRotation(cameraHolderTransform.forward);
        }
    }

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

    private void SmoothCameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
    }

    void GODMODE(){
        if(Input.GetKeyDown(KeyCode.V)){
            baseMode.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.C) || Input.GetKey(KeyCode.X)){

            followAnimalMode.Invoke();
        }
    }

    void CameraVolume(){
        globalVolume.profile = cameraProfile;

        if(isSheepDebugOn){
            Debug.Log("Camera Volume Event Is Fired");
        }
    }
    void BaseVolume(){
        globalVolume.profile = baseProfile;

        if(isSheepDebugOn){
            Debug.Log("Base Volume Event Is Fired");
        }
    }

    void DebugReferences(){
        if(isSheepDebugOn){
            if(globalVolume == null){
                Debug.Log("Did not find Volume. Searching by name in Hierarchy");
                globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();

                if(globalVolume == null) Debug.LogError("Could not find Global Volume");
            }

            if(baseProfile == null) Debug.Log("Did not find Base Profile.");
            if(cameraProfile == null) Debug.Log("Did not find Camera Profile.");
        }
    }

    void ClickingOnAnimal(){
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)){
                if(hit.transform.tag == "Prey" || hit.transform.tag == "Predator"){
                    Debug.Log("Clicked on Animal. Animal ID is: " + hit.transform.gameObject.GetComponent<Animal_BaseClass>().animalId);
                    followAnimal = true;
                    followedAnimal = hit.transform.gameObject;
                    followAnimalMode.Invoke();
                }
            }
        }
    }


    
}