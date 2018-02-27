using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPiece : MonoBehaviour
{
    public GameObject VCam;
    public bool Started = false;
    public bool Forced = false;
    public int Movement = 0;
    public int PersuasionTokens = 0;
    public int CombatTokens = 0;
    public BoardTile LastCheckpoint;
    public int DistanceFromEnd
    {
        get
        {
            return currentTile.DistanceFromEnd;
        }
    }

    private Transform trans;
    private NavMeshAgent nav;
    private Animator anim; 
    private BoardTile currentTile;
    private Vector3 lookatTarget;

    void Start()
    {
        trans = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Speed",nav.velocity.magnitude);
        if (nav.remainingDistance<nav.stoppingDistance)
        {
            nav.updateRotation = false;
            Vector3 diff = lookatTarget - trans.position;
            float angle = Vector3.SignedAngle(trans.forward, diff,Vector3.up);
            
            trans.Rotate(0, angle * 0.01f, 0);
        }
        else
        {
            nav.updateRotation = true;
        }
    }

    public void Teleport(Vector3 position)
    {
        Forced = true;
        trans.position = position;
        SetPosition(position);
    }

    public void SetPosition(Vector3 position)
    {
        lookatTarget = position;
        nav.SetDestination(position);
    }

    public void SetLookAt(Vector3 position)
    {
        lookatTarget = position;
    }

    public void RegisterToTile(BoardTile tile)
    {
        currentTile = tile;
        if (!Started)
        {
            Started = true;
        }
    }

    public void DeregisterToTile()
    {
        currentTile = null;
    }

    public void Rolled(int movement)
    {
        Movement = movement;
        currentTile.MovePiece(this);
    }

    public void StartCombat()
    {

    }

    public void StartTurn()
    {
        VCam.SetActive(true);
        currentTile.StartTurn();
    }

    public void EndTurn()
    {
        VCam.SetActive(false);
    }
}
