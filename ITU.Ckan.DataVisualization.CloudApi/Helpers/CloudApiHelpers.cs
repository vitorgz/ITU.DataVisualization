using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
using ITU.Ckan.DataVisualization.InternalDsl.Helpers;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.Helpers
{
    public class CloudApiHelpers
    {
        public static object ConvertArrayToSpecificType(object data, Type dataType)
        {
            var objArr = (data as object[]);
            var arr = Array.CreateInstance(dataType, objArr.Length);

            if (dataType == typeof(int)) //TODO change by 0, not remove values
                objArr = objArr.Where(x => x != null).ToArray();

            arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, dataType));

            return arr;
        }

        public static Type ResolveType(object type)
        {
            switch (type.ToString())
            {
                case "int": return typeof(int);
                case "int2": return typeof(int);
                case "int4": return typeof(int);
                case "int8": return typeof(long);
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
            JObject json = JObject.Parse(response.ToString());

            foreach (var item in fields)
            {
                var jsonValues = from p in json["result"]["records"]
                                 select (string)p[item.id.ToString()];


                if (item.record == null)
                {
                    item.record = new Record();
                }

                item.record.name = item.id.ToString();
                item.record.value = jsonValues.ToArray();
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
            var dataType = CloudApiHelpers.ResolveType(xField.type);
            var data = CloudApiHelpers.ConvertArrayToSpecificType(xField.record.value, dataType); //(xField.record.value as object[]).OfType().ToArray(); ;
            ;

            if (table.column == null) table.column = new Column();
            table.column.Value = data;
            table.column.Type = dataType;
            table.column.name = xField.name;

            //map yAxys
            var rows = new List<Row>();
            var series = fields.Where(y => y.selected && !y.xAxys);
            foreach (var item in series)
            {
                var row = new Row() { name = item.name, Type = CloudApiHelpers.ResolveType(item.type), Value = CloudApiHelpers.ConvertArrayToSpecificType(item.record.value, CloudApiHelpers.ResolveType(item.type)) };
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
            //TODO problems converting lists
            var rec = DslConverterHelpers.ConvertToStringArray(record.value).ToList();           
            // var rec= record.value as List<string>;
            //if(rec ==null)
            //    rec = record.value as List<double>;

            var total = rec.Count();

            var dist = rec.Distinct();

            var data = rec
                .GroupBy(s => s)
                .Select(g => new PieChartDTO { Name = g.Key, Percentage = ((double)g.Count() / (double)total) * 100.0 }).ToList();

            var table = new Table();
            table.column = new Column() {Value = data };

            return table;

        }
    }
}
