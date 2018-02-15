using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Notifications NotificationView;
    public PlayerPiece[] Pieces;

    private int tileMask;
    private Transform trans;
    private bool playing;
    private int activePlayerIndex = 0;
    private BoardTile activeTile;
    private bool picking;

    #region UnityMagic
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            trans = GetComponent<Transform>();
            tileMask = LayerMask.GetMask("Tile");
            this.ExecuteDelayed(init, 0.2f);
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
                    hitTile.SpawnWizard(WizardPiece.WizardType.Apprentice);
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
        PlayerPiece player = Pieces[activePlayerIndex];
        playerName.text = string.Format("Player {0}\nC Tokens:\t{1}\nP Tokens:\t{2}", activePlayerIndex + 1, player.CombatTokens, player.PersuasionToken);
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
        Pieces[activePlayerIndex].Rolled(number);
        yield return new WaitForSeconds(0.3f);
        Roller.SetActive(false);
    }

    public void GetDirection(BoardTile tile)
    {
        activeTile = tile;
        DirectionPicker.SetActive(true);
    }

    public void Direction(int direction)
    {
        DirectionPicker.SetActive(false);
        activeTile.DirectionPicked(direction);

    }

    public void EndTurn()
    {
        activePlayerIndex = (activePlayerIndex + 1) % GlobalVals.Instance.PlayerCount;
        //StartTurn();
        startWizardPlacement();
    }

    private void startWizardPlacement()
    {
        picking = true;
        NotificationView.DisplayNotification("Place a wizard", 1);
    }
}
