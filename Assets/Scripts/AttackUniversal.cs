using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUniversal : MonoBehaviour
{
    private SphereCollider SC;
    public float Delay = 0.25f;
    public AudioClip SwishSound;
    private AudioSource AS;
    public KeyCode AttackButton;
    private bool CanPlayMoreSwooshSounds = true;
    private float DelayCur;
    private void Start()
    {
        SC = GetComponent<SphereCollider>();
        DelayCur = Delay;
        AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(AttackButton))
        {
            SC.enabled = true;
            if(CanPlayMoreSwooshSounds == true)
            {
                AS.PlayOneShot(SwishSound);
                CanPlayMoreSwooshSounds = false;
            }
        }

        if(SC.enabled == true)
        {
            DelayCur -= Time.deltaTime;

            if(DelayCur <= 0)
            {
                SC.enabled = false;
                CanPlayMoreSwooshSounds = true;
                DelayCur = Delay;
            }
        }
    }
}
