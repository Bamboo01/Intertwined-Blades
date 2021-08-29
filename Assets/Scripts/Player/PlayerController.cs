using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController :  MonoBehaviour, BaseGazeInterface
{
    [SerializeField] Transform ShieldTransform;
    [SerializeField] Transform SwordTransform;
    [SerializeField] Transform BlockTransform;

    [Header("Sword stuff")]
    [SerializeField] GameObject Sword;
    [SerializeField] public Collider SwordCollider;

    [Header("Shield stuff")]
    [SerializeField] GameObject Shield;
    [SerializeField] Collider ShieldCollider;

    [Header("Player Properties")]
    [SerializeField] Camera playerCamera;
    [SerializeField] int startingHealth = 3;
    [SerializeField] float startingStamina = 6.0f;
    [SerializeField] float BlockDuration = 1.0f;
    [SerializeField] float AttackDuration = 1.0f;
    [SerializeField] float ThrustLength = 1.0f;
    [SerializeField] float MaxStamina = 5.0f;
    [SerializeField] int MaxHealth = 5;
    [SerializeField] float ShieldStaminaDrain = 5.0f;
    [SerializeField] float SwordStaminaDrain = 1.5f;
    [SerializeField] float StaminaRechargeDelay = 2.75f;
    [SerializeField] float StaminaRechargeRate = 2.0f;

    [Header("Player UI")]
    [SerializeField] Image crosshairImage;
    [SerializeField] Image staminaFill;
    [SerializeField] GameObject healthPrefab;
    [SerializeField] Transform healthContent;
    [SerializeField] float crosshairZoomDuration = 0.4f;

    public bool isBlockingLastFrame;
    public bool isBlocking;
    float blockToHoldDistance;

    public bool isAttacking;
    bool canAttack = false;

    float staminaRechargeTimer;
    float staminaCounter;

    int health;

    void SwitchIsBlockingState()
    {
        if (staminaCounter <= ShieldStaminaDrain * Time.deltaTime && isBlocking)
        {
            isBlocking = false;
            return;
        }


        Shield.transform?.DOKill();
        ShieldCollider.enabled = true;
        Vector3 finalpos = !isBlocking ? ShieldTransform.position : BlockTransform.position;
        Transform parent = !isBlocking ? ShieldTransform : BlockTransform;

        float duration = Mathf.Lerp(0.0f, blockToHoldDistance, (Shield.transform.position - finalpos).magnitude);
        duration *= BlockDuration;
        Shield.transform.parent = parent;
        Shield.transform.DOLocalMove(Vector3.zero, duration)
        .OnComplete(()=> 
        { 
            Shield.transform.localPosition = Vector3.zero;
            Shield.transform.localRotation = Quaternion.identity;
            ShieldCollider.enabled = false;
        });
        Shield.transform.DOLocalRotate(Vector3.zero, duration);
    }

    void Attack()
    {
        if (staminaCounter <= SwordStaminaDrain)
        {
            return;
        }

        StopStaminaRecharge();

        isAttacking = true;
        SwordCollider.enabled = true;
        SwordTransform?.DOKill();
        Vector3 originalPos = SwordTransform.position;
        Vector3 originalLocalPos = SwordTransform.localPosition;
        Vector3 originalRot = SwordTransform.localRotation.eulerAngles;

        Vector3 finalRot = originalRot;
        finalRot.z = 90.0f;

        Vector3 aimedPosition = playerCamera.transform.position + (playerCamera.transform.forward * ThrustLength);
        Vector3 dir = (aimedPosition - originalPos).normalized;
        Vector3 finalPos = originalPos + (ThrustLength * dir);

        staminaCounter -= SwordStaminaDrain;

        SwordTransform.DOLocalRotate(finalRot, AttackDuration * 0.4f).Play().OnComplete(() =>
        {
            SwordTransform.DOMove(finalPos, AttackDuration * 0.6f).OnComplete(() =>
            {
                SwordTransform.DOLocalMove(originalLocalPos, AttackDuration * 0.6f).Play().OnComplete(() =>
                {
                    SwordTransform.DOLocalRotate(originalRot, AttackDuration * 0.4f).Play().OnComplete(() =>
                    {
                        SwordCollider.enabled = false;
                        isAttacking = false;
                    });
                });
            }).Play();
        }).Play();
    }

    public void _Start()
    {
        foreach (Transform a in healthContent)
        {
            Destroy(a.gameObject);
        }

        MaxStamina = startingStamina;
        MaxHealth = startingHealth;

        staminaRechargeTimer = StaminaRechargeDelay;
        staminaCounter = MaxStamina;
        staminaFill.fillAmount = staminaCounter / MaxStamina;

        SwordCollider.enabled = false;
        ShieldCollider.enabled = false;

        Sword.transform.parent = SwordTransform;
        Sword.transform.localPosition = Vector3.zero;
        Shield.transform.parent = ShieldTransform;
        Shield.transform.localPosition = Vector3.zero;

        blockToHoldDistance = (BlockTransform.position - ShieldTransform.position).magnitude;

        health = startingHealth;
        for (int i = 0; i < startingHealth; i++)
        {
            Instantiate(healthPrefab, healthContent);
        }
    }

    void Update()
    {
        if (staminaCounter <= ShieldStaminaDrain * Time.deltaTime)
        {
            isBlocking = false;
        }

        if (isBlocking)
        {
            staminaCounter -= ShieldStaminaDrain * Time.deltaTime;
            StopStaminaRecharge();
        }

        if (Player.Instance._isScreenTouched)
        {
            if (!isAttacking && canAttack && !isBlocking)
            {
                Attack();
            }
            if (!canAttack && !isAttacking && staminaCounter >= ShieldStaminaDrain * Time.deltaTime)
            {
                isBlocking = true;
                ShieldCollider.enabled = true;
            }
        }
        else
        {
            isBlocking = false;
            ShieldCollider.enabled = false;
        }

        if (isBlocking != isBlockingLastFrame)
        {
            isBlockingLastFrame = isBlocking;
            SwitchIsBlockingState();
        }

        staminaFill.fillAmount = staminaCounter / MaxStamina;

        RechargeStamina();
    }

    void RechargeStamina()
    {
        if (staminaCounter <= MaxStamina)
        {
            staminaRechargeTimer -= Time.deltaTime;
            if (staminaRechargeTimer <= 0.0f)
            {
                staminaCounter += Time.deltaTime * StaminaRechargeRate;

                if (staminaCounter > MaxStamina)
                {
                    staminaCounter = MaxStamina;
                }
            }
        }
    }

    void StopStaminaRecharge()
    {
        staminaRechargeTimer = StaminaRechargeDelay;
    }

    public void OnGazeEnter(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (hit.collider.gameObject.GetComponent<EnemyController>().stunController.gameObject.activeSelf)
            {
                // Hardcoded...
                canAttack = true;
                float remaining = crosshairImage.rectTransform.localScale.x - 0.3f;
                float durationMod = Mathf.Lerp(0.0f, 1.0f, remaining / 0.7f);
                crosshairImage.rectTransform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), crosshairZoomDuration * durationMod);
            }
        }
    }

    public void OnGazeStay(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!hit.collider.gameObject.GetComponent<EnemyController>().stunController.gameObject.activeSelf)
            {
                float remaining = 1.0f - crosshairImage.rectTransform.localScale.x;
                float durationMod = Mathf.Lerp(0.0f, 1.0f, remaining / 0.7f);
                crosshairImage.rectTransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), crosshairZoomDuration * durationMod);
                canAttack = false;
            }
            else if (crosshairImage.rectTransform.localScale.x >= 0.3f)
            {
                // Hardcoded...
                canAttack = true;
                float remaining = crosshairImage.rectTransform.localScale.x - 0.3f;
                float durationMod = Mathf.Lerp(0.0f, 1.0f, remaining / 0.7f);
                crosshairImage.rectTransform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), crosshairZoomDuration * durationMod);
            }
        }
    }

    public void OnGazeExit(GameObject gazedObject)
    {
        if (gazedObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (gazedObject.GetComponent<EnemyController>().stunController.gameObject.activeSelf)
            {
                canAttack = false;
                float remaining = 1.0f - crosshairImage.rectTransform.localScale.x;
                float durationMod = Mathf.Lerp(0.0f, 1.0f, remaining / 0.7f);
                crosshairImage.rectTransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), crosshairZoomDuration * durationMod);
            }
        }
    }

    public void TakeDamage(int dmg = 1)
    {
        if (health < 0)
        {
            return;
        }

        for (int i = 0; i < dmg; i++)
        {
            healthContent.GetChild(MaxHealth - health).gameObject.SetActive(false);
            health--;
        }

        if (health == 0)
        {
            LoadHandler.Instance.ChangeScene("GameOverScene");
        }
    }

    public void IncreaseMaxHealth(int num)
    {
        health += num;
        MaxHealth += num;

        for (int i = 0; i < num; i++)
        {
            Instantiate(healthPrefab, healthContent);
        }
    }

    public void HealHealth(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (health == MaxHealth)
            {
                return;
            }
            health++;
            healthContent.GetChild(MaxHealth - health).gameObject.SetActive(true);
        }
    }

    public void IncreaseStamina(float s)
    {
        MaxStamina += s;
        staminaCounter = MaxStamina;
    }

    [ContextMenu("Take1Damage")]
    void takedmg()
    {
        TakeDamage();
    }
}