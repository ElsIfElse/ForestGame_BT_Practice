using UnityEngine;

public abstract class Animal_AnimatorBaseClass : MonoBehaviour
{
    public abstract void PlayWalk();
    public abstract void PlayRun();
    public abstract void PlayIdle();
    public abstract void PlayEat();
    public abstract void PlayAttack();
    public abstract void PlayDeath();
    
}
