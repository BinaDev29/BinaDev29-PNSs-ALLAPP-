namespace Application.Dto.ClientApplications
{
    public class CreateClientApplicationDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}