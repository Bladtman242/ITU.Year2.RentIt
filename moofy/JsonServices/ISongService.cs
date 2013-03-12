using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace moofy.JsonServices {
    [ServiceContract]
    interface ISongService {

        [OperationContract]
        SongWrapper GetSong(string id);

        [OperationContract]
        SuccessFlag PurchaseSong(string id, int userId);

        [OperationContract]
        SuccessFlag RentSong(string id, int userId);

        [OperationContract]
        SuccessFlagDownload DownloadSong(string id, string userId);

        [OperationContract]
        ListOfSongs ListAllSongs();

        [OperationContract]
        ListOfSongs FilterSongs(string filter);

        [OperationContract]
        ListOfSongs FilterAndSortSongs(string filter, string sortBy);

        [OperationContract]
        SuccessFlagUpload UploadSong();

        [OperationContract]
        SuccessFlagId CreateSong(int managerid, string tmpid, string title, int release, string artist, string album, string[] genres, int rentalPrice, int purchasePrice);

        [OperationContract]
        SuccessFlag DeleteSong(string id, int managerid);

    }
}
