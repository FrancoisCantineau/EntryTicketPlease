using System;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    /// <summary>
    /// Date du jour
    /// </summary>
    public DateTime currentDate; 
    
    /// <summary>
    /// Heure de départ du shift
    /// </summary>
    public int startingHour;

    public int endingHour;

    /// <summary>
    /// Longueur du shift
    /// </summary>
    public int shiftLength;

    /// <summary>
    /// Nombre de personnes voulant entrer
    /// </summary>
    public int queueLength;

    /// <summary>
    /// Compris entre 0 et 1
    /// Chances que les documents fournis par la personne présentent au moins une incohérence
    /// </summary>
    public float incoherencesOdds;

    /// <summary>
    /// Compris entre 0 et 1
    /// Chances que le visiteur tente vraiment de frauder
    /// </summary>
    public float fraudOdds;

    /// <summary>
    /// Règles supplémentaires pour la journées
    /// </summary>
    public Notice notice;


    /// <summary>
    /// Une section du parc est-elle fermée
    /// </summary>
    public ClosedSection closedSection;

    public static RoundData Default(DateTime currentDate)
    {
        RoundData data = new();
        data.currentDate = currentDate;
        data.startingHour = 8;
        data.endingHour = 18;
        data.shiftLength = 10;
        data.queueLength = 15;
        data.incoherencesOdds = 0.2f;
        data.fraudOdds = 0.1f;
        data.notice = new();
        data.closedSection = ClosedSection.N;
        return data;
    }
}

/// <summary>
/// Conditions additionnelles pour accepter ou refuser un visiteur
/// </summary>
public struct Notice
{
    /// <summary>
    /// Faut-il prêter attention à la règle de taille ? 
    /// </summary>
    public bool heightLimitEnabled;
    public float heightLimit;

    /// <summary>
    /// Y a t-il une extension de validité des tickets ?
    /// </summary>
    public bool validityExtensionEnabled;
    public int validityExtensionDays;

    /// <summary>
    /// Les enfants sont ils autorisés ? 
    /// </summary>
    public bool kidsAreForbidenEnabled;

    /// <summary>
    /// Y a t'il un age au delà duquel les visiteurs ne sont pas acceptés ? 
    /// </summary>
    public bool maxAgeRestrictionEnabled;
    public int maxAgeRestriction;

    /// <summary>
    /// Y a t-il un poids maximal autorisé ? 
    /// </summary>
    public bool maxWeightRestrictionEnabled;
    public int maxWeightRestriction;

    /// <summary>
    /// Y a t'il un poids minimal requis ? 
    /// </summary>
    public bool minWeightRestrictionEnabled;
    public int minWeightRestriction;

    /// <summary>
    /// Y a t-il une heure après laquelle les enfants ne sont plus admis ? 
    /// </summary>
    public bool kidsNotAllowedAfterHourEnabled;
    public int kidsNotAllowedAfterHour;

    /// <summary>
    /// Y a t-il une section interdite ? 
    /// </summary>
    public bool ForbiddenSectionEnabled;
    public ClosedSection closedSection;
}

public enum ClosedSection
{
    N,
    A,
    B,
    C
}