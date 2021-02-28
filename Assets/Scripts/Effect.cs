using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem ps;
    AudioSource aso;
    // Start is called before the first frame update
    void Start()
    {
        aso = GetComponent<AudioSource>();
        aso.PlayOneShot(aso.clip);
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
