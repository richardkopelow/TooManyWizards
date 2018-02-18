using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public BoardTile[] NextTiles;
    public BoardTile BackLink;
    public GameObject VCam;
    public WizardPiece Wizard;
    public bool Checkpoint;
    
    public int DistanceFromEnd
    {
        get
        {
            if (NextTiles.Length==0)
            {
                return 0;
            }
            int minDistance=int.MaxValue;
            foreach (BoardTile tile in NextTiles)
            {
                if (tile.DistanceFromEnd<minDistance)
                {
                    minDistance = tile.DistanceFromEnd;
                }
            }
            return minDistance+1;
        }
    }

    private Transform trans;

    private Transform wizardNode;
    private Transform[] playerNodes;
    private List<PlayerPiece> playerPieces;

    void Start()
    {
        trans = GetComponent<Transform>();
        wizardNode = trans.Find("WizardNode");
        playerNodes = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            playerNodes[i] = trans.Find("PlayerNode" + i);
        }
        playerPieces = new List<PlayerPiece>(GlobalVals.Instance.PlayerCount);
        foreach (BoardTile tile in NextTiles)
        {
            tile.BackLink = this;
        }
    }

    public void RegisterPlayerPiece(PlayerPiece piece)
    {
        playerPieces.Add(piece);
        piece.RegisterToTile(this);
        coordinatePositioning();
    }

    private void coordinatePositioning()
    {
        int positionOffset = playerPieces.Count % 2 == 0 ? 1 : 0;
        Vector3 lookat = Wizard != null || NextTiles.Length == 0 ? wizardNode.position : NextTiles[0].GetComponent<Transform>().position;
        for (int i = 0; i < playerPieces.Count; i++)
        {
            playerPieces[i].SetPosition(playerNodes[i + positionOffset].position);
            playerPieces[i].SetLookAt(lookat);
        }
    }

    public void MovePiece(PlayerPiece piece)
    {
        if (NextTiles.Length == 1)
        {
            movePiece(piece, 0);
        }
        else
        {
            GameManager.Instance.GetDirection();
        }
    }

    private void movePiece(PlayerPiece piece, int index)
    {
        if (NextTiles.Length > index)
        {
            piece.SetPosition(NextTiles[index].GetComponent<Transform>().position);
            playerPieces.Remove(piece);
            piece.DeregisterToTile();
            piece.Movement--;
        }
    }

    public void DirectionPicked(int direction)
    {
        movePiece(GameManager.Instance.ActivePlayer, direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerPiece")
        {
            GameManager.Instance.RegisterTile(this);
            PlayerPiece piece = other.GetComponent<PlayerPiece>();
            if (Checkpoint)
            {
                piece.LastCheckpoint = this;
            }
            if (piece.Movement > 0)
            {
                if (NextTiles.Length == 0)
                {
                    //End Game
                }
                else
                {
                    MovePiece(piece);
                }
            }
            else
            {
                if (piece.Started)
                {
                    if (Wizard != null)
                    {
                        StartCombat();
                        piece.EndTurn();
                    }
                    else
                    {
                        GameManager.Instance.EndTurn();
                        piece.EndTurn();
                    }
                }
                RegisterPlayerPiece(piece);
            }
        }
    }

    public void SpawnWizard(WizardPiece.WizardType pieceType)
    {
        GameObject wizardPrefab = wizardPrefab = Resources.Load<GameObject>(pieceType.GetName());
        Transform wizardTrans = Instantiate<GameObject>(wizardPrefab).GetComponent<Transform>();
        wizardTrans.parent = trans;
        wizardTrans.localPosition = wizardNode.localPosition;
        wizardTrans.localEulerAngles = new Vector3(-90, 180, 0);
        Wizard = wizardTrans.GetComponent<WizardPiece>();
        Wizard.tile = this;
    }

    public void StartCombat()
    {
        VCam.SetActive(true);
        GameManager.Instance.StartCombat(Wizard);
    }

    public void EndCombat(bool attack, bool win)
    {

        VCam.SetActive(false);
        PlayerPiece player = GameManager.Instance.ActivePlayer;
        if (win)
        {
            if (attack)
            {
                Wizard.Die();
                player.CombatTokens = Wizard.FightTokenReward;
                player.PersuasionTokens = Wizard.PersuasionTokenReward;

                CleanupCombat();
            }
            else
            {
                Wizard.PersuasionReward();
            }
        }
        else
        {
            Wizard.Penalty(player);
            Debug.Log("Lose");

            CleanupCombat();
        }
    }

    public void CleanupCombat()
    {
        GameManager.Instance.EndTurn();
    }
}
