using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Visitor : MonoBehaviour
{
    [SerializeField] public GameObject prefab;

    [SerializeField] public bool isAllowed;
    

    [System.Serializable] // Visitor's ID struct containing all infos for ID
    public struct VisitorID
    {
        [SerializeField] private string name;
        [SerializeField] private int age;
        [SerializeField] private float height;
        [SerializeField] private float weight;
        [SerializeField] public Gender genre;
    

        public VisitorID(string m_name, int m_age, float m_height, float m_weight, Gender m_genre)//Constructor
        {
            this.name = m_name;
            this.age = m_age;
            this.height = m_height;
            this.weight = m_weight;
            this.genre = m_genre;
        }

        // Getters (Properties)
        public string Name
        {
            get { return name; }
        }

        public int Age
        {
            get { return age; }
        }

        public float Height
        {
            get { return height; }
        }

        public float Weight
        {
            get { return weight; }
        }

        public Gender Gender
        {
            get { return genre; }
        }


        public override string ToString()
        {
            return $"Name: {name}, Age: {age}, Height: {height}, Weight: {weight}, Genre: {genre}";
        }
    }
    [System.Serializable]
    public struct VisitorTicket// Visitor's Ticket struc containing all infos for ticket
    {


        [SerializeField] private string ticketName;
        [SerializeField] private int price;
        [SerializeField] private bool isValid;
        [SerializeField] private char section;

        

        public VisitorTicket(string m_name, int m_price, bool m_isValid, char m_section)//Constructor
        {
            this.ticketName = m_name;
            this.price = m_price;
            this.isValid = m_isValid;
            this.section = m_section;
        }

        // Getters (Properties)
        public string TicketName
        {
            get { return ticketName; }
        }

        public int Price
        {
            get { return price; }
        }

        public bool IsValid
        {
            get { return isValid; }
        }

        public char Section
        {
            get { return section; }
        }


        public override string ToString()
        {
            return $"Name: {ticketName}, Price: {price}, IsValid: {isValid}, Section: {section}";
        }
    }


    public VisitorTicket ticket;
    public VisitorID id;

 
    //Initialize visitor's informations

    public void Initialize(VisitorID m_id, VisitorTicket m_ticket)
    {
        ticket = m_ticket;
        id = m_id;  
        
    }
    public bool GetIsAllowed()
    {
        return isAllowed;
    }
    public void SetIsAllowed(bool m_isAllowed)
    {
  
        isAllowed = m_isAllowed;
      
    }

    public void SetPrefab(GameObject m_prefab)
    {
        prefab = m_prefab;
    }

}
