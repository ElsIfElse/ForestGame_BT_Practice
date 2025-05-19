using System.Collections;
using UnityEngine;

public class Animation_Manager_Wolf : Animal_AnimatorBaseClass
{
    Animator animator;
    string idleAnimation = "idle";
    // string digAnimation = "dig";
    string sitAnimation = "sit";
    string[] idleAnimations = {"idle", "dig", "sit"};

    bool isHelperRunning = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public override void  PlayIdle(){

        int index = Random.Range(0, idleAnimations.Length);

        if(idleAnimations[index] == sitAnimation && !isHelperRunning){
            StartCoroutine(IdleHelper(idleAnimations[index], idleAnimation));
        }
        if(idleAnimations[index] != sitAnimation){
            animator.Play(idleAnimations[index]);
        }
    }

    IEnumerator IdleHelper(string anim_01, string anim_02){
        animator.Play(anim_01);
        yield return new WaitForSeconds(3.3f);
        animator.Play(anim_02);
        isHelperRunning = false;
    }
    public override void PlayWalk(){
        animator.Play("walk");
    }

    public override void PlayRun(){
        animator.Play("run");
    }
    public override void PlayEat(){
        animator.Play("eat");
    }
    public override void PlayAttack(){
        animator.Play("attack");
    }


}
