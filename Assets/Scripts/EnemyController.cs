using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private GameObject UIPrefab;
    [SerializeField] public float maxDistance;
    [SerializeField] private float offsetDistance;
    
    //mouse interactions
    private GameObject myProperties;
    private Popup myPopup;
    private GameObject myPlayer;
    private Camera cam;

    public EnemyLikes enemyLike;
    [SerializeField] private float detectionRadius = 4f;
    private Coroutine playerPathfind;
    private bool playerPathfindingRunning = false;
    private Coroutine adPathfind;
    private bool isFollowingAd = false;

    [SerializeField] private LayerMask castable;
    [SerializeField] private float speed = 7f;
    
    private string currentCoroutineName;
    private MapGenerator mapGenerator;
    private Vector2 targetPos;
    private List<Vector2> path;
    private Node targetNode;

    private Rigidbody2D rb;

    private int currentPathIndx;

    private GameObject player;

    private GameObject adToFollow;

    [SerializeField] private Sprite cdSprite;
    [SerializeField] private Sprite benSprite;
    [SerializeField] private Sprite dogFoodSprite;
    [SerializeField] private Sprite satSprite;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CheckForDistractions());
        mapGenerator = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        myProperties = Instantiate(UIPrefab, canvas.transform);
        myProperties.SetActive(false);
        myPopup = myProperties.GetComponent<Popup>();
        cam = Camera.main;
    }

    public void ChangePopup() {
        Image spriteRenderer = myPopup.GetComponent<Image>();
        switch (enemyLike) {
            case EnemyLikes.CD:
                spriteRenderer.sprite = cdSprite;
                break;
            case EnemyLikes.BEN_SHAPIRO:
                spriteRenderer.sprite = benSprite;
                break;
            case EnemyLikes.DOG_FOOD:
                spriteRenderer.sprite = dogFoodSprite;
                break;
            case EnemyLikes.SAT:
                spriteRenderer.sprite = satSprite;
                break;
        }
    }
    

    void Update() {
        if (!EnoughDistanceToPlayer()) return;
        //update every 5 frames if there's lag
        HoverUI();
        HandleMovement();
        KillPlayer();
    }

    private void KillPlayer() {
        if (Vector2.Distance(transform.position, player.transform.position) < 1) {
            Debug.Log("Kill Player");
            SceneManager.LoadScene("StartingAreaScene");
        }
    }

    private void HoverUI()
    {
        Vector3 enemyToPlayer = myPlayer.transform.position - transform.position;
        enemyToPlayer.z = 0.0f;
        if (enemyToPlayer.sqrMagnitude > maxDistance*maxDistance)
        {
            if(!myPopup.decaying) StartCoroutine(myPopup.Die());
            return;
        }
        Vector3.Normalize(enemyToPlayer);
        Vector3 offsetBy = enemyToPlayer * offsetDistance;
        
        Vector2 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider == null)
        {
            if(!myPopup.decaying) StartCoroutine(myPopup.Die());
            return;
        }
        if(hit.collider == GetComponent<BoxCollider2D>())
        {
            myProperties.SetActive(true);
            if(!myPopup.growing) StartCoroutine(myPopup.Grow());
            MoveProperties(offsetBy);
        }
    }

    private void MoveProperties(Vector3 toOffset)
    {
        // move ui by this amount
        Vector2 screenCoord = RectTransformUtility.WorldToScreenPoint(cam, transform.position);
        myProperties.GetComponent<RectTransform>().transform.position = screenCoord + (Vector2) toOffset;
    }
    
    private void HandleMovement()
    {

        if (path != null)
        {
            if (currentPathIndx >= path.Count) {
                return;
            }
            Vector2 targetPos = path[currentPathIndx];
            if (Vector2.Distance(transform.position, targetPos) > 1f)
            {
                Vector2 moveDir = (targetPos - (Vector2) transform.position).normalized;
                
                transform.position = (Vector2) transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndx++;
                if (currentPathIndx >= path.Count)
                {
                    path = null;
                }
            }
        }
    }

    IEnumerator CheckForDistractions()
    {
        while (true)
        {
            if (!EnoughDistanceToPlayer()) {
                yield return new WaitForSeconds(0.1f);
            }
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, castable);
            if (hit != null)
            {
                if (hit.tag == "Bullet")
                {
                    if (!isFollowingAd) {
                        if (hit.gameObject.GetComponent<DistractionBullet>().type == enemyLike) {
                            adToFollow = hit.gameObject;
                            Debug.Log("Follow ad");
                            if (playerPathfind != null)
                            {
                                StopCoroutine(playerPathfind);
                                playerPathfindingRunning = false;
                            }
                            adPathfind = StartCoroutine(FollowAd());
                            isFollowingAd = true;
                        }
                    }
                    
                } else if (hit.tag == "Player")
                {
                    if (adToFollow == null)
                    {
                        Debug.Log("Follow player");
                        if (adPathfind != null) {
                            StopCoroutine(adPathfind);
                        }

                        if (!playerPathfindingRunning) {
                            playerPathfind = StartCoroutine(FollowPlayer());
                            isFollowingAd = false;
                        }
                    }
                }

            }
            Debug.Log(playerPathfind);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FollowPlayer()
    {
        while (adToFollow == null) {
            if (!EnoughDistanceToPlayer()) {
                yield break;
            }
            SetTargetPos(player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FollowAd()
    {
        while (adToFollow != null)
        {
            path = mapGenerator.GetPathfinder().FindPath(transform.position, adToFollow.transform.position);
            yield return new WaitForSeconds(0.3f);
        }

        adToFollow = null;
    }

    public void SetTargetPos(Vector2 target)
    {
        currentPathIndx = 0;
        path = mapGenerator.GetPathfinder().FindPath(transform.position, target);
        if (path != null && path.Count > 1)
        {
            path.RemoveAt(0);
        }
    }

    public bool EnoughDistanceToPlayer() {
        return Vector2.Distance(transform.position, player.transform.position) < 8f;
    }
    
}