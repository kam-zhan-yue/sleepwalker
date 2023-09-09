using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet enemyRuntimeSet;
    [SerializeField] Animator screenFadeAnim;
    const int LAST_LEVEL = 5; //make this the build index of the first last level, not the level number

    // Start is called before the first frame update
    private void Awake()
    {
        // //set up what enemies are in the scene
        // activeEnemies.Clear();
        // GameObject[] active = GameObject.FindGameObjectsWithTag("Enemy");
        // foreach (GameObject o in active)
        // {
        //     activeEnemies.Add(o);
        // }
    }

    public void OnEnemyAdded(GameObject _enemy)
    {
        Debug.Log("Enemy Added!");
    }

    public void OnEnemyRemoved(GameObject _enemy)
    {
        Debug.Log($"Enemy Removed! Remaining Enemies: {enemyRuntimeSet.items.Count}");
        CheckEnemiesDead();
    }

    private void CheckEnemiesDead()
    {
        //load next level if no more enemies
        if (enemyRuntimeSet.items.Count <= 0)
        {
            Debug.Log("No enemies");
            LoadLevel();
        }
    }

    public void OnPlayerDead()
    {
        //TODO: Do something here?
    }

    //load the next build index or go to home page if game is over
    //can be called externally if we need a skip level hack or something
    private void LoadLevel(bool _deathCase = false)
    {
        Debug.Log("Loading next level now");
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        IEnumerator coroutine;

        if (!_deathCase)
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

    private IEnumerator FadeToLoad(float _waitTime, int _sceneToLoad)
    {
        //start fade animation
        screenFadeAnim.SetTrigger(AnimationHelper.FadeOutParameter);
        //wait then load scene
        yield return new WaitForSeconds(_waitTime);
        Debug.Log("Fade out complete");
        SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Single);
    }
}
