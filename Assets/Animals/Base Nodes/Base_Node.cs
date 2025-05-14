using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Node
{
    public string nodeName;
    public List<Base_Node> children = new List<Base_Node>();
    public enum Node_States{
        Success,
        Failure,
        Running
    }

    public abstract Node_States Tick();
    public abstract void Reset();
    public void AddChild(Base_Node child){
        children.Add(child);
    }
}

public class Root_Node : Base_Node{
    public Base_Node child;

    public Root_Node(string nodeName){
        this.nodeName = nodeName;
    }

    public override Node_States Tick()
    {
        var result = child.Tick();
        return result;
    }

    public override void Reset()
    {
        child.Reset();
    }
}

public class Sequence_Node : Base_Node
{
    int currentChildIndex = 0;
    bool resetOnRunning;
    public Sequence_Node(string nodeName,bool resetOnRunning = false){
        this.nodeName = nodeName;
        this.resetOnRunning = resetOnRunning;
    }
    public override void Reset()
    {
        foreach(var child in children){
            child.Reset();
        }

        currentChildIndex = 0;
    }

    public override Node_States Tick()
    {
        while(currentChildIndex < children.Count){
            var result = children[currentChildIndex].Tick();

            if(result == Node_States.Failure){
                Reset();
                return Node_States.Failure;
            }
            if(result == Node_States.Running){
                if(resetOnRunning){
                    Reset();
                }
                return Node_States.Running;
            }

            currentChildIndex++;
        }
        Reset();
        return Node_States.Success;
    }
}

public class Fallback_Node : Base_Node
{
    int currentChildIndex = 0;
    bool resetOnRunning;
    public Fallback_Node(string nodeName,bool resetOnRunning = false){
        this.nodeName = nodeName;
        this.resetOnRunning = resetOnRunning;
    }
    public override void Reset()
    {
        currentChildIndex = 0;
        foreach(var child in children){
            child.Reset();
        }
    }

    public override Node_States Tick()
    {
        while(currentChildIndex < children.Count){
            var result = children[currentChildIndex].Tick();

            if(result == Node_States.Success){
                Reset();
                return Node_States.Success;
            }
            if(result == Node_States.Running){
                if(resetOnRunning){
                    Reset();
                }
                return Node_States.Running;
            }

            currentChildIndex++;
        }
        Reset();
        return Node_States.Failure;
    }
}

public class Leaf_Node : Base_Node
{
    bool isDone = false;
    bool isInterrupted;
    bool resetOnRunning;
    public override void Reset(){}
    Action action;
    bool isDebugOn = false;
    public Leaf_Node(string nodeName,bool resetOnRunning = false,bool isDebugOn = false){
        this.nodeName = nodeName;
        this.resetOnRunning = resetOnRunning;
        this.isDebugOn = isDebugOn;
    }

    public void SetAction(Action action){
        this.action = action;
    }

    public override Node_States Tick()
    {
        if(isDebugOn){
            Debug.Log(nodeName + " is running");
        }

        if(isDone){
            Reset();
            return Node_States.Success;
        }

        if(isInterrupted){
            return Node_States.Failure;
        }
        if(resetOnRunning){
            Reset();
        }

        action.Invoke();
        return Node_States.Running;
    }
}

public class Condition_Node : Base_Node
{
    public bool condition;
    bool resetOnSuccess;
    bool resetOnFailure;
    bool isDebugOn = false;
    public Condition_Node(string nodeName,bool isDebugOn = false,bool resetOnSuccess = false,bool resetOnFailure = false){
        this.nodeName = nodeName;
        this.resetOnFailure = resetOnFailure;
        this.resetOnSuccess = resetOnSuccess;
        this.isDebugOn = isDebugOn;
    }
    public override void Reset(){}

    public override Node_States Tick()
    {
        if(isDebugOn){
            Debug.Log(nodeName + " is being checked and it is: " + condition);
        }

        if(condition){
            if(resetOnSuccess){
                Reset();
            }

            return Node_States.Success;
        }

        else{
            if(resetOnFailure){
                Reset();
            }  

            return Node_States.Failure;
        }
    }
}