namespace Application.Commons;

public record struct HmacSha(byte[] ComputeHash, byte[] Key);
