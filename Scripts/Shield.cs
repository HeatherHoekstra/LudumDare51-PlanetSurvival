using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public AudioClip explosionSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag !="Home" && collision.gameObject.tag != "Shield" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Loot")
        {
            if(transform.parent.tag == "Player")
            {
                transform.parent.gameObject.GetComponent<Player>().PushBack(collision.gameObject);
                GameManager.Instance.shieldSelfActive = false;

                if (transform.parent.gameObject.GetComponent<Player>().hasLoot)
                {
                    transform.parent.gameObject.GetComponent<Player>().DropLoot();
                }
            }
            else
            {
               transform.parent.GetComponent<AudioSource>().PlayOneShot(explosionSound, .7f);
                GameManager.Instance.shieldPlanetActive = false;
            }
            transform.parent.GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
            GameManager.Instance.UpdateUI();

        }
    }
}
