using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private float totalTime;
    [SerializeField] private float currentTime = 5;
    [SerializeField] private GameObject spawnpoints;
    [SerializeField] private GameObject planets;
    [SerializeField] private List<GameObject> spawnpointsList = new List<GameObject>();
    [SerializeField] private List<GameObject> planetsList = new List<GameObject>();
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] private float minSpeedAsteroid;
    [SerializeField] private float maxSpeedAsteroid;
    public GameObject playerPlanet;
    public GameObject player;
    public float planetMinSpeed;
    public float planetMaxSpeed;
    public GameObject lootPrefab;   

    public int points;
    public int shieldSelfCost;
    public int shieldPlanetCost;
    public bool shieldSelfActive;
    public bool shieldPlanetActive;
    public GameObject playerPlanetShield;
    public int speedUpCost;
    public int growPlanetCost;
    public int playerPlanetState;

    [SerializeField] private int level = 1;
    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;
    [SerializeField] private float levelUpTime;
    [SerializeField] private float levelUpTimer;


    public float spawnTime = 1;

   // public int playerPlanetState;

    public GameObject UI;
    public UI UIScript;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        Time.timeScale = 0;
    }

        // Start is called before the first frame update
    void Start()
    {
        UIScript = UI.GetComponent<UI>();

        //make list of spawnpoints and planets
        foreach(Transform planet in planets.transform)
        {
            planetsList.Add(planet.gameObject);
        }
        foreach(Transform spawnpoint in spawnpoints.transform)
        {
            spawnpointsList.Add(spawnpoint.gameObject);
        }

        playerPlanetState = playerPlanet.GetComponent<PlayerPlanet>().state;
        growPlanetCost = playerPlanetState * 4;
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;

        currentTime += Time.deltaTime;
        if(currentTime >= spawnTime)
        {
            currentTime = 0;
            SpawnAsteroid();
        }
        levelUpTimer += Time.deltaTime;
        if(level1 && levelUpTimer > levelUpTime)
        {
            levelUpTimer = 0;
            LevelUp();
            levelUpTime *= 2f;
        }

        if(level == 3)
        {
            if(planetsList.Count < 2)
            {
                GameOver(true, "Your home planet is the last one in the universe!");
            }
        }
    }

    private void SpawnAsteroid()
    {
        //choose spawnpoint and planet
        GameObject currentTarget;
        int index = Random.Range(0, spawnpointsList.Count);
        GameObject currentSpawnPoint = spawnpointsList[index];

        if (planetsList.Count > 0)
        {
            index = Random.Range(0, planetsList.Count);
            currentTarget = planetsList[index];
        }
        else
        {
            currentTarget = playerPlanet;
        }

        //spawn asteroid and set target and speed.
        GameObject newAsteroid = Instantiate(asteroidPrefab, currentSpawnPoint.transform.position, Quaternion.identity);
        Asteroid script = newAsteroid.GetComponent<Asteroid>();
        script.target = currentTarget.transform;
        script.speed = Random.Range(minSpeedAsteroid, maxSpeedAsteroid);
    }

    public IEnumerator DestroyPlanet(GameObject hitPlanet)
    {

        if (planetsList.Contains(hitPlanet))
        {
            planetsList.Remove(hitPlanet);
        }
        hitPlanet.GetComponent<SpriteRenderer>().enabled = false;
        hitPlanet.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        
        Destroy(hitPlanet);      
        
    }

    public void SpawnLoot(Transform pos, int number)
    {
        while(number > 0)
        {
            float x = Random.Range(-5, 5);
            float y = Random.Range(-5, 5);

            Vector2 newPos = new Vector2(pos.position.x - x, pos.position.y - y);

            GameObject newLoot = Instantiate(lootPrefab, newPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            if(level == 2)
            {
                newLoot.transform.localScale = new Vector2(1.5f, 1.5f);
            }
            else if (level == 3)
            {
                newLoot.transform.localScale = new Vector2(2, 2);
            }
            number--;
        }

       
    }

    public void ActivateShieldSelf()
    {
        player.GetComponent<Player>().ActivateShield();
        shieldSelfActive = true;
    }

    public void ActivateShieldPlanet()
    {
        playerPlanetShield.SetActive(true);
        shieldPlanetActive = true;
    }

    public void SpeedUpPlayer()
    {
        player.GetComponent<Player>().speed *= 1.5f;
        player.GetComponent<Player>().rb.drag *= 1.1f;
    }

    public void GrowPlayerPlanet()
    {
        PlayerPlanet script = playerPlanet.GetComponent<PlayerPlanet>();

        if(script.state < script.maxState)
        {
            script.Grow();            
        }
    }

    public void UpdateUI()
    {
        UIScript.UpdateUI();
    }

    public void LevelUp()
    {
        if(level == 1)
        {
            level2.SetActive(true);
            level1.SetActive(false);            

            GameObject newPlanets = level2.transform.Find("Planets2").gameObject;
            int children = newPlanets.transform.childCount;

            while(children > 0)
            {
                foreach (Transform planet in newPlanets.transform)
                {
                    planetsList.Add(planet.gameObject);
                    planet.transform.parent = planets.transform;
                }

                children = newPlanets.transform.childCount;
            }

            spawnpointsList.Clear();
            GameObject newSpawnPoints = level2.transform.Find("Spawnpoints2").gameObject;
            //children = newSpawnPoints.transform.childCount;

            foreach (Transform spawnpoint in newSpawnPoints.transform)
            {
                spawnpointsList.Add(spawnpoint.gameObject);
            }

            level = 2;            
        }
        else if(level == 2)
        {
            level3.SetActive(true);
            level2.SetActive(false);            

            GameObject newPlanets = level3.transform.Find("Planets3").gameObject;
            int children = newPlanets.transform.childCount;
            while (children > 0)
            {
                foreach (Transform planet in newPlanets.transform)
                {
                    planetsList.Add(planet.gameObject);
                    planet.transform.parent = planets.transform;
                }

                children = newPlanets.transform.childCount;
            }

            spawnpointsList.Clear();
            GameObject newSpawnPoints = level3.transform.Find("Spawnpoints3").gameObject;
            //children = newSpawnPoints.transform.childCount;

            foreach(Transform spawnpoint in newSpawnPoints.transform)
            {
                spawnpointsList.Add(spawnpoint.gameObject);
            }
            level = 3;
        }

        spawnTime /= 1.5f;
        player.GetComponent<Animator>().SetTrigger("grow");
    }

    public void GameOver(bool won, string reason)
    {
        player.GetComponent<Player>().canMove = false;
        UI.GetComponent<UI>().GameOver(won, reason);
    }
}
