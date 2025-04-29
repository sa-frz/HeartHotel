namespace HeartHotel.Models
{
    public class ProgramHallsViewModel
    {
        public string? VenueHallName { get; set; }
        public string? VenueHallAddress { get; set; }
        public string? ProgramName { get; set; }
        public int ProgramConductorsId { get; set; }
        public IEnumerable<ProgramConductorViewModel>? ProgramConductors { get; set; }
    }

    public partial class ProgramConductorViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string SaatAz { get; set; } = null!;

        public string SaatTa { get; set; } = null!;
    }

    public class ProgramViewModel
    {
        public string? Date { get; set; }
        public string? ShowDate { get; set; }
        public string? ProgramName { get; set; }
        public IEnumerable<ProgramDataViewModel>? ProgramData { get; set; }
        public IEnumerable<moderatorsDataViewModel>? moderatorsData { get; set; }
        public int? ProgramId { get; set; }
        public int? VenueHallId { get; set; }
        public int ThemeId { get; set; }
    }

    public class ProgramDataViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Time { get; set; }
    }

    public class moderatorsDataViewModel
    {
        public int ChairId { get; set; }
        public int RoleId { get; set; }
    }

    public class GetProgram
    {
        public string? Date { get; set; }
        public int VenueHallId { get; set; }
    }


    // Define a request model
    public class NotifyGroupRequest
    {
        public string Contents { get; set; }
        public string Icon { get; set; }
    }
}