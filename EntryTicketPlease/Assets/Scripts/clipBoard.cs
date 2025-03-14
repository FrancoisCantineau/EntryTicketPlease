using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class clipBoard : MonoBehaviour
{

    RoundData roundData;
    [SerializeField] TextMeshProUGUI clipBoardText;
    string res;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnEnable()
    {
        GameManager.Instance.BeginRound.AddListener(onRoundBegin);
    }
    private void OnDisable()
    {
        GameManager.Instance.BeginRound.RemoveListener(onRoundBegin);  
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void onRoundBegin(RoundData roundData)
    {
        if(roundData.notice.minWeightRestrictionEnabled)
        {
            res += "Les personnes doivent au moins peusé " + roundData.notice.minWeightRestriction.ToString() + "Kg" + "<br>";
        }
        if(roundData.notice.maxWeightRestrictionEnabled)
        {
            res += "Les personnes doivent peusé au macximum" + roundData.notice.maxWeightRestriction.ToString() + "Kg" + "<br>";
        }
        if(roundData.notice.heightLimitEnabled)
        {
            res += "Les personnes doivent au minimum mesuré" + roundData.notice.heightLimit.ToString() + "cm" + "<br>";
        }
        if(roundData.notice.kidsAreForbidenEnabled)
        {
            res += "Les enfants ne sont pas autorisé" + "<br>";
        }
        if(roundData.notice.kidsNotAllowedAfterHourEnabled)
        {
            res += "Les enfants ne sont plus autorisé apres " + roundData.notice.kidsNotAllowedAfterHour.ToString() + "<br>";
        }
        if(roundData.notice.maxAgeRestrictionEnabled)
        {
            res += "Les personne ne doivent pas avoir plus de " + roundData.notice.maxAgeRestriction.ToString() + "ans" + "<br>";
        }
        if(roundData.notice.validityExtensionEnabled)
        {
            res += "Les billets on une extension de validitée de " + roundData.notice.validityExtensionDays.ToString() + "jours" + "<br>";
        }
        clipBoardText.text = res;
    }
}
