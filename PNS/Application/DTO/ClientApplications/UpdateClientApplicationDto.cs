using System;

namespace Application.Dto.ClientApplications
{
    public class UpdateClientApplicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}