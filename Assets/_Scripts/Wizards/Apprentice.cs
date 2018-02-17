using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apprentice : WizardPiece
{
    public override void Penalty(PlayerPiece player)
    {
        base.Penalty(player);
        BoardTile targetTile = tile;
        for (int i = 0; i < 3; i++)
        {
            targetTile = targetTile.BackLink;
        }
        player.Teleport(targetTile.GetComponent<Transform>().position);
    }

    public override void PersuasionReward()
    {
        base.PersuasionReward();
    }
}
