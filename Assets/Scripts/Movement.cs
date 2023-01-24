using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //Audio
    public AudioClip jumpSound;
    public AudioClip landSound;

    //Movement
    public float moveSpeed;
    public float JumpForce;
    private bool grounded;
    private float moveHorizontal;
    private float moveVertical;

    //physics
    private Rigidbody2D rb2D;

    //collisions
    private Vector2 moveInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffSet = 0.05f;
    public ContactFilter2D movementFilter;

    //Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start(){
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Camera.main.GetComponent<AudioSource>().PlayOneShot(landSound);
        moveSpeed = 6;
        JumpForce = 250;
        grounded = false;
    }

    // Update is called once per frame
    void Update(){
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");


        //Movement translation
        if ( moveHorizontal < 0.1f){
            transform.localScale = new Vector3(-0.94f,0.79f,1.0f);
        }else if (moveHorizontal > 0.1f){
            transform.localScale = new Vector3(0.94f,0.79f,1.0f);
        }
        animator.SetBool("running",moveHorizontal != 0.0f);


        //Jump
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
        if(Physics2D.Raycast(transform.position, Vector3.down, 1f)){
            grounded = true;
            animator.SetBool("jumping",false);
        }else{
            grounded = false;
            animator.SetBool("jumping",true);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && grounded){
            Jump();
            Camera.main.GetComponent<AudioSource>().PlayOneShot(jumpSound);
        }

        //Run sound
        if (!grounded || this.GetComponent<Rigidbody2D>().velocity.x == 0){
            StopRunSound();
        }else{
            this.GetComponent<AudioSource>().pitch = Mathf.Clamp(this.GetComponent<Rigidbody2D>().velocity.magnitude,1f,1.8f);
            //this.GetComponent<AudioSource>().panStereo = Mathf.Clamp(this.GetComponent<Transform>().position.x/10,-1,1);
            PlayRunSound();
        }
    }

    // Same as update but sincronize with the frames that can be lost
    private void FixedUpdate() {
        rb2D.velocity = new Vector2(moveHorizontal * moveSpeed, rb2D.velocity.y);
        /*
        bool success = MovePlayer(moveInput);

        if (!success){
            success = MovePlayer(new Vector2(moveInput.x, 0));
        }
        */
    }

    private void Jump(){
        rb2D.AddForce(Vector2.up * JumpForce);
    }

    public bool MovePlayer(Vector2 direction){
        
        //Check for potential collitions
        int count = rb2D.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffSet);

        if (count == 0){
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            //No collisions
            rb2D.MovePosition(rb2D.position + moveVector);
            return true;
        }else{
            //Print collisions
            foreach (RaycastHit2D hit in castCollisions){
                print(hit.ToString());
            }
            return false;
        }
    }


    //Sounds
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
