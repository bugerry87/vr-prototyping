using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRoll : MonoBehaviour
{
	public enum Axis
	{
		XY, XZ, YX, YZ, ZX, ZY
	}

	public Transform origin;
	public Axis axis;
	public Material material;

	private void Start()
	{
		if (!material)
		{
			var mr = GetComponent<MeshRenderer>();
			if (mr)
			{
				material = mr.material;
			}
		}
	}

	private void Update()
    {
        if (material)
		{
			var offset = origin.InverseTransformPoint(transform.position);
			switch (axis)
			{
				case Axis.XY:
					material.mainTextureOffset = new Vector2(offset.x, offset.y);
					break;
				case Axis.XZ:
					material.mainTextureOffset = new Vector2(offset.x, offset.z);
					break;
				case Axis.YX:
					material.mainTextureOffset = new Vector2(offset.y, offset.x);
					break;
				case Axis.YZ:
					material.mainTextureOffset = new Vector2(offset.y, offset.z);
					break;
				case Axis.ZX:
					material.mainTextureOffset = new Vector2(offset.z, offset.x);
					break;
				case Axis.ZY:
					material.mainTextureOffset = new Vector2(offset.z, offset.y);
					break;
			}
		}
    }
}
