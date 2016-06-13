using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
using ITU.Ckan.DataVisualization.InternalDsl.Helpers;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TB.ComponentModel;

namespace ITU.Ckan.DataVisualization.CloudApi.Helpers
{
    public class CloudApiHelpers
    {
        public static object ConvertArrayToSpecificType(object data, Type dataType)
        {
            
            var objArr = (data as string[]);
            var arr = Array.CreateInstance(dataType, objArr.Length);

            if (dataType == typeof(int)) //TODO change by 0, not remove values
                objArr = objArr.Where(x => x != null).ToArray();

            arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, dataType));

            return arr;
        }

        public static object ConvertToType(object sourceValues, Type dataType)
        {

            var src = sourceValues as List<string>;
            var convertedValues = src.ConvertToEnumerable(dataType.GetTypeInfo()).ToList();

            return convertedValues;
        }

        public static Type ResolveType(object type)
        {
            switch (type.ToString().ToLower())
            {
                case "int": return typeof(int);
                case "int2": return typeof(int);
                case "int4": return typeof(int);
                case "int8": return typeof(long);
                case "int16": return typeof(int);
                case "int32": return typeof(int);
                case "timestamp": return typeof(DateTime);
                case "float4": return typeof(float);
                case "time": return typeof(TimeSpan);
                case "float8": return typeof(double);
                case "bool": return typeof(bool);
                case "decimal": return typeof(decimal);
                case "text": return typeof(string);
                case "numeric": return typeof(int);
                default: return typeof(string);
            }
        }

        public static void ProcessJsonResponse(object response, List<Field> fields)
        {
            JObject json = null;
            if (!string.IsNullOrEmpty(response.ToString()))
                json = JObject.Parse(response.ToString());
            else
                return;

            foreach (var item in fields)
            {
                var type =  item.type;
                var jsonValues = from p in json["result"]["records"]
                                 select p[item.id.ToString()].Convert(type as Type);


                if (item.record == null)
                {
                    item.record = new Record();
                }

                item.record.name = item.id.ToString();
                if(jsonValues.Any())
                   item.record.value = jsonValues.ToList();
            }

            //dts.records = json
        }

        public static Table CreateDataTable(VisualDTO visual)
        {
            //select X-Axys
            var table = new Table();

            //map xAxys //TODO move to the fluent
            var fields = visual.sources.SelectMany(x => x.fields);
            var xField = fields.Where(y => y.xAxys).FirstOrDefault();
            var dataType = ResolveType(xField.type);
            
            //TODO REVIEW
            //var data = ConvertArrayToSpecificType(xField.record.value, dataType); //(xField.record.value as object[]).OfType().ToArray(); ;
            
            if (table.column == null) table.column = new Column();
            table.column.Value = (xField.record.value as IEnumerable).ConvertToEnumerable<string>(); //x axys data
            table.column.Type = dataType;
            table.column.name = xField.name;

            //map yAxys
            var rows = new List<Row>();
            var series = fields.Where(y => y.selected && !y.xAxys);
            foreach (var item in series)
            {
                var row = new Row()
                {
                    name = item.id.ToString(),
                    Type = ResolveType(item.type),
                    Value = item.record !=null ? item.record.value : null //ConvertArrayToSpecificType(item.record.value, ResolveType(item.type))
                };
                rows.Add(row);
            }

            //if (table.rows == null) //TODO perhaps Initilize() fluent api
            table.rows = rows;

            return table;
        }

        public static List<T> ConvertToListOfType<T>(ListDeserialize response) where T : new()
        {
            //using reflection
            var list = new List<T>();

            foreach (var item in response.result)
            {
                var t = new T();
                PropertyInfo prop = t.GetType().GetProperty("name", BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(t, item, null);
                }
                list.Add(t);
            }
                       

            return list;
        }

        public static void MergeData(VisualDTO visual)
        {

        }

        public static Table PieChartAnalizeAndCreateTable(Record record)
        {
            //var rec = ConvertToType(record.value, typeof(string)) as List<object>;
            //var stringRec = rec.OfType<string

            var stringRec = ((IList)record.value).Cast<string>().ToList();
            //if (stringRec == null) stringRec = ((List<int>)(record.value)).Cast<string>().ToList();

            var total = stringRec.Count();
            var dist = stringRec.Distinct();

            var analyisData = stringRec
                .GroupBy(s => s)
                .Select(g => new object[] { g.Key, ((double)g.Count() / (double)total) * 100.0 }).ToArray();

            var data = new Table();
            data.rows = new List<Row>() { new Row() { Value = analyisData } };            

            return data;
        }
    }
}
