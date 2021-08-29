using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [Header("Animation Related")]
    public Animator animator;

    [Header("Enemy Properties")]
    public EnemyProperties enemyProperties;

    [Header("Enemy Prefabs")]
    public GameObject WeaponPrefab;
    public GameObject healthPrefab;

    [Header("Enemy parents")]
    public Transform rightHand;
    public Transform Head;
    public Transform healthContent;
    public Canvas HPCanvas;

    [Header("Enemy Monobehaviours")]
    public EnemyStunController stunController;
    public Collider enemyToEnemyCollider;
    public Collider enemyTriggerCollider;

    [HideInInspector] public Transform center;
    [HideInInspector] public Vector3 axis = Vector3.up;
    [HideInInspector] public Vector3 desiredPosition;

    [HideInInspector] public bool isInBackline = false;
    [HideInInspector] public Collider weaponCollider;
    [HideInInspector] public bool isTransitioning = false;
    [HideInInspector] private UnityAction<Collider> OnCollisionTrigger;
    [HideInInspector] public UnityAction<Collider> OnWeaponCollision;
    [HideInInspector] public UnityAction<Collision> OnEnemyCollision;
    [HideInInspector] public UnityAction OnEnemyDeath;
    [HideInInspector] public EnemyStateMachine stateMachine = new EnemyStateMachine();

    [HideInInspector] public int terrainMask;
    [HideInInspector] public bool isVulnerable;
    [HideInInspector] public int health;
    

    void Awake()
    {
        terrainMask = LayerMask.GetMask("Terrain");
    }

    void Start()
    {
        isVulnerable = false;
        center = Player.Instance.controller.transform;
        transform.position = (transform.position - center.position).normalized * enemyProperties.EngageRadius + center.position;
        stateMachine.Init(this);
        stateMachine.AddState<EnemyCirclingState>();
        stateMachine.AddState<EnemyIdleState>();
        stateMachine.AddState<EnemyAttackState>();
        stateMachine.AddState<EnemyDeadState>();
        stateMachine.AddState<EnemyStunnedState>();
        stateMachine.AddState<EnemyEngageState>();
        stateMachine.AddState<EnemyRetreatState>();
        stateMachine.AddState<EnemySpawningState>();

        stateMachine.ChangeState(EnemyStates.SPAWNING);
        GameObject wep = Instantiate(WeaponPrefab, rightHand);
        wep.GetComponent<EnemyWeapon>().controller = this;
        weaponCollider = wep.GetComponent<Collider>();
        weaponCollider.enabled = false;

        health = enemyProperties.startingHealth;
        for (int i = 0; i < enemyProperties.startingHealth; i++)
        {
            Instantiate(healthPrefab, healthContent);
        }
    }

    void Update()
    {
        if (!isTransitioning)
        {
            transform.LookAt(center, Vector3.up);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            RaycastHit hit;
            Vector3 pos = transform.position;
            pos.y += 100.0f;
            if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, terrainMask))
            {
                transform.position = hit.point;
            }

            float r = !isInBackline ? enemyProperties.EngageRadius : enemyProperties.RetreatRadius;
            desiredPosition = (transform.position - center.position).normalized * r + center.position;
            transform.position = desiredPosition;
        }
        stateMachine.Update();
    }

    void LateUpdate()
    {
        HPCanvas.transform.position = Head.position;
        Vector3 temp = HPCanvas.transform.localPosition;
        HPCanvas.transform.localPosition = new Vector3(temp.x, enemyProperties.EnemyHeight + 0.05f, temp.z);
        HPCanvas.transform.LookAt(HPCanvas.transform.localPosition - Camera.main.transform.forward, Camera.main.transform.up);
    }

    void OnTriggerEnter(Collider other)
    {
        OnCollisionTrigger?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        enemyTriggerCollider.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        OnEnemyCollision?.Invoke(collision);
    }

    public void SwapOnEnemyCollisionEvent(UnityAction<Collider> newAction = null)
    {
        OnCollisionTrigger = newAction;
    }

    public void SwapOnSwordCollisionEvent(UnityAction<Collider> newAction = null)
    {
        OnWeaponCollision = newAction;
    }

    public void StrafeLeft()
    {
        float r = !isInBackline ? enemyProperties.EngageRadius : enemyProperties.RetreatRadius;
        transform.RotateAround(center.position, axis, enemyProperties.StrafeSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center.position).normalized * r + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * enemyProperties.StrafeSpeed);
    }

    public void StrafeRight()
    {
        float r = !isInBackline ? enemyProperties.EngageRadius : enemyProperties.RetreatRadius;
        transform.RotateAround(center.position, axis, -enemyProperties.StrafeSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center.position).normalized * r + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * enemyProperties.StrafeSpeed);
    }

    public void Retreat()
    {
        isTransitioning = true;
        float radiusDistance = enemyProperties.RetreatRadius - enemyProperties.EngageRadius;
        Vector3 retreatStep = -(transform.forward * radiusDistance);

        Vector3 midwayPosition = transform.position + (retreatStep * 0.5f) + new Vector3(0, enemyProperties.RetreatHeight, 0);
        Vector3 endPosition = transform.position + (retreatStep);

        Vector3 endPositionTemp = endPosition;

        endPositionTemp.y += 100.0f;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(endPositionTemp, Vector3.down, out hit))
        {
            endPositionTemp.y = hit.point.y;
        }

        endPosition = endPositionTemp;

        transform.DOMoveX(endPosition.x, enemyProperties.RetreatDuration).SetEase(Ease.Linear).Play();
        transform.DOMoveZ(endPosition.z, enemyProperties.RetreatDuration).SetEase(Ease.Linear).Play();

        transform.DOMoveY(midwayPosition.y, enemyProperties.RetreatDuration * 0.5f).SetEase(Ease.OutCubic).Play().OnComplete
            (() =>
            {
                transform.DOMoveY(endPosition.y, enemyProperties.RetreatDuration * 0.5f).SetEase(Ease.InCubic).Play().OnComplete(() =>
                {
                    isInBackline = true;
                    isTransitioning = false;
                });
            });
    }

    public void Engage()
    {
        isTransitioning = true;
        float radiusDistance = enemyProperties.RetreatRadius - enemyProperties.EngageRadius;
        Vector3 retreatStep = (transform.forward * radiusDistance);

        Vector3 midwayPosition = transform.position + (retreatStep * 0.5f) + new Vector3(0, enemyProperties.RetreatHeight, 0);
        Vector3 endPosition = transform.position + (retreatStep);

        Vector3 endPositionTemp = endPosition;

        endPositionTemp.y += 100.0f;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(endPositionTemp, Vector3.down, out hit))
        {
            endPositionTemp.y = hit.point.y;
        }

        endPosition = endPositionTemp;

        transform.DOMoveX(endPosition.x, enemyProperties.RetreatDuration).SetEase(Ease.Linear).Play();
        transform.DOMoveZ(endPosition.z, enemyProperties.RetreatDuration).SetEase(Ease.Linear).Play();

        transform.DOMoveY(midwayPosition.y, enemyProperties.RetreatDuration * 0.5f).SetEase(Ease.OutCubic).Play().OnComplete
            (() =>
            {
                transform.DOMoveY(endPosition.y, enemyProperties.RetreatDuration * 0.5f).SetEase(Ease.InCubic).Play().OnComplete(() =>
                {
                    isInBackline = false;
                    isTransitioning = false;
                });
            });
    }

    [ContextMenu("Instagib")]
    public void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    public void StopEnemyCollisionResponse()
    {
        StartCoroutine(_StopEnemyCollisionResponse());
    }

    IEnumerator _StopEnemyCollisionResponse()
    {
        enemyToEnemyCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);
        enemyToEnemyCollider.enabled = true;
        yield break;
    }
}
