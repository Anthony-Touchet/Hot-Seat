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

    public void ButtonPress(GameObject go)
    {
        var text = go.GetComponentInChildren(typeof(Text)) as Text;
        var lottoNum = text.text;

        if (participants.Contains(lottoNum))
        {
            participants.Remove(lottoNum);
            return;
        }
            
        participants.Add(lottoNum);
    }
}
