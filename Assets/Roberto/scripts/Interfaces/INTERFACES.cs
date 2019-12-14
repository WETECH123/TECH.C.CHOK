
public interface ITeam
{
    Team getTeam();
}
public enum Team
{
    chocolate = 1,
    vanilla=2,
    matcha=3,
    strawberry=4
}

public interface IAttack<T>
{
    void attack(T t);
   
}

public interface IHealth
{
     void Healing(float heal);
     void takeDamage(float damageToTake);
}

public enum sp_weapon_type
{
    icepistol,
    caramelThrower,
    spiceThrower,
    ladle
}

public interface IFroze
{
    void frozing();
    bool isfrozen { get; set; }
}

public interface IBurn
{
    void burn();
    bool isBurning { get; set; }
    void getFireDamage();
    float fireLifeTime { get; set; }
    float damageforSecond { get; set;}
    float damageDelay { get; set;}
    float damageDelayCounter { get; set;}
}

public interface ISpeed
{
    void speedDown();
    void speedUp();
    float changeSpeedTime { get; set; }
    float changeSpeedCounter { get; set; }
}
public interface Iflour
{
   void activeFlour();
}
public interface IJump
{
    void jump();
}
public interface IConfused
{
    void confuse();
}




