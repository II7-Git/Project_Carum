using System;
using UnityEngine;

namespace Carum.UI
{
    [RequireComponent(typeof(Animator))]
    public class SceneCurtainScript : MonoBehaviour
    {
        public bool autoClear = true;
        private Animator _animator;
        // Start is called before the first frame update
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            if (autoClear)
                WhiteSceneAnimation();
        }


        public void WhiteSceneAnimation()
        {
            _animator.SetTrigger("whiteTrigger");
            _animator.ResetTrigger("blackTrigger");
        }
        public void BlackSceneAnimation()
        {
            _animator.SetTrigger("blackTrigger");
            _animator.ResetTrigger("whiteTrigger");

        }
    }
}
