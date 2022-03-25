using FrogeFormular.ReadCSV;
using FrogeFormular.Models;
using Microsoft.Data.Sqlite;
using LiteDB;


var _db = new LiteDatabase("FormData.db");
var dbset_FormData = _db.GetCollection<BaseEntity>("FormTable");
var dbset_UniqueSpanishCars = _db.GetCollection<UniqueCars>("UniqueSpanishCars"); //Lite dåligt namn då det är sorterat på El-bil.
dbset_UniqueSpanishCars.EnsureIndex("CarName"); //Kommer att söka efter dem :) vid update :)

/* Read from CSV fil */
GetDataFromFile GDFF = new();
var list = GDFF.ParseFormularData();

if (_db.CollectionExists("FormTable"))
{
    //Bör rensa tabell om vi lägger in hel listan igen och igen eller så gör vi det inte och får bara fler rader i Databasen kommer va dubblerade men resultaten blir densamma,  bara att antalet som svara på vårat formulär inte är 100% korrekt ;)
    _db.DropCollection("FormTable");
    dbset_FormData = _db.GetCollection<BaseEntity>("FormTable");
    dbset_FormData.InsertBulk(list);    
}


Console.WriteLine("\n===== Hur många deltog i undersökningen? =====");

var totalsvar = dbset_FormData.Count();   
Console.WriteLine("Antal enkätsvar:  " + totalsvar + "st");


Console.WriteLine("\n=====Ålder=====");


var Oldest = dbset_FormData.Max(x => x.Age);
var Youngest = dbset_FormData.Find(x=> x.Age != null).Min(x => x.Age); 
var AvrageAge = dbset_FormData.Find(x => x.Age > 0).Select(x => x.Age).Average();//Inte intresserade när de är 0 men skulle de vara 1 år kanske ett intresse finns för en https://www.jollyroom.se/leksaker/elbilar-elfordon/bilar/volvo-xc90-kinetic-elbil-svart ;) skulle även här kunna ha >18


Console.WriteLine($"Äldst: {Oldest} år , Yngst: {Youngest} år, Medelålder: {Math.Round((double)AvrageAge)} år."); //Avrundar Avrage blir snyggare vid tex. 2.5 rundar den ner till 2.


//Hämtar alla som svarade JA på spanish car. 
var results = dbset_FormData.Query()
      .Where(x => x.IsSpanishCar == true)
      .OrderBy(x => x.CarModels)
      .ToList();

//Linq fick inte LiteDb att lira med groupBy.
var g = results.GroupBy(x => x.CarModels)
        .Select(group => new UniqueCars { CarName = group.Key, Count = group.Count() })
        .OrderByDescending(x => x.Count);

Console.WriteLine($"\nHur många planerar köpa en elbil?: {results.Count}");

/*Spara Bilarna i databasen i egen collection för skojs skull och testa sql lite update - Det var därför som id finns i UniqueCars */
foreach (var item in g)
{
    var findCarname = dbset_UniqueSpanishCars.FindOne(i => i.CarName == item.CarName);

  
    if(findCarname != null)
    {
        findCarname.Count = item.Count;
        dbset_UniqueSpanishCars.Update(findCarname);
    }
    else
    {
        var testr = dbset_UniqueSpanishCars.Insert(item);
    }
}
Console.WriteLine("\n=====Populära bilmärken El-bil(spanskbil): =====");

var test = dbset_UniqueSpanishCars.FindAll();
foreach( var item  in test)
{
    Console.WriteLine(item.CarName + " : " + item.Count);
}


var AllCars = dbset_FormData.FindAll()
    .Where(x => !x.IsSpanishCar)
    .GroupBy(x =>x.CarModels)
    .Select(group => new UniqueCars { CarName = group.Key, Count = group.Count() })
    .OrderByDescending(x => x.Count);


Console.WriteLine("\n=====Populära bilmärken inte El-bil: =====");

foreach (var item in AllCars)
{
    Console.WriteLine(item.CarName + " : " + item.Count);
}

//Kan utgå efter Min,Max ålder får att ta reda på vilka ålderspann som är relevanta :)
Console.WriteLine("\n=====I vilken åldersspann är elbilar mest populär? =====");

int age20to30 = dbset_FormData.Count(x => x.IsSpanishCar && x.Age >= 20 && x.Age <= 30); 
Console.WriteLine("Antal personer mellan 20 till 30 som vill köpa elbil: " + age20to30);

int age30to40 = dbset_FormData.Count(x => x.IsSpanishCar && x.Age > 31 && x.Age <= 40);
Console.WriteLine("Antal personer mellan 31 till 40 som vill köpa elbil: " + age30to40);

int agePlus40 = dbset_FormData.Count(x => x.IsSpanishCar && x.Age > 41);
Console.WriteLine("Antal personer äldre än 41 som vill köpa elbil: " + agePlus40);

