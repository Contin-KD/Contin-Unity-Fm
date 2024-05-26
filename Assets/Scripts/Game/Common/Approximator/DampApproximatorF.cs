using UnityEngine;

public struct DampApproximatorF
{
    public float Current { get; private set; }
    public float Destination { get; set; }
    public float Duration { get; set; }

    private float velocity;

    public DampApproximatorF(float duration)
    {
        Current = default;
        Destination = default;
        Duration = duration;
        velocity = default;
    }

    public void Reset()
    {
        Current = default;
        Destination = default;
        velocity = default;
    }

    public void Update()
    {
        if (Mathf.Approximately(Current, Destination))
            return;

        Current = Mathf.SmoothDamp(Current, Destination, ref velocity, Duration);
    }

    public void Immediate()
    {
        Current = Destination;
        velocity = default;
    }
}
