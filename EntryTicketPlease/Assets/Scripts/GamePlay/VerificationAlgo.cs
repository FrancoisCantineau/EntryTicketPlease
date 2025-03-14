using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificationAlgo : MonoBehaviour
{

    private float minHeight;
    private char forbiddenSection;
    private float maxWeight;

    /// <summary>
    /// Delegate function to choose verifications for each day.
    /// </summary>
    /// <param name="visitor"></param>
    /// <returns></returns>
    public delegate bool VisitorCondition(Visitor visitor);


    private List<VisitorCondition> activeConditions = new List<VisitorCondition>();

    public void UpdateAlgorithm()
    {
        
    }

    /// <summary>
    /// Adds a new function for the delegate
    /// </summary>
    /// <param name="condition"></param>
    private void AddCondition(VisitorCondition condition)
    {
        if (!activeConditions.Contains(condition))
        {
            activeConditions.Add(condition);
        }
    }

    /// <summary>
    /// Removes a function for the delegate
    /// </summary>
    /// <param name="condition"></param>
    public void RemoveCondition(VisitorCondition condition)
    {
        if (activeConditions.Contains(condition))
        {
            activeConditions.Remove(condition);
        }
    }

    /// <summary>
    /// Check is the visitor is allowed depending on the conditions
    /// </summary>
    /// <param name="Visitor"></param>
    public bool IsVisitorAllowed(Visitor visitor)
    {
        foreach (var condition in activeConditions)
        {
            if (!condition(visitor))
            {
                return false; // if fails, visitor not allowed
            }
        }
        return true;
    }

    //DEFAULT CONDITIONS
    private bool HasValidTicket(Visitor visitor)
    {
        return visitor.ticket.IsValid;
    }

    private bool AgeIsRight(Visitor visitor)
    {
        return visitor.id.Age == visitor.ticket.Age;
    }

    private bool NameIsRight(Visitor visitor)
    {
        return visitor.ticket.Name == visitor.id.Name;
    }

    //VARIABLE CONDITIONS

    private bool HasMinHeight(Visitor visitor)
    {
        return visitor.id.Height >= minHeight;
    }

    private bool IsSectionValid(Visitor visitor)
    {
        return visitor.ticket.Section != forbiddenSection;
    }
    private bool HasValidWeight(Visitor visitor)
    {
        return visitor.id.Weight <= maxWeight;
    }


}



