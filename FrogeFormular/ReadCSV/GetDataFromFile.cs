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
                    int? age = null;

                    if (!string.IsNullOrEmpty(columns[1])) //Borde här bara kunna kolla efter string Empty men jaja
                    {
                        age = Convert.ToInt32(columns[1]);  
                    }

                    formData.Add(new BaseEntity
                    {

                        Age = age,
                        IsSpanishCar = isElbil,
                        CarModels = columns[3] != "" ? columns[3] : "Inte ifyllt fält.", //kollar för tom sträng
                    }) ; 
                }

            }
            return formData;    
        }
    }
 }
