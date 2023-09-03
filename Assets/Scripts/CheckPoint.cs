using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public BoxCollider collider;
    public ParticleSystem glow;
    public GameObject saveText;
    public GameObject light;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        glow = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DataPersistenceManager.instance.SaveGame();
            collider.enabled = false;
            saveText.SetActive(true);
            light.SetActive(false);
            glow.Stop();
            StartCoroutine(HideTextAndDeleteCheckpoint(5f));
        }
    }

    IEnumerator HideTextAndDeleteCheckpoint(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        saveText.SetActive(false);
        Destroy(gameObject);
    }
}
