using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet enemyRuntimeSet;
    [SerializeField] Animator screenFadeAnim;
    const int LAST_LEVEL = 5; //make this the build index of the first last level, not the level number
    private CoroutineHandle fadeCoroutine;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
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
        Scene activeScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetInt(activeScene.name, 1);
        int currentLevel = activeScene.buildIndex;

        if (!_deathCase)
        {
            if (currentLevel != LAST_LEVEL)
            {
                fadeCoroutine = Timing.RunCoroutine(FadeToLoad(1f, currentLevel + 1).CancelWith(gameObject));
            }
            else
            {
                //or whatever the index of the home page is
                fadeCoroutine = Timing.RunCoroutine(FadeToLoad(1f, 0).CancelWith(gameObject));
            }
        } 
        else
        {
            //restart the level if they died by reloading the same scene
            //(i think this works)
            fadeCoroutine = Timing.RunCoroutine(FadeToLoad(1f, currentLevel).CancelWith(gameObject));
        }
    }

    private IEnumerator<float> FadeToLoad(float _waitTime, int _sceneToLoad)
    {
        //start fade animation
        if(screenFadeAnim != null)
            screenFadeAnim.SetTrigger(AnimationHelper.FadeOutParameter);
        //wait then load scene
        yield return Timing.WaitForSeconds(_waitTime);
        Debug.Log("Fade out complete");
        SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Single);
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
}
