using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarkPaint : MonoBehaviour
{
    public int resolution = 512; //새로 만들어질 텍스쳐의 사이즈
    private Texture2D whiteMap; //텍스쳐를 새로 만들때 하얗게 하기 위해서 만든 변수

    public float brushSize; //칠 할 브러시의 크기
    public float brushRand; //칠 할 브러시의 크기 편차
    public Texture2D brushTexture; //칠 할 브러시의 모양
    private Vector2 stored;//현재 클릭한 위치의 uv를 확인

    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();

    void Start()
    {
        MakeWhiteTexture();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            Collider coll = hit.collider;
            if (coll != null && hit.transform.tag == "Floor")
            {
                if (!paintTextures.ContainsKey(coll))
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    paintTextures.Add(coll, GetWhiteRT());
                    rend.material.SetTexture("_PaintMap", paintTextures[coll]);
                }
                if (stored != hit.textureCoord)
                {
                    stored = hit.textureCoord;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.y *= resolution;
                    pixelUV.x *= resolution;
                    DrawTexture(paintTextures[coll], pixelUV.x, pixelUV.y);
                }

            }

        }
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {
        RenderTexture.active = rt; // 랜더텍스쳐를 준비하고..
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
        float t_Size = brushSize * Random.Range(1f - brushRand, 1f + brushRand);         // draw brushtexture
        Rect t_rect = new Rect(posX - brushTexture.width / t_Size, (rt.height - posY) - brushTexture.height / t_Size, brushTexture.width / (t_Size * 0.5f), brushTexture.height / (t_Size * 0.5f));
        Graphics.DrawTexture(t_rect, brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// 이런과정을 다 거쳐서 끝냅시다.
    }

    void MakeWhiteTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }

    RenderTexture GetWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }
}
