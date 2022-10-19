using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clarao : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSecondsRealtime(2f);
        Color c = gameObject.GetComponent<Image>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            gameObject.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(.1f); ;
        }
    }
}
