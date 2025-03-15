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

    void Start()
    {
        Visitor.VisitorTicket ticket = VisitorsManager.Instance.GetVisitor().GetComponent<Visitor>().ticket;
        prenom.GetComponent<TextMeshProUGUI>().SetText(ticket.TicketName);
        section.GetComponent<TextMeshProUGUI>().SetText(ticket.Section.ToString());
        prix.GetComponent<TextMeshProUGUI>().text = ticket.Price.ToString();
    }
}
