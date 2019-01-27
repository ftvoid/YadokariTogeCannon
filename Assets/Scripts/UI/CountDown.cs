using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDown : MonoBehaviour
{
    //カウントダウン画像、0から順番に3,2,1,GO
    [SerializeField] Sprite[] countDownImages;

    [SerializeField] Vector2 numberSize;
    [SerializeField] Vector2 textSize;

    //画像コンポーネント
    Image image;
    //サイズ変更用
    RectTransform rect;

    //少しずつ透明になっていくフラグ
    bool alphaChange = false;
    //↑の処理用,alpha値
    float alpha;
    //透明になる早さ
    [SerializeField] float alphaSpeed = 0.01f;

    private UnityAction _onComplete;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        rect = gameObject.GetComponent<RectTransform>();
        //開始時は透明化
        image.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //少しずつ透明になる処理
        if(alphaChange)
        {
            alpha -= alphaSpeed;
            image.color = new Color(1, 1, 1, alpha);
            //透明化した場合処理を終了
            if(alpha <= 0)
            {
                alphaChange = false;
                image.color = new Color(1, 1, 1, 0);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCountDown();
        }
    }

    /// <summary>
    /// カウントダウンを開始
    /// </summary>
    public void StartCountDown(UnityAction onComplete = null)
    {
        _onComplete = onComplete;
        image.color = new Color(1, 1, 1, 1);
        rect.sizeDelta = numberSize;
        StartCoroutine("Count");
    }

    private IEnumerator Count()
    {
        //3→2→1の順に画像を切り替え
        for(int i = 0; i < countDownImages.Length - 1;i++)
        {
            image.sprite = countDownImages[i];
            yield return new WaitForSeconds(1.0f);
        }

        //GOの画像を表示と同時にゲーム開始
        rect.sizeDelta = textSize;
        image.sprite = countDownImages[countDownImages.Length - 1];
        _onComplete?.Invoke();
        alphaChange = true;
        alpha = 1;
    }
}
