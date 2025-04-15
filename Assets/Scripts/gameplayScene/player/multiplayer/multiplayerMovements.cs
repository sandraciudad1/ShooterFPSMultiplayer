using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class multiplayerMovements : MonoBehaviourPun
{
    CharacterController controller;
    [SerializeField] Animator animator; 
    Vector3 velocity;
    
    bool isGrounded;
    bool loadValue = false;

    float speed = 5.0f;
    float jumpHeight = 1.0f;
    float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (gameManager.gameManagerInstance.canStart == 1)
        {
            if (!loadValue)
            {
                gameManager.gameManagerInstance.LoadProgress();
                speed = gameManager.gameManagerInstance.playerSpeed;
                loadValue = true;
            }

            // Controls local player
            if (!photonView.IsMine) return;

            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);

            bool isRunning = moveX != 0 || moveZ != 0;
            if (animator != null)
            {
                // Syncronize runnin animation
                photonView.RPC("SyncAnimation", RpcTarget.All, "run", isRunning);

                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    photonView.RPC("SyncAnimation", RpcTarget.All, "jump", true);
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    if(!isGrounded) {
                        photonView.RPC("SyncAnimation", RpcTarget.All, "jump", false);
                    }
                }
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    [PunRPC]
    void SyncAnimation(string parameter, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameter, value);
        }
    }
}