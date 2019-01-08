using DentHub.Data;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DentHub.Web.Services.DataServices.Tests
{
     public class RatingServiceTests
    {
        [Fact]
        public void GetAverageDentistRating_WithValidDentistId_ShouldReturnString()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var rating1 = new Rating
            {
                Id = 1,
                PatientId = "1",
                DentistId = "1",
                RatingByPatient = 6
            };

            var rating2 = new Rating
            {
                Id = 2,
                PatientId = "1",
                DentistId = "1",
                RatingByPatient = 10
            };
            var rating3 = new Rating
            {
                Id = 3,
                PatientId = "1",
                DentistId = "1",
                RatingByPatient = 8
            };
            var rating4 = new Rating
            {
                Id = 4,
                PatientId = "1",
                DentistId = "2",
                RatingByPatient = 6
            };

            dbContext.Ratings.Add(rating1);
            dbContext.Ratings.Add(rating2);
            dbContext.Ratings.Add(rating3);
            dbContext.Ratings.Add(rating4);
            dbContext.SaveChanges();

            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new RatingService(ratingRepository);
            var result = service.GetAverageDentistRating("1");
            Assert.Equal("8", result);
        }

        [Fact]
        public void GetAverageDentistRating_WithValidDentistIdAndNoRatings_ShouldReturnNotRated()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var rating1 = new Rating
            {
                Id = 1,
                PatientId = "1",
                DentistId = "2",
                RatingByPatient = 6
            };

            var rating2 = new Rating
            {
                Id = 2,
                PatientId = "1",
                DentistId = "3",
                RatingByPatient = 10
            };
            var rating3 = new Rating
            {
                Id = 3,
                PatientId = "1",
                DentistId = "4",
                RatingByPatient = 8
            };
            var rating4 = new Rating
            {
                Id = 4,
                PatientId = "1",
                DentistId = "5",
                RatingByPatient = 6
            };

            dbContext.Ratings.Add(rating1);
            dbContext.Ratings.Add(rating2);
            dbContext.Ratings.Add(rating3);
            dbContext.Ratings.Add(rating4);
            dbContext.SaveChanges();

            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new RatingService(ratingRepository);
            var result = service.GetAverageDentistRating("1");
            Assert.Equal("Not Rated", result);
        }

        [Fact]
        public void GetAveragePatientRating_WithValidPatientId_ShouldReturnString()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var rating1 = new Rating
            {
                Id = 1,
                PatientId = "1",
                DentistId = "1",
                RatingByDentist = 6
            };

            var rating2 = new Rating
            {
                Id = 2,
                PatientId = "1",
                DentistId = "1",
                RatingByDentist = 10
            };
            var rating3 = new Rating
            {
                Id = 3,
                PatientId = "1",
                DentistId = "1",
                RatingByDentist = 8
            };
            var rating4 = new Rating
            {
                Id = 4,
                PatientId = "2",
                DentistId = "2",
                RatingByDentist = 6
            };

            dbContext.Ratings.Add(rating1);
            dbContext.Ratings.Add(rating2);
            dbContext.Ratings.Add(rating3);
            dbContext.Ratings.Add(rating4);
            dbContext.SaveChanges();

            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new RatingService(ratingRepository);
            var result = service.GetAveragePatientRating("1");
            Assert.Equal("8", result);
        }

        [Fact]
        public void GetAveragePatientRating_WithValidPatientIdAndNoRatings_ShouldReturnNotRated()
        {
            var options = new DbContextOptionsBuilder<DentHubContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Give a Unique name to the DB
               .Options;
            var dbContext = new DentHubContext(options);

            var rating1 = new Rating
            {
                Id = 1,
                PatientId = "2",
                DentistId = "1",
                RatingByDentist = 6
            };

            var rating2 = new Rating
            {
                Id = 2,
                PatientId = "2",
                DentistId = "1",
                RatingByDentist = 10
            };
            var rating3 = new Rating
            {
                Id = 3,
                PatientId = "3",
                DentistId = "1",
                RatingByDentist = 8
            };
            var rating4 = new Rating
            {
                Id = 4,
                PatientId = "4",
                DentistId = "1",
                RatingByDentist = 6
            };

            dbContext.Ratings.Add(rating1);
            dbContext.Ratings.Add(rating2);
            dbContext.Ratings.Add(rating3);
            dbContext.Ratings.Add(rating4);
            dbContext.SaveChanges();

            var ratingRepository = new DbRepository<Rating>(dbContext);
            var service = new RatingService(ratingRepository);
            var result = service.GetAveragePatientRating("1");
            Assert.Equal("Not Rated", result);
        }
    }
}
