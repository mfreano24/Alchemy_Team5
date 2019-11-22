using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameObject Wave1;
    public GameObject Wave2;
    public GameObject Wave3;
    public GameObject Wave4;
    public GameObject Wave5;
    public GameObject Wave6;
    public GameObject em;
    WaveManager wv;
    void Start()
    {
        wv = em.GetComponent<WaveManager>();
        Wave1.GetComponent<AudioSource>().volume=1;
    }

    void Update() {
        if(wv.curr_wave == 2){
            Wave1.GetComponent<AudioSource>().volume = 0;
            Wave2.GetComponent<AudioSource>().volume = 1;
        }
        if(wv.curr_wave == 3){
            Wave2.GetComponent<AudioSource>().volume = 0;
            Wave3.GetComponent<AudioSource>().volume = 1;
        }
        if(wv.curr_wave == 4){
            Wave3.GetComponent<AudioSource>().volume = 0;
            Wave4.GetComponent<AudioSource>().volume = 1;
        }
        if(wv.curr_wave == 5){
            Wave4.GetComponent<AudioSource>().volume = 0;
            Wave5.GetComponent<AudioSource>().volume = 1;
        }
        if(wv.curr_wave == 6){
            Wave5.GetComponent<AudioSource>().volume = 0;
            Wave6.GetComponent<AudioSource>().volume = 1;
        }

        
    }
}
