using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScaler : MonoBehaviour
{
    public float value, maxValue;
    public Transform rect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rect.localScale = new Vector3(value /maxValue, 1,1);
    }
}
