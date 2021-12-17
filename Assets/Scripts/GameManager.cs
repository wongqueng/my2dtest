using System;
using System.Collections;
using System.Collections.Generic;
using MyGame;
using UnityEngine;
using UnityRoyale;


public class GameManager : MonoBehaviour {
    [SerializeField] ThinkingPlaceable player;
    // Start is called before the first frame update


    private List<ThinkingPlaceable> ememys = new List<ThinkingPlaceable>();
    private List<Projectile> allProjectiles = new List<Projectile>();

    void Start() {
        player.OnDealDamage += OnPlaceableDealtDamage;


        Archer[] archers = FindObjectsOfType<Archer>();
        for (int i = 0; i < archers.Length; i++) {
            ememys.Add(archers[i]);
            archers[i].OnDie += OnPlaceableDead;
            archers[i].OnProjectileFired += OnProjectileFired;
            archers[i].OnDieAniEnd += OnDeadAniEnd;
        }
    }

    // Update is called once per frame
    void Update() {
        if (player.state != ThinkingPlaceable.States.Dead) {
            for (int i = 0; i < ememys.Count; i++) {
                ThinkingPlaceable ememy = ememys[i];
                if (ememy.state != ThinkingPlaceable.States.Dead) {
                    if (Math.Abs(ememy.transform.position.x - player.transform.position.x) <= 5) {
                        Quaternion rot;
                        if (ememy.transform.position.x > player.transform.position.x) {
                            ememy.GetComponent<SpriteRenderer>().flipX = false;
                            // ememy.transform.localScale = new Vector3(-1, 0, 0);
                        } else {
                            ememy.GetComponent<SpriteRenderer>().flipX = true;
                            // ememy.transform.localScale = new Vector3(1, 1, 1);
                        }
                        ememy.StartAttack();
                    } else {
                        ememy.Stop();
                    }
                }
            }
        }
        // Projectile currProjectile;
        // float progressToTarget;
        // for (int prjN = 0; prjN < allProjectiles.Count; prjN++) {
        //     currProjectile = allProjectiles[prjN];
        //     progressToTarget = currProjectile.Move();
        //     if (progressToTarget >= 1f) {
        //         if (currProjectile.target.state !=
        //             ThinkingPlaceable.States.Dead) {
        //             float newHP = currProjectile.target.SufferDamage(currProjectile.damage);
        //             player.GetHit();
        //         }
        //         Destroy(currProjectile.gameObject);
        //         allProjectiles.RemoveAt(prjN);
        //     }
        // }
    }

    private void OnPlaceableDealtDamage(ThinkingPlaceable p) {
        for (int i = 0; i < ememys.Count; i++) {
            ThinkingPlaceable ememy = ememys[i];
            int playerface = player.GetComponent<MyPlayer>().getFace();
            float distance = ememy.transform.position.x - player.transform.position.x;
            if (Math.Abs(ememy.transform.position.y-1 - player.transform.position.y) <= 1&&((playerface==1&&distance>=0&&distance <= 2)||(playerface==-1&&distance<=0&&distance >= -2))) {
                // ememy.GetHit();

                float newHealth = ememy.SufferDamage(p.damage);
                if (ememy.state != ThinkingPlaceable.States.Dead) {
                    ememy.GetHit();
                }
            }
        }
    }

    private void OnPlaceableDead(Placeable p) {
        p.OnDie -= OnPlaceableDead;
        ememys.Remove((ThinkingPlaceable) p);
    }

    private void OnDeadAniEnd(ThinkingPlaceable p) {
        p.OnProjectileFired -= OnProjectileFired;
        Destroy(p.gameObject);
    }

    private void OnProjectileFired(ThinkingPlaceable p) {
        if (p.state != ThinkingPlaceable.States.Dead) {
            Quaternion rot;
            if (p.transform.position.x > player.transform.position.x) {
                rot = Quaternion.Euler(0f, 0f, 0f);
            } else {
                rot = Quaternion.Euler(180f, 0, 0f);
            }

            Vector3 initposition = new Vector3();
            initposition.x = p.transform.position.x;
            initposition.y = p.transform.position.y - 0.2f;
            initposition.z = p.transform.position.z;
            Projectile prj =
                Instantiate<GameObject>(p.projectilePrefab, initposition, rot)
                    .GetComponent<Projectile>();
            if (p.transform.position.x > player.transform.position.x) {
                prj.direction = -1;
            } else {
                prj.direction = 1;
            }

            prj.target = player;
            allProjectiles.Add(prj);
        }
    }
}