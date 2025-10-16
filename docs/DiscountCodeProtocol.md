# Discount Code TCP Protocol

Binary communication protocol for generating and using discount codes.  
Designed for low overhead, extensibility, and straightforward error handling.

---

## Message Structure

All messages (requests and responses) share the following fixed header structure:

| Field         | Size (bytes) | Description                           |
|---------------|--------------|-------------------------------------|
| Command       | 1            | Command identifier (e.g., 0x01 = GENERATE, 0x02 = USE) |
| PayloadLength | 2            | Payload length in bytes (UInt16, little-endian)         |
| Payload       | N            | Command-specific payload                                   |

---

## Commands (Request)

| Code | Name     | Description                  |
|-------|----------|------------------------------|
| 0x01  | GENERATE | Request to generate discount codes |
| 0x02  | USE      | Request to use a specific discount code |
| 0x03  | STATUS*  | (Optional) Request server status |
| 0xFF  | ERROR    | Server error response |

\* `STATUS` command is optional and not implemented in the current version.

---

## Command Details

### GENERATE – Generate Discount Codes

**Request Payload:**

| Field  | Type   | Description                  |
|--------|--------|------------------------------|
| Count  | UInt16 | Number of codes to generate (max 2000) |
| Length | Byte   | Length of each code (7 or 8 characters) |

**Response Payload:**

| Field  | Type | Description       |
|--------|------|-------------------|
| Result | Byte | 1 = Success, 0 = Failure |

---

### USE – Use a Discount Code

**Request Payload:**

| Field | Type   | Description                          |
|-------|--------|------------------------------------|
| Code  | String | Exactly 8 ASCII characters (no null terminator) |

**Response Payload:**

| Field  | Type | Description       |
|--------|------|-------------------|
| Result | Byte | 1 = Success, 0 = Failure |

---

### ERROR – Error Response

The server may respond with an ERROR command (0xFF) to any request in case of failure.

**Response Payload:**

| Field     | Type   | Description               |
|-----------|--------|---------------------------|
| ErrorCode | String | ASCII error code (e.g. `ERR_DUPLICATE_CODE`) |

---

## Example Error Codes

| Code                 | Description                        |
|----------------------|----------------------------------|
| ERR_INVALID_LENGTH   | Invalid code length requested     |
| ERR_DUPLICATE_CODE   | Generated code already exists     |
| ERR_CODE_NOT_FOUND   | Provided code does not exist      |
| ERR_CODE_ALREADY_USED| Provided code was already used    |
| ERR_MALFORMED_PACKET | Malformed or invalid message      |

---

## Encoding

- All string fields use ASCII encoding.
- Codes are sent without null terminators.

---

## Notes

- The protocol header is always 3 bytes (1 byte command + 2 bytes payload length).
- The server supports parallel request processing.
- Generated codes are persisted to storage and survive server restarts.
- Maximum of 2000 codes can be generated per request.
- Code length must be either 7 or 8 characters.
- Clients and server must handle malformed or invalid packets gracefully.
- The protocol is designed to be extensible for future commands.