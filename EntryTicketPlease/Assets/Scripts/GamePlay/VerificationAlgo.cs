using System.Collections;
using System.Collections.Generic;
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

        //Default conditions
        AddCondition(HasValidTicket);
        AddCondition(AgeMatch);
        AddCondition(HasValidName);


        if (roundData.notice.heightLimitEnabled)
        {
            AddCondition(HasValidHeight);
            minHeight = roundData.notice.heightLimit;
        }
        if (roundData.notice.maxWeightRestrictionEnabled)
        {
            AddCondition(HasValidWeight);
            maxWeight = roundData.notice.maxWeightRestriction;
        }
        if (roundData.notice.minWeightRestrictionEnabled)
        {
            AddCondition(HasValidHeight);
            maxWeight = roundData.notice.minWeightRestriction;
        }
        if (roundData.notice.maxAgeRestrictionEnabled)
        {
            AddCondition(HasValidAge);
            maxAge = roundData.notice.maxAgeRestriction;
        }
        if (roundData.notice.kidsAreForbidenEnabled)
        {
            AddCondition(HasValidAge);
            minAge = 10;
        }
        if (roundData.notice.ForbiddenSectionEnabled)
        {
            AddCondition(HasValidSection);
            forbiddenSection = forbiddenSection = roundData.notice.closedSection.ToString()[0]; 
        }

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

    private static bool AgeMatch(Visitor visitor)
    {
        return visitor.id.Age == visitor.ticket.Age;
    }

    private static bool HasValidName(Visitor visitor)
    {
        return visitor.ticket.Name == visitor.id.Name;
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
        return visitor.id.Age >=minAge && visitor.id.Age <= maxAge;
    }


}



