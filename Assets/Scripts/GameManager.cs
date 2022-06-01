using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] balls;
    public GameObject startingBox;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI title2Text;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    [SerializeField] private float fadeSpeed = 0.33f;
    private int numberOfBalls;
    private Counter counter;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        numberOfBalls = Random.Range(1, 11);

        for(int i = 0; i < numberOfBalls; i++)
        {
            balls[i].SetActive(true);
        }

        counter = GameObject.Find("EndingBox").GetComponent<Counter>();

        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(counter.Count == numberOfBalls)
        {
            GameOver();
        }
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2);

        while (titleText.color.a < 1 && title2Text.color.a < 1)
        {
            float fadeAmount = titleText.color.a + (fadeSpeed * Time.deltaTime);
            Color titleColor = new Color(titleText.color.r, titleText.color.g, titleText.color.b, fadeAmount);
            
            float fadeAmount2 = title2Text.color.a + (fadeSpeed * Time.deltaTime);
            Color title2Color = new Color(title2Text.color.r, title2Text.color.g, title2Text.color.b, fadeAmount2);

            titleText.color = titleColor;
            title2Text.color = title2Color;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (titleText.color.a > 0 && title2Text.color.a > 0)
        {
            float fadeAmount = titleText.color.a - (fadeSpeed * Time.deltaTime);
            Color titleColor = new Color(titleText.color.r, titleText.color.g, titleText.color.b, fadeAmount);
            
            float fadeAmount2 = title2Text.color.a - (fadeSpeed * Time.deltaTime);
            Color title2Color = new Color(title2Text.color.r, title2Text.color.g, title2Text.color.b, fadeAmount2);

            titleText.color = titleColor;
            title2Text.color = title2Color;
            yield return null;
        }
    }

    private void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
