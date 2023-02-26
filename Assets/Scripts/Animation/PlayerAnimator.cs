using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAnimator : MonoBehaviour
{
    public Animator MyDinoAnimator;
    public HealthManager MyDinosHealth;

    bool isFacingLeft;
    public SpriteRenderer spriteRenderer;

    public bool isActivePlayer;

    public enum Player{
        player1,
        player2
        }

    public Player currentDino;

    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = false;

        if(currentDino == Player.player1)
        {
            isActivePlayer = true;
        }
        else
        {
            isActivePlayer = false;
        }
        
        if (MyDinoAnimator == null)
        {
            MyDinoAnimator = GetComponent<Animator>();
        }

        if (MyDinosHealth == null)
        {
            MyDinosHealth = GetComponentInParent<HealthManager>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        CameraScript.OnPlayerSwitched += OnPlayerSwitched;
    }
    private void OnDisable()
    {
        CameraScript.OnPlayerSwitched -= OnPlayerSwitched;
    }
    void OnPlayerSwitched(bool isPlayerOne)
    {
      
        if(currentDino == Player.player1)
        {

            if (isPlayerOne)
            {
                isActivePlayer = true;
            }
            else
            {
                isActivePlayer = false;
                //MyDinoAnimator.StopPlayback();
                MyDinoAnimator.SetBool("idling", true);
                //MyDinoAnimator.Play("Idle");

            }
        }


            //switched to player 2 and is player 2

            else if(currentDino == Player.player2)
        {


            if (!isPlayerOne)
            {
                isActivePlayer = true;
            }
            else
            {
                isActivePlayer = false;
                //MyDinoAnimator.StopPlayback();
                MyDinoAnimator.SetBool("idling", true);
                //MyDinoAnimator.Play("Idle");

            }
        }
        else
        {

        }
        
    }
    private void Update()
    {
		if (Input.GetMouseButtonDown(0) && isActivePlayer)
        {
			MyDinoAnimator.SetBool("Attack", true);
			//MyDinoAnimator.StopPlayback();
			//MyDinoAnimator.Play("Attack");
			StartCoroutine(AttackDelay());
            //attacking, do not run the rest.
        }
        if (MyDinosHealth.isDead == true)
        {
            Debug.Log("dino has died");
            MyDinoAnimator.SetBool("isDead", true);
            //MyDinoAnimator.StopPlayback();
            //MyDinoAnimator.Play("Death");

        }
        else
        {
            if (isActivePlayer == false)
            {
                MyDinoAnimator.SetBool("idling", true);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //MyDinoAnimator.StopPlayback();
                    MyDinoAnimator.SetBool("Attack", true);
                }
                GetMouseLocation();
                if (isFacingLeft == true)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
       
       
    }
	IEnumerator AttackDelay(){
		yield return new WaitForSeconds(0.5f);
		MyDinoAnimator.SetBool("Attack", false);

	}
    void FixedUpdate()
    {
        if (MyDinosHealth.isDead == true)
        {
            Debug.Log("dino has died");
            MyDinoAnimator.SetBool("isDead", true);
            //MyDinoAnimator.StopPlayback();
            //MyDinoAnimator.Play("Death");

        }
        else
        {
            if (isActivePlayer)
            {
                float hor = Input.GetAxisRaw("Horizontal");//get left right movement input
                float ver = Input.GetAxisRaw("Vertical");//get up down movement input

                if (hor != 0 || ver != 0)//has horizontal input
                {
                    MyDinoAnimator.SetBool("idling", false);
                    //MyDinoAnimator.Play("Run_Right");
                }
                /*else if (ver != 0)
                {
                    MyDinoAnimator.SetBool("idling", false);
                    if (ver < 0)
                    {
                        MyDinoAnimator.Play("Run_Forward");
                    }
                    else
                    {
                        MyDinoAnimator.Play("Run_Back");
                    }
                }*/
                else
                {
                    MyDinoAnimator.SetBool("idling", true);
                }
            }
            

        }

        


    }

    public Vector3 direction;
    public float deg;
    void GetMouseLocation()
    {
        
        Vector3 mousePos = Input.mousePosition;


        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 worldPos = Vector3.zero;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            worldPos = hit.point;
        }

        direction = worldPos - transform.position;


        deg = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (deg < 0)
        {
            isFacingLeft = true;
        }
        else
        {
            isFacingLeft = false;
        }


    }
}
