using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class clipBoard : MonoBehaviour
{

    RoundData roundData;
    [SerializeField] GameObject text;
    private TextMeshPro clipBoardText;
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
            res += "- People need to weight at least " + roundData.notice.minWeightRestriction.ToString() + "Kg" + "<br>";
        }
        if(roundData.notice.maxWeightRestrictionEnabled)
        {
            res += "- People need to weight at most " + roundData.notice.maxWeightRestriction.ToString() + "Kg" + "<br>";
        }
        if(roundData.notice.heightLimitEnabled)
        {
            res += "- People need to be at least " + roundData.notice.heightLimit.ToString() + " cm tall" + "<br>";
        }
        if(roundData.notice.kidsAreForbidenEnabled)
        {
            res += "- Kids are not allowed" + "<br>";
        }
        if(roundData.notice.kidsNotAllowedAfterHourEnabled)
        {
            res += "- Kids are not allowed after " + roundData.notice.kidsNotAllowedAfterHour.ToString() + "<br>";
        }
        if(roundData.notice.maxAgeRestrictionEnabled)
        {
            res += "- People shouldn't be older than " + roundData.notice.maxAgeRestriction.ToString() + " years old" + "<br>";
        }
        if(roundData.notice.validityExtensionEnabled)
        {
            res += "- Tickets are valid for " + roundData.notice.validityExtensionDays.ToString() + " more days" + "<br>";
        }
        text.GetComponent<TextMeshPro>().text = res;
    }
}
