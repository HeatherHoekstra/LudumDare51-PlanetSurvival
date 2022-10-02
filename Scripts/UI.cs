using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private Button shieldSelfButton;
    [SerializeField] private Button shieldPlanetButton;
    [SerializeField] private Button speedUpButton;
    public GameObject speedUpObject;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Button growButton;
    [SerializeField] private TextMeshProUGUI growText;

    [SerializeField] GameObject menu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI reasonText;

    public AudioClip buySound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        coins.text = GameManager.Instance.points.ToString();

        if(GameManager.Instance.points >= GameManager.Instance.shieldSelfCost && !GameManager.Instance.shieldSelfActive)
        {
            shieldSelfButton.interactable = true;
        }
        else
        {
            shieldSelfButton.interactable = false;
        }
        if (GameManager.Instance.points >= GameManager.Instance.shieldPlanetCost && !GameManager.Instance.shieldPlanetActive)
        {
            shieldPlanetButton.interactable = true;
        }
        else
        {
            shieldPlanetButton.interactable = false;
        }
        if (GameManager.Instance.points >= GameManager.Instance.speedUpCost)
        {
            speedUpButton.interactable = true;
        }
        else
        {
            speedUpButton.interactable = false;
        }
        if (GameManager.Instance.points >= GameManager.Instance.growPlanetCost)
        {
            growButton.interactable = true;
        }
        else
        {
            growButton.interactable = false;
        }

        string newText = "Grow Planet (" + GameManager.Instance.growPlanetCost.ToString();
        newText = newText + "C)";
        growText.text = newText;
    }

    public void BuyShieldSelf()
    {
        GetComponent<AudioSource>().PlayOneShot(buySound, .5f);
        GameManager.Instance.points -= GameManager.Instance.shieldSelfCost;
        GameManager.Instance.ActivateShieldSelf();
        UpdateUI();
    }

    public void BuyShieldPlanet()
    {
        GetComponent<AudioSource>().PlayOneShot(buySound, .5f);
        GameManager.Instance.points -= GameManager.Instance.shieldPlanetCost;
        GameManager.Instance.ActivateShieldPlanet();
        UpdateUI();
    }

    public void BuySpeedUp()
    {
        GetComponent<AudioSource>().PlayOneShot(buySound, .5f);
        GameManager.Instance.points -= GameManager.Instance.speedUpCost;
        GameManager.Instance.SpeedUpPlayer();
        GameManager.Instance.speedUpCost += 5;
        string newText = "+Speed (" + GameManager.Instance.speedUpCost.ToString();
        newText = newText + "C)";
        speedText.text = newText;
        UpdateUI();
    }

    public void BuyGrowPlanet()
    {
        GetComponent<AudioSource>().PlayOneShot(buySound, .5f);
        GameManager.Instance.points -= GameManager.Instance.growPlanetCost;
        GameManager.Instance.GrowPlayerPlanet();
        string newText = "Grow Planet (" + GameManager.Instance.growPlanetCost.ToString();
        newText = newText + "C)";
        growText.text = newText;
        UpdateUI();
    }

    public void GameOver(bool won, string reason)
    {
        Time.timeScale = 0.1f;
        if (won)
        {
            gameOverText.text = "You won!";
        }

        reasonText.text = reason;

        gameOverMenu.SetActive(true);
        menu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Play()
    {
        menu.SetActive(false);
        playMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(5);
        Time.timeScale = 0;
    }
}
