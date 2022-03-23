using FrogeFormular.ReadCSV;
using FrogeFormular.Models;
using Microsoft.Data.Sqlite;
using LiteDB;


var _db = new LiteDatabase("FormData.db");
var dbset_FormData = _db.GetCollection<BaseEntity>("FormTable");
var dbset_UniqueCars = _db.GetCollection<UniqueCars>("UniqueCars");
dbset_UniqueCars.EnsureIndex("CarName"); //Kommer att söka efter dem :) vid update :)


GetDataFromFile GDFF = new();

var list = GDFF.ParseFormularData();

dbset_FormData.InsertBulk(list);     //Bör rensa tabell om vi lägger in hel listan igen och igen eller så gör vi det inte och får bara fler rader i Databasen kommer va dubblerade men resultaten bör vara lika bara att antalet som svara på vårat formulär inte är 100% sant ;)=

Console.WriteLine("=====Test ReadFromDataFile=====");


foreach (var item in list)
{
    Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
}
List<BaseEntity> elbil = list.Where(x => x.IsSpanishCar == true).ToList();
Console.WriteLine($"Svara ja på elbil: {elbil.Count}");

Console.WriteLine("=====End Test ReadFromDataFile=====");

Console.WriteLine("=====Ålder=====");


var Oldest = dbset_FormData.Max(x => x.Age);
var Youngest = dbset_FormData.Min(x => x.Age);
var AvrageAge = dbset_FormData.Find(x => x.Age > 0).Select(x => x.Age).Average();//Om inte fältet är ifyllt blir 0 standard + är man 0 är inte statestiken relevant här kan vi även ändra till 18 osv.


Console.WriteLine($"Äldst: {Oldest} år , Yngst: {Youngest} år, Medelålder: {AvrageAge}");


Console.WriteLine("=====End Ålder=====");

// Get a collection (or create, if doesn't exist) 

var results = dbset_FormData.Query()
      .Where(x => x.IsSpanishCar == true)
      .OrderBy(x => x.CarModels)
      .ToList();

//Linq LiteDb har inte support direkt för GroupBy.
var g = results.GroupBy(x => x.CarModels)
        .Select(group => new UniqueCars { CarName = group.Key, Count = group.Count() })
        .OrderByDescending(x => x.Count);

Console.WriteLine($"{results.Count}");
Console.WriteLine("=====Alla Elibilar=====");
foreach (var item in results)
{
    Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
}
Console.WriteLine("=====Populäraset Elibilarna=====");
foreach (var item in g)
{
    Console.WriteLine($"{item.CarName} : {item.Count}");
}

/*Spara Bilarna i databasen i egen collection */
foreach (var item in g)
{
    var findCarname = dbset_UniqueCars.FindOne(i => i.CarName == item.CarName);

    // set document _id using id parameter
    Console.WriteLine();
    if(findCarname != null)
    {
        findCarname.Count = item.Count;
        dbset_UniqueCars.Update(findCarname);
    }
    else
    {
        var testr = dbset_UniqueCars.Insert(item);
    }
}
Console.WriteLine("=====Populäraset Elibilarna laddad från DB för test=====");

var test = dbset_UniqueCars.FindAll();
foreach( var item  in test)
{
    Console.WriteLine(item.CarName + " : " + item.Count);
}


var AllCars = dbset_FormData.FindAll()
    .GroupBy(x =>x.CarModels)
    .Select(group => new UniqueCars { CarName = group.Key, Count = group.Count() })
    .OrderByDescending(x => x.Count);


Console.WriteLine("=====Populäraset Bilarna=====");

foreach (var item in AllCars)
{
    Console.WriteLine(item.CarName + " : " + item.Count);
}
//var getModels = dbset_FormData.Query()
//     .Select(x => x.CarModels.Distinct())
//     .OrderBy(x => x.CarModels)
//     .ToList();

//Grouperar med med Carmodels 

