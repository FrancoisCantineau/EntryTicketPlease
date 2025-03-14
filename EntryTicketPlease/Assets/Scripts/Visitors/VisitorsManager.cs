using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class VisitorsManager : SingletonMB<VisitorsManager>
{
    private float validityTreshHoldValue = 0.3f; //Used to increase or decrease invalid tickets
    private float fraudValue = 1f; //Variable to increase or decrease fraud among visitors

    private int currentVisitorIndex = 0;
    private string ticketName;
    private int ticketAge;

    public Transform spawnPoint;

    private string[] menNames = { "Bastien", "Mathias", "Francois" };
    private string[] womenNames = { "Lucie", "Julie", "Marion" };
    private char[] sections = { 'A', 'B', 'C'};

    [SerializeField] private CharacterData characterData;

    private List<Visitor> visitorQueue = new List<Visitor>();

    void Start()
    {
       
    }



    /// <summary>
    /// Demands a float parameter between 0.0f and 1.0f. 
    /// 0.0 = 0% fraud and 1.0 = 100% fraud 
    /// </summary>
    
    public void SetFraudPercentage(float m_fraudValue)
    {
        fraudValue = m_fraudValue;
    }

    /// <summary>
    /// Set a value for ticket validity probability.
    /// Demands a float parameter between 0.0f and 1.0f. 
    /// 0.0 = 0% invalid tickets and 1.0 = 100% invalid tickets 
    /// </summary>
    public void SetValidityTreshHold(float m_validityTreshHoldValue)
    {
        validityTreshHoldValue = m_validityTreshHoldValue;
    }


    /// <summary>
    /// Creates visitors queue with a parameter to change visitor's daily amount
    /// </summary>
    public void CreateQueue(int m_visitorsAmount)
    {
        visitorQueue.Clear();

        for (int i = 0; i < m_visitorsAmount; i++)
        {
            InitializeVisitor(i);

        }
    }

    /// <summary>
    /// Start visitor's Queue & first spawn. 
    /// Parameter demands a daily visitors amount
    /// </summary>
    public void RestartVisitors(int m_visitorsAmount)
    {
      
        CreateQueue(m_visitorsAmount);
        SpawnVisitors();
    }


    public void NextVisitor()
    {
        if (currentVisitorIndex < visitorQueue.Count)
        {
            SpawnVisitors();
        }
        else
        {
            Debug.Log("Aucun autre visiteur dans la queue.");
        }
    }
    



    private void InitializeVisitor(int i)
    {
    
        Visitor newVisitor = new Visitor();

        Gender[] genres = { Gender.Male, Gender.Female };
        Gender randomGender = genres[Random.Range(0, genres.Length)];

        GameObject prefabVisitor = characterData.GetVisitorModel(randomGender);
      

        bool hasValidTicket = Random.value > validityTreshHoldValue;

        string name = "none";
        ticketName = name;

        int age = Random.Range(5, 80);
        ticketAge = age;

        

        //GameObject prefabVisitor = characterData.GetVisitorModel(randomGender);

        char section = sections[Random.Range(0, sections.Length)];

        float height = 0;
        float weight = 0;

        

        if (randomGender == Gender.Male)
        {
            name = menNames[Random.Range(0, menNames.Length)];
            ticketName = name;

             

            if (age < 12) // Childrens
            {
                height = Random.Range(90f, 140f);
                weight = Random.Range(15f, 45f);
            }
            else if (age < 18) // Teens
            {
                height = Random.Range(140f, 180f);
                weight = Random.Range(40f, 75f);
            }
            else // Adults
            {
                height = Random.Range(170f, 190f);
                float bmi = Random.Range(18f, 30f);
                weight = bmi * (height / 100) * (height / 100);
            }
        }
        else if (randomGender == Gender.Female)
        {
            name = womenNames[Random.Range(0, womenNames.Length)];
            ticketName = name;
            if (age < 12) // Children
            {
                height = Random.Range(90f, 140f);
                weight = Random.Range(15f, 40f);
            }
            else if (age < 18) //Teens
            {
                height = Random.Range(140f, 175f);
                weight = Random.Range(40f, 65f);
            }
            else // Adults
            {
                height = Random.Range(150f, 175f);
                float bmi = Random.Range(18f, 30f);
                weight = bmi * (height / 100) * (height / 100);
            }
        }

        //Check if visitor will fraud
        if (Random.value < fraudValue)
        {
            Fraud(randomGender);
        }


        newVisitor.SetPrefab(prefabVisitor);

       //fill visitor's structs
        Visitor.VisitorID id = new Visitor.VisitorID(name, age, height, weight, randomGender);
        Visitor.VisitorTicket ticket = new Visitor.VisitorTicket(ticketName, ticketAge, hasValidTicket, section);
  
        newVisitor.Initialize(id, ticket);


        //Condition check to see if allowed
        VerificationAlgo.IsVisitorAllowed(newVisitor);



        visitorQueue.Add(newVisitor);
    }


    /// <summary>
    /// Modifies the infos for the ticket
    /// </summary>
    private void Fraud(Gender genre)
    {

            bool changeName = Random.value < fraudValue;
            bool changeAge = Random.value < fraudValue;

        //Name fraud
        if (changeName)
            {
                 if (genre == Gender.Male)
                {
                    string newName = menNames[Random.Range(0, menNames.Length)];
                    ticketName = newName;
                }
                else if (genre == Gender.Female)
                {
                    string newName = womenNames[Random.Range(0, womenNames.Length)];
                    ticketName = newName;
                }

            }   
    
        //Age fraud
        if (changeAge)
            {
                
                int newAge = Random.Range(5, 80);
                ticketAge = newAge; 
            }
        
    }


    /// <summary>
    /// This method spawns visitor (the next one on the queue)
    /// </summary>
    public void SpawnVisitors()
    {
        if (visitorQueue.Count > 0)
        {

            if (currentVisitorIndex < visitorQueue.Count)
            {
                if (spawnPoint != null)
                {

                    Visitor visitorToSpawn = visitorQueue[currentVisitorIndex];

                    GameObject currentVisitor = Instantiate(visitorToSpawn.prefab, spawnPoint.position, Quaternion.identity);

                    Visitor newVisitor = currentVisitor.AddComponent<Visitor>();

                    newVisitor.Initialize(visitorToSpawn.id, visitorToSpawn.ticket);
                    newVisitor.SetPrefab(visitorToSpawn.prefab);
                    currentVisitorIndex++;
                }

            }
            else
            {
                Debug.Log("La queue des visiteurs est vide.");
            }
        }
        
    }


}
