using System.Collections;
using System.Collections.Generic;

public class BiDict<TFirst, TSecond>
{
    private Dictionary<TFirst, TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
    private Dictionary<TSecond, TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();

    public void Add(TFirst first, TSecond second)
    {
        firstToSecond.Add(first, second);
        if(second != null)
            secondToFirst.Add(second, first);
    }

    public bool TryGetValue(TFirst first, out TSecond second)
    {
        firstToSecond.TryGetValue(first, out second);
        return second != null ? true : false ;
    }

    public bool TryGetValue(TSecond second, out TFirst first)
    {
        return secondToFirst.TryGetValue(second, out first);
    }
}
