using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VerificationAlgo
{
    private static float minHeight;

    private static char  forbiddenSection;

    private static float maxWeight;
    private static float minWeight;

    private static int minAge;
    private static int maxAge;

    /// <summary>
    /// Delegate function to choose verifications for each day.
    /// </summary>
    /// <param name="visitor"></param>
    /// <returns></returns>
    public delegate bool VisitorCondition(Visitor visitor);


    private static List<VisitorCondition> activeConditions = new List<VisitorCondition>();

    public static void UpdateAlgorithm(RoundData roundData)
    {
        activeConditions.Clear();

        minHeight = 0;
        minWeight = 0;
        maxWeight = 999;
        minAge = 0;
        maxAge = 999;

        // Default conditions
        AddCondition(HasValidTicket);
        AddCondition(HasValidName);

        // Variable conditions
        if (roundData.notice.heightLimitEnabled)
        {
            minHeight = roundData.notice.heightLimit;
            AddCondition(HasValidHeight);
        }
        if (roundData.notice.maxWeightRestrictionEnabled)
        {
            maxWeight = roundData.notice.maxWeightRestriction;
            AddCondition(HasValidWeight);
        }
        if (roundData.notice.minWeightRestrictionEnabled)
        {
            minWeight = roundData.notice.minWeightRestriction;
            AddCondition(HasValidWeight);
        }
        if (roundData.notice.maxAgeRestrictionEnabled)
        {
            maxAge = roundData.notice.maxAgeRestriction;
            AddCondition(HasValidAge);
        }
        if (roundData.notice.kidsAreForbidenEnabled)
        {
            minAge = GameSettings.ChildrensMaxAge;
            AddCondition(HasValidAge);
        }
        if (roundData.closedSection != ClosedSection.N)
        {
            AddCondition(HasValidSection);
            forbiddenSection = roundData.closedSection.ToString()[0];
        }
        if (roundData.priceGrid.childrenPriceModifEnabled || roundData.priceGrid.teensPriceModifEnabled || roundData.priceGrid.adultsPriceModifEnabled)
        {
            AddCondition(HasValidPrice);
        }

        // Afficher les conditions actives
        PrintActiveConditions();
    }

    /// <summary>
    /// Adds a new function for the delegate
    /// </summary>
    /// <param name="condition"></param>
    private static void AddCondition(VisitorCondition condition)
    {
        if (!activeConditions.Contains(condition))
        {
            activeConditions.Add(condition);
        }
    }


    /// <summary>
    /// Check is the visitor is allowed depending on the conditions
    /// </summary>
    /// <param name="Visitor"></param>
    public static bool IsVisitorAllowed(Visitor visitor)
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
    private static bool HasValidTicket(Visitor visitor)
    {
        return visitor.ticket.IsValid;
    }
    private static bool HasValidName(Visitor visitor)
    {
        return visitor.ticket.TicketName == visitor.id.Name;
    }
    

    //VARIABLE CONDITIONS

    private static bool HasValidHeight(Visitor visitor)
    {
        return visitor.id.Height >= minHeight ;
    }

    private static bool HasValidSection(Visitor visitor)
    {
        return visitor.ticket.Section != forbiddenSection;
    }
    private static bool HasValidWeight(Visitor visitor)
    {
        return visitor.id.Weight <= maxWeight && visitor.id.Weight >= minWeight;
    }
    private static bool HasValidAge(Visitor visitor)
    {
    
        return visitor.id.Age > minAge && visitor.id.Age < maxAge;
    }
    private static bool HasValidPrice(Visitor visitor)
    {
        return visitor.ticket.Price == GameSettings.priceTable
     .FirstOrDefault(entry => visitor.id.Age >= entry.Key.min_age && visitor.id.Age <= entry.Key.max_age)
     .Value;
    }

    public static void PrintActiveConditions()
    {
        Debug.Log("Active Conditions for this round:");
        foreach (var condition in activeConditions)
        {
            Debug.Log(condition.Method.Name); 
        }
    }
}




