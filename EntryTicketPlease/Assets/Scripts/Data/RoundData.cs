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
    /// Heure de départ du shift
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
    /// Chances que les documents fournis par la personne présentent au moins une incohérence
    /// </summary>
    float incoherencesOdds;


    /// <summary>
    /// Règles supplémentaires pour la journées
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
    /// Faut-il prêter attention à la règle de taille ? 
    /// </summary>
    bool heightLimitEnabled;
    float heightLimit;

    /// <summary>
    /// Y a t-il une extension de validité des tickets ?
    /// </summary>
    bool validityExtensionEnabled;
    int validityExtensionDays;

    /// <summary>
    /// Les enfants sont ils autorisés ? 
    /// </summary>
    bool kidsAreForbidenEnabled;

    /// <summary>
    /// Y a t'il un age au delà duquel les visiteurs ne sont pas acceptés ? 
    /// </summary>
    bool maxAgeRestrictionEnabled;
    int maxAgeRestriction;

    /// <summary>
    /// Y a t-il un poids maximal autorisé ? 
    /// </summary>
    bool maxWeightRestrictionEnabled;
    int maxWeightRestriction;

    /// <summary>
    /// Y a t'il un poids minimal requis ? 
    /// </summary>
    bool minWeightRestrictionEnabled;
    int minWeightRestriction;

    /// <summary>
    /// Y a t-il une heure après laquelle les enfants ne sont plus admis ? 
    /// </summary>
    bool kidsNotAllowedAfterHourEnabled;
    int kidsNotAllowedAfterHour;
}