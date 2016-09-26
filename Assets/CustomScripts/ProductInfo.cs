using UnityEngine;
using System.Collections;

public class ProductInfo : MonoBehaviour {

    public Type type;
    public enum Type
    {
        AvocadoProduct,//1
        CarVisorProduct,//2
        NailPolishProduct,//3
        UniverseBluRayProduct,//4
        JetLagPillProduct,//5
        BalanceBarProduct,//6
        CandleProduct,//7
        OutOfStock//8
    }

    public Type Get()
    {
        return type;
    }

}
