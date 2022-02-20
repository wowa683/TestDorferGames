using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrassComponent : MonoBehaviour
{
    public float Speed = 5f;

    private Transform _backPack;
    private PlayerController _playerController;
    private bool _atBackpack;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _backPack = GameObject.FindGameObjectWithTag("Backpack").transform;

		StartCoroutine(WaitAndToBackPack(1f));

	}

    private IEnumerator WaitAndToBackPack(float sec)
	{
        yield return new WaitForSeconds(sec);
        ToBackpack();
	}

    private IEnumerator MoveToBackPack()
	{
        float accelerator = 1f;
        var endPos = Vector3.zero;

        while (transform.localPosition != endPos)
		{
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Speed * Time.deltaTime * accelerator);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Speed * Time.deltaTime * accelerator);
            accelerator++;
            yield return null;
        }

        _playerController.AddBlockToBackpack(gameObject);
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag != "Player") return;

        ToBackpack();
    }

    private void ToBackpack()
	{
        if (_playerController.BackpackIsNotFull() && !_atBackpack)
        {
            transform.parent = _backPack;
            _atBackpack = true;
            StartCoroutine(MoveToBackPack());
        }
    }
}
