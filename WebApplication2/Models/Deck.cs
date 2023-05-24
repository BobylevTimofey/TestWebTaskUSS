using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Card>? Cards { get; set; }
    }
}
