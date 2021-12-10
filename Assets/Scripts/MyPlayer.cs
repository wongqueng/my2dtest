using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityRoyale;


namespace MyGame {
    public class MyPlayer : ThinkingPlaceable {
        [SerializeField] float m_speed = 4.0f;
        [SerializeField] float m_jumpForce = 7.5f;
        [SerializeField] float m_rollForce = 6.0f;
        [SerializeField] bool m_noBlood = false;
        [SerializeField] GameObject m_slideDust;

        private Animator m_animator;
        private Rigidbody2D m_body2d;
        private RectTransform m_RectTransform;
        private bool m_grounded = false;
        private bool m_block = false;
        private bool m_rolling = false;
        private int m_facingDirection = 1;
        private int m_currentAttack = 0;
        private float m_timeSinceAttack = 0.0f;
        private float m_delayToIdle = 0.0f;

        private void Start() {
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0) {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
            } else if (inputX < 0) {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
            }

            // Move
            if (!m_rolling) m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
            if (Input.GetKeyDown(KeyCode.J) && !m_rolling) {
                // m_body2d.velocity = new Vector2(10 * m_speed, m_body2d.velocity.y);
                m_animator.SetTrigger("Attack1");
            } else if (Input.GetKeyDown(KeyCode.K) && !m_rolling) {
                m_block = true;
                // m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", m_block);
            } else if (Input.GetKeyUp(KeyCode.K) && !m_rolling) {
                m_block = false;
                m_animator.SetBool("IdleBlock", m_block);
            } else if (Input.GetKeyDown("space") && m_grounded && !m_rolling) {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            } else if (Input.GetKeyDown("left shift") && !m_rolling) {
                m_rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity =
                    new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.CompareTag("Ground")) {
                // Debug.Log("ground");
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            } else if (collision.collider.CompareTag("Enemy")) {
                // Debug.Log("Enemy");
                // m_animator.SetTrigger("Hurt");
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // m_animator.SetTrigger("Hurt");
            if (state == States.Dead) {
                return;
            }

            if (other.CompareTag("Enemy")) {
                m_animator.SetTrigger("Hurt");
                m_body2d.MovePosition(m_body2d.position
                                      + new Vector2(
                                          transform.position.x - other.transform.position.x, 0) *
                                      m_speed * 0.2f);
            } else if (other.CompareTag("Arror")) {
                var projectile = other.gameObject.GetComponent<Projectile>();
                if (!m_block||m_facingDirection==projectile.direction) {
                    SufferDamage(projectile.damage);
                    if (state != States.Dead) {
                        GetHit();
                    }
                } else {
                    m_animator.SetTrigger("Block");
                }
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D other) {
            // Debug.Log("TriggerStay");
        }

        void OnTriggerExit2D(Collider2D other) { }

        protected override void Die() {
            base.Die();
            m_animator.SetTrigger("Death");
        }

        public override void GetHit() {
            if (state != States.Dead) {
                m_animator.SetTrigger("Hurt");
            }
        }

        void AE_ResetRoll() {
            // Animator   tmpanimator=null;
            // tmpanimator.SetBool("Grounded", true);
            m_rolling = false;
        }
    }
}