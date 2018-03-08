using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{
    public Text CombatTokenDisplay;
    public Text PersuasionTokenDisplay;

    private Text fightInfo;
    private int _persuasionTokens;
    private int persuasionTokens
    {
        get { return _persuasionTokens; }
        set
        {
            _persuasionTokens = value;
            PersuasionTokenDisplay.text = _persuasionTokens.ToString();
        }
    }
    private int _combatTokens;
    private int combatTokens
    {
        get { return _combatTokens; }
        set
        {
            _combatTokens = value;
            CombatTokenDisplay.text = _combatTokens.ToString();
        }
    }

    private void Start()
    {
        fightInfo = GetComponent<Transform>().GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void Show(WizardPiece wizard)
    {
        persuasionTokens = 0;
        combatTokens = 0;
        fightInfo.text = string.Format("{0}\nFight Strength:\t{1}\nstubbornness:\t{2}", wizard.Name, wizard.FightStrength, wizard.PersuasionStrength);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void EndCombat(bool attack)
    {
        if (attack)
        {
            GameManager.Instance.EndCombat(attack, combatTokens);
            GameManager.Instance.ActivePlayer.CombatTokens -= combatTokens;
        }
        else
        {
            GameManager.Instance.EndCombat(attack, persuasionTokens);
            GameManager.Instance.ActivePlayer.PersuasionTokens -= persuasionTokens;
        }
        Hide();
    }

    public void RemoveCombatToken()
    {
        if (combatTokens > 0)
        {
            combatTokens--;
        }
    }

    public void RemovePersuasionToken()
    {
        if (persuasionTokens > 0)
        {
            persuasionTokens--;
        }
    }

    public void AddCombatToken()
    {
        if (combatTokens < GameManager.Instance.ActivePlayer.CombatTokens)
        {
            combatTokens++;
        }
    }

    public void AddPersuasionToken()
    {
        if (persuasionTokens < GameManager.Instance.ActivePlayer.PersuasionTokens)
        {
            persuasionTokens++;
        }
    }
}
