


using FrogeFormular.ReadCSV;
using FrogeFormular.Models;
using Microsoft.Data.Sqlite;
using LiteDB;

GetDataFromFile GDFF = new();

var list = GDFF.ParseFormularData();

foreach (var item in list)
{
    Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
}


List<BaseEntity> elbil = list.Where(x=> x.IsSpanishCar == true).ToList();

Console.WriteLine($"{elbil.Count}");


using (var db = new LiteDatabase("FormData.db"))
{
    // Get a collection (or create, if doesn't exist) 
    var dbset_FormData = db.GetCollection<BaseEntity>("FormTable"); //Bör rensa tabell om vi lägger in hel listan . 




    // Insert new customer document (Id will be auto-incremented)
    dbset_FormData.InsertBulk(list);

    var results = dbset_FormData.Query()
      .Where(x => x.IsSpanishCar == true)
      .OrderBy(x => x.CarModels)
      .ToList();

    Console.WriteLine($"{results.Count}");
    foreach (var item in results)
    {
        Console.WriteLine($"{item.Age} : {item.IsSpanishCar} : {item.CarModels}");
    }

}
