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
        public virtual ObjectId? _id { get; set; }//Så att det går att uppdatera smidigt ;)
        public int Count { get; set; }  
        public string CarName { get; set; }    
    }
}
