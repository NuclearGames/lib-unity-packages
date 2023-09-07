namespace LogServiceClient.Runtime.Constants {
    public static class LogServiceResultCodes {
        public static class PutDevice {
            public static class Ok {
                public const int HTTP_CODE = 200;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class Created {
                public const int HTTP_CODE = 201;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class NotFound {
                public const int HTTP_CODE = 404;
                public const LogServiceInternalResultCodes DB_NOT_FOUND = LogServiceInternalResultCodes.DbNotFound;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE
                        && (errorCode == DB_NOT_FOUND);
                }
            }

            public static class Internal {
                public const int HTTP_CODE = 500;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }
        }

        public static class GetSession {
            public static class Created {
                public const int HTTP_CODE = 201;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class NotFound {
                public const int HTTP_CODE = 404;
                public const LogServiceInternalResultCodes DB_NOT_FOUND = LogServiceInternalResultCodes.DbNotFound;
                public const LogServiceInternalResultCodes DEVICE_NOT_FOUND = LogServiceInternalResultCodes.DeviceNotFound;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE
                        && (errorCode == DB_NOT_FOUND || errorCode == DEVICE_NOT_FOUND);
                }
            }

            public static class Internal {
                public const int HTTP_CODE = 500;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }
        }

        public static class GetReport {
            public static class Created {
                public const int HTTP_CODE = 201;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class NotFound {
                public const int HTTP_CODE = 404;
                public const LogServiceInternalResultCodes DB_NOT_FOUND = LogServiceInternalResultCodes.DbNotFound;
                public const LogServiceInternalResultCodes SESSION_NOT_FOUND = LogServiceInternalResultCodes.SessionIdNotFound;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE
                        && (errorCode == DB_NOT_FOUND || errorCode == SESSION_NOT_FOUND);
                }
            }

            public static class Internal {
                public const int HTTP_CODE = 500;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }
        }

        public static class PostEvents {
            public static class Created {
                public const int HTTP_CODE = 201;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }

            public static class NotFound {
                public const int HTTP_CODE = 404;
                public const LogServiceInternalResultCodes DB_NOT_FOUND = LogServiceInternalResultCodes.DbNotFound;
                public const LogServiceInternalResultCodes REPORT_NOT_FOUND = LogServiceInternalResultCodes.ReportIdNotFound;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE
                        && (errorCode == DB_NOT_FOUND || errorCode == REPORT_NOT_FOUND);
                }
            }

            public static class Internal {
                public const int HTTP_CODE = 500;
                public static bool Check(long httpCode, LogServiceInternalResultCodes? errorCode) {
                    return httpCode == HTTP_CODE;
                }
            }
        }
    }
}
