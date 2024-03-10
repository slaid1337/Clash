using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Background : Singletone<Background>
{
    private void Awake()
    {
        base.Awake();
    }

    public BoxCollider2D bounds => GetComponent<BoxCollider2D>();
}
