namespace SimonVossSearch.Core;

public class Field
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Type { get; set; }
    public string Property { get; set; } // (??)
    public string Value { get; set; }
    public double Weight { get; set; }
    public double WCoef { get; set; }
    public Field(Guid id, string type, string property, string value, Guid parentId)
    {
        Id = id;
        Type = type;
        Property = property;
        Value = value;
        Weight = 0;
        ParentId = parentId;
    }

    public void CalculateWeight(double w)
    {
        WCoef = w;
        if ((int)w == 1)
            Weight = w * SelfWeight()*10;
        else
            Weight = w * SelfWeight();
    }

    public void CalculateWeight(string parentProperty, double parentCoef)
    {
        Weight = ParentWeight(parentProperty) * parentCoef;
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

    private int ParentWeight(string parentProperty)
    {
        switch (Type)
        {
            case "Lock" :
                switch (parentProperty)
                {
                    case "Name":
                        return 8;
                        break;
                    case "ShortCut":
                        return 5;
                }
                ;
                break;
            case "Medium":
                switch (parentProperty)
                {
                    case "Name":
                        return 8;
                }
                ;
                break;
                
        }
        return 0;
    }
}