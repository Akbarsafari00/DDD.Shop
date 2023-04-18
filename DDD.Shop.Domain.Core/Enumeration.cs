using System.Reflection;

namespace DDD.Shop.Domain.Core;

public abstract class Enumeration<T> : IEquatable<Enumeration<T>> where T : Enumeration<T>
{
    public string Name { get; }
    public int Value { get; }

    protected Enumeration(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll()
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Enumeration<T>);
    }

    public bool Equals(Enumeration<T> other)
    {
        return other != null && GetType() == other.GetType() && Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Value);
    }

    public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
    {
        return !Equals(left, right);
    }
}
