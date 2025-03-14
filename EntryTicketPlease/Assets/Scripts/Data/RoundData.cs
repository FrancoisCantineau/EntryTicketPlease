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
    /// Heure de d�part du shift
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
    /// Chances que les documents fournis par la personne pr�sentent au moins une incoh�rence
    /// </summary>
    public float incoherencesOdds;

    /// <summary>
    /// Compris entre 0 et 1
    /// Chances que le visiteur tente vraiment de frauder
    /// </summary>
    public float fraudOdds;

    /// <summary>
    /// R�gles suppl�mentaires pour la journ�es
    /// </summary>
    public Notice notice;


    /// <summary>
    /// Une section du parc est-elle ferm�e
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
    /// Faut-il pr�ter attention � la r�gle de taille ? 
    /// </summary>
    public bool heightLimitEnabled;
    public float heightLimit;

    /// <summary>
    /// Y a t-il une extension de validit� des tickets ?
    /// </summary>
    public bool validityExtensionEnabled;
    public int validityExtensionDays;

    /// <summary>
    /// Les enfants sont ils autoris�s ? 
    /// </summary>
    public bool kidsAreForbidenEnabled;

    /// <summary>
    /// Y a t'il un age au del� duquel les visiteurs ne sont pas accept�s ? 
    /// </summary>
    public bool maxAgeRestrictionEnabled;
    public int maxAgeRestriction;

    /// <summary>
    /// Y a t-il un poids maximal autoris� ? 
    /// </summary>
    public bool maxWeightRestrictionEnabled;
    public int maxWeightRestriction;

    /// <summary>
    /// Y a t'il un poids minimal requis ? 
    /// </summary>
    public bool minWeightRestrictionEnabled;
    public int minWeightRestriction;

    /// <summary>
    /// Y a t-il une heure apr�s laquelle les enfants ne sont plus admis ? 
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