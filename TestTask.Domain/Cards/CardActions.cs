namespace TestTask.Domain.Cards;

[Flags]
public enum CardActions
{
    None = 0,
    Action1 = 1 << 0, // 1
    Action2 = 1 << 1, // 2
    Action3 = 1 << 2, // 4
    Action4 = 1 << 3, // 8
    Action5 = 1 << 4, // 16
    Action6 = 1 << 5, // 32
    Action7 = 1 << 6, // 64
    Action8 = 1 << 7, // 128
    Action9 = 1 << 8,  // 256
    Action10 = 1 << 9,  // 512
    Action11 = 1 << 10,  // 1024
    Action12 = 1 << 11,  // 2048
    Action13 = 1 << 12,  // 4096
}