using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{

    public bool isFiring, alive = true;
    public AnimationCurve preFireBeamStrength;
    public AnimationCurve beamFireStength;

    public Material preFireMat, beamFireMat;

    public float beamFireTime, beamFireTotalTime, beamFireSpeed;

    private SpookyGameManager spookyGameManager;

    public GameObject beam;
    public ColidingBeam colidingBeam;
    public Light spotLight;
    public GameObject beamParticleSystemPrefab;




    // TUNE HERE 
    private float baseDamagePerSecond = 2.5f, baseSize = 0.25f, baseDuration = 1f;
    private float dpsLevelExponent = 2f, sizeLevelExponent = 2f, durationLevelExponent = 2f;






    public AudioSource fireSound, cooldownSound;
    public float fireSoundNormalVolume = 1f;
    public float cooldownSoundNormalVolume = 0.25f;
    public float fireSoundCooldownTime = 0f;
    public float fireSoundCooldownTotalTime = 0.25f;

    public EquipmentConfiguration equipmentConfiguration;


    // Start is called before the first frame update
    void Start()
    {
        spookyGameManager = GameObject.FindObjectOfType<SpookyGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) { return; }

        // Update weapon
        Upgrade u = spookyGameManager.GetComponent<UpgradeManager>().UpgradeOrZero(equipmentConfiguration);
        float damagePerSecond = baseDamagePerSecond * (Mathf.Pow(dpsLevelExponent, u.power));
        beamFireTotalTime = baseDuration * (Mathf.Pow(durationLevelExponent, u.speed));
        float size = baseSize * (Mathf.Pow(sizeLevelExponent, u.special));

        float strength;
        Material mat;
        Color spotColor;
        if (isFiring) {
            mat = beamFireMat;
            beamFireTime += Time.deltaTime * beamFireSpeed;
            strength = beamFireStength.Evaluate(beamFireTime / beamFireTotalTime);
            strength *= size;
            spotColor = new Color(0f, 0.5f, 1f);

            for (int i =  colidingBeam.attackables.Count - 1; i >= 0 ; i--)
            {
                Attackable item = colidingBeam.attackables[i];
                if (item != null) {
                    item.InflictDamage(damagePerSecond * Time.deltaTime, this);
                } else {
                    colidingBeam.attackables.RemoveAt(i);
                }
            }

            if (beamFireTime > (beamFireTotalTime * beamFireSpeed)) {
                isFiring = false;
                beamFireTime = 0;
                cooldownSound.Play();
            }
        } else {
            mat = preFireMat;
            strength = preFireBeamStrength.Evaluate(spookyGameManager.timerTime / spookyGameManager.timerTotalTime);
            spotColor = new Color(1f, 0f, 0f);

            if (fireSoundCooldownTime < fireSoundCooldownTotalTime) {
                fireSoundCooldownTime += Time.deltaTime;
                if (fireSoundCooldownTime >= fireSoundCooldownTotalTime) {
                    fireSound.Stop();
                } 
            }
            
            fireSound.volume = fireSoundNormalVolume * (1 - (fireSoundCooldownTime / fireSoundCooldownTotalTime));
            cooldownSound.volume = cooldownSoundNormalVolume * (fireSoundCooldownTime / fireSoundCooldownTotalTime);
        }

        beam.SetActive(strength > 0.005);

        beam.GetComponent<Renderer>().material = mat;
        beam.transform.localScale = new Vector3(strength, strength, 100);
        spotLight.color = spotColor;
        spotLight.spotAngle = strength * 100;
    }

    public void Fire() {
        if (!alive) { return; }
        isFiring = true;
        beamFireTime = 0;
        fireSound.Play();
        fireSound.volume = fireSoundNormalVolume;
        cooldownSound.volume = 0;
        fireSoundCooldownTime = 0;
    }

    public void HandlePlayerDeath() {
        alive = false;
        if(isFiring) {
            isFiring = false;
            beamFireTime = 0;
            cooldownSound.Play();
        }
        
    }
}
