using System.Collections;
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
    private bool picking;
    private WizardPiece.WizardType wType;

    #region UnityMagic
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            trans = GetComponent<Transform>();
            tileMask = LayerMask.GetMask("Tile");
            this.ExecuteDelayed(init, 0.2f);

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
                    }
                }
            }
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (picking && Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 400, tileMask))
            {
                BoardTile hitTile = hit.transform.GetComponent<BoardTile>();
                if (hitTile.Wizard == null)
                {
                    hitTile.SpawnWizard(wType);
                    picking = false;

                    StartTurn();
                }
            }
        }
    }
    #endregion

    private void init()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        Text playerName = Constant.Find("PlayerName").GetComponent<Text>();
        PlayerPiece player = PlayerPieces[activePlayerIndex];
        playerName.text = string.Format("Player {0}\nC Tokens:\t{1}\nP Tokens:\t{2}", activePlayerIndex + 1, player.CombatTokens, player.PersuasionTokens);
        player.StartTurn();
        Roller.SetActive(true);
    }

    public void Roll()
    {
        StartCoroutine(roll());
    }
    IEnumerator roll()
    {
        int number = (int)(Random.value * 6) + 1;
        Text rollText = Roller.transform.Find("RollNumber").GetComponent<Text>();
        for (int i = 0; i < 30 + number; i++)
        {
            rollText.text = (i % 6 + 1).ToString();
            yield return new WaitForSeconds(0.05f);
        }
        PlayerPieces[activePlayerIndex].Rolled(number);
        yield return new WaitForSeconds(0.3f);
        Roller.SetActive(false);
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

    private void startWizardPlacement()
    {
        picking = true;
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
        NotificationView.DisplayNotification("Place a "+wType.GetName()+" wizard", 6);
    }

    public void StartCombat(WizardPiece wizard)
    {
        activeWizard = wizard;
        Combat.Show(wizard);
    }

    public void EndCombat(bool attack, int bonus)
    {
        int toBeat = (attack ? activeWizard.FightStrength : activeWizard.PersuasionStrength) - bonus;
        int die = (int)(Random.value * 6) + 1;

        activeTile.EndCombat(attack, die > toBeat);
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
