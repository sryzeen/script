using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    /// <summary>
    /// ////////////////////////////////////////////////////////////START///////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    private Vector3 startingP;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Camera movement
    public float sensitivity = 1f;
    public Camera cam;

    float rotationY = 0f;
    float rotationX = 0f;

    //Player movements
    public float walkSpeed = 1000;
    Rigidbody rb;
    Vector3 moveDirection;


    //Jump
    public float jumpSpeed = 600f;

    //Gun
    public float damageGun = 10f;
    public float rangeGun = 50f;
    private Animation animated;

    //Rifle
    public float damageRifle = 15f;
    public float rangeRifle = 100f;

    //Weapons
    public GameObject hand;
    public GameObject gun;
    public GameObject rifle;


    //Ammo
    private int ammoLoaded;
    public int magazine = 7;
    public int magazineStock = 14;
    public int magazineMax = 70;

    //Score
    private int score;


    //Display
    public Text magDisplay;
    public Text scoreDisplay;
    public Text hitDisplay;
    public Slider slider;

    /// <summary>
    /// ////////////////////////////////////////////////////////////AWAKE///////////////////////////////////////////////////////////////
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        animated = GetComponent<Animation>();
        ammoLoaded = magazine;
        score = 0;
        startingP = transform.position;
        hitDisplay.text = " ";
        slider.value = 0;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////FIXEDUPDATE///////////////////////////////////////////////////////////////
    /// </summary>
    void FixedUpdate()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded()) Jump();
        }

        if (Input.GetButton("Action"))
        {
            animated.Play("Interact");
        }

        if (transform.GetChild(0).GetChild(0).tag == "SoloHand")
        {
            //Firing
            if (Input.GetButtonDown("Fire1") && ammoLoaded > 0)
            {
                animated.Play("Shooting");

                if (Input.GetMouseButton(1))
                {
                    animated.Play("AimedShoot");
                }
            }
            //Reloading
            if (Input.GetButton("Reload") && ammoLoaded < magazine && magazineStock != 0)
            {
                if (Input.GetMouseButton(1))
                {
                    animated.Play("AimedReload");
                }
                else
                {
                    animated.Play("Reloading");
                }

            }
            //Supply
            if (Input.GetButton("Action"))
            {
                animated.Play("Interact");
            }
            //Aiming
            if (Input.GetMouseButton(1))
            {
                animated.CrossFade("Aimed");
                sensitivity = 3f;
                walkSpeed = 600;
            }
            else
            {
                animated.CrossFade("Idle");
                sensitivity = 7f;
                walkSpeed = 1000;
            }
        }

        if (transform.GetChild(0).GetChild(0).tag == "TwoHand")
        {
            //Firing
            if (Input.GetButtonDown("Fire1") && ammoLoaded > 0)
            {
                animated.Play("ShootingRifle");

                /*if (Input.GetMouseButton(1))
                {
                    animated.Play("AimedShoot");
                }*/
            }
            //Reloading
            /*if (Input.GetButton("Reload") && ammoLoaded < magazine && magazineStock != 0)
            {
                if (Input.GetMouseButton(1))
                {
                    animated.Play("AimedReload");
                }
                else
                {
                    animated.Play("Reloading");
                }

            }*/
            //Supply
            /*if (Input.GetButton("Action"))
            {
                animated.Play("Interact");
            }
            //Aiming
            if (Input.GetMouseButton(1))
            {
                animated.CrossFade("Aimed");
                sensitivity = 3f;
                walkSpeed = 600;
            }*/
            else
            {
                animated.CrossFade("Idle");
                sensitivity = 7f;
                walkSpeed = 1000;
            }
        }


    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////AMMO////////////////////////////////////////////////////////////////
    /// </summary>

    void ReloadMag()
    {
        if (magazineStock > 0)
        {
            magazineStock -= magazine - ammoLoaded;
            ammoLoaded = magazine;
            if (magazineStock < 0) magazineStock = 0;
            //Replace the whole Mag
            //magazineStock -= magazine;
        }
    }
    void AmmoUsed()
    {
          ammoLoaded--;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////AMMODISPENSER//////////////////////////////////////////////////////////
    /// </summary>
    void Supply()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2))
        {
            
            hitDisplay.text = hit.transform.name;

            if (hit.transform.tag == "SoloHand")
            {
                ///heee
            }

            if (hit.transform.name == "Ammo")
            {
                if (magazineStock <= magazineMax) magazineStock += magazine;
                if (magazineStock > magazineMax) magazineStock = magazineMax;
            }
        }
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////UPDATE///////////////////////////////////////////////////////////////
    /// </summary>
    void Update()
    {

        float horizontalMov = Input.GetAxisRaw("Horizontal");
        float verticalMov = Input.GetAxisRaw("Vertical");

        rotationX += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY += Input.GetAxis("Mouse X") * sensitivity;

        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

        if (CanMove(transform.right * horizontalMov))
        {
 
        }
        else horizontalMov = 0;

        if (CanMove(transform.forward * verticalMov))
        {
            
        }
        else verticalMov = 0;

        moveDirection = (horizontalMov * transform.right + verticalMov * transform.forward).normalized;

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }



        DisplayUI();
        
        //Level reset Out of bound
        if (transform.position.y < -100) transform.position = startingP;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////MOVE///////////////////////////////////////////////////////////////
    /// </summary>
    void Move()
    {
        Vector3 yVelFix = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDirection * walkSpeed * Time.deltaTime;
        rb.velocity += yVelFix;
    }

    CapsuleCollider col;

    //On the ground
    bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.6f)) return true;
        else return false;
    }


    //Jump
    void Jump()
    {
        rb.velocity += new Vector3(0, jumpSpeed * Time.deltaTime, 0);
    }

    //Wall perception
    bool CanMove(Vector3 direction)
    {
        float distantToPoints = col.height / 2 - col.radius;

        Vector3 point1 = transform.position + col.center + Vector3.up * distantToPoints;
        Vector3 point2 = transform.position + col.center - Vector3.up * distantToPoints;

        float radius = col.radius * 0.95f;
        float castDistance = 0.5f;

        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, direction, castDistance);

        foreach (RaycastHit objectHit in hits)
        {
            if (objectHit.transform.tag == "Wall")
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////UI DISPLAY///////////////////////////////////////////////////////////////
    /// </summary>

    void DisplayUI()
    {
        if (transform.GetChild(0).GetChild(0).tag == "SoloHand")
        {
            scoreDisplay.text = "Killed : " + score.ToString();
            magDisplay.text = ammoLoaded + " / " + magazineStock;
        }
    }
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////GUN///////////////////////////////////////////////////////////////
    /// </summary>

    void ShootGun()
    {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rangeGun))
        {      
            hitDisplay.text = hit.transform.name;

            Target target = hit.transform.GetComponent<Target>();

            if (target != null && hit.transform.tag == "Enemy")
            {
                slider.value = target.health;

                target.Damaged(damageGun);

                slider.value = target.HealthRatio();

                if (target.EnemyState())
                {
                    score++;
                    slider.value = 0;
                }
            }
            else slider.value = 0;
        }
    }

    void ShootRifle()
    {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rangeRifle))
        {
            hitDisplay.text = hit.transform.name;

            Target target = hit.transform.GetComponent<Target>();

            if (target != null && hit.transform.tag == "Enemy")
            {
                slider.value = target.health;

                target.Damaged(damageRifle);

                slider.value = target.HealthRatio();

                if (target.EnemyState())
                {
                    score++;
                    slider.value = 0;
                }
            }
            else slider.value = 0;
        }
    }
}
