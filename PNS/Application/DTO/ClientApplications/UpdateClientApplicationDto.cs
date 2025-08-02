using System;

namespace Application.Dto.ClientApplications
{
    public class UpdateClientApplicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        // አዲሱ LogoUrl ንብረት
        public string? LogoUrl { get; set; }
        // IsActive ንም እንደ አስፈላጊነቱ ማከል ትችላለህ
        public bool IsActive { get; set; }
    }
}