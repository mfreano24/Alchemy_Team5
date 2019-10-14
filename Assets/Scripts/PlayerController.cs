using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float playerSpeed;
  Vector3 moveDirection;
  CharacterController cc;
  BoxCollider2D sw;
  int endlag = 0;
  float face_Front_x = 0.0f;
  float face_Front_y = 0.0f;
  KeyCode last_Pressed;
  //for reference purposes
  public GameObject player;
  public GameObject sword;
  //for instance purposes
  private GameObject sword_inst;


  void Awake(){
      cc = GetComponent<CharacterController>();
      face_Front_x = 0;
      face_Front_y = -1;
  }
  void Update () 
  {
      //Move First
      Move();
      //Then Basic Attack
      //Then Potion
  }
  void Move(){
    if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ){//NEED A CONDITION THAT WORKS BETTER HERE
      Assign_LastDirection();
    }
    if(endlag == 0){
      moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0.0f);
      moveDirection *= playerSpeed;
      cc.Move(moveDirection * Time.deltaTime);
      Attack();
        
      }

      else{
          endlag -= 1; //frame countdown
          if(endlag == 0){
              Destroy(sword_inst);
          }
      }
  }
/*ABILITIES */

  void Attack(){
    //ISSUE WITH SWORD FACINGS: there is a good 20-30 frame window where switching between A and D / W and S where the input.getaxis function returns 0 instead of +-1.
    //Possile remedies?
    if(Input.GetKeyDown(KeyCode.Mouse0)){
        sword_inst = Instantiate(sword);
        sw = sword_inst.GetComponent<BoxCollider2D>();
        sword_inst.transform.position = new Vector3(player.transform.position.x + 1.25f*face_Front_x, player.transform.position.y + 1.25f*face_Front_y, 0.0f);
        //rotation
        if(face_Front_x == -1){
          sword_inst.transform.eulerAngles = new Vector3(0,0,90 - face_Front_y * 45);
        }
        else if(face_Front_x == 0){
          sword_inst.transform.eulerAngles = new Vector3(0,0,90 - 90 * face_Front_y);
        }
        else if(face_Front_x == 1){
          sword_inst.transform.eulerAngles = new Vector3(0,0,-90 + face_Front_y * 45);
        }
        endlag+=60;//normally 15
    }
        
  }
  void usePotion(){
      //this will just contain the player action because potions become separate entities after dropping
      if(Input.GetKeyDown(KeyCode.Mouse1)){
        endlag+=3;
        //play animation
      }
      
  }

  /*MOVEMENT BASED FUNCTIONS */
  void Assign_LastDirection(){
    face_Front_x = Mathf.Ceil(Input.GetAxis("Horizontal"));
    face_Front_y = Mathf.Ceil(Input.GetAxis("Vertical")); 
  }
}
