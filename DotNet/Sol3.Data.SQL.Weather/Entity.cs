using System;

namespace Sol3.Data.SQL.Weather
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
