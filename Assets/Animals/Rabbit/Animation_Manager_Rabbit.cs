using UnityEngine;

public class Animation_Manager_Rabbit : Animal_AnimatorBaseClass
{
     Animator animator;
    [HideInInspector]
    public string walkAnimation = "walk";
    [HideInInspector]
    public string runAnimation = "run";
    [HideInInspector]
    public string eatAnimation = "eating";
    [HideInInspector]
    public string idleAnimation = "idle";

    public override void PlayWalk(){
        animator.Play(runAnimation);
    }

    public override void PlayRun(){
        animator.Play(runAnimation);
    }
    public override void PlayEat(){
        animator.Play(idleAnimation);
    }
    public override void PlayIdle(){
        animator.Play(idleAnimation);
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public override void PlayAttack()
    {
        Debug.Log("Attack");
    }
}
