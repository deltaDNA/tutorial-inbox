using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeltaDNA;
using JSONObject = System.Collections.Generic.Dictionary<string, object>;

namespace DDNAInboxTutorial
{
    public class InboxTutorial : MonoBehaviour
    {
        private static EmailList myEmailList = null;

        public GameObject SimpleMailPanel;
        public GameObject SimpleMailItemPrefab;
        public GameObject SimpleMail;

        public GameObject RichMailPanel;
        public GameObject RichMailItemPrefab;
        public GameObject RichMail;

        public Rigidbody coin;              // Reference to the Coin prefab


        private void Awake()
        {
            SimpleMailPanelHide();
            SimpleMailPanelClear();
            SimpleMail.GetComponent<SimpleMail>().HideMail();

            RichMailPanelHide();
            RichMailPanelClear();   //Todo
            RichMail.GetComponent<SimpleMail>().HideMail(); // Todo

        }

        // Use this for initialization
        void Start()
        {

            // Enter additional configuration here
            DDNA.Instance.ClientVersion = "v1.0.0";
            DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

            // Launch the SDK
            DDNA.Instance.StartSDK(
                "51623112369231521871823313815012",
                "https://collect11781ttrln.deltadna.net/collect/api",
                "https://engage11781ttrln.deltadna.net"
            );

            myEmailList = new EmailList();

        }




        public void ResetUserID()
        {
            if (DDNA.Instance.isActiveAndEnabled && !DDNA.Instance.IsUploading)
            {
                DDNA.Instance.ClearPersistentData();
                myEmailList.Clear();
                SimpleMailPanelClear();
                RichMailPanelClear();
                this.Start();
            }
        }




        // Use Engage to download targeted emails.
        public void SimpleCheckMail()
        {
            SimpleMailPanelShow();

            // Iterate until Engage returns empty response
            Engagement simpleMailCheck = new Engagement("simpleMailCheck");

            object message;
            DDNA.Instance.RequestEngagement(simpleMailCheck, (response) =>
            {
                if (response.TryGetValue("message", out message))
                    SimpleEmailReceived(response);
                else
                    Debug.Log("No More Mail");
            });

        }

        public void RichCheckMail()
        {
            RichMailPanelShow();

            // Iterate until Engage returns empty response
            Engagement richMailCheck = new Engagement("richMailCheck");


            DDNA.Instance.RequestEngagement(richMailCheck, (response) => {
                ImageMessage imageMessage = ImageMessage.Create(response);
                   
                if (imageMessage != null)
                {
                    

                    // This example will show the image as soon as the background
                    // and button images have been downloaded.
                    imageMessage.OnDidReceiveResources += () => {
                        Debug.Log("Image Message loaded resources.");
                        //imageMessage.Show();
                    };

                    // Add a handler for the 'dismiss' action.
                    imageMessage.OnDismiss += (ImageMessage.EventArgs obj) => {
                        Debug.Log("Image Message dismissed by " + obj.ID);
                       
                    };

                    // Add a handler for the 'action' action.
                    imageMessage.OnAction += (ImageMessage.EventArgs obj) => {
                        Debug.Log("Image Message actioned by " + obj.ID + " with command " + obj.ActionValue);
                        if (obj.ActionValue == "GIFT") DropCoins(100);
                    };

                    imageMessage.FetchResources();
                    RichEmailReceived(response.JSON, imageMessage);
                }
                else
                { 
                    Debug.Log("No More Mail");
                }
            }, (exception) => {
                Debug.Log("Engage reported an error: " + exception.Message);
            });       
        }



        private void SimpleEmailReceived(JSONObject engageResponse)
        {
            Email newEmail = new Email(engageResponse);

            if (newEmail.id != null)
            {
                myEmailList.Add(newEmail);
                Debug.Log("Email Received : (" + myEmailList.Count() + ")");
            }
            
            // Look for another mail.   
            SimpleCheckMail();
        }
        private void RichEmailReceived(JSONObject engageResponse, ImageMessage imageMessage)
        {
            Email newEmail = new Email(engageResponse, imageMessage);

            if (newEmail.id != null)
            {
                myEmailList.Add(newEmail);
                Debug.Log("Email Received : (" + myEmailList.Count() + ")");
            }
            RichCheckMail();
        }



        // Simple Mail Display Panel 
        // --------------------------

