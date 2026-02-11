using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seeders
{
    public class SessionSeed
    {
        private readonly CinemaDbContext _context;

        public SessionSeed(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Sessions.AnyAsync())
                return;

            var halls = await _context.Halls.AsNoTracking().ToListAsync();
            var movies = await _context.Movies.AsNoTracking().ToListAsync();

            if (!halls.Any() || !movies.Any())
                return;

            var today = DateTime.UtcNow.Date;
            var sessions = new List<SessionEntity>();
            var random = new Random();

            for (int day = 0; day < 7; day++)
            {
                var currentDate = today.AddDays(day);

                var activeMoviesForDay = movies
                    .Where(m => m.RentalStartDate <= currentDate && m.RentalEndDate >= currentDate)
                    .ToList();

                if (!activeMoviesForDay.Any()) continue;

                var moviesQueue = new Queue<MovieEntity>();

                foreach (var hall in halls)
                {
                    var workStart = currentDate.AddHours(10); 
                    var workEnd = currentDate.AddHours(22);   
                    var currentTime = workStart;

                    while (currentTime < workEnd)
                    {

                        if (!moviesQueue.Any())
                        {
                            RefillQueue(moviesQueue, activeMoviesForDay, random);
                        }

                        MovieEntity selectedMovie = null;

                        var timeRemaining = workEnd - currentTime;

                        int attempts = moviesQueue.Count;
                        for (int i = 0; i < attempts; i++)
                        {
                            var candidate = moviesQueue.Dequeue();

                            if (candidate.Duration <= timeRemaining.TotalMinutes)
                            {
                                selectedMovie = candidate;
                                break; 
                            }
                            else
                            {
                                moviesQueue.Enqueue(candidate);
                            }
                        }

                        if (selectedMovie == null)
                        {
                            break;
                        }

                        var sessionEnd = currentTime.AddMinutes(selectedMovie.Duration);

                        sessions.Add(new SessionEntity
                        {
                            Id = Guid.NewGuid(),
                            MovieId = selectedMovie.Id,
                            HallId = hall.Id,
                            StartTime = currentTime,
                            EndTime = sessionEnd,
                            SessionStatus = SessionStatusEnum.Scheduled
                        });

                        currentTime = sessionEnd.AddMinutes(15);
                    }
                }
            }

            await _context.Sessions.AddRangeAsync(sessions);
            await _context.SaveChangesAsync();
        }

        private void RefillQueue(Queue<MovieEntity> queue, List<MovieEntity> sourceMovies, Random rng)
        {
            var shuffled = sourceMovies.OrderBy(x => rng.Next()).ToList();

            foreach (var movie in shuffled)
            {
                queue.Enqueue(movie);
            }
        }
    }
}

