using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit_Blackboard : Prey_Blackboard
{
    public override void Start()
    {
        SetAnimalBreed("Rabbit");
        base.Start();
    }

}
