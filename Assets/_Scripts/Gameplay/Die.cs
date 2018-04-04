using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public float MaxStrength;
    public float MinStrength;
    public Vector3 LaunchDirection;

    Rigidbody rigid;
    private int castMask;
    private Vector3[] directions;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        castMask = LayerMask.GetMask("Ground");
        directions = new Vector3[] {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };
    }

    public Coroutine Roll(CoroutineOut<int> result)
    {
        Coroutine rollCo = StartCoroutine(roll(result));
        //StartCoroutine(destroyAfter(rollCo));
        return rollCo;
    }

    private IEnumerator roll(CoroutineOut<int> result)
    {
        yield return null;// new WaitForFixedUpdate();
        rigid.rotation = Random.rotationUniform;
        rigid.velocity = (LaunchDirection.normalized + Vector3.right * Random.Range(-1, 1)) * Random.Range(MinStrength, MaxStrength);
        rigid.angularVelocity = Random.onUnitSphere * Random.Range(5, 10);
        yield return null;// new WaitForFixedUpdate();
        yield return null;// new WaitForFixedUpdate();
        while (rigid.velocity.sqrMagnitude > 0.001f)
        {
            yield return null;//new WaitForFixedUpdate();
        }
        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(rigid.position, directions[i], castMask))
            {
                result.Data = i;
            }
        }
        result.Done = true;
    }

    private IEnumerator destroyAfter(Coroutine co)
    {
        yield return co;
    }
}
