using UnityEngine;

public readonly struct AnimationHash
{
    public readonly string name;
    public readonly int hash;

    public AnimationHash(string name)
    {
        this.name = name;
        hash = Animator.StringToHash(name);
    }

    public static bool operator ==(AnimationHash left, AnimationHash right)
    {
        return left.hash == right.hash;
    }

    public static bool operator !=(AnimationHash left, AnimationHash right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AnimationHash)obj);
    }

    public override int GetHashCode()
    {
        return hash;
    }
}
