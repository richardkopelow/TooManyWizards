using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public float MaxStrength;
    public float MinStrength;
    public Vector3 LaunchDirection;
    public GameObject VCam;

    private Transform trans;
    private Rigidbody rigid;
    private int castMask;
    private Vector3[] directions;
    private bool destroy;

    private void Start()
    {
        trans = GetComponent<Transform>();
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
        VCam.SetActive(true);
        Coroutine rollCo = StartCoroutine(roll(result));
        StartCoroutine(destroyCoroutine());
        return rollCo;
    }

    private IEnumerator roll(CoroutineOut<int> result)
    {
        yield return null;// new WaitForFixedUpdate();
        rigid.rotation = Random.rotationUniform;
        rigid.velocity = (LaunchDirection.normalized + Vector3.right * Random.Range(-0.3f, 0.3f)) * Random.Range(MinStrength, MaxStrength);
        rigid.angularVelocity = Random.onUnitSphere * Random.Range(5, 10);
        yield return null;// new WaitForFixedUpdate();
        yield return null;// new WaitForFixedUpdate();
        while (rigid.velocity.sqrMagnitude > 0.001f)
        {
            yield return null;//new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(3);
        float minDist = float.MaxValue;
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(rigid.position, trans.TransformDirection(directions[i]), out hit, 3, castMask))
            {
                if (hit.distance < minDist)
                {
                    minDist = hit.distance;
                    result.Data = i + 1;
                }
            }
        }
        result.Done = true;
        VCam.SetActive(false);
        destroy = true;
    }

    private IEnumerator destroyCoroutine()
    {
        yield return new WaitUntil(()=>destroy);
        Destroy(gameObject);
    }
}
