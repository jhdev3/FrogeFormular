using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogeFormular.Models
{
    internal class BaseEntity
    {

        //public int Id { get; set; } //Låta databasen skapa id eller skapa själva? 

        public int Age { get; set; }    

        public bool IsSpanishCar { get; set; }  

        public string CarModels { get; set; }    



    }
}
