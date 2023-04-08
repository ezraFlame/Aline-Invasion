using LDtkUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{

	protected LDtkFields data;
	protected Rigidbody2D rb;

	protected virtual void Start() {
		data = GetComponent<LDtkFields>();
		rb = GetComponent<Rigidbody2D>();
	}

	public virtual void Damage(int damage) {
		Health -= damage;
	}

	[SerializeField]
	private int health;
	public int Health {
		get => health;
		set {
			health = value;
			if (health > MaxHealth) health = MaxHealth;
			if (health < 0) health = 0;
		}
	}

	[SerializeField]
	private int maxHealth;
	public int MaxHealth { get => maxHealth; set => maxHealth = value; }
}
