using UnityEngine;


public struct Distance
{

	public static float Space1D (float a, float b)
	{
		
		return (float)Mathf.Sqrt ( (a- b) *  (a - b) );

	}

	public static float Space2D (Vector2 a, Vector2 b)
	{
		return (float)Mathf.Sqrt ( (a.x - b.x) *  (a.x - b.x)  +  (a.y - b.y) *  (a.y - b.y) );

	}

	public static float Space3D (Vector3 a, Vector3 b)
	{
		return (float)Mathf.Sqrt ( (a.x - b.x) *  (a.x - b.x )  +  (a.y - b.y) *  (a.y - b.y) +  (a.z - b.z) *  (a.z - b.z) );

	}

	public static float Space1DSquared(float a, float b)
	{

		return (float)(a - b) * (a - b);

	}

	public static float Space2DSquared(Vector2 a, Vector2 b)
	{
		return (float)(a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);

	}

	public static float Space3DSquared(Vector3 a, Vector3 b)
	{
		return (float)(a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);

	}


}
