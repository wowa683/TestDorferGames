using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public RectTransform center;
	public RectTransform knob;
	public float range;
	public bool fixedJoystick;

	[HideInInspector]
	public Vector2 direction;

	Vector2 _start;
	bool _isOverPanel;
	bool _isShow;

	void Start()
	{
		ShowHide(false);
	}

	void Update()
	{
		Vector2 pos = Input.mousePosition;

		if (Input.GetMouseButtonDown(0) && _isOverPanel)
		{
			ShowHide(true);
			_start = pos;

			knob.position = pos;
			center.position = pos;
		}
		else if (Input.GetMouseButton(0) && _isShow)
		{
			knob.position = pos;
			knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);

			if (knob.position != Input.mousePosition && !fixedJoystick)
			{
				Vector3 outsideBoundsVector = Input.mousePosition - knob.position;
				center.position += outsideBoundsVector;
			}

			direction = (knob.position - center.position).normalized;
		}
		if (Input.GetMouseButtonUp(0) && _isShow)
		{
			ShowHide(false);
			direction = Vector2.zero;
		}
	}

	void ShowHide(bool state)
	{
		center.gameObject.SetActive(state);
		knob.gameObject.SetActive(state);
		_isShow = state;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_isOverPanel = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isOverPanel = false;
	}
}
