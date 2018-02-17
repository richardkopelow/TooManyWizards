using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{
    private Text fightInfo;

    private int PersuasionTokens;
    private int CombatTokens;

    private void Start()
    {
        fightInfo = GetComponent<Transform>().GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void Show(WizardPiece wizard)
    {
        PersuasionTokens = 0;
        CombatTokens = 0;
        fightInfo.text = string.Format("{0}\nFight Strength:\t{1}\nstubbornness:\t{2}", wizard.Name, wizard.FightStrength, wizard.PersuasionStrength);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void EndCombat(bool attack)
    {
        GameManager.Instance.EndCombat(attack, attack ? CombatTokens : PersuasionTokens);
        Hide();
    }
}
