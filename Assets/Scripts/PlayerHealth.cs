using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    UIScript ui;
    PlayerScript player;
    public int numOflives = 3;
    public Image[] imageLivesArray;
    [SerializeField] Transform knockBackPos;

    [SerializeField] AudioSource healSound;
    [SerializeField] AudioSource damageSound;
    [SerializeField] AudioSource gameOverSound;

    private void Start()
    {
        player = GetComponent<PlayerScript>();
        ui = FindObjectOfType<UIScript>();
    }

    void Update()
    {
        numOflives = Mathf.Clamp(numOflives, 0, 3);

        for (int i = 0; i < imageLivesArray.Length; i++)
        {
            if (i < numOflives)
            {
                imageLivesArray[i].enabled = true;
            }
            else imageLivesArray[i].enabled = false;
        }

        if (numOflives <= 0)
        {
            gameOverSound.Play();
            ui.GameOver();
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        damageSound.Play();
        numOflives--;
        StartCoroutine(Knockback());
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("health"))
    //    {
    //        other.gameObject.SetActive(false);
    //        numOflives++;
    //        Debug.Log("I am hitting");
    //    }
    //}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("health"))
        {
            healSound.Play();
            hit.gameObject.SetActive(false);
            numOflives++;
        }
    }

    IEnumerator Knockback()
    {
        player.enabled = false;
        player.controller.enabled = false;
        transform.position = new Vector3(knockBackPos.position.x, knockBackPos.position.y, 0);
        yield return new WaitForSeconds(0.05f);
        player.enabled = true;
        player.controller.enabled = true;
    }
}
