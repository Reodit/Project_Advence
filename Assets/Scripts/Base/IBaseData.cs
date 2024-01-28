using System.Data;

public interface IBaseData
{
    void InitializeFromTableData(DataRow row);
}