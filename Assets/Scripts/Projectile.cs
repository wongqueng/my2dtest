using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
	public class Projectile : MonoBehaviour
	{
		[HideInInspector] public ThinkingPlaceable target;
		 public float damage;
		private float speed = 3f;
		[HideInInspector] public float direction = 1;
		[HideInInspector] public long createtime;
		private void Awake() {
			 createtime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}

		private void Update() {
			if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - createtime > 3000) {
				Destroy(gameObject);
				return;
			}
			transform.Translate(direction * Time.deltaTime * speed,0,0);
		}

		// public float Move(){
			// transform.position.x += direction * Time.deltaTime * speed;

			// transform.position.Set();
			// if (Math.Abs(transform.position.x - target.transform.position.x) < 0.5f) {
			// 	return 1;
			// }

			// return 0;
		// }
	}
}