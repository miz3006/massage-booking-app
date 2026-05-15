using System;

namespace MassageStudio.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Name            { get; set; } = default!;
        public string Description     { get; set; } = default!;
        public int    DurationMinutes { get; set; }
        public decimal Price          { get; set; }

        public string?   OwnerId     { get; set; }
        public DateTime  DateCreated { get; set; }
        public DateTime? DateEdited  { get; set; }
    }
}
