using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    
    Animator myAnimator;
    public GameObject myEnemy;
    public UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update

    private void OnEnable()
    {
        
    }

    public void SetIsDead(bool newIsDead)
    {
        isDead = newIsDead;
        myAnimator.SetBool("isDead", newIsDead);
        StartCoroutine("ResetAfterDeath");
    }

    public void Attack()
    {
        myAnimator.SetBool("isAttacking", true);
        StartCoroutine("ResetAttack");
    }

    public bool GetIsPlayingDeathAnimation()
    {
        return myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Die");
    }
    IEnumerator ResetAfterDeath()
    {
        yield return new WaitForEndOfFrame();
        myAnimator.SetBool("isDead", false);
        isDead = false;
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForEndOfFrame();
        myAnimator.SetBool("isAttacking", false);
    }

    float myRotation_X;
    float myRotation_y;
    float myRotation_z;
    void Start()
    {
        isAttacking = false;
        isDead = false;
        myAnimator = GetComponent<Animator>();

        if(myEnemy == null)
        {
            myEnemy = transform.parent.gameObject;
        }
        myRotation_X = transform.eulerAngles.x;
        myRotation_y = transform.eulerAngles.y;
        myRotation_z = transform.eulerAngles.z;

    }
    void AdjustRotation()
    {
        myRotation_X = myEnemy.transform.rotation.x - 90f;
        transform.eulerAngles = new Vector3(90, 0, 0);
    }
    bool isAttacking;
    bool isDead;
    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
       CheckNavAgentDirection();
    }

    public Vector3 cross;

    public enum MoveDirection
    {
        up, down, left, right
    }

    void CheckNavAgentDirection()
    {
        if(myAnimator.GetBool("IsDead")== true)
        {
            myAnimator.Play("Enemy_Die");
        }
        //ignore if it is attacking
        else if(isAttacking == false && isDead == false)
        {
            // Get the NavMeshAgent component attached to the object
           
            Vector3 velocity = agent.velocity;

            Vector3 forward = transform.forward;

            cross = Vector3.Cross(velocity, forward);

            if (cross.x > 0.3 || cross.x < -0.3)
            {
                myAnimator.SetBool("prioritizeUpDown", true);
                myAnimator.SetFloat("Vertical", cross.x);
            }
            else 
            {
                myAnimator.SetBool("prioritizeUpDown", false);
                if (cross.y > 0)
                {
                    myAnimator.SetBool("facingRight", true);
                    //facing right
                }
                else
                {
                    myAnimator.SetBool("facingRight", false);
                    //facing left.
                }
               
               // myAnimator.SetFloat("Horizontal", cross.y);
            }
        }
    }


}
