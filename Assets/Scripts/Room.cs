using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
	public GameObject virtualCamera;

	private void Start() {
		CinemachineConfiner2D confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
		confiner.m_BoundingShape2D = GetComponent<PolygonCollider2D>();

		CinemachineVirtualCamera cam = virtualCamera.GetComponent<CinemachineVirtualCamera>();
		cam.Follow = FindObjectOfType<PlayerController>().transform;

		var children = transform.GetComponentsInChildren<TilemapCollider2D>();

		foreach (var child in children)
		{
			child.gameObject.layer = 3;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && !other.isTrigger)
		{
			virtualCamera.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player") && !other.isTrigger)
		{
			virtualCamera.SetActive(false);
		}
	}
}
