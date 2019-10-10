using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    int health;
    public GameObject sword;
    BoxCollider2D hurtbox;
    void Start()
    {
        health = 50;
        hurtbox = GetComponent<BoxCollider2D>();
    }
    void Update(){
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player_Weapon")){
            health-=5;
            Debug.Log("Hit- Dummy Health is now "+health);
        }
    }
}
