using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEntryTickets : MonoBehaviour
{
    [SerializeField] GameObject prenom;
    [SerializeField] GameObject section;
    [SerializeField] GameObject prix;
    // Start is called before the first frame update
    void Start()
    {
        VisitorsManager.Instance.spawnEvent.AddListener(defineTicketText);
    }

    // Update is called once per frame
    void Update()
    {
        prenom.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.TicketName;
        section.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.Section.ToString();
        prix.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.Price.ToString();
    }

    private void OnDisable()
    {
        VisitorsManager.Instance.spawnEvent.RemoveListener(defineTicketText);
    }

    private void defineTicketText()
    {
        prenom.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.TicketName;
        section.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.Section.ToString();
        prix.GetComponent<TextMeshProUGUI>().text = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket.Price.ToString();
    }
}
