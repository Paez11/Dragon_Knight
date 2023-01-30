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
    private bool doubleJump;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;

    //physics
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    
    //layers
    public LayerMask groundcap;

    //collisions
    private Vector2 moveInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffSet = 0.05f;
    public ContactFilter2D movementFilter;

    //Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start(){
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        //Camera.main.GetComponent<AudioSource>().PlayOneShot(landSound);
        moveSpeed = 6;
        JumpForce = 250;
        isJumping = false;
    }

    // Update is called once per frame
    void Update(){
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Movement translation
        if ( moveHorizontal < 0.0f){
            transform.localScale = new Vector3(-0.94f,0.79f,1.0f);
        }else if (moveHorizontal > 0.0f){
            transform.localScale = new Vector3(0.94f,0.79f,1.0f);
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded()){
            Jump();
            Camera.main.GetComponent<AudioSource>().PlayOneShot(jumpSound);
        }


        //Run sound
        if (!isGrounded() || this.GetComponent<Rigidbody2D>().velocity.x == 0){
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
        animator.SetBool("running",moveHorizontal != 0.0f);
    }

    private bool isGrounded(){
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, 
        new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, groundcap);    
        return raycastHit2D.collider != null;
    }

    private void Jump(){
        rb2D.AddForce(Vector2.up * JumpForce,ForceMode2D.Impulse);
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
