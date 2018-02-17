using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warlock : WizardPiece
{
    public override void Penalty(PlayerPiece player)
    {
        base.Penalty(player);
        player.Teleport(player.LastCheckpoint.GetComponent<Transform>().position);
    }

    public override void PersuasionReward()
    {
        base.PersuasionReward();
    }
}
