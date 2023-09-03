using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    Rigidbody rigidbody;
    public List<ParticleSystem> eyes_vfx = new List<ParticleSystem>();
    public ParticleSystem spawn_vfx;
    ParticleSystem weapon_trail;

    public List<GameObject> meshes = new List<GameObject>();

    public List<GameObject> skeletons = new List<GameObject>();

    private void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        weapon_trail = GetComponentInChildren<ParticleSystem>();
    }

    public void PushTowardsTarget()
    {
        rigidbody.AddForce(transform.forward * 50, ForceMode.VelocityChange);
    }

    public void PushBack()
    {
        rigidbody.AddForce(-transform.forward * 30, ForceMode.VelocityChange);
    }

    public void MakeVisible()
    {
        for (int i = 0; i < meshes.Count; i++)
        {
            meshes[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    public void ActivateEyes()
    {
        if (eyes_vfx != null)
        {
            for (int i = 0; i < eyes_vfx.Count; i++)
            {
                eyes_vfx[i].Play();
            }
        }
    }

    public void SpawnParticles()
    {
        spawn_vfx.Play();
    }
    public void MakeWeaponVisible()
    {
        MeshRenderer[] weaponMeshes = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < weaponMeshes.Length; i++)
        {
            weaponMeshes[i].enabled = true;
        }
    }

    public void ActivateWeaponTrail()
    {
        if (weapon_trail != null) weapon_trail.Play();
    }

    public void DeactivateWeaponTrail()
    {
        if (weapon_trail != null) weapon_trail.Stop();
    }

    public void SummonSkeletons()
    {
        for (int i = 0; i < skeletons.Count; i++)
        {
            skeletons[i].GetComponent<EnemyManager>().enabled = true;
            skeletons[i].GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}
