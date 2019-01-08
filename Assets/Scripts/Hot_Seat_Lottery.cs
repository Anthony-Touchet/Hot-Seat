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
    public List<string> Participants
    {
        get { return participants; }
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

            if (lottoNum.Contains("B") || lottoNum.Contains("G"))
            {               
                newColor.highlightedColor = new Color(.5f, 0, 0);
                newColor.normalColor = new Color(.5f, 0, 0);
            }
            else if (lottoNum.Contains("I") || lottoNum.Contains("O"))
            {
                newColor.highlightedColor = new Color(0, .5f, 0);
                newColor.normalColor = new Color(0, .5f, 0);
            }
            else if (lottoNum.Contains("N"))
            {
                newColor.highlightedColor = new Color(0,0,.5f);
                newColor.normalColor = new Color(0, 0, .5f);
            }
            toggle.colors = newColor;

            if(participants.Count < 10)
            {
                LottoStartButton.SetActive(false);
            }

            return;
        }

        participants.Add(lottoNum);

        newColor.highlightedColor = new Color(.5f, .5f, 0);
        newColor.normalColor = new Color(.5f, .5f, 0);
        toggle.colors = newColor;

        if (participants.Count >= 10)
        {
            LottoStartButton.SetActive(true);
        }

        return;
    }

    public void QuitProgram()
    {
        Application.Quit();
    }
}

