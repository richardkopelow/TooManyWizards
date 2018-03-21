using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPiece : MonoBehaviour
{
    private const int MAX_TELEPORTS = 5;
    private const int MAX_WARLOCKS = 10;

    public enum WizardType
    {
        Apprentice=1,
        Warlock=2,
        Teleport=3
    }

    public WizardType Type;
    public string Name;
    public int FightStrength;
    public int PersuasionStrength;
    public int FightTokenReward;
    public int PersuasionTokenReward;
    public BoardTile tile;
    public GameObject Spell;

    private Animator anim;


    public static WizardType RandomWizardType()
    {
        int roll = (int)(Random.value * 6);

        if (roll < 1)
        {
            if (GameManager.Instance.TeleportCount == MAX_TELEPORTS)
            {
                roll += 1;
            }
            else
            {
                GameManager.Instance.TeleportCount++;
                return WizardPiece.WizardType.Teleport;
            }
        }
        if (roll < 3)
        {
            if (GameManager.Instance.WarlockCount ==MAX_WARLOCKS)
            {
                roll += 2;
            }
            else
            {
                GameManager.Instance.WarlockCount++;
                return WizardPiece.WizardType.Warlock;
            }
        }

        return WizardPiece.WizardType.Apprentice;
    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Die()
    {
        Destroy(gameObject, 0.1f);
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
