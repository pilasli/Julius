using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Starting method for fade animation
    public void FadeToLevel()
    {
        animator.SetTrigger("FadeOut");
    }
}
