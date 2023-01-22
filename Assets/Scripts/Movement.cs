using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public AudioClip jumpSound;
    public AudioClip landSound;
    public float Speed = 6;
    public float JumpForce = 250;
    private bool Grounded;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float Vertical;
    private Animator Animator;

    // Start is called before the first frame update
    void Start(){
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        //Camera.main.GetComponent<AudioSource>().PlayOneShot(landSound);
    }

    // Update is called once per frame
    void Update(){
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");


        //Movement translation
        if ( Horizontal < 0.0f){
            transform.localScale = new Vector3(-0.94f,0.79f,1.0f);
        }else if (Horizontal > 0.0f){
            transform.localScale = new Vector3(0.94f,0.79f,1.0f);
        }

        Animator.SetBool("running",Horizontal != 0.0f);


        //Jump
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
        if(Physics2D.Raycast(transform.position, Vector3.down, 1f)){
            Grounded = true;
            Animator.SetBool("jumping",false);
        }else{
            Grounded = false;
            Animator.SetBool("jumping",true);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && Grounded){
            Jump();
            Camera.main.GetComponent<AudioSource>().PlayOneShot(jumpSound);
        }

        //Run sound
        if (!Grounded || this.GetComponent<Rigidbody2D>().velocity.x == 0){
            StopRunSound();
        }else{
            this.GetComponent<AudioSource>().pitch = Mathf.Clamp(this.GetComponent<Rigidbody2D>().velocity.magnitude,1f,1.8f);
            //this.GetComponent<AudioSource>().panStereo = Mathf.Clamp(this.GetComponent<Transform>().position.x/10,-1,1);
            PlayRunSound();
        }
    }

    private void FixedUpdate() {
        Rigidbody2D.velocity = new Vector2(Horizontal* Speed, Rigidbody2D.velocity.y);
    }

    private void Jump(){
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void StopRunSound(){
        if (this.GetComponent<AudioSource>().isPlaying){
            this.GetComponent<AudioSource>().Stop();
        }
    }

    private void PlayRunSound(){
        if (!this.GetComponent<AudioSource>().isPlaying){
            this.GetComponent<AudioSource>().Play();
        }
    }
}
