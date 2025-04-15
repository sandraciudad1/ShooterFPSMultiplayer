using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class raycast : MonoBehaviourPun
{
    [SerializeField] Camera camera;
    [SerializeField] Transform weaponTransform; 
    [SerializeField] float recoilAmount = 0.1f; 
    [SerializeField] float recoilSpeed = 5f; 
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TextMeshProUGUI bulletText;
    [SerializeField] TextMeshProUGUI totalBulletText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] AudioSource coinAudioSource;
    [SerializeField] Image progressBar;

    private Vector3 originalWeaponPosition; 
    enemyPool enemyPool;
    float nextFireTime = 0f;
    public int bulletCounter;
    int shootCounter = 0;
    bool initialized = false;

    void Start()
    {
        gameManager.gameManagerInstance.weaponType = 3;
        gameManager.gameManagerInstance.SaveProgress();
        bulletCounter = checkBulletCounter();

        originalWeaponPosition = weaponTransform.localPosition;
    }

    public void initializeRaycastValues()
    {
        bulletText.text = bulletCounter.ToString();
        totalBulletText.text = bulletCounter.ToString();

        enemyPool = GameObject.Find("enemyPool").GetComponent<enemyPool>();

        initialized = true;
    }

    void Update()
    {
        if (gameManager.gameManagerInstance.gameType.Equals("one") && !initialized)
        {
            initializeRaycastValues();
            gameManager.gameManagerInstance.canStart = 1;
            gameManager.gameManagerInstance.SaveProgress();
        }

        gameManager.gameManagerInstance.LoadProgress();
        if (gameManager.gameManagerInstance.canStart == 1 && initialized)
        {
            if (canShootWeapon1() || canShootOtherWeapon())
            {
                nextFireTime = Time.time + checkFireRate();
                muzzleEffect.Play();
                shootAudioSource.Play();

                photonView.RPC("ApplyRecoil", RpcTarget.AllViaServer); 

                RaycastHit hit;
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
                {
                    if (hit.transform.CompareTag("enemy"))
                    {
                        if (gameManager.gameManagerInstance.weaponType == 0)
                        {
                            increaseShootCounter(hit, 2);
                        }
                        else if (gameManager.gameManagerInstance.weaponType == 1)
                        {
                            increaseShootCounter(hit, 1);
                        }
                        else
                        {
                            kill(hit);
                        }
                    }
                    else if (hit.transform.CompareTag("Player"))
                    {
                        PhotonView photonView = hit.transform.root.GetComponent<PhotonView>();
                        if (photonView != null && !photonView.IsMine)
                        {
                            photonView.RPC("TakeDamage", RpcTarget.All, 0.1f);
                        }
                    }
                }

                bulletCounter--;
                bulletText.text = bulletCounter.ToString();
            }
            else
            {
                muzzleEffect.Stop();
            }

            if (bulletCounter <= 7)
            {
                bulletText.color = Color.red;
            }
            else
            {
                bulletText.color = Color.white;
            }

            if (bulletCounter <= 0 && Input.GetKeyDown(KeyCode.R) && gameManager.gameManagerInstance.coins >= 20)
            {
                coinAudioSource.Play();
                bulletCounter = checkBulletCounter();
                bulletText.text = bulletCounter.ToString();
                gameManager.gameManagerInstance.coins -= 20;
                gameManager.gameManagerInstance.SaveProgress();
                coinsText.text = gameManager.gameManagerInstance.coins.ToString();
            }
        }
        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, originalWeaponPosition, Time.deltaTime * recoilSpeed);
    }

    void kill(RaycastHit hit)
    {
        GameObject enemy = hit.transform.gameObject;
        enemy.GetComponent<enemyMovement>().RemoveMinimapMarker();

        enemyPool.returnObject(hit.transform.gameObject);
        UIcontroller ui = GameObject.Find("Player").GetComponent<UIcontroller>();
        if (ui != null)
        {
            ui.increaseDeadCounter();
            ui.updateCoins(25);
        }
    }

    void increaseShootCounter(RaycastHit hit, int limit)
    {
        shootCounter++;
        if (shootCounter >= limit)
        {
            kill(hit);
            shootCounter = 0;
        }
    }

    bool canShootWeapon1()
    {
        if (Input.GetMouseButtonDown(0) && gameManager.gameManagerInstance.weaponType == 0 && bulletCounter > 0)
        {
            return true;
        }
        return false;
    }

    bool canShootOtherWeapon()
    {
        if (Input.GetMouseButton(0) && gameManager.gameManagerInstance.weaponType > 0 && bulletCounter > 0 && Time.time >= nextFireTime)
        {
            return true;
        }
        return false;
    }

    float checkFireRate()
    {
        if (gameManager.gameManagerInstance.weaponType == 1)
        {
            return 0.5f;
        }
        if (gameManager.gameManagerInstance.weaponType == 2)
        {
            return 0.3f;
        }
        return 0.1f;
    }

    public int checkBulletCounter()
    {
        if (gameManager.gameManagerInstance.weaponType == 0)
        {
            return 10;
        }
        if (gameManager.gameManagerInstance.weaponType == 1)
        {
            return 20;
        }
        if (gameManager.gameManagerInstance.weaponType == 2)
        {
            return 30;
        }
        return 50;
    }

    public void updateBullets(int bullets)
    {
        bulletText.text = bullets.ToString();
    }

    [PunRPC]
    void ApplyRecoil()
    {
        if (!photonView.IsMine) return; 
        weaponTransform.localPosition -= Vector3.forward * recoilAmount; 
    }
}
