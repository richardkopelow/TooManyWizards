using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWizard : WizardPiece
{
    public override void Penalty(PlayerPiece player)
    {
        base.Penalty(player);
        PlayerPiece minPlayer = GameManager.Instance.ActivePlayer;
        int minDistance = int.MaxValue;
        foreach (PlayerPiece piece in GameManager.Instance.PlayerPieces)
        {
            int d = piece.DistanceFromEnd;
            if (d<minDistance)
            {
                minDistance = d;
                minPlayer = piece;
            }
        }
        Vector3 minPosition = minPlayer.GetComponent<Transform>().position;
        minPlayer.Teleport(player.GetComponent<Transform>().position);
        player.Teleport(minPosition);
    }

    public override void PersuasionReward()
    {
        base.PersuasionReward();
        
    }
}
