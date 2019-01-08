using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winner_Selection_Animation : MonoBehaviour
{
    public Hot_Seat_Lottery hot_Seat_Lottery;
    public float animationOffset;
    public float verticalForce;
    public float spinForce;
    public float startDelay;
    public float endDelay;
    public GameObject selectionScreen;
    public GameObject winnerScreen;
    public Text winnerDisplayText;

    private string winner;
    private GameObject winnerButton;
    private List<GameObject> buttons = new List<GameObject>();
    private float nextAnimationTime = 0;
    private System.Random random = new System.Random();

    void Start()
    {
        winner = hot_Seat_Lottery.Winner;
        buttons = hot_Seat_Lottery.buttons;
        List<GameObject> tempList = new List<GameObject>();

        foreach(GameObject go in hot_Seat_Lottery.buttons)
        {  
            //Find winner
            if (go.GetComponentInChildren<Text>().text == winner)
            {
                tempList.Add(go);
                continue;
            }

            // Get Non participants
            if (!hot_Seat_Lottery.Participants.Contains(go.GetComponentInChildren<Text>().text))
            {
                var gray = ColorBlock.defaultColorBlock;
                gray.highlightedColor = Color.gray;
                gray.normalColor = Color.gray;

                go.GetComponent<Toggle>().colors = gray;

                tempList.Add(go);

                Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
                rb.mass = 10000f;

                rb.velocity = (transform.up * verticalForce);

                var rotate = go.AddComponent<Rotate>();
                rotate.rotationSpeed = spinForce;
                var dd = go.AddComponent<Destroy_Delay>();
                dd.timeAlive = 10f;
            }
        }
        
        foreach (GameObject go in tempList)
        {
            if (buttons.Contains(go))
                buttons.Remove(go);
        }

        nextAnimationTime = Time.time + startDelay;
    }

    // Update is called once per frame
    void Update()
    {

        if (buttons.Count == 0 && Time.time >= (nextAnimationTime + endDelay))
        {
            selectionScreen.SetActive(false);
            winnerScreen.SetActive(true);
            winnerDisplayText.text = winner;
            this.enabled = false;
        }
            

        if(Time.time >= nextAnimationTime && buttons.Count > 0)
        {
            nextAnimationTime = Time.time + animationOffset;
            var go = buttons[random.Next(0, buttons.Count)];
            buttons.Remove(go);

            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.mass = 10000f;

            rb.velocity = (transform.up * verticalForce);

            var rotate = go.AddComponent<Rotate>();
            rotate.rotationSpeed = spinForce;
            var dd = go.AddComponent<Destroy_Delay>();
            dd.timeAlive = 10f;
        }
    }
}
