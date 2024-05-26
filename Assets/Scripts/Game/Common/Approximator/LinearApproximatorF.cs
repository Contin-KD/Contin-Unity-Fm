using UnityEngine;

public struct LinearApproximatorF
{
    public float Current { get; set; }
    public float Destination { get; set; }
    public float Speed { get; set; }


    public LinearApproximatorF(float speed)
    {
        Speed = speed;
        Current = default;
        Destination = default;
    }

    public void Reset()
    {
        Current = default;
        Destination = default;
    }

    public void Update(float deltaTime)
    {
        if (Mathf.Approximately(Current, Destination))
            return;

        Current = Mathf.MoveTowards(Current, Destination, Speed * deltaTime);
    }

    public void Immediate()
    {
        Current = Destination;
    }
}
