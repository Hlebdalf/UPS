using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurseGraph : MonoBehaviour
{   
    private struct vertex
    {
        public int x; public int y;
    }
    private const float shift = 1e-5f;
    private float x = 0;
    private Color color = Color.black;
    private Color erazeColor = Color.white;
    private List<vertex> erazer = new List<vertex>();
    private Texture2D texture;
    private int Height;
    private int Weight;
    public const int iterations = 100;
    public float A = 1;
    public float B = 0.5f;
    public int Price;
    void Start()
    {
        texture = gameObject.GetComponent<RawImage>().mainTexture as Texture2D;
        Height = texture.height;
        Weight = texture.width;
        StartCoroutine(GraphCoroutine());
    }
    private float Weierstrass(float x, float A, float B)
    {
        
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

    IEnumerator GraphCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            for (int i = 0; i < Weight * 100; i++) {
                
                float y = Weierstrass(x, A, B);           
                texture.SetPixel((int)x, (int)(y * 10 + Height / 2), color);
                vertex toEraze = new vertex { x = (int)x, y = (int)(y * 10 + Height / 2) };
                erazer.Add(toEraze);
                float UVX = gameObject.GetComponent<RawImage>().uvRect.x;
                gameObject.GetComponent<RawImage>().uvRect = new Rect(UVX + shift, 0, 1, 1);
                x += 0.01f;
                if ((int)(x * 100) % 1000 == 0)
                {
                    yield return null;
                }
                Price = (int)(y * 10 + Height / 2);
            }
            
            texture.Apply();
            yield return null;
            x -= 200;
            foreach (vertex i in erazer)
            {    
                texture.SetPixel(i.x, i.y, erazeColor);
            }
            erazer.Clear();
        }
    }

}


