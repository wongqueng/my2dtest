using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    public class ThinkingPlaceable : Placeable
    {
        [HideInInspector] public States state = States.Dragged;
        public enum States
        {
            Dragged, //when the player is dragging it as a card on the play field
            Idle, //at the very beginning, when dropped
            Seeking, //going for the target
            Attacking, //attack cycle animation, not moving
            Dead, //dead animation, before removal from play field
        }

        public AttackType attackType;
        public enum AttackType
        {
            Melee,
            Ranged,
        }


        public float hitPoints=100;
        public float attackRange=2;
        public float attackRatio=3;
        public float lastBlowTime = -1000f;
        public float damage=20;

        public float timeToActNext = 0f;

		//Inspector references
		[Header("Projectile for Ranged")]
		public GameObject projectilePrefab;
		public Transform projectileSpawnPoint;


		public UnityAction<ThinkingPlaceable> OnDealDamage, OnProjectileFired,OnDieAniEnd;


        public virtual void StartAttack()
        {
            state = States.Attacking;
        }

        public virtual void DealBlow()
        {
            lastBlowTime = Time.time;
        }

		//Animation event hooks
		public void DealDamage()
        {
	        Debug.Log("DealDamage");
			//only melee units play audio when the attack deals damage
			// if(attackType == AttackType.Melee)
				//audioSource.PlayOneShot(attackAudioClip, 1f);
			if(OnDealDamage != null)
				OnDealDamage(this);
		}
		public void DieAniEnd()
        {
			//only melee units play audio when the attack deals damage
			// if(attackType == AttackType.Melee)
				//audioSource.PlayOneShot(attackAudioClip, 1f);
			if(OnDieAniEnd != null)
				OnDieAniEnd(this);
		}
		public void FireProjectile()
        {
			//ranged units play audio when the projectile is fired
			//audioSource.PlayOneShot(attackAudioClip, 1f);

			if(OnProjectileFired != null)
				OnProjectileFired(this);
		}

        public virtual void Seek()
        {
            state = States.Seeking;
        }

        protected void TargetIsDead(Placeable p)
        {
            //Debug.Log("My target " + p.name + " is dead", gameObject);
            state = States.Idle;
            timeToActNext = lastBlowTime + attackRatio;
        }
        
        public virtual void GetHit() {
        }
        public float SufferDamage(float amount)
        {
            hitPoints -= amount;
            //Debug.Log("Suffering damage, new health: " + hitPoints, gameObject);
            if(state != States.Dead
				&& hitPoints <= 0f)
            {
                Die();
            }

            return hitPoints;
        }

		public virtual void Stop()
		{
			state = States.Idle;
		}

        protected virtual void Die()
        {
            state = States.Dead;
			//audioSource.pitch = Random.Range(.9f, 1.1f);
			//audioSource.PlayOneShot(dieAudioClip, 1f);

			if(OnDie != null)
            	OnDie(this);
        }
    }

