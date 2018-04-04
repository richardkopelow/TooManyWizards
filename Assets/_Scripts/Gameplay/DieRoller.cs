using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoller : MonoBehaviour
{
    public GameObject DiePrefab;

    private Transform trans;

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    public Coroutine RollDie(CoroutineOut<int> result)
    {
        GameObject die = Instantiate(DiePrefab,trans.position,trans.rotation);
        return die.GetComponent<Die>().Roll(result);
    }
}

