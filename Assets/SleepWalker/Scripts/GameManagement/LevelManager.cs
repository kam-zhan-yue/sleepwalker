using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Animator screenFadeAnim;
    [SerializeField] List<GameObject> activeEnemies;
    const int LAST_LEVEL = 5; //make this the build index of the first last level, not the level number
    float time = 0;

    // Start is called before the first frame update
    void Awake()
    {
        //set up what enemies are in the scene
        activeEnemies.Clear();
        GameObject[] active = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in active)
        {
            activeEnemies.Add(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //so it doesn't check every frame but every few seconds
        if (time >= 1)
        {
            Debug.Log("Checking for enemies...");
            foreach (GameObject enemy in activeEnemies)
            {
                float enemyHealth = enemy.GetComponent<Health>().GetHeathPercentage(); ;
                if (enemyHealth <= 0)
                {
                    RemoveEnemy(enemy);
                }
            }
            Debug.Log($"There are {activeEnemies.Count} enemies");
        }

        //load next level if no more enemies
        if (activeEnemies.Count <= 0)
        {
            Debug.Log("No enemies");
            LoadLevel();
        }
    }

    //removes enemy from active enemies list
    //should work from in this script but can be called externally for any reason, not static though
    public void RemoveEnemy(GameObject e)
    {
        activeEnemies.Remove(e);
    }
    
    //load the next build index or go to home page if game is over
    //can be called externally if we need a skip level hack or something
    public void LoadLevel(bool deathCase = false)
    {
        Debug.Log("Loading next level now");
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        IEnumerator coroutine;

        if (!deathCase)
        {
            if (currentLevel != LAST_LEVEL)
            {
                coroutine = FadeToLoad(1f, currentLevel + 1);
            }
            else
            {
                coroutine = FadeToLoad(1f, 0); //or whatever the index of the home page is
            }
        } else
        {
            //restart the level if they died by reloading the same scene
            //(i think this works)
            coroutine = FadeToLoad(1f, currentLevel);
        }

        StartCoroutine(coroutine);
    }

    IEnumerator FadeToLoad(float waitTime, int sceneToLoad)
    {
        //start fade animation
        screenFadeAnim.SetTrigger("FadeOut");
        //wait then load scene
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Fade out complete");
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
