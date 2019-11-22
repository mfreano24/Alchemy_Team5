using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxInstance : MonoBehaviour
{
    Animator anim;
    string potionName;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(potionName + "_Hitbox");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSprite(string potion){
        potionName = potion;
        Start();
    }


}
