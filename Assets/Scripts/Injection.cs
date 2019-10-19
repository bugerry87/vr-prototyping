using UnityEngine;

public class Injection : MonoBehaviour
{
	public GameObject prefab;

	public Transform host;

	private GameObject instance;

    // Start is called before the first frame update
    private void Start()
    {
		instance = Instantiate(prefab, host);
    }

	private void OnDestroy()
	{
		Destroy(instance);
	}
}
