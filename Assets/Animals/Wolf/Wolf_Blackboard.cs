using System.Collections;
using UnityEngine;

public class Wolf_Blackboard : Predator_Blackboard
{
    public override void Start()
    {
        SetAnimalBreed("Wolf");
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
} 
