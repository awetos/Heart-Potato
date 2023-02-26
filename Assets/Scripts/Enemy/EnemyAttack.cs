using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public delegate void attackPlayer(int damage);
    public static event attackPlayer OnAttackPlayer;
    
    

  [SerializeField]  bool isAttacking;
    [SerializeField] float attackSpeed;
    [SerializeField] public int damageAmount;

    private void Start()
    {
        isAttacking = false;
        if(attackSpeed == 0)
        {
            attackSpeed = 0.5f;
        }
        if(damageAmount == 0)
        {
            damageAmount = 10;
        }

        if(myAnimator == false)
        {
            myAnimator = GetComponentInChildren<Animator>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {

            isAttacking = true;
            StartCoroutine("Attacking");
        }
    }

    public Animator myAnimator;
    IEnumerator Attacking()
    {
        while (isAttacking == true)
        {
            Camera.main.GetComponent<HealthReferences>().TakeDamage(damageAmount);
            OnAttackPlayer(damageAmount);
            myAnimator.Play("Enemy_Attack");
            myAnimator.SetBool("IsAttacking",true);
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    private void OnCollisionExit()
    {
        isAttacking = false;
    }
}
