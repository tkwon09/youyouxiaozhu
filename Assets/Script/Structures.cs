#region struct
public struct buff
{
    public string buffName;
    public bool isTemp;
    public float time;

    public buff(string bName, bool temp, float t)
    {
        buffName = bName;
        isTemp = temp;
        time = t;
    }
}

public struct wbuff
{
    public buff buffToAdd;
    public float probability;

    public wbuff(buff b, float p)
    {
        buffToAdd = b;
        probability = p;
    }
}

public struct damage
{
    public damageType type;
    public int pDamage;
    public int cDamage;
    public Element element;

    public damage(damageType t, int pd, int cd = 0, Element e = Element.none)
    {
        type = t;
        pDamage = pd;
        cDamage = cd;
        element = e;
    }
}
#endregion

#region enum
public enum damageType { physical, blended, chi };
public enum EnemyType { idle, attack, guard };
public enum Element { none = -1, gold, wood, water, fire, earth };
public enum buffType { Enhence, Impair };
#endregion

#region interface
interface setbuffparam
{
    void setTime(float durtime);
}

public interface EnemyBehaviors
{
    void GetHurt();
    void Attack();
    void Die();
    void Parry();
    bool Special();
    int GetSpecialCost();
}
#endregion