using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    private float t = 0f;
    [SerializeField] private RawImage rawImage;

    private void Awake()
    {
        if (rawImage == null) { rawImage = GetComponent<RawImage>(); }
    }

    private void Update()
    {
        if (t <= 1.5f)
        {
            rawImage.color = new Color(0f, 0f, 0f,
            Mathf.Lerp(0f, 1f, t));

            t += Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}
