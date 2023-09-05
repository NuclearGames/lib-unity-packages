namespace LogServiceClient.Runtime.Constants {
    public static class LogServiceResultCodes {
        public static class PostEvents {
            public static class Created {
                public const int HTTP_CODE = 201;

                public static bool Check(long httpCode, string errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class NotFound {
                public const int HTTP_CODE = 404;
                public const string DB_NOT_FOUND = "DbNotFound";
                public const string REPORT_NOT_FOUND = "ReportIdNotFound";

                public static bool Check(long httpCode, string errorCode) {
                    return httpCode == HTTP_CODE
                        && (errorCode == DB_NOT_FOUND || errorCode == REPORT_NOT_FOUND);
                }
            }

            public static class Internal {
                public const int HTTP_CODE = 500;

                public static bool Check(long httpCode, string errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }
        }
    }
}
