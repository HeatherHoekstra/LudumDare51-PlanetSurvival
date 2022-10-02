using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanet : MonoBehaviour
{
    public int state;
    public int maxState;
    public bool justHit;

    public AudioClip explosionSound;


    public ParticleSystem explosion;
    [SerializeField] private float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    public void Grow()
    {        
        state += 1;
        GameManager.Instance.playerPlanetState = state;
        GameManager.Instance.growPlanetCost = state *4;
        transform.localScale = new Vector2(transform.localScale.x *2, transform.localScale.y *2);

    }

    public void Shrink()
    {
        if (!justHit)
        {
            GetComponent<AudioSource>().PlayOneShot(explosionSound, .6f);
            explosion.Play();
            state -= 1;
            GameManager.Instance.growPlanetCost = state * 4;
            GameManager.Instance.UpdateUI();
            if (state > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                GameManager.Instance.GameOver(false, "Your home planet was destroyed!");
            }
            justHit = true;
            StartCoroutine(waitAfterHit());
        }
    }

    private IEnumerator waitAfterHit()
    {
        yield return new WaitForSeconds(.6f);
        justHit = false;
    }
}
