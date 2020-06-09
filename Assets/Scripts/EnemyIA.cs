using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SeriousCorona
{
    public class EnemyIA : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;
        public Transform pos;

        public Transform playerT;
        public float aggroDist;
        private int i;
        private int layerMask;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>(); 
            //agent.destination = pos.position;
            playerT = FindObjectOfType<ThirdPersonCharacter>().transform;
            layerMask = LayerMask.GetMask("Default");
        }

        // Update is called once per frame
        void Update()
        {
            if (i == 10)
            {
                i = 0;
                if (Vector3.Distance(transform.position, playerT.position) < aggroDist)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, playerT.position - transform.position, out hit, aggroDist, layerMask) 
                        && hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                    {
                        print(hit.collider.gameObject.name);
                        agent.destination = playerT.position;
                    }
                }
            }
            else
                i++;


            Vector3 move = transform.InverseTransformDirection(agent.velocity);
            move = Vector3.ProjectOnPlane(move, Vector3.up);
            float m_TurnAmount = Mathf.Atan2(move.x, move.z);
            float m_ForwardAmount = move.z;

            animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        }
    }
}
