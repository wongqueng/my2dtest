using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : ThinkingPlaceable
{

    private Animator m_animator;
    private float lastBlowTime = -1000f;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    protected override void Die()
    {
        base.Die();
        m_animator.SetTrigger("Dead");

    }
    public  override void StartAttack()
    {
        base.StartAttack();
        if (Time.time >= lastBlowTime + attackRatio) {
            lastBlowTime = Time.time;
            m_animator.SetTrigger("Attack");
        }
    }
    public override  void Stop()
    {
        base.Stop();
        m_animator.SetTrigger("Idel");
    }

    public override void GetHit() {
        if (state != States.Dead) {
            m_animator.SetTrigger("Hit");
        }
    }



    private void OnCollisionEnter2D(Collision2D other) {
    }

    private void OnTriggerEnter2D(Collider2D other) {
    }

    private void OnTriggerStay2D(Collider2D other) {
        // Debug.Log("ArcherTriggerStay");
    }

    void OnTriggerExit2D(Collider2D other) {
    }
}
