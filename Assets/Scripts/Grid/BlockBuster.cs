using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockBuster : MonoBehaviour
{
    // [SerializeField] OnBlockInteract[] _blocks;
    [SerializeField] LineCheker[] _check;
    [SerializeField] private ScorePopup scorePopup;

    private Coroutine _clearCoroutine;
    private int _lastScore;

    void OnEnable()
    {
        GameEventBus.Subscribe<GridUpdateEvent>(NullAct);
    }
    void OnDisable()
    {
        GameEventBus.Unsubscribe<GridUpdateEvent>(NullAct);
    }
    void NullAct(GridUpdateEvent evt)
    {
        ClearLineAction();
    }



    public void ClearLineAction()
    {
        int lineCount = FullLine();
        if (lineCount == 0) return;

        //라인 클리어 전 점수 저장
        _lastScore = ScoreSystem.Instance.Score;

        GameEventBus.Raise(new LineClearedEvent(lineCount));

        //라인 클리어 후 증가한 점수
        int gainedScore = ScoreSystem.Instance.Score - _lastScore;

        if(gainedScore <= 0) gainedScore = (lineCount * lineCount) * 100; // 기본 점수 보정

        LineClear(gainedScore);

        GameEventBus.Raise(new GridUpdateEvent());
    }

    int FullLine()
    {
        int _line = 0;
        foreach (LineCheker _l in _check)
        {
            if (_l.IsLineFull()) _line++;
        }
        return _line;
    }


    void LineClear(int gainedScore)
    {
        Vector3 sumPos = Vector3.zero;
        int count = 0;

        foreach (LineCheker l in _check)
        {
            if (!l.IsLineFull()) continue;

            foreach (GridTile tile in l._tielOnLine)
            {
                sumPos += tile.transform.position;
                count++;
            }
        }

        if (count > 0 && scorePopup != null)
        {
            Vector3 centerPos = sumPos / count;
            scorePopup.Show(gainedScore, centerPos);
        }

        // 실제 블록 제거
        foreach (LineCheker l in _check)
        {
            if (!l.IsLineFull()) continue;

            foreach (GridTile tile in l._tielOnLine)
            {
                tile.GetComponent<OnBlockInteract>().ClearBlock();
            }
        }
    }

}
