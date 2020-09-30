using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBall : MonoBehaviour
{
    private Vector3 OriginalPosition;
    private Rigidbody RB;
    // Start is called before the first frame update
    void Start()
    {
        OriginalPosition = transform.position;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -16)
        {
            transform.position = OriginalPosition;
            RB.velocity = Vector3.zero;
        }
    }
}
