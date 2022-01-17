using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    public GameObject Player;

    private NavMeshAgent agent;

    public float EnemyDistanceRun = 4.0f;

    [SerializeField] private Animator animator;
    private bool movimiento = true;
    private float speed;
    public float timeAttack;
    private float currentTime;

    public AudioClip ZombieAttackSound;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeAttack;
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (movimiento)
        {
            float distance = Vector3.Distance(transform.position, Player.transform.position);

            if (distance < EnemyDistanceRun)
            {
                Vector3 dirToPlayer = transform.position - Player.transform.position;

                Vector3 newPos = transform.position - dirToPlayer;

                agent.SetDestination(newPos);
            }
        }
        else
        {
            if (currentTime >= timeAttack)
            {
                currentTime = 0;
                animator.SetTrigger("Attack");
                AudioSource.PlayClipAtPoint(ZombieAttackSound, agent.transform.position, 1);
                Invoke(nameof(AplicarDmg), 1.0f);
                
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }

    }

    private void AplicarDmg()
    {
        Player.GetComponent<PlayerHealth>().Damage();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if (other.CompareTag("Player"))
        {
            movimiento = false;
            agent.speed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movimiento = true;
            agent.speed = speed;
        }
    }
}
