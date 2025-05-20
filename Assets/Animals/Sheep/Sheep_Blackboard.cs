using System.Collections;
using UnityEngine;

public class Sheep_Blackboard : Prey_Blackboard
{
    public override void Start()
    {
        SetAnimalBreed("Sheep");
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Dying()
    {
        animalAgent.ResetPath();
        animalAgent.isStopped = true;
        
        if (!isDying)
        {
            isDying = true;
            worldStatus.RemoveAnimal(gameObject);
            animator.PlayDeath();
        }
    }

    // IEnumerator DeathTimer()
    // {
    //     yield return new WaitForSeconds(2.6f);
    //     Destroy(gameObject);
    // }
}
