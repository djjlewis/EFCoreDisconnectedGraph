using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace EFUpdateGraph
{
    public class FloorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoomDto> RoomDtos { get; set; } = new List<RoomDto>();
    }
}