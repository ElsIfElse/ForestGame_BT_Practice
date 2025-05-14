using UnityEngine;

public class Rabbit_Tree : MonoBehaviour
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
    Rabbit_Blackboard rabbitStatus;
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
        RootNode.Reset();
    }

    void GetReferences(){
        rabbitStatus = gameObject.GetComponent<Rabbit_Blackboard>();
        worldStatus = GameObject.FindWithTag("WorldStatus").GetComponent<World_Status>();
    }
    void ConnectTrees(){
        RootNode.child = MainSequence;

        MainSequence.children.Add(isSafeHead_Fallback);
        MainSequence.children.Add(isDayHead_Fallback);
        // MainSequence.children.Add(isHungryHead_Fallback);
        MainSequence.children.Add(wandering_Action);
    }
    void Build_IsSafe(){
        isSafeHead_Fallback.AddChild(noEnemy_Condition);
        isSafeHead_Fallback.AddChild(noEnemy_Sequence);

        noEnemy_Sequence.AddChild(staySafe_Fallback);
        noEnemy_Sequence.AddChild(staySafe_Action);

        staySafe_Fallback.AddChild(isSafe_Condition);
        staySafe_Fallback.AddChild(runToSafety_Action);

        staySafe_Action.SetAction(rabbitStatus.IdleStayStill);
        runToSafety_Action.SetAction(rabbitStatus.GoToClosestSafePlace);
    }
    void Build_IsDay(){
        isDayHead_Fallback.AddChild(isDay_Condition);
        isDayHead_Fallback.AddChild(isDay_Sequence);

        isDay_Sequence.AddChild(staySleep_Fallback);
        isDay_Sequence.AddChild(sleepAction);

        staySleep_Fallback.AddChild(isHome_Condition);
        staySleep_Fallback.AddChild(goHome_Action);

        sleepAction.SetAction(rabbitStatus.IdleStayStill);
        goHome_Action.SetAction(rabbitStatus.GoHome);
    } 

    void Build_Wandering(){
        // isAtPositionHead_Fallback.AddChild(sequence01);
        // isAtPositionHead_Fallback.AddChild(sequence02);

        // sequence01.AddChild(atPosition_Condition);
        // sequence01.AddChild(stayAtPosition_Action);

        // sequence02.AddChild(go_Fallback);
        // sequence01.AddChild(wandering_Action);

        // go_Fallback.AddChild(hasDestination_Condition);
        // go_Fallback.AddChild(getDestination_Action);

        // stayAtPosition_Action.SetAction(sheepStatus.StandStillAndWander); 
        wandering_Action.SetAction(rabbitStatus.Wandering_Action); 
        // getDestination_Action.SetAction(rabbitStatus.GetRandomMeadowLocationForWandering);
    }
    void CheckConditions(){
        noEnemy_Condition.condition = !rabbitStatus.isEnemyNear;
        isSafe_Condition.condition = rabbitStatus.isSafe;

        isDay_Condition.condition = worldStatus.isDay;
        isHome_Condition.condition = rabbitStatus.isHome;

        // notHungry_Condition.condition = !rabbitStatus.isHungry;
        // atFood_Condition.condition = rabbitStatus.atFood;

        atPosition_Condition.condition = rabbitStatus.isAtWanderingLocation;
        // hasDestination_Condition.condition = rabbitStatus.hasWanderingLocation;
    }
}
