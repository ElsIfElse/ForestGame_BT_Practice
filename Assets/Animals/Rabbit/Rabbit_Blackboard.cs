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
            animalCollection.RemoveAnimalFromGame(gameObject);
            animator.PlayDeath(); 
            notifications.CreateMessageObject(Feedback_Notifications.messageTypes.World_Notification, animalName + " died");
        }
    }

}
