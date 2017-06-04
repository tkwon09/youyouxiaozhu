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

public enum damageType { physical, blended, chi };
public struct damage
{
    public damageType type;
    public int pDamage;
    public int cDamage;

    public damage(damageType t, int pd, int cd = 0)
    {
        type = t;
        pDamage = pd;
        cDamage = cd;
    }
}