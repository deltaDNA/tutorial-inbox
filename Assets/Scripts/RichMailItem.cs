using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeltaDNA;

namespace DDNAInboxTutorial
{
    public class RichMailItem : MonoBehaviour
    {

        //public UnityEngine.UI.Text subject;
        public UnityEngine.UI.Text sender;
        public UnityEngine.UI.Text received;
        private bool isRead;
        private string id;

        private EmailList parentEmailList;
        private Email email;
        //private DeltaDNA.ImageMessage imageMessage;
        public GameObject imageObject;              // Reference to the ImageObject prefab
        // Use this for initialization
        void Start()
        {

        }

        public void SetMailDetails(Email email, EmailList parentEmailList)
        {
            this.parentEmailList = parentEmailList;
            this.email = email;

            this.sender.text = "From : " + email.sender;
            this.received.text = "Received : " + email.sent;
            this.id = email.id;

            //this.email.imageMessage = imageMessage;

            // Check we got an engagement with a valid image message.
            if (this.email.imageMessage != null)
            {

                Debug.Log("Look, an imageMessage");

                //GameObject emailImage = (GameObject)Instantiate(imageObject, gameObject.transform.position, Random.rotation);
                //emailImage.transform.SetParent(transform.parent);
                //emailImage.url = this.email.url;
            }
        }
        public void DeleteMail()
        {
            parentEmailList.Remove(email);
            Destroy(gameObject);
        }

        public void ViewMail()
        {
            //InboxTutorial it = GameObject.FindObjectOfType<InboxTutorial>();
            //it.RichMailShow(this.email);
            if (this.email.imageMessage != null)
            {
                

                this.email.imageMessage.Show();
            }
        }

    }
}