using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationStarter : MonoBehaviour
{
    public int animation = 21;
    private int prevAnimation = 21;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.SetInteger("animation",animation);
    }

    private void Update()
    {
        if (prevAnimation != animation)
        {
            prevAnimation = animation;
            _animator.SetInteger("animation",animation);

        }
    }
}
