using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region PRIVATE VARIABLES
    private int maxNumLives = 3;
    private int lives;
    private int score;
    private Camera mainCamera;
    public float cameraHalfWidth;
    public float cameraHalfHeight;

    #endregion

    #region SINGLETON

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    instance = container.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    #endregion
    // Start is called before the first frame update
    #region MONOBEHAVIOUR METHODS
    void Start()
    {
        mainCamera = Camera.main;
        lives = maxNumLives;
        StartCoroutine(SpawnAsteroids());
    }
    #endregion

    #region PUBLIC METHODS
    // Lose a life.
    public void LoseLife()
    {
        lives--;

        if (lives == 0)
            Restart();
    }

    // Gain points.
    public void GainPoints(int points)
    {
        score += points;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    // Spawn asteroids every few seconds.
    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            

            yield return new WaitForSeconds(Random.Range(2f, 8f));
            SpawnAsteroid();
        }
    }

    // Spawn an asteroid off the screen.
    private void SpawnAsteroid()
    {
        BrickScript newAsteroid = PoolManager.Instance.Spawn(Constants.ASTEROID_PREFAB_NAME).GetComponent<BrickScript>();

        Vector2 direction = newAsteroid.GetForceApplied();

        SpriteRenderer spriteRenderer = newAsteroid.GetComponentInChildren<SpriteRenderer>();
        float halfWidth = spriteRenderer.bounds.size.x / 2.0f;
        float halfHeight = spriteRenderer.bounds.size.y / 2.0f;

        // Asteroid moving up and right
        if (direction.x >= 0 && direction.y >= 0)
        {
            // Enter from bottom of screen
            if (Random.Range(0, 2) == 0)
                newAsteroid.transform.position = new Vector3(Random.Range(mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x), mainCamera.transform.position.y - cameraHalfHeight - halfHeight, newAsteroid.transform.position.z);
            // Enter from left of screen
            else
                newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x - cameraHalfWidth - halfWidth, Random.Range(mainCamera.transform.position.y - cameraHalfHeight, mainCamera.transform.position.y), newAsteroid.transform.position.z);
        }
        // Asteroid moving down and right
        else if (direction.x >= 0 && direction.y < 0)
        {
            // Enter from top of screen
            if (Random.Range(0, 2) == 0)
                newAsteroid.transform.position = new Vector3(Random.Range(mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x), mainCamera.transform.position.y + cameraHalfHeight + halfHeight, newAsteroid.transform.position.z);
            // Enter from left of screen
            else
                newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x - cameraHalfWidth - halfWidth, Random.Range(mainCamera.transform.position.y, mainCamera.transform.position.y + cameraHalfHeight), newAsteroid.transform.position.z);
        }
        // Asteroid moving up and left
        else if (direction.x < 0 && direction.y >= 0)
        {
            // Enter from bottom of screen
            if (Random.Range(0, 2) == 0)
                newAsteroid.transform.position = new Vector3(Random.Range(mainCamera.transform.position.x, mainCamera.transform.position.x + cameraHalfWidth), mainCamera.transform.position.y - cameraHalfHeight - halfHeight, newAsteroid.transform.position.z);
            // Enter from right of screen
            else
                newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x + cameraHalfWidth + halfWidth, Random.Range(mainCamera.transform.position.y - cameraHalfHeight, mainCamera.transform.position.y), newAsteroid.transform.position.z);
        }
        //Asteroid moving down and left
        else
        {
            // Enter from top of screen
            if (Random.Range(0, 2) == 0)
                newAsteroid.transform.position = new Vector3(Random.Range(mainCamera.transform.position.x, mainCamera.transform.position.x + cameraHalfWidth), mainCamera.transform.position.y + cameraHalfHeight + halfHeight, newAsteroid.transform.position.z);
            // Enter from right of screen
            else
                newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x + cameraHalfWidth + halfWidth, Random.Range(mainCamera.transform.position.y, mainCamera.transform.position.y + cameraHalfHeight), newAsteroid.transform.position.z);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
