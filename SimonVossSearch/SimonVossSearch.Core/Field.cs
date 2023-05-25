namespace SimonVossSearch.Core;

public class Field
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Property { get; set; } // (??)
    public string Value { get; set; } 
    public float StrLen { get; set; }
    public float Distance { get; set; }
    public float Weight { get; set; }
    public Field(Guid id, string type, string property, string value, string targetString)
    {
        Id = id;
        Type = type;
        Property = property;
        Value = value;
        
        StrLen = value.Length;
        Distance = CalculateDistance(value.ToLower(), targetString.ToLower());
        if (Distance == 0)
            Weight = 10 * SelfWeight();
        else if (Distance < StrLen)
            Weight = NormalizeDistance((float)Distance, (float)StrLen);
    }

    private float NormalizeDistance(float distance, float strlen)
    {
        return 1 - distance / strlen;
    }
    private int CalculateDistance(string src, string str)
    {
        var srcLength = src.Length;
        var strLength = str.Length;
        
        int[,] mtx = new int[srcLength + 1, strLength + 1];
        
        for (int i = 0; i <= srcLength; i++)
            mtx[i, 0] = i;
        for (int j = 0; j <= strLength; j++)
            mtx[0, j] = j;
        
        for (int i = 1; i <= srcLength; i++)
        {
            for (int j = 1; j <= strLength; j++)
            {
                int cost = (src[i - 1] == str[j - 1]) ? 0 : 1;
                mtx[i, j] = Math.Min(
                    Math.Min(mtx[i - 1, j] + 1, // delete
                        mtx[i, j - 1] + 1), // insert
                    mtx[i - 1, j - 1] + cost); // substitute
            }
        }

        return mtx[srcLength, strLength];
    
    }

    private int SelfWeight()
    {
        switch (Type)
        {
            case "Building":
                switch (Property)
                {
                    case "ShortCut":
                        return 7;
                    case "Description":
                        return 5;
                    case "Name":
                        return 9;
                }
                break;
            case "Lock" :
                switch (Property)
                {
                    case "Type":
                        return 3;
                    case "Name":
                        return 10;
                    case "SerialNumber":
                        return 8;
                    case "Floor":
                        return 6;
                    case "RoomNumber":
                        return 6;
                    case "Description":
                        return 6;
                }

                ;
                break;
            case "Group":
                switch (Property)
                {
                      case  "Name":
                          return 9;
                      case "Description":
                          return 5;
                }

                break;
            case "Medium":
                switch (Property)
                {
                        case "Type":
                            return 3;
                        case "Owner":
                            return 10;
                        case "SerialNumber":
                            return 8;
                        case "Description" :
                            return 6;
                }

                break;
        }

        return 1;
    }

    /*private int ParentWeight()
    {
        int weight=0;
        switch (Type)
        {
            case "Lock" :
                switch (ParentProperty)
                {
                    case "Name":
                        weight += 8;
                        break;
                    case "ShortCut":
                        weight += 5;
                        break;
                }
                ;
                break;
        }
        return weight;
    }*/
}