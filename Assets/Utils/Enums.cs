public enum GameOverState
{
    // 掉下去、流沙陷阱、食物消失、用户输入
    Falling, Trapped, FoodDisapper, UserInterrupt,
}

public enum FoodKind
{
    // 辣椒、香蕉、冰块
    pepper, banana, ice,
}

public enum ObjectKind
{
    // 食物（包括冰块），障碍物， 身体， 空气， 陷阱
    food, obstacle, body, air, trap,
}