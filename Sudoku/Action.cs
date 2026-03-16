namespace Sudoku;


internal class Action
{
    internal ActionType Type {get; init;}

    internal int CellIndex {get; init;}
    internal int CellValue {get; init;}

    internal Action(ActionType resultType, int cellIndex, int cellValue)
    {
        Type = resultType;
        CellIndex = cellIndex;
        CellValue = cellValue;
    }


}