namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    //public enum ResponseStatus
    //{
    //    Continue = 100,

    //    Ok = 200,
    //    Created = 201,
    //    LowOnStorageSpace = 250,

    //    MultipleChoices = 300,
    //    MovedPermanently = 301,
    //    MovedTemporarily = 302,
    //    SeeOther = 303,
    //    NotModified = 304,
    //    UseProxy = 305,

    //    BadRequest = 400, // all
    //    UnAuthorized = 401, //all

    //    PaymentRequired = 402, //all
    //    Forbidden = 403, //all
    //    NotFound = 404, //all
    //    MethodNotAllowed = 405, //all
    //    NotAcceptable = 406, //all
    //    ProxyAuthenticationRequired = 407, //all
    //    RequestTimeout = 408, //all
    //    Gone = 410, //all
    //    LengthRequired = 411, //all
    //    PreconditionFailed = 412, //DESCRIBE, SETUP
    //    RequestEntityTooLarge = 413, //all
    //    Request_URITooLong = 414, //all
    //    UnsupportedMediaType = 415, //all
    //    InvalidParameter = 451, //SETUP
    //    IllegalConferenceIdentifier = 452, //SETUP
    //    NotEnoughBandwidth = 453, //SETUP
    //    SessionNotFound = 454, //all
    //    MethodNotValidInThisState = 455, //all
    //    HeaderFieldNotValid = 456, //all
    //    InvalidRange = 457, //PLAY
    //    ParameterIsReadOnly = 458, //SET_PARAMETER
    //    AggregateOperationNotAllowed = 459, //all
    //    OnlyAggregateOperationAllowed = 460, //all
    //    UnsupportedTransport = 461, //all
    //    DestinationUnreachable = 462, //all

    //    InternalServerError = 500, //all
    //    NotImplemented = 501, //all
    //    BadGateway = 502, //all
    //    ServiceUnavailable = 503, //all
    //    GatewayTimeout = 504, //all
    //    RTSPVersionNotSupported = 505, //all
    //    OptionNotSupport = 551, //all
    //}

    public enum ResponseStatus
    {
        Continue = 100,

        Ok = 200,
        Created = 201,
        LowOnStorageSpace = 250,

        MultipleChoices = 300,
        MovedPermanently = 301,
        MovedTemporarily = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,

        BadRequest = 400, // all
        UnAuthorized = 401, //all

        PaymentRequired = 402, //all
        Forbidden = 403, //all
        NotFound = 404, //all
        MethodNotAllowed = 405, //all
        NotAcceptable = 406, //all
        ProxyAuthenticationRequired = 407, //all
        RequestTimeout = 408, //all
        Gone = 410, //all
        LengthRequired = 411, //all
        PreconditionFailed = 412, //DESCRIBE, SETUP
        RequestEntityTooLarge = 413, //all
        Request_URITooLong = 414, //all
        UnsupportedMediaType = 415, //all
        InvalidParameter = 451, //SETUP
        IllegalConferenceIdentifier = 452, //SETUP
        NotEnoughBandwidth = 453, //SETUP
        SessionNotFound = 454, //all
        MethodNotValidInThisState = 455, //all
        HeaderFieldNotValid = 456, //all
        InvalidRange = 457, //PLAY
        ParameterIsReadOnly = 458, //SET_PARAMETER
        AggregateOperationNotAllowed = 459, //all
        OnlyAggregateOperationAllowed = 460, //all
        UnsupportedTransport = 461, //all
        DestinationUnreachable = 462, //all

        InternalServerError = 500, //all
        NotImplemented = 501, //all
        BadGateway = 502, //all
        ServiceUnavailable = 503, //all
        GatewayTimeout = 504, //all
        RTSPVersionNotSupported = 505, //all
        OptionNotSupport = 551, //all
    }
}