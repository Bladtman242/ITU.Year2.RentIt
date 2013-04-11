using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace moofy.JsonServices {
    [ServiceContract]
    interface IMovieService {

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}")]
        MovieWrapper GetMovie(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/purchase")]
        SuccessFlag PurchaseMovie(string id, int userId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/rent")]
        SuccessFlag RentMovie(string id, int userId);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}/download?userId={userId}")]
        SuccessFlagDownload DownloadMovie(string id, string userId);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter")]
        MovieWrapper[] ListAllMovies();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter/{filter}")]
        MovieWrapper[] FilterMovies(string filter);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter/{filter}?sortBy={sortBy}")]
        MovieWrapper[] FilterAndSortMovies(string filter, string sortBy);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "upload?extension={ext}")]
        SuccessFlagUpload UploadMovie(string ext, Stream fileStream);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "create")]
        SuccessFlagId CreateMovie(int managerid, int tmpid, string title, int release, string[] directors, string[] genres, string description, int rentalPrice, int purchasePrice, string coverUri);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/delete")]
        SuccessFlag DeleteMovie(string id, int managerid);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/rate")]
        SuccessFlag RateMovie(string id, int userId, int rating);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}/ratings?userId={userId}")]
        RatingWrapper GetMovieRatings(string id, string userId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/update")]
        SuccessFlag UpdateMovie(string id, int managerid, string title = null, string description = null, int rentalPrice = -1, int purchasePrice = -1, int release = -1, string coverUri = null, string[] genres = null, string[] directors = null);

    }
}
