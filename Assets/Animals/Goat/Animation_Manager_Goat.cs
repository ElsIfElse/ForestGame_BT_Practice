using UnityEngine;

public class Animation_Manager_Goat : Animal_AnimatorBaseClass
{
    Animator animator;
    [HideInInspector]
    public string walkAnimation = "walk_forward";
    [HideInInspector]
    public string runAnimation = "run_forward";
    [HideInInspector]
    public string eatAnimation = "eating";
    [HideInInspector]
    public string idleAnimation = "idle"; 

    public override void PlayWalk(){
        animator.Play(walkAnimation);
    }
    public override void PlayRun(){
        animator.Play(runAnimation);
    }
    public override void PlayEat(){
        animator.CrossFade(eatAnimation,0.2f);
    }
    public override void PlayIdle(){
        animator.Play(idleAnimation);
    }

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        if(animator == null) Debug.Log("No animator found on  " + gameObject.name);
    }

    public override void PlayAttack()
    {
        Debug.Log("Attack");
    }
}
