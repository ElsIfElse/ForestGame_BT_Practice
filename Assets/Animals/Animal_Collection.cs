using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class Animal_Collection : MonoBehaviour
{

    int objectId = 0;

    [HideInInspector]
    public Dictionary<int,GameObject> sheepDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> wolfDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> rabbitDict = new Dictionary<int,GameObject>();
    [HideInInspector]
    public Dictionary<int,GameObject> goatDict = new Dictionary<int,GameObject>();
    public Dictionary<int,GameObject> bearDict = new Dictionary<int,GameObject>();

    //
    [HideInInspector] public UnityEvent wolfAdded;
    [HideInInspector] public UnityEvent sheepAdded;
    [HideInInspector] public UnityEvent rabbitAdded;
    [HideInInspector] public UnityEvent goatAdded;
    [HideInInspector] public UnityEvent bearAdded;
    
    [HideInInspector] public UnityEvent wolfRemoved;
    [HideInInspector] public UnityEvent sheepRemoved;
    [HideInInspector] public UnityEvent rabbitRemoved;
    [HideInInspector] public UnityEvent goatRemoved;
    [HideInInspector] public UnityEvent bearRemoved;

    private void Start()
    {
        Addressables.InitializeAsync();
    }
    //
    List<GameObject> allAnimals = new List<GameObject>();
    public List<GameObject> GetAllAnimals()
    {
        return allAnimals;
    }
    public void AddAnimalTo_AllAnimals(GameObject animal)
    {
        allAnimals.Add(animal);
        Debug.Log("Animal " + animal.name + " is added to list.");
    }
    public void RemoveAnimalFrom_AllAnimals(GameObject animal)
    {
        allAnimals.Remove(animal);
    }
    //
    List<GameObject> animalsWithCamera = new List<GameObject>();
    public List<GameObject> GetAnimalsWithCamera()
    {
        return animalsWithCamera;
    }
    public void AddAnimalTo_AnimalsWithCamera(GameObject animal)
    {
        animalsWithCamera.Add(animal);
    }
    public void RemoveAnimalFrom_AnimalsWithCamera(GameObject animal)
    {
        for (int i = 0; i < animalsWithCamera.Count; i++)
        {
            if (animalsWithCamera[i] = animal)
            {
                animalsWithCamera.Remove(animal);
                return;
            }
        }
    }
    //
    List <GameObject> friendlyAnimals = new List<GameObject>();
    public List<GameObject> GetFriendlyAnimals()
    {
        return friendlyAnimals;
    }
    public void AddAnimalTo_FriendlyAnimals(GameObject animal)
    {
        friendlyAnimals.Add(animal);
    }
    public void RemoveAnimalFrom_FriendlyAnimals(GameObject animal)
    {
        for (int i = 0; i < friendlyAnimals.Count; i++)
        {
            if (friendlyAnimals[i] = animal)
            {
                friendlyAnimals.Remove(animal);
                return;
            }
        }
    }
    public bool SearchForAnimal(GameObject animal)
    {
        return allAnimals.Contains(animal);
    }
    public void RemoveAnimalFromGame(GameObject animal)
    {
        RemoveAnimalFrom_AllAnimals(animal);
        RemoveAnimalFrom_AnimalsWithCamera(animal);
        RemoveAnimalFrom_FriendlyAnimals(animal);
        RemoveAnimal(animal);
    }
    //

    public void AddWolf(GameObject wolf){
        wolfDict.Add(objectId,wolf);
        allAnimals.Add(wolf);
        objectId++;

        wolfAdded.Invoke();
    }
    public void AddSheep(GameObject sheep){
        sheepDict.Add(objectId,sheep);
        allAnimals.Add(sheep);
        objectId++;

        sheepAdded.Invoke();
    }
    public void AddRabbit(GameObject rabbit){
        rabbitDict.Add(objectId,rabbit);
        allAnimals.Add(rabbit);
        objectId++;

        rabbitAdded.Invoke();
    }
    public void AddGoat(GameObject goat){
        goatDict.Add(objectId,goat);
        allAnimals.Add(goat);
        objectId++;

        goatAdded.Invoke();
    }
    public void AddBear(GameObject bear)
    {
        bearDict.Add(objectId,bear);
        allAnimals.Add(bear);
        objectId++;

        bearAdded.Invoke();   
    }
    public void RemoveWolf(int id){
        Addressables.ReleaseInstance(wolfDict[id]);
        wolfDict.Remove(id);
        wolfRemoved.Invoke();
    }
    public void RemoveSheep(int id){
        Addressables.ReleaseInstance(sheepDict[id]);
        sheepDict.Remove(id);
        sheepRemoved.Invoke();
    }
    public void RemoveRabbit(int id){
        Addressables.ReleaseInstance(rabbitDict[id]);
        rabbitDict.Remove(id);
        rabbitRemoved.Invoke();
    }
    public void RemoveGoat(int id){
        Addressables.ReleaseInstance(goatDict[id]);
        goatDict.Remove(id);
        goatRemoved.Invoke();
    }
    public void RemoveBear(int id){
        Addressables.ReleaseInstance(bearDict[id]);
        bearDict.Remove(id);
        bearRemoved.Invoke();
    }
    public void RemoveAnimal(GameObject targetAnimal){
        string animalBreed = targetAnimal.GetComponent<AnimalBlackboard_Base>().animalBreed;
        switch (animalBreed)
        {
            case "Wolf":
                    StartCoroutine(Remover_Coroutine(()=>RemoveWolf(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId)));
                    RemoveAnimalFrom_AllAnimals(targetAnimal);
                    RemoveAnimalFrom_AnimalsWithCamera(targetAnimal);
                // RemoveWolf(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Sheep":
                    StartCoroutine(Remover_Coroutine(()=>RemoveSheep(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),2.7f));
                    RemoveAnimalFrom_AllAnimals(targetAnimal);
                    RemoveAnimalFrom_AnimalsWithCamera(targetAnimal);

                // RemoveSheep(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Rabbit":
                    StartCoroutine(Remover_Coroutine(()=>RemoveRabbit(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),1.1f));
                    RemoveAnimalFrom_AllAnimals(targetAnimal);
                    RemoveAnimalFrom_AnimalsWithCamera(targetAnimal);

                // RemoveRabbit(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;

            case "Goat":
                    StartCoroutine(Remover_Coroutine(()=>RemoveGoat(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId),2.7f));
                    RemoveAnimalFrom_AllAnimals(targetAnimal);
                    RemoveAnimalFrom_AnimalsWithCamera(targetAnimal);

                // RemoveGoat(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;
                
            case "Bear":
                    StartCoroutine(Remover_Coroutine(()=>RemoveBear(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId)));
                    RemoveAnimalFrom_AllAnimals(targetAnimal);
                    RemoveAnimalFrom_AnimalsWithCamera(targetAnimal);

                // RemoveBear(targetAnimal.GetComponent<AnimalBlackboard_Base>().animalId);
                break;
        }
    }

    IEnumerator Remover_Coroutine(Action action,float timeToWait = 0f){
        yield return new WaitForSeconds(timeToWait);
        action();
    }

}   
