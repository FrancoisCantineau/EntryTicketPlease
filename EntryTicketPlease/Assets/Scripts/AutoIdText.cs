using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoIdText : MonoBehaviour
{
    [SerializeField] GameObject name;
    [SerializeField] GameObject gender;
    [SerializeField] GameObject age;
    [SerializeField] GameObject weight;
    [SerializeField] GameObject size;
    // Start is called before the first frame update
    void Start()
    {
        Visitor.VisitorID id = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().id;
        name.GetComponent<TextMeshProUGUI>().text = id.Name;
        gender.GetComponent<TextMeshProUGUI>().text = id.Gender.ToString();
        age.GetComponent<TextMeshProUGUI>().text = id.Age.ToString();
        weight.GetComponent<TextMeshProUGUI>().text = Math.Round(id.Weight).ToString();
        size.GetComponent<TextMeshProUGUI>().text = Math.Round(id.Height).ToString();
    }
}
