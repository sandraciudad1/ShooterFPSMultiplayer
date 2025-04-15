using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMovement : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    float speed = 5f;
    float rotationSpeed = 2f;
    float detectionRange = 1.5f;

    private GameObject minimapMarker;
    enemyGenerator generator;

    void Start()
    {
        generator = GameObject.Find("enemyGenerator").GetComponent<enemyGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && gameManager.gameManagerInstance.gameType.Equals("one"))
        {
            Vector3 direction = (player.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("attack", true);
                StartCoroutine(waitForLeave());
            }

            // Updates enemy position in minimap
            if (minimapMarker != null)
            {
                Vector2 minimapPos = minimap.instance.WorldToMinimapPosition(transform.position);
                minimapMarker.GetComponent<RectTransform>().localPosition = minimapPos;
            }
        }
    }

    IEnumerator waitForLeave()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("leave", true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

        if (generator != null && minimapMarker != null)
        {
            generator.RemoveMarker(minimapMarker);
        }
    }

    public void SetMinimapMarker(GameObject marker)
    {
        if (marker != null)
        {
            minimapMarker = marker;
        }
    }

    public GameObject GetMinimapMarker()
    {
        return minimapMarker;
    }

    public void RemoveMinimapMarker()
    {
        if (minimapMarker != null)
        {
            generator.RemoveMarker(minimapMarker);
            minimapMarker = null;
        }
    }

    public void DealDamage()
    {
        UIcontroller ui = GameObject.Find("Player").GetComponent<UIcontroller>();
        if (ui != null)
        {
            ui.modifyLife(-0.1f);
        }
    }
}
