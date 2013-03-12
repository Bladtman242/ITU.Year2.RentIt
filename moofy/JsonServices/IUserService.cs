using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace moofy.JsonServices {
    [ServiceContract]
    public interface IUserService {
        [OperationContract]
        SuccessFlag CreateUser(string name, string username, string email, string password);

        [OperationContract]
        UserWrapper GetUser(string id);

        [OperationContract]
        SuccessFlagId Login(string username, string password);

        [OperationContract]
        SuccessFlag DepositMoney(string id, int moneyAmount);

        [OperationContract]
        ListOfMovies GetMoviesFromUser(string id);

        [OperationContract]
        ListOfMovies GetCurrentMoviesFromUser(string id);
    }
}
