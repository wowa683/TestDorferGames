using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Joystick _joystick;
	[SerializeField]
	private CharacterController _controller;
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private float _speed;
	[SerializeField]
	private float _gravity;
	[SerializeField]
	private GameObject _scythe;
	[SerializeField]
	private int _maxCountBlocks = 40;

	Vector3 moveDirection;
	bool isHarvesting;

	public List<GameObject> GrassCubes;
	public int CoinsCount { get; set; }
	public int GetCubesCount => GrassCubes.Count;
	public int GetMaxCountBlocks => _maxCountBlocks;

	private void Start()
	{
		GrassCubes = new List<GameObject>();
		CoinsCount = PlayerPrefs.GetInt("Coins");
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (isHarvesting) return;

		Vector2 direction = _joystick.direction;

		if (_controller.isGrounded)
		{
			moveDirection = new Vector3(direction.x, 0, direction.y);

			Quaternion targetRotation = moveDirection != Vector3.zero ? Quaternion.LookRotation(moveDirection) : transform.rotation;
			transform.rotation = targetRotation;

			moveDirection = moveDirection * _speed;
		}

		moveDirection.y = moveDirection.y - (_gravity * Time.deltaTime);
		_controller.Move(moveDirection * Time.deltaTime);

		_animator.SetFloat("speed", direction != Vector2.zero ? 1f : 0f);
	}

	public void Harvest()
	{
		if (isHarvesting) return;

		isHarvesting = true;
		_animator.SetBool("harvesting", isHarvesting);
		StartCoroutine(StopHarvesting());
	}

	private IEnumerator StopHarvesting()
	{
		yield return new WaitForSeconds(0.3f);
		_scythe.SetActive(true);
		yield return new WaitForSeconds(0.2f);

		_scythe.SetActive(false);
		_animator.SetBool("harvesting", false);
		isHarvesting = false;
	}

	public bool BackpackIsNotFull()
	{
		return GrassCubes.Count < _maxCountBlocks;
	}

	public void AddBlockToBackpack(GameObject block)
	{
		GrassCubes.Add(block);
		if (GrassCubes.Count > 1)
		{
			block.SetActive(false);
		}
	}
}
