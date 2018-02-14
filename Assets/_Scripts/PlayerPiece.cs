using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPiece : MonoBehaviour
{
    public GameObject VCam;
    public int Movement = 0;
    public int PersuasionToken = 0;
    public int CombatTokens = 0;

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
        if (currentTile.Wizard!=null)
        {
            EnterCombat();
        }
        else
        {
            EndTurn();
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

    public void EnterCombat()
    {

    }

    public void StartTurn()
    {
        VCam.SetActive(true);
    }

    public void EndTurn()
    {
        VCam.SetActive(false);
        GameManager.Instance.EndTurn();
    }
}
