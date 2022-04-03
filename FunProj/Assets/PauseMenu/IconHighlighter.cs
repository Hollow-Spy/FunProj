using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconHighlighter : MonoBehaviour
{
    IEnumerator Scalecoroutine;
    Image image;
    [SerializeField] GameObject clickSFX, HoverSFX;

    private void Start()
    {
        image = GetComponentInChildren<Image>();

    }

    public void UnHighlightText()
    {
        image.color = Color.white;
        image.transform.localScale = new Vector3(1, 1, 1);
        StopCoroutine(Scalecoroutine);
    }
    public void Clicked()
    {

        Instantiate(clickSFX, transform.position, Quaternion.identity);
        Scalecoroutine = scalenumerator();
        StartCoroutine(Scalecoroutine);
    }

    public void HighLightText()
    {
        Instantiate(HoverSFX, transform.position, Quaternion.identity);
        image.color = Color.yellow;

        Scalecoroutine = scalenumerator();
        StartCoroutine(Scalecoroutine);

    }
    IEnumerator scalenumerator()
    {
        yield return null;
        while (image.transform.localScale.x > .8f)
        {
            yield return null;
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, new Vector3(.7f, .7f, .7f), 15 * Time.deltaTime);
        }

        while (image.transform.localScale.x < 1.2f)
        {
            yield return null;
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, new Vector3(1.3f, 1.3f, 1.3f), 35 * Time.deltaTime);
        }

    }

}
