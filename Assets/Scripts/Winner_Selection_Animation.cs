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

    private string winner;
    private GameObject winnerButton;
    private List<GameObject> buttons = new List<GameObject>();
    private float nextAnimationTime = 0;
    private System.Random random = new System.Random();

    void Start()
    {
        winner = hot_Seat_Lottery.Winner;
        buttons = hot_Seat_Lottery.buttons;

        foreach(GameObject go in hot_Seat_Lottery.buttons)
        {
            if(go.GetComponentInChildren<Text>().text == winner)
            {
                winnerButton = go;
            }
        }

        buttons.Remove(winnerButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttons.Count == 0)
            this.enabled = false;

        if(Time.time >= nextAnimationTime)
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
