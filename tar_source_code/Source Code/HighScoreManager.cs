using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Ez a szkript az adatbázissal való kapcsolatért felelős
/// </summary>
public class HighScoreManager : MonoBehaviour {

    /// <summary>
    /// A kapcsolat string, ez a string mutatja meg az adatbázis elérhetőségét
    /// </summary>
    private string connectionString;

    /// <summary>
    /// Ez a lista tartalmazza az összes pontot
    /// </summary>
    private List<HighScore> highScores = new List<HighScore>();

    /// <summary>
    /// Ez a prefab az új adatokért felelős
    /// </summary>
    public GameObject scorePrefab;

    /// <summary>
    /// Ez a szüleje az összes toplista objektumnak
    /// </summary>
    public Transform scoreParent;

    /// <summary>
    /// A maximálisan megmutatható összegek száma
    /// </summary>
    public int topRanks;

    /// <summary>
    /// Az adott pontmennyiség, amit az adatbázisban eltárolunk
    /// </summary>
    public int saveScores;

    /// <summary>
    /// Név bekérő segéd
    /// </summary>
    public TMP_InputField enterName;

    /// <summary>
    /// Átmeneti (placeholder) dialógus, mielőtt még a felhasználó nevet írna be
    /// </summary>
    public GameObject nameDialog;

	// Az alkalmazás vagy script meghívásakor lefutó kód
	void Start ()
    {
        //Megadjuk az .sqlite fájl elérhetőségi útvonalát, ami ebben az esetben az Assets mappán belül lesz
        connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";

        //Létrehozza az adatbázist, ha az nem létezne (Friss telepítés után gyakori)
        CreateTable();

        //Letörli a felesleges pontokat
        DeleteExtraScore();
        
        //Megmutatja a pontokat a táblázatban
        ShowScores();
	}
	
	// Minden egyes képkockával lefutó kód
	void Update ()
   {
        if (Input.GetKeyDown(KeyCode.Escape)) //Az ESC billentyű lenyomásával megjelenik és/vagy eltűnik a név bekérő ablak
        {
            nameDialog.SetActive(!nameDialog.activeSelf);
        }
	}

    /// <summary>
    /// Létrehozza az adatbázist, ha az nem létezne (Friss telepítés után gyakori)
    /// </summary>
    private void CreateTable()
    {
        //Létrehozza a kapcsolatot
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Megnyitja a kapcsolatot
            dbConnection.Open();

            //Létrehozza a parancsot, hogy lehessen interaktálni az adatbázissal
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                //SQLite lekérdező string 
                string sqlQuery = String.Format("CREATE TABLE if not exists HighScores (PlayerID INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Score INTEGER NOT NULL , Date DATETIME NOT NULL  DEFAULT CURRENT_DATE)");

                //Meghívja a parancsot
                dbCmd.CommandText = sqlQuery;

                //Lefuttatja a parancsot
                dbCmd.ExecuteScalar();

                //Bezárja a kapcsolatot
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Az "OK" gomb lenyomásával lefutó metódus
    /// </summary>
    public void EnterName()
    {

        if (enterName.text != string.Empty) //Ellenőrzi, hogy nem üres-e a mező
        {
            int score = UnityEngine.Random.Range(1, 101); //1-től 101-ig egy véletlenszerű pontot fog generálni

            InsertScore(enterName.text, score); //Beírja a megadott pontot az adatbázisba

            enterName.text = string.Empty; //Üríti a szövegmezőt

            ShowScores(); //Frissül az adatbázis és a megjelenítés is, kiírja a jelenlegi adatokat a táblázatba

        }
    }

    /// <summary>
    /// Beírja a megadott pontot az adatbázisba
    /// </summary>
    /// <param name="name">A játékos neve</param>
    /// <param name="newScore">A játékos pontja</param>
    private void InsertScore(string name, int newScore)
    {
        GetScores(); //Lekéri az adott pontokat az adatbázisból

        int hsCount = highScores.Count; //Eltárolja a pontok számát

        if (highScores.Count > 0) //Ha van már beírt adat
        {
            HighScore lowestScore = highScores[highScores.Count - 1]; //Egy referenciát készít a legalacsonyabb ponthoz

            //Ha a legalacsonyabb pont felülírásra kerül
            if (lowestScore != null && saveScores > 0 && highScores.Count >= saveScores && newScore > lowestScore.Score)
            {
                DeleteScore(lowestScore.ID); //Törli a legalacsonyabb pontot

                hsCount--; //A pontok mennyiségét csökkenti
            }
        }
        if (hsCount < saveScores) //Ha van még szabad hely a listán, akkor új pontokat fog rögzíteni az adatbázisban
        {
            //Létrehozza a kapcsolatot
            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
            {
                //Megnyitja a kapcsolatot
                dbConnection.Open();

                //Létrehozza a parancsot, hogy lehessen interaktálni az adatbázissal
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    //SQLite lekérdező string
                    string sqlQuery = String.Format("INSERT INTO HighScores(Name,Score) VALUES(\"{0}\",\"{1}\")", name, newScore);
                    
                    dbCmd.CommandText = sqlQuery; //Meghívja a parancsot
                    dbCmd.ExecuteScalar(); //Lefuttatja a parancsot
                    dbConnection.Close();//Bezárja a kapcsolatot


                }
            }
        }
    }

