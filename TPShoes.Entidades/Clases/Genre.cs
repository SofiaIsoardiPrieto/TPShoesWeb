﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPShoes.Entidades.Clases
{
    public class Genre:ICloneable
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public ICollection<Shoe> Shoes { get; set; } = new List<Shoe>();
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
