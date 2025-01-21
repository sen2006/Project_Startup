using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PseudoDictionary<T, U>
{
    // PSEUDODICTIONARY ENTRIES
    // & DICTIONARY CONVERSION

    [SerializeField] List<PseudoKeyValuePair<T, U>> entries;
    private Dictionary<T, U> actualDictionary = new();

    /// <summary>
    /// gets the total coult of entries in the dictionary
    /// </summary>
    public int Count
    {
        get
        {
            actualDictionary = FromPseudoDictionaryToActualDictionary();
            return actualDictionary.Count;
        }
    }

    // INDEXER

    public U this[T index]
    {
        get
        {
            actualDictionary = FromPseudoDictionaryToActualDictionary();
            return actualDictionary[index];
        }
    }

    // FROM DICTIONARY TO PSEUDO

    public List<PseudoKeyValuePair<T, U>> FromActualDictionaryToPseudoDictionary(Dictionary<T, U> actualDictionary)
    {
        List<PseudoKeyValuePair<T, U>> pseudoDictionary = new();

        foreach (KeyValuePair<T, U> pair in actualDictionary)
            pseudoDictionary.Add(new(pair.Key, pair.Value));

        return pseudoDictionary;
    }

    public List<PseudoKeyValuePair<T, U>> FromActualDictionaryToPseudoDictionary()
        => FromActualDictionaryToPseudoDictionary(actualDictionary);

    // FROM PSEUDO TO DICTIONARY

    public Dictionary<T, U> FromPseudoDictionaryToActualDictionary(List<PseudoKeyValuePair<T, U>> pseudoDictionary)
    {
        Dictionary<T, U> dictionary = new();

        foreach (PseudoKeyValuePair<T, U> entry in pseudoDictionary)
            dictionary.Add(entry.Key, entry.Value);

        return dictionary;
    }

    public Dictionary<T, U> FromPseudoDictionaryToActualDictionary()
        => FromPseudoDictionaryToActualDictionary(entries);

    // OPERATIONS

    public void Add(T key, U value)
    {
        actualDictionary = FromPseudoDictionaryToActualDictionary();
        actualDictionary.Add(key, value);
        entries = FromActualDictionaryToPseudoDictionary();
    }

    public void Remove(T key)
    {
        actualDictionary = FromPseudoDictionaryToActualDictionary();
        actualDictionary.Remove(key);
        entries = FromActualDictionaryToPseudoDictionary();
    }

    public void Clear()
    {
        actualDictionary.Clear();
        entries = new();
    }

    public U TryGetValue(T key)
    {
        TryGetValue(key, out U value);
        return value;
    }

    public bool TryGetValue(T key, out U value)
    {
        actualDictionary = FromPseudoDictionaryToActualDictionary();
        return actualDictionary.TryGetValue(key, out value);
    }

    public bool ContainsKey(T key)
    {
        actualDictionary = FromPseudoDictionaryToActualDictionary();
        return actualDictionary.ContainsKey(key);
    }

    public U[] GetValueArray()
    {
        return this.FromPseudoDictionaryToActualDictionary().Values.ToArray();
    }
    public T[] GetKeyArray()
    {
        return this.FromPseudoDictionaryToActualDictionary().Keys.ToArray();
    }

    public U[] Values
    {
        get
        { return GetValueArray(); }
    }

    public T[] Keys
    {
        get { return GetKeyArray(); }
    }

}

[System.Serializable]
public struct PseudoKeyValuePair<T, U>
{
    [SerializeField] T key;
    [SerializeField] U value;

    public T Key { get => key; }
    public U Value { get => value; }

    public PseudoKeyValuePair(T key, U value)
    {
        this.key = key;
        this.value = value;
    }
}