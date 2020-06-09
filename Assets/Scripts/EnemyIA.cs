using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SeriousCorona
{
    public class EnemyIA : MonoBehaviour
    {
        public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;
        private Animator animator;
        public Transform pos;

        public Transform playerT;
        public float aggroDist;
        private int i;
        private int layerMask;
        private bool playerVisible = false;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>(); 
            //agent.destination = pos.position;
            layerMask = LayerMask.GetMask("Default");

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            GotoNextPoint();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerT == null)
            {
                playerT = FindObjectOfType<ThirdPersonCharacter>().transform;
            }

            if (playerT == null)
                return;

            if (i == 10)
            {
                i = 0;
                if (Vector3.Distance(transform.position, playerT.position) < aggroDist)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, playerT.position - transform.position, out hit, aggroDist, layerMask))
                    {
                        if (hit.collider.gameObject.CompareTag("Player"))
                        {
                            agent.destination = playerT.position;
                            playerVisible = true;
                        }
                        else
                        {
                            playerVisible = false;
                        }
                    }
                }
            }
            else
                i++;

            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 1f && !playerVisible)
                GotoNextPoint();

            Vector3 move = transform.InverseTransformDirection(agent.velocity);
            move = Vector3.ProjectOnPlane(move, Vector3.up);
            float m_TurnAmount = Mathf.Atan2(move.x, move.z);
            float m_ForwardAmount = move.z;

            animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;
            

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }
    }
}
