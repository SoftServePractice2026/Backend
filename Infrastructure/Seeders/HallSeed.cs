using Domain.Entities;
using Domain.Entities.Enums;

namespace Infrastructure.Seeders
{
    public class HallSeed
    {
        private readonly CinemaDbContext _context;

        public HallSeed(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Halls.Any())
            {
                var hallSizes = new List<HallSizeEnum>
                {
                    HallSizeEnum.Small, HallSizeEnum.Small,
                    HallSizeEnum.Medium, HallSizeEnum.Medium,
                    HallSizeEnum.Large, HallSizeEnum.Large
                };

                for (int i = 0; i < hallSizes.Count; i++)
                {
                    var currentSize = hallSizes[i];

                    HallEntity hall = new HallEntity()
                    {
                        Name = $"Зал {i + 1}",
                        HallSize = currentSize,
                        IsActive = true,
                    };

                 
                    hall.Seats = GenerateSeats(currentSize, hall);

                    await _context.Halls.AddAsync(hall);
                }

                await _context.SaveChangesAsync();
            }
        }
        private List<SeatEntity> GenerateSeats(HallSizeEnum size, HallEntity hall)
        {
            var seats = new List<SeatEntity>();
            int hallSize = 0;
            switch (size)
            {
                case HallSizeEnum.Small: hallSize = 5; break;
                case HallSizeEnum.Medium: hallSize = 6; break;
                case HallSizeEnum.Large: hallSize = 7; break;
            }

            for (int i = 1; i <= hallSize; i++)
            {
                for (int j = 1; j <= hallSize; j++)
                {
                    var seat = new SeatEntity()
                    {
                        RowNumber = i,
                        SeatNumber = j,
                        Hall = hall, 
                        SeatStatus = SeatStatusEnum.Normal
                    };

                    if (i == hallSize)
                    {
                        seat.SeatType = SeatTypeEnum.VIP;
                    }
                    else
                    {
                        seat.SeatType = SeatTypeEnum.Standart;
                    }

                    seats.Add(seat);
                }
            }
            return seats;
        }
    }
}
