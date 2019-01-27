using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルシーン
/// </summary>
public class TitleScene : MonoBehaviour
{
    private enum State
    {
        Title,
        Credit,
    }

    private enum CursorState
    {
        Start,
        Credit,
    }

    [SerializeField]
    private Image _start;

    [SerializeField]
    private Image _credit;

    [SerializeField]
    private GameObject _panelCredit;

    private bool _inputLeft;
    private bool _inputRight;

    private bool _inputLeftPrev;
    private bool _inputRightPrev;

    private const float ThresholdDown = 0.7f;
    private const float ThresholdUp = 0.3f;

    private State _state = State.Title;
    private CursorState _cursorState = CursorState.Start;

    private void Awake()
    {
        _start.color = Color.white;
        _credit.color = Color.gray;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("Title");
    }

    private void Update()
    {
        var input = Input.GetAxisRaw("Horizontal");

        _inputLeftPrev = _inputLeft;
        _inputRightPrev = _inputRight;

        if ( input > 0 )
        {
            if ( input >= ThresholdDown )
            {
                _inputRight = true;
            }
            else if ( input <= ThresholdUp )
            {
                _inputRight = false;
            }
        }
        else
        {
            input = -input;

            if ( input >= ThresholdDown )
            {
                _inputLeft = true;
            }
            else if ( input <= ThresholdUp )
            {
                _inputLeft = false;
            }
        }

        if(_state == State.Title )
        {
            if ( !_inputLeftPrev && _inputLeft )
            {
                OnPrev();
            }

            if ( !_inputRightPrev && _inputRight )
            {
                OnNext();
            }
        }

        if ( Input.GetButtonDown("Submit") )
        {
            OnDecide();
        }
    }

    private void OnNext()
    {
        Debug.Log("OnNext");
        SoundManager.Instance.PlaySE("Cursor");

        switch ( _cursorState )
        {
            case CursorState.Start:
                _start.color = Color.gray;
                _credit.color = Color.white;
                _cursorState = CursorState.Credit;
                break;

            case CursorState.Credit:
                _start.color = Color.white;
                _credit.color = Color.gray;
                _cursorState = CursorState.Start;
                break;
        }
    }

    private void OnPrev()
    {
        Debug.Log("OnPrev");
        SoundManager.Instance.PlaySE("Cursor");

        switch ( _cursorState )
        {
            case CursorState.Start:
                _start.color = Color.gray;
                _credit.color = Color.white;
                _cursorState = CursorState.Credit;
                break;

            case CursorState.Credit:
                _start.color = Color.white;
                _credit.color = Color.gray;
                _cursorState = CursorState.Start;
                break;
        }
    }

    private void OnDecide()
    {
        SoundManager.Instance.PlaySE("Decide");

        if ( _state == State.Credit )
        {
            _panelCredit.SetActive(false);
            _state = State.Title;
            return;
        }

        switch ( _cursorState )
        {
            case CursorState.Start:
                SceneChanger.Load("mainScene");
                break;

            case CursorState.Credit:
                _panelCredit.SetActive(true);
                _state = State.Credit;
                break;

        }
    }
}
