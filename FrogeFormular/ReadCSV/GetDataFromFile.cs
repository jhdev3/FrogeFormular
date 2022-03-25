using FrogeFormular.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogeFormular.ReadCSV
{
    internal class GetDataFromFile
    {
        private string FileName = @"formularData.csv";


       public List<BaseEntity> ParseFormularData()
        {
            var formData = new List<BaseEntity>();
            using (var reader = new TextFieldParser(FileName))
            {
                reader.SetDelimiters(new string[] { "," });

                reader.ReadLine(); // Läs och glöm bort första raden

                while (!reader.EndOfData)
                {
                    string[] columns = reader.ReadFields();

                    bool isElbil = false;
                    if (columns[2] == "Spansk bil  (El-bil)") 
                    { 
                        isElbil = true; //Alltid markerad i formuläret behöver inte kolla för null
                    }      

                    formData.Add(new BaseEntity
                    {
                        Age = Convert.ToInt32(columns[1]),
                        IsSpanishCar = isElbil,
                        CarModels = columns[3]
                    }); ;
                }

            }
            return formData;    
        }
    }
 }


    //public int Age { get; set; }

    //public bool IsSpanishCar { get; set; }

    //public string? CarModels { get; set; }//är inte ombligatoriskt att fylla i.    

// "Tidstämpel","Hur gammal är du ?  ","Bil typ?","Vilket bil märke?"
//"2022/03/22 8:53:03 fm CET","45","Inte Elbil","Volvo "
//"2022/03/22 8:53:29 fm CET","23","Inte Elbil","Opel"
//"2022/03/22 8:54:23 fm CET","33","Spansk bil  (El-bil)","volkswagen"
//"2022/03/22 8:54:59 fm CET","35","Spansk bil  (El-bil)","Cykel"
//"2022/03/22 8:56:31 fm CET","26","Inte Elbil","Koenigsegg"
//"2022/03/22 9:09:17 fm CET","33","Spansk bil  (El-bil)","Tesla"
//"2022/03/22 9:13:22 fm CET","38","Spansk bil  (El-bil)","Tesla"
//"2022/03/22 9:46:54 fm CET","25","Spansk bil  (El-bil)","Tesla"


//List<int> ages = new List<int>();

//using (var reader = new TextFieldParser(@"oscar_age_female.csv"))
//{
//    reader.SetDelimiters(new string[] { "," });

//    reader.ReadLine(); // Läs och glöm bort första raden

//    while (!reader.EndOfData)
//    {
//        string[] columns = reader.ReadFields();

//        ages.Add(int.Parse(columns[2]));

//        //Console.WriteLine(columns[3]);
//    }
//}