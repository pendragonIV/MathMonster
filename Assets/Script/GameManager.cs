using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    public Transform resultContainer;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;

    private float time = 0;
    #endregion


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        SetupLevel();
    }

    private void SetupLevel()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        GridCellManager.instance.SetMap(map.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>());
        resultContainer = map.transform.GetChild(1);
        time = currentLevelData.timeLimit;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (isGameWin || isGameLose || isGamePause)
        {
            return;
        }
        time -= Time.deltaTime;
        gameScene.SetTime(currentLevelData.timeLimit, time);
        if (time <= 0)
        {
            Lose();
        }
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }

        isGameWin = true;

        StartCoroutine(WaitToWin());
        LevelManager.instance.levelData.SaveDataJSON();
    }

    public void ChangeResult(double result)
    {
        gameScene.SetResult(result);
    }

    public void Lose()
    {
        isGameLose = true;
        StartCoroutine(WaitToLose());
    }

    private IEnumerator WaitToLose()
    {
        yield return new WaitForSecondsRealtime(.5f);
        gameScene.ShowLosePanel();
    }

    private IEnumerator WaitToWin()
    {
        yield return new WaitForSecondsRealtime(.5f);
        gameScene.ShowWinPanel();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public void PauseGame()
    {
        isGamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePause = false;
        Time.timeScale = 1;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }

    public void CheckResult(int result)
    {
        foreach (Transform child in resultContainer)
        {
            if (child.GetComponent<ResultBlock>().GetNumber() == result)
            {
                GridCellManager.instance.RemovePlacedCell(GridCellManager.instance.GetObjCell(child.gameObject.transform.position));
                child.DOScale(0, .5f).OnComplete(() => Destroy(child.gameObject));
                return;
            }
        }
    }
}

