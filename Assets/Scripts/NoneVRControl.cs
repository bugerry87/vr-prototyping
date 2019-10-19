using UnityEngine;
using GB.Attributes;

public class NoneVRControl : MonoBehaviour
{
	public float sensitivity = 1f;

	public Transform controller;

	new public Camera camera;

	public string depthAxis = "Mouse ScrollWheel";

	[SerializeField] private float depth = 30f;

	void Start()
	{
		if (!controller)
		{
			controller = transform;
		}

		if (!camera)
		{
			camera = Camera.main;
		}
	}

	void Update()
    {
		depth += Input.GetAxis(depthAxis) * sensitivity;
		var ray = camera.ScreenPointToRay(Input.mousePosition);
		var target = ray.origin + ray.direction * depth;

		if (controller)
		{
			controller.LookAt(target);
		}
    }
}
