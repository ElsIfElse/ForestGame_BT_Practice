using UnityEngine;

public class Wolf_Tree : MonoBehaviour
{
    Root_Node RootNode = new Root_Node("Root Node");
    Sequence_Node MainSequence = new Sequence_Node("Main Sequence");
    // ######################## IS DAY TIME ######################## \\
    Fallback_Node isSleepTime_HeadFallback = new Fallback_Node("Is Day Head Fallback");
    Condition_Node isSleepTime_Condition = new Condition_Node("Is Day Condition",isDebugOn);
    Sequence_Node isSleepTime_Sequence = new Sequence_Node("Is Day Sequence");
    // -- //
    Fallback_Node stayHomeAndSleep_Fallback = new Fallback_Node("Stay Home And Sleep Fallback");
    Leaf_Node stayHomeAndSleep_Action = new Leaf_Node("Stay Home And Sleep Action",false,isDebugOn);
    // -- //
    Condition_Node isHome_Condition = new Condition_Node("Is Home Condition",isDebugOn);
    Leaf_Node goHome_Action = new Leaf_Node("Go Home Action",false,isDebugOn);
    // ######################## NOT HUNGRY ######################## \\
    Fallback_Node notHungry_HeadFallback = new Fallback_Node("Not Hungry Head Fallback");
    Condition_Node notHungry_Condition = new Condition_Node("Not Hungry Condition",isDebugOn);
    Sequence_Node notHungry_Sequence = new Sequence_Node("Not Hungry Sequence");
    // -- //
    Fallback_Node eat_fallback = new Fallback_Node("Eat Fallback");
    Leaf_Node eat_Action = new Leaf_Node("Eat Action",false,isDebugOn);
    // -- //
    Condition_Node hasFood_Condition = new Condition_Node("Has Food Condition",isDebugOn);
    Sequence_Node hasFood_Sequence = new Sequence_Node("Has Food Sequence");
    // -- //
    Fallback_Node attack_Fallback = new Fallback_Node("Attack Fallback");
    Leaf_Node attack_Action = new Leaf_Node("Attack Action",false,isDebugOn);
    // -- //
    Condition_Node canAttack_Condition = new Condition_Node("Can Attack Condition",isDebugOn);
    Sequence_Node canAttack_Sequence = new Sequence_Node("Can Attack Sequence");
    // -- //
    Fallback_Node chase_Fallback = new Fallback_Node("Chase Fallback");
    Leaf_Node chase_Action = new Leaf_Node("Chase Action",false,isDebugOn);
    // -- //
    Condition_Node canSeePrey_Condition = new Condition_Node("Can See Prey Condition",isDebugOn);
    Leaf_Node goToHuntLocation_Action = new Leaf_Node("Go To Hunt Location Action",false,isDebugOn);


    
    // ######################## WANDER/IDLE ######################## \\
    Leaf_Node Wandering_Action = new Leaf_Node("Wandering Node",false,isDebugOn);

    // Wolf_State wolfStatus;
    Wolf_Blackboard wolfStatus;
    static bool isDebugOn = false; 

    void Start()
    {
        GetReferences();
        ConnectTrees();
        
        Build_IsSleepTime();
        Build_NotHungry();
        Wandering_Action.SetAction(wolfStatus.Wandering_Action);

    }
    void Update()
    {
        CheckConditions();
        RootNode.Tick();
        RootNode.Reset();
    }
    void CheckConditions(){
        isSleepTime_Condition.condition = wolfStatus.isDay;
        isHome_Condition.condition = wolfStatus.isHome;
        //
        notHungry_Condition.condition = wolfStatus.notHungry;
        hasFood_Condition.condition = wolfStatus.hasFood;
        canAttack_Condition.condition = wolfStatus.canAttack; 
        canSeePrey_Condition.condition = wolfStatus.canSeePrey;
    }
 
    void ConnectTrees(){
        RootNode.child = MainSequence;

        MainSequence.children.Add(isSleepTime_HeadFallback);
        MainSequence.children.Add(notHungry_HeadFallback);
        MainSequence.children.Add(Wandering_Action);
    }
    void Build_IsSleepTime(){
        isSleepTime_HeadFallback.AddChild(isSleepTime_Condition);
        isSleepTime_HeadFallback.AddChild(isSleepTime_Sequence);

        isSleepTime_Sequence.AddChild(stayHomeAndSleep_Fallback);
        isSleepTime_Sequence.AddChild(stayHomeAndSleep_Action);

        stayHomeAndSleep_Fallback.AddChild(isHome_Condition);
        stayHomeAndSleep_Fallback.AddChild(goHome_Action);

        stayHomeAndSleep_Action.SetAction(wolfStatus.Sleep_Action);
        goHome_Action.SetAction(wolfStatus.GoHome); 
    }
    void Build_NotHungry(){
        notHungry_HeadFallback.AddChild(notHungry_Condition);
        notHungry_HeadFallback.AddChild(notHungry_Sequence);

        notHungry_Sequence.AddChild(eat_fallback);
        notHungry_Sequence.AddChild(eat_Action);

        eat_fallback.AddChild(hasFood_Condition);
        eat_fallback.AddChild(hasFood_Sequence);

        hasFood_Sequence.AddChild(attack_Fallback);
        hasFood_Sequence.AddChild(attack_Action);

        attack_Fallback.AddChild(canAttack_Condition);
        attack_Fallback.AddChild(canAttack_Sequence);

        canAttack_Sequence.AddChild(chase_Fallback);
        canAttack_Sequence.AddChild(chase_Action);

        chase_Fallback.AddChild(canSeePrey_Condition);
        chase_Fallback.AddChild(goToHuntLocation_Action);

        eat_Action.SetAction(wolfStatus.Eat_Action);
        attack_Action.SetAction(wolfStatus.Attack_Action);
        chase_Action.SetAction(wolfStatus.Chase_Action);
        goToHuntLocation_Action.SetAction(wolfStatus.GoToHuntingLocation_Action);
    }
    void GetReferences(){
        // wolfStatus = GetComponent<Wolf_State>();
        wolfStatus = GetComponent<Wolf_Blackboard>();

    }
}
