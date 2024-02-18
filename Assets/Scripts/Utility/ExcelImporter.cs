using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public static class ExcelImporter
    {
        public static void ReadAllExcelFiles()
        {
            string[] filePaths = Directory.GetFiles(Consts.DataSheetFolderPath, "*Data.xlsx", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                CreateScriptableFromDataTable(ExcelToDataSet(filePath), Consts.SCRIPTABLEOBJECT_SAVE_PATH);
            }
        }
    
        private static Type GetTypeFromString(string dataTypeString)
        {
            return dataTypeString switch
            {
                // 기본 타입
                "int" => typeof(int),
                "float" => typeof(float),
                "string" => typeof(string),
                "double" => typeof(double),
                "long" => typeof(long),
                "uint" => typeof(uint),
                "bool" => typeof(bool),
                // TODO enum 추가 필요
                "enum<Status>" => typeof(Enums.Status),
                "enum<MonsterType>" => typeof(Enums.MonsterType),
                "enum<SkillType>" => typeof(Enums.SkillType),
                // Nullable 기본 타입
                "nullableint" => typeof(int?),
                "nullablefloat" => typeof(float?),
                "nullabledouble" => typeof(double?),
                "nullablelong" => typeof(long?),
                "nullableuint" => typeof(uint?),
                // List 타입 (non-nullable)
                // TODO List 타입 추가 필요
                "List<int>" => typeof(List<int>),
                "List<float>" => typeof(List<float>),
                "List<uint>" => typeof(List<uint>),
                "List<bool>" => typeof(List<bool>),
                "List<long>" => typeof(List<long>),
                "List<double>" => typeof(List<double>),
                _ => throw new InvalidOperationException($"지원하지 않는 데이터 타입입니다. : {dataTypeString}")
            };
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            DataSet dataSet = new DataSet(Path.GetFileName(filePath));

            if (!filePath.EndsWith(".xlsx") && !filePath.EndsWith(".xls"))
            {
                throw new Exception("지원되지 않는 파일 형식입니다. .xlsx 또는 xls 파일만 테이블화 가능합니다.");
            }

            IWorkbook workbook;
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = filePath.EndsWith(".xlsx") ? (IWorkbook)new XSSFWorkbook(file) : new HSSFWorkbook(file);
            }

            // Load All Sheets
            Dictionary<ISheet, string> activeSheets = new Dictionary<ISheet, string>();

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                if (!workbook.IsSheetHidden(i))
                {
                    activeSheets.Add(workbook.GetSheetAt(i), workbook.GetSheetName(i));
                }
            }

            foreach (var sheet in activeSheets.Keys)
            {
                DataTable dataTable = new DataTable(sheet.SheetName);

                IRow headerRow = sheet.GetRow(0);           // Header
                IRow dataTypeRow = sheet.GetRow(1);         // DataType
                IRow variableNameRow = sheet.GetRow(2);     // VariableName

                List<int> validRowNumList = new List<int>();
            
                if (headerRow == null)
                {
                    throw new InvalidOperationException($"Header값이 Null 입니다. 엑셀 시트가 빈 시트인가요? 빈 엑셀 시트는 삭제해주세요.");
                }

                else if (dataTypeRow == null)
                {
                    throw new InvalidOperationException($"DataType 값이 Null 입니다. 엑셀 시트가 빈 시트인가요? 빈 엑셀 시트는 삭제해주세요.");
                }

                else if (variableNameRow == null)
                {
                    throw new InvalidOperationException($"Variable 값이 Null 입니다. 엑셀 시트가 빈 시트인가요? 빈 엑셀 시트는 삭제해주세요.");
                }

                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    var headerValue = headerRow.GetCell(i).StringCellValue;

                    if (headerValue != Consts.DataSheetSymbolALLKEY &&
                        headerValue != Consts.DataSheetSymbolALL &&
                        headerValue != Consts.DataSheetSymbolDESIGN)
                    {
                        throw new InvalidOperationException($"헤더 값이 정의된 상수 값이 아닙니다. \n입력한 값 : {headerValue}");
                    }

                    if (headerValue == Consts.DataSheetSymbolDESIGN)
                    {
                        dataTable.Columns.Add(Consts.DataSheetSymbolDESIGNCOLUNM + i, typeof(string));
                        continue;
                    }

                    string dataTypeString = dataTypeRow.GetCell(i).StringCellValue;

                    Type targetType = GetTypeFromString(dataTypeString);

                    string columnName = variableNameRow.GetCell(i).StringCellValue;

                    if (dataTable.Columns.Contains(columnName))
                    {
                        throw new InvalidOperationException($"테이블 '{dataTable.TableName}'에 이미 '{columnName}' 이름의 컬럼이 존재합니다.");
                    }

                    Type columnType;
                
                    // target type이 nullable인지 검사
                    if (targetType.IsGenericType && 
                        targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        columnType = Nullable.GetUnderlyingType(targetType); 
                        dataTable.Columns.Add(variableNameRow.GetCell(i).StringCellValue, columnType);
                        dataTable.Columns[i].AllowDBNull = true;
                    }

                    else if (targetType.IsGenericType && 
                             targetType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        dataTable.Columns.Add(variableNameRow.GetCell(i).StringCellValue, targetType);
                        dataTable.Columns[i].AllowDBNull = true;
                    }
                    
                    else
                    {
                        columnType = targetType;
                        dataTable.Columns.Add(variableNameRow.GetCell(i).StringCellValue, columnType);
                        dataTable.Columns[i].AllowDBNull = false;
                        int validRowNum = 0;
                        for (int rowIndex = 3; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow currentRow = sheet.GetRow(rowIndex);
                            ICell currentCell = currentRow.GetCell(i);
                            if (currentCell == null)
                            {
                                continue;
                            }
                            switch (currentCell.CellType)
                            {
                                case CellType.String:
                                    if (currentCell.StringCellValue != null)
                                    {
                                        validRowNum++;
                                    }
                                    break;

                                case CellType.Numeric:
                                    validRowNum++;
                                    break;

                                case CellType.Boolean:
                                    validRowNum++;
                                    break;

                                default:
                                    break;
                            }
                        }
                    
                        validRowNumList.Add(validRowNum);
                    }
                }

                if (validRowNumList.Max() == 0)
                {
                    throw new InvalidOperationException($"테이블에 유효한 Row가 존재하지 않습니다.");
                }

                for (int rowIndex = 3; rowIndex <= validRowNumList.Max() + 2; rowIndex++)
                {
                    IRow currentRow = sheet.GetRow(rowIndex);
                    DataRow dataRow = dataTable.NewRow();

                    for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                    {
                        ICell currentCell = currentRow.GetCell(colIndex);
                        string currentCellValue = currentCell?.ToString() ?? string.Empty;

                        var headerValue = headerRow.GetCell(colIndex).StringCellValue;

                        if (headerValue == Consts.DataSheetSymbolDESIGN)
                        {
                            continue;
                        }
                    
                        // Nullable 타입 체크
                        var dataType = dataTable.Columns[colIndex].DataType;
                        
                        bool isNullableType = dataTable.Columns[colIndex].AllowDBNull;
                    
                        // 유일한 키 체크
                        if (headerValue == Consts.DataSheetSymbolALLKEY)
                        {
                            if (dataTable.AsEnumerable().Any(row => row[colIndex].ToString() == currentCellValue))
                            {
                                throw new InvalidOperationException($"중복된 키 값 발견: {currentCellValue} 위치: [{rowIndex + 1}, {colIndex + 1}]");
                            }

                            if (isNullableType)
                            {
                                throw new InvalidOperationException("Nullable 자료형은 ALL;KEY 옵션을 사용할 수 없습니다.");
                            }
                        }
                    
                        // targetType이 Nullable<T> 일 경우 또는 List<T>이고 T가 Nullable<T>이거나 참조 타입인 경우(dataType.IsGenericType) isNullableType = true;

                        if (string.IsNullOrEmpty(currentCellValue))
                        {
                            if (isNullableType)
                            {
                                object defaultValue = dataType.IsValueType ? Activator.CreateInstance(dataType) : null;
                                dataRow[colIndex] = defaultValue;
                            }

                            else
                            {
                                throw new InvalidOperationException($"Null 또는 기본값이 허용되지 않는 타입입니다. 위치: [{rowIndex + 1}, {colIndex + 1}]");
                            }
                        }

                        else
                        {
                            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                string[] listElements = currentCellValue.Split(',');
                                Type elementType = dataRow.Table.Columns[colIndex].DataType.GetGenericArguments()[0];

                                IList data = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                                foreach (string value in listElements)
                                {
                                    data.Add(Convert.ChangeType(value, elementType));
                                }

                                dataRow[colIndex] = Convert.ChangeType(data, dataRow.Table.Columns[colIndex].DataType);
                            }

                            else
                            {
                                if (dataType.IsEnum)
                                {
                                    object enumValue = Enum.Parse(dataType, currentCellValue, true); // true는 대소문자를 무시하고 파싱
                                    dataRow[colIndex] = enumValue;
                                }

                                else
                                {
                                    dataRow[colIndex] = Convert.ChangeType(currentCellValue, dataType);
                                }
                            }
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        public static void CreateScriptableFromDataTable(DataSet dataSet, string dataPath)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                // Scriptable Object 인스턴싱
                // Scriptable Object 하위에 List<> 오브젝트가 있는지 확인
                // 실제 Object 생성
                // Object를 List화 해서 Scriptable Object에 담는 과정 필요

                // 클래스 이름 규칙
                string className = table.TableName + "ScriptableObject";

                // ScriptableObject data type
                Type scriptableObjectDataType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => typeof(ScriptableObject).IsAssignableFrom(type)
                                            && !type.IsInterface
                                            && !type.IsAbstract
                                            && type.Name == className);

                if (scriptableObjectDataType == null)
                {
                    throw new InvalidOperationException($"ScriptableObject class '{className}'을 찾을 수 없습니다.");
                }

                string assetPath = $"{dataPath}/{table.TableName}.asset";
                ScriptableObject soInstance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath) ?? ScriptableObject.CreateInstance(scriptableObjectDataType);

                // 실 데이터를 생성할 Instance data type 
                Type instanceDataType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => typeof(IBaseData).IsAssignableFrom(type)
                                            && !type.IsInterface
                                            && !type.IsAbstract
                                            && type.Name == table.TableName);

                if (instanceDataType == null)
                {
                    throw new InvalidOperationException($"IBaseDataEx를 상속받은 '{table.TableName}' 이름의 클래스를 찾을 수 없습니다.");
                }

                // Scriptable필드 이름은 시트의 이름과 동일하다고 가정.
                FieldInfo listField = scriptableObjectDataType.GetFields()
                    .FirstOrDefault(field => field.FieldType.IsGenericType
                                             && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                                             && field.FieldType.GetGenericArguments()[0] == instanceDataType);

                if (listField == null)
                {
                    throw new InvalidOperationException($"'{className}'에는 '{scriptableObjectDataType.Name}' 타입의 'List<{instanceDataType.Name}>' 필드가 필요합니다.");
                }

                // 타입 캐스팅이 바로 안되어서 리스트를 2개 만든 후 우회
                var createdList = Activator.CreateInstance(listField.FieldType);
                var tempList = new List<IBaseData>();

                // Data Load from DataTable in DataSet
                foreach (DataRow row in table.Rows)
                {
                    IBaseData data = (IBaseData)Activator.CreateInstance(instanceDataType);
                    data.InitializeFromTableData(row);
                    tempList.Add(data);
                }

                var addMethod = listField.FieldType.GetMethod("Add");
                foreach (var item in tempList)
                {
                    addMethod.Invoke(createdList, new[] { item });
                }

                // 리스트를 ScriptableObject에 설정
                listField.SetValue(soInstance, createdList);

                if (AssetDatabase.Contains(soInstance))
                {
                    EditorUtility.SetDirty(soInstance);
                }
                else
                {
                    AssetDatabase.CreateAsset(soInstance, assetPath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}


    


