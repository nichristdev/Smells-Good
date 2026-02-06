using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBar : MonoBehaviour
{
    [SerializeField] BlackBarType blackBarType;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        float aspectRatio = Camera.main.aspect;
        print(aspectRatio);

        if (aspectRatio > 1.7f && aspectRatio < 1.8f)
        {
            print("16 : 9");
            rectTransform.sizeDelta = new Vector2(240f, rectTransform.rect.height);

            switch (blackBarType)
            {
                case BlackBarType.Left:
                    rectTransform.anchoredPosition = new Vector2(240f / 2, rectTransform.anchoredPosition.y);
                    break;
                case BlackBarType.Right:
                    rectTransform.anchoredPosition = new Vector2(-240f / 2, rectTransform.anchoredPosition.y);
                    break;
            }
        }else if(aspectRatio > 1.9f && aspectRatio < 2.1f)
        {
            print("18 : 9");
            rectTransform.sizeDelta = new Vector2(320, rectTransform.rect.height);

            switch (blackBarType)
            {
                case BlackBarType.Left:
                    rectTransform.anchoredPosition = new Vector2(320 / 2, rectTransform.anchoredPosition.y);
                    break;
                case BlackBarType.Right:
                    rectTransform.anchoredPosition = new Vector2(-320 / 2, rectTransform.anchoredPosition.y);
                    break;
            }
        }
        else if (aspectRatio > 2.1f && aspectRatio < 2.2f)
        {
            print("19 : 9");
            rectTransform.sizeDelta = new Vector2(353.41f, rectTransform.rect.height);

            switch (blackBarType)
            {
                case BlackBarType.Left:
                    rectTransform.anchoredPosition = new Vector2(353.41f / 2, rectTransform.anchoredPosition.y);
                    break;
                case BlackBarType.Right:
                    rectTransform.anchoredPosition = new Vector2(-353.41f / 2, rectTransform.anchoredPosition.y);
                    break;
            }
        }
        else if (aspectRatio > 2.2f)
        {
            print("20 : 9");
            rectTransform.sizeDelta = new Vector2(384.25f, rectTransform.rect.height);

            switch (blackBarType)
            {
                case BlackBarType.Left:
                    rectTransform.anchoredPosition = new Vector2(384.25f / 2, rectTransform.anchoredPosition.y);
                    break;
                case BlackBarType.Right:
                    rectTransform.anchoredPosition = new Vector2(-384.25f / 2, rectTransform.anchoredPosition.y);
                    break;
            }
        }
    }

    private bool Approximately(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }
}

public enum BlackBarType
{
    Left,
    Right
}

