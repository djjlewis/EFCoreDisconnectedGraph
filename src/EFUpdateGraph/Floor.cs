using System.Collections;
using System.Collections.Generic;

namespace EFUpdateGraph
{
    public class Floor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}