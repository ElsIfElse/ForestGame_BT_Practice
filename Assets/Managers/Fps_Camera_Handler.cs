using UnityEngine;
using Unity.Cinemachine;

public class Fps_Camera_Handler : MonoBehaviour
{
    CinemachineCamera fpsCamera;
    CinemachineBasicMultiChannelPerlin headBop;
    [Header("Head Bop Settings")]

    [Space]
    
    [Header("Stand")]
    [SerializeField] private float standHeadBopFrequency;
    [SerializeField] private float standHeadBopAmplitude;

    [Header("Walk")]
    [SerializeField] private float walkHeadBopFrequency;
    [SerializeField] private float walkHeadBopAmplitude;
    
    [Header("Run")]
    [SerializeField] private float runHeadBopFrequency;
    [SerializeField] private float runHeadBopAmplitude;
    void Start()
    {
        fpsCamera = gameObject.GetComponent<CinemachineCamera>();
        headBop = gameObject.GetComponent<CinemachineBasicMultiChannelPerlin>();

        headBop.FrequencyGain = walkHeadBopFrequency;
        headBop.AmplitudeGain = walkHeadBopAmplitude;
    }
    public void SetHeadBop_Stand()
    {
        headBop.FrequencyGain = standHeadBopFrequency;
        headBop.AmplitudeGain = standHeadBopAmplitude;
    }

    public void SetHeadBop_Walk()
    {
        headBop.FrequencyGain = walkHeadBopFrequency;
        headBop.AmplitudeGain = walkHeadBopAmplitude;
    }
    public void SetHeadBop_Run()
    {
        headBop.FrequencyGain = runHeadBopFrequency;
        headBop.AmplitudeGain = runHeadBopAmplitude;
    }
}
