using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Linq;

public class GrassComponent : MonoBehaviour
{
	[SerializeField]
	private GrassBlockComponent[] _grassBlocks;
	[SerializeField]
	private Material _material;
	[SerializeField]
	private Transform _plane;
	[SerializeField]
	private float _waitSeconds;
	[SerializeField]
	private GameObject _prefabCubeGrass;

	private List<GameObject> _lowGrass;

	private void Start()
	{
		_lowGrass = new List<GameObject>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Scythe") return;
		if (_grassBlocks.Where(x => !x.IsCut).ToList().Count == 0) return;

		int i = 0;
		foreach (var item in _grassBlocks)
		{
			if (item != null && !item.IsCut)
			{
				SlicedHull sh = item.gameObject.Slice(_plane.transform.position, _plane.transform.up, _material);
				GameObject low = sh.CreateLowerHull(item.gameObject, _material);
				_lowGrass.Add(low);
				low.transform.position = item.transform.position;
				low.transform.parent = transform;
				item.IsCut = true;
				item.gameObject.SetActive(false);
				i++;
			}
			if (i == 3) break;
		}

		CheckCutAll();
	}

	private void CheckCutAll()
	{
		if (_grassBlocks.Where(x => !x.IsCut).ToList().Count != 0) return;

		var cubeGrass = Instantiate(_prefabCubeGrass, new Vector3(transform.position.x, 0.7f, transform.position.z), Quaternion.Euler(0f, 0f, 0f));

		StartCoroutine(ResetBlocks());
	}

	private IEnumerator ResetBlocks()
	{
		yield return new WaitForSeconds(_waitSeconds);

		foreach (var item in _lowGrass)
		{
			Destroy(item);
		}
		_lowGrass.Clear();

		foreach (var item in _grassBlocks)
		{
			item.IsCut = false;
			item.gameObject.SetActive(true);
		}
	}
}
