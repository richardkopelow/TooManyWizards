using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool _paused;

    public bool Paused
    {
        get { return _paused; }
        set {
            _paused = value;
            if (_paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }


    public Transform Constant;
    public GameObject Roller;
    public GameObject DirectionPicker;
    public CombatHUD Combat;
    public Notifications NotificationView;
    public PauseMenu PauseMenu;
    public PlayerPiece[] PlayerPieces;
    public PlayerPiece ActivePlayer
    {
        get
        {
            return PlayerPieces[activePlayerIndex];
        }
    }
    public int TeleportCount;
    public int WarlockCount;

    private int tileMask;
    private Transform trans;
    private int activePlayerIndex = 0;
    private BoardTile activeTile;
    private WizardPiece activeWizard;
    private WizardPiece.WizardType wType;
    private bool[] uiStates;

    #region UnityMagic
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            trans = GetComponent<Transform>();
            tileMask = LayerMask.GetMask("Tile");
            uiStates = new bool[5];

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

    private void Update()
    {
        if (!Paused)
        {
            if (Input.GetButtonUp("Pause"))
            {
                PauseMenu.Show();
            }
        }
    }
    #endregion

    private IEnumerator placeWizard()
    {
        //Get a random wizard type to place
        wType = WizardPiece.RandomWizardType();

        //Check if there is anywhere to place this type of wizard
        BoardTile[] tiles = FindObjectsOfType<BoardTile>();
        bool openSpace = false;
        foreach (BoardTile tile in tiles)
        {
            if (tile.Wizard==null||tile.Wizard.Type<wType)
            {
                openSpace = true;
            }
        }

        if (openSpace)
        {
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
                        if (wType > hitTile.Wizard.Type)
                        {
                            hitTile.Wizard.Die();
                            hitTile.Wizard = null;
                            hitTile.SpawnWizard(wType);
                            done = true;
                        }
                    }
                }
                yield return null;
            }
        }
        else
        {
            yield return NotificationView.DisplayNotification("Nowhere to place a " + wType.GetName() + " wizard", 3);
        }
    }

    private IEnumerator gameLoop()
    {
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            yield return StartCoroutine(playerTurn());
            activePlayerIndex = (activePlayerIndex + 1) % PlayerPieces.Length;
        }
    }

    private IEnumerator playerTurn()
    {
        //Init Player HUD TODO: Refactor this into a HUD screen controller
        Text playerName = Constant.Find("Background/PlayerName").GetComponent<Text>();
        PlayerPiece player = PlayerPieces[activePlayerIndex];
        playerName.text = string.Format("Player {0}\nC Tokens:\t{1}\nP Tokens:\t{2}", activePlayerIndex + 1, player.CombatTokens, player.PersuasionTokens);

        player.StartTurn();
        CoroutineOut<int> dieRoll = new CoroutineOut<int>();
        yield return rollDie(dieRoll);
        yield return new WaitForSeconds(1.2f);
        yield return ActivePlayer.Move(dieRoll);
        ActivePlayer.DisableCamera();//TODO: refactor all camera management to a manager singleton

        if (activeTile.Wizard != null)//HACK: I don't like that the game manager is checking if the wizard is null
        {
            yield return activeTile.Combat();
        }
        yield return null;//This is to gap the calls so there will be no double dipping on input
        yield return StartCoroutine(placeWizard());
        NotificationView.Hide();
        
    }

    private Coroutine rollDie(CoroutineOut<int> result)
    {
        return StartCoroutine(rollDieCoroutine(result));
    }

    private bool rollClicked = false;
    private IEnumerator rollDieCoroutine(CoroutineOut<int> result)
    {
        Roller.SetActive(true);
        yield return new WaitUntil(() => rollClicked);

        Roller.SetActive(false);
        yield return DieRoller.Instance.RollDie(result);
        rollClicked = false;
    }

    public void Roll()
    {
        rollClicked = true;
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

    public Coroutine RunCombat(AttackResult res, WizardPiece wizard)
    {
        activeWizard = wizard;

        return StartCoroutine(runCombat(res, wizard));

    }

    private IEnumerator runCombat(AttackResult res, WizardPiece wizard)
    {
        CombatScreenResult uiResult = new CombatScreenResult();
        yield return Combat.GetCombatMove(uiResult, wizard);

        int toBeat = (uiResult.Attack ? activeWizard.FightStrength : activeWizard.PersuasionStrength) - uiResult.Bonus;
        CoroutineOut<int> die = new CoroutineOut<int>();
        yield return DieRoller.Instance.RollDie(die);
        yield return new WaitForSeconds(1.2f);
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

    public void HideUI()
    {
        uiStates[0] = Constant.gameObject.activeInHierarchy;
        Constant.gameObject.SetActive(false);
        uiStates[1] = Roller.activeInHierarchy;
        Roller.SetActive(false);
        uiStates[2] = DirectionPicker.activeInHierarchy;
        DirectionPicker.SetActive(false);
        uiStates[3] = Combat.gameObject.activeInHierarchy;
        Combat.gameObject.SetActive(false);
        uiStates[4] = NotificationView.gameObject.activeInHierarchy;
        NotificationView.gameObject.SetActive(false);
    }

    public void RestoreUI()
    {
        Constant.gameObject.SetActive(uiStates[0]);
        Roller.SetActive(uiStates[1]);
        DirectionPicker.SetActive(uiStates[2]);
        Combat.gameObject.SetActive(uiStates[3]);
        NotificationView.gameObject.SetActive(uiStates[4]);
    }
}
