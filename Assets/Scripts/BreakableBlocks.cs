using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocks : MonoBehaviour
{
    UIScript ui;
    PlayerHealth playerHealth;
    int[] blockLife = { 1, 5, 3, 6 };
    public int randomBlockLives;
    public Benefit benefit;

    [SerializeField] GameObject coinObj;
    [SerializeField] GameObject lifeObj;

    GameObject chosenOne;

    [SerializeField] AudioSource coinSound;


    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIScript>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        randomBlockLives = blockLife[Random.Range(0, blockLife.Length)];
    }

    public void TakeDamage()
    {
        randomBlockLives--;
        switch (benefit)
        {
            case Benefit.lifeUp:
                chosenOne = lifeObj;
                if (chosenOne == lifeObj && randomBlockLives == 0) 
                {
                    Instantiate(chosenOne, new Vector3(transform.position.x, transform.position.y + 1, 0f), Quaternion.identity);
                }
                break;

            case Benefit.coins:
                chosenOne = coinObj;
                coinSound.Play();
                Instantiate(chosenOne, new Vector3(transform.position.x, transform.position.y + 1, 0f), Quaternion.identity);
                ui.coinScore++;
                break;
        }
        gameObject.SetActive(!(randomBlockLives <= 0));
    }
}

public enum Benefit
{
    lifeUp,
    coins
}
