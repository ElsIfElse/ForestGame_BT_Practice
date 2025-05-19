using System.Collections;
using UnityEngine;

public class Bear_BlackBoard : Predator_Blackboard
{
    public override void Start()
    {
        SetAnimalBreed("Bear");
        base.Start();
    }
}
