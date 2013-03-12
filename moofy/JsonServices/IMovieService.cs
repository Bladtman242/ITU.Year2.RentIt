using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace moofy.JsonServices {
    [ServiceContract]
    interface IMovieService {

        [OperationContract]
        MovieWrapper GetMovie(string id);

        [OperationContract]
        SuccessFlag PurchaseMovie(string id, int userId);

        [OperationContract]
        SuccessFlag RentMovie(string id, int userId);

        [OperationContract]
        SuccessFlagDownload DownloadMovie(string id, string userId);

        [OperationContract]
        ListOfMovies ListAllMovies();

        [OperationContract]
        ListOfMovies FilterMovies(string filter);

        [OperationContract]
        ListOfMovies FilterAndSortMovies(string filter, string sortBy);

        [OperationContract]
        SuccessFlagUpload UploadMovie();

        [OperationContract]
        SuccessFlagId CreateMovie(int managerid, string tmpid, string title, int release, string[] directors, string[] genres, string description, int rentalPrice, int purchasePrice);

        [OperationContract]
        SuccessFlag DeleteMovie(string id, int managerid);

    }
}
