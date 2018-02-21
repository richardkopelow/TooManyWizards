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
    public string Name;
    public int FightStrength;
    public int PersuasionStrength;
    public int FightTokenReward;
    public int PersuasionTokenReward;
    public BoardTile tile;

    private Animator anim;

    protected virtual void Start()
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

    public virtual void Penalty(PlayerPiece player)
    {

    }

    public virtual void PersuasionReward()
    {
        tile.CleanupCombat();
    }

}
