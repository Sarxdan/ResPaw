using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{

    public float speed = 5f;
    public float clamppos;

    public Vector3 StartPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, clamppos);
        transform.position = StartPosition + Vector3.left * newPosition;
    }
}
