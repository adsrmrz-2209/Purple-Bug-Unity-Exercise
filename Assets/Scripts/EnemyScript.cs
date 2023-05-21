using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    RaycastHit hit;
    Rigidbody rb;
    public Transform raycastPos;
    //NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
       //agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        transform.Rotate(ObstacleDetected() ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0));
        //if (!ObstacleDetected()) rb.MovePosition(transform.position + transform.forward * 1.7f * Time.deltaTime);
        if (!ObstacleDetected()) transform.Translate(transform.forward * 1.7f * Time.deltaTime, Space.World);
    }

    private bool ObstacleDetected()
    {
        Physics.Raycast(raycastPos.position, transform.forward, out hit, 0.11f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("obstacle")) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastPos.position, transform.forward * 0.11f);
    }

    public void Die()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.MovePosition(Vector3.zero);
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(new Vector3(0f, 3f, -3f), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) other.GetComponent<PlayerHealth>().TakeDamage();
        if(other.CompareTag("killZone")) gameObject.SetActive(false); //better to disable the enemy than to destroy to prevent garbage collection for better performance
        

    }



}
