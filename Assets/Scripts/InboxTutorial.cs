using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaDNA;
using JSONObject = System.Collections.Generic.Dictionary<string, object>;

namespace DDNAInboxTutorial
{
    public class InboxTutorial : MonoBehaviour
    {
        private EmailList myEmailList = null; 

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
                DDNA.Instance.StopSDK();
                DDNA.Instance.ClearPersistentData();
                this.Start();
            }
            
        }





        // Use Engage to download targeted emails.
        public void SimpleCheckMail()
        {
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
            Debug.Log("Email Received");

            Email newEmail = new Email(engageResponse);

            if (newEmail.id != null && newEmail.message != null)
            {
                myEmailList.Add(newEmail);
            }
           
            SimpleCheckMail();  // Look for another mail.            
        }
        
    }
}