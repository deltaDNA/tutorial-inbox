using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaDNA; 

public class InboxTutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // Enter additional configuration here
        DDNA.Instance.ClientVersion = "v1.0.0";
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

        // Launch the SDK
        DDNA.Instance.StartSDK(
            "51623112369231521871823313815012",
            "https://collect11781ttrln.deltadna.net/collect/api",
            "https://engage11781ttrln.deltadna.net"
        );

        
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
