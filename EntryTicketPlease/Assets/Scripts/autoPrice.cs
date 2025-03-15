using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class autoPrice : MonoBehaviour
{
    [SerializeField] GameObject kidsPrice;
    [SerializeField] GameObject teensPrice;
    [SerializeField] GameObject adultsPrice;
    // Start is called before the first frame update
    void Start()
    {
        kidsPrice.GetComponent<TextMeshProUGUI>().text = GameSettings.priceTable[(5, 12)].ToString() + "€";
        teensPrice.GetComponent<TextMeshProUGUI>().text = GameSettings.priceTable[(13, 17)].ToString() + "€";
        adultsPrice.GetComponent<TextMeshProUGUI>().text = GameSettings.priceTable[(18,100)].ToString()  + "€";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