        public void SimpleMailPanelHide()
        {
            SimpleMailPanel.SetActive(false);
            SimpleMailPanelClear();
        }
        public void SimpleMailPanelShow()
        {
            SimpleMailPanelPopulate();
            SimpleMailPanel.SetActive(true);
        }
        public void SimpleMailPanelPopulate()
        {
            int counter = 0;
            foreach (Email e in myEmailList.Emails)
            {
                if (e.emailType == "SIMPLE")
                {
                    AddSimpleMailDisaplyObject(e, counter);
                    counter++;
                }
            }
        }
        public void AddSimpleMailDisaplyObject(Email newEmail, int counter)
        {
            GameObject newMailItem = (GameObject)Instantiate(SimpleMailItemPrefab, transform.position, transform.rotation);
            newMailItem.transform.SetParent(SimpleMailPanel.transform);

            newMailItem.transform.localPosition = new Vector3(164.0f, -27.0f - (counter * 45.0f), 0);
            newMailItem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            SimpleMailItem m = newMailItem.GetComponent<SimpleMailItem>();
            m.SetMailDetails(newEmail, myEmailList);
        }

        public void SimpleMailPanelClear()
        {
            List<GameObject> mailPanels = new List<GameObject>();
            foreach (Transform child in SimpleMailPanel.transform)
            {
                if (child.name.StartsWith("Simple Mail Item Panel"))
                {
                    Destroy(child.gameObject);
                }
            }
        }








        public void SimpleMailShow(Email email)
        {
            SimpleMail.GetComponent<SimpleMail>().ShowMail(email);
        }


        // Simple Mail Display Panel 
        // --------------------------

        public void RichMailPanelHide()
        {
            RichMailPanel.SetActive(false);
            RichMailPanelClear();
        }
        public void RichMailPanelShow()
        {
            RichMailPanelPopulate();
            RichMailPanel.SetActive(true);
        }
        public void RichMailPanelPopulate()
        {
            int counter = 0;
            foreach (Email e in myEmailList.Emails)
            {
                if (e.emailType == "RICH")
                {
                    AddRichMailDisaplyObject(e, counter);
                    counter++;
                }
            }
        }
        public void AddRichMailDisaplyObject(Email newEmail, int counter)
        {//Todo
            GameObject newMailItem = (GameObject)Instantiate(RichMailItemPrefab, transform.position, transform.rotation);
            newMailItem.transform.SetParent(RichMailPanel.transform);

            newMailItem.transform.localPosition = new Vector3(-45.0f, -5.0f - (counter * 45.0f), 0);
            newMailItem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            RichMailItem m = newMailItem.GetComponent<RichMailItem>();
            m.SetMailDetails(newEmail, myEmailList);
        }

        public void RichMailPanelClear()
        { //Todo
            List<GameObject> mailPanels = new List<GameObject>();
            foreach (Transform child in RichMailPanel.transform)
            {
                if (child.name.StartsWith("Rich Mail Item Panel"))
                {
                    Destroy(child.gameObject);
                }
            }
        }



        public void RichMailShow(Email email)
        {
            SimpleMail.GetComponent<SimpleMail>().ShowMail(email);
        }



        


        public void SimpleMailAction(Email email)
        {

            if (email.action != null 
                && email.value != null 
                && System.Convert.ToDateTime(email.expiryTimestamp) > System.DateTime.UtcNow)
            {
                switch (email.action)
                {
                    case "GIFT":
                        if (email.amount > 0)
                        {
                            Debug.Log(string.Format("Gifting Player {0} {1}", email.amount, email.value));
                            DropCoins(email.amount);
                        }
                        break;
                    case "DEEPLINK":
                        if (email.value != null)
                        {
                            Debug.Log("Navigating to " + email.value);
                            Application.OpenURL(email.value);
                        }
                        break;
                }
            }
        }



        void DropCoins(int numberOfCoins)
        {
            // Drop new coins into scene from above camera
            for (int loop = 0; loop < numberOfCoins; loop++)
            {
                Vector3 coinStartingPosition = new Vector3(Random.Range(-8.0f,8.0f), Random.Range(6.0f, 25.0f), Random.Range(-5.0f, 5.0f));
                Rigidbody coinClone = (Rigidbody)Instantiate(coin, coinStartingPosition, Random.rotation);

            }
        }

    }
}