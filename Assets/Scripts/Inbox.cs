using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using JSONObject = System.Collections.Generic.Dictionary<string, object>;

namespace DDNAInboxTutorial
{
    [System.Serializable]
    public class EmailList
    {        
        #region properties
        public List<Email> Emails = new List<Email>();               
        private string filename = Application.persistentDataPath + "/emails.dat";
        private static bool isLoading = false;
        #endregion

        #region methods

        public EmailList()
        {            
            if (!isLoading)
                Load();             
        }
        public int Count()
        {           
            return this.Emails.Count;
        }
        public int UnreadCount()
        {
            int counter = 0; 
            foreach(Email e in this.Emails)
            {
                if (!e.read) counter++;
            }
            return counter;
        }
        public void Add(Email email)
        {
            Emails.Add(email);            
            Save();
            
        }
        public void Remove(Email email)
        {
            Emails.Remove(email);
            Debug.Log("Removed Email from List :" + this.Emails.Count + " emails.");
            Save();
        }

        public void Clear()
        {
            this.Emails.Clear();
            Debug.Log("Cleared Email List :" + this.Emails.Count + " emails.");
            Save();
        }
        private void Load()
        {
            isLoading = true;
            this.Emails.Clear();

            Debug.Log(filename);
            // JSON string that will be parsed in to tournament List
            string json = null;

            // Check local storage file exists, then load
            // ==========================================
            if (File.Exists(filename))
            {
                json = File.ReadAllText(filename);                                
            }

            // Parse JSON in to Tournament List object
            // =======================================
            if (json != null)
            {
                Emails = this.FromJSON(json);
            }

            isLoading = false;
            Debug.Log("Loaded Email List :" + this.Emails.Count + " emails.");
        }

        // Save JSON Tournament List to Local Storage
        // ==========================================
        private void Save()
        {
            if (isLoading ) return;

            string emailData = this.ToJSON();
            File.WriteAllText(filename, emailData);            
        }

        // Convert the Email List to a JSON string
        // ============================================
        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }

        // Convert JSON string in to a List of Emails
        // ===============================================
        public List<Email> FromJSON(string json)
        {
            EmailList tempEmailList = new EmailList();

            tempEmailList = JsonUtility.FromJson<EmailList>(json);
            return tempEmailList.Emails;
        }
        #endregion

    }


    [System.Serializable]

    public class Email
    {
        #region properties     
        public string id;
        public string subject;
        public string message;
        public string sender;
        public string sent;
        public bool read;
        public string expiryTimestamp;
        public string action;
        public string value;
        public int amount;
        #endregion

        // Basic Constructor
        public Email()
        {

        }

        // Constructor
        public Email(string subject, string message, string sender, DateTime expiryTimestamp, string action, string value, int amount)
        {
            // Instantiate new Email
            this.subject = subject;
            this.message = message;
            this.sender = sender;
            this.sent = DateTime.Now.ToString();
            this.read = false;
            this.expiryTimestamp = expiryTimestamp.ToString();
            this.action = action;
            this.value = value;
            this.amount = amount;
        }

        public Email(JSONObject engageResponse)
        {
            object parameters;
            engageResponse.TryGetValue("parameters", out parameters);

            JSONObject p = parameters as JSONObject;

            if (engageResponse.ContainsKey("transactionID")) this.id = engageResponse["transactionID"].ToString();
            if (engageResponse.ContainsKey("message")) this.message = engageResponse["message"].ToString();
            if (engageResponse.ContainsKey("heading")) this.subject = engageResponse["heading"].ToString();

            if (p.ContainsKey("mailAction")) this.action = p["mailAction"].ToString();
            if (p.ContainsKey("mailActionValue")) this.value = p["mailActionValue"].ToString();
            if (p.ContainsKey("mailActionAmount")) this.amount = System.Convert.ToInt32(p["mailActionAmount"]);

            if (p.ContainsKey("mailActionExpiryDuration")) this.expiryTimestamp = System.DateTime.Now.AddHours(System.Convert.ToInt64(p["mailActionExpiryDuration"])).ToString();
            if (p.ContainsKey("mailActionExpiryTimestamp")) this.expiryTimestamp = p["mailActionExpiryDuration"].ToString();

            this.read = false;
            this.sender = "deltaDNA";
            this.sent = System.DateTime.Now.ToString();
        }



        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }


        public Email FromJSON(string email)
        {
            Email t = new Email();
            t = JsonUtility.FromJson<Email>(email);

            return t;
        }
    }
}