using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Purchase {
        private File file;
        private DateTime endTime;

        public Purchase(File file, DateTime endTime) {
            this.file = file;
            this.endTime = endTime;
        }

        public File File {
            get { return file; }
        }

        public DateTime EndTime {
            get { return endTime; }
        }
    }
}
