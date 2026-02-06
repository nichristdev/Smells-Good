using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipTrigger : MonoBehaviour
{
    [SerializeField] float WaitTime;
    [SerializeField] float BlipTime;
    [SerializeField] string SfxName;
    [SerializeField] Sprite ClipSprite;
    [SerializeField] Image ClipImage;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(WaitTime);
        ClipImage.sprite = ClipSprite;
        ClipImage.gameObject.SetActive(true);
        AudioManager.Instance.PlaySound(SfxName, 1, 1, false);
        yield return new WaitForSeconds(BlipTime);
        ClipImage.gameObject.SetActive(false);
    }
}
