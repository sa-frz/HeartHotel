namespace HeartHotel.Models
{
    public class ProgramViewModel
    {
        public string? Date { get; set; }
        public string? ShowDate { get; set; }
        public string? ProgramName { get; set; }
        public IEnumerable<ProgramDataViewModel>? ProgramData { get; set; }
        public int VenueHallId { get; set; }
        public int ThemeId { get; set; }
    }

    public class ProgramDataViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Time { get; set; }
    }

    public class GetProgram
    {
        public string? Date { get; set; }
        public int VenueHallId { get; set; }
    }
}