    /// <summary>
    /// Lekéri a pontokat az adatbázisból
    /// </summary>
    private void GetScores()
    {
        //Üríti a listát
        highScores.Clear();

        //Létrehozza a kapcsolatot
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Megnyitja a kapcsolatot
            dbConnection.Open();

            //Létrehozza a parancsot, hogy lehessen interaktálni az adatbázissal
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //Kiválaszt mindent az adatbázis táblából
                string sqlQuery = "SELECT * FROM HighScores";

                //Meghívja a parancsot
                dbCmd.CommandText = sqlQuery;

                // Létrehoz egy olvasót ami segítségével betöltheti a pontokat
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read()) //Ha van sor, amit be lehet olvasni
                    {
                        //Hozzáadja az új megadott pontot a listához
                        highScores.Add(new HighScore(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(1), reader.GetDateTime(3)));
                    }

                    //Bezárja a kapcsolatot
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        highScores.Sort(); //Csökkenő sorrendben sorba rendezi a pont listát
    }

    /// <summary>
    /// Kitöröl egy bizonyos sort az adatbázisból
    /// </summary>
    /// <param name="id">Adatbázis azonosító</param>
    private void DeleteScore(int id)
    {
        //Létrehozza a kapcsolatot
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open(); //Megnyitja a kapcsolatot

            //Létrehozza a parancsot, hogy lehessen interaktálni az adatbázissal
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //SQLite lekérdező string
                string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", id);

                //Meghívja a parancsot
                dbCmd.CommandText = sqlQuery;

                //Lefuttatja a parancsot
                dbCmd.ExecuteScalar();

                //Bezárja a kapcsolatot
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Megmutatja a pontokat a táblázatban
    /// </summary>
    private void ShowScores()
    {
        GetScores(); //Lekérdezi a pontokat az adatbázisból

        //Az összes pontot megvizsgálja
        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score"))
        {
            //Az előző pontokat eltünteti
            Destroy(score);
        }

        for (int i = 0; i < topRanks; i++) //Csak a megadott mennyiségű pontot láthatjuk
        {
            if (i <= highScores.Count - 1) //A tömbön kívüli index mutatást akadályozza meg
            {
                GameObject tmpObjec = Instantiate(scorePrefab); //Egy új eredményt fog megadni

                HighScore tmpScore = highScores[i]; //A jelenlegi eredményt fogja megmutatni

                //A helyezést növekedő sorrendben fogja kiírni egymás alá
                tmpObjec.GetComponent<HighScoreScript>().SetScore(tmpScore.Name, tmpScore.Score.ToString(), "#" + (i + 1).ToString());

                tmpObjec.transform.SetParent(scoreParent); //A pont szülejét állítja be

                tmpObjec.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); // Az objektum rendes skálázásáról gondoskodik (1 = natív méret)
            }

        }
    }

    /// <summary>
    /// Törli a már nem megjelenített pontokat
    /// </summary>
    private void DeleteExtraScore()
    {
        GetScores(); //Lekérdezi a pontokat az adatbázisból

        if (saveScores <= highScores.Count) //Ha a jelenlegi pontok száma kisebb, mint az eltárolható (megjeleníthető) mennyiség
        {
            int deleteCount = highScores.Count - saveScores; //Eltárolja a törölt pontok mennyiségét

            highScores.Reverse(); //Növekvő sorrendbe helyezi a listát, ezért könnyebb lesz törölni az alacsony pontokat

            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //Létrehozza a kapcsolatot
            {
                dbConnection.Open(); //Megnyitja a kapcsolatot

                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //Létrehozza a parancsot, hogy lehessen interaktálni az adatbázissal
                {
                    for (int i = 0; i < deleteCount; i++) //Kitörli a pontokat
                    {
                        //Az adatok törlésére szólgáló SQLite lekérdező string
                        string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", highScores[i].ID);

                        //Meghívja a parancsot
                        dbCmd.CommandText = sqlQuery;

                        dbCmd.ExecuteScalar(); //Lefuttatja a parancsot
                    }

                    dbConnection.Close(); //Bezárja a kapcsolatot
                }
            }
        }
    }
}
