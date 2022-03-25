using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogeFormular.Models
{
    /// <summary>
    /// Data från fråge formulär, till dessa data typer som ska sparas i en databas.
    /// </summary>
    internal class BaseEntity
    {

        //public int Id { get; set; } //Låta databasen skapa id eller skapa själva? 

        public int? Age { get; set; }    

        public bool IsSpanishCar { get; set; }  //Obligatoriskt att svara på I formuläret

        public string? CarModels { get; set; }//är inte ombligatoriskt att fylla i.    

    }
}
