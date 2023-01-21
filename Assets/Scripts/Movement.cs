using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float Speed = 6;
    public float JumpForce = 250;
    private bool Grounded;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private Animator Animator;

    // Start is called before the first frame update
    void Start(){
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update(){
        Horizontal = Input.GetAxisRaw("Horizontal");

        if ( Horizontal < 0.0f){
            transform.localScale = new Vector3(-0.94f,0.79f,1.0f);
        }else if (Horizontal > 0.0f){
            transform.localScale = new Vector3(0.94f,0.79f,1.0f);
        }

        Animator.SetBool("running",Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);
        if(Physics2D.Raycast(transform.position, Vector3.down, 1f)){
            Grounded = true;
        }else{
            Grounded = false;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && Grounded){
            Jump();
        }
    }

    private void FixedUpdate() {
        Rigidbody2D.velocity = new Vector2(Horizontal* Speed, Rigidbody2D.velocity.y);
    }

    private void Jump(){
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }
}
