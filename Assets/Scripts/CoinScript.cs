using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour {

    private float coinAge = 0f;             // Age of coin based on when it landed

    public float decayDuration;             // Minimum number of seconds before coin decays
    public float decayRandomiser;           // Randomiser for coin decay duration


    private void Start()
    {
        Rigidbody r = gameObject.GetComponent<Rigidbody>();
        r.mass = Random.Range(40.0f, 100.0f);
        r.drag = Random.Range(0.2f, 1.0f);
        r.angularDrag = Random.Range(0.7f, 10.2f);
        r.transform.parent = transform;
    }
    void Update () {

        // Update coin age
        coinAge += Time.deltaTime; 
        if (coinAge > decayDuration + decayRandomiser)
        {
            // Destroy coin if coin has decayed
            DestroyCoin();
        }
        
	}
   


    void DestroyCoin()
    {
        // Remove coin from scene and tell demo script so the HUD can be updated.     
        Destroy(gameObject);
    }
}
