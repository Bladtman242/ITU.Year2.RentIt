﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace moofy.JsonServices {
    [ServiceContract]
    interface ISongService {

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}")]
        SongWrapper GetSong(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/purchase")]
        SuccessFlag PurchaseSong(string id, int userId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/rent")]
        SuccessFlag RentSong(string id, int userId);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}/download?userId={userId}")]
        SuccessFlagDownload DownloadSong(string id, string userId);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter")]
        SongWrapper[] ListAllSongs();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter/{filter}")]
        SongWrapper[] FilterSongs(string filter);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter/{filter}?sortBy={sortBy}")]
        SongWrapper[] FilterAndSortSongs(string filter, string sortBy);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "filter/?sortBy={sortBy}")]
        SongWrapper[] SortSongs(string sortBy);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "upload")]
        SuccessFlagUpload UploadSong(Stream fileStream);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "create")]
        SuccessFlagId CreateSong(int managerid, int tmpid, string title, string description, int release, string artist, string album, string[] genres, int rentalPrice, int purchasePrice, string coverUri);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/delete")]
        SuccessFlag DeleteSong(string id, int managerid);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/rate")]
        SuccessFlag RateSong(string id, int userId, int rating);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}/ratings?userId={userId}")]
        RatingWrapper GetSongRatings(string id, string userId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/update")]
        SuccessFlag UpdateSong(string id, int managerid, string artist = null, string album = null, string title = null, string description = null, int rentalPrice = -1, int purchasePrice = -1, int release = -1, string coverUri = null, string[] genres = null);

    }
}
