using System;

namespace Application.Dto.ClientApplications
{
    public class ClientApplicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ApiKey { get; set; } = string.Empty;
    }
}