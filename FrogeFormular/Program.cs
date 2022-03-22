using FrogeFormular.ReadCSV;
using FrogeFormular.Models;
using Microsoft.Data.Sqlite;
using LiteDB;


var _db = new LiteDatabase("FormData.db");
var dbset_FormData = _db.GetCollection<BaseEntity>("FormTable");

GetDataFromFile GDFF = new();

var list = GDFF.ParseFormularData();
dbset_FormData.InsertBulk(list);     //Bör rensa tabell om vi lägger in hel listan . 



foreach (var item in list)
{
    Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
}


List<BaseEntity> elbil = list.Where(x=> x.IsSpanishCar == true).ToList();

Console.WriteLine($"{elbil.Count}");


    // Get a collection (or create, if doesn't exist) 

    var results = dbset_FormData.Query()
      .Where(x => x.IsSpanishCar == true)
      .OrderBy(x => x.CarModels)
      .ToList();

    Console.WriteLine($"{results.Count}");
    foreach (var item in results)
    {
        Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
    }

