/*
 * Source: https://github.com/cdanek/KaimiraWeightedList/blob/main/WeightedList.cs
 *
 * MIT License
 *
 * Modified to fit project such as removing unused methods and properties.
 */


using System.Collections;

namespace BoykisserBot.Math;

/// <summary>
///     A single item for a list with matching T. Create one or more WeightedListItems, add to a Collection
///     and Add() to the WeightedList for a single calculation pass.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct WeightedListItem<T>(T item, int weight)
{
    internal readonly T Item = item;
    internal readonly int Weight = weight;
}

public class WeightedList<T> : IEnumerable<T>
{
    private readonly List<int> _alias = [];

    private readonly List<T> _list = [];
    private readonly List<int> _probabilities = [];
    private readonly Random _rand = new();
    private readonly List<int> _weights = [];
    private bool _areAllProbabilitiesIdentical;

    /// <summary>
    ///     Create a WeightedList with the provided items and an optional System.Random.
    /// </summary>
    public WeightedList(ICollection<WeightedListItem<T>> listItems)
    {
        foreach (WeightedListItem<T> item in listItems)
        {
            _list.Add(item.Item);
            _weights.Add(item.Weight);
        }

        Recalculate();
    }

    public int TotalWeight { get; private set; }

    /// <summary>
    ///     Minimum weight in the structure. 0 if Count == 0.
    /// </summary>
    public int MinWeight { get; private set; }

    /// <summary>
    ///     Maximum weight in the structure. 0 if Count == 0.
    /// </summary>
    public int MaxWeight { get; private set; }

    public IReadOnlyList<T> Items => _list.AsReadOnly();

    public T this[int index] => _list[index];

    public int Count => _list.Count;

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public T Next()
    {
        if (Count == 0) return default;
        int nextInt = _rand.Next(Count);
        if (_areAllProbabilitiesIdentical) return _list[nextInt];
        int nextProbability = _rand.Next(TotalWeight);
        return nextProbability < _probabilities[nextInt] ? _list[nextInt] : _list[_alias[nextInt]];
    }

    public void AddWeightToAll(int weight)
    {
        if (weight + MinWeight <= 0)
            throw new ArgumentException(
                $"Subtracting {-1 * weight} from all items would set weight to non-positive for at least one element.");
        for (int i = 0; i < Count; i++) _weights[i] = FixWeight(_weights[i] + weight);

        Recalculate();
    }

    public void SubtractWeightFromAll(int weight)
    {
        AddWeightToAll(weight * -1);
    }

    public void SetWeightOfAll(int weight)
    {
        if (weight <= 0) throw new ArgumentException("Weight cannot be non-positive.");
        for (int i = 0; i < Count; i++) _weights[i] = FixWeight(weight);
        Recalculate();
    }

    public void Add(T item, int weight)
    {
        _list.Add(item);
        _weights.Add(FixWeight(weight));
        Recalculate();
    }

    public void Add(ICollection<WeightedListItem<T>> listItems)
    {
        foreach (WeightedListItem<T> listItem in listItems)
        {
            _list.Add(listItem.Item);
            _weights.Add(FixWeight(listItem.Weight));
        }

        Recalculate();
    }

    public void Clear()
    {
        _list.Clear();
        _weights.Clear();
        Recalculate();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item, int weight)
    {
        _list.Insert(index, item);
        _weights.Insert(index, FixWeight(weight));
        Recalculate();
    }

    public void Remove(T item)
    {
        int index = IndexOf(item);
        RemoveAt(index);
        Recalculate();
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _weights.RemoveAt(index);
        Recalculate();
    }

    public void SetWeight(T item, int newWeight)
    {
        SetWeightAtIndex(IndexOf(item), FixWeight(newWeight));
    }

    public int GetWeightOf(T item)
    {
        return GetWeightAtIndex(IndexOf(item));
    }

    public void SetWeightAtIndex(int index, int newWeight)
    {
        _weights[index] = FixWeight(newWeight);
        Recalculate();
    }

    public int GetWeightAtIndex(int index)
    {
        return _weights[index];
    }

    /// <summary>
    ///     https://www.keithschwarz.com/darts-dice-coins/
    /// </summary>
    private void Recalculate()
    {
        TotalWeight = 0;
        _areAllProbabilitiesIdentical = false;
        MinWeight = 0;
        MaxWeight = 0;
        bool isFirst = true;

        _alias.Clear(); // STEP 1
        _probabilities.Clear(); // STEP 1

        List<int> scaledProbabilityNumerator = new(Count);
        List<int> small = new(Count); // STEP 2
        List<int> large = new(Count); // STEP 2
        foreach (int weight in _weights)
        {
            if (isFirst)
            {
                MinWeight = MaxWeight = weight;
                isFirst = false;
            }

            MinWeight = weight < MinWeight ? weight : MinWeight;
            MaxWeight = MaxWeight < weight ? weight : MaxWeight;
            TotalWeight += weight;
            scaledProbabilityNumerator.Add(weight * Count); // STEP 3
            _alias.Add(0);
            _probabilities.Add(0);
        }

        // Degenerate case, all probabilities are equal.
        if (MinWeight == MaxWeight)
        {
            _areAllProbabilitiesIdentical = true;
            return;
        }

        // STEP 4
        for (int i = 0; i < Count; i++)
            if (scaledProbabilityNumerator[i] < TotalWeight)
                small.Add(i);
            else
                large.Add(i);

        // STEP 5
        while (small.Count > 0 && large.Count > 0)
        {
            int l = small[^1]; // 5.1
            small.RemoveAt(small.Count - 1);
            int g = large[^1]; // 5.2
            large.RemoveAt(large.Count - 1);
            _probabilities[l] = scaledProbabilityNumerator[l]; // 5.3
            _alias[l] = g; // 5.4
            int tmp = scaledProbabilityNumerator[g] + scaledProbabilityNumerator[l] -
                      TotalWeight; // 5.5, even though using ints for this algorithm is stable
            scaledProbabilityNumerator[g] = tmp;
            if (tmp < TotalWeight)
                small.Add(g); // 5.6 the large is now in the small pile
            else
                large.Add(g); // 5.7 add the large back to the large pile
        }

        // STEP 6
        while (large.Count > 0)
        {
            int g = large[^1]; // 6.1
            large.RemoveAt(large.Count - 1);
            _probabilities[g] = TotalWeight; //6.1
        }

        // STEP 7 - Can't happen for this implementation but left in source to match Keith Schwarz's algorithm
#pragma warning disable S125 // Sections of code should not be commented out
        //while (small.Count > 0)
        //{
        //    int l = small[^1]; // 7.1
        //    small.RemoveAt(small.Count - 1);
        //    _probabilities[l] = _totalWeight;
        //}
#pragma warning restore S125 // Sections of code should not be commented out
    }

    // Throw an exception when adding a bad weight.
    private static int FixWeight(int weight)
    {
        return weight <= 0 ? throw new ArgumentException("Weight cannot be non-positive") : weight;
    }
}