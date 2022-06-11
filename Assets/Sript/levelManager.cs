using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public int hitpoint = 3;
    public GameObject player;
    //private int score = 0;
    public Text healthText;

    private void Start()
    {
        healthText.text = "Health: " + hitpoint;
    }
    private void Update()
    {
        if(hitpoint == 0)
        {
            Destroy(player);
            //display game over and click to retry button
        }
        
    }

    public void takeDmg()
    {
        hitpoint --;
        healthText.text = "Health: " + hitpoint;
    }
    public void win()
    {
        //if player.win = true
        //say you win and button to return to the main menu
    }

}
