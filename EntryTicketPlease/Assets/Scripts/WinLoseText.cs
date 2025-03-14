using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseText : MonoBehaviour
{
    [SerializeField] bool isWinning;
    [SerializeField] TextMeshProUGUI winLosetext;
    [SerializeField] Button nextDay;
    [SerializeField] RawImage star1;
    [SerializeField] RawImage star2;
    [SerializeField] RawImage star3;
    [SerializeField] int nbStars;
    // Start is called before the first frame update
    void Start()
    {
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        if (isWinning)
        {
            winLosetext.text = "You Win";
            switch (nbStars)
            {
                case 1:
                    star1.gameObject.SetActive(true);
                    break;
                case 2:
                    star1.gameObject.SetActive(true);
                    star2.gameObject.SetActive(true);
                    break;
                case 3:
                    star1.gameObject.SetActive(true);
                    star2.gameObject.SetActive(true);
                    star3.gameObject.SetActive(true);
                    break;
            }
        }
        else
        {
            nextDay.gameObject.SetActive(false);
            winLosetext.text = "You lose";
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
