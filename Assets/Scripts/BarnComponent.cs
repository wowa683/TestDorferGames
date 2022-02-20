using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnComponent : MonoBehaviour
{
	[SerializeField]
	private float _waitTime = 0.2f;
	[SerializeField]
	private float _speed = 5f;
	[SerializeField]
	private GameObject _coinPrefab;
	[SerializeField]
	private Transform _coinTarget;
	[SerializeField]
	private Transform _coinStartPos;
	[SerializeField]
	private int _coinPrice;

	private PlayerController _player;
	private UIConponent _canvas;

	private void Start()
	{
		_canvas = FindObjectOfType<UIConponent>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player") return;

		if (_player == null)
		{
			_player = other.GetComponent<PlayerController>();
		}

		if (_player != null)
		{
			if (_player.GrassCubes.Count == 0) return;

			StartCoroutine(GiveAllCubes());
		}
	}

	private IEnumerator GiveAllCubes()
	{
		for (int i = 0; i < _player.GrassCubes.Count;)
		{
			if (_player.GrassCubes[i] == null) continue;

			_player.GrassCubes[i].gameObject.SetActive(true);
			StartCoroutine(GiveCube(_player.GrassCubes[i].GetComponent<CubeGrassComponent>()));

			Destroy(_player.GrassCubes[i], 1.5f);
			_player.GrassCubes.Remove(_player.GrassCubes[i]);

			yield return new WaitForSeconds(_waitTime);
		}
	}

	private IEnumerator GiveCube(CubeGrassComponent cube)
	{
		float accelerator = 1f;
		while (cube != null && cube.transform.position != transform.position)
		{
			cube.transform.position = Vector3.Lerp(cube.transform.position, transform.position, _speed * Time.deltaTime * accelerator);

			accelerator += 1f;
			yield return null;
		}

		//yield return new WaitForSeconds(_waitTime);

		var coin = Instantiate(_coinPrefab);
		coin.transform.SetParent(_canvas.transform);
		coin.transform.position = _coinStartPos.position;
		accelerator = 1f;
		while (coin != null && coin.transform.position != _coinTarget.position)
		{
			coin.transform.position = Vector3.Lerp(coin.transform.position, _coinTarget.position, _speed * Time.deltaTime * accelerator);

			accelerator += 1f;
			yield return null;
		}
		Destroy(coin);
		_canvas.StartAnimateCoin();

		for (int i = 0; i < _coinPrice; i++)
		{
			_player.CoinsCount++;
			yield return new WaitForSeconds(_waitTime);
		}

		PlayerPrefs.SetInt("Coins", _player.CoinsCount);
	}
}
