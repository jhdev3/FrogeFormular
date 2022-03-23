using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogeFormular.Models
{
    internal class UniqueCars
    {
        [BsonId]
        public virtual ObjectId? _id { get; set; }//Så att det går att uppdatera smidigt ;) Jag valde virtual bara för db sätter idt skapar man bar ett Unique cars som inte ska sparas behövs inte Object id
        public int Count { get; set; }  
        public string CarName { get; set; }    
    }
}
