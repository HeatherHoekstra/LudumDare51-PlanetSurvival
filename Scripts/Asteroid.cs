using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public float spinSpeed;
    public GameObject spawnpoint;
    public Transform target;
    private Vector3 targetDirection;

    // Start is called before the first frame update
    void Start()
    {
        targetDirection = (target.position - transform.position);
        spinSpeed = speed *100; // * .5f;

        int chooseDir = Random.Range(0, 2);
        if (chooseDir == 1)
        {
            spinSpeed = spinSpeed * -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move towards target

        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        transform.position += (targetDirection * (speed * Time.deltaTime));
        //spin
        //transform.eulerAngles = new Vector3(0, 0, transform.rotation.z + spinSpeed * Time.deltaTime);
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Edge"))
        {
           if (collision.gameObject.tag == "Planet")
            {
                //GameManager.Instance.PlanetHit(collision.gameObject);
                collision.gameObject.GetComponent<Planet>().Shrink();
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerPlanet")
        {
            collision.gameObject.GetComponent<PlayerPlanet>().Shrink();          
        }
        if (collision.tag != "Loot") {
            Destroy(gameObject);
        }
    }
}
