using Astra;
using System;
using UnityEngine;

public class AngleTracker : MonoBehaviour {
    [SerializeField] private JointToTrack jointToTrack;

    public enum JointToTrack {
        LeftElbow,
        RightElbow,
        LeftKnee,
        RightKnee,
    }

    public static event EventHandler<OnAngleCalculatedEventArgs> OnAngleCalculated;

    public class OnAngleCalculatedEventArgs : EventArgs {
        public JointToTrack joint; public float angle;
    }

    private bool bodyStreamActive = false;

    private void Start() {
        BodyTrackingStarter.Instance.OnBodyStreamStarted += Instance_OnBodyStreamStarted;
    }

    private void Instance_OnBodyStreamStarted() {
        bodyStreamActive = true;
    }

    private float CalculateAngleAndInvokeEvent(Astra.Joint j1, Astra.Joint j2, Astra.Joint j3) {
        var j1Vector = new Vector3(j1.WorldPosition.X, j1.WorldPosition.Y, j1.WorldPosition.Z);
        var j2Vector = new Vector3(j2.WorldPosition.X, j2.WorldPosition.Y, j2.WorldPosition.Z);
        var j3Vector = new Vector3(j3.WorldPosition.X, j3.WorldPosition.Y, j3.WorldPosition.Z);

        float angle = Vector3.Angle(j1Vector-j2Vector, j3Vector-j2Vector);

        OnAngleCalculated?.Invoke(this, new OnAngleCalculatedEventArgs {
            joint = jointToTrack, angle = angle,
        });

        return angle;
    }

    private void Update() {
        if (bodyStreamActive) {
            Body body = AstraSDKManager.Instance.Bodies[0];

            if (body.Status == Astra.BodyStatus.Tracking) {
                var joints = body.Joints;

                switch (jointToTrack) {
                    case JointToTrack.LeftElbow:
                        CalculateAngleAndInvokeEvent(joints[2], joints[3], joints[4]);
                        break;
                    case JointToTrack.RightElbow:
                        CalculateAngleAndInvokeEvent(joints[5], joints[6], joints[7]);
                        break;
                    case JointToTrack.LeftKnee:
                        CalculateAngleAndInvokeEvent(joints[10], joints[11], joints[12]);
                        break;
                    case JointToTrack.RightKnee:
                        CalculateAngleAndInvokeEvent(joints[13], joints[14], joints[15]);
                        break;
                    default:
                        Debug.LogError("No joint selected");
                        break;
                }
            }
        }
    }
}