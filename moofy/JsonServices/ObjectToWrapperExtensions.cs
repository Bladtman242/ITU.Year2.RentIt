using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using moofy.Backend;

namespace moofy.JsonServices {
    public static class ObjectToWrapperExtensions {

        public static MovieWrapper ToWrapper(this Movie movie) {
            return new MovieWrapper() {
                id = movie.Id,
                title = movie.Title,
                release = movie.Year,
                genres = movie.Genres.ToArray<string>(),
                directors = movie.Directors.ToArray(),
                description = movie.Description,
                rentalPrice = movie.RentPrice,
                purchasePrice = movie.BuyPrice,
                avgRating = movie.AverageRating,
                numberOfVotes = movie.NumberOfRatings,
                coverUri = movie.CoverUri,
                viewCount = movie.ViewCount
            };
        }

        public static MovieWrapper[] ToWrapper(this Movie[] movieArray) {
            MovieWrapper[] result = new MovieWrapper[movieArray.Length];
            for (int i = 0; i < movieArray.Length; i++) {
                result[i] = movieArray[i].ToWrapper();
            }
            return result;
        }

        public static SongWrapper ToWrapper(this Song song) {
            return new SongWrapper() {
                id = song.Id,
                title = song.Title,
                release = song.Year,
                genres = song.Genres.ToArray<string>(),
                album = song.Album,
                artists = song.Artists.ToArray(),
                rentalPrice = song.RentPrice,
                purchasePrice = song.BuyPrice,
                avgRating = song.AverageRating,
                numberOfVotes = song.NumberOfRatings,
                coverUri = song.CoverUri,
                viewCount = song.ViewCount
            };
        }

        public static SongWrapper[] ToWrapper(this Song[] songArray) {
            SongWrapper[] result = new SongWrapper[songArray.Length];
            for (int i = 0; i < songArray.Length; i++) {
                result[i] = songArray[i].ToWrapper();
            }
            return result;
        }

        public static UserWrapper ToWrapper(this User user) {
            return new UserWrapper() {
                id = user.Id,
                name = user.Name,
                username = user.Username,
                email = user.Email,
                balance = user.Balance,
                admin = user.IsAdmin
            };
        }

        public static UserWrapper[] ToWrapper(this User[] userArray) {
            UserWrapper[] result = new UserWrapper[userArray.Length];
            for (int i = 0; i < userArray.Length; i++) {
                result[i] = userArray[i].ToWrapper();
            }
            return result;
        }
    }
}
