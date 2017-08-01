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

        public GameObject RichMailPanel;

        private void Awake()
        {
            SimpleMailPanelHide();
            SimpleMailPanelClear();
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
                this.Start();
            }
        }




        // Use Engage to download targeted emails.
        public void SimpleCheckMail()
        {
            SimpleMailPanelShow();

             // Iterate until Engage returns empty response
             Engagement simpleMailCheck = new Engagement("simpleMailCheck");

                object message ;
                DDNA.Instance.RequestEngagement(simpleMailCheck, (Dictionary<string, object> response) => {
                    if (response.TryGetValue("message", out message))
                        SimpleEmailReceived(response);
                    else
                        Debug.Log("No More Mail");
                });   
  
        }



        private void SimpleEmailReceived(Dictionary<string, object> engageResponse)
        {           
            Email newEmail = new Email(engageResponse);

            if (newEmail.id != null && newEmail.message != null)
            {
                myEmailList.Add(newEmail);                
            }
            
            Debug.Log("Email Received : (" + myEmailList.Count() + ")");
            SimpleCheckMail();  // Look for another mail.            
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
       
        public void SimpleMailPanelPopulate()
        {
            int counter = 0; 
            foreach(Email e in myEmailList.Emails)
            {
                AddSimpleMailDisaplyObject(e, counter);
                counter++;
            }
        }

    }
}