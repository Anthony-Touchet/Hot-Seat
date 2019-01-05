using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hot_Seat_Lottery : MonoBehaviour
{
    public List<GameObject> buttons;
    public List<string> participants = new List<string>();

    // Start is called before the first frame update
    public void StartLottery()
    {
        
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
            return;
        }

        participants.Add(lottoNum);

        newColor.highlightedColor = Color.yellow;
        newColor.normalColor = Color.yellow;
        toggle.colors = newColor;
        return;
    }
}
