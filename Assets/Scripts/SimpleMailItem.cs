using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DDNAInboxTutorial
{
    public class SimpleMailItem : MonoBehaviour {

        public UnityEngine.UI.Text subject;
        public UnityEngine.UI.Text sender;
        public UnityEngine.UI.Text received;
        private bool isRead;
        private string id;

        private EmailList parentEmailList;
        private Email email;

        // Use this for initialization
        void Start() {

        }

        public void SetMailDetails(Email email,EmailList parentEmailList)
        {
            this.parentEmailList = parentEmailList;
            this.email = email;

            this.subject.text =     "Subject : "    + email.subject;
            this.sender.text =      "From : "       + email.sender;
            this.received.text =    "Received : "   + email.sent;
            this.id = email.id;

            if (email.read)
            {
                this.subject.fontStyle = FontStyle.Bold;
            }
            else
            {
                this.subject.fontStyle = FontStyle.Normal;
            }
        }

        public void DeleteMail()
        {           
            parentEmailList.Remove(email); 
            Destroy(gameObject);
    }

    }
}