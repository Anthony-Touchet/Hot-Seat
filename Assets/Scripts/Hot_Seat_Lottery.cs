using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Hot_Seat_Lottery : MonoBehaviour
{
    public string path;

    public List<GameObject> buttons;
    public List<GameObject> rewardToggles;
    public InputField passwordInput;

    public Winner_Selection_Animation wsa;
    public GameObject LottoStartButton;
    public int selectionWait;

    public string Winner
    {
        get { return winner; }
    }

    public string PayOut
    {
        get { return winnerPayout; }
    }

    public List<string> Participants
    {
        get { return participants; }
    }
        
    private string winner;
    private string winnerPayout = "";

    private List<string> participants = new List<string>();
    public List<int> possibleRewards = new List<int>();

    private string lottoHistoryFileName = "/Winner Log.txt";
    private string rewardLogFileName = "/Reward Table.txt";

    public void StartLottery()
    {
        path = Application.persistentDataPath;
        // See if files Exists
        FileStream logStream;

        logStream = (File.Exists(Application.persistentDataPath + lottoHistoryFileName)) ?
            File.Open(Application.persistentDataPath + lottoHistoryFileName, FileMode.Open) :
            File.Create(Application.persistentDataPath + lottoHistoryFileName);

        // Get the old data(log)
        StreamReader streamReader = new StreamReader(logStream);
        var oldData = streamReader.ReadToEnd();
        logStream.Position = 0;
        streamReader.DiscardBufferedData();

        System.Random r = new System.Random();
        List<string> finalists = new List<string>();
        int finalistCount = participants.Count / 4;
        bool lookinfForWinner = true;

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
                tempString += streamReader.ReadLine() + " ";
            }

            logStream.Position = 0;
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
        bool lookingForPayout = true;
        while (lookingForPayout)
        {
            int ticket = r.Next(0, 100);
            if (ticket <= 21 && possibleRewards.Contains(20))
            {
                winnerPayout = "$20";
                lookingForPayout = false;
            }
            else if (ticket >= 22 && ticket <= 41 && possibleRewards.Contains(30))
            {
                winnerPayout = "$30";
                lookingForPayout = false;
            }

            else if (ticket >= 42 && ticket <= 56 && possibleRewards.Contains(35))
            {
                winnerPayout = "$35";
                lookingForPayout = false;
            }

            else if (ticket >= 57 && ticket <= 70 && possibleRewards.Contains(40))
            {
                winnerPayout = "$40";
                lookingForPayout = false;
            }

            else if (ticket >= 71 && ticket <= 77 && possibleRewards.Contains(50))
            {
                winnerPayout = "$50";
                lookingForPayout = false;
            }

            else if (ticket >= 78 && ticket <= 84 && possibleRewards.Contains(60))
            {
                winnerPayout = "$60";
                lookingForPayout = false;
            }

            else if (ticket >= 85 && ticket <= 90 && possibleRewards.Contains(80))
            {
                winnerPayout = "$80";
                lookingForPayout = false;
            }

            else if (ticket >= 91 && ticket <= 94 && possibleRewards.Contains(100))
            {
                winnerPayout = "$100";
                lookingForPayout = false;
            }

            else if (ticket >= 95 && ticket <= 98 && possibleRewards.Contains(150))
            {
                winnerPayout = "$150";
                lookingForPayout = false;
            }

            else if (ticket > 98 && possibleRewards.Contains(200))
            {
                winnerPayout = "$200";
                lookingForPayout = false;
            }
        }

        logStream.Position = 0;
        StreamWriter streamWriter = new StreamWriter(logStream);
        string newData = winner + " " + winnerPayout + " " + DateTime.Now.ToString("dddd MMMM dd, yyyy HH:mm") + " ";
        streamWriter.WriteLine(newData);
        streamWriter.Write(oldData);
        //string currentLine;
        //while ((currentLine = streamReader.ReadLine()) != null)
        //{
        //    var parsedData = currentLine.Split(' ');
        //    string[] newParsedData = new string[parsedData.Length - 2];
        //    for (int i = 0; i < parsedData.Length; i++)
        //    {
        //        if (i <= 1)
        //            continue;

        //        newParsedData[i - 2] = parsedData[i];
        //    }

        //    string dateString = "";
        //    foreach (string s in newParsedData)
        //    {
        //        dateString += s + " ";
        //    }

        //    DateTime loggedTime = DateTime.ParseExact(dateString, "dddd MMMM dd, yyyy HH:mm",
        //        System.Globalization.CultureInfo.CurrentCulture);
        //    var nowTime = DateTime.Now;
        //    if (loggedTime.Year >= nowTime.Year - 1)
        //    {
        //        // If from previous year
        //        if (loggedTime.Year == nowTime.Year - 1)
        //        {
        //            DateTime newNowTime = new DateTime(nowTime.Year, nowTime.Month + 10, nowTime.Day);
        //            if (loggedTime.Month >= newNowTime.Month)
        //            {
        //                streamWriter.WriteLine(currentLine);
        //            }
        //        }
        //        //If from same year
        //        if (loggedTime.Month >= nowTime.Month - 2)
        //        {
        //            streamWriter.WriteLine(currentLine);
        //        }
        //    }
        //}

        streamWriter.Close();
        logStream.Close();

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

    public void LoadRewards()
    {
        FileStream rewardStream;

        rewardStream = (File.Exists(Application.persistentDataPath + rewardLogFileName)) ?
            File.Open(Application.persistentDataPath + rewardLogFileName, FileMode.Open) :
            File.Create(Application.persistentDataPath + rewardLogFileName);

        StreamReader streamReader = new StreamReader(rewardStream);
        var data = streamReader.ReadToEnd();

        // Else Decode Data
        List<int> result = new List<int>();
        List<string> temp = new List<string>();

        var parsedData = data.Split('!');
        foreach (string s in parsedData)
            temp.Add(s);
        foreach (string s in temp)
        {
            int o = 0;
            string number = "";
            var parsedString = s.Split(' ');
            foreach (string ss in parsedString)
            {
                Int32.TryParse(ss, out o);
                number += (char)(o / 87);
            }

            int num = 0;
            Int32.TryParse(number, out num);
            result.Add(num);
        }
        result.Remove(result.Last());
        rewardStream.Close();

        possibleRewards = result;
    }

    public void WriteRewards()
    {
        if (possibleRewards.Contains(0))
            possibleRewards.Remove(0);

        // Encoding Sequence
        string result = "";
        foreach (int i in possibleRewards)
        {
            string temp = i.ToString();
            foreach (char c in temp)
            {
                result += (int)c * 87;
                result += " ";
            }
            result += '!';
        }

        File.WriteAllText(Application.persistentDataPath + rewardLogFileName, result);
    }

    public void CheckPassword()
    {
       if(passwordInput.text == "iloveyou143")
        {
            foreach(GameObject go in rewardToggles)
            {
                go.SetActive(true);
            }
            ColorBlock newColor = ColorBlock.defaultColorBlock;
            newColor.normalColor = Color.green;
            newColor.highlightedColor = Color.green;

            passwordInput.colors = newColor;

            foreach(GameObject go in rewardToggles)
            {
                var toggle = go.GetComponent<Toggle>();
                string rewardString = go.GetComponentInChildren<Text>().text.Remove(0, 1);
                int rewardInt;
                Int32.TryParse(rewardString, out rewardInt);

                toggle.isOn = (possibleRewards.Contains(rewardInt)) ? true : false;
            }

        }

        else
        {
            ColorBlock newColor = ColorBlock.defaultColorBlock;
            newColor.normalColor = Color.red;
            newColor.highlightedColor = Color.red;

            passwordInput.colors = newColor;
        }
    }

    public void ResetColorBlock()
    {
        ColorBlock newColor = ColorBlock.defaultColorBlock;
        passwordInput.colors = newColor;
    }

    public void OnRewardToggle(GameObject go)
    {
        bool toggle = go.GetComponent<Toggle>().isOn;
        int reward = 0;
        Int32.TryParse(go.GetComponentInChildren<Text>().text.Remove(0, 1), out reward);

        if (toggle == true && !possibleRewards.Contains(reward))
        {
            possibleRewards.Add(reward);
        }

        else if (toggle == false && possibleRewards.Contains(reward))
        {
            possibleRewards.Remove(reward);
        }
    }
}

