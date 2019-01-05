using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hot_Seat_Lottery : MonoBehaviour
{
    public List<GameObject> buttons;
    public Winner_Selection_Animation wsa;
    public GameObject LottoStartButton;

    public string Winner
    {
        get { return winner; }
    }
        
    private string winner;
    private List<string> participants = new List<string>();

    public void StartLottery()
    {
        System.Random r = new System.Random();
        List<string> finalists = new List<string>();
        int finalistCount = participants.Count / 4;
        for(int i =0; i < finalistCount; i++)
        {
            finalists.Add(participants[r.Next(0, participants.Count)]);
        }

        winner = finalists[r.Next(0, finalists.Count)];

        wsa.enabled = true;
    }

    public void TogglePress(GameObject go)
    {
        ColorBlock newColor = ColorBlock.defaultColorBlock;
        var text = go.GetComponentInChildren<Text>();
        Toggle toggle = go.GetComponent<Toggle>();
        string lottoNum = text.text;

        if (participants.Contains(lottoNum))
        {
            participants.Remove(lottoNum);

            if (lottoNum.Contains("r"))
            {               
                newColor.highlightedColor = new Color(1, .2f, .2f);
                newColor.normalColor = new Color(1, .2f, .2f);
            }
            else if (lottoNum.Contains("g"))
            {
                newColor.highlightedColor = new Color(0, 1, 0);
                newColor.normalColor = new Color(0, 1, 0);
            }
            else if (lottoNum.Contains("b"))
            {
                newColor.highlightedColor = new Color(0,1,1);
                newColor.normalColor = new Color(0, 1, 1);
            }
            toggle.colors = newColor;

            if(participants.Count < 10)
            {
                LottoStartButton.SetActive(false);
            }

            return;
        }

        participants.Add(lottoNum);

        newColor.highlightedColor = Color.yellow;
        newColor.normalColor = Color.yellow;
        toggle.colors = newColor;

        if (participants.Count >= 10)
        {
            LottoStartButton.SetActive(true);
        }

        return;
    }
}

