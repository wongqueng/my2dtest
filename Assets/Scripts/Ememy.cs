using UnityEngine;

namespace MyGame {
    public class Ememy : MonoBehaviour {
        private float heath = 100;




        public void GetHit(float hit) {
            heath -= hit;


        }


    }
}