using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire3DSound : MonoBehaviour
{
    AudioSource fire;

    // Start is called before the first frame update
    void Start()
    {
        fire = SoundManager.Instance.PlaySoundAtPointByName("Fire", transform.position, false);
        fire.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        if (fire != null)
        {
            fire.gameObject.SetActive(false);
        }
    }
}
