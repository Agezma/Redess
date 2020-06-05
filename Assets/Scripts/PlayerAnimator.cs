using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    Animator anim;

    public PlayerAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void SetHorizontal(float horSpeed)
    { 
        anim.SetFloat("HorizontalSpeed", horSpeed);
    }
    public void SetVertical(float verSpeed)
    {
        anim.SetFloat("VerticalSpeed", verSpeed);
    }
    public void SetTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
    public void Shoot()
    {
    }
    public void Respawn()
    {
        anim.Play("Move");
    }
}
