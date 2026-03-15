

namespace Sudoku.Technique;

internal enum ResultType
{
    SET,
    REMOVE
}

internal class Result
{
    internal ResultType Type {get; init;}

    internal int CellIndex {get; init;}
    internal int CellValue {get; init;}

    internal Result(ResultType resultType, int cellIndex, int cellValue)
    {
        Type = resultType;
        CellIndex = cellIndex;
        CellValue = cellValue;
    }


}