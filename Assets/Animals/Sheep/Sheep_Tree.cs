using Unity.VisualScripting;
using UnityEngine;

public class Sheep_Tree : MonoBehaviour
{
    // ######################## Base Nodes ######################## \\
    Root_Node RootNode = new("Sheep Root");
    Sequence_Node MainSequence = new("Main Sequence",true);

    // ######################## IS SAFE ######################## \\
    Fallback_Node isSafeHead_Fallback = new("Is Safe Head Fallback",true);
    // -- //
    Condition_Node noEnemy_Condition = new("No Enemy Condition",isDebugOn);
    Sequence_Node noEnemy_Sequence = new("No Enemy Sequence",true);
    // -- //
    Fallback_Node staySafe_Fallback = new("Stay Fallback",true);
    Leaf_Node staySafe_Action = new("Stay Action",false,isDebugOn);
    // -- //
    Condition_Node isSafe_Condition = new("Is Safe Condition",isDebugOn);
    Leaf_Node runToSafety_Action = new("Run To Safety Action",false,isDebugOn);

    // ######################## IS DAY ######################## \\
    Fallback_Node isDayHead_Fallback = new("Is Day Head Fallback",true);
    // -- //
    Condition_Node isDay_Condition = new("Is Day Condition",isDebugOn);
    Sequence_Node isDay_Sequence = new("Is Day Sequence",true);
    // -- //
    Fallback_Node staySleep_Fallback = new("Stay Fallback",true);
    Leaf_Node sleepAction = new("Sleep Action",false,isDebugOn);
    // -- //
    Condition_Node isHome_Condition = new("Is Home Condition",isDebugOn);
    Leaf_Node goHome_Action = new("Go Home Action",false,isDebugOn);

    // ######################## IS HUNGRY ######################## \\
    Fallback_Node isHungryHead_Fallback = new("Is Hungry Head Fallback",true);
    // -- //
    Condition_Node notHungry_Condition = new("Not Hungry Condition",isDebugOn);
    Sequence_Node notHungry_Sequence = new("Not Hungry Sequence",true);
    // -- //
    Fallback_Node eat_Fallback = new("Eat Fallback",true);
    Leaf_Node eat_Action = new("Eat Action",false,isDebugOn);
    // -- //
    Condition_Node atFood_Condition = new("At Food Condition",isDebugOn);
    Leaf_Node findFood_Action = new("Find Food Action",false,isDebugOn);

    // ######################## WANDERING ######################## \\
    Fallback_Node isAtPositionHead_Fallback = new("Is At Position Head Fallback",true);
    Sequence_Node sequence01 = new("Sequence 01");
    Sequence_Node sequence02 = new("Sequence 02");
    // -- //
    Condition_Node atPosition_Condition = new("At Position Condition",isDebugOn);
    Leaf_Node stayAtPosition_Action = new("Stay At Position Action",false,isDebugOn);
    // -- //
    Fallback_Node go_Fallback = new("Go Fallback",true);
    Leaf_Node wandering_Action = new("Wandering Action",false,isDebugOn);
    // -- //
    Condition_Node hasDestination_Condition = new("Has Destination Condition",isDebugOn);
    Leaf_Node getDestination_Action = new("Get Destination Action",false,isDebugOn);

    static bool isDebugOn = false;
    // Sheep_Status sheepStatus;
    // Sheep_Blackboard sheepStatus;
    Prey_Blackboard sheepStatus;
    World_Status worldStatus;

    void Start()
    {
        GetReferences();
        ConnectTrees();

        Build_IsSafe();
        Build_IsDay();
        Build_Wandering();
    }
    void Update()
    {
        CheckConditions();
        RootNode.Tick();
        // RootNode.Reset();
    }

    void GetReferences(){
        sheepStatus = gameObject.GetComponent<Prey_Blackboard>();
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
    }
    void ConnectTrees(){
        RootNode.child = MainSequence;

        MainSequence.children.Add(isSafeHead_Fallback);
        MainSequence.children.Add(isDayHead_Fallback);
        MainSequence.children.Add(wandering_Action);
    }
    void Build_IsSafe()
    {
        isSafeHead_Fallback.AddChild(noEnemy_Condition);
        isSafeHead_Fallback.AddChild(noEnemy_Sequence);

        noEnemy_Sequence.AddChild(staySafe_Fallback);
        noEnemy_Sequence.AddChild(staySafe_Action);

        staySafe_Fallback.AddChild(isSafe_Condition);
        staySafe_Fallback.AddChild(runToSafety_Action);

        staySafe_Action.SetAction(sheepStatus.Hiding);
        runToSafety_Action.SetAction(sheepStatus.GetToSafePlace_Action);
        runToSafety_Action.SetIsDone(()=>sheepStatus.isFleeDone); 
    }
    void Build_IsDay(){
        isDayHead_Fallback.AddChild(isDay_Condition);
        isDayHead_Fallback.AddChild(isDay_Sequence);

        isDay_Sequence.AddChild(staySleep_Fallback);
        isDay_Sequence.AddChild(sleepAction);

        staySleep_Fallback.AddChild(isHome_Condition);
        staySleep_Fallback.AddChild(goHome_Action);

        sleepAction.SetAction(sheepStatus.StayHomeAndSleep_Action);
        goHome_Action.SetAction(sheepStatus.GoHome); 
    }

    void Build_Wandering()
    {
        wandering_Action.SetAction(sheepStatus.Wandering); 
        
    }
    void CheckConditions(){
        noEnemy_Condition.condition = !sheepStatus.isEnemyNearby;
        isSafe_Condition.condition = sheepStatus.isSafe;

        isDay_Condition.condition = worldStatus.isDay;
        isHome_Condition.condition = sheepStatus.isHome;

        // notHungry_Condition.condition = !sheepStatus.isHungry;
        // atFood_Condition.condition = sheepStatus.atFood;

        // atPosition_Condition.condition = sheepStatus.isAtWanderingLocation;
        // hasDestination_Condition.condition = sheepStatus.hasWanderingLocation;
    }

    // Unused

    //     void Build_IsHungry(){
    //     isHungryHead_Fallback.AddChild(notHungry_Condition);
    //     isHungryHead_Fallback.AddChild(notHungry_Sequence);

    //     notHungry_Sequence.AddChild(eat_Fallback);
    //     notHungry_Sequence.AddChild(eat_Action);

    //     eat_Fallback.AddChild(atFood_Condition);
    //     eat_Fallback.AddChild(findFood_Action);

    //     findFood_Action.SetAction(sheepStatus.Wander);
    //     eat_Action.SetAction(sheepStatus.Eat);
    // }
}
