namespace BoykisserBot.Math;

/* The amount of time an expedition should take is determined by the overall level of their party.
 *
 * The stronger the party, the less time it should take. However the time should not be too short
 * as to make the expedition trivial. If a player's party level is too low, the expedition will not be allowed to
 * start, thus when x = 0, y = MinTime.
 *
 * The time an expedition should take is modeled by a set of 3 equations roughly modeled by the following graph,
 * where the x-axis is the power of the party above the minimum required power and the y-axis is the time the
 * expedition should take.
 *
 * Note that this graph is annotated with abbreviations for key values.
 *  (MxT)
 * |-----(TDB)
 * |     \
 * |      \
 * |       \
 * |        \ (Slope is not directly calculated)
 * |         \
 * |          \
 * |           \
 * |       (TDE)-----
 *              (MnT)
 * Key:
 * TDB = Time Decrease Begin
 * TDE = Time Decrease End
 * MxT = Max Time
 * MnT = Min Time
 *
 *
 * Again, this is a very rough model. The actual equations are as follows:
 * 1. x < TDB: y = MxT
 * 2. TBD <= x <= TDE: y = MxT - ( ( ( MxT - MnT ) / ( TDE - TDB ) ) * ( x - TDB ) )
 * 3. x > TDE: y = MnT
 *
 * The actual values for these equations vary depending on the expedition rarity.
 *
 * The minimum power is simply the the power of the current expedition rarity + the power of the previous rarity.
 * (except for common, which is 1)
 */
internal class ExpeditionData(
    int minPower,
    TimeSpan minTime,
    TimeSpan maxTime,
    double timeDecreaseBegin,
    double timeDecreaseEnd)
{
    /// <summary>
    ///     The maximum time the expedition can take.
    /// </summary>
    private readonly TimeSpan _maxTime = maxTime;

    /// <summary>
    ///     The minimum time the expedition can take.
    /// </summary>
    private readonly TimeSpan _minTime = minTime;

    /// <summary>
    ///     The point at which the time starts decreasing.
    /// </summary>
    private readonly double _timeDecreaseBegin = timeDecreaseBegin;

    /// <summary>
    ///     The point at which the time stops decreasing.
    /// </summary>
    private readonly double _timeDecreaseEnd = timeDecreaseEnd;


    /// <summary>
    ///     The minimum power required to start the expedition.
    /// </summary>
    public readonly int MinPower = minPower;

    /// <summary>
    ///     Get the time an expedition should take based on the power of the party.
    /// </summary>
    /// <param name="power">Mean power level of the party</param>
    /// <returns></returns>
    public double GetTime(double power)
    {
        // Refer to the graph above for explanation.
        if (power < _timeDecreaseBegin) return _maxTime.TotalMinutes;
        if (power > _timeDecreaseEnd) return _minTime.TotalMinutes;
        return _maxTime.TotalMinutes - (
            _maxTime.TotalMinutes - _minTime.TotalMinutes
        ) / (
            _timeDecreaseEnd - _timeDecreaseBegin
        ) * (
            power - MinPower - _timeDecreaseBegin
        );
    }
}
