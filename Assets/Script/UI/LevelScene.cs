using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;
    [SerializeField]
    private Transform bg;
    public Transform sceneTransition;

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
    }

    void Start()
    {
        PrepareLevels();
        bg.DOShakePosition(10f, 100f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1);
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    private void PrepareLevels()
    {
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableHolder();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableHolder();
            }

            holder.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
            holder.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
            holder.rotation = Quaternion.Euler(0, -90, 0);

            holder.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), .2f, false)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true)
                .SetDelay(.3f * i);
            holder.GetChild(0).GetComponent<CanvasGroup>().DOFade(1, .2f)
                .SetUpdate(true)
                .SetDelay(.2f * i);
            holder.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .2f)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true).SetDelay(.3f * i);

        }
    }

}
 