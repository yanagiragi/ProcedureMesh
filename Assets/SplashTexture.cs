using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Remain Bugs: Bugs when painting boundaries
 */

[RequireComponent(typeof(MeshCollider))]
public class SplashTexture : MonoBehaviour {

    [SerializeField]
    private Texture2D _brushTexture;
    [SerializeField]
    private Color _brushColor = Color.black;

    private Material _material;
    private Texture2D _texture;
    private RaycastHit _hit;
	
    void Awake ()
    {
        _material = GetComponent<MeshRenderer>().material;

        _texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        Color[] colors = _texture.GetPixels();
        for(int i = 0; i < colors.Length; ++i)
        {
            colors[i] = Color.clear;
        }
        _texture.SetPixels(colors);
        _texture.Apply();

        _material.SetTexture("_MainTex", _texture);
        _material.SetTexture("_DispTex", _texture);
	}

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                UpdateSplashTexture();
            }
        }
    }

    private void UpdateSplashTexture()
    {
        // Log Hit point's UV [Require MeshCollider]
        Debug.Log(_hit.textureCoord);

        Vector2 uv = _hit.textureCoord;

        uv.x *= _texture.width;
        uv.y *= _texture.height;

        int centerX = (int)uv.x;
        int centerY = (int)uv.y;

        int minX = centerX - _brushTexture.width / 2;
        int maxX = centerX + _brushTexture.width / 2;
        int minY = centerY - _brushTexture.height / 2;
        int maxY = centerY + _brushTexture.height / 2;

        minX = Mathf.Clamp(minX, 0, _texture.width);
        maxX = Mathf.Clamp(maxX, 0, _texture.width);
        minY = Mathf.Clamp(minY, 0, _texture.height);
        maxY = Mathf.Clamp(maxY, 0, _texture.height);

        Color brushPixel = Color.white;

        for(int x = minX; x < maxX; ++x)
        {
            for(int y = minY; y < maxY; ++y)
            {
                brushPixel = _brushTexture.GetPixel(x - centerX + _brushTexture.width / 2, y - centerY + _brushTexture.height / 2);
                if (brushPixel.a == 0)
                    continue;
                _texture.SetPixel(x, y, _brushColor);
            }
        }

        _texture.Apply();
    }
}
