using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedDemon : MonoBehaviour {
    float duration = 100f;
    public GameObject lampLight;
    public GameObject bedDemon;
    public GameObject eyes;
    public GameObject Kid;
    public Light underbed;
    private float minWaitTime = .1f;
    private float maxWaitTime =-.5f;

    #region Singleton
    private static BedDemon instance = null;

    private BedDemon()
    {
    }

    public static BedDemon Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BedDemon();
            }
            return instance;
        }
    }

    #endregion

    // Use this for initialization
    void Start () {
//        startDelay();
        startGameLighting();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (underbed.range < 100)
        {
            increaseLight();
        }

        if( Time.deltaTime >= duration)
        {
            endGameLighting();    
        }
    }

    public void startGameLighting()
    {
        //lamp light goes off
        lampLight.SetActive(false);
        bedDemon.SetActive(true);
        underbed.range = 10;

    }

    public void lowHealthLighting(float _health)
    {
        flicker(underbed); 
    }

    IEnumerator flicker(Light _gLight)
    {
        //enable and disable rapidly 
        
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        _gLight.enabled = !_gLight.enabled;

    }

    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(10f);
    }

    IEnumerator increaseLight()
    {
        yield return new WaitForSeconds(2f);
        underbed.range++;
    }

    public void endGameLighting()
    {
        bedDemon.SetActive(false);
        lampLight.SetActive(true);     
    }
}
