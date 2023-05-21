using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    readonly float[] randomX = { -2f, 2 };
    float xVal;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        xVal = randomX[Random.Range(0, randomX.Length)];
        rb.AddForce(new Vector3(xVal, 2f, 0.0f), ForceMode.Impulse);

        //if(time > 0f) 
        //{
        //    time -= Time.deltaTime;
        //}
        //else gameObject.SetActive(false);
        //StartCoroutine(DestroyMe());
    }

    //IEnumerator DestroyMe()
    //{
    //    yield return new WaitForSeconds(1f);

    //}

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject); //creates garbage
        gameObject.SetActive(false);
    }
}
