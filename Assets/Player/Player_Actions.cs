using UnityEngine;

public class Player_Actions : MonoBehaviour
{
    [Header("Raycast Settings")]
    Ray interactionRay;
    [SerializeField] GameObject playerCamera;
    [SerializeField] float interactionRayLength = 10f;
    [SerializeField] LayerMask interactionlayerMask;


    [SerializeField] Vector3 raycastOffset;

    RaycastHit interactionHitInfo;
    Manager_Collector managerCollector;
    UI_Manager uiManager;

    void Start()
    {
        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        uiManager = managerCollector.uiManager;
    }
    void Update()
    {
        InteractionRaycasting();
    }

    // INITIALIZATION
    void SetRay()
    {
        interactionRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    }

    // RAYCASTING
    void InteractionRaycasting()
    {
        SetRay();

        Debug.DrawRay(interactionRay.origin, interactionRay.direction * interactionRayLength, Color.red);


        if (Physics.Raycast(interactionRay.origin, interactionRay.direction, out interactionHitInfo, interactionRayLength,interactionlayerMask))
        {
            Debug.Log("Hit Object's Layer Is: "+interactionHitInfo.transform.gameObject.layer);
            if (interactionHitInfo.transform.gameObject.layer == 7)
            {
                uiManager.TurnOnIndicator_PickFood();
            }
            else if (interactionHitInfo.transform.gameObject.layer == 8)
            {
                uiManager.TurnOnIndicator_FeedAnimal();
            }
        }
        else
        {
            uiManager.TurnOffIndicator();
        }
        
    }

    // INTERACTIONS
    void PickFoodAction()
    {
        
    }
    void SetCameraOnAnimalAction()
    {
        
    }

    // UTILITIES


    // REFERENCES

    // DEBUGGING

}
