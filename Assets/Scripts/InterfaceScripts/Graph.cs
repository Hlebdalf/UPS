using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    private int s = 0;
    private float x = 0;
    private Color color = Color.black;
    private Texture2D texture;
    private int Height;
    public float A = 1;
    public float B = 0.5f;
    void Start()
    {
        texture = gameObject.GetComponent<RawImage>().mainTexture as Texture2D;
        Height = texture.height;
        int Weight = texture.width;
        StartCoroutine(TestCoroutine());
    }
    private float Weierstrass(float x, float A, float B)
    {
        const int iterations = 100;
        float total = 0;
        for (int n = 0; n < iterations; n++)
        {
            float cos = Mathf.Cos(Mathf.Pow(B, n) * Mathf.PI * x);
            if (cos > 1) cos = 0;
            else if (cos < -1) cos = 0;

            total += Mathf.Pow(A, n) * cos;
        }
        return (float)total;
    }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            float y = Weierstrass(x, A, B);
            texture.SetPixel((int)x, (int)(y * 10 + Height / 2), color);
            x += 0.05f;
            texture.Apply();

            print("1");
        }
    }
}


