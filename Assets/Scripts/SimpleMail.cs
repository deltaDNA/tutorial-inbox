using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DDNAInboxTutorial
{
    public class SimpleMail : MonoBehaviour
    {

        public UnityEngine.UI.Text subject;
        public UnityEngine.UI.Text message;
        public UnityEngine.UI.Text sender;
        public UnityEngine.UI.Text received;
        public UnityEngine.UI.Text expiry;
        public UnityEngine.UI.Button actionButton;
        private Email email; 




        public void HideMail()
        {
            gameObject.SetActive(false);
        }
        public void ShowMail(Email email)
        {
            this.email = email;
            subject.text = this.email.subject;
            message.text = this.email.message;
            sender.text = this.email.sender;
            received.text = this.email.sent;
            expiry.text = this.email.expiryTimestamp;
            if (!string.IsNullOrEmpty(this.email.label))
            {
                actionButton.GetComponentInChildren<Text>().text = this.email.label;
                actionButton.gameObject.SetActive(true);
            }
            else
            {
               actionButton.gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }

        public void MailActionButtonClick()
        {
            Debug.Log("Email Action Button Clicked");
            InboxTutorial it = GameObject.FindObjectOfType<InboxTutorial>();
            it.SimpleMailAction(email);
        }


    }
}