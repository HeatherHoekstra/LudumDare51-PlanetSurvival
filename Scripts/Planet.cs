using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Transform playerPlanet;
    public float rotationSpeed;
    public float circleRadius;
    public float angle;
    [SerializeField] private float spinSpeed;

    [SerializeField] private int state =1;
    [SerializeField] private ParticleSystem explosion;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        playerPlanet = GameManager.Instance.playerPlanet.transform;

        circleRadius = Vector2.Distance(transform.position, playerPlanet.position);

        if (angle == 0)
        {
            angle = Random.Range(0, 361);
        }
        //Vector3 targetDir = playerPlanet.position.normalized - transform.position.normalized;
        //angle = Vector3.Angle(targetDir, transform.forward);

        rotationSpeed = Random.Range(GameManager.Instance.planetMinSpeed, GameManager.Instance.planetMaxSpeed);
        int chooseDir = Random.Range(0, 2);
        if(chooseDir == 1)
        {
            rotationSpeed = rotationSpeed * -1f;
        }

        spinSpeed = rotationSpeed * 100;
    }

    // Update is called once per frame
    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;

        Vector3 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * circleRadius;
        transform.position = playerPlanet.position + offset;

        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

    }

    public void Shrink()
    {
        GetComponent<AudioSource>().PlayOneShot(explosionSound, .5f);
        explosion.Play();
        state -= 1;
        if (state < 1)
        {
            StartCoroutine(GameManager.Instance.DestroyPlanet(gameObject));
        }
        else
        {
            transform.localScale = new Vector2(transform.localScale.x/2, transform.localScale.y/2);
        }
        int amount = Random.Range(1, 4) * (state + 1);
        GameManager.Instance.SpawnLoot(transform, amount);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Home")
        {
            if(GameManager.Instance.playerPlanetState >= state)
            {
                GetComponent<AudioSource>().PlayOneShot(explosionSound, .2f);
                explosion.Play();
                StartCoroutine(GameManager.Instance.DestroyPlanet(gameObject));
                GameManager.Instance.SpawnLoot(transform, (Random.Range(1, 4) * (state + 1)));
            }
        }
        if(collision.gameObject.tag == "Shield")
        {
            if(collision.transform.parent.gameObject.tag == "Home")
            {
                GetComponent<AudioSource>().PlayOneShot(explosionSound, .2f);
                explosion.Play();
                StartCoroutine(GameManager.Instance.DestroyPlanet(gameObject));
                GameManager.Instance.SpawnLoot(transform, (Random.Range(1, 4) * (state + 1)));
            }
            else
            {
                //GetComponent<AudioSource>().PlayOneShot(explosionSound, .5f);
                explosion.Play();
                Shrink();
            }
        }
    }
}
