using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    public Shader shader;
    Material _ppmat;

    [Range (0,1)]
    public float hit;

    private void Start()
    {
        _ppmat = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _ppmat);
    }

    private void Update()
    {
        _ppmat.SetFloat("_Hit", hit);
    }
    
}
