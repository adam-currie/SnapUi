using System;
using System.Collections.Generic;
using System.Text;

namespace SnapUi {

    public class RequiredAncestorNotFoundException: Exception {

        public RequiredAncestorNotFoundException()
            : base(CreateMessage()) { }

        public RequiredAncestorNotFoundException(string message)
            : base(message) { }

        public RequiredAncestorNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public RequiredAncestorNotFoundException(Type typeOfRequired)
            : this(CreateMessage(typeOfRequired.FullName)) { }

        public RequiredAncestorNotFoundException(Type typeOfRequired, Exception innerException)
            : this(CreateMessage(typeOfRequired.FullName), innerException) { }

        private static string CreateMessage(string? ancestor = null) =>
            "Required ancestor"
            + ((ancestor == null) ? " " : ":'" + ancestor + "' ")
            + "not found.";

    }
}
