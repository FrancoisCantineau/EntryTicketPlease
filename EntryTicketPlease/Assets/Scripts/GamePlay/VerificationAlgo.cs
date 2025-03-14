using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificationAlgo : MonoBehaviour
{
    public static VerificationAlgo Instance;

    private float minHeight;

    private char  forbiddenSection;

    private float maxWeight;
    private float minWeight;

    private int minAge;
    private int maxAge;

    /// <summary>
    /// Delegate function to choose verifications for each day.
    /// </summary>
    /// <param name="visitor"></param>
    /// <returns></returns>
    public delegate bool VisitorCondition(Visitor visitor);


    private List<VisitorCondition> activeConditions = new List<VisitorCondition>();

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void UpdateAlgorithm(RoundData roundData)
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
    private void AddCondition(VisitorCondition condition)
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

    private bool AgeMatch(Visitor visitor)
    {
        return visitor.id.Age == visitor.ticket.Age;
    }

    private bool HasValidName(Visitor visitor)
    {
        return visitor.ticket.Name == visitor.id.Name;
    }

    //VARIABLE CONDITIONS

    private bool HasValidHeight(Visitor visitor)
    {
        return visitor.id.Height >= minHeight ;
    }

    private bool HasValidSection(Visitor visitor)
    {
        return visitor.ticket.Section != forbiddenSection;
    }
    private bool HasValidWeight(Visitor visitor)
    {
        return visitor.id.Weight <= maxWeight && visitor.id.Weight >= minWeight;
    }
    private bool HasValidAge(Visitor visitor)
    {
        return visitor.id.Age >=minAge && visitor.id.Age <= maxAge;
    }


}



