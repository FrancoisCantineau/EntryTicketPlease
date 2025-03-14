using System;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    /// <summary>
    /// Date du jour
    /// </summary>
    DateTime currentDate; 
    
    /// <summary>
    /// Heure de d�part du shift
    /// </summary>
    int startingHour;

    /// <summary>
    /// Longueur du shift
    /// </summary>
    int shiftLength;

    /// <summary>
    /// Nombre de personnes voulant entrer
    /// </summary>
    int queueLength;

    /// <summary>
    /// Compris entre 0 et 1
    /// Chances que les documents fournis par la personne pr�sentent au moins une incoh�rence
    /// </summary>
    float incoherencesOdds;


    /// <summary>
    /// R�gles suppl�mentaires pour la journ�es
    /// </summary>
    Notice notice;


    public static RoundData Default(DateTime currentDate)
    {
        RoundData data = new();
        data.startingHour = 8;
        data.shiftLength = 10;
        data.queueLength = 15;
        data.incoherencesOdds = 0.2f;

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
    bool heightLimitEnabled;
    float heightLimit;

    /// <summary>
    /// Y a t-il une extension de validit� des tickets ?
    /// </summary>
    bool validityExtensionEnabled;
    int validityExtensionDays;

    /// <summary>
    /// Les enfants sont ils autoris�s ? 
    /// </summary>
    bool kidsAreForbidenEnabled;

    /// <summary>
    /// Y a t'il un age au del� duquel les visiteurs ne sont pas accept�s ? 
    /// </summary>
    bool maxAgeRestrictionEnabled;
    int maxAgeRestriction;

    /// <summary>
    /// Y a t-il un poids maximal autoris� ? 
    /// </summary>
    bool maxWeightRestrictionEnabled;
    int maxWeightRestriction;

    /// <summary>
    /// Y a t'il un poids minimal requis ? 
    /// </summary>
    bool minWeightRestrictionEnabled;
    int minWeightRestriction;

    /// <summary>
    /// Y a t-il une heure apr�s laquelle les enfants ne sont plus admis ? 
    /// </summary>
    bool kidsNotAllowedAfterHourEnabled;
    int kidsNotAllowedAfterHour;
}