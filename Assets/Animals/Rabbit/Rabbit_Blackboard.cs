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
