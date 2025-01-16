using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimpleCameraMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float Multiplier;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.localPosition += new Vector3(0, Multiplier*0.01f, 0);
        if (Input.GetKey(KeyCode.S))
            transform.localPosition += new Vector3(0, -Multiplier*0.01f, 0);
        if (Input.GetKey(KeyCode.A))
            transform.localPosition += new Vector3(-Multiplier*0.01f, 0, 0);
        if (Input.GetKey(KeyCode.D))
            transform.localPosition += new Vector3(Multiplier*0.01f, 0, 0);
    }
}