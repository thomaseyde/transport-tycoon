using System;

namespace TransportTycoon.Domain
{
    public class Option
    {
        public static readonly None None = None.Value;

        public static Option<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }
    }

    
    public abstract class Option<T>
    {
        public static implicit operator Option<T>(None v) => new None<T>();

        public static implicit operator Option<T>(T value) => new Some<T>(value);
        public abstract void MatchSome(Action<T> action);
        public abstract T Reduce(T whenNone);
        public abstract Option<TResult> Map<TResult>(Func<T, TResult> map);
        public abstract Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map);
        public abstract Option<T> Filter(Func<T, bool> predicate);
    }

    public class Some<T> : Option<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            _value = value;
        }

        public override void MatchSome(Action<T> action)
        {
            action(_value);
        }

        public override T Reduce(T whenNone)
        {
            return _value;
        }

        public override Option<TResult> Map<TResult>(Func<T, TResult> map)
        {   
            return map(_value);
        }

        public override Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map)
        {
            return map(_value);
        }

        public override Option<T> Filter(Func<T, bool> predicate)
        {
            if (predicate(_value))
            {
                return this;
            }

            return Option.None;
        }

        public override bool Equals(object obj)
        {
            if (obj is Some<T> some)
            {
                return _value.Equals(some._value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }

    public class None<T> : Option<T>
    {
        public override void MatchSome(Action<T> action)
        {
        }

        public override T Reduce(T whenNone)
        {
            return whenNone;
        }

        public override Option<TResult> Map<TResult>(Func<T, TResult> map)
        {
            return Option.None;
        }

        public override Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map)
        {
            return Option.None;
        }

        public override Option<T> Filter(Func<T, bool> predicate)
        {
            return Option.None;
        }

        public override bool Equals(object obj)
        {
            return obj is None || obj is None<T>;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    public sealed class None 
    {
        public override bool Equals(object obj)
        {
            return obj is None;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static readonly None Value = new None();
    }

}