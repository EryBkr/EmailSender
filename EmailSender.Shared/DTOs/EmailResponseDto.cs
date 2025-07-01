namespace EmailSender.Shared.DTOs
{
    public sealed class EmailResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? EmailId { get; set; } // Mongo ID / Message ID
    }
}
