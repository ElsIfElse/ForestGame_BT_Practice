using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Goat_Blackboard : Prey_Blackboard
{
       public override void Start()
    {   
        SetAnimalBreed("Goat");
        SetAnimalType("Prey");
        base.Start();
    }
}
