﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRange : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 14.5f;

    Collider2D[] hitEnemies;

    GameObject thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.Find("Player");
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    }

    // Update is called once per frame
    void Update()
    {
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemies.Length > 0)
        {
            this.GetComponentInParent<Enemies>().animator.SetBool("Chase", true);
        }
        else
        {
            this.GetComponentInParent<Enemies>().animator.SetBool("Chase", false);
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        //Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
