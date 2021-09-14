using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))] // запрещ€ет удал€ть определенного компонента
public class MyPlayerMovement : MonoBehaviour
{
    [Header("Player Property")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpForce;

    [SerializeField] private GameObject animObject;
    [SerializeField] private Animator animator; // получаем компонент
    [SerializeField] private GameObject Weapon;

    [SerializeField] private GameObject[] health;
    [SerializeField] private AttackController attackController;

    [SerializeField] private Image hungryLevel;

    private SpriteRenderer spriteRenderer;

    private float currentPlayerSpeed; // она по дефолту = 0, в нее сохран€ем 
    private Rigidbody2D rb;
    private bool groundCheck; // проверка земли

    private string lastObjectEated;
    Vector3 defaultPosition;

    Vector3 defaultWeaponPosition;
    Quaternion defaultWeaponRotation;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = animObject.GetComponent<SpriteRenderer>(); // плучаем ссылку
        animator = animObject.GetComponent<Animator>(); // получаем компонент
        defaultPosition = this.transform.position;
        defaultWeaponPosition = this.Weapon.transform.localPosition;
        defaultWeaponRotation = this.Weapon.transform.localRotation;
    }

    private void Start()
    {
        StartCoroutine("MakeHungry");
    }

    // ƒвижение
    private void Update()
    {
        if (isDeath)
        {
            return;
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            var speed = move.x * playerSpeed;

            if (speed > 0)
            {
                RightMove();
            }
            else if (speed < 0)
            {
                LeftMove();
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentPlayerSpeed = 0;
        }
        
        // ƒ« добавить enum
        // —делать флип через ‘лип
        // 4 способа UIExample
        rb.velocity = new Vector2(currentPlayerSpeed * Time.deltaTime, rb.velocity.y);  //обращ€емс€ к свойству ƒвижение объекта velocity

        animator.SetFloat("Speed", Mathf.Abs(currentPlayerSpeed)); // указываем параметр и значениу

    }

    public void Attack()
    {
        if (attackController.Hit())
        {
            animator.SetTrigger("Attack");
        }
    }


    public void RightMove()
    {
        currentPlayerSpeed = playerSpeed;
        spriteRenderer.flipX = false;
        animator.SetBool("IsLeft", false);
        this.Weapon.transform.localPosition = defaultWeaponPosition;
        this.Weapon.transform.localRotation = defaultWeaponRotation;
    }
    public void LeftMove()
    {
        currentPlayerSpeed = -playerSpeed;
        spriteRenderer.flipX = true;
        animator.SetBool("IsLeft", true);
        var position = defaultWeaponPosition;
        var rotation = defaultWeaponRotation;

        position.x *= -1;
        rotation.y *= -1;

        this.Weapon.transform.localPosition = position;
        this.Weapon.transform.localRotation = rotation;
    }

    public void StopMove()
    {
        currentPlayerSpeed = 0f;
    }
    public void Jump()
    {
        if (groundCheck)
        {
            animator.SetTrigger("Jump");

            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce); // rb.velocity.x - не затрагиваем ихменение » —ј
            groundCheck = false;
        }
    }

    public void Death()
    {
        if (isDeath)
        {
            return;
        }
       
        StartCoroutine("StartDeath");
    }

    private IEnumerator StartDeath()
    {
        isDeath = true;
        currentPlayerSpeed = 0.0f;
        this.animator.SetTrigger("IsDeath");
        hungryLevelCount = 100;
        yield return new WaitForSeconds(1);
        this.transform.position = defaultPosition;
        for (int i = 0; i < health.Length; i++)
        {
            health[i].SetActive(true);
        }
        isDeath = false;

    }

    bool isDeath = false;
    public void EateFruit(GameObject fruit)
    {
        GameObject.Destroy(fruit);

        if (!attackController.IsHit)
        {
            for (int i = 0; i < health.Length; i++)
            {
                if (health[i].gameObject.activeSelf)
                {
                    continue;
                }
                else
                {
                    health[i].SetActive(true);
                    return;
                }
            }

            hungryLevelCount += 25;
        }
    }

    public void EateMushroom(GameObject mushroom)
    {
        Debug.Log("Eating mushrooms");
        GameObject.Destroy(mushroom);

        if (!attackController.IsHit)
        {
            for (int i = health.Length - 1; i >= 0; i--)
            {
                if (health[i].activeSelf)
                {
                    health[i].SetActive(false);
                    return;
                }
            }
            // если жизней активных не находим - умираем
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Banana":
                {
                    EateFruit(collision.gameObject);
                    lastObjectEated = collision.gameObject.tag;
                }
                break;
            case "Mushroom":
                {
                    EateMushroom(collision.gameObject);
                    lastObjectEated = collision.gameObject.tag;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        groundCheck = true;

        switch (collision.tag)
        {
            case "Banana":
                {
                    EateFruit(collision.gameObject);
                    lastObjectEated = collision.tag;
                }
                break;
            case "Mushroom":
                {
                    EateMushroom(collision.gameObject);
                    lastObjectEated = collision.tag;
                }
                break;
            case "Orange":
                {
                    EateFruit(collision.gameObject);
                    lastObjectEated = collision.tag;
                }
                break;
            case "SpecialMushroom":
                {
                    EateMushroom(collision.gameObject);
                    lastObjectEated = collision.tag;
                }
                break;
            case "Water":
                {
                    if (lastObjectEated != "SpecialMushroom")
                    {
                        this.transform.position = defaultPosition;
                    }
                    else
                    {
                        int currentSceneNumber = int.Parse(SceneManager.GetActiveScene().name);
                        try
                        {
                            string nextSceneName = (currentSceneNumber + 1).ToString();
                            SceneManager.LoadScene(nextSceneName);
                        }
                        catch
                        {
                            SceneManager.LoadScene(currentSceneNumber.ToString());
                        }
                    }
                    lastObjectEated = collision.tag;
                }
                break;
            
        }
    }


    private int hungryLevelCount = 100;

    IEnumerator MakeHungry()
    {
        while (true)
        {
            hungryLevelCount-=1;

            if (hungryLevelCount <= 0)
            {
                Death();
            }

            if (hungryLevelCount > 100)
                hungryLevelCount = 100;

            hungryLevel.fillAmount = 0.01f * hungryLevelCount;
            yield return new WaitForSeconds(0.25f);
        }
    }

    // движение
    //if (transform.localScale.x > 0)
    //{
    //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //}

    //if (transform.localScale.x < 0)
    //{
    //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //}
}
