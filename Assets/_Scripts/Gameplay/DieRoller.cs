using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoller : MonoBehaviour
{
    private static DieRoller _instance;

    public static DieRoller Instance
    {
        get { return _instance; }
    }

    public GameObject DiePrefab;

    private Transform trans;

    private void Awake()
    {
        _instance = this;
    }

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

