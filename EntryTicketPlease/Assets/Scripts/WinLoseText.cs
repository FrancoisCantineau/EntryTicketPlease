using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLoseText : MonoBehaviour
{
    [SerializeField] bool isWinning;
    [SerializeField] TextMeshProUGUI winLosetext;
    // Start is called before the first frame update
    void Start()
    {
        if (isWinning)
        {
            winLosetext.text = "You Win";
        }
        else
        {
            winLosetext.text = "You lose";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
