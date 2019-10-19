using System;
using UnityEngine;
using UnityEngine.Events;
using GB.Attributes;

public class RayGrasp : MonoBehaviour
{
	[Serializable]
	public struct GraspRange
	{
		public float min;
		public float max;
	}

	[SerializeField] private Transform pivot;

	[SerializeField] private Transform offset;

	[SerializeField] private GraspRange range;

	[SerializeField] private Transform graspedObject;

	public LayerMask graspLayer;

	public LayerMask occlusionLayer;

	new public Camera camera;

	public string graspButton = "Fire1";

	[Header("Info")]

	[SerializeField] [ReadOnly] private float graspDepth;

	[SerializeField] [ReadOnly] private float graspDot;

	[SerializeField] [ReadOnly] private float graspUpperLog;

	[SerializeField] [ReadOnly] private float graspLowerLog;

	[SerializeField] [ReadOnly] private float currentDepth;

	[SerializeField] [ReadOnly] private float currentDot;

	[Header("Events")]

	public TransformEvent OnGrasp;

	public UnityEvent OnRelease;

	public float CurrentDepth
	{
		get
		{
			return currentDepth;
		}
		protected set
		{
			currentDepth = value;
		}
	}

	public float CurrentDot
	{
		get
		{
			return currentDot;
		}
		protected set
		{
			currentDot = value;
		}
	}

	public GraspRange Range
	{
		get
		{
			return range;
		}
		set
		{
			range.min = Mathf.Min(value.min, value.max);
			range.max = Mathf.Max(value.max, value.max);
		}
	}

	void Start()
	{
		if (!camera)
		{
			camera = Camera.main;
		}

		if (!pivot)
		{
			pivot = new GameObject("Pivot").transform;
			pivot.parent = transform;
		}

		if (!offset)
		{
			offset = new GameObject("Offset").transform;
			offset.parent = pivot;
		}
	}

	void Update()
	{
		if (Input.GetButtonDown(graspButton))
		{
			CheckGraspableObject();
		}
		else if (Input.GetButton(graspButton))
		{
			UpdatePivot();
		}
		else if (Input.GetButtonUp(graspButton))
		{
			OnRelease.Invoke();
			Reset();
		}
	}

	protected void CheckGraspableObject()
	{
		var ray = new Ray(
			transform.position + transform.forward * range.max,
			-transform.forward);

		var hits = Physics.RaycastAll(ray, range.max - range.min, graspLayer);
		Array.Sort(hits, FarestHit);

		if (hits.Length != 0)
		{
			var hit = hits[0];
			graspedObject = hit.transform;
			pivot.position = hit.point;
			offset.rotation = graspedObject.rotation;
			offset.position = graspedObject.position;
			graspDot = Vector3.Dot(transform.forward, camera.transform.forward);
			graspUpperLog = -Mathf.Log(1 - graspDot, 2);
			graspLowerLog = -Mathf.Log(graspDot, 2);
			graspDepth = range.max - hit.distance;
			OnGrasp.Invoke(graspedObject);
		}
	}

	protected void UpdatePivot()
	{
		if (graspedObject)
		{
			RaycastHit hit;
			var ray = new Ray(
				transform.position + transform.forward * range.min,
				transform.forward);

			var pos = pivot.localPosition;
			var dot = Vector3.Dot(transform.forward, camera.transform.forward);

			if (dot > 0f)
			{
				var upper = Mathf.Pow(dot, graspUpperLog);
				var lower = Mathf.Pow(1 - dot, graspLowerLog);
				var propos = range.max * (upper - lower + (graspDepth / range.max));
				pos.z = Mathf.Clamp(propos, range.min, range.max);
			}
			else
			{
				pos.z = range.min;
			}

			if (Physics.Raycast(ray, out hit, range.max - range.min, occlusionLayer))
			{
				pos.z = Mathf.Min(pos.z, hit.distance);
			}

			pivot.localPosition = pos;
			CurrentDepth = pos.z;
			CurrentDot = dot;

			graspedObject.position = offset.position;
			graspedObject.rotation = offset.rotation;
		}
	}

	public void Reset()
	{
		graspedObject = null;
		pivot.localPosition = new Vector3(0f, 0f, range.min);
		offset.localPosition = Vector3.zero;
		offset.localRotation = Quaternion.identity;
		graspDepth = 0f;
		graspDot = 0f;
	}

	public static int FarestHit(RaycastHit a, RaycastHit b)
	{
		return a.distance < b.distance ? 1
			: a.distance > b.distance ? -1
			: 0;
	}
}
