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
    public override void Dying()
    {
        if (!isDying)
        { 

            animalAgent.ResetPath();
            animalAgent.isStopped = true;

            isDying = true;
            worldStatus.RemoveAnimal(gameObject);
            animator.PlayDeath(); 
        }
    }
}
