using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceScript : MonoBehaviour
{
    float[] randomX = {-2f, 2};
    float xVal;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        xVal = randomX[Random.Range(0, randomX.Length)];
        rb.AddForce(new Vector3(xVal, 2f, 0.0f), ForceMode.Impulse);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    rb.isKinematic = true;
    //    Collider collider = GetComponent<Collider>();
    //    collider.isTrigger = true;
    //}

}
