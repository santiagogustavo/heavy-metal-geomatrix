using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum RumblePattern {
    Constant,
    Pulse,
    Linear
}

public class Rumbler : MonoBehaviour {
    private RumblePattern activeRumblePattern;
    private float rumbleDuration;
    private float pulseDuration;
    private float lowA;
    private float lowStep;
    private float highA;
    private float highStep;
    private float rumbleStep;
    private bool isMotorActive = false;

    public void RumbleConstant(float low, float high, float duration) {
        activeRumblePattern = RumblePattern.Constant;
        lowA = low;
        highA = high;
        rumbleDuration = Time.time + duration;
    }

    public void RumblePulse(float low, float high, float burstTime, float duration) {
        activeRumblePattern = RumblePattern.Pulse;
        lowA = low;
        highA = high;
        rumbleStep = burstTime;
        pulseDuration = Time.time + burstTime;
        rumbleDuration = Time.time + duration;
        isMotorActive = true;
        GetGamepad()?.SetMotorSpeeds(lowA, highA);
    }

    public void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float duration) {
        activeRumblePattern = RumblePattern.Linear;
        lowA = lowStart;
        highA = highStart;
        lowStep = (lowEnd - lowStart) / duration;
        highStep = (highEnd - highStart) / duration;
        rumbleDuration = Time.time + duration;
    }

    public void StopRumble() {
        GetGamepad()?.SetMotorSpeeds(0, 0);
    }

    private Gamepad GetGamepad() {
        return Gamepad.current;
    }

    private void Update() {
        if (Time.time > rumbleDuration) {
            StopRumble();
            return;
        }

        Gamepad gamepad = GetGamepad();
        if (gamepad == null) {
            return;
        }

        switch (activeRumblePattern) {
            case RumblePattern.Constant:
                gamepad.SetMotorSpeeds(lowA, highA);
                break;
            case RumblePattern.Pulse:
                if (Time.time > pulseDuration) {
                    isMotorActive = !isMotorActive;
                    pulseDuration = Time.time + rumbleStep;
                    if (!isMotorActive) {
                        gamepad.SetMotorSpeeds(0, 0);
                    } else {
                        gamepad.SetMotorSpeeds(lowA, highA);
                    }
                }
                break;
            case RumblePattern.Linear:
                gamepad.SetMotorSpeeds(lowA, highA);
                lowA += (lowStep * Time.deltaTime);
                highA += (highStep * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    void OnDestroy() {
        StopAllCoroutines();
        StopRumble();
    }
}
