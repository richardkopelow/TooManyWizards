using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMatchWidth : MonoBehaviour
{
    void Start()
    {
        RectTransform trans = GetComponent<RectTransform>();
        trans.sizeDelta = new Vector2(0, trans.rect.width);
    }
}
