using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseText : MonoBehaviour
{
    [SerializeField] bool isWinning;
    [SerializeField] TextMeshProUGUI winLosetext;
    [SerializeField] RawImage star1;
    [SerializeField] RawImage star2;
    [SerializeField] RawImage star3;
    [SerializeField] int nbStars;
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
        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (nbStars)
        {
            case 1:
                star1.enabled = true;
                break;
            case 2:
                star1.enabled = true;
                star2.enabled = true;
                break;
            case 3:
                star1.enabled= true;
                star2.enabled= true;
                star3.enabled= true;
                break;
        }
    }
}
