using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
            [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public enum STATES { IDLE, SNEAK, WALK, RUN, JUMPING, CHAT, ATTACKING, LOCKED, DEAD };


        // set per default any character as idle
        public STATES state = STATES.IDLE;

                public string label = "Default Character";

        public float health = 100.0f;
        public float stamina = 100.0f;

        // is the character a player ?
        public bool isPlayer = false;

           // Start is called before the first frame update
        void Start()
        {
            if (this.GetComponent<Animator>() != null)
            {
                anim = this.GetComponent<Animator>();
            }
        }

        public STATES GetMoveState()
        {
            float input = anim.GetFloat("InputVertical");
            if (input <= 0f) return STATES.IDLE;
            if (input <= 0.50f) return STATES.SNEAK;
            if (input <= 1f) return STATES.WALK;
            return STATES.RUN;

        }

        public bool IsDead()
        {
            return health <= 0.0f || state == STATES.DEAD;
        }

        public bool isLocked(){
            return state == STATES.LOCKED;
        }

        public void Lock() 
        {
            if (state == STATES.LOCKED) return;
            state = STATES.LOCKED;

            //disable user inputs
            Invector.CharacterController.vThirdPersonInput ccI = this.GetComponent<Invector.CharacterController.vThirdPersonInput>();
            ccI.enabled = false;


            //disable ThirsPerson Controller
            Invector.CharacterController.vThirdPersonController cc = this.GetComponent<Invector.CharacterController.vThirdPersonController>();
            cc.enabled = false;

            //remove players velocity
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;

            //set sprint to false (just in case the player sprints)
            cc.Sprint(false);
        }

        public void Unlock() 
        {
            if (state != STATES.LOCKED) return;
            state = STATES.IDLE;

            Invector.CharacterController.vThirdPersonInput ccI = this.GetComponent<Invector.CharacterController.vThirdPersonInput>();
            ccI.enabled = true;

            Invector.CharacterController.vThirdPersonController cc = this.GetComponent<Invector.CharacterController.vThirdPersonController>();
            cc.enabled = true;

        }

        public void TakeDamage()
        {
           
        }

        public void beKilledInstantly(){
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Lock();
            health = 0;
            this.state = STATES.DEAD;
        }
}
