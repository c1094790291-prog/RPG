public class PlayerType
{
    public int id;//编号
    public string name;//卡名
    public int maxhp;//最大血量
    public int hp;//血量
    public PlayerType(int _id, string _name, int _maxhp, int _hp)
    {
        this.id = _id;
        this.name = _name;
        this.maxhp = _maxhp;
        this.hp = _hp;
    }
}