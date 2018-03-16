using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    Transform trans;

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        trans.Rotate(0, 0, 720 * Time.deltaTime);
    }
}
