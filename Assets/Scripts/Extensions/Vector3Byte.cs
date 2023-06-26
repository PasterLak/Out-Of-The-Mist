using System;
using UnityEngine;

public struct Vector3Byte
{
	public byte x;
	public byte y;
	public byte z;

	public Vector3Byte(byte x, byte y, byte z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3Byte(Vector3Int v)
	{
		this.x = (byte)v.x;
		this.y = (byte)v.y;
		this.z = (byte)v.z;
	}
	public Vector3Byte(Vector3 v)
	{
		this.x = (byte)v.x;
		this.y = (byte)v.y;
		this.z = (byte)v.z;
	}
	public static Vector3Byte Zero
	{
		get { return new Vector3Byte(0, 0, 0); }
	}

	public static Vector3Byte Right
	{
		get { return new Vector3Byte(1, 0, 0); }
	}
	public static Vector3Byte Forward
	{
		get { return new Vector3Byte(0, 0, 1); }
	}

	
	public static Vector3Byte Up
	{
		get { return new Vector3Byte(0, 1, 0); }
	}

	public static Vector3Byte operator + (Vector3Byte v1, Vector3Byte v2)
	{
		Vector3Byte result = new Vector3Byte();

		if (v1.x + v2.x <= Byte.MaxValue) result.x = (byte)(v1.x + v2.x);
		else result.x = Byte.MaxValue;

		if (v1.y + v2.y <= Byte.MaxValue) result.y = (byte)(v1.y + v2.y);
		else result.y = Byte.MaxValue;

		if (v1.z + v2.z <= Byte.MaxValue) result.z = (byte)(v1.z + v2.z);
		else result.z = Byte.MaxValue;

		return result;
	}

	public static Vector3Byte operator -(Vector3Byte v1, Vector3Byte v2)
	{
		Vector3Byte result = new Vector3Byte();

		if (v1.x - v2.x >= 0) result.x = (byte)(v1.x - v2.x);
		else result.x = 0;

		if (v1.y - v2.y >= 0) result.y = (byte)(v1.y - v2.y);
		else result.y = 0;

		if (v1.z - v2.z >= 0) result.z = (byte)(v1.z - v2.z);
		else result.z = 0;


		return result;
	}

	public static explicit operator Vector3Int(Vector3Byte v)
	{
		return new Vector3Int(v.x, v.y, v.z);
	}
	public static explicit operator Vector3Byte(Vector3Int v)
	{
		return new Vector3Byte(v);
	}

	public static explicit operator Vector3(Vector3Byte v)
	{
		return new Vector3(v.x, v.y, v.z);
	}
	public static explicit operator Vector3Byte(Vector3 v)
	{
		return new Vector3Byte(v);
	}

	public static ushort Distance(Vector3Byte a, Vector3Byte b)
	{
		return (ushort)Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
	}



	public override string ToString()
	{
		
		return string.Format("({0},{1},{2})", x, y, z);
	}


}