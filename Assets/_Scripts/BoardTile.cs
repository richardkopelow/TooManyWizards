using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private Transform trans;

    private Transform wizardNode;
    private Transform[] playerNodes;
	private PlayerPiece[] playerPieces;

    void Start()
    {
        trans = GetComponent<Transform>();
        wizardNode = trans.Find("WizardNode");
        playerNodes = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            playerNodes[i] = trans.Find("PlayerNode" + i);
        }
		playerPieces = new PlayerPiece[GlobalVals.Instance.PlayerCount];
    }

	public Transform RegisterPlayerPiece(PlayerPiece piece)
	{
		//TODO: Make this actually keep track of players on the tile and return the proper node
		//It should also issue position refresh requests to the pieces to account for pieces entering
		return playerNodes[0];
	}
}
