using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.Analytics;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class VisitorsManager : SingletonMB<VisitorsManager>
{
    private float validityTreshHoldValue = 0.3f; //Used to increase or decrease invalid tickets
    private float fraudValue = 0f; //Variable to increase or decrease fraud among visitors

    private int currentVisitorIndex = 0;
    private string ticketName;
    private int ticketPrice;

    [SerializeField] private GameObject currentVisitor;

    public Transform spawnPoint;

    private string[] menNames = { "Bastien", "Mathias", "Francois", "Clement" };
    private string[] womenNames = { "Lucie", "Julie", "Marion", "Celine" };
    private char[] sections = { 'A', 'B', 'C'};

    

    [SerializeField] private CharacterData characterData;

    private List<Visitor> visitorQueue = new List<Visitor>();

    public UnityEvent spawnEvent;
    /// <summary>
    /// Demands a float parameter between 0.0f and 1.0f. 
    /// 0.0 = 0% fraud and 1.0 = 100% fraud 
    /// </summary>
    private void Start()
    {
        spawnEvent = new UnityEvent();
    }
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
    /// Returns Visitor currently used
    /// </summary>
    public GameObject GetVisitor()
    {
        return currentVisitor;
    }

    public int GetQueueSize()
    {
        return visitorQueue.Count;
    }


    public bool  NextVisitor()
    {
        if (currentVisitorIndex < visitorQueue.Count)
        {
            SpawnVisitors();
            return true;
        }
        else
        {
            Debug.Log("Aucun autre visiteur dans la queue.");
            return false;
        }
    }
    



    private void InitializeVisitor(int i)
    { 
        

        GameObject newVisitorObject = new GameObject("Visitor" + i);
        Visitor addedVisitor = newVisitorObject.AddComponent<Visitor>();



        Gender[] genres = { Gender.Male, Gender.Female };
        Gender randomGender = genres[Random.Range(0, genres.Length)];

        GameObject prefabVisitor = characterData.GetVisitorModel(randomGender);
      

        bool hasValidTicket = Random.value > validityTreshHoldValue;

        string name = "none";
        ticketName = name;

        int age = Random.Range(GameSettings.ChildrensMinAge, GameSettings.AdultsMaxAge);

        ticketPrice = GameSettings.priceTable
    .FirstOrDefault(entry => age >= entry.Key.min_age && age <= entry.Key.max_age)
    .Value;



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
                height = Random.Range(GameSettings.VisitorMinHeight, 140f);
                weight = Random.Range(GameSettings.VisitorMinWeight, 45f);
            }
            else if (age < 18) // Teens
            {
                height = Random.Range(140f, 180f);
                weight = Random.Range(40f, 75f);
            }
            else // Adults
            {
                height = Random.Range(170f, GameSettings.VisitorMaxHeight);
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
                height = Random.Range(GameSettings.VisitorMinHeight, 140f);
                weight = Random.Range(GameSettings.VisitorMinWeight, 40f);
            }
            else if (age < 18) //Teens
            {
                height = Random.Range(140f, 175f);
                weight = Random.Range(40f, 65f);
            }
            else // Adults
            {
                height = Random.Range(150f, GameSettings.VisitorMinHeight);
                float bmi = Random.Range(18f, 30f);
                weight = bmi * (height / 100) * (height / 100);
            }
        }

        //Check if visitor will fraud
        if (Random.value < fraudValue)
        {
            Fraud(randomGender, age);
        }

        
        addedVisitor.SetPrefab(prefabVisitor);

       //fill visitor's structs
        Visitor.VisitorID id = new Visitor.VisitorID(name, age, height, weight, randomGender);
        Visitor.VisitorTicket ticket = new Visitor.VisitorTicket(ticketName, ticketPrice, hasValidTicket, section);

       
        addedVisitor.Initialize(id, ticket);
        
        visitorQueue.Add(addedVisitor);

        Destroy(newVisitorObject);
    }


    /// <summary>
    /// Modifies the infos for the ticket
    /// </summary>
    private void Fraud(Gender genre, int age)
    {

            bool changeName = Random.value < fraudValue;
            bool changePrice = Random.value < fraudValue;

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
    
        //Price fraud
        if (changePrice)
            {
                
                int newPrice = ticketPrice ;

            List<int> lowerPrices = GameSettings.priceTable.Values
            .Where(price => price < ticketPrice)
            .ToList();

            newPrice = lowerPrices.Count > 0 ? lowerPrices[Random.Range(0, lowerPrices.Count)] : ticketPrice;

            Debug.Log($"�ge : {age}, Prix r�el : {ticketPrice}, Prix modifi� : {newPrice}");

            ticketPrice = newPrice ;
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

                    visitorToSpawn.isAllowed = true;

                    GameObject currentVisit = Instantiate(visitorToSpawn.prefab, spawnPoint.position, Quaternion.identity);
                    currentVisit.name = "Visitor" + currentVisitorIndex;

                    Visitor newVisitor = currentVisit.AddComponent<Visitor>();

                    newVisitor.Initialize(visitorToSpawn.id, visitorToSpawn.ticket);
                    newVisitor.SetPrefab(visitorToSpawn.prefab);

                    newVisitor.AddComponent<CharacterNavMeshMovement3D>();
                    newVisitor.AddComponent<NavMeshAgent>();

                    currentVisitor = currentVisit;

                    currentVisitor.AddComponent<CharacterNavMeshMovement3D>();
                    currentVisitor.AddComponent<NavMeshAgent>();

                    currentVisitorIndex++;

                    spawnEvent.Invoke();
                }

            }
            else
            {
                Debug.Log("La queue des visiteurs est vide.");
            }
        }
        
    }

    /// <summary>
    /// This method check if the current visitor is allowed in the park
    /// Returns result
    /// </summary>
    public bool CheckValidityCurrentVisitor()
    {
        Visitor visitorComponent = currentVisitor.GetComponent<Visitor>();

        visitorComponent.SetIsAllowed(VerificationAlgo.IsVisitorAllowed(visitorComponent));

        return visitorComponent.GetIsAllowed();

    }


}
