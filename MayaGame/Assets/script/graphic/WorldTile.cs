using UnityEngine;
using System.Collections;

public enum TileAxis
{
    X,
    Y,
    Z
}

public class WorldTile : MonoBehaviour {
    public float worldLength=10f;
    public bool autoAxis = true;
    public TileAxis surfAxis;
    
	// Use this for initialization
	void Start () {
        Renderer render = GetComponent<Renderer>();
        Vector3 size = transform.localScale;
        Vector2 size2D = Vector3.one;
        if (autoAxis)
        {
            if (size.x < size.y)
            {
                if (size.x < size.z)
                {
                    surfAxis = TileAxis.X;
                }
                else
                {
                    surfAxis = TileAxis.Z;
                }
            }
            else
            {
                if (size.y < size.z)
                {
                    surfAxis = TileAxis.Y;
                }
                else
                {
                    surfAxis = TileAxis.Z;
                }
            }
        }

        switch (surfAxis)
        {
            case TileAxis.X:
                size2D = new Vector2(size.z, size.y);
                break;
            case TileAxis.Y:
                size2D = new Vector2(size.x, size.z);
                break;
            case TileAxis.Z:
                size2D = new Vector2(size.x, size.y);
                break;
        }
        size2D = size2D / worldLength;
        render.material.mainTextureScale = size2D;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
