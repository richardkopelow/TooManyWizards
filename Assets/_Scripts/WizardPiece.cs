using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPiece : MonoBehaviour
{
    public enum WizardType
    {
        Apprentice,
        Warlock,
        Teleportation
    }

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void Attack()
    {
        anim.SetBool("Attack", true);
    }

    public void AttackFinished()
    {
        anim.SetBool("Attack", false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}
