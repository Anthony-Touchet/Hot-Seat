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

    public InputField passwordInput;
    public InputField rewardInput;
    public GameObject loginUI;
    public GameObject rewardUI;
    public RectTransform rewardContentRoot;
    public GameObject rewardPrefab;

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
    private List<string> activeRewards = new List<string>();              //Active rewards to be pulled from for the winner


    public GameObject rewardFailsafeText;

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

        if(activeRewards.Count == 0){
            rewardFailsafeText.SetActive(true);
            return;
        }

        else{
            rewardFailsafeText.SetActive(false);
        }

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
            string tempString = null;
            for(int i = 0; i < selectionWait; i++) 
            {
                var emptyCheck = streamReader.ReadLine();
                if(emptyCheck != "")
                    tempString += emptyCheck + " ";
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
            if(activeRewards.Count == 0)
            {
                winnerPayout = "$20";
                lookingForPayout = false;
            }

            int ticket = r.Next(0, 100);

            if(ticket == 100)
            {
                winnerPayout = activeRewards[activeRewards.Count - 1];
                lookingForPayout = false;
                continue;
            }

            var increment = 100 / activeRewards.Count;
            int pos = 0;

            while(ticket > increment)
            {
                pos++;
                ticket -= increment;
            }

            winnerPayout = activeRewards[pos];
            lookingForPayout = false;

            //if (ticket <= 21 && activeRewards.Contains("$20"))
            //{
            //    winnerPayout = "$20";
            //    lookingForPayout = false;
            //}
            //else if (ticket >= 22 && ticket <= 41 && activeRewards.Contains("$30"))
            //{
            //    winnerPayout = "$30";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 42 && ticket <= 56 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$35";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 57 && ticket <= 70 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$40";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 71 && ticket <= 77 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$50";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 78 && ticket <= 84 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$60";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 85 && ticket <= 90 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$80";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 91 && ticket <= 94 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$100";
            //    lookingForPayout = false;
            //}

            //else if (ticket >= 95 && ticket <= 98 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$150";
            //    lookingForPayout = false;
            //}

            //else if (ticket > 98 && activeRewards.Contains(""))
            //{
            //    winnerPayout = "$200";
            //    lookingForPayout = false;
            //}
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

            if(participants.Count < 10 && activeRewards.Count <= 0)
            {
                LottoStartButton.SetActive(false);
            }

            return;
        }

        participants.Add(lottoNum);

        newColor.highlightedColor = new Color(.5f, .5f, 0);
        newColor.normalColor = new Color(.5f, .5f, 0);
        toggle.colors = newColor;

        if (participants.Count >= 10 && activeRewards.Count > 0)
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
        List<string> result = new List<string>();
        List<string> temp = new List<string>();

        var parsedData = data.Split('!');
        foreach (string s in parsedData)
            temp.Add(s);
        foreach (string s in temp)
        {
            int o = 0;
            string rewardCoded = "";
            var parsedString = s.Split(' ');
            foreach (string ss in parsedString)
            {

                Int32.TryParse(ss, out o);
                var n = (char)((o / 87));
                if (n == '\0')
                    continue;
                rewardCoded += n;
            }

            result.Add(rewardCoded);
        }
        result.Remove(result.Last());
        rewardStream.Close();

        activeRewards = result;
        rewardFailsafeText.SetActive((activeRewards.Count <= 0) ? true : false);
    }

    public void WriteRewards()
    {
        if (activeRewards.Contains(""))
            activeRewards.Remove("");

        // Encoding Sequence
        string result = "";
        foreach (string i in activeRewards)
        {
            string temp = i.ToString();
            foreach (char c in temp)
            {
                var sam = (int)((c * 87));
                result += sam;
                result += " ";
            }

            result.Remove(result.Length - 1);
            result += '!';
        }

        File.WriteAllText(Application.persistentDataPath + rewardLogFileName, result);
    }

    public void CheckPassword()
    {
       if(passwordInput.text == "iloveyou143")
        {
            loginUI.SetActive(false);
            rewardUI.SetActive(true);

            foreach(string i in activeRewards)
            {
                var inst = Instantiate(rewardPrefab, rewardContentRoot);
                inst.GetComponentInChildren<Text>().text = i;

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
        string reward = go.GetComponentInChildren<Text>().text;

        if (toggle == true && !activeRewards.Contains(reward))
        {
            activeRewards.Add(reward);
        }

        else if (toggle == false && activeRewards.Contains(reward))
        {
            activeRewards.Remove(reward);
        }
    }

    public void AddPossibleReward()
    {

        string rewardText = rewardInput.text;
        rewardInput.text = "";

        if (activeRewards.Contains(rewardText) || string.IsNullOrEmpty(rewardText) || rewardText.All(char.IsWhiteSpace))
        {
            return;
        }

        var inst = Instantiate(rewardPrefab, rewardContentRoot);
        inst.GetComponentInChildren<Text>().text = rewardText;
        activeRewards.Add(rewardText);

        rewardContentRoot.sizeDelta = new Vector2(0, activeRewards.Count * 80);
    }

    public void DeleteSelectedRewards()
    {
        List<GameObject> destroyList = new List<GameObject>();

        foreach(Transform trans in rewardContentRoot)
        {
            string transText = trans.GetComponentInChildren<Text>().text;
            if (trans.GetComponent<Toggle>().isOn)
            {
                destroyList.Add(trans.gameObject);
                activeRewards.Remove(transText);
            }
            
        }

        while (destroyList.Count > 0)
        {
            var temp = destroyList[0];
            destroyList.Remove(temp);
            Destroy(temp);
        }

        rewardContentRoot.sizeDelta = new Vector2(0, activeRewards.Count * 80);
    }
}

