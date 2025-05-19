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
}
