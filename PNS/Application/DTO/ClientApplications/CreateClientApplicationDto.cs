namespace Application.Dto.ClientApplications
{
    public class CreateClientApplicationDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        // አዲሱ LogoUrl ንብረት
        public string? LogoUrl { get; set; }
        // IsActive ንም እንደ አስፈላጊነቱ ማከል ትችላለህ
        public bool IsActive { get; set; } = true;
    }
}