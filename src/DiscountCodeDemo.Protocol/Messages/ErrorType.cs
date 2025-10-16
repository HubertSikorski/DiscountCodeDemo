namespace DiscountCodeDemo.Protocol.Messages;

public enum ErrorType : byte
{
    ERR_INVALID_REQUEST = 0x01,
    ERR_INVALID_LENGTH = 0x02,
    ERR_DUPLICATE_CODE = 0x03,
    ERR_CODE_NOT_FOUND = 0x04,
    ERR_CODE_ALREADY_USED = 0x05
}