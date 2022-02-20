using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIConponent : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _coinCount;
    [SerializeField]
    private Transform _coinImage;
    [SerializeField]
    private float _deltaTremor;
    [SerializeField]
    private TMP_Text _cubeCount;

	private PlayerController _player;
	private Vector3 _startPosCoinImage;

	private void Start()
	{
		_player = FindObjectOfType<PlayerController>();
		_startPosCoinImage = new Vector3(_coinImage.position.x, _coinImage.position.y, _coinImage.position.z);
	}

	private void Update()
	{
		_coinCount.text = _player.CoinsCount.ToString();
		_cubeCount.text = $"{_player.GetCubesCount.ToString()}/{_player.GetMaxCountBlocks.ToString()}";
	}

	public void StartAnimateCoin()
	{
		StartCoroutine(AnimateCoin());
	}

	private IEnumerator AnimateCoin()
	{
		var i = 0.1f;
		while (i > 0f)
		{
			_coinImage.position = new Vector3((_coinImage.position.x + Random.Range(-_deltaTremor, _deltaTremor)), 
				(_coinImage.position.y + Random.Range(-_deltaTremor, _deltaTremor)), 
				_coinImage.position.z);

			i -= Time.deltaTime;
			yield return null;
		}
		_coinImage.position = _startPosCoinImage;
	}
}
