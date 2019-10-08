using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float playerSpeed;
  Vector3 moveDirection;
  CharacterController cc;
  int endlag = 0;
  float face_Front_x = 0.0f;
  float face_Front_y = 0.0f;
  KeyCode last_Pressed;
  //BOOLEAN STATES
  bool roll = false;
  bool slash = false;
  bool drop = false;
  //for reference purposes
  public GameObject player;
  public GameObject sword;
  //for instance purposes
  private GameObject sword_inst;


  void Awake(){
      cc = GetComponent<CharacterController>();
      Cursor.visible = true;
  }
  void Update () 
  {
      //Move First
      Move();
      //Then Basic Attack
      //Then Potion
  }
  void Move(){
    Assign_LastDirection();
      if(endlag == 0){
        if(Input.GetKeyDown(KeyCode.R)){
            DodgeRoll();
        }
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0.0f);
        moveDirection *= playerSpeed;
        //Slight acceleration and decel at the beginning and end of movement so that its not good old jerky unity movement
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
  void DodgeRoll(){
      //IN TESTING DEBUG MODE RN
      //One iteration takes like 10^-11 seconds???
      for(int i = 0;i<35;i++){
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0.0f);
        moveDirection *= playerSpeed;
        cc.Move(moveDirection * Time.deltaTime);
      }
      
      endlag+=15;
  }

  void Attack(){
    if(Input.GetKeyDown(KeyCode.Mouse0)){
        sword_inst = Instantiate(sword);
        sword_inst.transform.position = new Vector3(player.transform.position.x + face_Front_x, player.transform.position.y + face_Front_y, 0.0f);
        //rotation
        sword_inst.transform.eulerAngles = new Vector3(0,0,0);
        endlag+=15;
    }
        
  }
  void usePotion(){
      //implement this

      endlag+=3;
  }

  /*MOVEMENT BASED FUNCTIONS */
  void Assign_LastDirection(){
    /*if(Input.GetKeyDown(KeyCode.W)){
        last_Pressed = KeyCode.W;
        face_Front_y = 1.5f;
        face_Front_x = 0f;
    }
    else if(Input.GetKeyDown(KeyCode.A)){
        last_Pressed = KeyCode.A;
        face_Front_y = 0f;
        face_Front_x = -1f;
        
    }
    else if(Input.GetKeyDown(KeyCode.S)){
        last_Pressed = KeyCode.S;
        face_Front_y = -1.5f;
        face_Front_x = 0f;
    }
    else if(Input.GetKeyDown(KeyCode.D)){
        last_Pressed = KeyCode.D;
        face_Front_y = 0f;
        face_Front_x = 1f; 
    }*/
    face_Front_x = Input.GetAxis("Horizontal");
    face_Front_y = 1.5f * Input.GetAxis("Vertical"); 
  }
}
