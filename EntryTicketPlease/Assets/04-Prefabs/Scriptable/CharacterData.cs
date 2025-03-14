using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/CharacterData", order = 0)]
public class CharacterData : ScriptableObject
{

    [SerializeField] private GameObject[] maleVisitorsPrefab;  
    [SerializeField] private GameObject[] femaleVisitorsPrefab; 

   
    public GameObject GetVisitorModel(Gender genre)
    {
        switch (genre)
        {
            case Gender.Male:
                return maleVisitorsPrefab[Random.Range(0,maleVisitorsPrefab.Length)];  
            case Gender.Female:
                return femaleVisitorsPrefab[Random.Range(0, femaleVisitorsPrefab.Length)];
            default:
                return null;
        }
    }
}
