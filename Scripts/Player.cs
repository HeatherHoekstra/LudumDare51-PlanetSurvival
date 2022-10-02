using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 axis;

    public Rigidbody2D rb;
    public float speed = 0;
   
    public bool hasLoot;
    public int lootCounter;

    public GameObject shield;

    public List<GameObject> collectedLoot = new List<GameObject>();
    [SerializeField] private float pushBack;
    public ParticleSystem explosion;

    public bool justHit;
    public bool canMove = true;

    [SerializeField] GameObject tail;
    private Vector3 prevPos;
    [SerializeField] private float turnSpeed;

    public AudioClip explosionSound;
    public AudioClip collectSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            axis = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f) * speed;

            rb.AddForce(axis, ForceMode2D.Force);
            
        }

        if(Input.GetAxisRaw("Horizontal") !=0)
        {
            if (!tail.activeInHierarchy)
            {
                tail.SetActive(true);
                tail.GetComponent<ParticleSystem>().Play();
            }
        }
        else if(Input.GetAxisRaw("Vertical") != 0)
        {
            if (!tail.activeInHierarchy)
            {
                tail.SetActive(true);
                tail.GetComponent<ParticleSystem>().Play();
            }            
        }
        else
        {
            if (tail.activeInHierarchy)
            {
                tail.SetActive(false);
            }
        }

        Vector2 movementDir = transform.position - prevPos;
        if (movementDir != Vector2.zero)
        {
            float angle = (Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);
        }

        prevPos = transform.position;

    }

    private void Update()
    {

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= -0.05 && viewPos.x <= 1.05 && viewPos.y >= -0.05 && viewPos.y <= 1.05)
        {

        }
        else
        {
            GameManager.Instance.GameOver(false, "You were lost in space");
        }
    }

    public void ActivateShield()
    {
        shield.SetActive(true);
    }

    public void PushBack(GameObject collision)
    {
        GetComponent<AudioSource>().PlayOneShot(explosionSound, .5f);
        Vector2 dir = transform.position - collision.transform.position;
        dir.Normalize();
        rb.AddForce(dir * pushBack, ForceMode2D.Impulse);
        justHit = true;
        StartCoroutine(waitAfterPushBack());
    }

    private IEnumerator waitAfterPushBack()
    {
       yield return new WaitForSeconds(1.5f);
        justHit = false;
    }

    public void DropLoot()
    {
        foreach (GameObject loot in collectedLoot)
        {
            loot.transform.parent = null;
        }
        collectedLoot.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Loot")
        {
            hasLoot = true;
            collision.gameObject.transform.parent = gameObject.transform;
            collectedLoot.Add(collision.gameObject);
        }

        if(collision.gameObject.tag == "Home" && hasLoot)
        {
            foreach(GameObject loot in collectedLoot)
            {
                GameManager.Instance.points += 1;
                Destroy(loot);
                hasLoot = false;
            }
            GetComponent<AudioSource>().PlayOneShot(collectSound, .7f);
            collectedLoot.Clear();
            GameManager.Instance.UpdateUI();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!justHit)
        {
            explosion.Play();
            PushBack(collision.gameObject);

            GameManager.Instance.GameOver(false, "You died");
        }
    }
}
