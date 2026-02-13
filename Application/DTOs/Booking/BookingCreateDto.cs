namespace Application.DTOs.Booking;

public class BookingCreateDto
{
    public Guid ShowtimeId { get; set; }
    public List<string> Seats { get; set; } = new();
}