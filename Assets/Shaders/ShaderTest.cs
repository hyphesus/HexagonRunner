using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private WorldCurver worldCurver;
    [SerializeField] Material material;  
    [SerializeField] float scrollSpeed = 2;  
    [SerializeField] private float curvePower;

    void Update()
    {
        MoveTexture();
    }

    private void MoveTexture()
    {
        Vector2 offset = material.mainTextureOffset;
        offset.y += scrollSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;

        PlayerMove();
    }
    public void PlayerMove()
    {
        if (true)
        {
            Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            if (moveVector.x != 0)
            {
                curvePower = Mathf.Clamp(curvePower, -0.015f, 0.015f);
                curvePower += moveVector.x * 0.0001f;
                worldCurver.curveStrength = curvePower;
                Debug.Log(worldCurver.curveStrength);
            }
            else
            {
                return;
            }
        }
    }
}
