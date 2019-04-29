using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class FPPlayerLevelOne : MonoBehaviour
{
    /// <summary>
    /// ////////////////////////////////////////////////////////////START///////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    //Camera movement
    public float sensitivity = 7f;
    public Camera cam;
    float rotationY = 0f;
    float rotationX = 0f;

    //Player movements
    public float walkSpeed = 1000;
    Rigidbody rb;
    Vector3 moveDirection;


    //Jump
    float maxHeight = 0;

    //PlayerStatus
    public int totalHP = 100;
    int currentHP;
    public float force;

    //Display
    public Text heightDisplay;
    public Text hitDisplay;
    public Slider slider;
    public Text finishDisplay;
    int levelEND = 0;
    public Text chronoDisplay;

    //Chrono
    private float chrono;
    private float minute;

    //DamageZone
    float nextTick = 0f;
    public float tickRate = 1f;

    /// <summary>
    /// ////////////////////////////////////////////////////////////AWAKE///////////////////////////////////////////////////////////////
    /// </summary>
    void Awake()
    {
        chrono = 0f;
        minute = 0f;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        hitDisplay.text = " ";
        //startingP = transform.position;
        currentHP = totalHP;
        slider.value = currentHP;
        force = 2.5f;
        finishDisplay.enabled = false;
        transform.Find("MalusEffect").GetComponent<ParticleSystem>().Stop();
        transform.Find("BonusEffect").GetComponent<ParticleSystem>().Stop();
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
            maxHeight = 0;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Interaction();
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

        //Calcul de la hauteur du saut
        if (maxHeight < transform.position.y)
        {
            maxHeight = transform.position.y;
        }

        DisplayUI();

        //Level reset Out of bound
        if (transform.position.y < -50) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        chrono = (Time.timeSinceLevelLoad / 1 - Time.timeSinceLevelLoad % 1) - (minute * 60);
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
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);

        //rb.velocity += new Vector3(0, jumpSpeed * Time.deltaTime, 0);
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
        heightDisplay.text = maxHeight.ToString() + " m";
        if (slider.value > currentHP)   slider.value --;
        if (slider.value < currentHP)   slider.value ++;
        if (chrono == 59) minute++;
        if (chrono == -1) chrono = 0;
        chronoDisplay.text =minute + "'" + chrono + "''";
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////HEALTH INTERACTION///////////////////////////////////////////////////////////////
    /// </summary>
    void Interaction()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 7))
        {


            if (hit.transform.parent.transform.name == "Minus")
            {
                hitDisplay.text = hit.transform.tag;
                currentHP -= 25;
                //cam.transform.localPosition += new Vector3(0f, -0.2f, 0f);
                force += 2.5f;
                //Reset if you die
                if (currentHP <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            else if (hit.transform.parent.transform.name == "Plus")
            {
                if (currentHP < totalHP)
                {
                    hitDisplay.text = hit.transform.tag;
                    currentHP += 25;
                    //cam.transform.localPosition += new Vector3(0f, 0.2f, 0f);
                    force -= 2.5f;
                }
            }
            else hitDisplay.text = "Ground";
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////RELATION WITH ALLYY-ENEMY///////////////////////////////////////////////////////////////
    /// </summary>

    void OnTriggerStay(Collider theCollider)
    {
        if (theCollider.gameObject.transform.parent.transform.name == "Minus" && Time.time >= nextTick)
        {
            currentHP -= 10;
            //cam.transform.localPosition += new Vector3(0f, -0.2f, 0f);
            force += 2.5f;

            //Reset if you die
            if (currentHP <= 0)
            {
                //cam.transform.localPosition += new Vector3(0f, 1f, 0f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            nextTick = Time.time + 1 / tickRate;
            StatusNotif(transform.Find("MalusEffect").GetComponent<ParticleSystem>());
        }

        if (theCollider.gameObject.transform.parent.transform.name == "Plus" && Time.time >= nextTick)
        {
            if (currentHP < totalHP)
            {
                currentHP += 10;
                StatusNotif(transform.Find("BonusEffect").GetComponent<ParticleSystem>());
                //cam.transform.localPosition += new Vector3(0f, 0.2f, 0f);
                force -= 2.5f;
            }
            nextTick = Time.time + 1 / tickRate;

        }

        if (theCollider.gameObject.transform.parent.transform.name == "Finish")
        {
            finishDisplay.enabled = true;
            levelEND++;
            if (levelEND >= 90 && SceneManager.GetActiveScene().buildIndex == 2) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        /*if (theCollider.gameObject.transform.parent.transform.name == "Begin")
        {
            chrono = 0f;
        }*/
    }

    void OnTriggerExit(Collider theCollider)
    {
        transform.Find("MalusEffect").GetComponent<ParticleSystem>().Stop();
        transform.Find("BonusEffect").GetComponent<ParticleSystem>().Stop();

        /*if (theCollider.gameObject.transform.parent.transform.name == "Begin")
        {
            chrono = 0f;
        }*/
    }

    void StatusNotif(ParticleSystem status)
    {
        status.Play();
    }
}