using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Feedback_Notifications : MonoBehaviour
{
    //
    [Header("Parent Objects")]
    [SerializeField] GameObject interactionFailed_Parent;
    [SerializeField] GameObject worldNotification_Parent;
    [SerializeField] GameObject streamNotification_Parent;

    [SerializeField] GameObject[] worldNotifiacation_Slots = new GameObject[4];
    int maxSlotNumber = 4;

    [Space]
    [Header("Prefabs")]

    //
    [SerializeField] GameObject interactionFailed_Prefab;
    [SerializeField] GameObject worldNotification_Prefab;
    [SerializeField] GameObject streamNotification_Prefab;

    List<GameObject> messages = new();
    public enum messageTypes
    {
        Interaction_Fail,
        World_Notification,
        Stream_Notification
    }

    void Start()
    {

    }

    public void CreateMessageObject(Enum type, string message)
    {
        switch (type)
        {
            case messageTypes.Interaction_Fail:
                Create_InteractionFail(message);
                break;
            case messageTypes.World_Notification:
                Create_WorldNotification(message);
                break;
            case messageTypes.Stream_Notification:
                Create_StreamNotification(message);
                break;
        }
    }

    void Create_InteractionFail(string message)
    {
        GameObject newMessage = Instantiate(interactionFailed_Prefab, interactionFailed_Parent.transform.position, Quaternion.identity);
        CanvasGroup canvasGroup = newMessage.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f);
        newMessage.transform.DOMove(newMessage.transform.position + Vector3.up * 40, 3f);

        newMessage.transform.SetParent(interactionFailed_Parent.transform);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;

        StartCoroutine(FadeOut_Coroutine(newMessage));

    }
    void Create_WorldNotification(string message)
    {
        Debug.Log(messages.Count);

        MoveMessages();
        GameObject newMessage = Instantiate(worldNotification_Prefab, worldNotifiacation_Slots[0].transform.position, Quaternion.identity);
        messages.Insert(0,newMessage);

        CanvasGroup canvasGroup = newMessage.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f);    


        newMessage.transform.SetParent(worldNotifiacation_Slots[0].transform);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;

        StartCoroutine(FadeOut_Coroutine(newMessage));
        
    }
    void Create_StreamNotification(string message)
    {
        MoveMessages();
        GameObject newMessage = Instantiate(streamNotification_Prefab, worldNotifiacation_Slots[0].transform.position, Quaternion.identity);
        messages.Insert(0,newMessage);

        CanvasGroup canvasGroup = newMessage.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f);

        newMessage.transform.SetParent(worldNotifiacation_Slots[0].transform);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;

        StartCoroutine(FadeOut_Coroutine(newMessage));
        
    }

    void MoveMessages()
    {
        if (messages.Count >= maxSlotNumber)
        {
            GameObject oldestMessage = messages[messages.Count-1];
            messages.RemoveAt(messages.Count-1);
            Destroy(oldestMessage);
        }

        if (messages.Count > 0)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (i < worldNotifiacation_Slots.Length)
                {
                    messages[i].transform.SetParent(worldNotifiacation_Slots[i+1].transform);
                    messages[i].transform.position = worldNotifiacation_Slots[i+1].transform.position;
                }
            }
        }
    }
    IEnumerator FadeOut_Coroutine(GameObject notification)
    {
        yield return new WaitForSeconds(2f);
        notification.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);

        if (messages.Contains(notification))
        {
            messages.Remove(notification);
        }

        Destroy(notification);
    }


    
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         CreateMessageObject(messageTypes.World_Notification, "1");
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         CreateMessageObject(messageTypes.World_Notification, "2");
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha4))
    //     {
    //         CreateMessageObject(messageTypes.World_Notification, "3");
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha5))
    //     {
    //         CreateMessageObject(messageTypes.World_Notification, "4");
    //     }
    // }
}   
