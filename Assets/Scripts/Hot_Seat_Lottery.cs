using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Hot_Seat_Lottery : MonoBehaviour
{
    public List<GameObject> buttons;
    public Winner_Selection_Animation wsa;
    public GameObject LottoStartButton;
    public int selectionWait;

    public string Winner
    {
        get { return winner; }
    }
    public List<string> Participants
    {
        get { return participants; }
    }
        
    private string winner;
    private string winnerPayout = "";
    private List<string> participants = new List<string>();
    private string lottoHistoryFileName = "/Winner Log.txt";
    private string winningsSelectionFileName;

    public void StartLottery()
    {
        // See if file Exists
        FileStream saveStream;

        saveStream = (File.Exists(Application.persistentDataPath + lottoHistoryFileName)) ?
            File.Open(Application.persistentDataPath + lottoHistoryFileName, FileMode.Open) :
            File.Create(Application.persistentDataPath + lottoHistoryFileName);
        
        StreamReader streamReader = new StreamReader(saveStream);
        var oldData = streamReader.ReadToEnd();
        saveStream.Position = 0;
        streamReader.DiscardBufferedData();

        System.Random r = new System.Random();
        List<string> finalists = new List<string>();
        int finalistCount = participants.Count / 4;
        bool lookinfForWinner = true;

        bool canChangePool = false;

        //Look for winner
        while (lookinfForWinner)
        {
            // Get a few finalists
            for (int i = 0; i < finalistCount; i++)
            {
                finalists.Add(participants[r.Next(0, participants.Count)]);
            }

            // Select a winning canidate
            var tempWinner = finalists[(int)((Time.time * 10000) - (Time.time * 10000 - r.Next(0, finalists.Count)))];
            //Searching to see if they were selected recently
            string tempString = "";
            for(int i = 0; i < selectionWait; i++) 
            {
                tempString += streamReader.ReadLine();
            }
            var parsedString = tempString.Split(' ');
            List<string> tempList = new List<string>();

            foreach (string s in parsedString)
            {
                if (s.Contains("B-") || s.Contains("I-") || s.Contains("N-") || s.Contains("G-") ||
                    s.Contains("O-"))
                    tempList.Add(s);
            }
                
            if (!tempList.Contains(tempWinner))
            {
                lookinfForWinner = false;
                winner = tempWinner;
            }
                
        }

        // Get a winnerPayout
        int ticket = r.Next(0, 100);
        if(ticket <= 21)
        {
            winnerPayout = "$20";
        }
        else if (ticket >= 22 && ticket <= 41)
        {
            winnerPayout = "$30";
        }

        else if (ticket >= 42 && ticket <= 56)
        {
            winnerPayout = "$35";
        }

        else if (ticket >= 57 && ticket <= 70)
        {
            winnerPayout = "$40";
        }

        else if (ticket >= 71 && ticket <= 77)
        {
            winnerPayout = "$50";
        }

        else if (ticket >= 78 && ticket <= 84)
        {
            winnerPayout = "$60";
        }

        else if (ticket >= 85 && ticket <= 90)
        {
            winnerPayout = "$80";
        }

        else if (ticket >= 91 && ticket <= 94)
        {
            winnerPayout = "$100";
        }

        else if (ticket >= 95 && ticket <= 98)
        {
            winnerPayout = "$30";
        }

        else if (ticket > 98 )
        {
            winnerPayout = "$200";
        }

        saveStream.Position = 0;
        StreamWriter streamWriter = new StreamWriter(saveStream);
        string newData = winner + " " + winnerPayout + " " + DateTime.Now.ToString("dddd MMMM dd, yyyy @ HH:mm") + " ";
        streamWriter.WriteLine(newData);
        streamWriter.Write(oldData);

        streamWriter.Close();
        saveStream.Close();

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

