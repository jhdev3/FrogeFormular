using FrogeFormular.ReadCSV;
using FrogeFormular.Models;
using Microsoft.Data.Sqlite;
using LiteDB;


var _db = new LiteDatabase("FormData.db");
var dbset_FormData = _db.GetCollection<BaseEntity>("FormTable");

GetDataFromFile GDFF = new();

var list = GDFF.ParseFormularData();
dbset_FormData.InsertBulk(list);     //Bör rensa tabell om vi lägger in hel listan . 

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

var NumberOfAgeEntries = dbset_FormData.Query().Where(x => x.Age > 0).Count();
var SumAges = dbset_FormData.Find(x => x.Age > 0).Select(x => x.Age).Sum();
//IEnumerable<int> numQuery1 =
//           from Age in dbset_FormData
//           where Age > 2
//           select Age;
////  db.Find().Sum()



Console.WriteLine($"Äldst: {Oldest} år , Yngst: {Youngest} år, Medelålder: {SumAges/NumberOfAgeEntries}");



Console.WriteLine("=====End Ålder=====");

// Get a collection (or create, if doesn't exist) 

var results = dbset_FormData.Query()
      .Where(x => x.IsSpanishCar == true)
      .OrderBy(x => x.CarModels)
      .ToList();


var g = results.GroupBy(x => x.CarModels)
        .OrderBy(group => group.Key)
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



//var getModels = dbset_FormData.Query()
//     .Select(x => x.CarModels.Distinct())
//     .OrderBy(x => x.CarModels)
//     .ToList();

//Grouperar med med Carmodels 

