using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
    [NonSerialized] public Keybinds keybinds;
    private Camera cam;
    private Vector2 mousePos;
    
    //for shooting
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 5f;

    [NonSerialized] public EnemyLikes weaponType = EnemyLikes.SAT;
    
    [SerializeField] private float moveSpeed = 10f;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 40f;
    [SerializeField] public float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private float dashingShakeTime = 0.2f;
    [SerializeField] private float dashingShakeMagnitude = 10.0f;
    [SerializeField] private float dashingScale = 0.0f;
    [SerializeField] private float fireShakeMagnitude = 5.0f;
    
    private TrailRenderer trail;
    private Dash myDash;

    [SerializeField] private RectTransform sat;
    [SerializeField] private RectTransform ben;
    [SerializeField] private RectTransform cd;
    [SerializeField] private RectTransform dogFood;

    [SerializeField] private Sprite satSprite;
    [SerializeField] private Sprite benSprite;
    [SerializeField] private Sprite cdSprite;
    [SerializeField] private Sprite dogFoodSprite;
    
    
    private RectTransform selector;
    
    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        keybinds = new Keybinds();
        keybinds.Enable();
        keybinds.Player.Dash.started += Dash;
        keybinds.Player.Fire.started += Fire;
        keybinds.Player.FirstWeapon.started += FirstWeapon;
        keybinds.Player.SecondWeapon.started += SecondWeapon;
        keybinds.Player.ThirdWeapon.started += ThirdWeapon;
        keybinds.Player.FourthWeapon.started += FourthWeapon;
        trail = GetComponent<TrailRenderer>();
        myDash = GetComponent<Dash>();
        Cursor.visible = false;
        selector = GameObject.FindGameObjectWithTag("Selector").GetComponent<RectTransform>();
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        AudioManager.instance.Play("Gun");
        //create bullet + rigidbody
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Sprite sprite = null;

        switch (weaponType) {
            case EnemyLikes.CD:
                sprite = cdSprite;
                break;
            case EnemyLikes.BEN_SHAPIRO:
                sprite = benSprite;
                break;
            case EnemyLikes.DOG_FOOD:
                sprite = dogFoodSprite;
                break;
            case EnemyLikes.SAT:
                sprite = satSprite;
                break;
        }
        
        bullet.GetComponent<SpriteRenderer>().sprite = sprite;
        bullet.GetComponent<SpriteRenderer>().sortingOrder = 1000;
        bullet.GetComponent<Transform>().localScale = new Vector3(4, 4, 1);
        bullet.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //set velocity of bullet (direction and speed)
        rb.velocity = firePoint.up * bulletForce;
        bullet.GetComponent<DistractionBullet>().type = weaponType;
        StartCoroutine(cam.GetComponent<CameraShaker>().Shake(dashingShakeTime, fireShakeMagnitude));
    }

    // Update is called once per frame
    void Update() {
        MovePlayer();
        mousePos = keybinds.Player.Look.ReadValue<Vector2>();
        mousePos = cam.ScreenToWorldPoint(mousePos);
        //might need to use fixed update for this
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x)* Mathf.Rad2Deg -90f;
        rb.rotation = angle;
    }
    

    void Awake()
    {
        cam = Camera.main;
    }

    private void MovePlayer() {
        if (isDashing) {
            return;
        }
        Vector2 moveDir = keybinds.Player.Move.ReadValue<Vector2>();
        rb.velocity = moveDir * moveSpeed;
    }

    private void Dash(InputAction.CallbackContext context) {
        if (canDash) {
            AudioManager.instance.Play("Dash");
            StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
        // FindObjectOfType<AudioManager>().Play("Dash");//play sound
        canDash = false;
        isDashing = true;
        Vector2 dir = keybinds.Player.Move.ReadValue<Vector2>();
        rb.velocity = dir * dashingPower;
        //trail.emitting = true;
        myDash.StartParticles();
        StartCoroutine(cam.GetComponent<CameraShaker>().Shake(dashingShakeTime, dashingShakeMagnitude));
        StartCoroutine(Scale(dashingScale));
        yield return new WaitForSeconds(dashingTime);
        //trail.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gameController.RefreshLevelExposed();
        }
    }
    
    private IEnumerator Scale(float scaleMagnitude)
    {
        float elapsedTime = 0.0f;
        Vector3 origScale = transform.localScale;
        float halfTime = dashingTime / 2.0f;
        while (elapsedTime < dashingTime)
        {
            if (elapsedTime <= halfTime)
            {
                transform.localScale = Vector3.Lerp(origScale, origScale * scaleMagnitude, elapsedTime/halfTime);
            }
            else
            {
                transform.localScale = Vector3.Lerp(origScale * scaleMagnitude, origScale, (elapsedTime - halfTime)/halfTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = origScale;

    }
    
    void FirstWeapon(InputAction.CallbackContext context) {
        weaponType = EnemyLikes.SAT; //SAT
        selector.position = sat.position;
    }
    
    void SecondWeapon(InputAction.CallbackContext context) {
        weaponType = EnemyLikes.BEN_SHAPIRO; //Ben Shapiro
        selector.position = ben.position;
    }
    
    void ThirdWeapon(InputAction.CallbackContext context) {
        weaponType = EnemyLikes.CD; //CD
        selector.position = cd.position;
    }
    
    void FourthWeapon(InputAction.CallbackContext context) {
        weaponType = EnemyLikes.DOG_FOOD; //Dog Food
        selector.position = dogFood.position;
    }
}
