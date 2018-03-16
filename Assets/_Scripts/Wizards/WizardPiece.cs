using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPiece : MonoBehaviour
{
    public enum WizardType
    {
        Apprentice,
        Warlock,
        Teleport
    }
    public string Name;
    public int FightStrength;
    public int PersuasionStrength;
    public int FightTokenReward;
    public int PersuasionTokenReward;
    public BoardTile tile;
    public GameObject Spell;

    private Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Die()
    {
        Destroy(gameObject,0.1f);
    }

    public virtual void Penalty(PlayerPiece player)
    {

    }

    public virtual Coroutine PersuasionReward()
    {
        return StartCoroutine(persuasionReward());
    }

    protected virtual IEnumerator persuasionReward()
    {
        yield return null;
    }

}
