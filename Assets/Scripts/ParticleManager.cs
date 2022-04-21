using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject particleSystemPrefab;

    [SerializeField] bool testParticleSystem;


    private void Awake() {
        if (particleSystemPrefab == null) throw new MissingComponentException("Particle System Prefab not found");
    }

    private void Update() {
        if (testParticleSystem) {
            GenerateParticle(Vector2.zero, Color.white);
        }
        testParticleSystem = false;
    }

    public void GenerateFromGameObject(GameObject chip) {
        GenerateParticle(chip.transform.position, chip.GetComponent<SpriteRenderer>().color);
    }
    
    private void GenerateParticle(Vector2 location, Color color) {
        GameObject particleSystemGO = Instantiate(particleSystemPrefab, location, Quaternion.identity);
        ParticleSystem.MainModule main = particleSystemGO.GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }
}
