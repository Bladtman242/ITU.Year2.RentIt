using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace moofy.JsonServices {
    [ServiceContract]
    interface IManagerService {
        [OperationContract]
        SuccessFlag PromoteUserToManager(int managerid, int userid);

        [OperationContract]
        SuccessFlag DemoteManagerToUser(int managerid, int userid);
    }
}
