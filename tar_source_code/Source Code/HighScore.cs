using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ez a toplista class, a pontokat kezeli.
/// </summary>
class HighScore : IComparable<HighScore>
{
    /// <summary>
    /// A pont
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// A pont tulajdonosának a neve
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Az időpont, amikor a pontot létrehozták
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// A toplista adatbázis azonosítója
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// A toplista konstruktora
    /// </summary>
    /// <param name="id">Adatbázis azonosító</param>
    /// <param name="score">A pont</param>
    /// <param name="name">A pont tulajdonosának a neve</param>
    /// <param name="date">Az időpont, amikor a pontot létrehozták</param>
    public HighScore(int id, int score, string name, DateTime date)
    {
        this.Score = score;
        this.Name = name;
        this.ID = id;
        this.Date = date;
    }

    /// <summary>
    /// A pontokat rendezi sorba a listában
    /// </summary>
    /// <param name="other">A pont, amit a többi ponthoz hasonlít</param>
    public int CompareTo(HighScore other)
    {

        if (other.Score < this.Score) // Ha a másik pont kisebb a jelenlegi pontnál
        {
            return -1;
        }
        else if (other.Score > this.Score) // Ha a másik pont nagyobb a jelenlegi pontnál
        {
            return 1;
        }
        else if (other.Date < this.Date) // Ha egyenlő a két pont, akkor a dátumot ellenőrzi
        {
            return -1;
        }
        else if (other.Date > this.Date) // Ha egyenlő a két pont, akkor a dátumot ellenőrzi
        {
            return 1; 
        }

        // 0-t ad vissza értéknek, ha a két dátum megegyezik
        return 0;
    }
}
