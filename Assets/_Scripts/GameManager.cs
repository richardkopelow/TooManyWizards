﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public Transform Constant;
    public GameObject Roller;
    public GameObject DirectionPicker;
    public CombatHUD Combat;
    public Notifications NotificationView;
    public PlayerPiece[] PlayerPieces;
    public PlayerPiece ActivePlayer
    {
        get
        {
            return PlayerPieces[activePlayerIndex];
        }
    }

    private int tileMask;
    private Transform trans;
    private int activePlayerIndex = 0;
    private BoardTile activeTile;
    private WizardPiece activeWizard;
    private WizardPiece.WizardType wType;

    #region UnityMagic
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            trans = GetComponent<Transform>();
            tileMask = LayerMask.GetMask("Tile");

            PlayerPieces = new PlayerPiece[4];
            List<PlayerPiece.ClassEnum> classes = GlobalVals.Instance.PlayerClasses;
            PlayerPiece[] playerPieces = FindObjectsOfType<PlayerPiece>();
            for (int i = 0; i < classes.Count; i++)
            {
                for (int j = 0; j < playerPieces.Length; j++)
                {
                    if (playerPieces[j].Class == classes[i])
                    {
                        PlayerPieces[i] = playerPieces[j];
                        playerPieces[i].PlayerIndex = i;
                    }
                }
            }

            StartCoroutine(gameLoop());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private IEnumerator placeWizard()
    {
        int roll = (int)(Random.value * 6);

        if (roll < 1)
        {
            wType = WizardPiece.WizardType.Teleport;
        }
        else
        {
            if (roll < 3)
            {
                wType = WizardPiece.WizardType.Warlock;
            }
            else
            {
                wType = WizardPiece.WizardType.Apprentice;
            }
        }
        NotificationView.DisplayNotification("Place a " + wType.GetName() + " wizard", 6);

        bool done = false;
        while (!done)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 400, tileMask))
                {
                    BoardTile hitTile = hit.transform.GetComponent<BoardTile>();
                    if (hitTile.Wizard == null)
                    {
                        hitTile.SpawnWizard(wType);
                        done = true;
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator gameLoop()
    {
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(playerTurn());
    }

    private IEnumerator playerTurn()
    {
        //Init Player HUD TODO: Refactor this into a HUD screen controller
        Text playerName = Constant.Find("PlayerName").GetComponent<Text>();
        PlayerPiece player = PlayerPieces[activePlayerIndex];
        playerName.text = string.Format("Player {0}\nC Tokens:\t{1}\nP Tokens:\t{2}", activePlayerIndex + 1, player.CombatTokens, player.PersuasionTokens);

        player.StartTurn();
        int dieVal = 0;
        yield return rollDie(out dieVal);
        yield return ActivePlayer.Move(dieVal);
        ActivePlayer.DisableCamera();//TODO: refactor all camera management to a manager singleton

        if (activeTile.Wizard != null)//HACK: I don't like that the game manager is checking if the wizard is null
        {
            yield return activeTile.Combat();
        }

        yield return StartCoroutine(placeWizard());

        GameManager.Instance.EndTurn();
    }

    private Coroutine rollDie(out int dieVal)
    {
        dieVal = (int)(Random.value * 6) + 1;
        return StartCoroutine(rollDie(dieVal));
    }

    private bool rollClicked = false;
    private IEnumerator rollDie(int dieVal)
    {
        Roller.SetActive(true);
        yield return new WaitUntil(() => rollClicked);
        
        yield return StartCoroutine(roll(dieVal));
        Roller.SetActive(false);
        rollClicked = false;
    }

    public void Roll()
    {
        rollClicked = true;
    }

    private IEnumerator roll(int dieVal)//TODO: refactor this into a Roller screen controller
    {
        Text rollText = Roller.transform.Find("RollNumber").GetComponent<Text>();
        for (int i = 0; i < 30 + dieVal; i++)
        {
            rollText.text = (i % 6 + 1).ToString();
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
    }

    public void RegisterTile(BoardTile tile)
    {
        activeTile = tile;
    }

    public void GetDirection()
    {
        DirectionPicker.SetActive(true);
    }

    public void Direction(int direction)
    {
        DirectionPicker.SetActive(false);
        activeTile.DirectionPicked(direction);

    }

    public void EndTurn()
    {
        activePlayerIndex = (activePlayerIndex + 1) % PlayerPieces.Length;
        startWizardPlacement();
    }

    public Coroutine RunCombat(AttackResult res, WizardPiece wizard)
    {
        activeWizard = wizard;

        return StartCoroutine(combat(res, wizard));

    }

    private IEnumerator combat(AttackResult res, WizardPiece wizard)
    {
        CombatScreenResult uiResult = new CombatScreenResult();
        yield return Combat.GetCombatMove(uiResult, wizard);

        int toBeat = (uiResult.Attack ? activeWizard.FightStrength : activeWizard.PersuasionStrength) - uiResult.Bonus;
        int die = (int)(Random.value * 6) + 1;
        res.Attack = uiResult.Attack;
        res.Win = die > toBeat;
    }

    public void EndGame(int playerIndex)
    {
        StartCoroutine(endGame(playerIndex));
    }

    private IEnumerator endGame(int playerIndex)
    {
        yield return NotificationView.DisplayNotification(string.Format("Player {0} is the winner!", playerIndex + 1), 5);
        SceneManager.LoadScene("MainMenu");
    }
}
